using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Mandrill.DataTransferObjects.MandrillWebhooksApi
{
	public class AddWebHookRequest
	{
		public Guid MandrillProfileUuid { get; set; }
		public string WebhookUrl { get; set; }
		public string Description { get; set; }
		public string[] Events { get; set; }
	}
}