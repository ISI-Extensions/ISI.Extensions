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
using ISI.Extensions.AspNetCore.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.MessageBus.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.ServiceApplication
{
	public class ServiceManager
	{
		private Microsoft.Extensions.Hosting.IHost _host;
		private ISI.Extensions.MessageBus.IMessageBus _messageBus;

		public async Task<bool> StartAsync()
		{
			var hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

			Startup.Context.LoggerConfigurator.AddLogger(hostBuilder, Startup.Context.ConfigurationRoot, Startup.Context.ActiveEnvironment);

			hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
			{
				var configuration = Startup.Context.ConfigurationRoot.GetConfiguration<ISI.Platforms.Configuration>();

				webHostBuilder.UseSetting(WebHostDefaults.ApplicationKey, Startup.Context.RootAssembly.FullName);

				webHostBuilder.UseStartup<WebStartup>();

				webHostBuilder.ConfigureServices(services =>
				{
					services
						.AddOptions()
						.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(Startup.Context.ConfigurationRoot)
						.AddAllConfigurations(Startup.Context.ConfigurationRoot)
						.AddConfiguration<Microsoft.Extensions.Hosting.ConsoleLifetimeOptions>(Startup.Context.ConfigurationRoot)
						.AddConfiguration<Microsoft.Extensions.Hosting.HostOptions>(Startup.Context.ConfigurationRoot)

						.AddConfigurationRegistrations(Startup.Context.ConfigurationRoot)
						.ProcessServiceRegistrars(Startup.Context.ConfigurationRoot)

						.AddTransient<Microsoft.Extensions.Logging.ILogger>(serviceProvider => serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger<Startup>())
						.AddSingleton<Microsoft.Extensions.FileProviders.IFileProvider>(provider => provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.StaticFileOptions>>().Value.FileProvider)
						;

					if (configuration.UseMessageBus)
					{
						services.AddMessageBus(Startup.Context.ConfigurationRoot);
					}

					services
						.AddSingleton<ISI.Extensions.Security.IPermissionProcessor, ISI.Extensions.Security.DefaultPermissionProcessor>()

						.AddSingleton<RoutingHelper>()

						.ProcessMigrationSteps()
						;
				});

				webHostBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());

				webHostBuilder.UseKestrel((builderContext, kestrelOptions) =>
				{
					var kestrelConfiguration = Startup.Context.ConfigurationRoot.GetKestrelConfigurationSection();

					kestrelOptions.Configure(kestrelConfiguration, reloadOnChange: false);
				});
			});

			_host = hostBuilder.Build();

			await _host.StartAsync().ContinueWith(_ =>
			{
				var server = _host.Services.GetRequiredService<global::Microsoft.AspNetCore.Hosting.Server.IServer>();
				var addressFeature = server.Features.Get<global::Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

				var logMessageBuilder = new System.Text.StringBuilder();
				logMessageBuilder.AppendLine("Address(es)");
				foreach (var address in addressFeature.Addresses)
				{
					logMessageBuilder.AppendLine($"  {address}");
					RoutingHelper.SetBaseUrl(address);
				}
				Startup.Context.LoggerConfigurator.Information(logMessageBuilder.ToString());

				var configuration = Startup.Context.ConfigurationRoot.GetConfiguration<ISI.Platforms.Configuration>();

				if (configuration.UseMessageBus)
				{
					_messageBus = _host.Services.GetRequiredService<ISI.Extensions.MessageBus.IMessageBus>();
					var enterpriseCacheManagerApi = _host.Services.GetService<ISI.Extensions.Caching.IEnterpriseCacheManagerApi>();

					_messageBus.Build(_host.Services, new ISI.Extensions.MessageBus.MessageBusBuildRequestCollection()
					{
						Startup.Context.GetAddSubscriptions,
						enterpriseCacheManagerApi.GetAddSubscriptions,
					});

					_messageBus.StartAsync();
				}

				Startup.Context.PostStartup?.Invoke(_host);

				return _host.Services.SetServiceLocator();
			});

			return true;
		}

		public async Task<bool> StopAsync()
		{
			await _host.StopAsync();

			if (_messageBus != null)
			{
				await _messageBus.StopAsync();
			}

			ISI.Extensions.Threads.ExitAll();

			_host.Dispose();
			_messageBus?.Dispose();

			return true;
		}
	}
}
