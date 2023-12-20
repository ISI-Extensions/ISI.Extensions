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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.Topshelf.Extensions;
using Microsoft.Extensions.Configuration;
using Topshelf;

namespace ISI.Platforms.ServiceApplication
{
	public class Program
	{
		internal static ISI.Platforms.ILoggerConfigurator _loggerConfigurator { get; set; }

		public static int Main(ServiceApplicationStartupRequest request)
		{
			_loggerConfigurator = request.LoggerConfigurator;

			System.AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

			var configurationsPath = string.Format("Configuration{0}", System.IO.Path.DirectorySeparatorChar);

			var activeEnvironment = configurationBuilder.GetActiveEnvironmentConfig($"{configurationsPath}isi.extensions.environmentsConfig.json");

			var connectionStringPath = string.Format("Configuration{0}", System.IO.Path.DirectorySeparatorChar);
			configurationBuilder.AddClassicConnectionStringsSectionFile($"{connectionStringPath}connectionStrings.config");
			configurationBuilder.AddClassicConnectionStringsSectionFiles(activeEnvironment.ActiveEnvironments, environment => $"{connectionStringPath}connectionStrings.{environment}.config");
#if !DEBUG
			configurationBuilder.AddDataPathClassicConnectionStringsSectionFile(System.IO.Path.Combine(typeof(Program).Namespace, "connectionStrings.config"));
#endif

			configurationBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
			configurationBuilder.AddJsonFile("appsettings.json", optional: false);
			configurationBuilder.AddJsonFiles(activeEnvironment.ActiveEnvironments, environment => $"appsettings.{environment}.json");
#if !DEBUG
			configurationBuilder.AddDataPathJsonFile(System.IO.Path.Combine(typeof(Program).Namespace, "appsettings.json"));
#endif

			configurationBuilder.AddEnvironmentConfiguration();

			var configurationRoot = configurationBuilder.Build().ApplyConfigurationValueReaders();

			request.LoggerConfigurator.SetBaseLogger(typeof(Program), configurationRoot, activeEnvironment.ActiveEnvironment);


			var showConfig = request.Args.NullCheckedAny(arg => string.Equals(arg, "--showConfig", StringComparison.InvariantCultureIgnoreCase));
#if DEBUG
			showConfig = true;
#endif
			if (showConfig)
			{
				foreach (System.Collections.DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
				{
					System.Console.WriteLine($"  EV \"{environmentVariable.Key}\" => \"{environmentVariable.Value}\"");
				}

				System.Console.WriteLine($"ActiveEnvironment: {activeEnvironment.ActiveEnvironment}");
				System.Console.WriteLine($"ActiveEnvironments: {string.Join(", ", activeEnvironment.ActiveEnvironments.Select(e => string.Format("\"{0}\"", e)))}");

				foreach (var keyValuePair in configurationRoot.AsEnumerable())
				{
					System.Console.WriteLine($"  Config \"{keyValuePair.Key}\" => \"{keyValuePair.Value}\"");
				}
			}

			return (int)Topshelf.HostFactory.Run(hostConfigurator =>
			{
				var configuration = configurationRoot.GetConfiguration<ISI.Extensions.Topshelf.Configuration>();

				hostConfigurator.SetDescription(configuration);
				hostConfigurator.SetDisplayName(configuration);
				hostConfigurator.SetServiceName(configuration);

				hostConfigurator.RunAs(configuration);

				_loggerConfigurator.AddLogger(hostConfigurator);

				hostConfigurator.StartAutomatically();

				hostConfigurator.EnableServiceRecovery(recoveryConfig =>
				{
					recoveryConfig.RestartService(1); // restart the service after 1 minute
					recoveryConfig.RestartService(1); // restart the service after 1 minute
					recoveryConfig.SetResetPeriod(1); // set the reset interval to one day
				});

				hostConfigurator.Service<ServiceManager>(configurator =>
				{
					//configurator.ConstructUsing(serviceFactory => serviceProvider.GetService<ServiceManager>());
					configurator.ConstructUsing(serviceFactory => new ServiceManager());
					configurator.WhenStarted((service, control) =>
					{
						control.RequestAdditionalTime(TimeSpan.FromMinutes(10));
						service.StartAsync(request).Wait();
						return true;
					});
					configurator.WhenStopped((service, control) =>
					{
						service.StopAsync().Wait();
						_loggerConfigurator.CloseAndFlush();
						return true;
					});
				});
			});
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			try
			{
				var exception = unhandledExceptionEventArgs.ExceptionObject as Exception ?? new Exception(string.Format("An unhandled exception occurred in this application: {0}", unhandledExceptionEventArgs.ExceptionObject));

				_loggerConfigurator.Error(exception, "Unhandled Exception");
			}
			catch
			{
				// do not terminate any thread
			}
		}
	}
}
