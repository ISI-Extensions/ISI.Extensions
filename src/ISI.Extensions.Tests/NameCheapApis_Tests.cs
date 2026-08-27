#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class NameCheapApis_Tests
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
		public void GetTxtRecords_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var domainsApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.DomainsApi>();

			var txtRecords = domainsApi.GetTxtRecords(new()
			{
				Domain = "muthmanor.com",
				Name = "_acme-challenge",
				NameServer = "8.8.8.8",
			}).Values;
		}

		[Test]
		public void GetDnsRecords_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var domainsApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.DomainsApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var dnsRecords = domainsApi.GetDnsRecords(new()
				{
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					Domain = "isi-net.com",
					//Domain = "isi.services",
				}).DnsRecords;
			}
		}

		[Test]
		public void SetDnsRecords_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var domainsApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.DomainsApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				domainsApi.SetDnsRecords(new()
				{
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					Domain = "muthmanor.com",
					DnsRecords =
					[
						new ISI.Extensions.Dns.DnsRecord()
						{
							Name = "_acme-challenge",
							Data = "syNbKycoWcK4rrO_VcN2VWVsurWpSS3z6Ftl6pTYpBg",
							RecordType = ISI.Extensions.Dns.RecordType.TextRecord,
							Ttl = TimeSpan.FromMinutes(10),
						},
						new ISI.Extensions.Dns.DnsRecord()
						{
							Name = "_acme-challenge",
							Data = "Jbw0sk_5m0g39QbqtFEcXpKP_wH-sSOwMLAz8_0tsn8",
							RecordType = ISI.Extensions.Dns.RecordType.TextRecord,
							Ttl = TimeSpan.FromMinutes(10),
						},
						//new ISI.Extensions.Dns.DnsRecord()
						//{
						//	Name = "@",
						//	Data = "10.165.0.1",
						//	RecordType = ISI.Extensions.Dns.RecordType.A,
						//},
					]
				});
			}
		}

		[Test]
		public void ListApproverEmailAddresses_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var sslCertificatesApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.SslCertificatesApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var listCertificatesResponse = sslCertificatesApi.ListCertificates(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});

				var certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault(certificate => certificate.Status == ISI.Extensions.NameCheap.NameCheapSslCertificateStatus.Active);

				var apiResponse = sslCertificatesApi.ListApproverEmailAddresses(new()
				{
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					DomainName = certificate.CommonName,
					CertificateType = certificate.CertificateType,
				});
			}
		}

		[Test]
		public void ListCertificates_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var sslCertificatesApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.SslCertificatesApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var apiResponse = sslCertificatesApi.ListCertificates(new()
				{
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});
			}
		}

		[Test]
		public void GetCertificate_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var sslCertificatesApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.SslCertificatesApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var listCertificatesResponse = sslCertificatesApi.ListCertificates(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});

				var certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault(certificate => certificate.Status == ISI.Extensions.NameCheap.NameCheapSslCertificateStatus.Active);
				//var certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault();

				var getCertificateResponse = sslCertificatesApi.GetCertificate(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
				});

				var csr = getCertificateResponse.Certificates.NullCheckedFirstOrDefault(c => c.CertificateType == ISI.Extensions.Certificates.CertificateType.CertificateSigningRequest);

				var parsedCsr = ISI.Extensions.Certificates.CertificateSigningRequestParameters.Parse(csr.Certificate);

			}
		}

		[Test]
		public void ReissueCertificate_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var sslCertificatesApi = ServiceProvider.GetService<ISI.Extensions.NameCheap.SslCertificatesApi>();

			using (var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler())
			{
				var listCertificatesResponse = sslCertificatesApi.ListCertificates(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});

				var certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault();

				var getCertificateResponse = sslCertificatesApi.GetCertificate(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
				});

				var reissueCertificateResponse = sslCertificatesApi.ReissueCertificate(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
					Csr = getCertificateResponse.Certificates.FirstOrDefault(c => c.CertificateType == ISI.Extensions.Certificates.CertificateType.CertificateSigningRequest).Certificate,
				});

				listCertificatesResponse = sslCertificatesApi.ListCertificates(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});

				certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault(cert =>  (cert.Status == ISI.Extensions.NameCheap.NameCheapSslCertificateStatus.Purchased) &&  string.Equals(cert.CommonName, certificate.CommonName, StringComparison.InvariantCultureIgnoreCase));

				var listApproverEmailAddressesResponse = sslCertificatesApi.ListApproverEmailAddresses(new()
				{
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					DomainName = certificate.CommonName,
					CertificateType = certificate.CertificateType,
				});


				var activateCertificateResponse = sslCertificatesApi.ActivateCertificate(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
					Csr = getCertificateResponse.Certificates.FirstOrDefault(c => c.CertificateType == ISI.Extensions.Certificates.CertificateType.CertificateSigningRequest).Certificate,
					AdminEmailAddress = listApproverEmailAddressesResponse.GenericEmailAddresses.NullCheckedFirstOrDefault(),
				});


				listCertificatesResponse = sslCertificatesApi.ListCertificates(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
				});

				certificate = listCertificatesResponse.Certificates.NullCheckedFirstOrDefault(cert =>  (cert.Status == ISI.Extensions.NameCheap.NameCheapSslCertificateStatus.Purchased) &&  string.Equals(cert.CommonName, certificate.CommonName, StringComparison.InvariantCultureIgnoreCase));

				var updateCertificateVerificationResponse = sslCertificatesApi.UpdateCertificateVerification(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
				});



				getCertificateResponse = sslCertificatesApi.GetCertificate(new()
				{
					//Url = "https://api.sandbox.namecheap.com",
					//ApiUser = settings.GetValue("NameCheap.Sandbox.ApiUser"),
					//ApiKey = settings.GetValue("NameCheap.Sandbox.ApiKey"),
					ApiUser = settings.GetValue("NameCheap.ApiUser"),
					ApiKey = settings.GetValue("NameCheap.ApiKey"),
					VendorCertificateKey = certificate.VendorCertificateKey,
				});

			}
		}
	}
}
