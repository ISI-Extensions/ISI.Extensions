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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels;

namespace ISI.Extensions.TrueNAS
{
	internal delegate Task TrueNASWebSocketApiExecuteAsync(ITrueNASWebSocketApi trueNasWebSocketApi);

	internal class TrueNASWebSocketApiWrapper : IDisposable
	{
		internal async Task ExecuteAsync(string trueNASApiUrl, string userName, string apiKey, TrueNASWebSocketApiExecuteAsync trueNasWebSocketApiExecute, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var webSocket = new System.Net.WebSockets.ClientWebSocket())
			{
				var uri = new UriBuilder(trueNASApiUrl);
				uri.Scheme = (string.Equals(uri.Scheme, Uri.UriSchemeHttp) ? Uri.UriSchemeWs : Uri.UriSchemeWss);
				uri.SetPathAndQueryString("api/current");

				await webSocket.ConnectAsync(uri.Uri, System.Threading.CancellationToken.None);

				var webSocketMessageHandler = new StreamJsonRpc.WebSocketMessageHandler(webSocket);

				var trueNasWebSocketApi = StreamJsonRpc.JsonRpc.Attach<ITrueNASWebSocketApi>(webSocketMessageHandler);

				var loginResponse = await trueNasWebSocketApi.LoginAsync(new()
				{
					Mechanism = "API_KEY_PLAIN",
					Username = userName,
					ApiKey = apiKey,
				});

				if (!string.Equals(loginResponse.ResponseType, "SUCCESS", StringComparison.InvariantCultureIgnoreCase))
				{
					throw new System.Security.Authentication.AuthenticationException("Not Authenticated");
				}

				await trueNasWebSocketApiExecute(trueNasWebSocketApi);

				if (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
				{
					await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Client shutting down", System.Threading.CancellationToken.None);
				}
			}
		}

		public void Dispose()
		{
			// TODO release managed resources here
		}
	}
}
