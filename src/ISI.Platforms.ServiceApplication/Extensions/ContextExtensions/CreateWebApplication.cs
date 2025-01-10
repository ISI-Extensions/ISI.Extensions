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
		public static Microsoft.AspNetCore.Builder.WebApplication CreateWebApplication(this ServiceApplicationContext context)
		{
			var webApplicationOptions = new Microsoft.AspNetCore.Builder.WebApplicationOptions()
			{
				Args = context.Args,
				//EnvironmentName = source.EnvironmentName,
				ApplicationName = context.ServiceName,
			};

			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				var contentRootPath = System.IO.Directory.GetCurrentDirectory();
				var webRootPath = System.IO.Path.Combine(contentRootPath, "wwwroot");

				webApplicationOptions = new Microsoft.AspNetCore.Builder.WebApplicationOptions()
				{
					Args = context.Args,
					//EnvironmentName = source.EnvironmentName,
					ApplicationName = context.ServiceName,
					ContentRootPath = contentRootPath,
					WebRootPath = webRootPath,
				};
			}

			var webApplicationBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(webApplicationOptions);

			webApplicationBuilder.Services.AddSingleton(context);

			var configuration = context.ConfigurationRoot.GetConfiguration<ISI.Platforms.Configuration>();

			webApplicationBuilder.Services.AddSingleton<AspNetCoreBackgroundService>();
			webApplicationBuilder.Services.AddHostedService<AspNetCoreBackgroundService>();

			if (configuration.UseMessageBus)
			{
				webApplicationBuilder.Services.AddSingleton<MessageBusBackgroundService>();
				webApplicationBuilder.Services.AddHostedService<MessageBusBackgroundService>();
			}

			context.LoggerConfigurator?.AddLogger(webApplicationBuilder.Services, context.ConfigurationRoot, context.ActiveEnvironment);

			context.LoggerConfigurator?.Information($"System.IO.Directory.GetCurrentDirectory() = {System.IO.Directory.GetCurrentDirectory()}");
			
			if (context.RunningAsService)
			{
				context.LoggerConfigurator?.Information("Running As Service");

				if (Environment.OSVersion.Platform == PlatformID.Unix)
				{
					context.LoggerConfigurator?.Information("AddSystemd()");
					webApplicationBuilder.Services.AddSystemd();
				}
				else
				{
					context.LoggerConfigurator?.Information("AddWindowsService()");
					webApplicationBuilder.Services.AddWindowsService(options =>
					{
						options.ServiceName = context.ServiceName;
					});
				}
			}

			context.HostBuilderConfigureServices?.Invoke(webApplicationBuilder);

			var mvcBuilder = webApplicationBuilder.Services
					.AddControllersWithViews()
					.AddApplicationPart(context.RootAssembly)
					.AddISIExtensionsAspNetCore()
					//.AddRazorRuntimeCompilation(options => options.FileProviders.Add(new ISI.Extensions.VirtualFileVolumesFileProvider()))
					.AddNewtonsoftJson(options =>
					{
						options.SerializerSettings.Converters = ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer.JsonConverters();
						options.SerializerSettings.DateParseHandling = global::Newtonsoft.Json.DateParseHandling.None;
					})
				;

			if (context.AddSignalR)
			{
				webApplicationBuilder.Services.AddSignalR();
			}

			context.WebStartupMvcBuilder?.Invoke(mvcBuilder);
			context.WebStartupConfigureServices?.Invoke(webApplicationBuilder.Services);

			context.LoggerConfigurator.AddRequestLogging(webApplicationBuilder);

			webApplicationBuilder.Services
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(context.ConfigurationRoot)
				.AddAllConfigurations(context.ConfigurationRoot)
				.AddConfiguration<Microsoft.Extensions.Hosting.ConsoleLifetimeOptions>(context.ConfigurationRoot)
				.AddConfiguration<Microsoft.Extensions.Hosting.HostOptions>(context.ConfigurationRoot)

				.AddConfigurationRegistrations(context.ConfigurationRoot)
				.ProcessServiceRegistrars(context.ConfigurationRoot)

				.AddTransient<Microsoft.Extensions.Logging.ILogger>(serviceProvider => serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger<ServiceApplicationContext>())
				.AddSingleton<Microsoft.Extensions.FileProviders.IFileProvider>(serviceProvider => serviceProvider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.StaticFileOptions>>().Value.FileProvider)
				;

			if (configuration.UseMessageBus)
			{
				webApplicationBuilder.Services.AddMessageBus(context.ConfigurationRoot);
			}

			webApplicationBuilder.Services
				.AddSingleton<ISI.Extensions.Security.IPermissionProcessor, ISI.Extensions.Security.DefaultPermissionProcessor>()

				.AddSingleton<RoutingHelper>()

				.ProcessMigrationSteps()
				;

			webApplicationBuilder.WebHost.UseKestrel((builderContext, kestrelOptions) =>
			{
				var kestrelConfiguration = context.ConfigurationRoot.GetKestrelConfigurationSection();

				kestrelOptions.Configure(kestrelConfiguration, reloadOnChange: false);
			});

			var webApplication = webApplicationBuilder.Build();

			context.Host = webApplication;

			webApplication.Services.SetServiceLocator();

			context.TraceListener = new ISI.Extensions.Logging.LoggerTraceListener(webApplication.Services.GetRequiredService<ILogger>());
			System.Diagnostics.Trace.Listeners.Add(context.TraceListener);
			//TraceSources.Instance.InitLoggerTraceListener(context.TraceListener);

			return webApplication;
		}
	}
}
