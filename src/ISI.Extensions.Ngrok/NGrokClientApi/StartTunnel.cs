#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Ngrok.DataTransferObjects.NGrokClientApi;

namespace ISI.Extensions.Ngrok
{
	public partial class NGrokClientApi
	{
		public DTOs.StartTunnelResponse StartTunnel(DTOs.StartTunnelRequest request)
		{
			var tunnelName = request.TunnelName;

			if (string.IsNullOrWhiteSpace(tunnelName))
			{
				tunnelName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
			}

			var serviceRequest = new SerializableModels.ClientApi.StartTunnelRequest()
			{
				TunnelName = tunnelName,
				TunnelProtocol = ISI.Extensions.Enum<SerializableModels.ClientApi.TunnelProtocol>.Convert(request.TunnelProtocol),
				Hostname = request.Hostname,
				Subdomain = request.Subdomain,
				LocalAddress = request.LocalAddress,
				HostHeader = request.HostHeader,
				RemoteAddress = request.RemoteAddress,
				UseTls = ISI.Extensions.Enum<SerializableModels.ClientApi.UseTls>.Convert(request.UseTls),
				Inspect = request.Inspect,
			};

			var serializedRequest = Serialization.Serialize(serviceRequest).SerializedValue.Replace(@"https:\/\/localhost", @"https://localhost");

			var textRequest = new ISI.Extensions.WebClient.Rest.TextRequest(serializedRequest);

			var headers = GetHeaders();
			headers.ContentType = ISI.Extensions.WebClient.Rest.AcceptJsonHeaderValue;

			var serviceResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<ISI.Extensions.WebClient.Rest.TextRequest, SerializableModels.ClientApi.StartTunnelResponse>(GetUrl("api/tunnels"), headers, textRequest, true);

			if (GetTunnels(new()).Tunnels.Any(tunnel => string.Equals(tunnel.TunnelName, Configuration.PlaceHolderTunnelName, StringComparison.InvariantCultureIgnoreCase)))
			{
				StopTunnel(new()
				{
					TunnelName = Configuration.PlaceHolderTunnelName,
				});
			}

			return serviceResponse.NullCheckedConvert(r => r.Export());
		}
	}
}