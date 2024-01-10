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
		protected readonly Uri AcmeHostUri = new Uri(@"https://localhost:15633/directory");
		public readonly string AccountPemFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.pem");
		public readonly string AccountJwkAlgorithmKeyFullName = System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, "Account.JwkAlgorithmKey");

		protected string GetAccountPem() => System.IO.File.ReadAllText(AccountPemFullName);
		protected ISI.Extensions.JsonJwt.JwkAlgorithmKey GetAccountJwkAlgorithmKey() => ISI.Extensions.Enum<ISI.Extensions.JsonJwt.JwkAlgorithmKey>.Parse(System.IO.File.ReadAllText(AccountJwkAlgorithmKeyFullName));

		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; set; }
		protected ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory JwkBuilderFactory { get; set; }
		protected ISI.Extensions.JsonJwt.JwtEncoder JwtEncoder { get; set; }

		public ISI.Extensions.Acme.AcmeApi AcmeApi { get; set; }

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

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);
			serviceProvider.SetServiceLocator();



			Logger = serviceProvider.GetService<Microsoft.Extensions.Logging.ILogger>();
			DateTimeStamper = serviceProvider.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>();
			JsonSerializer = serviceProvider.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			JwkBuilderFactory = serviceProvider.GetService<ISI.Extensions.JsonJwt.JwkBuilders.JwkBuilderFactory>();
			JwtEncoder = serviceProvider.GetService<ISI.Extensions.JsonJwt.JwtEncoder>();
			AcmeApi = serviceProvider.GetService<ISI.Extensions.Acme.AcmeApi>();
		}


		[Test]
		public void CreateNewAcmeAccount()
		{
			var jwkAlgorithmKey = ISI.Extensions.JsonJwt.JwkAlgorithmKey.ES521;

			using (var jwtBuilder = JwkBuilderFactory.GetJwkBuilder(jwkAlgorithmKey))
			{
				var context = AcmeApi.GetAcmeHostContext(new()
				{
					AcmeHostDirectoryUri = AcmeHostUri,
					JwkAlgorithmKey = jwkAlgorithmKey,
					Pem = jwtBuilder.GetPrivatePem(),
				}).AcmeHostContext;

				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(AccountPemFullName));
				System.IO.File.WriteAllText(AccountPemFullName, context.Pem);
				System.IO.File.WriteAllText(AccountJwkAlgorithmKeyFullName, context.JwkAlgorithmKey.GetAbbreviation());

				var response = AcmeApi.CreateNewAcmeAccount(new()
				{
					AcmeHostContext = context,
					AccountName = "localhost",
					Contacts = new[] { "me@here.com" },
					TermsOfServiceAgreed = true,
				});
			}
		}


		[Test]
		public void CreateNewAcmeOrder()
		{
			var pem = GetAccountPem();
			var jwkAlgorithmKey = GetAccountJwkAlgorithmKey();

			using (var jwtBuilder = JwkBuilderFactory.GetJwkBuilder(jwkAlgorithmKey, pem))
			{
				var context = AcmeApi.GetAcmeHostContext(new()
				{
					AcmeHostDirectoryUri = AcmeHostUri,
					JwkAlgorithmKey = jwkAlgorithmKey,
					Pem = pem,
				}).AcmeHostContext;

				//var response = AcmeApi.NewAccount(new()
				//{
				//	AcmeHostContext = context,
				//	AccountName = "localhost",
				//	Contacts = new[] { "me@here.com" },
				//	TermsOfServiceAgreed = true,
				//});
			}
		}
	}
}