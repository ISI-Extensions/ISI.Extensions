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
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class GoDaddyApis_Tests
	{
		protected IServiceProvider ServiceProvider { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			configurationBuilder.AddJsonFile("appsettings.json", optional: false);
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
				.AddSingleton<ISI.Extensions.Ipify.IpifyApi>()
				.AddSingleton<ISI.Extensions.GoDaddy.DomainsApi>()

				.AddConfigurationRegistrations(configurationRoot)
				.ProcessServiceRegistrars(configurationRoot)
				;

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			ServiceProvider.SetServiceLocator();

			foreach (var keyValuePair in configurationRoot.AsEnumerable())
			{
				TestContext.Progress.WriteLine($"  Config \"{keyValuePair.Key}\" => \"{keyValuePair.Value}\"");
			}
		}

		[Test]
		public void DDNS_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var ipifyApi = ServiceProvider.GetService<ISI.Extensions.Ipify.IpifyApi>();
			var domainsApi = ServiceProvider.GetService<ISI.Extensions.GoDaddy.DomainsApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var externalIpAddress = ipifyApi.GetExternalIPv4().IpAddress;

				var dnsRecords = domainsApi.GetDnsRecords(new()
				{
					ApiKey = settings.GetValue("GoDaddy.ApiKey"),
					ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
					Domain = "MuthFamily.com",
				}).DnsRecords;

				var dnsRecord = new ISI.Extensions.Dns.DnsRecord()
				{
					Data = externalIpAddress,
					Name = "@",
					//Port = source.Port,
					//Priority = source.Priority,
					//Protocol = source.Protocol,
					//Service = source.Service,
					//Ttl = 3600,
					RecordType = ISI.Extensions.Dns.RecordType.AddressRecord,
					//Weight = source.Weight,
				};

				var xxx = domainsApi.SetDnsRecords(new()
				{
					ApiKey = settings.GetValue("GoDaddy.ApiKey"),
					ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
					Domain = "MuthFamily.com",
					DnsRecords = [dnsRecord],
				});
			}
		}

		[Test]
		public void SSL_Verify_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var domainsApi = ServiceProvider.GetService<ISI.Extensions.GoDaddy.DomainsApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var dnsRecord = new ISI.Extensions.Dns.DnsRecord()
				{
					Data = "xxxxxxxxxxxxxxxxxxxxxxx",
					Name = "xxxxxxxxxxxxxxxxxxxxxxxxx",
					//Port = source.Port,
					//Priority = source.Priority,
					//Protocol = source.Protocol,
					//Service = source.Service,
					//Ttl = 3600,
					RecordType = ISI.Extensions.Dns.RecordType.CanonicalNameRecord,
					//Weight = source.Weight,
				};

				var xxx = domainsApi.SetDnsRecords(new()
				{
					ApiKey = settings.GetValue("GoDaddy.ApiKey"),
					ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
					Domain = "westriversystems.com",
					DnsRecords = [dnsRecord],
				});
			}
		}
	}
}
