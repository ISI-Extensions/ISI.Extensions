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
using System.Runtime.Serialization;

namespace ISI.Extensions.Cloudflare.SerializableModels
{
	[DataContract]
	public class DnsRecord
	{
		public static DnsRecord ToSerializable(ISI.Extensions.Dns.DnsRecord dnsRecord, string domain)
		{
			return new()
			{
				Content = (dnsRecord.RecordType == ISI.Extensions.Dns.RecordType.TextRecord ? $"\"{dnsRecord.Data}\"" : dnsRecord.Data),
				Name = (string.IsNullOrWhiteSpace(dnsRecord.Name) ? domain : $"{dnsRecord.Name}.{domain}"),
				//Port = dnsRecord.Port,
				//Priority = dnsRecord.Priority,
				//Protocol = dnsRecord.Protocol,
				//Service = dnsRecord.Service,
				Ttl = (int)dnsRecord.Ttl.TotalSeconds,
				RecordType = dnsRecord.RecordType.GetAbbreviation(),
				Proxied = dnsRecord.Proxied,
				//Weight = dnsRecord.Weight,
				Comment = dnsRecord.Comment,
			};
		}

		public ISI.Extensions.Dns.DnsRecord Export()
		{
			return new()
			{
				Data = Content,
				Name = SubName,
				Port = Data?.Port,
				Priority = Data?.Priority ?? 10,
				//Protocol = Protocol,
				//Service = Service,
				Ttl = TimeSpan.FromSeconds(Ttl),
				RecordType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.ParseAbbreviation(RecordType),
				Proxied = Proxied,
				Weight = Data?.Weight,
				Comment = Comment,
			};
		}

		[IgnoreDataMember]
		public string SubName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(Name))
				{
					var nameParts = Name.Split('.');
					if (nameParts.Length > 2)
					{
						return string.Join(".", nameParts, 0, nameParts.Length - 2);
					}
				}

				return Name;
			}
		}

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "ttl", EmitDefaultValue = false)]
		public int Ttl { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string RecordType { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }

		[DataMember(Name = "content", EmitDefaultValue = false)]
		public string Content { get; set; }

		[DataMember(Name = "proxied", EmitDefaultValue = false)]
		public bool Proxied { get; set; }

		[DataMember(Name = "settings", EmitDefaultValue = false)]
		public DnsRecordSettings Settings { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string DnsRecordKey { get; set; }

		[DataMember(Name = "proxiable", EmitDefaultValue = false)]
		public bool Proxiable { get; set; }

		[DataMember(Name = "data", EmitDefaultValue = false)]
		public DnsRecordData Data { get; set; }

		[DataMember(Name = "comment_modified_on", EmitDefaultValue = false)]
		public DateTime? CommentModifiedDateTimeUtc { get; set; }

		[DataMember(Name = "tags_modified_on", EmitDefaultValue = false)]
		public DateTime? TagsModifiedDateTimeUtc { get; set; }

		[DataMember(Name = "created_on", EmitDefaultValue = false)]
		public DateTime? CreatedDateTimeUtc { get; set; }

		[DataMember(Name = "modified_on", EmitDefaultValue = false)]
		public DateTime? ModifiedDateTimeUtc { get; set; }
	}

	[DataContract]
	public class DnsRecordData
	{
		[DataMember(Name = "certificate", EmitDefaultValue = false)]
		public string Certificate { get; set; }

		[DataMember(Name = "matching_type", EmitDefaultValue = false)]
		public int? MatchingType { get; set; }

		[DataMember(Name = "selector", EmitDefaultValue = false)]
		public int? Selector { get; set; }

		[DataMember(Name = "usage", EmitDefaultValue = false)]
		public int? Usage { get; set; }

		[DataMember(Name = "port", EmitDefaultValue = false)]
		public int? Port { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public int? Priority { get; set; }

		[DataMember(Name = "target", EmitDefaultValue = false)]
		public string Target { get; set; }

		[DataMember(Name = "weight", EmitDefaultValue = false)]
		public int? Weight { get; set; }

		[DataMember(Name = "algorithm", EmitDefaultValue = false)]
		public int? Algorithm { get; set; }

		[DataMember(Name = "fingerprint", EmitDefaultValue = false)]
		public string Fingerprint { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public int? Type { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		public string Value { get; set; }
	}

	[DataContract]
	public class DnsRecordSettings
	{
		[DataMember(Name = "ipv4_only", EmitDefaultValue = false)]
		public bool Ipv4Only { get; set; }

		[DataMember(Name = "ipv6_only", EmitDefaultValue = false)]
		public bool Ipv6Only { get; set; }
	}
}