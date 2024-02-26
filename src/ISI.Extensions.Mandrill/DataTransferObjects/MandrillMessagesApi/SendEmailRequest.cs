using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Mandrill.DataTransferObjects.MandrillMessagesApi
{
	public class SendEmailRequest
	{
		public ISI.Extensions.Emails.IEmail Email { get; set; }
		public DateTime? ScheduledSendDateTimeUtc { get; set; }
	}
}