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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Telephony.DataTransferObjects.PhoneSwitchManagerApi;

namespace ISI.Extensions.Asterisk
{
	public partial class SwitchManager
	{
		public DTOs.OriginateCallResponse OriginateCall(DTOs.OriginateCallRequest request)
		{
			var response = new DTOs.OriginateCallResponse();
			
			var asteriskServerIpAddress = string.IsNullOrWhiteSpace(request.AsteriskServerIpAddress) ? Configuration.ServerIpAddress : request.AsteriskServerIpAddress;
			var asteriskServerPort = request.AsteriskServerPort ?? Configuration.ServerPort;
			var asteriskUserName = string.IsNullOrWhiteSpace(request.AsteriskUserName) ? Configuration.UserName : request.AsteriskUserName;
			var asteriskPassword = string.IsNullOrWhiteSpace(request.AsteriskServerIpAddress) ? Configuration.Password : request.AsteriskPassword;

			using (var clientSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp))
			{
				var serverEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(asteriskServerIpAddress), asteriskServerPort);

				clientSocket.Connect(serverEndPoint);
				clientSocket.Send(Encoding.ASCII.GetBytes(@"Action: Login
Username: " + asteriskUserName + @"
Secret: " + asteriskPassword + @"
ActionID: 1

"));

				var log = new StringBuilder();

				int bytesRead = 0;
				do
				{
					var buffer = new byte[1024];
					bytesRead = clientSocket.Receive(buffer);

					var asteriskResponse = Encoding.ASCII.GetString(buffer, 0, bytesRead);
					log.AppendLine(asteriskResponse);

					if (System.Text.RegularExpressions.Regex.Match(asteriskResponse, "Message: Authentication accepted", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
					{
						clientSocket.Send(Encoding.ASCII.GetBytes(@"Action: Originate
Channel: " + request.Channel + @"
Exten: " + request.Extension + @"
Context: " + request.Context + @"
Priority: 1
CallerID: " + request.Extension + "\r\n\r\n"));
						log.AppendLine("Calling......");

						System.Threading.Thread.Sleep(1000);

						response.Success = true;
						response.Log = log.ToString();

						return response;
					}
				} while (bytesRead != 0);

				response.Success = false;
				response.Log = log.ToString();
			}

			return response;
		}
	}
}