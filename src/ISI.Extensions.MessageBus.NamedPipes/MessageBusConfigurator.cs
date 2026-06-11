#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

NamedPipestribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* NamedPipestributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* NamedPipestributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
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

namespace ISI.Extensions.MessageBus.NamedPipes
{
	public class MessageBusConfigurator : ISI.Extensions.MessageBus.IMessageBusConfigurator
	{
		private readonly System.IServiceProvider _serviceProvider;
		private readonly NamedPipeConnectionManager _connectionManager;
		private readonly Func<Type, string> _getChannelName;
		private readonly ISI.Extensions.JsonSerialization.IJsonSerializer _jsonSerializer;

		public MessageBusConfigurator(
			System.IServiceProvider serviceProvider,
			NamedPipeConnectionManager connectionManager,
			Func<Type, string> getChannelName,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			_serviceProvider = serviceProvider;
			_connectionManager = connectionManager;
			_getChannelName = getChannelName;
			_jsonSerializer = jsonSerializer;
		}

		public void Subscribe<TController, TRequest, TResponse>(ControllerMessageBusConfigurator<TController, TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageEnvelope) =>
			{
				var handler = new ControllerConsumerAsync<TController, TRequest, TResponse>(_serviceProvider, _connectionManager, _jsonSerializer, _serviceProvider.GetService<TController>, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageEnvelope).Wait();
			});
		}

		public void Subscribe<TController, TRequest>(ControllerMessageBusConfigurator<TController, TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageSerialized) =>
			{
				var handler = new ControllerConsumerAsync<TController, TRequest>(_serviceProvider, _connectionManager, _jsonSerializer, _serviceProvider.GetService<TController>, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageSerialized).Wait();
			});
		}


		public void Subscribe<TController, TRequest, TResponse>(GetControllerMessageBusConfigurator<TController>.GetControllerDelegate getController, ControllerMessageBusConfigurator<TController, TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageSerialized) =>
			{
				var handler = new ControllerConsumerAsync<TController, TRequest, TResponse>(_serviceProvider, _connectionManager, _jsonSerializer, _serviceProvider.GetService<TController>, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageSerialized).Wait();
			});
		}

		public void Subscribe<TController, TRequest>(GetControllerMessageBusConfigurator<TController>.GetControllerDelegate getController, ControllerMessageBusConfigurator<TController, TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageSerialized) =>
			{
				var handler = new ControllerConsumerAsync<TController, TRequest>(_serviceProvider, _connectionManager, _jsonSerializer, _serviceProvider.GetService<TController>, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageSerialized).Wait();
			});
		}






		public void Subscribe<TRequest, TResponse>(MessageBusConfigurator<TRequest, TResponse>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TRequest : class
			where TResponse : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageSerialized) =>
			{
				var handler = new ConsumerAsync<TRequest, TResponse>(_serviceProvider, _connectionManager, _jsonSerializer, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageSerialized).Wait();
			});
		}

		public void Subscribe<TRequest>(MessageBusConfigurator<TRequest>.ProcessorDelegate processor, IsAuthorizedDelegate isAuthorized = null, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TRequest : class
		{
			var requestChannelName = _getChannelName(typeof(TRequest));

			_connectionManager.Subscribe(requestChannelName, (namedPipeConnection, messageSerialized) =>
			{
				var handler = new ConsumerAsync<TRequest>(_serviceProvider, _connectionManager, _jsonSerializer, processor, isAuthorized, onError);

				handler.Consume(namedPipeConnection, messageSerialized).Wait();
			});
		}
	}
}
