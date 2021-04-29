#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.MessageBus.AzureServiceBus
{
	public partial class MessageBus
	{
		public override async Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, TRequest request, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
			where TResponse : class
		{
			return await PublishAsync<TRequest, TResponse>(channelName, typeof(TRequest), request, timeout, timeToLive, cancellationToken);
		}

		public override async Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, Type requestType, TRequest request, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
			where TResponse : class
		{
			var response = (TResponse)null;

			var responseType = typeof(TResponse);

			var responseChannelName = string.Format("{0}-{1}", responseType.FullName, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting));

			if (string.IsNullOrWhiteSpace(channelName))
			{
				channelName = GetChannelName<TRequest>();
			}

			var messageEnvelope = GetMessageEnvelope(request, timeout ?? Configuration.DefaultResponseTimeOut, responseChannelName);

			var temporaryQueueDescription = new Microsoft.Azure.ServiceBus.Management.QueueDescription(responseChannelName)
			{
				AutoDeleteOnIdle = timeout ?? Configuration.DefaultResponseTimeOut,
			};

			try
			{
				await ManagementClient.CreateQueueAsync(temporaryQueueDescription, cancellationToken);

				var requestClient = new Microsoft.Azure.ServiceBus.QueueClient(Connection, channelName, default, Microsoft.Azure.ServiceBus.RetryPolicy.NoRetry);
				//var requestClient = new Microsoft.Azure.ServiceBus.QueueClient(Connection, channelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, new Microsoft.Azure.ServiceBus.RetryExponential(TimeSpan.Zero, TimeSpan.FromSeconds(30.0), Configuration.DefaultChannel.RetryLimit));

				var receiverClient = new Microsoft.Azure.ServiceBus.Core.MessageReceiver(Connection, responseChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.ReceiveAndDelete);

				await requestClient.SendAsync(messageEnvelope);

				var requestResponse = await receiverClient.ReceiveAsync(timeout ?? Configuration.DefaultResponseTimeOut);

				if (requestResponse?.Body != null)
				{
					response = JsonSerializer.Deserialize(responseType, Encoding.UTF8.GetString(requestResponse.Body)) as TResponse;
				}
			}
			catch (Exception exception)
			{

			}
			finally
			{
				await ManagementClient.DeleteQueueAsync(responseChannelName, cancellationToken);
			}

			return response;
		}
	}
}