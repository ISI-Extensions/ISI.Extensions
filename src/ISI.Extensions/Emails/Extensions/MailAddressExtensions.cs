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

namespace ISI.Extensions.Emails.Extensions
{
	public static class MailAddressExtensions
	{
		public static string Formatted(this IEmailAddress emailAddress)
		{
			var caption = emailAddress?.Caption?.Trim();
			var address = (emailAddress?.Address ?? string.Empty).Trim();

			return string.IsNullOrEmpty(caption) ? address : $"{caption}<{address}>";
		}

		public static System.Net.Mail.MailAddress ToMailAddress(this IEmailAddress emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress.Caption))
			{
				return new System.Net.Mail.MailAddress(emailAddress.Address);
			}

			return new System.Net.Mail.MailAddress(emailAddress.Address, emailAddress.Caption);
		}

		public static IEmailAddress ToEmailAddress(this System.Net.Mail.MailAddress mailAddress)
		{
			if (string.IsNullOrWhiteSpace(mailAddress.DisplayName))
			{
				return new EmailAddress(mailAddress.Address);
			}

			return new EmailAddress(mailAddress.Address, mailAddress.DisplayName);
		}
	}
}
