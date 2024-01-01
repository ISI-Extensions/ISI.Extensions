#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.AspNetCore.Tests
{
	public class LoggerConfigurator
	{
		public static Serilog.LoggerConfiguration UpdateLoggerConfiguration(Serilog.LoggerConfiguration loggerConfiguration, IServiceProvider serviceProvider, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment)
		{
			loggerConfiguration ??= new();

			loggerConfiguration
				.MinimumLevel.Verbose()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
				.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", Serilog.Events.LogEventLevel.Information)
				.Enrich.FromLogContext()
				.Enrich.WithEnvironmentUserName()
				.Enrich.WithMachineName()
				.Enrich.WithProcessId()
				.Enrich.WithThreadId()
				.Enrich.WithExceptionDetails()
				.Enrich.WithProperty("Environment", environment)
				.ReadFrom.Configuration(configuration)
				;

			if (serviceProvider != null)
			{
				loggerConfiguration.ReadFrom.Services(serviceProvider);
			}

			loggerConfiguration.WriteTo.Console();

			var webApplicationConfiguration = new ISI.Extensions.AspNetCore.Tests.Configuration();
			configuration.Bind(ISI.Extensions.AspNetCore.Tests.Configuration.ConfigurationSectionName, webApplicationConfiguration);

			if (!string.IsNullOrWhiteSpace(webApplicationConfiguration?.ElasticsearchLogging?.NodeUrl))
			{
				loggerConfiguration.WriteTo.Elasticsearch(new(new Uri(webApplicationConfiguration.ElasticsearchLogging.NodeUrl))
				{
					AutoRegisterTemplate = true,
					AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv6,
					IndexFormat = "custom-index-{0:yyyy.MM}",
					ModifyConnectionSettings = connectionConfiguration => connectionConfiguration.BasicAuthentication(webApplicationConfiguration.ElasticsearchLogging.UserName, webApplicationConfiguration.ElasticsearchLogging.Password),
				});
			}

			if (!string.IsNullOrWhiteSpace(webApplicationConfiguration?.LogDirectory))
			{
				loggerConfiguration.WriteTo.File(System.IO.Path.Combine(webApplicationConfiguration.LogDirectory, "log.txt"), rollingInterval: RollingInterval.Day);
			}

			return loggerConfiguration;
		}
	}
}
