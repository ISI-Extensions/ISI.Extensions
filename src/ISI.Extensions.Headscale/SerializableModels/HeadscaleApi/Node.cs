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

namespace ISI.Extensions.Headscale.SerializableModels.HeadscaleApi
{
	[DataContract]
	public class Node
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long NodeId { get; set; }

		[DataMember(Name = "nodeKey", EmitDefaultValue = false)]
		public string NodeKey { get; set; }

		[DataMember(Name = "machineKey", EmitDefaultValue = false)]
		public string MachineKey { get; set; }

		[DataMember(Name = "discoKey", EmitDefaultValue = false)]
		public string DiscoKey { get; set; }

		[DataMember(Name = "ipAddresses", EmitDefaultValue = false)]
		public string[] IpAddresses { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "user", EmitDefaultValue = false)]
		public User User { get; set; }

		[DataMember(Name = "preAuthKey", EmitDefaultValue = false)]
		public PreAuthKey PreAuthKey { get; set; }

		[DataMember(Name = "registerMethod", EmitDefaultValue = false)]
		public string RegisterMethod { get; set; }

		[DataMember(Name = "givenName", EmitDefaultValue = false)]
		public string GivenName { get; set; }

		[DataMember(Name = "online", EmitDefaultValue = false)]
		public bool Online { get; set; }

		[DataMember(Name = "approvedRoutes", EmitDefaultValue = false)]
		public string[] ApprovedRoutes { get; set; }

		[DataMember(Name = "availableRoutes", EmitDefaultValue = false)]
		public string[] AvailableRoutes { get; set; }

		[DataMember(Name = "subnetRoutes", EmitDefaultValue = false)]
		public string[] SubnetRoutes { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "expiry", EmitDefaultValue = false)]
		public string __ExpirationDateTimeUtc { get => ExpirationDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => ExpirationDateTimeUtc = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? ExpirationDateTimeUtc { get; set; }

		[DataMember(Name = "createdAt", EmitDefaultValue = false)]
		public string __CreatedDateTimeUtc { get => CreatedDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreatedDateTimeUtc = value.ToDateTimeUtc(); }
		[IgnoreDataMember]
		public DateTime CreatedDateTimeUtc { get; set; }

		[DataMember(Name = "lastSeen", EmitDefaultValue = false)]
		public string __LastSeenDateTimeUtc { get => LastSeenDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => LastSeenDateTimeUtc = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? LastSeenDateTimeUtc { get; set; }
	}
}