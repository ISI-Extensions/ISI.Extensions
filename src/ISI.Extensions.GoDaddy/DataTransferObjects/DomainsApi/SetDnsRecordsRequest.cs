using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi
{
	public partial class SetDnsRecordsRequest : AbstractRequest
	{
		public string DomainName { get; set; }
		public ISI.Extensions.Dns.DnsRecord[] DnsRecords { get; set; }
	}
}