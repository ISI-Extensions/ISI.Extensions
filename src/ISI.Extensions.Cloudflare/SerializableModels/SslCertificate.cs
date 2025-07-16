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
	public class SslCertificate
	{
		public ISI.Extensions.Cloudflare.SslCertificate Export()
		{
			return new ISI.Extensions.Cloudflare.SslCertificate()
			{
				SslCertificateId = SslCertificateId,
				BundleMethod = ISI.Extensions.Enum<SslCertificateBundleMethod>.Parse(BundleMethod),
				Hosts = Hosts.ToNullCheckedArray(),
				Priority = Priority,
				ZoneId = ZoneId,
				Type = ISI.Extensions.Enum<SslCertificateType>.Parse(Type),
				Status = ISI.Extensions.Enum<SslCertificateStatus>.Parse(Status),
				ExpiresDateTimeUtc = ExpiresDateTimeUtc,
				UploadedDateTimeUtc = UploadedDateTimeUtc,
				ModifiedDateTimeUtc = ModifiedDateTimeUtc,
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string SslCertificateId { get; set; }

		[DataMember(Name = "bundle_method", EmitDefaultValue = false)]
		public string BundleMethod { get; set; }

		[DataMember(Name = "hosts", EmitDefaultValue = false)]
		public string[] Hosts { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public int Priority { get; set; }

		[DataMember(Name = "zone_id", EmitDefaultValue = false)]
		public string ZoneId { get; set; }

		[DataMember(Name = "geo_restrictions", EmitDefaultValue = false)]
		public SslCertificateGeoRestrictions GeoRestrictions { get; set; }

		[DataMember(Name = "keyless_server", EmitDefaultValue = false)]
		public SslCertificateKeylessServer KeylessServer { get; set; }

		[DataMember(Name = "policy", EmitDefaultValue = false)]
		public string Policy { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }

		[DataMember(Name = "expires_on", EmitDefaultValue = false)]
		public DateTime? ExpiresDateTimeUtc { get; set; }

		[DataMember(Name = "uploaded_on", EmitDefaultValue = false)]
		public DateTime? UploadedDateTimeUtc { get; set; }

		[DataMember(Name = "modified_on", EmitDefaultValue = false)]
		public DateTime? ModifiedDateTimeUtc { get; set; }
	}

	[DataContract]
	public class SslCertificateGeoRestrictions
	{
		[DataMember(Name = "label", EmitDefaultValue = false)]
		public string Label { get; set; }
	}

	[DataContract]
	public class SslCertificateKeylessServer
	{
		[DataMember(Name = "host", EmitDefaultValue = false)]
		public string Host { get; set; }

		[DataMember(Name = "port", EmitDefaultValue = false)]
		public int Port { get; set; }

		[DataMember(Name = "tunnel", EmitDefaultValue = false)]
		public SslCertificateKeylessServerTunnel Tunnel { get; set; }
	}

	[DataContract]
	public class SslCertificateKeylessServerTunnel
	{
		[DataMember(Name = "private_ip", EmitDefaultValue = false)]
		public string PrivateIp { get; set; }

		[DataMember(Name = "vnet_id", EmitDefaultValue = false)]
		public string VNetId { get; set; }
	}
}
