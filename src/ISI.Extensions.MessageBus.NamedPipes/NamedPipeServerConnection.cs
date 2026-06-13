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
	public class NamedPipeServerConnection : NamedPipeConnection<System.IO.Pipes.NamedPipeServerStream>
	{
		public event EventHandler OnServerStarted;
		public event EventHandler OnClientConnected;

		public NamedPipeServerConnection(
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			string pipeName,
			OnRequestResponseMessageReceivedDelegate onMessageReceived = null)
			: base(jsonSerializer, pipeName, onMessageReceived)
		{
			
		}

		public override async Task ConnectAsync()
		{
			Initialize(new System.IO.Pipes.NamedPipeServerStream(PipeName, System.IO.Pipes.PipeDirection.InOut, 1, System.IO.Pipes.PipeTransmissionMode.Message, System.IO.Pipes.PipeOptions.Asynchronous));

			try
			{
				PipeStream.BeginWaitForConnection(WaitForConnectionCallBack, null);

				OnServerStarted?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}

		private void WaitForConnectionCallBack(IAsyncResult result)
		{
			PipeStream.EndWaitForConnection(result);

			OnClientConnected?.Invoke(this, EventArgs.Empty);

			StartReadingAsync().GetAwaiter().GetResult();
		}

		public override void Dispose()
		{
			PipeStream?.Disconnect();
			PipeStream?.Dispose();
		}
	}
}
