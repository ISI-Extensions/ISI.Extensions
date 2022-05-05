#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class Compression_Tests
	{
		public readonly string[] SourceFileParentDirectoryUrls = new[]
		{
			"godrives://mft.rrc.texas.gov/link/ec380e91-5926-4d63-891a-42877a81d32f", //OrganizationFilesSubDirectory
			"godrives://mft.rrc.texas.gov/link/caf63b5f-2218-42e5-8e55-9f88673477e7", //GasLedgersFilesSubDirectory
			"godrives://mft.rrc.texas.gov/link/abdf8b13-cc23-4489-b942-2ecd1171fae1", //OilLedgersFilesSubDirectory
		};

		public readonly string[] SourceFileNameRegexes = new[]
		{
			@"^orf.+\.ebc\.gz?$",
			@"^olf.+\.ebc\.gz?$",
			@"^gsf.+\.ebc\.gz?$",
			//@"^orf.+\.ebc(?:\.gz)?$",
			//@"^olf.+\.ebc(?:\.gz)?$",
			//@"^gsf.+\.ebc(?:\.gz)?$",
		};

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			ISI.Extensions.StartUp.Start();

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration)

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

				.AddConfigurationRegistrations(configuration)
				.ProcessServiceRegistrars()
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void Expander_7zip_Test()
		{
			var sourceFileUrl = @"C:\Users\ron.muth\Downloads\FacilityTransactionsArchive.20220316021546194.e44e2852-98f0-46bb-a7c3-fbba73125e1d.7z";

			var fileStreams = new ISI.Extensions.Stream.FileStreamCollection();

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(sourceFileUrl))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();

				fileStreams.Add(sourceFileUrl, stream, true, null);
			}
		}

		[Test]
		public void Expander_gz_Test()
		{
			var sourceFileUrl = @"C:\Users\ron.muth\Downloads\dbf900.ebc.gz";

			var fileStreams = new ISI.Extensions.Stream.FileStreamCollection();

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(sourceFileUrl))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();

				fileStreams.Add(sourceFileUrl, stream, true, null);
			}
		}
	}
}
