#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Tailscale.DataTransferObjects.LocalTailscaleApi;
using SerializableDTOs = ISI.Extensions.Tailscale.SerializableModels.LocalTailscaleApi;

namespace ISI.Extensions.Tailscale
{
	public partial class LocalTailscaleApi
	{
		public DTOs.WhoIsByIpAddressResponse WhoIsByIpAddress(DTOs.WhoIsByIpAddressRequest request)
		{
			var response = new DTOs.WhoIsByIpAddressResponse();

			var whoIsByIpAddressResponse = ISI.Extensions.Process.WaitForProcessResponse("tailscale", "whois", "--json", request.IpAddress);

			var whoIsResponse = JsonSerializer.Deserialize<SerializableDTOs.WhoIsResponse>(whoIsByIpAddressResponse.Output);

			response.Node = whoIsResponse.Node.NullCheckedConvert(node => new DTOs.WhoIsResponseNode()
			{
				NodeKey = node.ID,
				StableID = node.StableID,
				Name = node.Name,
				User = node.User,
				Key = node.Key,
				KeyExpiry = node.KeyExpiry,
				Machine = node.Machine,
				DiscoKey = node.DiscoKey,
				Addresses = node.Addresses.ToNullCheckedArray(),
				AllowedIps = node.AllowedIps.ToNullCheckedArray(),
				Endpoints = node.Endpoints.ToNullCheckedArray(),
				HostInfo = node.HostInfo.NullCheckedConvert(hostInfo => new DTOs.WhoIsResponseHostInfo()
				{
					HostName = hostInfo.HostName,
					RoutableIps = hostInfo.RoutableIps.ToNullCheckedArray(),
					Services = hostInfo.Services.ToNullCheckedArray(service => new DTOs.WhoIsResponseHostInfoService()
					{
						Proto = service.Proto,
						Port = service.Port,
					}),
				}),
				Created = node.Created,
				PrimaryRoutes = node.PrimaryRoutes.ToNullCheckedArray(),
				MachineAuthorized = node.MachineAuthorized,
				ComputedName = node.ComputedName,
				ComputedNameWithHost = node.ComputedNameWithHost,
			});

			response.UserProfile = whoIsResponse.UserProfile.NullCheckedConvert(userProfile => new DTOs.WhoIsResponseUserProfile()
			{
				ID = userProfile.ID,
				LoginName = userProfile.LoginName,
				DisplayName = userProfile.DisplayName,
				ProfilePictureUrl = userProfile.ProfilePictureUrl,
			});

			return response;
		}
	}
}