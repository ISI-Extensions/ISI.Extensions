#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.GetNugetPackageKeyResponse GetNugetPackageKey(DTOs.GetNugetPackageKeyRequest request)
		{
			var response = new DTOs.GetNugetPackageKeyResponse();

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				var arguments = new List<string>();

				arguments.Add("install");
				arguments.Add(request.PackageId);
				arguments.Add("-DependencyVersion ignore");
				if (!string.IsNullOrWhiteSpace(request.PackageVersion))
				{
					arguments.Add(string.Format("-Version {0}", request.PackageVersion));
				}
				if (!string.IsNullOrWhiteSpace(request.Source))
				{
					arguments.Add(string.Format("-Source \"{0}\"", request.Source));
				}
				if (request.NugetConfigFullNames.NullCheckedAny())
				{
					arguments.AddRange(GetConfigFileArguments(request.NugetConfigFullNames));
				}

				var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = new NullLogger(),
					ProcessExeFullName = GetNugetExeFullName(new DTOs.GetNugetExeFullNameRequest()).NugetExeFullName,
					Arguments = arguments.ToArray(),
					WorkingDirectory = tempDirectory.FullName,
				});

				if (!nugetResponse.Errored)
				{
					var packageFullName = System.IO.Directory.GetDirectories(tempDirectory.FullName).First();

					response.NugetPackageKey = new NugetPackageKey()
					{
						Package = request.PackageId,
						Version = System.IO.Path.GetFileName(packageFullName).Substring(request.PackageId.Length + 1),
					};

					var nupkgFullName = System.IO.Directory.GetFiles(packageFullName, "*.nupkg").NullCheckedFirstOrDefault();
					if (!string.IsNullOrWhiteSpace(nupkgFullName))
					{
						var nugetPackageDependencies = new HashSet<NugetPackageDependency>();

						var nuspecFullName = System.IO.Path.Combine(packageFullName, string.Format("{0}.nuspec", System.IO.Path.GetFileNameWithoutExtension(nupkgFullName)));

						using (var zipSteam = System.IO.File.OpenRead(nupkgFullName))
						{
							using (var zipArchive = new System.IO.Compression.ZipArchive(zipSteam, System.IO.Compression.ZipArchiveMode.Read))
							{
								var archiveEntry = zipArchive.Entries.FirstOrDefault(file => file.Name.EndsWith(".nuspec", StringComparison.InvariantCultureIgnoreCase));

								archiveEntry?.ExtractToFile(nuspecFullName);
							}
						}

						var nuspecXml = System.Xml.Linq.XElement.Parse(System.IO.File.ReadAllText(nuspecFullName));

						foreach (var metadata in nuspecXml.GetElementsByLocalName("metadata"))
						{
							foreach (var dependencies in metadata.GetElementsByLocalName("dependencies"))
							{
								foreach (var dependencyGroup in dependencies.GetElementsByLocalName("group"))
								{
									foreach (var dependency in dependencyGroup.GetElementsByLocalName("dependency"))
									{
										nugetPackageDependencies.Add(new NugetPackageDependency()
										{
											Package = dependency.GetAttributeByLocalName("id")?.Value ?? string.Empty,
											Version = dependency.GetAttributeByLocalName("version")?.Value ?? string.Empty,
										});
									}
								}
								foreach (var dependency in dependencies.GetElementsByLocalName("dependency"))
								{
									nugetPackageDependencies.Add(new NugetPackageDependency()
									{
										Package = dependency.GetAttributeByLocalName("id")?.Value ?? string.Empty,
										Version = dependency.GetAttributeByLocalName("version")?.Value ?? string.Empty,
									});
								}
							}
						}

						response.NugetPackageKey.Dependencies = nugetPackageDependencies.ToArray();
					}


					var assemblyFullNames = System.IO.Directory.GetFiles(packageFullName, "*.dll", System.IO.SearchOption.AllDirectories)
						.OrderBy(assemblyFullName => assemblyFullName, StringComparer.InvariantCultureIgnoreCase)
						.Select(assemblyFullName => assemblyFullName.Substring(packageFullName.Length).Trim('\\'))
						.Where(assemblyFileName => assemblyFileName.StartsWith("lib\\", StringComparison.InvariantCultureIgnoreCase));

					var nugetPackageKeyTargetFrameworks = new List<NugetPackageKeyTargetFramework>();

					foreach (var assemblyGroup in assemblyFullNames.GroupBy(System.IO.Path.GetDirectoryName, StringComparer.InvariantCultureIgnoreCase))
					{
						var pathPieces = assemblyGroup.Key.Split(new[] { '\\', '/' });

						var nugetPackageKeyTargetFrameworkAssemblies = new List<NugetPackageKeyTargetFrameworkAssembly>();

						var nugetPackageKeyTargetFramework = new NugetPackageKeyTargetFramework()
						{
							TargetFramework = (pathPieces.Length > 1 ? NuGet.Frameworks.NuGetFramework.Parse(pathPieces[1]) : null),
						};

						foreach (var assemblyFileName in assemblyGroup)
						{
							try
							{
								var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(System.IO.Path.Combine(packageFullName, assemblyFileName));

								var nugetPackageKeyTargetFrameworkAssembly = new NugetPackageKeyTargetFrameworkAssembly()
								{
									AssemblyName = assemblyName.FullName.Split(new[] { ',' }).First().Trim(),
									AssemblyFileName = System.IO.Path.GetFileName(assemblyFileName),
									HintPath = string.Format("{0}\\{1}", System.IO.Path.GetFileName(packageFullName), assemblyFileName.Replace("/", "\\")),
									AssemblyVersion = assemblyName.Version.ToString(),
									PublicKeyToken = string.Concat(assemblyName.GetPublicKeyToken().Select(b => b.ToString("X2"))).ToLower(),
								};

								nugetPackageKeyTargetFrameworkAssemblies.Add(nugetPackageKeyTargetFrameworkAssembly);

								nugetPackageKeyTargetFramework.Assemblies = nugetPackageKeyTargetFrameworkAssemblies.ToArray();

								nugetPackageKeyTargetFrameworks.Add(nugetPackageKeyTargetFramework);
							}
							catch (Exception exception)
							{
								Console.WriteLine(exception);
							}
						}
					}

					response.NugetPackageKey.TargetFrameworks = nugetPackageKeyTargetFrameworks.ToArray();
				}
			}

			return response;
		}
	}
}