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
using System.Text;

namespace ISI.Extensions.Cloudflare
{
	public class SslCertificate
	{
		public string SslCertificateId { get; set; }

		public SslCertificateBundleMethod BundleMethod { get; set; }

		public string[] Hosts { get; set; }

		public int Priority { get; set; }

		public string ZoneId { get; set; }

		public SslCertificateType Type { get; set; }

		public SslCertificateStatus Status { get; set; }
		
		public DateTime? ExpiresDateTimeUtc { get; set; }

		public DateTime? UploadedDateTimeUtc { get; set; }

		public DateTime? ModifiedDateTimeUtc { get; set; }

		public override string ToString() => $"{string.Join(";", Hosts ?? [])} {SslCertificateId}";
	}

	public enum SslCertificateBundleMethod
	{
		UnKnown,
		[ISI.Extensions.Enum("Ubiquitous", "ubiquitous")] Ubiquitous,
	}

	public enum SslCertificateType
	{
		UnKnown,
		[ISI.Extensions.Enum("Legacy", "legacy_custom")] Legacy,
		[ISI.Extensions.Enum("Sni", "sni_custom")] Sni,
	}

	public enum SslCertificateStatus
	{
		UnKnown,
		[ISI.Extensions.Enum("Initializing", "initializing")] Initializing,
		[ISI.Extensions.Enum("Pending", "pending")] Pending,
		[ISI.Extensions.Enum("Active", "active")] Active,
		[ISI.Extensions.Enum("Expired", "expired")] Expired,
		[ISI.Extensions.Enum("Deleted", "deleted")] Deleted,
	}
}
