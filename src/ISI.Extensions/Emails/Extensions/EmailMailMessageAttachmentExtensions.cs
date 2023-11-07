#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public static class EmailMailMessageAttachmentExtensions
	{
		public static System.Net.Mail.Attachment ToAttachment(this IEmailMailMessageAttachment emailMailMessageAttachment)
		{
			var attachment = new System.Net.Mail.Attachment(new System.IO.MemoryStream(emailMailMessageAttachment.Content), emailMailMessageAttachment.ContentType.ToContentType())
			{
				ContentId = emailMailMessageAttachment.ContentId,
				Name = emailMailMessageAttachment.Name,
				TransferEncoding = emailMailMessageAttachment.TransferEncoding.ToTransferEncoding(),
			};

			attachment.ContentDisposition.CreationDate = emailMailMessageAttachment.ContentDisposition.CreationDateUtc;
			attachment.ContentDisposition.DispositionType = emailMailMessageAttachment.ContentDisposition.DispositionType;
			attachment.ContentDisposition.FileName = emailMailMessageAttachment.ContentDisposition.FileName;
			attachment.ContentDisposition.Inline = emailMailMessageAttachment.ContentDisposition.Inline;
			attachment.ContentDisposition.ModificationDate = emailMailMessageAttachment.ContentDisposition.ModificationDateUtc;
			attachment.ContentDisposition.ReadDate = emailMailMessageAttachment.ContentDisposition.ReadDateUtc;
			attachment.ContentDisposition.Size = emailMailMessageAttachment.ContentDisposition.Size;

			if (emailMailMessageAttachment.NameEncoding.HasValue)
			{
				attachment.NameEncoding = System.Text.Encoding.GetEncoding(emailMailMessageAttachment.NameEncoding.GetValueOrDefault());
			}

			return attachment;
		}
		
		public static IEmailMailMessageAttachment ToEmailMailMessageAttachment(this System.Net.Mail.Attachment attachment)
		{
			var emailMailMessageAttachment = new EmailMailMessageAttachment( attachment.ContentStream, attachment.Name)
			{
				ContentId = attachment.ContentId,
				NameEncoding = attachment.NameEncoding?.CodePage,
				TransferEncoding = attachment.TransferEncoding.ToEmailMessageTransferEncoding(),
				ContentDisposition = new EmailMailMessageAttachmentContentDisposition()
				{
					CreationDateUtc = attachment.ContentDisposition.CreationDate,
					DispositionType = attachment.ContentDisposition.DispositionType,
					FileName = attachment.ContentDisposition.FileName,
					Inline = attachment.ContentDisposition.Inline,
					ModificationDateUtc = attachment.ContentDisposition.ModificationDate,
					ReadDateUtc = attachment.ContentDisposition.ReadDate,
					Size = attachment.ContentDisposition.Size
				}
			};

			return emailMailMessageAttachment;
		}
	}
}

