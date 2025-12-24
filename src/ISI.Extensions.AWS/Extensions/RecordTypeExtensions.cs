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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.AWS.DataTransferObjects;

namespace ISI.Extensions.AWS.Extensions
{
	public static class RecordTypeExtensions
	{
		public static Amazon.Route53.RRType GetRRType(this ISI.Extensions.Dns.RecordType recordType)
		{
			switch (recordType)
			{
				case ISI.Extensions.Dns.RecordType.AddressRecord:
					return Amazon.Route53.RRType.A;
				case ISI.Extensions.Dns.RecordType.IPv6AddressRecord:
					return Amazon.Route53.RRType.AAAA;
				case ISI.Extensions.Dns.RecordType.CertificationAuthorityAuthorization:
					return Amazon.Route53.RRType.CAA;
				case ISI.Extensions.Dns.RecordType.CanonicalNameRecord:
					return Amazon.Route53.RRType.CNAME;
				case ISI.Extensions.Dns.RecordType.DelegationSigner:
					return Amazon.Route53.RRType.DS;
				case ISI.Extensions.Dns.RecordType.HTTPSBinding:
					return Amazon.Route53.RRType.HTTPS;
				case ISI.Extensions.Dns.RecordType.MailExchangeRecord:
					return Amazon.Route53.RRType.MX;
				case ISI.Extensions.Dns.RecordType.NamingAuthorityPointer:
					return Amazon.Route53.RRType.NAPTR;
				case ISI.Extensions.Dns.RecordType.NameServerRecord:
					return Amazon.Route53.RRType.NS;
				case ISI.Extensions.Dns.RecordType.PTRResourceRecord:
					return Amazon.Route53.RRType.PTR;
				case ISI.Extensions.Dns.RecordType.StartOfAuthorityRecord:
					return Amazon.Route53.RRType.SOA;
				case ISI.Extensions.Dns.RecordType.ServiceLocator:
					return Amazon.Route53.RRType.SRV;
				case ISI.Extensions.Dns.RecordType.SSHPublicKeyFingerprint:
					return Amazon.Route53.RRType.SSHFP;
				case ISI.Extensions.Dns.RecordType.TLSACertificateAssociation:
					return Amazon.Route53.RRType.TLSA;
				case ISI.Extensions.Dns.RecordType.TextRecord:
					return Amazon.Route53.RRType.TXT;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
