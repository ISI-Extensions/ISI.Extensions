#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.UpdateNugetPackagesResponse UpdateNugetPackages(DTOs.UpdateNugetPackagesRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.UpdateNugetPackagesResponse();

			var ignorePackageIds = new HashSet<string>(request.IgnorePackageIds ?? new string[0], StringComparer.InvariantCultureIgnoreCase);

			var nugetPackageKeys = request.NugetPackageKeys ?? new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			using (NugetApi.GetNugetLock(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetLockRequest()
			{
				AddToLog = request.AddToLog,
			}).Lock)
			{
				var solutionFullNames = request.SolutionFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);
				for (var solutionIndex = 0; solutionIndex < solutionFullNames.Length; solutionIndex++)
				{
					var solutionFullName = solutionFullNames[solutionIndex];
					if (System.IO.Directory.Exists(solutionFullName))
					{
						var possibleSolutionFullNames = System.IO.Directory.GetFiles(solutionFullName, "*.sln", System.IO.SearchOption.AllDirectories);

						if (possibleSolutionFullNames.Length == 1)
						{
							solutionFullName = possibleSolutionFullNames.First();
						}
						else if (possibleSolutionFullNames.Length > 1)
						{
							var possibleSolutionName = System.IO.Path.GetFileName(solutionFullName);

							var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

							if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
							{
								solutionFullName = possibleSolutionFullName;
							}
							else
							{
								throw new Exception(string.Format("Cannot determine which solution to update \"{0}\"", solutionFullNames[solutionIndex]));
							}
						}
						else
						{
							throw new Exception(string.Format("Cannot find a solution to update \"{0}\"", solutionFullNames[solutionIndex]));
						}
					}

					var solutionSourceDirectory = System.IO.Path.GetDirectoryName(solutionFullName);
					var solutionDirectory = SourceControlClientApi.GetRootDirectory(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.GetRootDirectoryRequest()
					{
						FullName = solutionSourceDirectory,
					}).FullName;

					using (GetSolutionLock(new DTOs.GetSolutionLockRequest()
					{
						SolutionFullName = solutionDirectory,
						AddToLog = request.AddToLog,
					}).Lock)
					{
						logger.LogInformation(string.Format("Updating {0}", System.IO.Path.GetFileNameWithoutExtension(solutionFullName)));

						var isDirty = false;
						var success = true;

						if (success && request.UpdateWorkingCopyFromSourceControl)
						{
							success = SourceControlClientApi.UpdateWorkingCopy(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.UpdateWorkingCopyRequest()
							{
								FullName = solutionDirectory,
								IncludeExternals = true,
								AddToLog = request.AddToLog,
							}).Success;

							if (!success)
							{
								logger.LogError(string.Format("Error updating \"{0}\"", solutionDirectory));
							}
						}

						var nugetConfigFullNames = NugetApi.GetNugetConfigFullNames(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetConfigFullNamesRequest()
						{
							WorkingCopyDirectory = solutionSourceDirectory,
						}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);


						bool tryGetNugetPackageKey(string id, out ISI.Extensions.Nuget.NugetPackageKey key)
						{
							if (nugetPackageKeys.TryGetValue(id, out key))
							{
								return true;
							}

							if (ignorePackageIds.Contains(id))
							{
								return false;
							}

							var getLatestPackageVersionResponse = NugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
							{
								PackageId = id,
								NugetConfigFullNames = nugetConfigFullNames,
							});

							if (getLatestPackageVersionResponse.NugetPackageKey != null)
							{
								nugetPackageKeys.TryAdd(getLatestPackageVersionResponse.NugetPackageKey);
							}

							return nugetPackageKeys.TryGetValue(id, out key);
						}

						var csProjFullNames = new List<string>();

						{
							var solutionLines = System.IO.File.ReadAllLines(solutionFullName);

							foreach (var solutionLine in solutionLines)
							{
								if (solutionLine.Trim().StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
								{
									var pieces = solutionLine.Split(new[] { '=' }).ToList();

									pieces = pieces[1].Split(new[] { '"' }).Select(piece => piece.Trim()).ToList();

									pieces.RemoveAll(piece => string.Equals(piece, ","));
									pieces.RemoveAll(string.IsNullOrWhiteSpace);

									if (pieces[1].EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
									{
										csProjFullNames.Add(System.IO.Path.Combine(solutionSourceDirectory, pieces[1]));
									}
								}
							}
						}

						logger.LogInformation("Updating Projects");

						foreach (var csProjFullName in csProjFullNames.OrderBy(f => f, StringComparer.InvariantCultureIgnoreCase))
						{
							logger.LogInformation(string.Format("  {0}", System.IO.Path.GetFileNameWithoutExtension(csProjFullName)));

							var projectDirectory = System.IO.Path.GetDirectoryName(csProjFullName);

							var packagesConfigFullName = System.IO.Path.Combine(projectDirectory, "packages.config");

							if (System.IO.File.Exists(packagesConfigFullName))
							{
								var packagesConfig = System.IO.File.ReadAllText(packagesConfigFullName);

								try
								{
									var newPackagesConfig = NugetApi.UpdateNugetPackageVersionsInPackagesConfig(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInPackagesConfigRequest()
									{
										PackagesConfigXml = packagesConfig,
										TryGetNugetPackageKey = tryGetNugetPackageKey,
									}).PackagesConfigXml;

									if (HasChanges(packagesConfig, newPackagesConfig))
									{
										System.IO.File.WriteAllText(packagesConfigFullName, newPackagesConfig);
										isDirty = true;
									}
								}
								catch (Exception exception)
								{
									throw new Exception(string.Format("File: {0}", packagesConfigFullName), exception);
								}
							}

							var csProj = System.IO.File.ReadAllText(csProjFullName);

							try
							{
								var newCsProj = NugetApi.UpdateNugetPackageVersionsInCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInCsProjRequest()
								{
									CsProjXml = csProj,
									TryGetNugetPackageKey = tryGetNugetPackageKey,
									ConvertToPackageReferences = false,
								}).CsProjXml;

								if (HasChanges(csProj, newCsProj))
								{
									System.IO.File.WriteAllText(csProjFullName, newCsProj);
									isDirty = true;
								}
							}
							catch (Exception exception)
							{
								throw new Exception(string.Format("File: {0}", csProjFullName), exception);
							}

							var appConfigFileName = System.IO.Path.Combine(projectDirectory, "web.config");
							if (!System.IO.File.Exists(appConfigFileName))
							{
								appConfigFileName = System.IO.Path.Combine(projectDirectory, "app.config");

								if (!System.IO.File.Exists(appConfigFileName))
								{
									appConfigFileName = null;
								}
							}

							if (System.IO.File.Exists(appConfigFileName))
							{
								var appConfigXml = System.IO.File.ReadAllText(appConfigFileName);

								try
								{
									var newAppConfigXml = NugetApi.UpdateAssemblyRedirects(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateAssemblyRedirectsRequest()
									{
										CsProjXml = csProj,
										AppConfigXml = appConfigXml,
										NugetPackageKeys = nugetPackageKeys,
									}).AppConfigXml;

									if (HasChanges(appConfigXml, newAppConfigXml))
									{
										System.IO.File.WriteAllText(appConfigFileName, newAppConfigXml);
										isDirty = true;
									}
								}
								catch (Exception exception)
								{
									throw new Exception(string.Format("File: {0}", appConfigFileName), exception);
								}
							}
						}

						if (success && isDirty && request.CommitWorkingCopyToSourceControl)
						{
							try
							{
								var resharperCacheFullName = System.IO.Path.Combine(solutionSourceDirectory, "_ReSharper.Caches");
								if (System.IO.Directory.Exists(resharperCacheFullName))
								{
									System.IO.Directory.Delete(resharperCacheFullName, true);
								}
							}
							catch (Exception exception)
							{
								logger.LogError(string.Format("Error deleting Resharper Cache \"{0}\"", solutionDirectory));
							}

#if !DEBUG
							success = SourceControlClientApi.CommitWorkingCopy(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.CommitWorkingCopyRequest()
							{
								FullName = solutionDirectory,
								LogMessage = "update nuget packages",
							}).Success;
#endif

							if (!success)
							{
								logger.LogError(string.Format("Error commiting \"{0}\"", solutionDirectory));
							}
						}
					}
				}
			}

			return response;
		}
	}
}