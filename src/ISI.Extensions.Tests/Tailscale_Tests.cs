﻿#region Copyright & License
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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using NUnit.Framework;
using System.Runtime.Serialization;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class Tailscale_Tests
	{
		protected IServiceProvider ServiceProvider { get; set; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; set; }

		protected ISI.Extensions.Tailscale.TailscaleApi TailscaleApi { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
					.AddOptions()
					.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration)

					.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.ConsoleLogger())

					.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

					.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
					.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

					.AddSingleton<ISI.Extensions.Tailscale.TailscaleApi>()
				;

			services.AddAllConfigurations(configuration);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			ServiceProvider.SetServiceLocator();

			Logger = ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger>();
			DateTimeStamper = ServiceProvider.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>();
			JsonSerializer = ServiceProvider.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			TailscaleApi = ServiceProvider.GetService<ISI.Extensions.Tailscale.TailscaleApi>();
		}


		[Test]
		public void CreateAuthKey_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName);

			var createAuthKeyResponse = TailscaleApi.CreateAuthKey(new()
			{
				TailscaleApiKey = settings.GetValue("Tailscale-ronmuth-apikey"),
				Tailnet = settings.GetValue("Tailscale-ronmuth-tailnet"),
				Description = "test",
				Ephemeral = false,
				Preauthorized = true,
				Reusable = true,
			});
		}

		[Test]
		public void DateTimeStampVersion_HasValue_Test()
		{
			var dateTimeStampVersion = new ISI.Extensions.Scm.DateTimeStampVersion();

			Assert.That(!dateTimeStampVersion?.HasValue ?? false);
		}
	}
}
