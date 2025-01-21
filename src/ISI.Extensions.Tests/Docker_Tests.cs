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
	public class Docker_Tests
	{
		public IServiceProvider ServiceProvider { get; set; }

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

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			ServiceProvider.SetServiceLocator();
		}

		[Test]
		public void Login_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.Login(new()
			{
				Host = settings.DockerRegistry.DomainName,
				UserName = settings.DockerRegistry.UserName,
				Password = settings.DockerRegistry.Password,
			});
		}

		[Test]
		public void InspectImageManifest_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.InspectImageManifest(new()
			{
				ContainerRegistry = "mcr.microsoft.com",
				ContainerRepository = "dotnet/aspnet",
				ContainerImageTag = "9.0-alpine",
			});
		}

		[Test]
		public void ListContexts_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.ListContexts(new());
		}

		[Test]
		public void GetServerApiVersion_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.GetServerApiVersion(new()
			{
				Context = "isinydocker01",
			});
		}

		[Test]
		public void ComposeAll_Test()
		{
			ComposeDown_Test();
			ComposePull_Test();
			ComposeUp_Test();
		}

		[Test]
		public void ComposeDown_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.ComposeDown(new()
			{
				Context = "isinydocker01",
				ComposeDirectory = @"F:\ISI\Internal Projects\ISI.Docker.Recipes\isinydocker01\scmmanager",
				RemoveVolumes = true,
				EnvironmentFileFullNames =
				[
					@"S:\ISI.Production.env"
				],
				OnComposeDownStart = tryGetEnvironmentValue =>
				{
					if (tryGetEnvironmentValue("MESSAGE_BUS_CONNECTION_STRING", out var value))
					{
						Console.WriteLine(value);
					}
				},
				AddToLog = (level, description) => Console.WriteLine(description),
			});
		}

		[Test]
		public void ComposePull_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.ComposePull(new()
			{
				Context = "isinydocker01",
				ComposeDirectory = @"F:\ISI\Internal Projects\ISI.Docker.Recipes\isinydocker01\tailscale",
				AddToLog = (level, description) => Console.WriteLine(description),
			});
		}

		[Test]
		public void ComposeUp_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.ComposeUp(new()
			{
				Context = "isinydocker01",
				ComposeDirectory = @"F:\ISI\Internal Projects\ISI.Docker.Recipes\isinydocker01\scmmanager",
				AddToLog = (level, description) => Console.WriteLine(description),
			});
		}

		[Test]
		public void ReloadNginx_Test()
		{
			var dockerApi = ServiceProvider.GetService<ISI.Extensions.Docker.DockerApi>();

			var xxx = dockerApi.ReloadNginx(new()
			{
				Host = "ssh://isinydocker01.isi-net.com",
				Container = "isi-nginx-reverse-proxy",
				AddToLog = (level, description) => Console.WriteLine(description),
			});
		}
	}
}