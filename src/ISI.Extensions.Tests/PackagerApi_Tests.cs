#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;
using ISI.Extensions.Scm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class PackagerApi_Tests
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
		public void jan1st2000_Test()
		{
			var jan1st2000 = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			var xxx = jan1st2000.AddDays(8447);

		}

		[Test]
		public void PackageComponents_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var logger = new ISI.Extensions.TextWriterLogger(TestContext.Progress);
			var serialization = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.Nuget.Configuration(), logger, new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());
			var findToolsApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.VsWhereApi>();
			var msBuildApi = new ISI.Extensions.VisualStudio.MSBuildApi(logger, findToolsApi);
			var codeGenerationApi = new ISI.Extensions.VisualStudio.CodeGenerationApi(logger);
			var xmlTransformApi = new ISI.Extensions.VisualStudio.XmlTransformApi(logger);
			var packagerApi = new ISI.Extensions.VisualStudio.PackagerApi(logger, nugetApi, msBuildApi, codeGenerationApi, xmlTransformApi);
			var buildScriptApi = new ISI.Extensions.Scm.BuildScriptApi(logger);
			var sourceControlClientApi = new SourceControlClientApi(logger);
			var projectApi = new ISI.Extensions.VisualStudio.ProjectApi(logger);
			var solutionApi = new ISI.Extensions.VisualStudio.SolutionApi(new ISI.Extensions.VisualStudio.Configuration(), logger, serialization, buildScriptApi, sourceControlClientApi, msBuildApi, codeGenerationApi, projectApi, nugetApi);
			var sbomApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Sbom.SbomApi>();

			var configuration = "Release";

			var utcDateTime = DateTime.UtcNow;

			var buildRevision = solutionApi.GetBuildRevision(new()
			{
				UtcDateTime = utcDateTime,
			}).BuildRevision;

			var buildDateTimeStamp = string.Format("{0:yyyyMMdd.HHmmss}", utcDateTime);

			var solutionFullName = @"F:\ISI\Internal Projects\ISI.WebApplication\src\ISI.WebApplication.sln";
			var rootProjectFullName = @"F:\ISI\Internal Projects\ISI.WebApplication\src\ISI.WebApplication\ISI.WebApplication.csproj";
			var rootAssemblyVersionKey = "ISI.WebApplication";
			var artifactName = "ISI.WebApplication";

			//solutionApi.CleanSolution(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.CleanSolutionRequest()
			//{
			//	Solution = solutionFullName,
			//});

			//nugetApi.RestoreVisualStudioPackages(new ISI.Extensions.VisualStudio.DataTransferObjects.VisualStudioApi.RestoreVisualStudioPackagesRequest()
			//{
			//	MSBuildExe = msBuildApi.GetMSBuildExeFullName(new ISI.Extensions.VisualStudio.DataTransferObjects.MSBuildApi.GetMSBuildExeFullNameRequest()).MSBuildExeFullName,
			//	SolutionDirectory = System.IO.Path.GetDirectoryName(solutionFullName),
			//});

			var assemblyVersions = solutionApi.GetAssemblyVersionFiles(new()
			{
				Solution = solutionFullName,
				RootAssemblyVersionKey = rootAssemblyVersionKey,
				BuildRevision = buildRevision,
			}).AssemblyVersionFiles;


			var buildArtifactZipFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(solutionFullName), @$"..\Publish\{artifactName}.{buildDateTimeStamp}.zip");

			var bateTimeStampVersion = new DateTimeStampVersion(buildDateTimeStamp, assemblyVersions[rootAssemblyVersionKey].AssemblyVersion);

			packagerApi.PackageComponents(new()
			{
				BuildPlatform = ISI.Extensions.VisualStudio.MSBuildPlatform.Automatic,
				BuildVersion = ISI.Extensions.VisualStudio.MSBuildVersion.Latest,
				Configuration = configuration,
				AssemblyVersionFiles = assemblyVersions,
				SubDirectory = "ISI",
				PackageComponents =
				[
					//new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.PackageComponentConsoleApplication()
					//{
					//	ProjectFullName = @"F:\ISI\Clients\ISI\ISI.DocumentBuilder.Portal.WebApplication\src\ISI.Services\ISI.Services.DocumentBuilder.MigrationTool\ISI.Services.DocumentBuilder.MigrationTool.csproj",
					//	IconFileName = @"ISI.ico",
					//},
					new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.PackageComponentWebSite()
					{
						ProjectFullName = rootProjectFullName,
						IconFileName = @"ISI.ico",
						AfterBuildPackageComponent = context =>
						{
							var packageName = System.IO.Path.GetFileNameWithoutExtension(context.ProjectFullName);
							var packageSourceDirectory = System.IO.Path.GetDirectoryName(context.ProjectFullName);

							sbomApi.GeneratePackageSBom(new()
							{
								PackageComponentDirectory = context.PackageComponentDirectory,
								PackageSourceDirectory = packageSourceDirectory,
								PackageName = packageName,
								PackageVersion = assemblyVersions[rootAssemblyVersionKey].AssemblyVersion,
								PackageAuthor = "ISI",
								PackageNamespace = new Uri(@"https://www.isi-net.com/packages"),
							});


							//var arguments = new List<string>();

							//arguments.Add("generate");
							//arguments.Add($"-b \"{context.PackageComponentDirectory}\"");
							//arguments.Add($"-bc \"{packageSourceDirectory}\"");
							//arguments.Add($"-pn \"{packageName}\"");
							//arguments.Add($"-pv \"{assemblyVersions[rootAssemblyVersionKey].AssemblyVersion}\"");
							//arguments.Add($"-ps \"ISI\"");
							//arguments.Add("-nsb \"https:\\www.isi-net.com\\");

							//var xx = ISI.Extensions.Process.WaitForProcessResponse(new Process.ProcessRequest()
							//{
							//	ProcessExeFullName = "sbom-tool",
							//	Arguments = arguments,
							//});
						}
					}
				],
				PackageFullName = buildArtifactZipFileName,
				PackageBuildDateTimeStampVersion = bateTimeStampVersion,
			});


			//packagerApi.PackageComponents(new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.PackageComponentsRequest()
			//{
			//	Configuration = "Release",
			//	//BuildPlatform = ISI.Extensions.VisualStudio.MSBuildPlatform.x86,
			//	//PlatformTarget = ISI.Extensions.VisualStudio.BuildPlatformTarget.x86,
			//	SubDirectory = "ISI",
			//	PackageComponents = new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.IPackageComponent[]
			//	{
			//		new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.PackageComponentWindowsApplication()
			//		{
			//			ProjectFullName = "F:\\ISI\\WindowsApplication\\src\\Installer\\Installer.csproj",
			//			IconFullName = "F:\\ISI\\Lantern.ico",
			//			DoNotXmlTransformConfigs = true,
			//		},
			//		new ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi.PackageComponentWindowsApplication()
			//		{
			//			ProjectFullName = "F:\\ISI\\WindowsApplication\\src\\Interface\\Interface.csproj",
			//			IconFullName = "F:\\ISI\\Lantern.ico",
			//			DoNotXmlTransformConfigs = true,
			//		},
			//	},
			//	PackageFullName = "F:\\ISI\\WindowsApplication\\xxxx.zip",
			//	PackageVersion = "9.0.8049.6991",
			//	PackageBuildDateTimeStamp = string.Format("{0:yyyyMMdd.HHmmss}", DateTime.UtcNow),
			//});
		}
	}
}
