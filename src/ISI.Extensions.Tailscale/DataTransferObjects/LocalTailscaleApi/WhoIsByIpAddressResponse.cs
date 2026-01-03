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

namespace ISI.Extensions.Tailscale.DataTransferObjects.LocalTailscaleApi
{
	public class WhoIsByIpAddressResponse
	{
		public WhoIsResponseNode Node { get; set; }
		public WhoIsResponseUserProfile UserProfile { get; set; }
	}

	public class WhoIsResponseNode
	{
		public string NodeId { get; set; }
		public string StableId { get; set; }
		public string Name { get; set; }
		public long User { get; set; }
		public string Key { get; set; }
		public DateTime? KeyExpiry { get; set; }
		public string Machine { get; set; }
		public string DiscoKey { get; set; }
		public string[] Addresses { get; set; }
		public string[] AllowedIps { get; set; }
		public string[] Endpoints { get; set; }
		public WhoIsResponseHostInfo HostInfo { get; set; }
		public DateTime Created { get; set; }
		public string[] PrimaryRoutes { get; set; }
		public bool MachineAuthorized { get; set; }
		public string ComputedName { get; set; }
		public string ComputedNameWithHost { get; set; }
	}

	public class WhoIsResponseHostInfo
	{
		public string HostName { get; set; }
		public string[] RoutableIps { get; set; }
		public WhoIsResponseHostInfoService[] Services { get; set; }
	}

	public class WhoIsResponseHostInfoService
	{
		public string Proto { get; set; }
		public int Port { get; set; }
	}

	public class WhoIsResponseUserProfile
	{
		public string UserId { get; set; }
		public string LoginName { get; set; }
		public string DisplayName { get; set; }
		public string ProfilePictureUrl { get; set; }
	}
}
