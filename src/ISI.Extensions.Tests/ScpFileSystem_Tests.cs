﻿#region Copyright & License
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
	public class ScpFileSystem_Tests
	{
		public string Server { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
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

			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			Server = settings.GetValue("Asterisk.IpAddress");
			UserName = settings.GetValue("ISI.Installer.UserName");
			Password = settings.GetValue("ISI.Installer.Password");
		}

		[Test]
		public void GetFileSystemPathFile_Test()
		{
			var directory = "Pizza/Homemade";
			var pathName = "Topping.txt";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathFile = ISI.Extensions.FileSystem.GetFileSystemPathFile(attributedFullPath);

			Assert.IsTrue(fileSystemPathFile is ISI.Extensions.SshNet.ScpFileSystem.ScpFileSystemPathFile);
			Assert.AreEqual(fileSystemPathFile.Server, Server);
			Assert.AreEqual(fileSystemPathFile.UserName, UserName);
			Assert.AreEqual(fileSystemPathFile.Password, Password);
			Assert.AreEqual(fileSystemPathFile.Directory, directory);
			Assert.AreEqual(fileSystemPathFile.PathName, pathName);
			Assert.AreEqual(fileSystemPathFile.FullPath(), fullPath);
			Assert.AreEqual(fileSystemPathFile.AttributedFullPath(), attributedFullPath);
		}

		[Test]
		public void GetFileSystemPathDirectory_Test()
		{
			var directory = "Pizza/Homemade";
			var pathName = "Topping";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathDirectory = ISI.Extensions.FileSystem.GetFileSystemPathDirectory(attributedFullPath);

			Assert.IsTrue(fileSystemPathDirectory is ISI.Extensions.SshNet.ScpFileSystem.ScpFileSystemPathDirectory);
			Assert.AreEqual(fileSystemPathDirectory.Server, Server);
			Assert.AreEqual(fileSystemPathDirectory.UserName, UserName);
			Assert.AreEqual(fileSystemPathDirectory.Password, Password);
			Assert.AreEqual(fileSystemPathDirectory.Directory, directory);
			Assert.AreEqual(fileSystemPathDirectory.PathName, pathName);
			Assert.AreEqual(fileSystemPathDirectory.FullPath(), fullPath);
			Assert.AreEqual(fileSystemPathDirectory.AttributedFullPath(), attributedFullPath);
		}

		[Test]
		public void GetParentFileSystemPathDirectory_Test()
		{
			var rootDirectory = "Pizza";
			var localDirectory = "Homemade";
			var directory = $"{rootDirectory}/{localDirectory}";
			var pathName = "Topping.txt";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathDirectory = ISI.Extensions.FileSystem.GetFileSystemPathFile(attributedFullPath).GetParentFileSystemPathDirectory();

			Assert.IsTrue(fileSystemPathDirectory is ISI.Extensions.SshNet.ScpFileSystem.ScpFileSystemPathDirectory);
			Assert.AreEqual(fileSystemPathDirectory.Server, Server);
			Assert.AreEqual(fileSystemPathDirectory.UserName, UserName);
			Assert.AreEqual(fileSystemPathDirectory.Password, Password);
			Assert.AreEqual(fileSystemPathDirectory.Directory, rootDirectory);
			Assert.AreEqual(fileSystemPathDirectory.PathName, localDirectory);
			Assert.AreEqual(fileSystemPathDirectory.FullPath(), $"{rootDirectory}/{localDirectory}");
			Assert.AreEqual(fileSystemPathDirectory.AttributedFullPath(), $"scp://{UserName}:{Password}@{Server}/{rootDirectory}/{localDirectory}");
		}

		[Test]
		public void GetDirectoryFileSystemPaths_Test()
		{
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/";

			var fileSystemPaths = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(attributedFullPath, false);

			var fileSystemPathsRecursive = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(attributedFullPath, true);
		}

		[Test]
		public void FileExists_Test()
		{
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/setup.exe";

			Assert.IsTrue(ISI.Extensions.FileSystem.FileExists(attributedFullPath));
		}

		[Test]
		public void DirectoryExists_Test()
		{
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/support";

			Assert.IsTrue(ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));
		}

		[Test]
		public void Directory_Test()
		{
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/testDirectory";

			ISI.Extensions.FileSystem.CreateDirectory(attributedFullPath);

			Assert.IsTrue(ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));

			ISI.Extensions.FileSystem.RemoveDirectory(attributedFullPath);

			Assert.IsFalse(ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));
		}

		[Test]
		public void File_Test()
		{
			var attributedFullPath = $"scp://{UserName}:{Password}@{Server}/testFile.txt";

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

			Assert.IsTrue(ISI.Extensions.FileSystem.FileExists(attributedFullPath));

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(attributedFullPath))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();

				var readContent = stream.TextReadToEnd();

				Assert.AreEqual(content, readContent);
			}

			ISI.Extensions.FileSystem.RemoveFile(attributedFullPath);

			Assert.IsFalse(ISI.Extensions.FileSystem.FileExists(attributedFullPath));
		}
	}
}
