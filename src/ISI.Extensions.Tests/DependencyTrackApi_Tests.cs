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
	public class DependencyTrackApi_Tests
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
		public void GenerateCycloneDX_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var sbomApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Sbom.SbomApi>();

			var xxx = sbomApi.GenerateCycloneDX(new()
			{
				FullName = @"F:\ISI\Internal Projects\ISI.CertificateRegistry.ServiceApplication\src\ISI.Services.CertificateRegistry\ISI.Services.CertificateRegistry.csproj",
				//Framework = source.Framework,
				//Runtime = source.Runtime,
				//OutputDirectory = source.OutputDirectory,
				//OutputFilename = source.OutputFilename,
				//OutputJson = source.OutputJson,
				//ExcludeDependencies = source.ExcludeDependencies.ToNullCheckedArray(Convert),
				//ExcludeDevelopmentDependencies = source.ExcludeDevelopmentDependencies,
				//ExcludeTestProjects = source.ExcludeTestProjects,
				//AlternativeNugetUrl = source.AlternativeNugetUrl,
				//AlternativeNugetUserName = source.AlternativeNugetUserName,
				//AlternativeNugetPasswordApiKey = source.AlternativeNugetPasswordApiKey,
				//AlternativeNugetPasswordIsClearText = source.AlternativeNugetPasswordIsClearText,
				//Recursive = source.Recursive,
				//OmitSerialNumber = source.OmitSerialNumber,
				//GitHubUserName = source.GitHubUserName,
				//GitHubToken = source.GitHubToken,
				//GitHubBearerToken = source.GitHubBearerToken,
				//GitHubEnableLicenses = source.GitHubEnableLicenses,
				//DisablePackageRestore = source.DisablePackageRestore,
				//DisableHashComputation = source.DisableHashComputation,
				//DotnetCommandTimeout = source.DotnetCommandTimeout,
				//BaseIntermediateOutputPath = source.BaseIntermediateOutputPath,
				//ImportMetadataPath = source.ImportMetadataPath,
				//IncludeProjectPeferences = source.IncludeProjectPeferences,
				SetName = "ISI.Services.CertificateRegistry",
				SetVersion = new(10, 0, 0, 0),
				SetComponentType = ISI.Extensions.Sbom.ComponentType.Library,
				//SetNugetPurl = source.SetNugetPurl,
			});
		}

		[Test]
		public void UploadCycloneDX_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var dependencyTrackApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Sbom.DependencyTrackApi>();
			var sbomApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Sbom.SbomApi>();

			var xxx = sbomApi.GenerateCycloneDX(new()
			{
				FullName = @"F:\ISI\Internal Projects\ISI.CertificateRegistry.ServiceApplication\src\ISI.Services.CertificateRegistry\ISI.Services.CertificateRegistry.csproj",
				//Framework = source.Framework,
				//Runtime = source.Runtime,
				//OutputDirectory = source.OutputDirectory,
				//OutputFilename = source.OutputFilename,
				//OutputJson = source.OutputJson,
				//ExcludeDependencies = source.ExcludeDependencies.ToNullCheckedArray(Convert),
				//ExcludeDevelopmentDependencies = source.ExcludeDevelopmentDependencies,
				//ExcludeTestProjects = source.ExcludeTestProjects,
				//AlternativeNugetUrl = source.AlternativeNugetUrl,
				//AlternativeNugetUserName = source.AlternativeNugetUserName,
				//AlternativeNugetPasswordApiKey = source.AlternativeNugetPasswordApiKey,
				//AlternativeNugetPasswordIsClearText = source.AlternativeNugetPasswordIsClearText,
				//Recursive = source.Recursive,
				//OmitSerialNumber = source.OmitSerialNumber,
				//GitHubUserName = source.GitHubUserName,
				//GitHubToken = source.GitHubToken,
				//GitHubBearerToken = source.GitHubBearerToken,
				//GitHubEnableLicenses = source.GitHubEnableLicenses,
				//DisablePackageRestore = source.DisablePackageRestore,
				//DisableHashComputation = source.DisableHashComputation,
				//DotnetCommandTimeout = source.DotnetCommandTimeout,
				//BaseIntermediateOutputPath = source.BaseIntermediateOutputPath,
				//ImportMetadataPath = source.ImportMetadataPath,
				//IncludeProjectPeferences = source.IncludeProjectPeferences,
				SetName = "ISI.Services.CertificateRegistry",
				SetVersion = new(10, 0, 0, 0),
				SetComponentType = ISI.Extensions.Sbom.ComponentType.Library,
				//SetNugetPurl = source.SetNugetPurl,
			});

			var yyy = dependencyTrackApi.UploadCycloneDX(new ISI.Extensions.Sbom.DataTransferObjects.DependencyTrackApi.UploadCycloneDXWithProjectNameAndParentProjectNameAndCycloneDXFullNameRequest()
			{
				DependencyTrackApiUrl = settings.DependencyTrack.ApiUrl,
				DependencyTrackApiKey = settings.DependencyTrack.ApiKey,
				ProjectName = "ISI.Services.CertificateRegistry",
//ProjectTags = source.ProjectTags.ToNullCheckedArray(),
				AutoCreate = true,
				ProjectVersion = new(10, 0, 0, 0),
				IsLatestProjectVersion = true,
//ParentProjectName = source.ParentProjectName,
//ParentProjectVersion = source.ParentProjectVersion,
				CycloneDXFullName = @"F:\ISI\Internal Projects\ISI.Extensions\src\ISI.Extensions.Tests\bin\Debug\net9.0\bom.xml",
			});
		}
	}
}
