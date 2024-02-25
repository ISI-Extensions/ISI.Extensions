#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Emails.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Emails
{
	[EmailSender(EmailProviderUuid, EmailProviderName)]
	public class SmtpEmailSender : System.Net.Mail.SmtpClient, IEmailSender
	{
		public const string EmailProviderUuid = "96e5b9a9-7abe-400a-bb6e-67a9614bbaf5";
		public const string EmailProviderName = nameof(SmtpEmailSender);

		Guid IEmailSender.EmailProviderUuid => EmailProviderUuid.ToGuid();
		string IEmailSender.EmailProviderName => EmailProviderName;

		private readonly object _sendSync = new();

		public SmtpEmailSender(string host, int port = 25)
		{
			Host = host;
			Port = port;
		}

		public IEmailSenderResponse Send(string from, string to, string subject, string body)
		{
			return Send(new EmailMailMessage()
			{
				From = EmailAddress.Create(from),
				To = new[] { EmailAddress.Create(to) as IEmailAddress, },
				Subject = subject,
				Body = body,
			});
		}

		public IEmailSenderResponse Send(IEmailMailMessage emailMailMessage)
		{
			var email = new Email()
			{
				EmailMailMessage = emailMailMessage,
			};

			return Send(email);
		}

		public IEmailSenderResponse Send(IEmail email)
		{
			var response = new EmailSenderResponse()
			{
				EmailUuid = email.EmailUuid,
				EmailProviderUuid = EmailProviderUuid.ToGuid(),
			};

			lock (_sendSync)
			{
				if (string.IsNullOrEmpty(Host))
				{
					response.SentStatus = EmailSenderSentStatus.GeneralError;
					response.Message = "Host name is needed";
				}
				else
				{
					base.Send(email.EmailMailMessage.ToMailMessage());

					response.SentStatus = EmailSenderSentStatus.Sent;

					var recipients = new EmailAddressCollection();
					recipients.AddRange(email.EmailMailMessage.To);
					recipients.AddRange(email.EmailMailMessage.CC);
					recipients.AddRange(email.EmailMailMessage.Bcc);

					response.RecipientResponses = recipients.ToNullCheckedArray(recipient => new EmailSenderResponseRecipient()
					{
						EmailAddress = recipient.Formatted(),
						SentStatus = response.SentStatus,
					} as IEmailSenderResponseRecipient);
				}
			}

			return response;
		}
	}
}
