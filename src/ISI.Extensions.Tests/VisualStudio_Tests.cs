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
 
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class VisualStudio_Tests
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configurationRoot = configurationBuilder.Build().ApplyConfigurationValueReaders();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configurationRoot);

			services.AddAllConfigurations(configurationRoot)

				//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
				.AddLogging(builder => builder
						.AddConsole()
				//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
				)
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.TextWriterLogger(TestContext.Progress))

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddConfigurationRegistrations(configurationRoot)
				.ProcessServiceRegistrars(configurationRoot)
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void UpdateConfigFiles_Test()
		{
			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();
			var sourceControlClientApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.SourceControlClientApi>();

			var solutionFullNames = new List<string>();
			solutionFullNames.AddRange(System.IO.Directory.EnumerateDirectories(@"G:\XXXXXXXXXXXXXXXXXXXXX"));

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => solutionApi.GetSolutionDetails(new()
			{
				Solution = solution,
			}).SolutionDetails, ISI.Extensions.Extensions.NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpgradeNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				using (solutionApi.GetSolutionLock(new()
				{
					SolutionFullName = solutionDetails.SolutionFullName,
				}).Lock)
				{
					logger.Log(LogLevel.Information, solutionDetails.SolutionName);

					var dirtyFileNames = new HashSet<string>();

					if (!sourceControlClientApi.UpdateWorkingCopy(new()
					{
						FullName = solutionDetails.RootSourceDirectory,
						IncludeExternals = true,
					}).Success)
					{
						var exception = new Exception(string.Format("Error updating \"{0}\"", solutionDetails.RootSourceDirectory));
						logger.LogError(exception.Message);
						throw exception;
					}

					var configFullNames = new List<string>();
					configFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.config", System.IO.SearchOption.AllDirectories));

					foreach (var configFullName in configFullNames)
					{
						var content = System.IO.File.ReadAllText(configFullName);

						if (content.IndexOf("XXXXXXXXXXXXXXXXXXXXX", StringComparison.CurrentCulture) >= 0)
						{
							content = content.Replace("XXXXXXXXXXXXXXXXXXXXX", "XXXXXXXXXXXXXXXXXXXXX.XXXXXXXXXXXXXXXXXXXXX");

							System.IO.File.WriteAllText(configFullName, content);

							dirtyFileNames.Add(configFullName);
						}
					}

					if (dirtyFileNames.Any())
					{
						var commitLog = new StringBuilder();

						if (!sourceControlClientApi.Commit(new()
						{
							FullNames = dirtyFileNames,
							LogMessage = "update connection string name XXXXXXXXXXXXXXXXXXXXX to XXXXXXXXXXXXXXXXXXXXX.XXXXXXXXXXXXXXXXXXXXX",
							AddToLog = (logEntryLevel, description) => commitLog.AppendLine(description),
						}).Success)
						{
							var exception = new Exception(string.Format("Error committing \"{0}\"", solutionDetails.RootSourceDirectory));
							logger.LogError(exception.Message);
							throw exception;
						}
					}
				}
			}
		}

		[Test]
		public void GetUsedVisualStudioPackages_Test()
		{
			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.Nuget.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var solutionFullNames = new List<string>();
			solutionFullNames.AddRange(System.IO.File.ReadAllLines(@"S:\Central.SolutionFullNames.txt"));

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => solutionApi.GetSolutionDetails(new()
			{
				Solution = solution,
			}).SolutionDetails, ISI.Extensions.Extensions.NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			var nugetPackageKeys = new Nuget.NugetPackageKeyDictionary();

			foreach (var solutionDetails in solutionDetailsSet)
			{
				logger.Log(LogLevel.Information, solutionDetails.SolutionName);

				foreach (var projectDetails in solutionDetails.ProjectDetailsSet)
				{
					nugetPackageKeys.Merge(nugetApi.ExtractProjectNugetPackageDependenciesFromCsProj(new()
					{
						BuildTargetFrameworks = false,
						CsProjFullName = projectDetails.ProjectFullName,
						DoNotCheckForDifferentVersions = true,
					}).NugetPackageKeys.Where(nugetPackageKey => nugetPackageKey.Package.StartsWith("ISI.")));

					var packagesConfigFullName = System.IO.Path.Combine(projectDetails.ProjectDirectory, "packages.config");
					if (System.IO.File.Exists(packagesConfigFullName))
					{
						nugetPackageKeys.Merge(nugetApi.ExtractProjectNugetPackageDependenciesFromPackagesConfig(new()
						{
							PackagesConfigFullName = packagesConfigFullName,
						}).NugetPackageKeys.Where(nugetPackageKey => nugetPackageKey.Package.StartsWith("ISI.")));
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();

			foreach (var nugetPackageKey in nugetPackageKeys.Where(nugetPackageKey => nugetPackageKey.Package.StartsWith("ISI.")).OrderBy(nugetPackageKey => nugetPackageKey.Package, StringComparer.InvariantCultureIgnoreCase).ThenBy(nugetPackageKey => nugetPackageKey.Version, StringComparer.InvariantCultureIgnoreCase))
			{
				Console.WriteLine($"{nugetPackageKey.Package} {nugetPackageKey.Version}");
			}
		}
	}
}
