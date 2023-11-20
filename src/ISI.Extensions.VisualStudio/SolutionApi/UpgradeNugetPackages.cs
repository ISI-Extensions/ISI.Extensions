#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public DTOs.UpgradeNugetPackagesResponse UpgradeNugetPackages(DTOs.UpgradeNugetPackagesRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var getBuildServiceSolutionLock = request.GetBuildServiceSolutionLock ?? ((solutionFullName, addToLog) => new PhonyBuildServiceSolutionLock());

			var response = new DTOs.UpgradeNugetPackagesResponse();

			var ignorePackageIds = new HashSet<string>(request.IgnorePackageIds ?? Array.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);

			var nugetPackageKeys = request.NugetPackageKeys ?? new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			var solutionFullNames = request.SolutionFullNames.Where(solution => System.IO.Directory.Exists(solution) || System.IO.File.Exists(solution)).ToArray();

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => GetSolutionDetails(new()
			{
				Solution = solution,
				AddToLog = request.AddToLog,
			}).SolutionDetails, NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			logger.LogInformation("Update Nuget Packages For Solutions:");
			foreach (var solutionDetails in solutionDetailsSet)
			{
				logger.LogInformation(string.Format("  {0}", solutionDetails.SolutionName));
			}
			logger.LogInformation(string.Empty);

			if (request.UpdateWorkingCopyFromSourceControl)
			{
				foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
				{
					using (getBuildServiceSolutionLock(solutionDetails.SolutionFullName, description => logger.LogInformation(description)))
					{
						using (GetSolutionLock(new()
						{
							SolutionFullName = solutionDetails.SolutionFullName,
							AddToLog = description => logger.LogInformation(description),
						}).Lock)
						{
							logger.LogInformation(string.Format("Updating {0} from Source Control", solutionDetails.SolutionName));

							if (!SourceControlClientApi.UpdateWorkingCopy(new()
							{
								FullName = solutionDetails.RootSourceDirectory,
								IncludeExternals = true,
								AddToLog = description => logger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error updating \"{0}\"", solutionDetails.RootSourceDirectory));
								logger.LogError(exception.Message);
								throw exception;
							}
						}
					}
				}

				solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => GetSolutionDetails(new()
				{
					Solution = solution,
				}).SolutionDetails, NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();
			}
			
			logger.LogInformation("Upgrade Nuget Packages For Solutions:");
			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				logger.LogInformation(string.Format("  {0}", solutionDetails.SolutionName));
			}
			logger.LogInformation(string.Empty);
			
			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				using (getBuildServiceSolutionLock(solutionDetails.SolutionFullName, description => logger.LogInformation(description)))
				{
					using (GetSolutionLock(new()
					{
						SolutionFullName = solutionDetails.SolutionFullName,
						AddToLog = description => logger.LogInformation(description),
					}).Lock)
					{
						logger.LogInformation(string.Format("Updating {0} from Source Control", solutionDetails.SolutionName));

						var dirtyFileNames = new HashSet<string>();

						if (request.UpdateWorkingCopyFromSourceControl)
						{
							if (!SourceControlClientApi.UpdateWorkingCopy(new()
							{
								FullName = solutionDetails.RootSourceDirectory,
								IncludeExternals = true,
								AddToLog = description => logger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error updating \"{0}\"", solutionDetails.RootSourceDirectory));
								logger.LogError(exception.Message);
								throw exception;
							}
						}

						try
						{
							var nugetConfigFullNames = NugetApi.GetNugetConfigFullNames(new()
							{
								WorkingCopyDirectory = solutionDetails.SolutionDirectory,
							}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

							var solutionIgnorePackageIds = new HashSet<string>(solutionDetails.DoNotUpdatePackages ?? Array.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);
							solutionIgnorePackageIds.UnionWith(ignorePackageIds);

							void addNugetPackageKey(string package)
							{
								var getLatestPackageVersionResponse = NugetApi.GetNugetPackageKey(new()
								{
									Package = package,
									NugetConfigFullNames = nugetConfigFullNames,
								});

								if (getLatestPackageVersionResponse.NugetPackageKey != null)
								{
									logger.LogInformation(string.Format("  Added {0} {1}", getLatestPackageVersionResponse.NugetPackageKey.Package, getLatestPackageVersionResponse.NugetPackageKey.Version));

									nugetPackageKeys.TryAdd(getLatestPackageVersionResponse.NugetPackageKey);

									if (getLatestPackageVersionResponse.NugetPackageKey.Dependencies.NullCheckedAny())
									{
										foreach (var nugetPackageDependency in getLatestPackageVersionResponse.NugetPackageKey.Dependencies)
										{
											if (!nugetPackageKeys.ContainsKey(nugetPackageDependency.Package))
											{
												addNugetPackageKey(nugetPackageDependency.Package);
											}
										}
									}
								}
							}

							bool tryGetNugetPackageKey(string package, out ISI.Extensions.Nuget.NugetPackageKey nugetPackageKey)
							{
								logger.LogInformation($"  tryGetNugetPackageKey for {package}");
								
								if (solutionIgnorePackageIds.Contains(package))
								{
									nugetPackageKey = null;

									return false;
								}

								if (nugetPackageKeys.TryGetValue(package, out nugetPackageKey))
								{
									return true;
								}

								using (NugetApi.GetNugetLock(new()
								{
									AddToLog = description => logger.LogInformation(description),
								}).Lock)
								{
									addNugetPackageKey(package);
								}

								return nugetPackageKeys.TryGetValue(package, out nugetPackageKey);
							}

							logger.LogInformation("Updating Projects");

							foreach (var projectDetails in solutionDetails.ProjectDetailsSet.OrderBy(projectDetails => projectDetails.ProjectFullName, StringComparer.InvariantCultureIgnoreCase))
							{
								logger.LogInformation(string.Format("  {0}", projectDetails.ProjectName));

								var packagesConfigFullName = System.IO.Path.Combine(projectDetails.ProjectDirectory, "packages.config");

								if (System.IO.File.Exists(packagesConfigFullName))
								{
									if (request.ConvertToPackageReferences)
									{
										System.IO.File.Delete(packagesConfigFullName);
										dirtyFileNames.Add(packagesConfigFullName);
									}
									else
									{
										var packagesConfig = System.IO.File.ReadAllText(packagesConfigFullName);

										try
										{
											var newPackagesConfig = NugetApi.UpgradeNugetPackageVersionsInPackagesConfig(new()
											{
												PackagesConfigXml = packagesConfig,
												TryGetNugetPackageKey = tryGetNugetPackageKey,
											}).PackagesConfigXml;

											if (HasChanges(packagesConfig, newPackagesConfig))
											{
												System.IO.File.WriteAllText(packagesConfigFullName, newPackagesConfig);
												dirtyFileNames.Add(packagesConfigFullName);
											}
										}
										catch (Exception exception)
										{
											throw new(string.Format("File: {0}", packagesConfigFullName), exception);
										}
									}
								}

								var csProj = System.IO.File.ReadAllText(projectDetails.ProjectFullName);

								try
								{
									var newCsProj = NugetApi.UpgradeNugetPackageVersionsInCsProj(new()
									{
										CsProjXml = csProj,
										TryGetNugetPackageKey = tryGetNugetPackageKey,
										ConvertToPackageReferences = request.ConvertToPackageReferences,
									}).CsProjXml;

									if (HasChanges(csProj, newCsProj))
									{
										System.IO.File.WriteAllText(projectDetails.ProjectFullName, newCsProj);
										dirtyFileNames.Add(projectDetails.ProjectFullName);
									}
								}
								catch (Exception exception)
								{
									throw new(string.Format("File: {0}", projectDetails.ProjectFullName), exception);
								}

								var appConfigFileName = System.IO.Path.Combine(projectDetails.ProjectDirectory, "web.config");
								if (!System.IO.File.Exists(appConfigFileName))
								{
									appConfigFileName = System.IO.Path.Combine(projectDetails.ProjectDirectory, "app.config");

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
										var newAppConfigXml = NugetApi.UpgradeAssemblyRedirects(new()
										{
											CsProjXml = csProj,
											AppConfigXml = appConfigXml,
											NugetPackageKeys = nugetPackageKeys.Where(nugetPackageKey => !solutionIgnorePackageIds.Contains(nugetPackageKey.Package)),
											UpsertAssemblyRedirectsNugetPackageKeys = request.UpsertAssemblyRedirectsNugetPackageKeys,
											RemoveAssemblyRedirects = request.RemoveAssemblyRedirects,
										}).AppConfigXml;

										if (HasChanges(appConfigXml, newAppConfigXml))
										{
											System.IO.File.WriteAllText(appConfigFileName, newAppConfigXml);
											dirtyFileNames.Add(appConfigFileName);
										}
									}
									catch (Exception exception)
									{
										throw new(string.Format("File: {0}", appConfigFileName), exception);
									}
								}
							}
						}
						catch (Exception exception)
						{
							logger.LogError(exception.ErrorMessageFormatted());
							throw;
						}

						if (dirtyFileNames.Any() && request.CommitWorkingCopyToSourceControl)
						{
							try
							{
								var resharperCacheFullName = System.IO.Path.Combine(solutionDetails.SolutionDirectory, "_ReSharper.Caches");
								if (System.IO.Directory.Exists(resharperCacheFullName))
								{
									var directoryInfo = new System.IO.DirectoryInfo(resharperCacheFullName)
									{
										Attributes = System.IO.FileAttributes.Normal
									};
									
									foreach (var fileSystemInfo in directoryInfo.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
									{
										fileSystemInfo.Attributes = System.IO.FileAttributes.Normal;
									}

									System.IO.Directory.Delete(resharperCacheFullName, true);
								}
							}
							catch (Exception exception)
							{
								logger.LogError(string.Format("Error deleting Resharper Cache \"{0}\"", solutionDetails.SolutionDirectory));
							}

							if (!SourceControlClientApi.Commit(new()
							{
								FullNames = dirtyFileNames,
								LogMessage = "update nuget packages",
								AddToLog = description => logger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error committing \"{0}\"", solutionDetails.RootSourceDirectory));
								logger.LogError(exception.Message);
								throw exception;
							}
						}

						logger.LogInformation(string.Format("Upgraded Nuget Packages in {0}", solutionDetails.SolutionName));
					}

					if (!string.IsNullOrWhiteSpace(solutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages))
					{
						if (BuildScriptApi.TryGetBuildScript(solutionDetails.SolutionDirectory, out var buildScriptFullName))
						{
							logger.LogInformation(string.Format("Building {0}", solutionDetails.SolutionName));
							logger.LogInformation(string.Format("  BuildScriptFullName: {0}", buildScriptFullName));

							try
							{
								var nugetPackOutputDirectory = System.IO.Path.Combine(solutionDetails.RootSourceDirectory, "Nuget");

								var executeBuildTargetResponse = BuildScriptApi.ExecuteBuildTarget(new()
								{
									BuildScriptFullName = buildScriptFullName,
									Target = solutionDetails.ExecuteBuildScriptTargetAfterUpdateNugetPackages,
									Parameters = new[]
									{
										(ParameterName: "NugetPackOutputDirectory", ParameterValue: nugetPackOutputDirectory)
									},
									UseShell = false,
									AddToLog = description => logger.LogInformation(description),
								});

								if (executeBuildTargetResponse.Success)
								{
									System.Threading.Thread.Sleep(TimeSpan.FromMinutes(4));
								}
								else
								{
									var exception = new Exception(string.Format("Error Building \"{0}\"", solutionDetails.RootSourceDirectory));
									logger.LogError(exception.Message);
									throw exception;
								}

								if (System.IO.Directory.Exists(nugetPackOutputDirectory))
								{
									var updatedNugetPackageKeys = NugetApi.ListNugetPackageKeys(new()
									{
										Source = nugetPackOutputDirectory,
									}).NugetPackageKeys;

									if (updatedNugetPackageKeys.NullCheckedAny())
									{
										logger.LogInformation(string.Format("Refreshing NugetPackageKeys From: \"{0}\"", nugetPackOutputDirectory));
										foreach (var updatedNugetPackageKey in updatedNugetPackageKeys)
										{
											logger.LogInformation(string.Format("  {0} => {1}", updatedNugetPackageKey.Package, updatedNugetPackageKey.Version));
										}
										logger.LogInformation(string.Empty);

										nugetPackageKeys.Merge(updatedNugetPackageKeys);
									}
									else
									{
										nugetPackageKeys.Clear();
									}
								}
								else
								{
									nugetPackageKeys.Clear();
								}
							}
							catch (Exception exception)
							{
								logger.LogError(exception.ErrorMessageFormatted());
								throw;
							}

							logger.LogInformation(string.Format("Built {0}", solutionDetails.SolutionName));
						}
					}
				}
			}

			return response;
		}
	}
}