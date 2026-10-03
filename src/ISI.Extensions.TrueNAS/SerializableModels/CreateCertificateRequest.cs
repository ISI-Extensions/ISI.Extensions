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

namespace ISI.Extensions.TrueNAS.SerializableModels
{
	[DataContract]
	public class CreateCertificateRequest
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string CertificateName { get; set; }

		[DataMember(Name = "create_type", EmitDefaultValue = false)]
		public string CreateType { get; set; }

		[DataMember(Name = "add_to_trusted_store", EmitDefaultValue = false)]
		public bool AddToTrustedStore { get; set; }

		[DataMember(Name = "certificate", EmitDefaultValue = false)]
		public string BundleCertificate { get; set; }

		[DataMember(Name = "privatekey", EmitDefaultValue = false)]
		public string KeyCertificate { get; set; }

		[DataMember(Name = "csr", EmitDefaultValue = false)]
		public string Csr { get; set; }

		[DataMember(Name = "key_length", EmitDefaultValue = false)]
		public int? KeyLength { get; set; }

		[DataMember(Name = "key_type", EmitDefaultValue = false)]
		public string KeyType { get; set; }

		[DataMember(Name = "ec_curve", EmitDefaultValue = false)]
		public string EcCurve { get; set; }

		[DataMember(Name = "passphrase", EmitDefaultValue = false)]
		public string Passphrase { get; set; }

		[DataMember(Name = "city", EmitDefaultValue = false)]
		public string City { get; set; }

		[DataMember(Name = "common", EmitDefaultValue = false)]
		public string Common { get; set; }

		[DataMember(Name = "country", EmitDefaultValue = false)]
		public string Country { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "organization", EmitDefaultValue = false)]
		public string Organization { get; set; }

		[DataMember(Name = "organizational_unit", EmitDefaultValue = false)]
		public string OrganizationalUnit { get; set; }

		[DataMember(Name = "state", EmitDefaultValue = false)]
		public string State { get; set; }

		[DataMember(Name = "digest_algorithm", EmitDefaultValue = false)]
		public string DigestAlgorithm { get; set; }

		[DataMember(Name = "san", EmitDefaultValue = false)]
		public string[] San { get; set; }

		[DataMember(Name = "cert_extensions", EmitDefaultValue = false)]
		public object CertExtensions { get; set; }

		[DataMember(Name = "acme_directory_url", EmitDefaultValue = false)]
		public string AcmeDirectoryUrl { get; set; }

		[DataMember(Name = "csr_id", EmitDefaultValue = false)]
		public int? CsrId { get; set; }

		[DataMember(Name = "tos", EmitDefaultValue = false)]
		public bool? Tos { get; set; }

		[DataMember(Name = "dns_mapping", EmitDefaultValue = false)]
		public object DnsMapping { get; set; }

		[DataMember(Name = "renew_days", EmitDefaultValue = false)]
		public int? RenewDays { get; set; }
	}
}