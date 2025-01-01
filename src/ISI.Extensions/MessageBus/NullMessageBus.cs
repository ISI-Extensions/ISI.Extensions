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

namespace ISI.Extensions.MessageBus
{
	public class NullMessageBus : IMessageBus
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Build(IServiceProvider serviceProvider)
		{
			throw new NotImplementedException();
		}

		public void Build(IServiceProvider serviceProvider, Action<IMessageBusConfigurator> addSubscriptions)
		{
			throw new NotImplementedException();
		}

		public void Build(IServiceProvider serviceProvider, MessageBusBuildRequestCollection requests)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task PublishAsync<TRequest>(TRequest request, MessageBusMessageHeaderCollection headers = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task PublishAsync<TRequest>(string channelName, TRequest request, MessageBusMessageHeaderCollection headers = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<TResponse> PublishAsync<TRequest, TResponse>(TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<TResponse> PublishAsync<TRequest, TResponse>(Type requestType, TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, Type requestType, TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
		{
			throw new NotImplementedException();
		}
	}
}
