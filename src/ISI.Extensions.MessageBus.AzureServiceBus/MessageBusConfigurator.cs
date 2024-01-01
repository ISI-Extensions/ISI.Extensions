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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ISI.Extensions.MessageBus.AzureServiceBus
{
	public class MessageBusConfigurator : ISI.Extensions.MessageBus.IMessageBusConfigurator
	{
		private readonly System.IServiceProvider _serviceProvider;
		private readonly MessageBusBuildRequest _messageBusBuildRequest;
		private readonly string _connectionString;
		private readonly Func<Type, string> _getChannelName;
		private readonly ISI.Extensions.JsonSerialization.IJsonSerializer _jsonSerializer;

		protected string TopicPath = nameof(TopicPath);

		public MessageBusConfigurator(
			System.IServiceProvider serviceProvider,
			MessageBusBuildRequest messageBusBuildRequest,
			string connectionString,
			Func<Type, string> getChannelName,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			_serviceProvider = serviceProvider;
			_messageBusBuildRequest = messageBusBuildRequest;
			_connectionString = connectionString;
			_getChannelName = getChannelName;
			_jsonSerializer = jsonSerializer;
		}

		protected void CreateQueue(string channelName)
		{
			var managementClient = new Microsoft.Azure.ServiceBus.Management.ManagementClient(_connectionString);

			var existingQueues =  managementClient.GetQueuesAsync().GetAwaiter().GetResult();

			if (!existingQueues.NullCheckedAny(queue => string.Equals(queue.Path, channelName, StringComparison.CurrentCultureIgnoreCase)))
			{
				managementClient.CreateQueueAsync(channelName).Wait();
			}
		}

		public void Subscribe<TController, TRequest, TResponse>(ControllerMessageBusConfigurator<TController, TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ControllerConsumerAsync<TController, TRequest, TResponse>(_serviceProvider, _connectionString, _jsonSerializer, _serviceProvider.GetService<TController>, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}

		public void Subscribe<TController, TRequest>(ControllerMessageBusConfigurator<TController, TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ControllerConsumerAsync<TController, TRequest>(_serviceProvider, _connectionString, _jsonSerializer, _serviceProvider.GetService<TController>, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}


		public void Subscribe<TController, TRequest, TResponse>(GetControllerMessageBusConfigurator<TController>.GetControllerDelegate getController, ControllerMessageBusConfigurator<TController, TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ControllerConsumerAsync<TController, TRequest, TResponse>(_serviceProvider, _connectionString, _jsonSerializer, _serviceProvider.GetService<TController>, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}

		public void Subscribe<TController, TRequest>(GetControllerMessageBusConfigurator<TController>.GetControllerDelegate getController, ControllerMessageBusConfigurator<TController, TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ControllerConsumerAsync<TController, TRequest>(_serviceProvider, _connectionString, _jsonSerializer, _serviceProvider.GetService<TController>, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}






		public void Subscribe<TRequest, TResponse>(MessageBusConfigurator<TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TRequest : class
			where TResponse : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ConsumerAsync<TRequest, TResponse>(_serviceProvider, _connectionString, _jsonSerializer, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}

		public void Subscribe<TRequest>(MessageBusConfigurator<TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TRequest : class
		{
			isAuthorized ??= (headers, request) => true;

			var requestChannelName = _getChannelName(typeof(TRequest));

			CreateQueue(requestChannelName);

			var queueClient = new Microsoft.Azure.ServiceBus.QueueClient(_connectionString, requestChannelName, Microsoft.Azure.ServiceBus.ReceiveMode.PeekLock, default);

			queueClient.RegisterMessageHandler(
				async (message, token) =>
				{
					var handler = new ConsumerAsync<TRequest>(_serviceProvider, _connectionString, _jsonSerializer, processor, onError);

					await handler.Consume(message);

					await queueClient.CompleteAsync(message.SystemProperties.LockToken);
				},
				new Microsoft.Azure.ServiceBus.MessageHandlerOptions(async args => Console.WriteLine(args.Exception))
				{
					MaxConcurrentCalls = _messageBusBuildRequest.ConcurrentConsumerLimit ?? 10,
					AutoComplete = false,
				});
		}
	}
}
