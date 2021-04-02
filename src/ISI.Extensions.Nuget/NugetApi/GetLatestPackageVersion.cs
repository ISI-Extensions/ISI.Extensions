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
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.GetLatestPackageVersionResponse GetLatestPackageVersion(DTOs.GetLatestPackageVersionRequest request)
		{
			var response = new DTOs.GetLatestPackageVersionResponse();

			string nugetServer = null;
			var nugetServerVersion = 2;
			if (request.PackageNugetServers.ContainsKey(request.PackageId))
			{
				nugetServer = request.PackageNugetServers[request.PackageId];
			}
			else if (request.PackageNugetServers.Where(packageNugetServer => packageNugetServer.Key.EndsWith("*")).Any(packageNugetServer => request.PackageId.StartsWith(packageNugetServer.Key.TrimEnd('*'), StringComparison.InvariantCultureIgnoreCase)))
			{
				nugetServer = request.PackageNugetServers.Where(packageNugetServer => packageNugetServer.Key.EndsWith("*")).FirstOrDefault(packageNugetServer => request.PackageId.StartsWith(packageNugetServer.Key.TrimEnd('*'), StringComparison.InvariantCultureIgnoreCase)).Value;
			}
			else if (request.MainNugetPackageForConsideration.Contains(request.PackageId) || request.MainNugetPackageForConsideration.Where(mainNugetPackage => mainNugetPackage.EndsWith("*")).Any(mainNugetPackage => request.PackageId.StartsWith(mainNugetPackage.TrimEnd('*'), StringComparison.InvariantCultureIgnoreCase)))
			{
				nugetServer = "https://api-v2v3search-0.nuget.org";
				nugetServerVersion = 3;
			}



			if (nugetServerVersion == 3)
			{
				response.PackageVersion = GetLatestPackageVersionV3(request.PackageId, request.PackageNugetServers, request.MainNugetPackageForConsideration, nugetServer);
			}
			else
			{
				response.PackageVersion = GetLatestPackageVersionV2(request.PackageId, request.PackageNugetServers, request.MainNugetPackageForConsideration, nugetServer);
			}

			return response;
		}

		private string GetLatestPackageVersionV2(
			string package,
			IDictionary<string, string> packageNugetServers,
			HashSet<string> mainNugetPackageForConsideration,
			string nugetServer)
		{
			string packageVersion = null;

			if (!string.IsNullOrEmpty(nugetServer))
			{
				var url = string.Format("{0}/api/v2/FindPackagesById()?id='{1}'", nugetServer, package);

				while (!string.IsNullOrEmpty(url))
				{
					var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
					request.Method = System.Net.WebRequestMethods.Http.Get;

					try
					{
						using (var response = (System.Net.HttpWebResponse)request.GetResponse())
						{
							using (var responseReader = new System.IO.StreamReader(response.GetResponseStream()))
							{
								url = null;

								var key = string.Format("Packages(Id='{0}',Version='", package);

								while (!responseReader.EndOfStream)
								{
									var packageInfo = (responseReader.ReadLine() ?? string.Empty).Trim();

									if (packageInfo.StartsWith("<link rel=\"next\""))
									{
										url = packageInfo.Split(new[] { '\"' }).FirstOrDefault(u => u.StartsWith("http"));

										if (!string.IsNullOrEmpty(url))
										{
											url = System.Web.HttpUtility.HtmlDecode(url);
										}
									}

									var index = packageInfo.IndexOf(key, StringComparison.CurrentCulture);

									while (index >= 0)
									{
										var lineVersion = packageInfo.Substring(index + key.Length).Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

										if (lineVersion.All(ch => ".0123456789".Contains(ch)) && (string.IsNullOrEmpty(packageVersion) || (new System.Version(lineVersion) > new Version(packageVersion))))
										{
											if (string.IsNullOrEmpty(packageVersion) || ((new Version(lineVersion)) > (new Version(packageVersion))))
											{
												packageVersion = lineVersion;
											}
										}

										index = packageInfo.IndexOf(key, index + 1, StringComparison.CurrentCultureIgnoreCase);
									}
								}
							}
						}
					}
#pragma warning disable CS0168 // Variable is declared but never used
					catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
					{
						Console.WriteLine(url);
						throw;
					}
				}
			}

			return packageVersion;
		}



		[DataContract]
		public class NugetFeed
		{
			[DataMember(Name = "data", EmitDefaultValue = false)]
			public NugetFeedPackage[] Packages { get; set; }
		}

		[DataContract]
		public class NugetFeedPackage
		{
			[DataMember(Name = "@id", EmitDefaultValue = false)]
			public string Url { get; set; }

			[DataMember(Name = "@type", EmitDefaultValue = false)]
			public string Type { get; set; }

			[DataMember(Name = "registration", EmitDefaultValue = false)]
			public string Registration { get; set; }

			[DataMember(Name = "id", EmitDefaultValue = false)]
			public string Id { get; set; }

			[DataMember(Name = "version", EmitDefaultValue = false)]
			public string Version { get; set; }

			[DataMember(Name = "description", EmitDefaultValue = false)]
			public string Description { get; set; }

			[DataMember(Name = "summary", EmitDefaultValue = false)]
			public string Summary { get; set; }

			[DataMember(Name = "title", EmitDefaultValue = false)]
			public string Title { get; set; }

			[DataMember(Name = "licenseUrl", EmitDefaultValue = false)]
			public string LicenseUrl { get; set; }

			[DataMember(Name = "projectUrl", EmitDefaultValue = false)]
			public string ProjectUrl { get; set; }

			[DataMember(Name = "tags", EmitDefaultValue = false)]
			public string[] Tags { get; set; }

			[DataMember(Name = "authors", EmitDefaultValue = false)]
			public string[] Authors { get; set; }

			[DataMember(Name = "totalDownloads", EmitDefaultValue = false)]
			public int TotalDownloads { get; set; }

			[DataMember(Name = "verified", EmitDefaultValue = false)]
			public bool Verified { get; set; }

			[DataMember(Name = "versions", EmitDefaultValue = false)]
			public NugetFeedPackageVersion[] PackageVersions { get; set; }
		}

		[DataContract]
		public class NugetFeedPackageVersion
		{
			[DataMember(Name = "version", EmitDefaultValue = false)]
			public string Version { get; set; }

			[DataMember(Name = "downloads", EmitDefaultValue = false)]
			public int Downloads { get; set; }

			[DataMember(Name = "@id", EmitDefaultValue = false)]
			public string Url { get; set; }
		}



		private string GetLatestPackageVersionV3(
			string package,
			IDictionary<string, string> packageNugetServers,
			HashSet<string> mainNugetPackageForConsideration,
			string nugetServer)
		{
			string packageVersion = null;

			if (!string.IsNullOrEmpty(nugetServer))
			{
				var url = string.Format("{0}/query?q={1}&prerelease=false", nugetServer, package);

				var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
				request.Method = System.Net.WebRequestMethods.Http.Get;

				try
				{
					NugetFeed nugetFeed = null;

					using (var response = (System.Net.HttpWebResponse)request.GetResponse())
					{
						using (var memoryStream = new System.IO.MemoryStream())
						{
							response.GetResponseStream().CopyTo(memoryStream);

							memoryStream.Position = 0;

							using (var streamReader = new System.IO.StreamReader(memoryStream))
							{
								var json = streamReader.ReadToEnd();

								memoryStream.Position = 0;

								var knownTypes = new List<Type>();

								var dataContractSerializer = new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer();
								//var dataContractSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(type);
								nugetFeed = dataContractSerializer.Deserialize(typeof(NugetFeed), memoryStream) as NugetFeed;
							}
						}
					}

					var packageInformation = nugetFeed?.Packages.NullCheckedFirstOrDefault(x => string.Equals(x.Id, package, StringComparison.InvariantCultureIgnoreCase));

					if (packageInformation != null)
					{
						packageVersion = packageInformation?.PackageVersions?.ToNullCheckedArray(NullCheckCollectionResult.Empty).OrderBy(x => new Version(x.Version)).Last().Version;
					}
				}
#pragma warning disable CS0168 // Variable is declared but never used
				catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
				{
					Console.WriteLine(url);
					throw;
				}
			}

			return packageVersion;
		}
	}
}