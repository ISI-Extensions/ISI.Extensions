using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Cloudflare.DataTransferObjects.DomainsApi
{
	public class GetDnsRecordsRequest
	{
		public string Url { get; set; }
		public string ApiToken { get; set; }

		public string Domain { get; set; }
	}
}