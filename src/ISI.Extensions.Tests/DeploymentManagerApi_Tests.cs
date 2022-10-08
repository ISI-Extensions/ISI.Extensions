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

using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using ISI.Extensions.Scm.Extensions;
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
	public class DeploymentManagerApi_Tests
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
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.TextWriterLogger(TestContext.Progress))

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
		public void UpdateServicesManager_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var deploymentManagerApi = new ISI.Extensions.Scm.DeploymentManagerApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			deploymentManagerApi.UpdateServicesManager(new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.UpdateServicesManagerRequest()
			{
				//ServicesManagerUrl = "http://localhost:14258/",
				//Password = "87BEF140-045B-42D7-AB87-3E59F162BC39",
				ServicesManagerUrl = settings.GetValue("PRODUCTION-GOGS01-DeployManager-Url"),
				Password = settings.GetValue("PRODUCTION-GOGS01-DeployManager-Password"),
			});
		}

		[Test]
		public void DeployArtifact_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var scmApi = new ISI.Extensions.Scm.ScmApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));
			var authenticationToken = scmApi.GetAuthenticationToken(new ISI.Extensions.Scm.DataTransferObjects.ScmApi.GetAuthenticationTokenRequest()
			{
				ScmManagementUrl = settings.Scm.WebServiceUrl,
				UserName = settings.ActiveDirectory.UserName,
				Password = settings.ActiveDirectory.Password,
			}).AuthenticationToken;

			var deploymentManagerApi = new ISI.Extensions.Scm.DeploymentManagerApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var artifactName = "ISI.SCM.Scheduler.WindowsService";
			var artifactFileStoreUuid = new System.Guid("bb38fd6e-8659-4a6b-a7bd-2d88c71368b9");
			var artifactVersionFileStoreUuid = new System.Guid("864c9431-7ad1-426d-be8e-4f845a6cfd4c");
			var artifactDateTimeStampVersionUrl = string.Format("https://www.isi-net.com/file-store/download/{0:D}/{1}.Current.DateTimeStamp.Version.txt", artifactVersionFileStoreUuid, artifactName); 
			var artifactDownloadUrl = string.Format("https://www.isi-net.com/file-store/download/{0:D}/{1}.zip", artifactFileStoreUuid, artifactName); 

			deploymentManagerApi.DeployArtifact(new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployArtifactRequest()
			{
				ServicesManagerUrl = "http://localhost:14258/",
				Password = "87BEF140-045B-42D7-AB87-3E59F162BC39",
				//ServicesManagerUrl = settings.GetValue("PRODUCTION-GOGS01-DeployManager-Url"),
				//Password = settings.GetValue("PRODUCTION-GOGS01-DeployManager-Password"),
				AuthenticationToken = authenticationToken,

				BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
				ArtifactName = artifactName,
				ArtifactDateTimeStampVersionUrl = artifactDateTimeStampVersionUrl,
				ArtifactDownloadUrl = artifactDownloadUrl,
				//ToDateTimeStamp = dateTimeStampVersion.Value,
				ToEnvironment = "Production",
				ConfigurationKey = "Production",
				Components = new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.IDeployComponent[]
				{
					new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWindowsService()
					{
						PackageFolder = "ISI/ISI.SCM.Scheduler.WindowsService",
						DeployToSubfolder = "ISI.SCM.Scheduler.WindowsService",
						WindowsServiceExe = "ISI.SCM.Scheduler.WindowsService.exe",
					},
				},
			});
		}

		[Test]
		public void PrePushArtifact_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var scmApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.ScmApi>();
			var buildArtifactApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.BuildArtifactApi>();
			var deploymentManagerApi = new ISI.Extensions.Scm.DeploymentManagerApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var artifactName = "ISI.WindowsService";

			var authenticationToken = scmApi.GetAuthenticationToken(new ISI.Extensions.Scm.DataTransferObjects.ScmApi.GetAuthenticationTokenRequest()
			{
				ScmManagementUrl = settings.Scm.WebServiceUrl,
				UserName = settings.ActiveDirectory.UserName,
				Password = settings.ActiveDirectory.Password,
			}).AuthenticationToken;

			var dateTimeStampVersion = buildArtifactApi.GetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Extensions.Scm.DataTransferObjects.BuildArtifactApi.GetBuildArtifactEnvironmentDateTimeStampVersionRequest()
			{
				BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
				AuthenticationToken = authenticationToken,
				ArtifactName = artifactName,
				Environment = "UAT",
			}).DateTimeStampVersion;

			deploymentManagerApi.DeployArtifact(new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployArtifactRequest()
			{
				ServicesManagerUrl = "http://localhost:14258/",
				Password = "87BEF140-045B-42D7-AB87-3E59F162BC39",
				//ServicesManagerUrl = settings.GetValue("PRODUCTION-SQL01-DeployManager-Url"),
				//Password = settings.GetValue("PRODUCTION-SQL01-DeployManager-Password"),

				AuthenticationToken = authenticationToken,

				BuildArtifactManagementUrl = settings.Scm.WebServiceUrl,
				ArtifactName = artifactName,
				ToDateTimeStamp = dateTimeStampVersion.Value,
				ToEnvironment = "Production",
				ConfigurationKey = "Production",

				Components = new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.IDeployComponent[]
				{
					new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWindowsService()
					{
						PackageFolder = "ISI\\ISI.Api.WindowsService",
						DeployToSubfolder = "ISI.Api.WindowsService",
						WindowsServiceExe = "ISI.Api.WindowsService.exe",
					},
				},
			});
		}
	}
}
