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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.AspNetCore.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.MessageBus.Extensions;
using ISI.Platforms.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.ServiceApplication.Extensions
{
	public static partial class ContextExtensions
	{
		public static Microsoft.Extensions.Hosting.IHost CreateApplication(this ServiceApplicationContext context)
		{
			var applicationBuilder = Host.CreateApplicationBuilder(context.Args);

			applicationBuilder.Services.AddSingleton(context);

			var configuration = context.ConfigurationRoot.GetConfiguration<ISI.Platforms.Configuration>();

			applicationBuilder.Services.AddHostedService<AspNetCoreBackgroundService>();

			if (configuration.UseMessageBus)
			{
				applicationBuilder.Services.AddHostedService<MessageBusBackgroundService>();
			}

			context.LoggerConfigurator?.AddLogger(applicationBuilder.Services, context.ConfigurationRoot, context.ActiveEnvironment);

			context.HostBuilderConfigureServices?.Invoke(applicationBuilder);

			applicationBuilder.Services
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(context.ConfigurationRoot)
				.AddAllConfigurations(context.ConfigurationRoot)
				.AddConfiguration<Microsoft.Extensions.Hosting.ConsoleLifetimeOptions>(context.ConfigurationRoot)
				.AddConfiguration<Microsoft.Extensions.Hosting.HostOptions>(context.ConfigurationRoot)

				.AddConfigurationRegistrations(context.ConfigurationRoot)
				.ProcessServiceRegistrars(context.ConfigurationRoot)

				.AddTransient<Microsoft.Extensions.Logging.ILogger>(serviceProvider => serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger<ServiceApplicationContext>())
				.AddSingleton<Microsoft.Extensions.FileProviders.IFileProvider>(provider => provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.StaticFileOptions>>().Value.FileProvider)
				;

			if (configuration.UseMessageBus)
			{
				applicationBuilder.Services.AddMessageBus(context.ConfigurationRoot);
			}

			var application = applicationBuilder.Build();

			context.Host = application;

			application.Services.SetServiceLocator();

			return application;
		}
	}
}
