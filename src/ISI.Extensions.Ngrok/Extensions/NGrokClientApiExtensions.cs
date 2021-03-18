#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Ngrok.DataTransferObjects.NGrokClientApi;

namespace ISI.Extensions.Ngrok.Extensions
{
	public static class NGrokClientApiExtensions
	{
		public static bool DeleteAllCapturedTraffic(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi)
		{
			return nGrokClientApi.DeleteAllCapturedTraffic(new DTOs.DeleteAllCapturedTrafficRequest()).Success;
		}

		public static DTOs.GetCapturedTrafficResponse GetCapturedTraffic(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string tunnelName = null, int limit = 50)
		{
			return nGrokClientApi.GetCapturedTraffic(new DTOs.GetCapturedTrafficRequest()
			{
				TunnelName = tunnelName,
				Limit = limit,
			});
		}

		public static DTOs.GetCapturedTrafficDetailResponse GetCapturedTrafficDetail(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string trafficKey)
		{
			return nGrokClientApi.GetCapturedTrafficDetail(new DTOs.GetCapturedTrafficDetailRequest()
			{
				TrafficKey = trafficKey,
			});
		}

		public static DTOs.GetTunnelResponse GetTunnel(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string tunnelName)
		{
			return nGrokClientApi.GetTunnel(new DTOs.GetTunnelRequest()
			{
				TunnelName = tunnelName,
			});
		}

		public static DTOs.GetTunnelsResponse GetTunnels(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi)
		{
			return nGrokClientApi.GetTunnels(new DTOs.GetTunnelsRequest());
		}

		public static bool ReplayCapturedTraffic(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string trafficKey, string tunnelName = null)
		{
			return nGrokClientApi.ReplayCapturedTraffic(new DTOs.ReplayCapturedTrafficRequest()
			{
				TrafficKey = trafficKey,
				TunnelName = tunnelName,
			}).Success;
		}

		public static DTOs.StartTunnelResponse StartTunnel(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string tunnelName, TunnelProtocol protocol, int port, string hostHeader = null)
		{
			return nGrokClientApi.StartTunnel(new DTOs.StartTunnelRequest()
			{
				TunnelName = tunnelName,
				TunnelProtocol = protocol,
				Port  = port,
				HostHeader = hostHeader,
			});
		}

		public static bool StopTunnel(this ISI.Extensions.Ngrok.INGrokClientApi nGrokClientApi, string tunnelName)
		{
			return nGrokClientApi.StopTunnel(new DTOs.StopTunnelRequest()
			{
				TunnelName = tunnelName,
			}).Success;
		}
	}
}
