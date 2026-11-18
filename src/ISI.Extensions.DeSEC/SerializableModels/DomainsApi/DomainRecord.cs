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

namespace ISI.Extensions.DeSEC.SerializableModels.DomainsApi
{
	[DataContract]
	public class DomainRecord
	{
		public ISI.Extensions.Dns.DomainRecord Export()
		{
			return new()
			{
				Domain = Name,
			};
		}

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string __CreatedDateTime { get => CreatedDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreatedDateTime = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? CreatedDateTime { get; set; }

		[DataMember(Name = "keys", EmitDefaultValue = false)]
		public DomainRecordStatusKey[] Keys { get; set; }

		[DataMember(Name = "minimum_ttl", EmitDefaultValue = false)]
		public int MinimumTtl { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public string __Published { get => Published.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => Published = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? Published { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public DomainRecordStatus Status { get; set; }

		[DataMember(Name = "touched", EmitDefaultValue = false)]
		public string __TouchedDateTime { get => TouchedDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => TouchedDateTime = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? TouchedDateTime { get; set; }

		[DataMember(Name = "zonefile", EmitDefaultValue = false)]
		public string ZoneFile { get; set; }
	}

	public class DomainRecordStatus
	{
		[DataMember(Name = "checked", EmitDefaultValue = false)]
		public string __CheckedDateTime { get => CheckedDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CheckedDateTime = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? CheckedDateTime { get; set; }

		[DataMember(Name = "nameserver_status", EmitDefaultValue = false)]
		public string NameserverStatus { get; set; }

		[DataMember(Name = "nameservers", EmitDefaultValue = false)]
		public string[] Nameservers { get; set; }

		[DataMember(Name = "secure_since", EmitDefaultValue = false)]
		public string __SecureSinceDateTime { get => SecureSinceDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => SecureSinceDateTime = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? SecureSinceDateTime { get; set; }

		[DataMember(Name = "security_status", EmitDefaultValue = false)]
		public string SecurityStatus { get; set; }
	}

	public class DomainRecordStatusKey
	{
		[DataMember(Name = "dnskey", EmitDefaultValue = false)]
		public string DnsKey { get; set; }

		[DataMember(Name = "ds", EmitDefaultValue = false)]
		public string[] Ds { get; set; }

		[DataMember(Name = "managed", EmitDefaultValue = false)]
		public bool Managed { get; set; }
	}
}