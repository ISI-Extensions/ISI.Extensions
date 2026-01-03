#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Emails.Extensions
{
	public static class EmailMailMessageExtensions
	{
		public static System.Net.Mail.MailMessage ToEmailMailMessage(this IEmailMailMessage emailMailMessage)
		{
			var mailMessage = new System.Net.Mail.MailMessage()
			{
				From = emailMailMessage.From?.ToMailAddress(),
				Sender = emailMailMessage.Sender?.ToMailAddress(),

				Priority = emailMailMessage.Priority.ToMailPriority(),
				DeliveryNotificationOptions = emailMailMessage.DeliveryNotificationOptions.ToDeliveryNotificationOptions(),

				Subject = emailMailMessage.Subject,

				Body = emailMailMessage.Body,
				IsBodyHtml = emailMailMessage.IsBodyHtml,
			};

			foreach (var emailAddress in  emailMailMessage.To ?? [])
			{
				mailMessage.To.Add(emailAddress.ToMailAddress());
			}

			foreach (var emailAddress in  emailMailMessage.CC ?? [])
			{
				mailMessage.CC.Add(emailAddress.ToMailAddress());
			}

			foreach (var emailAddress in  emailMailMessage.Bcc ?? [])
			{
				mailMessage.Bcc.Add(emailAddress.ToMailAddress());
			}

			if (emailMailMessage.HeadersEncoding.HasValue)
			{
				mailMessage.HeadersEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.HeadersEncoding.GetValueOrDefault());
			}

			foreach (var header in emailMailMessage.Headers ?? new Dictionary<string, string>())
			{
				mailMessage.Headers[header.Key] = header.Value;
			}

			if (emailMailMessage.SubjectEncoding.HasValue)
			{
				mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.SubjectEncoding.GetValueOrDefault());
			}

			if (emailMailMessage.BodyEncoding.HasValue)
			{
				mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.BodyEncoding.GetValueOrDefault());
			}

			foreach (var emailMailMessageAttachment in emailMailMessage.Attachments ?? [])
			{
				mailMessage.Attachments.Add(emailMailMessageAttachment.ToAttachment());
			}

			foreach (var emailMailMessageAttachment in emailMailMessage.AlternateViews ?? [])
			{
				mailMessage.AlternateViews.Add(emailMailMessageAttachment.ToAlternateView());
			}

			return mailMessage;
		}
		
		public static IEmailMailMessage ToMailMessage(this System.Net.Mail.MailMessage mailMessage)
		{
			var emailMailMessage = new EmailMailMessage()
			{
				From = mailMessage.From?.ToEmailAddress(),
				Sender = mailMessage.Sender?.ToEmailAddress(),

				To = mailMessage.To.ToNullCheckedArray(emailAddress => emailAddress.ToEmailAddress()),
				CC = mailMessage.CC.ToNullCheckedArray(emailAddress => emailAddress.ToEmailAddress()),
				Bcc = mailMessage.Bcc.ToNullCheckedArray(emailAddress => emailAddress.ToEmailAddress()),

				Priority = mailMessage.Priority.ToEmailMessagePriority(),
				DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions.ToEmailMessageDeliveryNotificationOption(),

				SubjectEncoding = mailMessage.SubjectEncoding?.CodePage,
				Subject = mailMessage.Subject,

				BodyEncoding = mailMessage.BodyEncoding?.CodePage,
				Body = mailMessage.Body,
				IsBodyHtml = mailMessage.IsBodyHtml,

				Attachments = mailMessage.Attachments.ToNullCheckedArray(attachment => attachment.ToEmailMailMessageAttachment()),
				AlternateViews = mailMessage.AlternateViews.ToNullCheckedArray(attachment => attachment.ToEmailMailMessageAlternateView()),
			};
			
			emailMailMessage.HeadersEncoding = mailMessage.HeadersEncoding?.CodePage;
			foreach (var headerKey in mailMessage.Headers.AllKeys)
			{
				emailMailMessage.Headers.Add(headerKey, mailMessage.Headers[headerKey]);
			}

			return emailMailMessage;
		}
	}
}
