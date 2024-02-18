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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Emails.Extensions
{
	public static class MailMessageExtensions
	{
		public static IEmailMailMessage ToEmailMailMessage(this System.Net.Mail.MailMessage mailMessage)
		{
			return mailMessage.NullCheckedConvert(source => new EmailMailMessage()
			{
				From = source.From.NullCheckedConvert(MailAddressExtensions.ToEmailAddress),
				Sender = source.Sender.NullCheckedConvert(MailAddressExtensions.ToEmailAddress),
				To = source.To.ToNullCheckedArray(MailAddressExtensions.ToEmailAddress),
				CC = source.CC.ToNullCheckedArray(MailAddressExtensions.ToEmailAddress),
				Bcc = source.Bcc.ToNullCheckedArray(MailAddressExtensions.ToEmailAddress),
				Priority = source.Priority.ToEmailMessagePriority(),
				DeliveryNotificationOptions = source.DeliveryNotificationOptions.ToEmailMessageDeliveryNotificationOption(),
				HeadersEncoding = source.HeadersEncoding?.CodePage,
				SubjectEncoding = source.SubjectEncoding?.CodePage,
				Subject = source.Subject,
				BodyEncoding = source.BodyEncoding?.CodePage,
				Body = source.Body,
				IsBodyHtml = source.IsBodyHtml,
				Attachments = source.Attachments.ToNullCheckedArray(EmailMailMessageAttachmentExtensions.ToEmailMailMessageAttachment),
				AlternateViews = source.AlternateViews.ToNullCheckedArray(EmailMailMessageAlternateViewExtensions.ToEmailMailMessageAlternateView),
			});
		}

		public static System.Net.Mail.MailMessage ToMailMessage(this IEmailMailMessage emailMailMessage)
		{
			return emailMailMessage.NullCheckedConvert(source =>
			{
				var mailMessage = new System.Net.Mail.MailMessage()
				{
					From = source.From.NullCheckedConvert(MailAddressExtensions.ToMailAddress),
					Sender = source.Sender.NullCheckedConvert(MailAddressExtensions.ToMailAddress),
					Priority = source.Priority.ToMailPriority(),
					DeliveryNotificationOptions = source.DeliveryNotificationOptions.ToDeliveryNotificationOptions(),
					Subject = source.Subject,
					Body = source.Body,
					IsBodyHtml = source.IsBodyHtml,
				};

				if (source.To.NullCheckedAny())
				{
					foreach (var emailAddress in source.To)
					{
						mailMessage.To.Add(emailAddress.ToMailAddress());
					}
				}

				if (source.CC.NullCheckedAny())
				{
					foreach (var emailAddress in source.CC)
					{
						mailMessage.CC.Add(emailAddress.ToMailAddress());
					}
				}

				if (source.Bcc.NullCheckedAny())
				{
					foreach (var emailAddress in source.Bcc)
					{
						mailMessage.Bcc.Add(emailAddress.ToMailAddress());
					}
				}

				if (emailMailMessage.HeadersEncoding.HasValue)
				{
					mailMessage.HeadersEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.HeadersEncoding.GetValueOrDefault());
				}

				if (emailMailMessage.SubjectEncoding.HasValue)
				{
					mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.SubjectEncoding.GetValueOrDefault());
				}

				if (emailMailMessage.BodyEncoding.HasValue)
				{
					mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(emailMailMessage.BodyEncoding.GetValueOrDefault());
				}
				
				if (source.Attachments.NullCheckedAny())
				{
					foreach (var attachment in source.Attachments)
					{
						mailMessage.Attachments.Add(attachment.ToAttachment());
					}
				}
				
				if (source.AlternateViews.NullCheckedAny())
				{
					foreach (var alternateView in source.AlternateViews)
					{
						mailMessage.AlternateViews.Add(alternateView.ToAlternateView());
					}
				}

				return mailMessage;
			});
		}
	}
}
