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
using System.IO.Compression;
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.GetNugetPackageKeyResponse GetNugetPackageKey(DTOs.GetNugetPackageKeyRequest request)
		{
			var response = new DTOs.GetNugetPackageKeyResponse();

			var usedCachedNugetPackageKey = false;

			var nugetPackageKeyCacheDirectory = GetNugetPackageKeyCacheDirectory();

			string getCachedNugetPackageKeyDirectory(string package)
			{
				return System.IO.Path.Combine(nugetPackageKeyCacheDirectory, package);
			}

			string getCachedNugetPackageKeyFullName(string package, string version)
			{
				return System.IO.Path.Combine(getCachedNugetPackageKeyDirectory(package), $"{package}_{version}.json");
			}

			if (string.IsNullOrWhiteSpace(request.Version))
			{
				var foundNugetPackageKey = ListNugetPackageKeys(new()
				                           {
					                           Search = request.Package,
					                           ExactMatchOnly = true,
					                           Source = request.Source,
					                           NugetConfigFullNames = request.NugetConfigFullNames,
				                           }).NugetPackageKeys.NullCheckedFirstOrDefault() ??
				                           SearchNugetPackageKeys(new()
				                           {
					                           Search = request.Package,
					                           ExactMatchOnly = true,
					                           Source = request.Source,
					                           NugetConfigFullNames = request.NugetConfigFullNames,
				                           }).NugetPackageKeys.NullCheckedFirstOrDefault();

				if (foundNugetPackageKey != null)
				{
					response.NugetPackageKey = foundNugetPackageKey;

					return response;
				}
			}

			if (!string.IsNullOrWhiteSpace(request.Version))
			{
				var cachedNugetPackageKeyFullName = getCachedNugetPackageKeyFullName(request.Package, request.Version);

				if (System.IO.File.Exists(cachedNugetPackageKeyFullName))
				{
					using (var stream = System.IO.File.OpenRead(cachedNugetPackageKeyFullName))
					{
						response.NugetPackageKey = JsonSerializer.Deserialize<SerializableDTOs.INugetPackageKey>(stream)?.Export();
						usedCachedNugetPackageKey = true;
					}
				}
			}

			if (response.NugetPackageKey == null)
			{
				using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
				{
					var arguments = new List<string>();

					arguments.Add("install");
					arguments.Add(request.Package);
					arguments.Add("-DependencyVersion ignore");
					if (!string.IsNullOrWhiteSpace(request.Version))
					{
						arguments.Add($"-Version {request.Version}");
					}
					if (!string.IsNullOrWhiteSpace(request.Source))
					{
						arguments.Add($"-Source \"{request.Source}\"");
					}
					if (request.NugetConfigFullNames.NullCheckedAny())
					{
						arguments.AddRange(GetSourcesFromConfigFileArguments(request.NugetConfigFullNames));
					}

					var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = new NullLogger(),
						ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
						Arguments = arguments.ToArray(),
						WorkingDirectory = tempDirectory.FullName,
					});

					if (!nugetResponse.Errored)
					{
						var packageFullName = System.IO.Directory.GetDirectories(tempDirectory.FullName).First();

						var package = request.Package;
						var version = System.IO.Path.GetFileName(packageFullName).Substring(request.Package.Length + 1);

						var cachedNugetPackageKeyFullName = getCachedNugetPackageKeyFullName(package, version);

						if (System.IO.File.Exists(cachedNugetPackageKeyFullName))
						{
							using (var stream = System.IO.File.OpenRead(cachedNugetPackageKeyFullName))
							{
								response.NugetPackageKey = JsonSerializer.Deserialize<SerializableDTOs.INugetPackageKey>(stream)?.Export();
								usedCachedNugetPackageKey = true;
							}
						}

						if (response.NugetPackageKey == null)
						{
							response.NugetPackageKey = new()
							{
								Package = package,
								Version = version,
							};

							var nupkgFullName = System.IO.Directory.GetFiles(packageFullName, "*.nupkg").NullCheckedFirstOrDefault();
							if (!string.IsNullOrWhiteSpace(nupkgFullName))
							{
								var nugetPackageDependencies = new HashSet<NugetPackageDependency>();

								var nuspecFullName = System.IO.Path.Combine(packageFullName, $"{System.IO.Path.GetFileNameWithoutExtension(nupkgFullName)}.nuspec");

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
												nugetPackageDependencies.Add(new()
												{
													Package = dependency.GetAttributeByLocalName("id")?.Value ?? string.Empty,
													Version = dependency.GetAttributeByLocalName("version")?.Value ?? string.Empty,
												});
											}
										}

										foreach (var dependency in dependencies.GetElementsByLocalName("dependency"))
										{
											nugetPackageDependencies.Add(new()
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

								foreach (var assemblyFileName in assemblyGroup.Where(assemblyFileName => !assemblyFileName.EndsWith("msdia140.dll", StringComparison.InvariantCultureIgnoreCase)))
								{
									try
									{
										var assemblyName = System.Reflection.AssemblyName.GetAssemblyName(System.IO.Path.Combine(packageFullName, assemblyFileName));

										var nugetPackageKeyTargetFrameworkAssembly = new NugetPackageKeyTargetFrameworkAssembly()
										{
											AssemblyName = assemblyName.FullName.Split(new[] { ',' }).First().Trim(),
											AssemblyFileName = System.IO.Path.GetFileName(assemblyFileName),
											HintPath = $"{System.IO.Path.GetFileName(packageFullName)}\\{assemblyFileName.Replace("/", "\\")}",
											AssemblyVersion = assemblyName.Version.ToString(),
											PublicKeyToken = string.Concat(assemblyName.GetPublicKeyToken().Select(b => b.ToString("X2"))).ToLower(),
										};

										nugetPackageKeyTargetFrameworkAssemblies.Add(nugetPackageKeyTargetFrameworkAssembly);

										nugetPackageKeyTargetFramework.Assemblies = nugetPackageKeyTargetFrameworkAssemblies.ToArray();

										nugetPackageKeyTargetFrameworks.Add(nugetPackageKeyTargetFramework);
									}
									catch (Exception exception)
									{
										Console.WriteLine(System.IO.Path.Combine(packageFullName, assemblyFileName));
										Console.WriteLine(exception);
									}
								}
							}

							response.NugetPackageKey.TargetFrameworks = nugetPackageKeyTargetFrameworks.ToArray();
						}
					}
				}

				if ((response.NugetPackageKey != null) && !usedCachedNugetPackageKey && !string.IsNullOrWhiteSpace(nugetPackageKeyCacheDirectory))
				{
					System.IO.Directory.CreateDirectory(getCachedNugetPackageKeyDirectory(response.NugetPackageKey.Package));

					var cachedNugetPackageKeyFullName = getCachedNugetPackageKeyFullName(response.NugetPackageKey.Package, response.NugetPackageKey.Version);

					using (var stream = System.IO.File.OpenWrite(cachedNugetPackageKeyFullName))
					{
						JsonSerializer.Serialize(SerializableDTOs.NugetPackageKeyV1.ToSerializable(response.NugetPackageKey), stream, true);
					}
				}
			}

			return response;
		}
	}
}