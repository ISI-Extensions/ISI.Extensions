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
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.MessageBus.NamedPipes
{
	public interface INamedPipeConnection : IDisposable
	{
		Task SendAsync(MessageEnvelope messageEnvelope, OnRequestMessageReceivedDelegate onResponse = null);
		Task UnsubscribeAsync(string messageKey);
		Task ConnectAsync();
	}

	public abstract class NamedPipeConnection<T> : INamedPipeConnection
		where T : System.IO.Pipes.PipeStream
	{
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		protected System.Collections.Concurrent.ConcurrentDictionary<string, OnRequestMessageReceivedDelegate> OnResponseByMessageKey { get; }

		protected OnRequestResponseMessageReceivedDelegate OnMessageReceived { get; }

		protected string PipeName { get; }
		protected T PipeStream { get; set; }
		protected NamedPipeStream NamedPipeStream { get; set; }

		public NamedPipeConnection(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string pipeName,
			OnRequestResponseMessageReceivedDelegate onMessageReceived)
		{
			JsonSerializer = jsonSerializer;
			PipeName = pipeName;
			OnResponseByMessageKey = new System.Collections.Concurrent.ConcurrentDictionary<string, OnRequestMessageReceivedDelegate>(StringComparer.InvariantCultureIgnoreCase);
			OnMessageReceived = onMessageReceived;
		}

		public abstract Task ConnectAsync();

		protected void Initialize(T pipeStream)
		{
			PipeStream = pipeStream;

			NamedPipeStream = new NamedPipeStream(pipeStream);
		}

		protected async Task StartReadingAsync()
		{
			await Task.Factory.StartNew(async () =>
			{
				try
				{
					while (true)
					{
						var message = await NamedPipeStream.ReadString();

						await OnMessageReceivedAsync(message);
					}
				}
				catch (InvalidOperationException)
				{
					OnDisconnected();
					Dispose();
				}
			});
		}

		public async Task OnMessageReceivedAsync(string serializedMessageEnvelope)
		{
			var requestMessageEnvelope = JsonSerializer.Deserialize(typeof(MessageEnvelope), serializedMessageEnvelope) as MessageEnvelope;

			switch (requestMessageEnvelope.MessageDirection)
			{
				case MessageDirection.Request:
					OnMessageReceived?.Invoke(this, requestMessageEnvelope);
					break;
				case MessageDirection.Response:
					if (OnResponseByMessageKey.TryRemove(requestMessageEnvelope.MessageKey, out var onResponse))
					{
						onResponse(requestMessageEnvelope);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public async Task SendAsync(MessageEnvelope messageEnvelope, OnRequestMessageReceivedDelegate onResponse = null)
		{
			if ((onResponse != null) && !string.IsNullOrWhiteSpace(messageEnvelope.MessageKey))
			{
				OnResponseByMessageKey.TryAdd(messageEnvelope.MessageKey, onResponse);
			}

			var serializedMessageEnvelope = JsonSerializer.Serialize(typeof(MessageEnvelope), messageEnvelope, true);

			await NamedPipeStream.WriteString(serializedMessageEnvelope);
		}

		public async Task UnsubscribeAsync(string messageKey)
		{
			OnResponseByMessageKey.TryRemove(messageKey, out var onResponse);
		}

		public virtual void OnDisconnected() { }

		public abstract void Dispose();
	}
}
