#region Copyright & License
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
using ISI.Platforms.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Platforms.ServiceApplication.Extensions
{
	public static partial class ContextExtensions
	{
		public static void SetConfiguration(this ServiceApplicationContext context)
		{
			System.AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs) =>
			{
				try
				{
					var exception = unhandledExceptionEventArgs.ExceptionObject as Exception ?? new Exception(string.Format("An unhandled exception occurred in this application: {0}", unhandledExceptionEventArgs.ExceptionObject));

					context.LoggerConfigurator.Error(exception, "Unhandled Exception");
				}
				catch
				{
					// do not terminate any thread
				}
			};

			var showConfig = context.CommandLineArguments.HasOption("--showConfig");
#if DEBUG
			//showConfig = true;
#endif

			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

			var configurationsPath = string.Format("Configuration{0}", System.IO.Path.DirectorySeparatorChar);

			var activeEnvironmentConfiguration = configurationBuilder.GetActiveEnvironmentConfiguration($"{configurationsPath}isi.extensions.environmentsConfig.json");

			var connectionStringPath = string.Format("Configuration{0}", System.IO.Path.DirectorySeparatorChar);
			configurationBuilder.AddClassicConnectionStringsSectionFile($"{connectionStringPath}connectionStrings.config", true);
			configurationBuilder.AddClassicConnectionStringsSectionFiles(activeEnvironmentConfiguration.ActiveEnvironments, environment => $"{connectionStringPath}connectionStrings.{environment}.config");
//#if !DEBUG
			configurationBuilder.AddDataPathClassicConnectionStringsSectionFile(System.IO.Path.Combine(context.RootType.Namespace, "connectionStrings.config"));
//#endif

			configurationBuilder.SetBasePath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
			configurationBuilder.AddJsonFile("appsettings.json", optional: true);
			configurationBuilder.AddJsonFiles(activeEnvironmentConfiguration.ActiveEnvironments, environment => $"appsettings.{environment}.json");
//#if !DEBUG
			configurationBuilder.AddDataPathJsonFile(System.IO.Path.Combine(context.RootType.Namespace, "appsettings.json"));
//#endif

			configurationBuilder.AddEnvironmentConfiguration(showConfig);

			context.SetConfigurationRoot(configurationBuilder.Build().ApplyConfigurationValueReaders());
			context.SetActiveEnvironment(activeEnvironmentConfiguration.ActiveEnvironment);
			context.LoggerConfigurator?.SetBaseLogger(context);

			if (showConfig)
			{
				foreach (System.Collections.DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
				{
					System.Console.WriteLine($"  EV \"{environmentVariable.Key}\" => \"{environmentVariable.Value}\"");
				}

				System.Console.WriteLine($"ActiveEnvironment: {activeEnvironmentConfiguration.ActiveEnvironment}");
				System.Console.WriteLine($"ActiveEnvironments: {string.Join(", ", activeEnvironmentConfiguration.ActiveEnvironments.Select(e => string.Format("\"{0}\"", e)))}");

				foreach (var keyValuePair in context.ConfigurationRoot.AsEnumerable())
				{
					System.Console.WriteLine($"  Config \"{keyValuePair.Key}\" => \"{keyValuePair.Value}\"");
				}
			}
		}
	}
}
