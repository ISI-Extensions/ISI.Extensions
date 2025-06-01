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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.ServiceApplication
{
	public class MessageBusBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
	{
		protected ServiceApplicationContext Context { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		private static ISI.Extensions.MessageBus.IMessageBus _messageBus;

		public MessageBusBackgroundService(
			ServiceApplicationContext context,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Context = context;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}

		public async Task StartAsync(Microsoft.Extensions.Hosting.IHost host, System.Threading.CancellationToken cancellationToken)
		{
			Context.PreMessageBusBuild?.Invoke(host);

			_messageBus = host.Services.GetRequiredService<ISI.Extensions.MessageBus.IMessageBus>();

			_messageBus.Build(host.Services, new ISI.Extensions.MessageBus.MessageBusBuildRequestCollection()
			{
				Context.GetAddMessageBusSubscriptions,
			});

			_messageBus.StartAsync(cancellationToken);
		}

		protected override async Task ExecuteAsync(System.Threading.CancellationToken cancellationToken)
		{
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					try
					{
						await Task.Delay(1000, cancellationToken);
					}
					catch
					{
					}
				}

				if (_messageBus != null)
				{
					await _messageBus.StopAsync(cancellationToken);

					_messageBus?.Dispose();
				}
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, exception.ErrorMessageFormatted());

				// Terminates this process and returns an exit code to the operating system.
				// This is required to avoid the 'BackgroundServiceExceptionBehavior', which
				// performs one of two scenarios:
				// 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
				// 2. When set to "StopHost": will cleanly stop the host, and log errors.
				//
				// In order for the Windows Service Management system to leverage configured
				// recovery options, we need to terminate the process with a non-zero exit code.
				Environment.Exit(1);
			}
		}
	}
}
