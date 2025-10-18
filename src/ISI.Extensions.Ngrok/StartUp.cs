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
using System.Text;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.Ngrok.Extensions;

namespace ISI.Extensions.Ngrok
{
	public class StartUp : Microsoft.Extensions.Hosting.IHostedService
	{
		protected IServiceProvider ServiceProvider { get; }
		protected Configuration Configuration { get; }
		protected ILogger<StartUp> Logger { get; }
		protected Microsoft.Extensions.Hosting.IHostApplicationLifetime HostApplicationLifetime { get; }
		protected INGrokLocalServiceApi NGrokLocalServiceApi { get; }

		private ISI.Extensions.Ngrok.DataTransferObjects.NGrokLocalServiceApi.Tunnel[] _tunnels = null;

		public StartUp(
			IServiceProvider serviceProvider,
			Configuration configuration,
			ILogger<StartUp> logger,
			Microsoft.Extensions.Hosting.IHostApplicationLifetime hostApplicationLifetime,
			INGrokLocalServiceApi nGrokLocalServiceApi)
		{
			ServiceProvider = serviceProvider;
			Configuration = configuration;
			Logger = logger;
			HostApplicationLifetime = hostApplicationLifetime;
			NGrokLocalServiceApi = nGrokLocalServiceApi;

			serviceProvider.SetServiceLocator();
		}

		public System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken)
		{
			HostApplicationLifetime.ApplicationStarted.Register(OnStarted);
			HostApplicationLifetime.ApplicationStopping.Register(OnStopping);

			return System.Threading.Tasks.Task.CompletedTask;
		}

		public System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken)
		{
			return System.Threading.Tasks.Task.CompletedTask;
		}

		private void OnStarted()
		{
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			var server = ServiceProvider.GetRequiredService<global::Microsoft.AspNetCore.Hosting.Server.IServer>();
			var addressFeature = server.Features.Get<global::Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

			_tunnels = NGrokLocalServiceApi.CreateReplacementUrls(addressFeature.Addresses).Tunnels.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			foreach (var tunnel in _tunnels)
			{
				Logger.Log(LogLevel.Information, $"NGork tunnel created \"{tunnel.ExternalUrl}\" => \"{tunnel.LocalUrl}\"");
			}
		}

		private void OnStopping()
		{
			foreach (var tunnel in _tunnels.NullCheckedWhere(tunnel => tunnel.NewTunnel))
			{
				NGrokLocalServiceApi.StopTunnels(new()
				{
					TunnelNames = [tunnel.TunnelName],
				});

				Logger.Log(LogLevel.Information, $"NGork tunnel stopped \"{tunnel.ExternalUrl}\" => \"{tunnel.LocalUrl}\"");

				tunnel.NewTunnel = false;
			}
		}
	}
}