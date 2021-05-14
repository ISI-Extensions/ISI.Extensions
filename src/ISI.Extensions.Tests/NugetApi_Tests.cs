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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class NugetApi_Tests
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration)

				//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
				.AddLogging(builder => builder
						.AddConsole()
				//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
				)
				.AddTransient<Microsoft.Extensions.Logging.ILogger>(serviceProvider => serviceProvider.GetService<ILoggerFactory>().CreateLogger<Microsoft.VisualStudio.TestPlatform.TestHost.Program>())

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddConfigurationRegistrations(configuration)
				.ProcessServiceRegistrars()
				;






			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void Nuspec_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ConsoleLogger());

			var nuspec = nugetApi.GenerateNuspecFromProject(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GenerateNuspecFromProjectRequest()
			{
				ProjectFullName = @"F:\ISI\ISI.FrameWork\src\ISI.Wrappers\ISI.Wrappers.MassTransit\ISI.Wrappers.MassTransit.csproj",
				TryGetPackageVersion = (string package, out string version) =>
					 {
						 if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
						 {
							 version = "10.0.*";
							 return true;
						 }

						 if (package.StartsWith("ISI.Libraries", StringComparison.InvariantCultureIgnoreCase))
						 {
							 version = "10.0.*";
							 return true;
						 }

						 version = string.Empty;
						 return false;
					 }
			}).Nuspec;

			nuspec.Version = "2.0.0.0";
			//nuspec.IconUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions/Lantern.png");
			//nuspec.ProjectUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions");
			nuspec.Title = "ISI.Libraries";
			nuspec.Description = "ISI.Libraries";
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new[] { "Integrated Solutions, Inc." };
			nuspec.Owners = new[] { "Integrated  Solutions, Inc." };


			var xxx = nugetApi.BuildNuspec(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.BuildNuspecRequest()
			{
				Nuspec = nuspec,
			});
		}



		[Test]
		public void GetLatestPackageVersion_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ConsoleLogger());

			var packageVersion = nugetApi.GetLatestPackageVersion(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetLatestPackageVersionRequest()
			{
				PackageId = "ISI.Libraries",
			}).PackageVersion;
		}


		private bool HasChanges(string original, string newVersion)
		{
			string getCompressed(string value) => value
				.Replace(" ", string.Empty)
				.Replace("\t", string.Empty)
				.Replace("\r", string.Empty)
				.Replace("\n", string.Empty);

			return !string.Equals(getCompressed(original), getCompressed(newVersion), StringComparison.InvariantCultureIgnoreCase);
		}

		[Test]
		public void UpdatePackageVersions_Test()
		{
			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();
			var packageNugetServers = new Dictionary<string, string>();

			var mainNugetPackageForConsideration = new HashSet<string>();
			mainNugetPackageForConsideration.Add("AWSSDK.Core");
			mainNugetPackageForConsideration.Add("FluentValidation");
			mainNugetPackageForConsideration.Add("Polly");

			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.ConsoleLogger());

			{
				var sourceSolution = @"F:\ISI\ISI.FrameWork\src";

				var sourceCsProjFileNames = System.IO.Directory.GetFiles(sourceSolution, "*.csproj", System.IO.SearchOption.AllDirectories).Where(f => f.Substring(sourceSolution.Length).IndexOf("\\.svn\\") < 0);

				foreach (var csProjFileName in sourceCsProjFileNames)
				{
					var projectDirectory = System.IO.Path.GetDirectoryName(csProjFileName);

					var packagesConfigFullName = System.IO.Path.Combine(projectDirectory, "packages.config");

					if (System.IO.File.Exists(packagesConfigFullName))
					{
						nugetPackageKeys.Merge(nugetApi.ExtractProjectNugetPackageDependenciesFromPackagesConfig(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ExtractProjectNugetPackageDependenciesFromPackagesConfigRequest()
						{
							PackagesConfigFullName = packagesConfigFullName,
						}).NugetPackageKeys);
					}
				}

				foreach (var csProjFileName in sourceCsProjFileNames)
				{
					nugetPackageKeys.Merge(nugetApi.ExtractProjectNugetPackageDependenciesFromCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ExtractProjectNugetPackageDependenciesFromCsProjRequest()
					{
						CsProjFullName = csProjFileName,
					}).NugetPackageKeys);
				}

				//var packageNugetServers = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
				packageNugetServers.Add("ISI.*", "https://nuget.isi-net.com");
				packageNugetServers.Add("ICS.*", "https://nuget.swdcentral.com");
				packageNugetServers.Add("Tristar.*", "https://nuget.tristarfulfillment.com");
			}

			var solution = @"F:\ISI\Clients\TFS\Tristar.Scheduler";

			var dirtyFileNames = new HashSet<string>();

			bool TryGetNugetPackageKey(string id, out ISI.Extensions.Nuget.NugetPackageKey key)
			{
				if (nugetPackageKeys.TryGetValue(id, out key))
				{
					return true;
				}

				var version = nugetApi.GetLatestPackageVersion(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetLatestPackageVersionRequest()
				{
					PackageId = id,
				}).PackageVersion;

				nugetPackageKeys.TryAdd(id, version);

				return nugetPackageKeys.TryGetValue(id, out key);
			}

			var csProjFileNames = System.IO.Directory.GetFiles(solution, "*.csproj", System.IO.SearchOption.AllDirectories).Where(f => (f.Substring(solution.Length).IndexOf("\\.svn\\") < 0) && (f.Substring(solution.Length).IndexOf("\\src\\Resources\\") < 0));

			foreach (var csProjFileName in csProjFileNames)
			{
				var projectDirectory = System.IO.Path.GetDirectoryName(csProjFileName);

				var packagesConfigFullName = System.IO.Path.Combine(projectDirectory, "packages.config");

				if (System.IO.File.Exists(packagesConfigFullName))
				{
					var packagesConfig = System.IO.File.ReadAllText(packagesConfigFullName);

					try
					{
						var newPackagesConfig = nugetApi.UpdateNugetPackageVersionsInPackagesConfig(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInPackagesConfigRequest()
						{
							PackagesConfigXml = packagesConfig,
							TryGetNugetPackageKey = TryGetNugetPackageKey,
						}).PackagesConfigXml;

						if (HasChanges(packagesConfig, newPackagesConfig))
						{
							System.IO.File.WriteAllText(packagesConfigFullName, newPackagesConfig);

							dirtyFileNames.Add(packagesConfigFullName);
						}
					}
					catch (Exception exception)
					{
						throw new Exception(string.Format("File: {0}", packagesConfigFullName), exception);
					}
				}
			}

			foreach (var csProjFileName in csProjFileNames)
			{
				var csProj = System.IO.File.ReadAllText(csProjFileName);

				try
				{
					var newCsProj = nugetApi.UpdateNugetPackageVersionsInCsProj(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.UpdateNugetPackageVersionsInCsProjRequest()
					{
						CsProjXml = csProj,
						TryGetNugetPackageKey = TryGetNugetPackageKey,
						TryGetPackageHintPath = (ISI.Extensions.Nuget.NugetPackageKey nugetPackageKey, out string hintPath) =>
						{
							if (nugetPackageKey.Package.StartsWith("ISI.Extensions"))
							{
								hintPath = string.Format("{0}.{1}\\lib\\netstandard2.0\\{0}.dll", nugetPackageKey.Package, nugetPackageKey.Version);
								return true;
							}
							else if (nugetPackageKey.Package.StartsWith("ISI."))
							{
								hintPath = string.Format("{0}.{1}\\lib\\net48\\{0}.dll", nugetPackageKey.Package, nugetPackageKey.Version);
								return true;
							}
							else if (nugetPackageKey.Package.StartsWith("Tristar."))
							{
								hintPath = string.Format("{0}.{1}\\lib\\net48\\{0}.dll", nugetPackageKey.Package, nugetPackageKey.Version);
								return true;
							}
							else if (nugetPackageKey.Package.StartsWith("ICS."))
							{
								hintPath = string.Format("{0}.{1}\\lib\\net48\\{0}.dll", nugetPackageKey.Package, nugetPackageKey.Version);
								return true;
							}

							{
								var nugetDownloadUrl = string.Format("https://www.nuget.org/api/v2/package/{0}/{1}", nugetPackageKey.Package, nugetPackageKey.Version);

								var downloadResponse = ISI.Extensions.WebClient.Download.DownloadFile<System.IO.MemoryStream>(nugetDownloadUrl, new ISI.Extensions.WebClient.HeaderCollection());

								var zipArchive = new System.IO.Compression.ZipArchive(downloadResponse.Stream, System.IO.Compression.ZipArchiveMode.Read);

								var dlls = zipArchive.Entries.Select(entry => entry.FullName).Where(fullName => fullName.EndsWith(string.Format("{0}.dll", nugetPackageKey.Package))).ToArray();

								foreach (var dllPrefix in new[]
								{
										"lib/net48/",
										"lib/net4.8/",
										"lib/net472/",
										"lib/net4.72/",
										"lib/net461/",
										"lib/net4.61/",
										"lib/net46/",
										"lib/net4.6/",
										"lib/net40/",
										"lib/net4.0/",
										"lib/net35/",
										"lib/net3.5/",
										"lib/net20/",
										"lib/net2.0/",
										"lib/netstandard2.0/",
									})
								{
									foreach (var dll in dlls)
									{
										if (dll.StartsWith(dllPrefix, StringComparison.InvariantCultureIgnoreCase))
										{
											hintPath = string.Format("{0}.{1}\\{2}", nugetPackageKey.Package, nugetPackageKey.Version, dll.Replace("/", "\\"));
											return true;
										}
									}
								}
							}

							hintPath = string.Empty;
							return false;
						},
						ConvertToPackageReferences = false,
					}).CsProjXml;

					if (HasChanges(csProj, newCsProj))
					{
						System.IO.File.WriteAllText(csProjFileName, newCsProj);

						dirtyFileNames.Add(csProjFileName);
					}
				}
				catch (Exception exception)
				{
					throw new Exception(string.Format("File: {0}", csProjFileName), exception);

				}
			}

		}
	}
}
