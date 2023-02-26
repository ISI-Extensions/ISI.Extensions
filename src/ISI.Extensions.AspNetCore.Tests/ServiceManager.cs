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
using System.Threading.Tasks;
using ISI.Extensions.AspNetCore.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ISI.Extensions.AspNetCore.Tests
{
	public class ServiceManager
	{
		private Microsoft.Extensions.Hosting.IHost _webHost;

		public async Task<bool> StartAsync(Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot, string environment, string[] args)
		{
			_webHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
				.UseSerilog((context, services, loggerConfiguration) => LoggerConfigurator.UpdateLoggerConfiguration(loggerConfiguration, services, configurationRoot, environment))
				.ConfigureWebHostDefaults(webHostBuilder =>
				{
					webHostBuilder.UseStartup<Startup>();

					webHostBuilder.ConfigureServices(services =>
						{
							services
								.AddOptions()
								.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configurationRoot)
								.AddAllConfigurations(configurationRoot)
								.AddConfiguration<Microsoft.Extensions.Hosting.ConsoleLifetimeOptions>(configurationRoot)
								.AddConfiguration<Microsoft.Extensions.Hosting.HostOptions>(configurationRoot)
								.AddConfigurationRegistrations(configurationRoot)
								.ProcessServiceRegistrars()

								//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
								//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
								//.AddLogging(builder => builder
								//	.AddConsole()
								////.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
								//)
								.AddTransient<Microsoft.Extensions.Logging.ILogger>(serviceProvider => serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger<Program>())
								.AddSingleton<Microsoft.Extensions.FileProviders.IFileProvider>(provider => provider.GetService<Microsoft.Extensions.Options.IOptions<Microsoft.AspNetCore.Builder.StaticFileOptions>>().Value.FileProvider)

								.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache>(provider => new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()))
								.AddSingleton<ISI.Extensions.Caching.ICacheManager, ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.IMemoryCache>>()
							;
						});

					webHostBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());

					webHostBuilder.ConfigureKestrel(serverOptions =>
					{
						serverOptions.Limits.MaxConcurrentConnections = 100;
						serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
						serverOptions.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
						serverOptions.Limits.MinRequestBodyDataRate = new Microsoft.AspNetCore.Server.Kestrel.Core.MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
						serverOptions.Limits.MinResponseDataRate = new Microsoft.AspNetCore.Server.Kestrel.Core.MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
						serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
						serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
					});

					//webHostBuilder.UseKestrel();
					webHostBuilder.UseKestrel((builderContext, kestrelOptions) =>
					{
						var kestrelConfiguration = configurationRoot.GetKestrelConfigurationSection();

						kestrelOptions.Configure(kestrelConfiguration, reloadOnChange: false);
					});
				})

				//.UseServiceProviderFactory(new ISI.Extensions.DependencyInjection.ServiceProviderFactory(configurationRoot))

				.Build();


			await _webHost.StartAsync().ContinueWith(_ =>
			{
				var server = _webHost.Services.GetRequiredService<global::Microsoft.AspNetCore.Hosting.Server.IServer>();
				var addressFeature = server.Features.Get<global::Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

				Serilog.Log.Information("Address(es)");
				foreach (var address in addressFeature.Addresses)
				{
					Serilog.Log.Information($"  {address}");
				}

				return _webHost.Services.SetServiceLocator();
			});

			return true;
		}

		public async Task<bool> StopAsync()
		{
			await _webHost.StopAsync();

			_webHost.Dispose();

			return true;
		}
	}
}
