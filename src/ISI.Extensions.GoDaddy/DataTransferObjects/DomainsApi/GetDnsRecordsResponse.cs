using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi
{
	public partial class GetDnsRecordsResponse : AbstractResponse
	{
		public ISI.Extensions.Dns.DnsRecord[] DnsRecords { get; set; }
	}
}