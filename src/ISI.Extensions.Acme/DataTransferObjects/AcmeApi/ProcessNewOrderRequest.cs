using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Acme.DataTransferObjects.AcmeApi
{
	public delegate void SetDnsRecordDelegate(string rootDomainName, ISI.Extensions.Dns.DnsRecord dnsRecord);

	public interface IProcessNewOrderRequest : IRequest
	{
		HostContext HostContext { get; }

		string DomainName { get; }

		IOrderCertificateDomainPostRenewalAction[] PostRenewalActions { get; }
	}

	public class ProcessNewOrderUsingDnsRequest : IProcessNewOrderRequest
	{
		public HostContext HostContext { get; set; }

		public DateTime? CertificateNotBeforeDateTimeUtc { get; set; }
		public DateTime? CertificateNotAfterDateTimeUtc { get; set; }

		public string DomainName { get; set; }

		public string CountryName { get; set; }

		public string State { get; set; }

		public string Locality { get; set; }

		public string Organization { get; set; }

		public string OrganizationUnit { get; set; }

		public SetDnsRecordDelegate SetDnsRecord { get; set; }

		public IOrderCertificateDomainPostRenewalAction[] PostRenewalActions { get; set; }
	}
}