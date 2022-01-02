#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ISI.Extensions.MessageBus.MassTransit
{
	public class MessageBusConfigurator : ISI.Extensions.MessageBus.IMessageBusConfigurator
	{
		private System.IServiceProvider _serviceProvider;
		private readonly global::MassTransit.IReceiveEndpointConfigurator _receiveEndpointConfigurator;

		public MessageBusConfigurator(System.IServiceProvider serviceProvider, global::MassTransit.IReceiveEndpointConfigurator receiveEndpointConfigurator)
		{
			_serviceProvider = serviceProvider;
			_receiveEndpointConfigurator = receiveEndpointConfigurator;
		}
		public void Subscribe<TController, TRequest, TResponse>(Func<TController, TRequest, Task<TResponse>> processor, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ControllerConsumerAsync<TController, TRequest, TResponse>), type => new ControllerConsumerAsync<TController, TRequest, TResponse>(_serviceProvider.GetService<TController>, processor, onError));
		}

		public void Subscribe<TController, TRequest>(Func<TController, TRequest, Task> processor, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ControllerConsumerAsync<TController, TRequest>), type => new ControllerConsumerAsync<TController, TRequest>(_serviceProvider.GetService<TController>, processor, onError));
		}


		public void Subscribe<TController, TRequest, TResponse>(Func<TController> getController, Func<TController, TRequest, Task<TResponse>> processor, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TController : class
			where TRequest : class
			where TResponse : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ControllerConsumerAsync<TController, TRequest, TResponse>), type => new ControllerConsumerAsync<TController, TRequest, TResponse>(getController, processor, onError));
		}

		public void Subscribe<TController, TRequest>(Func<TController> getController, Func<TController, TRequest, Task> processor, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TController : class
			where TRequest : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ControllerConsumerAsync<TController, TRequest>), type => new ControllerConsumerAsync<TController, TRequest>(getController, processor, onError));
		}





		
		public void Subscribe<TRequest, TResponse>(Func<TRequest, Task<TResponse>> processor, ISI.Extensions.MessageBus.OnError<TRequest, TResponse> onError = null)
			where TRequest : class
			where TResponse : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ConsumerAsync<TRequest, TResponse>), type => new ConsumerAsync<TRequest, TResponse>(processor, onError));
		}

		public void Subscribe<TRequest>(Func<TRequest, Task> processor, ISI.Extensions.MessageBus.OnError<TRequest> onError = null)
			where TRequest : class
		{
			_receiveEndpointConfigurator.Consumer(typeof(ConsumerAsync<TRequest>), type => new ConsumerAsync<TRequest>(processor, onError));
		}

	}
}
