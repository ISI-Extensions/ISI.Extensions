﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget.Forms.Tests
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
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
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.ConsoleLogger())

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddConfigurationRegistrations(configuration)
				.ProcessServiceRegistrars()
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.SetServiceLocator();

			var serializationConfiguration = serviceProvider.GetService<ISI.Extensions.Serialization.Configuration>();
			serializationConfiguration.DefaultDataContractSerializerType = typeof(ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer).AssemblyQualifiedNameWithoutVersion();
			serializationConfiguration.DefaultSerializerType = typeof(ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer).AssemblyQualifiedNameWithoutVersion();

			var selectedItemPaths = new[]
			{
				@"F:\ISI\Internal Projects\ISI.WebApplication\",
				@"F:\ISI\Internal Projects\ISI.Gravity.WindowsService\",
				@"F:\ISI\Internal Projects\ISI.Identity.WindowsService\",
			};

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			System.Windows.Forms.Application.EnableVisualStyles();

			var form = ISI.Extensions.Nuget.Forms.UpdateNugetPackages.CreateForm(selectedItemPaths, true);

			System.Windows.Forms.Application.Run(form);
		}
	}
}