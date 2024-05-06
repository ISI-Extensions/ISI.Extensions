#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
		internal class SolutionDetailsWithNugetPackageDependencies : ISI.Extensions.VisualStudio.SolutionDetails
		{
			public bool IsBuilt { get; set; }
			public HashSet<string> NugetPackageDependencies { get; set; }
			public HashSet<string> NugetPackageDependenciesFromOtherSolutions { get; set; }
		}

		public DTOs.UpgradeNugetPackagesResponse UpgradeNugetPackages(DTOs.UpgradeNugetPackagesRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var getBuildServiceSolutionLock = request.GetBuildServiceSolutionLock ?? ((solutionFullName, addToLog) => new PhonyBuildServiceSolutionLock());

			var response = new DTOs.UpgradeNugetPackagesResponse();

			var ignorePackageIds = new HashSet<string>(request.IgnorePackageIds ?? Array.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);

			var nugetPackageKeys = request.NugetPackageKeys ?? new ISI.Extensions.Nuget.NugetPackageKeyDictionary();

			var solutionFullNames = request.SolutionFullNames
				.NullCheckedSelect(solution => GetSolutionFullName(new() { Solution = solution }).SolutionFullName, NullCheckCollectionResult.Empty)
				.Where(solutionFullName => !string.IsNullOrWhiteSpace(solutionFullName))
				.OrderBy(System.IO.Path.GetFileNameWithoutExtension, StringComparer.InvariantCultureIgnoreCase)
				.ToArray();

			var isProjectBuilt = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

			logger.LogInformation("Update Nuget Packages For Solutions:");
			foreach (var solutionFullName in solutionFullNames)
			{
				logger.LogInformation(string.Format("  {0}", solutionFullName));
			}
			logger.LogInformation(string.Empty);

			ILogger getSolutionLogger(string solutionFullName)
			{
				if (request.SetStatus != null)
				{
					return new AddToLogLogger((level, description) => request.SetStatus(solutionFullName, description), logger);
				}
				return logger;
			}

			if (request.UpdateWorkingCopyFromSourceControl && solutionFullNames.Any())
			{
				foreach (var solutionFullName in solutionFullNames)
				{
					var solutionLogger = getSolutionLogger(solutionFullName);

					using (getBuildServiceSolutionLock(solutionFullName, (logEntryLevel, description) => solutionLogger.LogInformation(description)))
					{
						using (GetSolutionLock(new()
						{
							SolutionFullName = solutionFullName,
							AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
						}).Lock)
						{
							solutionLogger.LogInformation(string.Format("Updating {0} from Source Control", solutionFullName));

							var rootSourceDirectory = SourceControlClientApi.GetRootDirectory(new()
							{
								FullName = System.IO.Path.GetDirectoryName(solutionFullName),
							}).FullName;

							if (!SourceControlClientApi.UpdateWorkingCopy(new()
							{
								FullName = rootSourceDirectory,
								IncludeExternals = true,
								AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error updating \"{0}\"", rootSourceDirectory));
								solutionLogger.LogError(exception.Message);
								throw exception;
							}

							solutionLogger.LogInformation(string.Format("Updated {0} from Source Control", rootSourceDirectory));
						}
					}
				}
			}

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution =>
				{
					var solutionDetails = GetSolutionDetails(new()
					{
						Solution = solution,
						AddToLog = request.AddToLog,
					}).SolutionDetails;

					var nugetPackageDependencies = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

					foreach (var projectDetails in solutionDetails.ProjectDetailsSet)
					{
						if (!isProjectBuilt.ContainsKey(projectDetails.ProjectName))
						{
							isProjectBuilt.Add(projectDetails.ProjectName, false);

							var projectNugetPackageDependencies = NugetApi.GetProjectNugetPackageDependencies(new()
							{
								ProjectFullName = projectDetails.ProjectFullName,
							}).NugetPackageKeys;

							foreach (var projectNugetPackageDependency in projectNugetPackageDependencies)
							{
								nugetPackageDependencies.Add(projectNugetPackageDependency.Package);
							}
						}
					}

					return new SolutionDetailsWithNugetPackageDependencies()
					{
						SolutionName = solutionDetails.SolutionName,
						SolutionDirectory = solutionDetails.SolutionDirectory,
						SolutionFullName = solutionDetails.SolutionFullName,
						RootSourceDirectory = solutionDetails.RootSourceDirectory,
						ProjectDetailsSet = solutionDetails.ProjectDetailsSet.ToNullCheckedArray(),
						SolutionFilterDetailsSet = solutionDetails.SolutionFilterDetailsSet.ToNullCheckedArray(),
						UpgradeNugetPackagesPriority = solutionDetails.UpgradeNugetPackagesPriority,
						ExecuteBuildScriptTargetAfterUpgradeNugetPackages = solutionDetails.ExecuteBuildScriptTargetAfterUpgradeNugetPackages,
						DoNotUpgradePackages = solutionDetails.DoNotUpgradePackages.ToNullCheckedArray(),
						IsBuilt = false,
						NugetPackageDependencies = new HashSet<string>(nugetPackageDependencies, StringComparer.InvariantCultureIgnoreCase),
						NugetPackageDependenciesFromOtherSolutions = new HashSet<string>(nugetPackageDependencies, StringComparer.InvariantCultureIgnoreCase),
					};
				}, NullCheckCollectionResult.Empty).Where(solutionDetails => solutionDetails != null).ToArray();

			foreach (var solutionDetails in solutionDetailsSet)
			{
				solutionDetails.NugetPackageDependenciesFromOtherSolutions.RemoveWhere(nugetPackageDependency => !isProjectBuilt.ContainsKey(nugetPackageDependency));
			}

			while (solutionDetailsSet.Any(solutionDetails => !solutionDetails.IsBuilt) && !request.CancellationToken.IsCancellationRequested)
			{
				var solutionDetails = solutionDetailsSet
																.Where(solutionDetails => !solutionDetails.IsBuilt)
																.Where(solutionDetails => solutionDetails.NugetPackageDependenciesFromOtherSolutions.All(nugetPackageDependency => isProjectBuilt[nugetPackageDependency]))
																.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase)
																.FirstOrDefault() ??
															solutionDetailsSet
																.Where(solutionDetails => !solutionDetails.IsBuilt)
																.Where(solutionDetails => !solutionDetails.NugetPackageDependenciesFromOtherSolutions.Any())
																.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase)
																.FirstOrDefault() ??
															solutionDetailsSet
																.Where(solutionDetails => !solutionDetails.IsBuilt)
																.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase)
																.FirstOrDefault();

				var stopWatch = new System.Diagnostics.Stopwatch();
				stopWatch.Start();

				var solutionLogger = getSolutionLogger(solutionDetails.SolutionFullName);

				using (getBuildServiceSolutionLock(solutionDetails.SolutionFullName, (logEntryLevel, description) => solutionLogger.LogInformation(description)))
				{
					request.PreAction?.Invoke(solutionDetails.SolutionFullName);

					using (GetSolutionLock(new()
					{
						SolutionFullName = solutionDetails.SolutionFullName,
						AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
					}).Lock)
					{
						solutionLogger.LogInformation(string.Format("Updating {0} from Source Control", solutionDetails.SolutionName));

						var dirtyFileNames = new HashSet<string>();

						if (request.UpdateWorkingCopyFromSourceControl)
						{
							if (!SourceControlClientApi.UpdateWorkingCopy(new()
							{
								FullName = solutionDetails.RootSourceDirectory,
								IncludeExternals = true,
								AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error updating \"{0}\"", solutionDetails.RootSourceDirectory));
								solutionLogger.LogError(exception.Message);
								throw exception;
							}
						}

						solutionLogger.LogInformation(string.Format("Upgrading Nuget Packages in {0}", solutionDetails.SolutionName));

						try
						{
							var nugetConfigFullNames = NugetApi.GetNugetConfigFullNames(new()
							{
								WorkingCopyDirectory = solutionDetails.SolutionDirectory,
							}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

							var solutionIgnorePackageIds = new HashSet<string>(solutionDetails.DoNotUpgradePackages ?? [], StringComparer.InvariantCultureIgnoreCase);
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
									solutionLogger.LogInformation(string.Format("  Added {0} {1}", getLatestPackageVersionResponse.NugetPackageKey.Package, getLatestPackageVersionResponse.NugetPackageKey.Version));

									nugetPackageKeys.TryAdd(getLatestPackageVersionResponse.NugetPackageKey);
								}
							}

							bool tryGetNugetPackageKey(string package, out ISI.Extensions.Nuget.NugetPackageKey nugetPackageKey)
							{
								if (solutionIgnorePackageIds.Contains(package))
								{
									nugetPackageKey = null;

									return false;
								}

								if (nugetPackageKeys.TryGetValue(package, out nugetPackageKey))
								{
									return true;
								}

								addNugetPackageKey(package);

								return nugetPackageKeys.TryGetValue(package, out nugetPackageKey);
							}

							Parallel.ForEach(solutionDetails.NugetPackageDependencies, nugetPackageDependency =>
							{
								tryGetNugetPackageKey(nugetPackageDependency, out var _);
							});

							solutionLogger.LogInformation("Updating Projects");

							foreach (var projectDetails in solutionDetails.ProjectDetailsSet.OrderBy(projectDetails => projectDetails.ProjectFullName, StringComparer.InvariantCultureIgnoreCase))
							{
								solutionLogger.LogInformation(string.Format("  {0}", projectDetails.ProjectName));

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
							solutionLogger.LogError(exception.ErrorMessageFormatted());
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
								solutionLogger.LogError(string.Format("Error deleting Resharper Cache \"{0}\"", solutionDetails.SolutionDirectory));
							}

							if (!SourceControlClientApi.Commit(new()
							{
								FullNames = dirtyFileNames,
								LogMessage = "update nuget packages",
								AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
							}).Success)
							{
								var exception = new Exception(string.Format("Error committing \"{0}\"", solutionDetails.RootSourceDirectory));
								solutionLogger.LogError(exception.Message);
								throw exception;
							}
						}

						solutionLogger.LogInformation(string.Format("Upgraded Nuget Packages in {0}", solutionDetails.SolutionName));
					}

					if (!string.IsNullOrWhiteSpace(solutionDetails.ExecuteBuildScriptTargetAfterUpgradeNugetPackages))
					{
						if (BuildScriptApi.TryGetBuildScript(solutionDetails.SolutionDirectory, out var buildScriptFullName))
						{
							solutionLogger.LogInformation(string.Format("Building {0}", solutionDetails.SolutionName));
							solutionLogger.LogInformation(string.Format("  BuildScriptFullName: {0}", buildScriptFullName));

							try
							{
								var nugetPackOutputDirectory = System.IO.Path.Combine(solutionDetails.RootSourceDirectory, "Nuget");

								NugetApi.RestoreNugetPackages(new()
								{
									Solution = solutionDetails.SolutionFullName,
									MSBuildExe = MSBuildApi.GetMSBuildExeFullName(new()).MSBuildExeFullName,

									AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
								});

								var executeBuildTargetResponse = BuildScriptApi.ExecuteBuildTarget(new()
								{
									BuildScriptFullName = buildScriptFullName,
									Target = solutionDetails.ExecuteBuildScriptTargetAfterUpgradeNugetPackages,
									Parameters = new[]
									{
										(ParameterName: "NugetPackOutputDirectory", ParameterValue: nugetPackOutputDirectory)
									},
									UseShell = false,
									AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
								});

								if (executeBuildTargetResponse.Success)
								{
									if (!System.IO.Directory.Exists(nugetPackOutputDirectory))
									{
										logger.LogInformation(string.Format("Sleeping for 4 min because \"{0}\" doesn't exist", nugetPackOutputDirectory));
										System.Threading.Thread.Sleep(TimeSpan.FromMinutes(4));
									}
								}
								else
								{
									var exception = new Exception(string.Format("Error Building \"{0}\"", solutionDetails.RootSourceDirectory));
									solutionLogger.LogError(exception.Message);
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
										var locallyCacheNupkgsResponse = NugetApi.LocallyCacheNupkgs(new()
										{
											NupkgFullNames = updatedNugetPackageKeys.Select(nugetPackageKey => System.IO.Path.Combine(nugetPackOutputDirectory, $"{nugetPackageKey.Package}.{nugetPackageKey.Version}.nupkg")),
											AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
										});

										solutionLogger.LogInformation(string.Format("Refreshing NugetPackageKeys From: \"{0}\"", nugetPackOutputDirectory));
										foreach (var updatedNugetPackageKey in updatedNugetPackageKeys)
										{
											solutionLogger.LogInformation(string.Format("  {0} => {1}", updatedNugetPackageKey.Package, updatedNugetPackageKey.Version));
										}
										solutionLogger.LogInformation(string.Empty);

										nugetPackageKeys.Merge(updatedNugetPackageKeys);

										//var cachedNugetPackageKeys = locallyCacheNupkgsResponse.CachedNugetPackageKeys;
										//var maxAttemptsToCheckAvailability = 5;

										//while (cachedNugetPackageKeys.Any() && (maxAttemptsToCheckAvailability-- > 0))
										//{
										//	var checkAvailabilityOfNupkgsResponse = NugetApi.CheckAvailabilityOfNupkgs(new()
										//	{
										//		NugetPackageKeys = cachedNugetPackageKeys,
										//		AddToLog = (logEntryLevel, description) => solutionLogger.LogInformation(description),
										//	});

										//	cachedNugetPackageKeys = checkAvailabilityOfNupkgsResponse.NupkgAvailabilities.Where(nupkgAvailability => !nupkgAvailability.Available).Select(nupkgAvailability => nupkgAvailability.NugetPackageKey).ToArray();

										//	if (cachedNugetPackageKeys.Any() && (maxAttemptsToCheckAvailability > 0))
										//	{
										//		solutionLogger.LogInformation("  The following Nupkg(s) not available yet, will check again in 30 seconds");

										//		System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
										//	}
										//}
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

								solutionLogger.LogInformation(string.Format("Built {0}", solutionDetails.SolutionName));
							}
							catch (Exception exception)
							{
								solutionLogger.LogError(exception.ErrorMessageFormatted());

								request.BuildScriptError?.Invoke(solutionDetails.SolutionFullName);

								if (!request.ContinueOnBuildScriptError)
								{
									throw;
								}
							}
						}
					}

					foreach (var projectDetails in solutionDetails.ProjectDetailsSet)
					{
						isProjectBuilt[projectDetails.ProjectName] = true;
					}

					solutionDetails.IsBuilt = true;

					request.PostAction?.Invoke(solutionDetails.SolutionFullName);
				}

				stopWatch.Stop();
				solutionLogger.LogInformation($"Nuget Upgrade took: {stopWatch.Elapsed.Formatted(TimeSpanExtensions.TimeSpanFormat.Default)}");
				solutionLogger.LogInformation(string.Empty);
			}

			return response;
		}
	}
}