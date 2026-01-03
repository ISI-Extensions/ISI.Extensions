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

namespace ISI.Extensions.Tests.Repository
{
	[TestFixture]
	public partial class PostgreSQL_Tests
	{
		//protected const string ConnectionString = @"Host=localhost;Username=testuser;Password=f2f5eeb1-55d9-441b-be56-8c976cac1338;Database=""ISI.Extensions"";";
		protected const string ConnectionString = @"Host=localhost;Username=masteradmin;Password=9ab831ceb061;Database=""ISI.Extensions"";";
		protected const string MasterConnectionString = @"Host=localhost;Username=masteradmin;Password=9ab831ceb061;Database=postgres;";

		protected Microsoft.Extensions.Configuration.IConfigurationRoot ConfigurationRoot { get; set; }
		protected ISI.Extensions.Repository.PostgreSQL.Configuration SqlServerConfiguration { get; set; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; set; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; set; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; set; }
		protected ISI.Extensions.Repository.IRepositorySetupApi RepositorySetupApi { get; set; }
		protected ISI.Extensions.Repository.IMigrationApi MigrationApi { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			ConfigurationRoot = configurationBuilder.Build().ApplyConfigurationValueReaders();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(ConfigurationRoot);

			services.AddAllConfigurations(ConfigurationRoot)

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
				.AddSingleton<ISI.Extensions.Security.ActiveDirectory.IActiveDirectoryApi, ISI.Extensions.Security.ActiveDirectory.ActiveDirectoryApi>()

				.AddConfigurationRegistrations(ConfigurationRoot)
				.ProcessServiceRegistrars(ConfigurationRoot)
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(ConfigurationRoot);

			serviceProvider.SetServiceLocator();

			//SqlServerConfiguration = new();
			//DateTimeStamper = new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper();
			//Serializer = new ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer();

			SqlServerConfiguration = serviceProvider.GetService<ISI.Extensions.Repository.PostgreSQL.Configuration>();
			DateTimeStamper = serviceProvider.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>();
			Serializer = serviceProvider.GetService<ISI.Extensions.JsonSerialization.IJsonSerializer>();
			
			RepositorySetupApi = new ISI.Extensions.Repository.PostgreSQL.RepositorySetupApi(ConfigurationRoot, Logger, DateTimeStamper, Serializer, MasterConnectionString);

			MigrationApi = new ISI.Extensions.Repository.MigrationApi(serviceProvider, RepositorySetupApi);
		}
	}
}