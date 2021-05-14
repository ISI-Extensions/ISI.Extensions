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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionNugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionNugetApi
	{
		public DTOs.UpdateNugetPackagesResponse UpdateNugetPackages(DTOs.UpdateNugetPackagesRequest request)
		{
			var response = new DTOs.UpdateNugetPackagesResponse();

			var ignorePackageIds = new HashSet<string>(request.IgnorePackageIds ?? new string[0], StringComparer.InvariantCultureIgnoreCase);

			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			foreach (var solutionFullName in request.SolutionFullNames ?? new string[0])
			{
				var isDirty = false;
				var success = true;

				if (success && request.UpdateWorkingCopyFromSourceControl)
				{
					success = SourceControlClientApi.UpdateWorkingCopy(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.UpdateWorkingCopyRequest()
					{
						FullName = solutionFullName,
						IncludeExternals = true,
					}).Success;

					if (!success)
					{
						Logger.LogError(string.Format("Error updating \"{0}\"", solutionFullName));
					}
				}

				var nugetConfigFullNames = NugetApi.GetNugetConfigFullNames(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetConfigFullNamesRequest()
				{
					WorkingCopyDirectory = solutionFullName,
				}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);


				bool TryGetNugetPackageKey(string id, out ISI.Extensions.Nuget.NugetPackageKey key)
				{
					if (nugetPackageKeys.TryGetValue(id, out key))
					{
						return true;
					}

					if (ignorePackageIds.Contains(id))
					{
						return false;
					}

					var getLatestPackageVersionResponse = NugetApi.GetLatestPackageVersion(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetLatestPackageVersionRequest()
					{
						PackageId = id,
						NugetConfigFullNames = nugetConfigFullNames,
					});

					nugetPackageKeys.TryAdd(id, getLatestPackageVersionResponse.PackageVersion, getLatestPackageVersionResponse.HintPath);

					return nugetPackageKeys.TryGetValue(id, out key);
				}

				var csProjFileNames = System.IO.Directory.GetFiles(solutionFullName, "*.csproj", System.IO.SearchOption.AllDirectories).Where(fullName => !SourceControlClientApi.IsSccDirectory(fullName));

				foreach (var csProjFileName in csProjFileNames)
				{
					var projectDirectory = System.IO.Path.GetDirectoryName(csProjFileName);

					var packagesConfigFullName = System.IO.Path.Combine(projectDirectory, "packages.config");

					if (System.IO.File.Exists(packagesConfigFullName))
					{
						var packagesConfig = System.IO.File.ReadAllText(packagesConfigFullName);

						try
						{
							var newPackagesConfig = NugetApi.UpdateNugetPackageVersionsInPackagesConfig(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInPackagesConfigRequest()
							{
								PackagesConfigXml = packagesConfig,
								TryGetNugetPackageKey = TryGetNugetPackageKey,
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

					var csProj = System.IO.File.ReadAllText(csProjFileName);

					try
					{
						var newCsProj = NugetApi.UpdateNugetPackageVersionsInCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInCsProjRequest()
						{
							CsProjXml = csProj,
							TryGetNugetPackageKey = TryGetNugetPackageKey,
							TryGetPackageHintPath = (ISI.Extensions.Nuget.NugetPackageKey nugetPackageKey, out string hintPath) =>
							{
								if (!ignorePackageIds.Contains(nugetPackageKey.Package))
								{
									hintPath = NugetApi.GetNugetPackageHintPath(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageHintPathRequest()
									{
										PackageId = nugetPackageKey.Package,
										PackageVersion = nugetPackageKey.Version,
										NugetConfigFullNames = nugetConfigFullNames,
									}).HintPath;

									return !string.IsNullOrWhiteSpace(hintPath);
								}

								hintPath = string.Empty;
								return false;
							},
							ConvertToPackageReferences = false,
						}).CsProjXml;

						if (HasChanges(csProj, newCsProj))
						{
							System.IO.File.WriteAllText(csProjFileName, newCsProj);
							isDirty = true;
						}
					}
					catch (Exception exception)
					{
						throw new Exception(string.Format("File: {0}", csProjFileName), exception);
					}
				}

				if (success && isDirty && request.CommitWorkingCopyToSourceControl)
				{
					success = SourceControlClientApi.CommitWorkingCopy(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.CommitWorkingCopyRequest()
					{
						FullName = solutionFullName,
						LogMessage = "update nuget packages",
					}).Success;

					if (!success)
					{
						Logger.LogError(string.Format("Error commiting \"{0}\"", solutionFullName));
					}
				}
			}

			return response;
		}
	}
}