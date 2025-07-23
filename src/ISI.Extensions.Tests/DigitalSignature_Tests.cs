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
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class DigitalSignature_Tests
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
				.AddSingleton<ISI.Extensions.Security.Ldap.ILdapApi, ISI.Extensions.Security.Ldap.LdapApi>()

				.AddConfigurationRegistrations(configurationRoot)
				.ProcessServiceRegistrars(configurationRoot)
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void DigitalSignature_Test()
		{
			var data = "7c2c9113-1985-46f6-8c57-7a7edc5ddfc58f3e1bd4-4487-4d61-a84c-e93e110e9c173635726b-2556-4d4c-a808-c50b05794aa375c27027-44c4-4852-9157-f8b9cbd2c0cc3f194688-f0b6-4d8a-801c-fcf795bdfce144edf6e3-5057-4d57-b9aa-70f4a58737ca";

			var hashAlgorithmName = System.Security.Cryptography.HashAlgorithmName.SHA256;
			var signaturePadding = System.Security.Cryptography.RSASignaturePadding.Pkcs1;

			var privateKeyPem = (string)null;
			var publicKeyPem = (string)null;

			using (var rsa = System.Security.Cryptography.RSA.Create(2048))
			{
				var certificateRequest = new System.Security.Cryptography.X509Certificates.CertificateRequest("CN=test", rsa, hashAlgorithmName, signaturePadding);

				using (var certificate = certificateRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1)))
				{
					using (var privateKey = certificate.GetRSAPrivateKey())
					{
						privateKeyPem = privateKey.ExportRSAPrivateKeyPem();
					}

					using (var publicKey = certificate.GetRSAPublicKey())
					{
						publicKeyPem = publicKey.ExportRSAPublicKeyPem();
					}
				}
			}

			var signature = (string)null;

			using (var rsa = System.Security.Cryptography.RSA.Create())
			{
				rsa.ImportFromPem(privateKeyPem);

				var dataBytes = Encoding.UTF8.GetBytes(data);

				signature = Convert.ToBase64String(rsa.SignData(dataBytes, System.Security.Cryptography.HashAlgorithmName.SHA256, System.Security.Cryptography.RSASignaturePadding.Pkcs1));
			}

			using (var rsa = System.Security.Cryptography.RSA.Create())
			{
				rsa.ImportFromPem(publicKeyPem);

				var dataBytes = Encoding.UTF8.GetBytes(data);
				var signatureBytes = Convert.FromBase64String(signature);

				var xxx = rsa.VerifyData(dataBytes, signatureBytes, System.Security.Cryptography.HashAlgorithmName.SHA256, System.Security.Cryptography.RSASignaturePadding.Pkcs1);
			}

		}
	}
}