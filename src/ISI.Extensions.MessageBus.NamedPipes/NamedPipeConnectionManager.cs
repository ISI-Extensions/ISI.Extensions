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

namespace ISI.Extensions.MessageBus.NamedPipes
{
	public delegate void OnRequestMessageReceivedDelegate(MessageEnvelope messageEnvelope);
	public delegate void OnRequestResponseMessageReceivedDelegate(INamedPipeConnection namedPipeConnection, MessageEnvelope messageEnvelope);
	
	public class NamedPipeConnectionManager
	{
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		protected string ConnectionString { get; }

		protected System.Collections.Concurrent.ConcurrentDictionary<string, INamedPipeConnection> NamedPipeConnectionsByChannelName { get; }

		public NamedPipeConnectionManager(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string connectionString)
		{
			JsonSerializer = jsonSerializer;
			ConnectionString = connectionString;
			NamedPipeConnectionsByChannelName = new System.Collections.Concurrent.ConcurrentDictionary<string, INamedPipeConnection>(StringComparer.InvariantCultureIgnoreCase);
		}

		protected INamedPipeConnection GetOrCreateNamedPipeConnection(string channelName, OnRequestResponseMessageReceivedDelegate onMessageReceived = null)
		{
			if (NamedPipeConnectionsByChannelName.TryGetValue(channelName, out var namedPipeConnection))
			{
				return namedPipeConnection;
			}
			
			if (string.IsNullOrWhiteSpace(ConnectionString))
			{
				namedPipeConnection = new NamedPipeServerConnection(JsonSerializer, channelName, onMessageReceived);
			}
			else
			{
				namedPipeConnection = new NamedPipeClientConnection(JsonSerializer, ConnectionString, channelName, onMessageReceived);
			}

			namedPipeConnection.ConnectAsync().GetAwaiter().GetResult();

			NamedPipeConnectionsByChannelName.TryAdd(channelName, namedPipeConnection);

			return namedPipeConnection;
		}

		public void Subscribe(string channelName, OnRequestResponseMessageReceivedDelegate onMessageReceived)
		{
			var namedPipeConnection = GetOrCreateNamedPipeConnection(channelName, onMessageReceived);
		}

		public async Task PublishAsync(string channelName, MessageEnvelope messageEnvelope, System.Threading.CancellationToken cancellationToken = default)
		{
			var namedPipeConnection = GetOrCreateNamedPipeConnection(channelName);

			await namedPipeConnection.SendAsync(messageEnvelope);
		}

		public async Task PublishAsync(string channelName, MessageEnvelope messageEnvelope, OnRequestMessageReceivedDelegate onMessageReceived, System.Threading.CancellationToken cancellationToken = default)
		{
			var namedPipeConnection = GetOrCreateNamedPipeConnection(channelName);

			await namedPipeConnection.SendAsync(messageEnvelope, onMessageReceived);
		}

		public async Task UnsubscribeAsync(string channelName, string messageKey, System.Threading.CancellationToken cancellationToken = default)
		{
			if (NamedPipeConnectionsByChannelName.TryGetValue(channelName, out var namedPipeConnection))
			{
				await namedPipeConnection.UnsubscribeAsync(messageKey);
			}
		}

		public async Task UnsubscribeAsync(string channelName, System.Threading.CancellationToken cancellationToken = default)
		{
			if (NamedPipeConnectionsByChannelName.TryRemove(channelName, out var namedPipeConnection))
			{
				namedPipeConnection.Dispose();
			}
		}

		public async Task UnsubscribeAllAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			foreach (var channelName in NamedPipeConnectionsByChannelName.Keys.ToArray())
			{
				await UnsubscribeAsync(channelName, cancellationToken);
			}
		}
	}
}
