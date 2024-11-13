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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.ServiceApplication
{
	public class AspNetCoreBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
	{
		protected ServiceApplicationContext Context { get; }
		protected ISI.Platforms.Configuration Configuration { get; }
		protected IServiceProvider ServiceProvider { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public AspNetCoreBackgroundService(
			ServiceApplicationContext context,
			ISI.Platforms.Configuration configuration,
			IServiceProvider serviceProvider,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Context = context;
			Configuration = configuration;
			ServiceProvider = serviceProvider;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}

		protected override async Task ExecuteAsync(System.Threading.CancellationToken cancellationToken)
		{
			await Task.Delay(5000, cancellationToken);

			var server = ServiceProvider.GetRequiredService<global::Microsoft.AspNetCore.Hosting.Server.IServer>();
			var addressFeature = server.Features.Get<global::Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

			var logMessageBuilder = new System.Text.StringBuilder();
			logMessageBuilder.AppendLine("Address(es)");
			if (string.IsNullOrWhiteSpace(Configuration.BaseUrl))
			{
				foreach (var address in addressFeature.Addresses)
				{
					logMessageBuilder.AppendLine($"  {address}");
					RoutingHelper.SetBaseUrl(address);
				}
			}
			else
			{
				foreach (var address in addressFeature.Addresses)
				{
					logMessageBuilder.AppendLine($"  internal {address}");
				}

				logMessageBuilder.AppendLine($"  {Configuration.BaseUrl}");
				RoutingHelper.SetBaseUrl(Configuration.BaseUrl);
			}

			Logger.LogInformation(logMessageBuilder.ToString());

			if (Configuration.UseMessageBus)
			{
				var messageBusBackgroundService = ServiceProvider.GetRequiredService<MessageBusBackgroundService>();

				messageBusBackgroundService.StartAsync(Context.Host, cancellationToken);
			}

			Context.PostStartup?.Invoke(Context.Host);

			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(1000, cancellationToken);
			}
			
			ISI.Extensions.Threads.ExitAll();
		}
	}
}
