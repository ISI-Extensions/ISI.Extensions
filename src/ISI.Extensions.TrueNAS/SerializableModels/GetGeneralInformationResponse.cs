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

namespace ISI.Extensions.TrueNAS.SerializableModels
{
	[DataContract]
	public class GetGeneralInformationResponse
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "language", EmitDefaultValue = false)]
		public string Language { get; set; }

		[DataMember(Name = "kbdmap", EmitDefaultValue = false)]
		public string Kbdmap { get; set; }

		[DataMember(Name = "birthday", EmitDefaultValue = false)]
		public GetGeneralInformationResponseBirthday Birthday { get; set; }

		[DataMember(Name = "timezone", EmitDefaultValue = false)]
		public string Timezone { get; set; }

		[DataMember(Name = "wizardshown", EmitDefaultValue = false)]
		public bool Wizardshown { get; set; }

		[DataMember(Name = "crash_reporting", EmitDefaultValue = false)]
		public bool CrashReporting { get; set; }

		[DataMember(Name = "usage_collection", EmitDefaultValue = false)]
		public bool UsageCollection { get; set; }

		[DataMember(Name = "ui_address", EmitDefaultValue = false)]
		public string[] UiAddress { get; set; }

		[DataMember(Name = "ui_v6address", EmitDefaultValue = false)]
		public string[] UiV6Address { get; set; }

		[DataMember(Name = "ui_port", EmitDefaultValue = false)]
		public int UiPort { get; set; }

		[DataMember(Name = "ui_httpsport", EmitDefaultValue = false)]
		public int UiHttpsport { get; set; }

		[DataMember(Name = "ui_httpsredirect", EmitDefaultValue = false)]
		public bool UiHttpsredirect { get; set; }

		[DataMember(Name = "ui_httpsprotocols", EmitDefaultValue = false)]
		public string[] UiHttpsprotocols { get; set; }

		[DataMember(Name = "ui_certificate", EmitDefaultValue = false)]
		public GetGeneralInformationResponseUiCertificate UiCertificate { get; set; }

		[DataMember(Name = "crash_reporting_is_set", EmitDefaultValue = false)]
		public bool CrashReportingIsSet { get; set; }

		[DataMember(Name = "usage_collection_is_set", EmitDefaultValue = false)]
		public bool UsageCollectionIsSet { get; set; }
	}

	[DataContract]
	public class GetGeneralInformationResponseBirthday
	{
		[DataMember(Name = "$date", EmitDefaultValue = false)]
		public long Date { get; set; }
	}

	[DataContract]
	public class GetGeneralInformationResponseUiCertificate
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public int Type { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "certificate", EmitDefaultValue = false)]
		public string Certificate { get; set; }

		[DataMember(Name = "privatekey", EmitDefaultValue = false)]
		public string Privatekey { get; set; }

		[DataMember(Name = "CSR", EmitDefaultValue = false)]
		public string Csr { get; set; }

		[DataMember(Name = "revoked_date", EmitDefaultValue = false)]
		public string RevokedDate { get; set; }

		[DataMember(Name = "signedby", EmitDefaultValue = false)]
		public string Signedby { get; set; }

		[DataMember(Name = "root_path", EmitDefaultValue = false)]
		public string RootPath { get; set; }

		[DataMember(Name = "certificate_path", EmitDefaultValue = false)]
		public string CertificatePath { get; set; }

		[DataMember(Name = "privatekey_path", EmitDefaultValue = false)]
		public string PrivatekeyPath { get; set; }

		[DataMember(Name = "csr_path", EmitDefaultValue = false)]
		public string CsrPath { get; set; }

		[DataMember(Name = "cert_type", EmitDefaultValue = false)]
		public string CertType { get; set; }

		[DataMember(Name = "revoked", EmitDefaultValue = false)]
		public bool Revoked { get; set; }

		[DataMember(Name = "issuer", EmitDefaultValue = false)]
		public string Issuer { get; set; }

		[DataMember(Name = "chain_list", EmitDefaultValue = false)]
		public string[] ChainList { get; set; }

		[DataMember(Name = "country", EmitDefaultValue = false)]
		public string Country { get; set; }

		[DataMember(Name = "state", EmitDefaultValue = false)]
		public string State { get; set; }

		[DataMember(Name = "city", EmitDefaultValue = false)]
		public string City { get; set; }

		[DataMember(Name = "organization", EmitDefaultValue = false)]
		public string Organization { get; set; }

		[DataMember(Name = "organizational_unit", EmitDefaultValue = false)]
		public string OrganizationalUnit { get; set; }

		[DataMember(Name = "common", EmitDefaultValue = false)]
		public string Common { get; set; }

		[DataMember(Name = "san", EmitDefaultValue = false)]
		public string[] San { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "DN", EmitDefaultValue = false)]
		public string Dn { get; set; }

		[DataMember(Name = "subject_name_hash", EmitDefaultValue = false)]
		public long SubjectNameHash { get; set; }

		[DataMember(Name = "extensions", EmitDefaultValue = false)]
		public GetGeneralInformationResponseUiCertificateExtensions Extensions { get; set; }

		[DataMember(Name = "digest_algorithm", EmitDefaultValue = false)]
		public string DigestAlgorithm { get; set; }

		[DataMember(Name = "lifetime", EmitDefaultValue = false)]
		public int Lifetime { get; set; }

		[DataMember(Name = "from", EmitDefaultValue = false)]
		public string From { get; set; }

		[DataMember(Name = "until", EmitDefaultValue = false)]
		public string Until { get; set; }

		[DataMember(Name = "serial", EmitDefaultValue = false)]
		public string Serial { get; set; }

		[DataMember(Name = "chain", EmitDefaultValue = false)]
		public bool Chain { get; set; }

		[DataMember(Name = "fingerprint", EmitDefaultValue = false)]
		public string Fingerprint { get; set; }

		[DataMember(Name = "key_length", EmitDefaultValue = false)]
		public int KeyLength { get; set; }

		[DataMember(Name = "key_type", EmitDefaultValue = false)]
		public string KeyType { get; set; }

		[DataMember(Name = "parsed", EmitDefaultValue = false)]
		public bool Parsed { get; set; }

		[DataMember(Name = "_internal", EmitDefaultValue = false)]
		public string Internal { get; set; }

		[DataMember(Name = "CA_type_existing", EmitDefaultValue = false)]
		public bool CaTypeExisting { get; set; }

		[DataMember(Name = "CA_type_internal", EmitDefaultValue = false)]
		public bool CaTypeInternal { get; set; }

		[DataMember(Name = "CA_type_intermediate", EmitDefaultValue = false)]
		public bool CaTypeIntermediate { get; set; }

		[DataMember(Name = "cert_type_existing", EmitDefaultValue = false)]
		public bool CertTypeExisting { get; set; }

		[DataMember(Name = "cert_type_internal", EmitDefaultValue = false)]
		public bool CertTypeInternal { get; set; }

		[DataMember(Name = "cert_type_CSR", EmitDefaultValue = false)]
		public bool CertTypeCsr { get; set; }
	}

	[DataContract]
	public class GetGeneralInformationResponseUiCertificateExtensions
	{
		[DataMember(Name = "AuthorityKeyIdentifier", EmitDefaultValue = false)]
		public string AuthorityKeyIdentifier { get; set; }

		[DataMember(Name = "SubjectKeyIdentifier", EmitDefaultValue = false)]
		public string SubjectKeyIdentifier { get; set; }

		[DataMember(Name = "KeyUsage", EmitDefaultValue = false)]
		public string KeyUsage { get; set; }

		[DataMember(Name = "BasicConstraints", EmitDefaultValue = false)]
		public string BasicConstraints { get; set; }

		[DataMember(Name = "ExtendedKeyUsage", EmitDefaultValue = false)]
		public string ExtendedKeyUsage { get; set; }

		[DataMember(Name = "CertificatePolicies", EmitDefaultValue = false)]
		public string CertificatePolicies { get; set; }

		[DataMember(Name = "AuthorityInfoAccess", EmitDefaultValue = false)]
		public string AuthorityInfoAccess { get; set; }

		[DataMember(Name = "SubjectAltName", EmitDefaultValue = false)]
		public string SubjectAltName { get; set; }

		[DataMember(Name = "Ct_precert_scts", EmitDefaultValue = false)]
		public string CtPrecertScts { get; set; }
	}
}