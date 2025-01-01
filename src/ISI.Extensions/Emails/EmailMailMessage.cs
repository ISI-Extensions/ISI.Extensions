#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Emails
{
	public class EmailMailMessage : IEmailMailMessage
	{
		public IEmailAddress From { get; set; }
		public IEmailAddress Sender { get; set; }
		public IEmailAddress[] To { get; set; }
		public IEmailAddress[] CC { get; set; }
		public IEmailAddress[] Bcc { get; set; }

		public EmailMessagePriority Priority { get; set; }
		public EmailMessageDeliveryNotificationOption DeliveryNotificationOptions { get; set; }

		public int? HeadersEncoding { get; set; }
		private IDictionary<string, string> _headers = null;
		public IDictionary<string, string> Headers => _headers ??= new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

		public int? SubjectEncoding { get; set; }
		public string Subject { get; set; }

		public int? BodyEncoding { get; set; }
		public string Body { get; set; }
		public bool IsBodyHtml { get; set; }

		public IEmailMailMessageAttachment[] Attachments { get; set; }
		public IEmailMailMessageAlternateView[] AlternateViews { get; set; }

		IEmailMailMessage IEmailMailMessage.Clone()
		{
			return new EmailMailMessage()
			{
				From = From?.Clone(),
				Sender = Sender?.Clone(),
				To = To.ToNullCheckedArray(to => to.Clone()),
				CC = CC.ToNullCheckedArray(cc => cc.Clone()),
				Bcc = Bcc.ToNullCheckedArray(bcc => bcc.Clone()),

				Priority = Priority,
				DeliveryNotificationOptions = DeliveryNotificationOptions,

				HeadersEncoding = HeadersEncoding,
				_headers = Headers.ToNullCheckedDictionary(header => header.Key, header => header.Value, StringComparer.InvariantCultureIgnoreCase),

				SubjectEncoding = SubjectEncoding,
				Subject = Subject,

				BodyEncoding = BodyEncoding,
				Body = Body,
				IsBodyHtml = IsBodyHtml,

				Attachments = Attachments.ToNullCheckedArray(attachment => attachment.Clone()),
				AlternateViews = AlternateViews.ToNullCheckedArray(alternateViews => alternateViews.Clone()),
			};
		}
	}
}
