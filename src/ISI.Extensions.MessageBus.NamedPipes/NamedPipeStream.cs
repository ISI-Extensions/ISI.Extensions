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

namespace ISI.Extensions.MessageBus.NamedPipes
{
	public class NamedPipeStream
	{
		protected System.IO.Pipes.PipeStream PipeStream { get; }

		public NamedPipeStream(
			System.IO.Pipes.PipeStream pipeStream)
		{
			PipeStream = pipeStream;
		}

		public async System.Threading.Tasks.Task<string> ReadString()
		{
			var payload = new StringBuilder();

			if (PipeStream.CanRead)
			{
				var buffer = new byte[256];

				do
				{
					var bytesRead = await PipeStream.ReadAsync(buffer, 0, buffer.Length);

					if (bytesRead > 0)
					{
						payload.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				while (!PipeStream.IsMessageComplete);
				
			}
			
			return payload.ToString();
		}

		public async System.Threading.Tasks.Task WriteString(string payload)
		{
			var payloadBytes = Encoding.Unicode.GetBytes(payload);
			
			if (PipeStream.CanWrite)
			{
				await PipeStream.WriteAsync(payloadBytes, 0, payloadBytes.Length);
				
				await PipeStream.FlushAsync();
				
				PipeStream.WaitForPipeDrain();
			}
		}
	}
}
