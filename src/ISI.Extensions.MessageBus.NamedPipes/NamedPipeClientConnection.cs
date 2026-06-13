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
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.MessageBus.NamedPipes
{
	public class NamedPipeClientConnection : NamedPipeConnection<System.IO.Pipes.NamedPipeClientStream>
	{
		public event EventHandler OnClientStarted;
		public event EventHandler OnConnectedToServer;

		protected string ServerName { get; }
		
		public NamedPipeClientConnection(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string serverName,
			string pipeName,
			OnRequestResponseMessageReceivedDelegate onMessageReceived = null) 
			: base(jsonSerializer, pipeName, onMessageReceived)
		{
			ServerName = serverName;
		}

		public override async Task ConnectAsync()
		{
			Initialize(new System.IO.Pipes.NamedPipeClientStream(ServerName, PipeName, System.IO.Pipes.PipeDirection.InOut, System.IO.Pipes.PipeOptions.Asynchronous));

			try
			{
				OnClientStarted?.Invoke(this, EventArgs.Empty);

				await PipeStream.ConnectAsync();
				
				PipeStream.ReadMode = System.IO.Pipes.PipeTransmissionMode.Message;

				OnConnectedToServer?.Invoke(this, EventArgs.Empty);

				await StartReadingAsync();
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		public override void Dispose()
		{
			PipeStream?.Dispose();
		}
	}
}
