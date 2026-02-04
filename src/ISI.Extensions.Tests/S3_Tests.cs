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
using ISI.Extensions.Scm.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class S3_Tests
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
		public void S3BlobClient_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			//var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("Minio-EndpointUrl"), settings.GetValue("Minio-AccessKey"), settings.GetValue("Minio-SecretKey"), "test-bucket");
			var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("rustfs-EndpointUrl"), settings.GetValue("rustfs-AccessKey"), settings.GetValue("rustfs-SecretKey"), "test");

			var content = string.Join("==>", Enumerable.Range(1, 10000).Select(i => Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));
			var fileName = $"{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)}-document.txt";

			var fileExists = s3BlobClient.FileExistsAsync(new()
			{
				FullName = fileName,
			}).GetAwaiter().GetResult().FileExists;

			Assert.That(!fileExists);

			using (var stream = new System.IO.MemoryStream())
			{
				stream.TextWrite(content);

				var xxx = s3BlobClient.WriteAsync(new()
				{
					FullName = fileName,
					Stream = stream,
				}).GetAwaiter().GetResult();
			}

			var fileExists2 = s3BlobClient.FileExistsAsync(new()
			{
				FullName = fileName,
			}).GetAwaiter().GetResult().FileExists;

			Assert.That(fileExists2);

			using (var stream = new System.IO.MemoryStream())
			{
				var xxx = s3BlobClient.ReadAsync(new()
				{
					FullName = fileName,
					Stream = stream,
				}).GetAwaiter().GetResult();

				stream.Rewind();

				var newContent = stream.TextReadToEnd();

				Assert.That(string.Equals(content, newContent, StringComparison.InvariantCulture));
			}

			var yyy = s3BlobClient.DeleteFileIfExistsAsync(new()
			{
				FullName = fileName,
			}).GetAwaiter().GetResult();
		}

		[Test]
		public void S3_Create_Test_Data()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			//var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("Minio-EndpointUrl"), settings.GetValue("Minio-AccessKey"), settings.GetValue("Minio-SecretKey"), "test-bucket");
			var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("rustfs-EndpointUrl"), settings.GetValue("rustfs-AccessKey"), settings.GetValue("rustfs-SecretKey"), "test");

			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					for (int k = 0; k < 5; k++)
					{
						var content = string.Join("==>", Enumerable.Range(1, 10000).Select(i => Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));
						var fileName = $"{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)}-document.txt";

						var fullName = $"dir{'A' + i}/dir{'G' + j}/dir{'N' + k}/{fileName}";

						using (var stream = new System.IO.MemoryStream())
						{
							stream.TextWrite(content);

							var xxx = s3BlobClient.WriteAsync(new()
							{
								FullName = fullName,
								Stream = stream,
							}).GetAwaiter().GetResult();
						}
					}
				}
			}
		}

		[Test]
		public void S3_ListFiles_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			//var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("Minio-EndpointUrl"), settings.GetValue("Minio-AccessKey"), settings.GetValue("Minio-SecretKey"), "test-bucket");
			var s3BlobClient = new ISI.Extensions.S3.S3BlobClient(settings.GetValue("rustfs-EndpointUrl"), settings.GetValue("rustfs-AccessKey"), settings.GetValue("rustfs-SecretKey"), "test");
			
			var dir85 = s3BlobClient.ListFilesAsync(new()
			{
				Prefix = "dir85",
			}).GetAwaiter().GetResult();
			
			var dir65 = s3BlobClient.ListFilesAsync(new()
			{
				Prefix = "dir65",
			}).GetAwaiter().GetResult();
			
			var dir = s3BlobClient.ListFilesAsync(new()
			{
				Prefix = "dir",
			}).GetAwaiter().GetResult();
		}

		[Test]
		public void File_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);
			settings.OverrideWithEnvironmentVariables();

			var uri = new UriBuilder(settings.GetValue("rustfs-EndpointUrl"));

			var attributedFullPath = $"s3://{settings.GetValue("rustfs-AccessKey")}:{settings.GetValue("rustfs-SecretKey")}@{uri.Host}/test/testFile.txt";

			var content = string.Join("==>", (Enumerable.Range(1, 1000)).Select(i => Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));

			using (var stream = new System.IO.MemoryStream())
			{
				using (var streamWriter = new System.IO.StreamWriter(stream))
				{
					streamWriter.Write(content);
					streamWriter.Flush();

					stream.Flush();
					stream.Rewind();

					using (var fileSystemStream = ISI.Extensions.FileSystem.OpenWrite(attributedFullPath, true, true))
					{
						stream.CopyTo(fileSystemStream);
						fileSystemStream.Flush();
					}
				}
			}

			Assert.That(ISI.Extensions.FileSystem.FileExists(attributedFullPath));

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(attributedFullPath))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();

				var readContent = stream.TextReadToEnd();

				Assert.That(string.Equals(content, readContent, StringComparison.Ordinal));
			}

			ISI.Extensions.FileSystem.RemoveFile(attributedFullPath);

			Assert.That(!ISI.Extensions.FileSystem.FileExists(attributedFullPath));
		}
	}
}