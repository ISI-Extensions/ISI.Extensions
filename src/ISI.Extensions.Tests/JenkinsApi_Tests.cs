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
	public class JenkinsApi_Tests
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
		public void GetServiceConfiguration_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "Tristar.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var serviceConfigurationYaml = jenkinsApi.GetServiceConfiguration(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.GetServiceConfigurationRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).ServiceConfigurationYaml;
		}

		[Test]
		public void JobStatus_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "Tristar.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var jenkinsApi = new ISI.Extensions.Jenkins.JenkinsApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var jobId = "Comcast.Business.Retro.Prime.Web.Build";

			var jobStatusStart = jenkinsApi.IsJobEnabled(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.IsJobEnabledRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			jenkinsApi.DisableJob(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.DisableJobRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			});

			var jobStatusMid = jenkinsApi.IsJobEnabled(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.IsJobEnabledRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			jenkinsApi.EnableJob(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.EnableJobRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			});

			var jobStatusEnd = jenkinsApi.IsJobEnabled(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.IsJobEnabledRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,

				JobId = jobId,
			}).IsEnabled;

			var runningJobIds = jenkinsApi.GetRunningJobIds(new ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi.GetRunningJobIdsRequest()
			{
				JenkinsUrl = settings.Jenkins.JenkinsUrl,
				UserName = settings.Jenkins.UserName,
				ApiToken = settings.Jenkins.ApiToken,
			}).JobIds;
		}
	}
}
