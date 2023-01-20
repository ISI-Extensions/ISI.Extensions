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
using System.Runtime.Serialization;

namespace ISI.Extensions.GoDaddy.SerializableModels.DomainsApi
{
	[DataContract]
	public class DnsRecord
	{
		public static DnsRecord ToSerializable(ISI.Extensions.Dns.DnsRecord dnsRecord)
		{
			return new()
			{
				Data = dnsRecord.Data,
				Name = dnsRecord.Name,
				Port = dnsRecord.Port,
				Priority = dnsRecord.Priority,
				Protocol = dnsRecord.Protocol,
				Service = dnsRecord.Service,
				Ttl = (long)dnsRecord.Ttl.TotalSeconds,
				RecordType = dnsRecord.RecordType.GetKey(),
				Weight = dnsRecord.Weight,
			};
		}

		public ISI.Extensions.Dns.DnsRecord Export()
		{
			return new()
			{
				Data = Data,
				Name = Name,
				Port = Port,
				Priority = Priority,
				Protocol = Protocol,
				Service = Service,
				Ttl = TimeSpan.FromSeconds(Ttl),
				RecordType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.Parse(RecordType),
				Weight = Weight,
			};
		}

		[DataMember(Name = "data", EmitDefaultValue = false)]
		public string Data { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "port", EmitDefaultValue = false)]
		public int Port { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public int Priority { get; set; }

		[DataMember(Name = "protocol", EmitDefaultValue = false)]
		public string Protocol { get; set; }

		[DataMember(Name = "service", EmitDefaultValue = false)]
		public string Service { get; set; }

		[DataMember(Name = "ttl", EmitDefaultValue = false)]
		public long Ttl { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string RecordType { get; set; }

		[DataMember(Name = "weight", EmitDefaultValue = false)]
		public int Weight { get; set; }
	}
}