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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class GoDriveFileSystem_Tests
	{
		//public const string DirectoryUrl = "godrives://mft.rrc.texas.gov/link/ec380e91-5926-4d63-891a-42877a81d32f"; //OrganizationFilesSubDirectory
		//public const string DirectoryUrl = "godrives://mft.rrc.texas.gov/link/caf63b5f-2218-42e5-8e55-9f88673477e7"; //GasLedgersFilesSubDirectory
		//public const string AttributedFullPath = "godrives://mft.rrc.texas.gov/link/abdf8b13-cc23-4489-b942-2ecd1171fae1"; //OilLedgersFilesSubDirectory
		//public const string AttributedFullPath = "godrives://mft.rrc.texas.gov/link/f2fe882e-557b-4a4c-93f8-bcfc3492bae5"; //Statewide API Data
		public const string AttributedFullPath = "godrives://mft.rrc.texas.gov/link/45010658-b04d-4556-a0aa-f2dff44c26dc"; //daily drilling permit

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
		public void GetDirectoryFileSystemPaths_Test()
		{
			var fileSystemPaths = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(AttributedFullPath, false);
		}

		[Test]
		public void Download_Test()
		{
			var fileSystemPaths = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(AttributedFullPath, false);

			var fileSystemPath = fileSystemPaths.Cast<ISI.Extensions.GoDrive.GoDriveFileSystem.GoDriveFileSystemPathFile>().Last();

			Console.WriteLine(fileSystemPath);

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(fileSystemPath))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();
			}
		}
	}
}
