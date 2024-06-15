using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.NameCheap.DataTransferObjects.DomainsApi
{
	public class GetDnsRecordsRequest : IRequest
	{
		public string Url { get; set; }
		public string ApiUser { get; set; }
		public string ApiKey { get; set; }

		public string DomainName { get; set; }
	}
}