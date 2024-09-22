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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class JenkinsApi_Tests
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
		public void QuietDown_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			jenkinsApi.QuietDown(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
				Reason = "Why Not",
			});
		}

		[Test]
		public void GetJobConfigXml_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jobId = "Backup.JenkinsConfigs";

			var xxx = jenkinsApi.GetJobConfigXml(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
				JobId = jobId,
			});
		}

		[Test]
		public void GetWorkspaceDetails_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jobId = "Backup.JenkinsConfigs";

			var xxx = jenkinsApi.GetWorkspaceDetails(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
				JobId = jobId,
			});
		}

		[Test]
		public void AddNugetInstall_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jobIds = jenkinsApi.GetJobIds(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).JobIds;

			foreach (var jobId in jobIds)
			{
				var configXml = jenkinsApi.GetJobConfigXml(new()
				{
					JenkinsUrl = settings.Jenkins.JenkinsUrl,
					UserName = settings.Jenkins.UserName,
					ApiToken = settings.Jenkins.ApiToken,
					JobId = jobId,
				}).ConfigXml;

				if ((configXml.IndexOf("<command>dotnet cake", StringComparison.CurrentCulture) > 0) && (configXml.IndexOf("nuget install isi.cake.addin") < 0))
				{
					configXml = configXml.Replace("<command>dotnet cake", "<command>nuget install isi.cake.addin -verbosity quiet -OutputDirectory tools\ndotnet cake");

					jenkinsApi.SetJobConfigXml(new()
					{
						JenkinsUrl = settings.Jenkins.JenkinsUrl,
						UserName = settings.Jenkins.UserName,
						ApiToken = settings.Jenkins.ApiToken,
						JobId = jobId,
						ConfigXml = configXml,
					});

					TestContext.Progress.WriteLine(jobId);
				}

				if ((configXml.IndexOf("<command>cd src", StringComparison.CurrentCulture) > 0) && (configXml.IndexOf("nuget install isi.cake.addin") < 0))
				{
					configXml = configXml.Replace("<command>cd src", "<command>cd src\nnuget install isi.cake.addin -verbosity quiet -OutputDirectory tools");

					jenkinsApi.SetJobConfigXml(new()
					{
						JenkinsUrl = settings.Jenkins.JenkinsUrl,
						UserName = settings.Jenkins.UserName,
						ApiToken = settings.Jenkins.ApiToken,
						JobId = jobId,
						ConfigXml = configXml,
					});

					TestContext.Progress.WriteLine(jobId);
				}
			}
		}

		[Test]
		public void SetJobConfigXml_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jenkinsConfigFullName = @"XXXXXXXXXXXXXXXXXXXX.jenkinsConfig";

			var jobId = System.IO.Path.GetFileNameWithoutExtension(jenkinsConfigFullName);

			var content = System.IO.File.ReadAllText(jenkinsConfigFullName);

			jenkinsApi.SetJobConfigXml(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
				JobId = jobId,
				ConfigXml = content,
			});
		}

		[Test]
		public void GetServiceConfiguration_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var serviceConfigurationYaml = jenkinsApi.GetServiceConfiguration(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).ServiceConfigurationYaml;
		}

		[Test]
		public void Delete_QA_Jobs_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jobIds = jenkinsApi.GetJobIds(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).JobIds.ToNullCheckedHashSet(NullCheckCollectionResult.Empty);

			jobIds.RemoveWhere(jobId => !jobId.Contains(".QA.", StringComparison.InvariantCulture));

			foreach (var jobId in jobIds)
			{
				jenkinsApi.DeleteJob(new()
				{
					JenkinsUrl = settings.Jenkins.JenkinsUrl,
					UserName = settings.Jenkins.UserName,
					ApiToken = settings.Jenkins.ApiToken,
					JobId = jobId,
				});
			}
		}

		[Test]
		public void JobStatus_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.Jenkins.Configuration(), new ISI.Extensions.TextWriterLogger(TestContext.Progress), new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer());

			var jobId = "ISI.Web.Build";

			var jobStatusStart = jenkinsApi.IsJobEnabled(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			jenkinsApi.DisableJob(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			});

			var jobStatusMid = jenkinsApi.IsJobEnabled(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			jenkinsApi.EnableJob(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			});

			var jobStatusEnd = jenkinsApi.IsJobEnabled(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			var runningJobIds = jenkinsApi.GetRunningJobIds(new()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).JobIds;
		}
	}
}
