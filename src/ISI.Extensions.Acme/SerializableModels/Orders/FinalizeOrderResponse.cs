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
using System.Runtime.Serialization;

namespace ISI.Extensions.Acme.SerializableModels.Orders
{
	[DataContract]
	public class FinalizeOrderResponse
	{
		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string __Status { get => OrderStatus.GetAbbreviation(); set => OrderStatus = ISI.Extensions.Enum<AcmeOrderStatus>.ParseAbbreviation(value); }
		[IgnoreDataMember]
		public AcmeOrderStatus OrderStatus { get; set; }

		[DataMember(Name = "expires", EmitDefaultValue = false)]
		public string __RequestExpiresDateTimeUtc { get => RequestExpiresDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => RequestExpiresDateTimeUtc = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? RequestExpiresDateTimeUtc { get; set; }

		[DataMember(Name = "notBefore", EmitDefaultValue = false)]
		public string __CertificateNotBeforeDateTimeUtc { get => CertificateNotBeforeDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => CertificateNotBeforeDateTimeUtc = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? CertificateNotBeforeDateTimeUtc { get; set; }

		[DataMember(Name = "notAfter", EmitDefaultValue = false)]
		public string __CertificateNotAfterDateTimeUtc { get => CertificateNotAfterDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => CertificateNotAfterDateTimeUtc = value.ToDateTimeUtcNullable(); }
		[IgnoreDataMember]
		public DateTime? CertificateNotAfterDateTimeUtc { get; set; }

		[DataMember(Name = "identifiers", EmitDefaultValue = false)]
		public OrderCertificateIdentifier[] Identifiers { get; set; }

		[DataMember(Name = "authorizations", EmitDefaultValue = false)]
		public string[] AuthorizationsUrls { get; set; }

		[DataMember(Name = "finalize", EmitDefaultValue = false)]
		public string FinalizeOrderUrl { get; set; }

		[DataMember(Name = "certificate", EmitDefaultValue = false)]
		public string GetCertificateUrl { get; set; }

		[DataMember(Name = "error", EmitDefaultValue = false)]
		public Error Error { get; set; }
	}
}