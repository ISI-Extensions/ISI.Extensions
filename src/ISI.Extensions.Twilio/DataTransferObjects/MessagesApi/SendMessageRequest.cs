using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Twilio.DataTransferObjects.MessagesApi
{
	public class SendMessageRequest : IRequest
	{
		public string AuthorizationKey { get; set; }
		public string AuthorizationToken { get; set; }

		public string From { get; set; }
		public string To { get; set; }
		public string Body { get; set; }
		public ISI.Extensions.Telephony.Messages.Media[] Media { get; set; }



		public string MessagingServiceKey { get; set; }

		public string StatusCallbackUrl { get; set; }

		public string ApplicationKey { get; set; }

		public decimal? MaxPrice { get; set; }

		public bool ProvideFeedback { get; set; }

		public int? ValidityPeriodInSeconds { get; set; }

		public string MaxRate { get; set; }

		public bool ForceDelivery { get; set; }

		public string ProviderKey { get; set; }

		public ISI.Extensions.Telephony.Messages.ContentRetention? ContentRetention { get; set; }

		public ISI.Extensions.Telephony.Messages.AddressRetention? AddressRetention { get; set; }

		public bool SmartEncoded { get; set; }
	}
}