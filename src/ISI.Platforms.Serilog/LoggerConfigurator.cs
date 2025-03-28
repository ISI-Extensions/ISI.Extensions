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
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;

namespace ISI.Platforms.Serilog
{
	public class LoggerConfigurator : ISI.Platforms.ILoggerConfigurator
	{
		public void SetBaseLogger(ServiceApplicationContext context)
		{
			global::Serilog.Log.Logger = UpdateLoggerConfiguration(null, null, context.ConfigurationRoot, context.ActiveEnvironment).CreateLogger();

			global::Serilog.Log.Information($"Starting {context.RootType.Namespace}");
			global::Serilog.Log.Information($"Version: {ISI.Extensions.SystemInformation.GetAssemblyVersion(context.RootAssembly)}");
			global::Serilog.Log.Information($"Data: {System.IO.Path.Combine(ISI.Extensions.IO.Path.DataRoot, context.RootType.Namespace)}");
		}

		public void AddLogger(object hostConfigurator)
		{

		}

		public void AddLogger(Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot, string activeEnvironment)
		{
			services.AddSerilog(loggerConfiguration => UpdateLoggerConfiguration(loggerConfiguration, null, configurationRoot, activeEnvironment));
		}

		public void AddRequestLogging(object applicationBuilder)
		{
			(applicationBuilder as Microsoft.AspNetCore.Builder.IApplicationBuilder)?.UseSerilogRequestLogging(options =>
			{
				// Customize the message template
				options.MessageTemplate = "Handled {RequestPath}";

				// Emit debug-level events instead of the defaults
				options.GetLevel = (httpContext, elapsed, ex) => global::Serilog.Events.LogEventLevel.Debug;

				// Attach additional properties to the request completion event
				options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
					diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
				};
			});
		}

		public void CloseAndFlush()
		{
			global::Serilog.Log.CloseAndFlush();
		}

		public void Information(string message)
		{
			global::Serilog.Log.Logger.Information(message);
		}

		public void Error(Exception exception, string message)
		{
			global::Serilog.Log.Logger.Error(exception, message);
		}


		public global::Serilog.LoggerConfiguration UpdateLoggerConfiguration(global::Serilog.LoggerConfiguration loggerConfiguration, IServiceProvider serviceProvider, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot, string environment)
		{
			loggerConfiguration ??= new();

			loggerConfiguration
				.MinimumLevel.Verbose()
				.MinimumLevel.Override("Microsoft", global::Serilog.Events.LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft", global::Serilog.Events.LogEventLevel.Warning)
				.MinimumLevel.Override("System", global::Serilog.Events.LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", global::Serilog.Events.LogEventLevel.Information)
				.Enrich.FromLogContext()
				.Enrich.WithEnvironmentUserName()
				.Enrich.WithMachineName()
				.Enrich.WithProcessId()
				.Enrich.WithThreadId()
				.Enrich.WithExceptionDetails()
				.Enrich.WithProperty("Environment", environment)
				.ReadFrom.Configuration(configurationRoot)
				;

			if (serviceProvider != null)
			{
				loggerConfiguration.ReadFrom.Services(serviceProvider);
			}

			loggerConfiguration.WriteTo.Console();

			var platformsConfiguration = new ISI.Platforms.Configuration();
			configurationRoot.Bind(ISI.Platforms.Configuration.ConfigurationSectionName, platformsConfiguration);

			var elkConfiguration = new ISI.Platforms.Elk.Configuration();
			configurationRoot.Bind(ISI.Platforms.Elk.Configuration.ConfigurationSectionName, elkConfiguration);

			if (!string.IsNullOrWhiteSpace(elkConfiguration?.NodeUrl))
			{
				loggerConfiguration.WriteTo.Elasticsearch(new global::Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elkConfiguration.NodeUrl))
				{
					AutoRegisterTemplate = true,
					AutoRegisterTemplateVersion = global::Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv6,
					IndexFormat = elkConfiguration.IndexFormat,
					ModifyConnectionSettings = connectionConfiguration =>
					{
						if(!string.IsNullOrWhiteSpace(elkConfiguration.Password))
						{
							connectionConfiguration.BasicAuthentication(elkConfiguration.UserName, elkConfiguration.Password);
						}

						if(!string.IsNullOrWhiteSpace(elkConfiguration.ApiKey))
						{
							connectionConfiguration.ApiKeyAuthentication(elkConfiguration.ApiKeyId, elkConfiguration.ApiKey);
						}

						return connectionConfiguration;
					},
				});
			}

			Console.WriteLine($"LogDirectory => {platformsConfiguration?.LogDirectory}");

			if (!string.IsNullOrWhiteSpace(platformsConfiguration?.LogDirectory))
			{
				loggerConfiguration.WriteTo.File(System.IO.Path.Combine(platformsConfiguration.LogDirectory, "log.txt"), rollingInterval: RollingInterval.Day);
			}

			return loggerConfiguration;
		}
	}
}
