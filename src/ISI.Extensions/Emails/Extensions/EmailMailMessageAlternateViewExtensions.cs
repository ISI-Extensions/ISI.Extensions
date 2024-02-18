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
	public static class EmailMailMessageAlternateViewExtensions
	{
		public static System.Net.Mail.AlternateView ToAlternateView(this IEmailMailMessageAlternateView emailMailMessageAlternateView)
		{
			var alternateView = new System.Net.Mail.AlternateView(new System.IO.MemoryStream(emailMailMessageAlternateView.Content), emailMailMessageAlternateView.ContentType.ToContentType())
			{
				ContentId = emailMailMessageAlternateView.ContentKey,
				TransferEncoding = emailMailMessageAlternateView.TransferEncoding.ToTransferEncoding(),
			};

			foreach (var linkedResource in emailMailMessageAlternateView.LinkedResources ?? Array.Empty<IEmailMailMessageLinkedResource>())
			{
				alternateView.LinkedResources.Add(linkedResource.ToEmailMailMessageLinkedResource());
			}

			return alternateView;
		}
		
		public static IEmailMailMessageAlternateView ToEmailMailMessageAlternateView(this System.Net.Mail.AlternateView alternateView)
		{
			return new EmailMailMessageAlternateView()
			{
				Content = alternateView.ContentStream.ReadBytes(),
				ContentKey = alternateView.ContentId,
				LinkedResources = alternateView.LinkedResources.ToNullCheckedArray(linkedResource => new EmailMailMessageLinkedResource(linkedResource)),
				ContentType = alternateView.ContentType.ToContentType(),
				TransferEncoding = alternateView.TransferEncoding.ToEmailMessageTransferEncoding(),
			};
		}
	}
}

