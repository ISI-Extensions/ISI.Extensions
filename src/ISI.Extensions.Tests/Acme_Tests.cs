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
		protected readonly Uri HostDirectoryUri = new Uri(@"https://acme-staging-v02.api.letsencrypt.org/directory");
		protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.pem");
		protected readonly string AccountSerializedJsonWebKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.SerializedJsonWebKey");
		protected readonly string AccountKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.key");
		protected readonly string CertificateKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.certificate.key");
		protected readonly string CertificateFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.certificate.crt");

		//protected readonly Uri AcmeHostUri = new Uri(@"https://localhost:15633/directory");
		//protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.pem");
		//protected readonly string AccountSerializedJsonWebKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.SerializedJsonWebKey");
		//protected readonly string AccountKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.key");

		protected string GetAccountPem() => System.IO.File.ReadAllText(AccountPemFullName);
		protected string GetAccountSerializedJsonWebKey() => System.IO.File.ReadAllText(AccountSerializedJsonWebKeyFullName);
		protected string GetAccountKey() => System.IO.File.ReadAllText(AccountKeyFullName);

		protected IServiceProvider ServiceProvider { get; set; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; set; }

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

					.AddSingleton<ISI.Extensions.Acme.AcmeApi>()

				;

			services.AddAllConfigurations(configuration);

			ServiceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			ServiceProvider.SetServiceLocator();

			Logger = ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger>();
			DateTimeStamper = ServiceProvider.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>();
			JsonSerializer = ServiceProvider.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			AcmeApi = ServiceProvider.GetService<ISI.Extensions.Acme.AcmeApi>();
			DomainsApi = ServiceProvider.GetService<ISI.Extensions.GoDaddy.DomainsApi>();
		}


		[Test]
		public void CreateNewAccount_Test()
		{
			var context = AcmeApi.CreateNewHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
			}).HostContext;

			var createNewAccountResponse = AcmeApi.CreateNewAccount(new()
			{
				HostContext = context,
				//AccountName = "localhost",
				Contacts = new[] { "ron.muth@isi-net.com" },
				TermsOfServiceAgreed = true,
			});

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AccountPemFullName));
			System.IO.File.WriteAllText(AccountPemFullName, context.Pem);
			System.IO.File.WriteAllText(AccountSerializedJsonWebKeyFullName, context.SerializedJsonWebKey);
			System.IO.File.WriteAllText(AccountKeyFullName, createNewAccountResponse.Account.AccountKey);
		}

		[Test]
		public void CreateNewOrder_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var pem = GetAccountPem();
			var serializedJsonWebKey = GetAccountSerializedJsonWebKey();
			var accountKey = GetAccountKey();

			var domainName = "www.muthmanor.com";

			var context = AcmeApi.GetHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
				SerializedJsonWebKey = serializedJsonWebKey,
				Pem = pem,
				AccountKey = accountKey,
			}).HostContext;

			var createNewOrderResponse = AcmeApi.CreateNewOrder(new()
			{
				HostContext = context,
				CertificateIdentifiers = new[]
				{
					new ISI.Extensions.Acme.OrderCertificateIdentifier()
					{
						CertificateIdentifierType = ISI.Extensions.Acme.OrderCertificateIdentifierType.Dns,
						CertificateIdentifierValue = domainName,
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

			//var orderCertificateIdentifier = createNewOrderResponse.Order.CertificateIdentifiers.First(c => c.CertificateIdentifierType == ISI.Extensions.Acme.OrderCertificateIdentifierType.Dns);


			var getAuthorizationResponse = AcmeApi.GetAuthorization(new()
			{
				HostContext = context,
				AuthorizationUrl = createNewOrderResponse.Order.AuthorizationUrls.First(),
			});


			var challenge = getAuthorizationResponse.Authorization.Challenges.NullCheckedFirstOrDefault(challenge => challenge.ChallengeType == ISI.Extensions.Acme.OrderCertificateIdentifierAuthorizationChallengeType.Dns01);


			var calculateDnsTokenResponse = AcmeApi.CalculateDnsToken(new()
			{
				HostContext = context,
				DomainName = domainName,
				ChallengeToken = challenge.Token,
			});



			//var dnsRecords = DomainsApi.GetDnsRecords(new()
			//{
			//	ApiKey = settings.GetValue("GoDaddy.ApiKey"),
			//	ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
			//	DomainName = domainName,
			//}).DnsRecords;

			var dnsRecord = new ISI.Extensions.Dns.DnsRecord()
			{
				Data = calculateDnsTokenResponse.DnsToken,
				Name = calculateDnsTokenResponse.DnsRecordName,
				//Port = source.Port,
				//Priority = source.Priority,
				//Protocol = source.Protocol,
				//Service = source.Service,
				Ttl = TimeSpan.FromMinutes(10),
				RecordType = ISI.Extensions.Dns.RecordType.TXT,
				//Weight = source.Weight,
			};

			var setDnsRecordsResponse = DomainsApi.SetDnsRecords(new()
			{
				ApiKey = settings.GetValue("GoDaddy.ApiKey"),
				ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
				DomainName = calculateDnsTokenResponse.DomainName,
				DnsRecords = new[] { dnsRecord },
			});

			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

			var completeChallengeResponse = AcmeApi.CompleteChallenge(new()
			{
				HostContext = context,
				ChallengeUrl = challenge.ChallengeUrl,
			});

			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));


			var getChallengeResponse = AcmeApi.GetChallenge(new()
			{
				HostContext = context,
				ChallengeUrl = challenge.ChallengeUrl,
			});


			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

			var createCertificateSigningRequestResponse = AcmeApi.CreateCertificateSigningRequest(new()
			{
				CertificateSigningRequestParameters = new ISI.Extensions.Acme.CertificateSigningRequestParameters()
				{
					CountryName = "US",
					State = "New York",
					Locality = "Glen Cove",
					Organization = "MuthManor",
					OrganizationUnit = null,
					CommonName = domainName,
				},
			});


			var finalizeOrderResponse = AcmeApi.FinalizeOrder(new()
			{
				HostContext = context,
				Order = createNewOrderResponse.Order,
				CertificateSigningRequest = createCertificateSigningRequestResponse.CertificateSigningRequest,
			});

			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

			var getOrderResponse = AcmeApi.GetOrder(new()
			{
				HostContext = context,
				OrderUrl = createNewOrderResponse.Order.OrderKey,
			});


			var getCertificateResponse = AcmeApi.GetCertificate(new()
			{
				HostContext = context,
				GetCertificateUrl = getOrderResponse.Order.GetCertificateUrl,
			});

			var certificatePem = getCertificateResponse.CertificatePem;

			System.IO.File.WriteAllText(CertificateKeyFullName, createCertificateSigningRequestResponse.PrivateKeyPem);
			System.IO.File.WriteAllText(CertificateFullName, certificatePem);

			// other stuff could have modified the request here, but you aren't
			// using any of the extra fancy options

			//System.IO.File.WriteAllText("fa.key", certificateSigningKey.ExportPkcs8PrivateKeyPem());
			//System.IO.File.WriteAllText("fa.csr", certificateSigningRequest.CreateSigningRequestPem());
			//System.IO.File.WriteAllText("publickey.pem", certificateSigningKey.ExportSubjectPublicKeyInfoPem());

		}

		[Test]
		public void ProcessNewOrder_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var pem = GetAccountPem();
			var serializedJsonWebKey = GetAccountSerializedJsonWebKey();
			var accountKey = GetAccountKey();

			var domainName = "*.muthmanor.com";

			var context = AcmeApi.GetHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
				SerializedJsonWebKey = serializedJsonWebKey,
				Pem = pem,
				AccountKey = accountKey,
			}).HostContext;

			var createNewOrderResponse = AcmeApi.ProcessNewOrder(new ISI.Extensions.Acme.DataTransferObjects.AcmeApi.ProcessNewOrderUsingDnsRequest()
			{
				HostContext = context,

				DomainName = domainName,

				CountryName = "US",
				State = "New York",
				Locality = "Glen Cove",
				Organization = "MuthManor",
				OrganizationUnit = null,

				SetDnsRecord = (rootDomainName, dnsRecord) =>
				{
					DomainsApi.SetDnsRecords(new()
					{
						ApiKey = settings.GetValue("GoDaddy.ApiKey"),
						ApiSecret = settings.GetValue("GoDaddy.ApiSecret"),
						DomainName = rootDomainName,
						DnsRecords = new[] { dnsRecord },
					});
				},
			});

			System.IO.File.WriteAllText(CertificateKeyFullName, createNewOrderResponse.PrivateKeyPem);
			System.IO.File.WriteAllText(CertificateFullName, createNewOrderResponse.CertificatePem);
		}
	}
}