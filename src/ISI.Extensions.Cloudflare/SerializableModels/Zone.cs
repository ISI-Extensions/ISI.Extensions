#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
	public class Zone
	{
		public ISI.Extensions.Cloudflare.Zone Export()
		{
			return new ISI.Extensions.Cloudflare.Zone()
			{
				ZoneId = ZoneId,
				AccountId = Account?.AccountId,
				Name = Name,
				OwnerId = Owner?.OwnerId,
				PlanId = ZonePlan.ZonePlanId,
				CNameSuffix = CNameSuffix,
				Paused = Paused,
				Permissions = Permissions.ToNullCheckedArray(),
				TenantId = Tenant?.TenantId,
				TenantUnitId = TenantUnit?.TenantUnitId,
				Type = Type,
				VanityNameServers = VanityNameServers.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string ZoneId { get; set; }

		[DataMember(Name = "account", EmitDefaultValue = false)]
		public Account Account { get; set; }

		[DataMember(Name = "meta", EmitDefaultValue = false)]
		public ZoneMeta Meta { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "owner", EmitDefaultValue = false)]
		public Owner Owner { get; set; }

		[DataMember(Name = "plan", EmitDefaultValue = false)]
		public ZonePlan ZonePlan { get; set; }

		[DataMember(Name = "cname_suffix", EmitDefaultValue = false)]
		public string CNameSuffix { get; set; }

		[DataMember(Name = "paused", EmitDefaultValue = false)]
		public bool Paused { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public string[] Permissions { get; set; }

		[DataMember(Name = "tenant", EmitDefaultValue = false)]
		public Tenant Tenant { get; set; }

		[DataMember(Name = "tenant_unit", EmitDefaultValue = false)]
		public TenantUnit TenantUnit { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "vanity_name_servers", EmitDefaultValue = false)]
		public string[] VanityNameServers { get; set; }
	}

	[DataContract]
	public class ZoneMeta
	{
		[DataMember(Name = "cdn_only", EmitDefaultValue = false)]
		public bool CdnOnly { get; set; }

		[DataMember(Name = "custom_certificate_quota", EmitDefaultValue = false)]
		public int CustomCertificateQuota { get; set; }

		[DataMember(Name = "dns_only", EmitDefaultValue = false)]
		public bool DnsOnly { get; set; }

		[DataMember(Name = "foundation_dns", EmitDefaultValue = false)]
		public bool FoundationDns { get; set; }

		[DataMember(Name = "page_rule_quota", EmitDefaultValue = false)]
		public int PageRuleQuota { get; set; }

		[DataMember(Name = "phishing_detected", EmitDefaultValue = false)]
		public bool PhishingDetected { get; set; }

		[DataMember(Name = "step", EmitDefaultValue = false)]
		public int Step { get; set; }
	}
}
