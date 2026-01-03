#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using global::MassTransit;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.MessageBus.MassTransit.RabbitMQ
{
	public partial class MessageBus
	{
		private void CreateChannel(System.IServiceProvider serviceProvider, global::MassTransit.IRabbitMqBusFactoryConfigurator configurator, Uri hostUri, ISI.Extensions.MessageBus.MessageBusBuildRequest messageBusBuildRequest)
		{
			if (messageBusBuildRequest.AddSubscriptions.NullCheckedAny())
			{
				configurator.ReceiveEndpoint(messageBusBuildRequest.ChannelPath, endPointConfigurator =>
				{
					if (messageBusBuildRequest.ConcurrentConsumerLimit.HasValue)
					{
						endPointConfigurator.UseConcurrencyLimit(messageBusBuildRequest.ConcurrentConsumerLimit.Value);
					}

					if (messageBusBuildRequest.RetryLimit > 0)
					{
						endPointConfigurator.UseMessageRetry(r => r.Interval(messageBusBuildRequest.RetryLimit, messageBusBuildRequest.RetryInterval));
					}
					else
					{
						endPointConfigurator.UseMessageRetry(r => r.None());
					}

					endPointConfigurator.AutoDelete = messageBusBuildRequest.AutoDelete;

					endPointConfigurator.PurgeOnStartup = messageBusBuildRequest.PurgeOnStartup;

					var uriBuilder = new UriBuilder(hostUri)
					{
						Path = messageBusBuildRequest.ChannelPath,
					};

					foreach (var channelName in messageBusBuildRequest.ChannelNames)
					{
						if (!string.IsNullOrWhiteSpace(channelName))
						{
							ChannelUriLookup.Add(channelName, uriBuilder.Uri);
						}
					}

					endPointConfigurator.UseInMemoryOutbox();

					var messageBusQueueConfigurator = new MessageBusConfigurator(serviceProvider, endPointConfigurator);

					foreach (var addSubscription in messageBusBuildRequest.AddSubscriptions)
					{
						addSubscription(messageBusQueueConfigurator);
					}
				});
			}
		}
	}
}