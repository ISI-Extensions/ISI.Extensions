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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Emails
{
	public class EmailMailMessageAttachment : IEmailMailMessageAttachment
	{
		public EmailMailMessageAttachment()
		{

		}

		public EmailMailMessageAttachment(System.IO.Stream stream, string name)
		{
			Content = stream.ReadBytes();
			Name = name;
		}

		public string ContentKey { get; set; }
		public string Name { get; set; }
		public int? NameEncoding { get; set; }
		public byte[] Content { get; set; }
		public IEmailMailMessageAttachmentContentDisposition ContentDisposition { get; set; }
		public IEmailMailMessageContentType ContentType { get; set; }
		public EmailMessageTransferEncoding TransferEncoding { get; set; }

		IEmailMailMessageAttachment IEmailMailMessageAttachment.Clone()
		{
			return new EmailMailMessageAttachment()
			{
				ContentKey = ContentKey,
				Name = Name,
				NameEncoding = NameEncoding,
				Content = Content.ToNullCheckedArray(),
				ContentDisposition = ContentDisposition.Clone(),
				ContentType = ContentType.Clone(),
				TransferEncoding = TransferEncoding,
			};
		}
	}
}
