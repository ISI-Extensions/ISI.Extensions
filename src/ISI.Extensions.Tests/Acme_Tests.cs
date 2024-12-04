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
		protected readonly Uri HostDirectoryUri = new Uri(@"https://acme-v02.api.letsencrypt.org/directory");
		protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-account.pem");
		protected readonly string AccountSerializedJsonWebKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-account.SerializedJsonWebKey");
		protected readonly string AccountKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-account.key");
		protected readonly string CertificateKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-account.certificate.key");
		protected readonly string CertificateFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-account.certificate.crt");

		//protected readonly Uri HostDirectoryUri = new Uri(@"https://acme-staging-v02.api.letsencrypt.org/directory");
		//protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.pem");
		//protected readonly string AccountSerializedJsonWebKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.SerializedJsonWebKey");
		//protected readonly string AccountKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.key");
		//protected readonly string CertificateKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.certificate.key");
		//protected readonly string CertificateFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "letsencrypt-staging-account.certificate.crt");

		//protected readonly Uri HostDirectoryUri = new Uri(@"https://localhost:15633/directory");
		//protected readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.pem");
		//protected readonly string AccountSerializedJsonWebKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.SerializedJsonWebKey");
		//protected readonly string AccountKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.key");
		//protected readonly string CertificateKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.certificate.key");
		//protected readonly string CertificateFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.certificate.crt");

		protected string GetAccountPem() => System.IO.File.ReadAllText(AccountPemFullName);
		protected string GetAccountSerializedJsonWebKey() => System.IO.File.ReadAllText(AccountSerializedJsonWebKeyFullName);
		protected string GetAccountKey() => System.IO.File.ReadAllText(AccountKeyFullName);

		protected IServiceProvider ServiceProvider { get; set; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; set; }

		protected ISI.Extensions.Acme.AcmeApi AcmeApi { get; set; }
		protected ISI.Extensions.Dns.IDomainsApi DomainsApi { get; set; }

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
			DomainsApi = ServiceProvider.GetService<ISI.Extensions.Dns.DomainsApi>();
		}

		[Test]
		public void CreateNewAccount_LetsEncrypt_Test()
		{
			var context = AcmeApi.CreateNewHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
			}).HostContext;

			var createNewAccountResponse = AcmeApi.CreateNewAccount(new()
			{
				HostContext = context,
				//AccountName = "localhost",
				Contacts = ["ron.muth@isi-net.com"],
				TermsOfServiceAgreed = true,
			});

			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AccountPemFullName));
			System.IO.File.WriteAllText(AccountPemFullName, context.Pem);
			System.IO.File.WriteAllText(AccountSerializedJsonWebKeyFullName, context.SerializedJsonWebKey);
			System.IO.File.WriteAllText(AccountKeyFullName, createNewAccountResponse.Account.AccountKey);
		}

		[Test]
		public void CreateNewAccount_CertificateManager_Test()
		{
			var context = AcmeApi.CreateNewHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
			}).HostContext;

			var createNewAccountResponse = AcmeApi.CreateNewAccount(new()
			{
				HostContext = context,
				AccountName = "localhost",
				Contacts = ["ron.muth@isi-net.com"],
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

			var domain = "www.muthmanor.com";

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
				CertificateIdentifiers =
				[
					new ISI.Extensions.Acme.OrderCertificateIdentifier()
					{
						CertificateIdentifierType = ISI.Extensions.Acme.OrderCertificateIdentifierType.Dns,
						CertificateIdentifierValue = domain,
					}
				],
			});

			var getAuthorizationResponse = AcmeApi.GetAuthorization(new()
			{
				HostContext = context,
				AuthorizationUrl = createNewOrderResponse.Order.AuthorizationUrls.First(),
			});

			var challenge = getAuthorizationResponse.Authorization.Challenges.NullCheckedFirstOrDefault(challenge => challenge.ChallengeType == ISI.Extensions.Acme.OrderCertificateIdentifierAuthorizationChallengeType.Dns01);

			var calculateDnsTokenResponse = AcmeApi.CalculateDnsToken(new()
			{
				HostContext = context,
				Domain = domain,
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
				DnsProviderUuid = ISI.Extensions.GoDaddy.DomainsApi.DnsProviderUuid,
				ApiUser = settings.GetValue("GoDaddy.ApiKey"),
				ApiKey = settings.GetValue("GoDaddy.ApiSecret"),
				Domain = calculateDnsTokenResponse.Domain,
				DnsRecords = [dnsRecord],
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
					CommonName = domain,
				},
			});

			var finalizeOrderResponse = AcmeApi.FinalizeOrder(new()
			{
				HostContext = context,
				FinalizeOrderUrl = createNewOrderResponse.Order.FinalizeOrderUrl,
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
				GetCertificatesUrl = getOrderResponse.Order.GetCertificatesUrl,
			});

			var certificatePem = getCertificateResponse.CertificatePem;

			System.IO.File.WriteAllText(CertificateKeyFullName, createCertificateSigningRequestResponse.PrivateKeyPem);
			System.IO.File.WriteAllText(CertificateFullName, certificatePem);
		}


		public class ProcessNewOrderDetails
		{
			public string Domain { get; set; }
			public string Organization { get; set; }
			public ISI.Extensions.Acme.DataTransferObjects.AcmeApi.SetDnsRecordDelegate SetDnsRecord { get; set; }

			public string CertificateKeyFullName { get; set; }
			public string CertificateFullName { get; set; }
		}

		private ProcessNewOrderDetails GetProcessNewOrderDetails(ISI.Extensions.Scm.Settings settings, string domainName, Guid dnsProviderUuid)
		{
			var response = new ProcessNewOrderDetails();

			response.Domain = domainName;
			response.Organization = domainName.TrimStart("*.");

			if (dnsProviderUuid == ISI.Extensions.GoDaddy.DomainsApi.DnsProviderUuid)
			{
				response.SetDnsRecord = (rootDomain, dnsRecord) =>
				{
					DomainsApi.SetDnsRecords(new()
					{
						DnsProviderUuid = ISI.Extensions.GoDaddy.DomainsApi.DnsProviderUuid,
						ApiUser = settings.GetValue("GoDaddy.ApiKey"),
						ApiKey = settings.GetValue("GoDaddy.ApiSecret"),

						Domain = rootDomain,
						DnsRecords = [dnsRecord],
					});
				};
			}
			else if (dnsProviderUuid == ISI.Extensions.NameCheap.DomainsApi.DnsProviderUuid)
			{
				response.SetDnsRecord = (rootDomain, dnsRecord) =>
				{
					DomainsApi.SetDnsRecords(new()
					{
						DnsProviderUuid = ISI.Extensions.NameCheap.DomainsApi.DnsProviderUuid,
						ApiUser = settings.GetValue("NameCheap.ApiUser"),
						ApiKey = settings.GetValue("NameCheap.ApiKey"),

						Domain = rootDomain,
						DnsRecords = [dnsRecord],
					});
				};
			}
			else if (dnsProviderUuid == ManualDomainsApi.DnsProviderUuid)
			{
				response.SetDnsRecord = (rootDomain, dnsRecord) =>
				{
					DomainsApi.SetDnsRecords(new()
					{
						DnsProviderUuid = ManualDomainsApi.DnsProviderUuid,

						Domain = rootDomain,
						DnsRecords = [dnsRecord],
					});
				};
			}

			response.CertificateKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, $"{domainName.Replace("*.", "_.")}.key");
			response.CertificateFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, $"{domainName.Replace("*.", "_.")}.pem");

			return response;
		}



		[Test]
		public void GetDnsProviders_Test()
		{
			var xxx = DomainsApi.GetDnsProviders(new());
		}



		[Test]
		public void ProcessNewOrder_LetsEncrypt_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var pem = GetAccountPem();
			var serializedJsonWebKey = GetAccountSerializedJsonWebKey();
			var accountKey = GetAccountKey();

			var processNewOrderDetails = GetProcessNewOrderDetails(settings, "*.ronmuth.com", ISI.Extensions.NameCheap.DomainsApi.DnsProviderUuid);

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

				Domain = processNewOrderDetails.Domain,

				OrganizationUnit = null,

				Organization = processNewOrderDetails.Organization,
				Locality = "Glen Cove",
				State = "New York",

				CountryName = "US",

				SetDnsRecord = processNewOrderDetails.SetDnsRecord,
			});

			System.IO.File.WriteAllText(processNewOrderDetails.CertificateKeyFullName, createNewOrderResponse.PrivateKeyPem);
			System.IO.File.WriteAllText(processNewOrderDetails.CertificateFullName, createNewOrderResponse.CertificatePem);
		}

		[Test]
		public void ProcessNewOrder_CertificateManager_Test()
		{
			var pem = GetAccountPem();
			var serializedJsonWebKey = GetAccountSerializedJsonWebKey();
			var accountKey = GetAccountKey();

			var domain = "*.isi-net.com";

			var context = AcmeApi.GetHostContext(new()
			{
				HostDirectoryUri = HostDirectoryUri,
				SerializedJsonWebKey = serializedJsonWebKey,
				Pem = pem,
				AccountKey = accountKey,
			}).HostContext;

			var createNewOrderResponse = AcmeApi.ProcessNewOrder(new ISI.Extensions.Acme.DataTransferObjects.AcmeApi.ProcessNewOrderUsingExistingCertificateRequest()
			{
				HostContext = context,

				Domain = domain,

				PostRenewalActions =
				[
					new ISI.Extensions.Acme.OrderCertificateDomainPostRenewalActionAcmeAgentWebHook()
					{
						SetCertificatesUrl = @"https://nginx/upload-certificates",
					}
				]
			});
		}
	}

	[ISI.Extensions.DomainsApi(_dnsProviderUuid, "ManualDomainsApi")]
	public class ManualDomainsApi : ISI.Extensions.Dns.IDomainsApi
	{
		internal const string _dnsProviderUuid = "d83036c2-5fc3-48d7-9d14-945be141107e";
		public static Guid DnsProviderUuid { get; } = _dnsProviderUuid.ToGuid();

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public ManualDomainsApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProvidersResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviders(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProvidersRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.SetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse();

			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine($"Domain Name: {request.Domain}");
			foreach (var dnsRecord in request.DnsRecords)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine($"RecordType: {dnsRecord.RecordType}");
				stringBuilder.AppendLine($"Name: {dnsRecord.Name}");
				stringBuilder.AppendLine($"Data: {dnsRecord.Data}");
			}

			System.Diagnostics.Debugger.Break();

			return response;
		}
	}
}
