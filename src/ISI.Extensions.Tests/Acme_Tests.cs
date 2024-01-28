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
	public class Acme_Tests
	{
		protected readonly Uri AcmeHostUri = new Uri(@"https://acme-staging-v02.api.letsencrypt.org/directory");
		protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.pem");
		protected readonly string AccountJwkAlgorithmKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.JwkAlgorithmKey");
		protected readonly string AccountSerializedJwkFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.SerializedJwk");

		//protected readonly Uri AcmeHostUri = new Uri(@"https://localhost:15633/directory");
		//protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.pem");
		//protected readonly string AccountJwkAlgorithmKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.JwkAlgorithmKey");
		//protected readonly string AccountSerializedJwkFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.SerializedJwk");

		protected string GetAccountPem() => System.IO.File.ReadAllText(AccountPemFullName);
		protected ISI.Extensions.JsonJwt.JwkAlgorithmKey GetAccountJwkAlgorithmKey() => ISI.Extensions.Enum<ISI.Extensions.JsonJwt.JwkAlgorithmKey>.Parse(System.IO.File.ReadAllText(AccountJwkAlgorithmKeyFullName));
		protected string GetAccountSerializedJwk() => System.IO.File.ReadAllText(AccountSerializedJwkFullName);

		protected IServiceProvider ServiceProvider { get; set; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; set; }
		protected ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory JwkBuilderFactory { get; set; }
		protected ISI.Extensions.JsonJwt.JwtEncoder JwtEncoder { get; set; }

		protected ISI.Extensions.Acme.AcmeApi AcmeApi { get; set; }
		protected ISI.Extensions.GoDaddy.DomainsApi DomainsApi { get; set; }

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

					.AddSingleton<ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory>()
					.AddSingleton<ISI.Extensions.JsonJwt.JwtEncoder>()

					.AddSingleton<ISI.Extensions.Acme.AcmeApi>()

				;

			services.AddAllConfigurations(configuration);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);
			
			ServiceProvider.SetServiceLocator();
			
			Logger = ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger>();
			DateTimeStamper = ServiceProvider.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>();
			JsonSerializer = ServiceProvider.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			JwkBuilderFactory = ServiceProvider.GetService<ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory>();
			JwtEncoder = ServiceProvider.GetService<ISI.Extensions.JsonJwt.JwtEncoder>();
			AcmeApi = ServiceProvider.GetService<ISI.Extensions.Acme.AcmeApi>();
			DomainsApi = ServiceProvider.GetService<ISI.Extensions.GoDaddy.DomainsApi>();
		}


		[Test]
		public void CreateNewAcmeAccount()
		{
			var jwkAlgorithmKey = ISI.Extensions.JsonJwt.JwkAlgorithmKey.RS256;

			using (var jwtBuilder = JwkBuilderFactory.GetJwkBuilder(jwkAlgorithmKey))
			{
				var context = AcmeApi.GetAcmeHostContext(new()
				{
					AcmeHostDirectoryUri = AcmeHostUri,
					JwkAlgorithmKey = jwkAlgorithmKey,
					SerializedJwk = jwtBuilder.GetSerializedJwk(),
					Pem = jwtBuilder.GetPrivatePem(),
				}).AcmeHostContext;

				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AccountPemFullName));
				System.IO.File.WriteAllText(AccountPemFullName, context.Pem);
				System.IO.File.WriteAllText(AccountJwkAlgorithmKeyFullName, context.JwkAlgorithmKey.GetAbbreviation());
				System.IO.File.WriteAllText(AccountSerializedJwkFullName, context.SerializedJwk);

				var response = AcmeApi.CreateNewAcmeAccount(new()
				{
					AcmeHostContext = context,
					//AccountName = "localhost",
					Contacts = new[] { "ron.muth@isi-net.com" },
					TermsOfServiceAgreed = true,
				});
			}
		}


		[Test]
		public void CreateNewAcmeOrder()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var pem = GetAccountPem();
			var jwkAlgorithmKey = GetAccountJwkAlgorithmKey();
			var serializedJwk = GetAccountSerializedJwk();

			using (var jwtBuilder = JwkBuilderFactory.GetJwkBuilder(jwkAlgorithmKey, pem))
			{
				var context = AcmeApi.GetAcmeHostContext(new()
				{
					AcmeHostDirectoryUri = AcmeHostUri,
					JwkAlgorithmKey = jwkAlgorithmKey,
					SerializedJwk = serializedJwk,
					Pem = pem,
				}).AcmeHostContext;

				var createNewAcmeOrderResponse = AcmeApi.CreateNewAcmeOrder(new()
				{
					AcmeHostContext = context,
					CertificateIdentifiers = new[]
					{
						new ISI.Extensions.Acme.AcmeOrderCertificateIdentifier()
						{
							CertificateIdentifierType = ISI.Extensions.Acme.AcmeOrderCertificateIdentifierType.Dns,
							CertificateIdentifierValue = "muthmanor.com,*.muthmanor.com",
						}
					},
					//PostRenewalActions = new []
					//{
					//	new ISI.Extensions.Acme.AcmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHook()
					//	{
					//		SetCertificatesUrl = @"https://nginx/upload-certificates",
					//	}
					//}
				});

				var acmeOrderCertificateIdentifier = createNewAcmeOrderResponse.AcmeOrder.CertificateIdentifiers.First(c => c.CertificateIdentifierType == ISI.Extensions.Acme.AcmeOrderCertificateIdentifierType.Dns);


				var dnsRecords = DomainsApi.GetDnsRecords(new()
				{
					ApiKey = settings.GetValue("GoDaddy.ApiKey"),
					ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
					DomainName = "MuthManor.com",
				}).DnsRecords;

				var dnsRecord = new ISI.Extensions.Dns.DnsRecord()
				{
					Data = acmeOrderCertificateIdentifier.CertificateIdentifierValue,
					Name = "_acme-challenge",
					//Port = source.Port,
					//Priority = source.Priority,
					//Protocol = source.Protocol,
					//Service = source.Service,
					//Ttl = 3600,
					RecordType = ISI.Extensions.Dns.RecordType.TXT,
					//Weight = source.Weight,
				};

				var xxx = DomainsApi.SetDnsRecords(new()
				{
					ApiKey = settings.GetValue("GoDaddy.ApiKey"),
					ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
					DomainName = "MuthManor.com",
					DnsRecords = new[] { dnsRecord },
				});


			}
		}
	}
}