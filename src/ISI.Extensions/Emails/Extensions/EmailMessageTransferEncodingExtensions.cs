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

namespace ISI.Extensions.Emails.Extensions
{
	public static class EmailMessageTransferEncodingExtensions
	{
		public static System.Net.Mime.TransferEncoding ToTransferEncoding(this EmailMessageTransferEncoding emailMessageTransferEncoding)
		{
			switch (emailMessageTransferEncoding)
			{
				case EmailMessageTransferEncoding.Unknown:
					return System.Net.Mime.TransferEncoding.Unknown;
				case EmailMessageTransferEncoding.QuotedPrintable:
					return System.Net.Mime.TransferEncoding.QuotedPrintable;
				case EmailMessageTransferEncoding.Base64:
					return System.Net.Mime.TransferEncoding.Base64;
				case EmailMessageTransferEncoding.SevenBit:
					return System.Net.Mime.TransferEncoding.SevenBit;
				case EmailMessageTransferEncoding.EightBit:
					return System.Net.Mime.TransferEncoding.EightBit;
				default:
					throw new ArgumentOutOfRangeException(nameof(emailMessageTransferEncoding), emailMessageTransferEncoding, null);
			}
		}

		public static EmailMessageTransferEncoding ToEmailMessageTransferEncoding(this System.Net.Mime.TransferEncoding transferEncoding)
		{
			switch (transferEncoding)
			{
				case System.Net.Mime.TransferEncoding.Unknown:
					return EmailMessageTransferEncoding.Unknown;
				case System.Net.Mime.TransferEncoding.QuotedPrintable:
					return EmailMessageTransferEncoding.QuotedPrintable;
				case System.Net.Mime.TransferEncoding.Base64:
					return EmailMessageTransferEncoding.Base64;
				case System.Net.Mime.TransferEncoding.SevenBit:
					return EmailMessageTransferEncoding.SevenBit;
				case System.Net.Mime.TransferEncoding.EightBit:
					return EmailMessageTransferEncoding.EightBit;
				default:
					throw new ArgumentOutOfRangeException(nameof(transferEncoding), transferEncoding, null);
			}
		}
	}
}
