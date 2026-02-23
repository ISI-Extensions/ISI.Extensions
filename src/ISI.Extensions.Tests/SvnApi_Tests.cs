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
	public class SvnApi_Tests
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
		public void TagAndNote_Test()
		{
			var buildDateTimeStamp = DateTime.UtcNow;

			var jan1st2000 = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			var buildRevision = $"{Math.Floor((buildDateTimeStamp.Date - jan1st2000).TotalDays)}.{Math.Floor(((buildDateTimeStamp - buildDateTimeStamp.Date).TotalSeconds) / 2)}";

			var serializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();
			var svnApi = new ISI.Extensions.Svn.SvnApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress), serializer);

			svnApi.TagAndNote(new()
			{
				WorkingCopyDirectory = @"F:\ISI\Internal Projects\ISI.WebApplication",
				TagName = $"4.1.{buildRevision} ({buildDateTimeStamp:yyyyMMdd.HHmmss})",
				DateTimeStamp = buildDateTimeStamp,
				TryGetExternalTagName = (string externalPath, out string externalTagName) =>
				{
					var externalPathPieces = externalPath.Split(new[] { '/' }).ToList();
					while (string.Equals(externalPathPieces.Last(), "trunk", StringComparison.InvariantCultureIgnoreCase))
					{
						externalPathPieces.RemoveAt(externalPathPieces.Count - 1);
					}

					switch (externalPathPieces.Last())
					{
						case "ISI.Extensions":
							externalTagName = $"1.1.{buildRevision} ({buildDateTimeStamp:yyyyMMdd.HHmmss})";
							return true;

						case "ISI.CMS":
							externalTagName = $"1.2.{buildRevision} ({buildDateTimeStamp:yyyyMMdd.HHmmss})";
							return true;

					}

					externalTagName = string.Empty;
					return false;
				},
			});

		}

		[Test]
		public void CheckOutFile_Test()
		{
			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				var serializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();
				var svnApi = new ISI.Extensions.Svn.SvnApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress), serializer);

				var sourceUrl = @"https://svn.isi-net.com/ISI/ISI.FrameWork/trunk/src/jenkins/ISI.FrameWork.Build.jenkinsConfig";

				svnApi.CheckOutSingleFile(new()
				{
					SourceUrl = sourceUrl,
					TargetFullName = tempDirectory.FullName,
				});
			}
		}

		[Test]
		public void List_Test()
		{
			var serializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();
			var svnApi = new ISI.Extensions.Svn.SvnApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress), serializer);

			var sourceUrl = @"https://svn.isi-net.com/ISI/ISI.FrameWork/trunk/src/";

			var fileNames = svnApi.List(new()
			{
				SourceUrl = sourceUrl,
				Depth = ISI.Extensions.Svn.Depth.Infinity,
			});
		}

		[Test]
		public void GetRevisionInfo_Test()
		{
			var serializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();
			var svnApi = new ISI.Extensions.Svn.SvnApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress), serializer);

			var getRevisionInfoResponse = svnApi.GetRevisionInfo(new()
			{
				RepositoryPath = @"\\isinySVN01\E$\SVN\ISI",
				Revision = 144328,
			});
		}

		[Test]
		public void ParseMessage_Test()
		{
			var messageParser = new ISI.Extensions.Svn.MessageParser();

			var getRevisionInfoResponse = messageParser.ParseMessage(@" refs CD-4915
Add support for day 70 qualified records based off of grace period.");

		}


		[Test]
		public void GetWorkingCopyCommitInformation_Test()
		{
			var serializer = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();
			var svnApi = new ISI.Extensions.Svn.SvnApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress), serializer);

			var xxx = svnApi.GetWorkingCopyCommitInformation(new()
			{
				FullName = @"F:\ISI\ISI.FrameWork\src\ISI.Libraries\ISI.Libraries.Repository.DynamoDB\ISI.Libraries.Repository.DynamoDB.csproj",
			});

			var xx1x = svnApi.GetWorkingCopyCommitInformation(new()
			{
				FullName = @"F:\ISI\ISI.FrameWork\src\ISI.Libraries\ISI.Libraries.Repository.DynamoDB",
			});
		}
	}
}
