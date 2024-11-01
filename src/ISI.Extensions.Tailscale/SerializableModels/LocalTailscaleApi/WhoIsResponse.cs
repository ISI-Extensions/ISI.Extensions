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
using System.Text;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Tailscale.SerializableModels.LocalTailscaleApi
{
	[DataContract]
	public class WhoIsResponse
	{
		[DataMember(Name = "Node", EmitDefaultValue = false)]
		public WhoIsResponseNode Node { get; set; }

		[DataMember(Name = "UserProfile", EmitDefaultValue = false)]
		public WhoIsResponseUserProfile UserProfile { get; set; }
	}

	[DataContract]
	public class WhoIsResponseNode
	{
		[DataMember(Name = "ID", EmitDefaultValue = false)]
		public string ID { get; set; }

		[DataMember(Name = "StableID", EmitDefaultValue = false)]
		public string StableID { get; set; }

		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "User", EmitDefaultValue = false)]
		public long User { get; set; }

		[DataMember(Name = "Key", EmitDefaultValue = false)]
		public string Key { get; set; }

		[DataMember(Name = "KeyExpiry", EmitDefaultValue = false)]
		public string __KeyExpiry { get => KeyExpiry.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => KeyExpiry = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? KeyExpiry { get; set; }

		[DataMember(Name = "Machine", EmitDefaultValue = false)]
		public string Machine { get; set; }

		[DataMember(Name = "DiscoKey", EmitDefaultValue = false)]
		public string DiscoKey { get; set; }

		[DataMember(Name = "Addresses", EmitDefaultValue = false)]
		public string[] Addresses { get; set; }

		[DataMember(Name = "AllowedIPs", EmitDefaultValue = false)]
		public string[] AllowedIps { get; set; }

		[DataMember(Name = "Endpoints", EmitDefaultValue = false)]
		public string[] Endpoints { get; set; }

		[DataMember(Name = "Hostinfo", EmitDefaultValue = false)]
		public WhoIsResponseHostInfo HostInfo { get; set; }

		[DataMember(Name = "Created", EmitDefaultValue = false)]
		public string __Created { get => Created.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => Created = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Created { get; set; }

		[DataMember(Name = "PrimaryRoutes", EmitDefaultValue = false)]
		public string[] PrimaryRoutes { get; set; }

		[DataMember(Name = "MachineAuthorized", EmitDefaultValue = false)]
		public bool MachineAuthorized { get; set; }

		[DataMember(Name = "ComputedName", EmitDefaultValue = false)]
		public string ComputedName { get; set; }

		[DataMember(Name = "ComputedNameWithHost", EmitDefaultValue = false)]
		public string ComputedNameWithHost { get; set; }
	}

	[DataContract]
	public class WhoIsResponseHostInfo
	{
		[DataMember(Name = "Hostname", EmitDefaultValue = false)]
		public string HostName { get; set; }

		[DataMember(Name = "RoutableIPs", EmitDefaultValue = false)]
		public string[] RoutableIps { get; set; }

		[DataMember(Name = "Services", EmitDefaultValue = false)]
		public WhoIsResponseHostInfoService[] Services { get; set; }
	}

	[DataContract]
	public class WhoIsResponseHostInfoService
	{
		[DataMember(Name = "Proto", EmitDefaultValue = false)]
		public string Proto { get; set; }

		[DataMember(Name = "Port", EmitDefaultValue = false)]
		public int Port { get; set; }
	}

	[DataContract]
	public class WhoIsResponseUserProfile
	{
		[DataMember(Name = "ID", EmitDefaultValue = false)]
		public string ID { get; set; }

		[DataMember(Name = "LoginName", EmitDefaultValue = false)]
		public string LoginName { get; set; }

		[DataMember(Name = "DisplayName", EmitDefaultValue = false)]
		public string DisplayName { get; set; }

		[DataMember(Name = "ProfilePicURL", EmitDefaultValue = false)]
		public string ProfilePictureUrl { get; set; }
	}
}
