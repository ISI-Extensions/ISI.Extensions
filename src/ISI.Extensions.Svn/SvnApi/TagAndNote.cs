#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.TagAndNoteResponse TagAndNote(DTOs.TagAndNoteRequest request)
		{
			var rgPropertyLineParser = new System.Text.RegularExpressions.Regex("(?:[^\"\\s]+)\\s*|\"(?:[\"]+)\"\\s*");
			var rgIsSVNUrl = new System.Text.RegularExpressions.Regex("^((?:http://)|(?:https://)|(?:svn://)|(?:file:///)|(?:svn\\+ssh://)|(?:svn\\+...://))");

			var response = new DTOs.TagAndNoteResponse();

			if (SvnIsInstalled)
			{
				var infos = GetWorkingCopyInfos(new()
				{
					UserName = request.UserName,
					Password = request.Password,
					Source = request.WorkingCopyDirectory,
					Depth = Depth.Infinity,
					IncludeExternals = true,
				}).Infos.ToNullCheckedArray(NullCheckCollectionResult.Empty);

				var workingCopyDirectory = request.WorkingCopyDirectory.TrimEnd('\\', '/');
				if (string.IsNullOrWhiteSpace(workingCopyDirectory))
				{
					workingCopyDirectory = ".";
				}

				var trunkInfo = infos.FirstOrDefault(info => string.Equals(info.Path.TrimEnd('\\', '/'), workingCopyDirectory, StringComparison.CurrentCultureIgnoreCase));
				if (trunkInfo != null)
				{
					var trunkUrl = GetTrunkUrl(trunkInfo.Url);

					if (!string.IsNullOrEmpty(trunkUrl))
					{
						Logger.LogInformation($"  trunkUrl=\"{trunkUrl}\"");

						var tagsUrl = GetTagsUrl(trunkUrl, request.TagName, request.DateTimeStamp, request.DateTimeMask);

						if (!string.IsNullOrEmpty(tagsUrl))
						{
							Logger.LogInformation($"  tagsUrl=\"{tagsUrl}\"");

							Logger.LogInformation("  trunk svn tag start");

							RemoteCopy(new()
							{
								UserName = request.UserName,
								Password = request.Password,
								SourceUrl = trunkUrl,
								TargetUrl = tagsUrl,
								LogMessage = $"Version: {request.TagName}\nDateTimeStamp: {request.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise)}",
								CreateParents = true,
							});

							Logger.LogInformation("  trunk svn tag done");

							Logger.LogInformation($"  TagsUrl=\"{tagsUrl}\"");
							Logger.LogInformation($"  Version=\"{request.TagName}\"");
							Logger.LogInformation($"  DateTimeStamp=\"{request.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise)}\"");

							var propertySets = GetProperties(new()
							{
								UserName = request.UserName,
								Password = request.Password,
								Source = request.WorkingCopyDirectory,
								Depth = Depth.Infinity,
							}).Properties.ToNullCheckedArray(NullCheckCollectionResult.Empty);

							var setPropertyRequests = new List<DTOs.SetRemotePropertyRequest>();

							request.TryGetExternalTagName ??= (string path, out string tagName) =>
							{
								tagName = string.Empty;
								return false;
							};

							foreach (var propertySet in propertySets)
							{
								foreach (var property in propertySet.Properties.Where(p => string.Equals(p.Key, PropertyName.Externals, StringComparison.InvariantCultureIgnoreCase)))
								{
									var info = infos.FirstOrDefault(info => string.Equals(info.Path.TrimEnd('\\', '/'), propertySet.Path.TrimEnd('\\', '/'), StringComparison.CurrentCultureIgnoreCase));

									var url = info.Url.Replace(trunkUrl, tagsUrl);

									var existingExternals = property.Value.Split(["\n", "\r"], StringSplitOptions.RemoveEmptyEntries);

									Logger.LogInformation($"  TagUrl=\"{url}\"");
									Logger.LogInformation($"  Path=\"{propertySet.Path}\"");
									Logger.LogInformation($"  Was:\n{string.Join("\r\n", existingExternals)}");

									for (var existingExternalIndex = 0; existingExternalIndex < existingExternals.Length; existingExternalIndex++)
									{
										var existingExternal = existingExternals[existingExternalIndex];

										var parts = (from object match in rgPropertyLineParser.Matches(existingExternal) select match.ToString().Trim());

										string uri = null;
										string directory = null;
										long? revision = null;

										foreach (var part in parts)
										{
											if (rgIsSVNUrl.Match(part).Success || part.StartsWith("^", StringComparison.InvariantCultureIgnoreCase))
											{
												var pieces = part.Split(["@"], StringSplitOptions.RemoveEmptyEntries);

												uri = pieces[0].Trim();

												if (pieces.Length > 1)
												{
													revision = pieces[1].Trim().ToLongNullable();
												}
											}
											else if (part.StartsWith("-r", StringComparison.InvariantCultureIgnoreCase))
											{
												revision = part.Replace("-r", string.Empty).Trim().ToLongNullable();
											}
											else
											{
												directory = part.Trim();
											}
										}

										revision ??= infos.FirstOrDefault(info => string.Equals(info.Path.TrimEnd('\\', '/'), System.IO.Path.Combine(propertySet.Path, directory).TrimEnd('\\', '/'), StringComparison.CurrentCultureIgnoreCase))?.Revision;

										Logger.LogInformation($"      testing=\"{uri.ToLower()}\"");

										if (request.TryGetExternalTagName(uri, out var externalTagName))
										{
											var externalTrunkUri = new Uri(GetRemoteUrl(System.IO.Path.Combine(propertySet.Path, directory)));
											var externalTrunkUrl = GetTrunkUrl(externalTrunkUri);
											var externalTagsUrl = GetTagsUrl(externalTrunkUri, externalTagName, request.DateTimeStamp, request.DateTimeMask);

											var trunkUri = new Uri(GetRemoteUrl(propertySet.Path));

											Logger.LogInformation("  external svn tag start");

											RemoteCopy(new()
											{
												UserName = request.UserName,
												Password = request.Password,
												SourceUrl = externalTrunkUrl,
												TargetUrl = externalTagsUrl,
												LogMessage = $"Version: {externalTagName}\nDateTimeStamp: {request.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise)}",
												CreateParents = true,
											});

											Logger.LogInformation("  external svn tag done");

											if (externalTrunkUri.ToString().Length > externalTrunkUrl.Length)
											{
												externalTagsUrl = $"{externalTagsUrl}{externalTrunkUri.ToString().Substring(externalTrunkUrl.Length)}";
											}

											externalTrunkUri = new(externalTagsUrl);

											#region Make Relative

											if (string.Equals(trunkUri.Scheme, externalTrunkUri.Scheme, StringComparison.InvariantCultureIgnoreCase))
											{
												if (string.Equals(trunkUri.Host, externalTrunkUri.Host, StringComparison.InvariantCultureIgnoreCase))
												{
													var trunkUrlPathParts = trunkUri.AbsolutePath.Split(["\\", "/"], StringSplitOptions.RemoveEmptyEntries).ToList();
													var externalTrunkUrlPathParts = externalTrunkUri.AbsolutePath.Split(["\\", "/"], StringSplitOptions.RemoveEmptyEntries).ToList();

													Logger.LogInformation($"      externalTrunkUrl=\"{externalTrunkUrl}\"");
													Logger.LogInformation($"      externalTagsUrl=\"{externalTagsUrl}\"");

													if (string.Equals(trunkUri.Scheme, "file", StringComparison.InvariantCultureIgnoreCase))
													{
														var trunkRepositoryRoot = System.IO.Path.GetDirectoryName(GetRepositoryRoot(externalTrunkUrl));
														Logger.LogInformation($"      trunkRepositoryRoot=\"{trunkRepositoryRoot}\"");
														var externalRepositoryRoot = System.IO.Path.GetDirectoryName(GetRepositoryRoot(externalTagsUrl));
														Logger.LogInformation($"      externalRepositoryRoot=\"{externalRepositoryRoot}\"");

														var trunkRepositoryHost = System.IO.Path.GetDirectoryName(trunkRepositoryRoot);
														Logger.LogInformation($"      trunkRepositoryHost=\"{trunkRepositoryHost}\"");
														var externalRepositoryHost = System.IO.Path.GetDirectoryName(externalRepositoryRoot);
														Logger.LogInformation($"      externalRepositoryHost=\"{externalRepositoryHost}\"");

														if (string.Equals(trunkRepositoryHost, externalRepositoryHost, StringComparison.InvariantCultureIgnoreCase))
														{
															var trunkRepositoryHostParts = trunkRepositoryHost.Split(["\\", "/"], StringSplitOptions.RemoveEmptyEntries).ToList();

															trunkUrlPathParts.RemoveRange(0, trunkRepositoryHostParts.Count() - 1);
															externalTrunkUrlPathParts.RemoveRange(0, trunkRepositoryHostParts.Count() - 1);
														}
													}

													if (trunkUrlPathParts.Any() && externalTrunkUrlPathParts.Any() && string.Equals(trunkUrlPathParts[0], externalTrunkUrlPathParts[0], StringComparison.InvariantCultureIgnoreCase))
													{
														externalTrunkUrlPathParts[0] = "^";
													}
													else
													{
														externalTrunkUrlPathParts[0] = "^/../" + externalTrunkUrlPathParts[0];
													}

													externalTagsUrl = string.Join("/", externalTrunkUrlPathParts.ToArray());
												}
											}

											#endregion

											existingExternals[existingExternalIndex] = $"{externalTagsUrl} {directory}";
										}
										else
										{
											existingExternals[existingExternalIndex] = $"{uri}@{revision} {directory}";
										}
									}

									var externals = string.Join("\r\n", existingExternals);

									Logger.LogInformation($"  Will Be:\n{externals}");

									setPropertyRequests.Add(new()
									{
										UserName = request.UserName,
										Password = request.Password,
										Uri = new(url),
										Key = PropertyName.Externals,
										Value = externals,
										LogMessage = "setting externals' revision",
									});
								}
							}

							foreach (var setRemotePropertyRequest in setPropertyRequests)
							{
								SetRemoteProperty(setRemotePropertyRequest);
							}
						}
						else
						{
							Logger.LogInformation("Missing tagsUrl");
						}
					}
					else
					{
						Logger.LogInformation("Missing trunkUrl");
					}
				}
				else
				{
					Logger.LogInformation("Not under source Control");
				}
			}

			return response;
		}
	}
}