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
using System.Text;

namespace ISI.Extensions.Dns
{
	public enum RecordType
	{
		[ISI.Extensions.Enum("Address record", "A")] AddressRecord,
		[ISI.Extensions.Enum("IPv6 address record", "AAAA")] IPv6AddressRecord,
		[ISI.Extensions.Enum("AFS database record", "AFSDB")] AFSDatabaseRecord,
		[ISI.Extensions.Enum("Address Prefix List", "APL")] AddressPrefixList,
		[ISI.Extensions.Enum("Certification Authority Authorization", "CAA")] CertificationAuthorityAuthorization,
		[ISI.Extensions.Enum("Child Copy of DNSKEY", "CDNSKEY")] ChildCopyOfDNSKEY,
		[ISI.Extensions.Enum("Child DS", "CDS")] ChildDS,
		[ISI.Extensions.Enum("Certificate record", "CERT")] CertificateRecord,
		[ISI.Extensions.Enum("Canonical name record", "CNAME")] CanonicalNameRecord,
		[ISI.Extensions.Enum("Child-to-Parent Synchronization", "CSYNC")] ChildToParentSynchronization,
		[ISI.Extensions.Enum("DHCP identifier", "DHCID")] DHCPIdentifier,
		[ISI.Extensions.Enum("DNSSEC Lookaside Validation record", "DLV")] DNSSECLookasideValidationRecord,
		[ISI.Extensions.Enum("Delegation name record", "DNAME")] DelegationNameRecord,
		[ISI.Extensions.Enum("DNS Key record", "DNSKEY")] DNSKeyRecord,
		[ISI.Extensions.Enum("Delegation signer", "DS")] 	DelegationSigner,
		[ISI.Extensions.Enum("MAC address (EUI-48)", "EUI48")] MACAddressEUI48,
		[ISI.Extensions.Enum("MAC address (EUI-64)", "EUI64")] MACAddressEUI64,
		[ISI.Extensions.Enum("Host Information", "HINFO")] HostInformation,
		[ISI.Extensions.Enum("Host Identity Protocol", "HIP")] HostIdentityProtocol,
		[ISI.Extensions.Enum("HTTPS Binding", "HTTPS")] HTTPSBinding,
		[ISI.Extensions.Enum("IPsec Key", "IPSECKEY")] IPsecKey,
		[ISI.Extensions.Enum("Key record", "KEY")] KeyRecord,
		[ISI.Extensions.Enum("Key Exchanger record", "KX")] KeyExchangerRecord,
		[ISI.Extensions.Enum("Location record", "LOC")] LocationRecord,
		[ISI.Extensions.Enum("Mail exchange record", "MX")] MailExchangeRecord,
		[ISI.Extensions.Enum("Naming Authority Pointer", "NAPTR")] NamingAuthorityPointer,
		[ISI.Extensions.Enum("Name server record", "NS")] NameServerRecord,
		[ISI.Extensions.Enum("Next Secure record", "NSEC")] NextSecureRecord,
		[ISI.Extensions.Enum("Next Secure record  version 3", "NSEC3")] NextSecureRecordVersion3,
		[ISI.Extensions.Enum("NSEC3 parameters", "NSEC3PARAM")] NSEC3Parameters,
		[ISI.Extensions.Enum("OpenPGP public key record", "OPENPGPKEY")] OpenPGPPublicKeyRecord,
		[ISI.Extensions.Enum("PTR Resource Record", "PTR")] PTRResourceRecord,
		[ISI.Extensions.Enum("Responsible Person", "RP")] ResponsiblePerson,
		[ISI.Extensions.Enum("DNSSEC signature", "RRSIG")] DNSSECSignature,
		[ISI.Extensions.Enum("Signature", "SIG")] Signature,
		[ISI.Extensions.Enum("S/MIME cert association", "SMIMEA")] SMIMECertAssociation,
		[ISI.Extensions.Enum("Start of authority record", "SOA")] StartOfAuthorityRecord,
		[ISI.Extensions.Enum("Service locator", "SRV")] ServiceLocator,
		[ISI.Extensions.Enum("SSH Public Key Fingerprint", "SSHFP")] SSHPublicKeyFingerprint,
		[ISI.Extensions.Enum("Service Binding", "SVCB")] ServiceBinding,
		[ISI.Extensions.Enum("DNSSEC Trust Authorities", "TA")] DNSSECTrustAuthorities,
		[ISI.Extensions.Enum("Transaction Key record", "TKEY")] TransactionKeyRecord,
		[ISI.Extensions.Enum("TLSA certificate association", "TLSA")] TLSACertificateAssociation,
		[ISI.Extensions.Enum("Transaction Signature", "TSIG")] TransactionSignature,
		[ISI.Extensions.Enum("Text record", "TXT")] TextRecord,
		[ISI.Extensions.Enum("Uniform Resource Identifier", "URI")] UniformResourceIdentifier,
		[ISI.Extensions.Enum("Message Digests for DNS Zones", "ZONEMD")] MessageDigestsForDNSZones,
		[ISI.Extensions.Enum("Authoritative Zone Transfer", "AXFR")] AuthoritativeZoneTransfer,
		[ISI.Extensions.Enum("Incremental Zone Transfer", "IXFR")] 	IncrementalZoneTransfer,
		[ISI.Extensions.Enum("Option", "OPT")] Option,
		[ISI.Extensions.Enum("Mail Destination", "MD")] MailDestination,
		[ISI.Extensions.Enum("Mail Forwarder", "MF")] MailForwarder,
		[ISI.Extensions.Enum("Sender Policy Framework", "SPF")] SenderPolicyFramework,
	}
}
