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

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public delegate void SetDnsRecordDelegate(string rootDomain, ISI.Extensions.Dns.DnsRecord dnsRecord);

	public interface IProcessNewOrderRequest : IRequest
	{
		HostContext HostContext { get; }

		string Domain { get; }

		IOrderCertificateDomainPostRenewalAction[] PostRenewalActions { get; }
	}

	public class ProcessNewOrderUsingExistingCertificateRequest : IProcessNewOrderRequest
	{
		public HostContext HostContext { get; set; }

		public string Domain { get; set; }

		public IOrderCertificateDomainPostRenewalAction[] PostRenewalActions { get; set; }
	}

	public class ProcessNewOrderUsingDnsRequest : IProcessNewOrderRequest
	{
		public HostContext HostContext { get; set; }

		public DateTime? CertificateNotBeforeDateTimeUtc { get; set; }
		public DateTime? CertificateNotAfterDateTimeUtc { get; set; }

		public string Domain { get; set; }

		public string CountryName { get; set; }

		public string State { get; set; }

		public string Locality { get; set; }

		public string Organization { get; set; }

		public string OrganizationUnit { get; set; }

		public SetDnsRecordDelegate SetDnsRecord { get; set; }

		public IOrderCertificateDomainPostRenewalAction[] PostRenewalActions { get; set; }
	}
}