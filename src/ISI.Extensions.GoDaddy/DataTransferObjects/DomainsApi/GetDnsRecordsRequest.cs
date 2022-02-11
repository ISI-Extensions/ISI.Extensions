using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi
{
	public partial class GetDnsRecordsRequest : AbstractRequest
	{
		public string DomainName { get; set; }
		public ISI.Extensions.Dns.RecordType? RecordType { get; set; }
		public string Name { get; set; }
	}
}