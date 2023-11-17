#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public class CodeSigningApi_Tests
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

				.AddConfigurationRegistrations(configurationRoot)
				.ProcessServiceRegistrars(configurationRoot)
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configurationRoot);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void SignAssemblies_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var logger = new ISI.Extensions.TextWriterLogger(TestContext.Progress);
			var codeSigningApi = new ISI.Extensions.VisualStudio.CodeSigningApi(logger, new ISI.Extensions.VisualStudio.VsixSigntoolApi(logger, new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.Nuget.Configuration(), logger, new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer())));

			var fileNames = System.IO.Directory.GetFiles(@"F:\ISI\Internal Projects\ISI.Extensions\Nuget");

			codeSigningApi.SignAssemblies(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.SignAssembliesRequest()
			{
				AssemblyFullNames = fileNames,
				CodeSigningCertificateTokenCertificateFileName = settings.CodeSigning.Token.CertificateFileName,
				CodeSigningCertificateTokenCryptographicProvider = settings.CodeSigning.Token.CryptographicProvider,
				CodeSigningCertificateTokenContainerName = settings.CodeSigning.Token.ContainerName,
				CodeSigningCertificateTokenPassword = settings.CodeSigning.Token.Password,
				TimeStampUri = new(settings.CodeSigning.TimeStampUrl),
				TimeStampDigestAlgorithm = ISI.Extensions.Enum<ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningDigestAlgorithm>.Parse(settings.CodeSigning.TimeStampDigestAlgorithm),
				CertificateFileName = settings.CodeSigning.CertificateFileName,
				CertificatePassword = settings.CodeSigning.CertificatePassword,
				CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
				DigestAlgorithm = ISI.Extensions.Enum<ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningDigestAlgorithm>.Parse(settings.CodeSigning.DigestAlgorithm),
				RunAsync = settings.CodeSigning.RunAsync,
			});
		}

		[Test]
		public void VsixSign_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var logger = new ISI.Extensions.TextWriterLogger(TestContext.Progress);
			var codeSigningApi = new ISI.Extensions.VisualStudio.CodeSigningApi(logger, new ISI.Extensions.VisualStudio.VsixSigntoolApi(logger, new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.Nuget.Configuration(), logger, new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer())));

			codeSigningApi.SignVsixes(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.SignVsixesRequest()
			{
				VsixFullNames = new[] { @"F:\ISI\Internal Projects\ISI.VisualStudio.Extensions\Publish\ISI.VisualStudio.Extensions.vsix" },
				CodeSigningCertificateTokenCertificateFileName = settings.CodeSigning.Token.CertificateFileName,
				CodeSigningCertificateTokenCryptographicProvider = settings.CodeSigning.Token.CryptographicProvider,
				CodeSigningCertificateTokenContainerName = settings.CodeSigning.Token.ContainerName,
				CodeSigningCertificateTokenPassword = settings.CodeSigning.Token.Password,
				TimeStampUri = new(settings.CodeSigning.TimeStampUrl),
				TimeStampDigestAlgorithm = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningDigestAlgorithm.Sha256,
				CertificateFileName = settings.CodeSigning.CertificateFileName,
				CertificatePassword = settings.CodeSigning.CertificatePassword,
				CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
				DigestAlgorithm = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningDigestAlgorithm.Sha256,
				//Settings = settings,
				//CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
				//CertificatePath = File(settings.CodeSigning.CertificateFileName),
				//CertificatePassword = settings.CodeSigning.CertificatePassword,
			});
		}
	}
}
