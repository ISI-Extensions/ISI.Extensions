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
using ISI.Extensions.Emails.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Emails
{
	public class EmailAddress : IEmailAddress
	{
		public const string CaptionRegexPattern = "(?:[a-zA-Z\\d!#$%&\'*\\(\\)\\-/=?^_`{|}~]+\\x20*|\"(?:(?=[\\x01-\\x7f])[^\"\\\\]|\\\\[\\x01-\\x7f])*\"\\x20*)*";
		public const string AddressRegexPattern = "(?<EmailName>(?!\\.)(?>\\.?[a-zA-Z\\d!#$%&\'*+\\-/=?^_`{|}~]+)+|\\\"(?:(?=[\\x01-\\x7f])[^\\\"\\\\]|\\\\[\\x01-\\x7f])*\\\")@(?<EmailDomain>(?:(?!-)[a-zA-Z\\d\\-]+(?<!-)\\.)+[a-zA-Z]{2,}|\\[(?:(?:(?(?<!\\[)\\.)(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)){4}|[a-zA-Z\\d\\-]*[a-zA-Z\\d]:(?:(?=[\\x01-\\x7f])[^\\\\\\[\\]]|\\\\[\\x01-\\x7f])+)\\])";
		public static readonly string EmailAddressRegexPattern = "^(?:(?<EmailCaption>(?>" + CaptionRegexPattern + ")*)(?<angle><))?" + AddressRegexPattern + "(?(angle)>)$";
				
		private static readonly System.Text.RegularExpressions.Regex EmailAddressRegex = new(EmailAddressRegexPattern, System.Text.RegularExpressions.RegexOptions.ECMAScript | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

		private string _address = string.Empty;
		public string Address
		{
			get => (_address ?? string.Empty).Trim();
			set => _address = value;
		}

		private string _caption = string.Empty;
		public string Caption
		{
			get => (_caption ?? string.Empty).Trim();
			set => _caption = value;
		}

		#region Constructors
		public EmailAddress()
		{
			Address = string.Empty;
			Caption = string.Empty;
		}
		public EmailAddress(string value) : this()
		{
			Value = value;
		}
		public EmailAddress(string address, string caption) : this()
		{
			SetValue(address, caption);
		}
		public EmailAddress(System.Net.Mail.MailAddress mailAddress)
			: this()
		{
			SetValue(mailAddress.Address, mailAddress.DisplayName);
		}
		public EmailAddress(IEmailAddress emailAddress)
			: this()
		{
			SetValue(emailAddress.Address, emailAddress.Caption);
		}
		#endregion

		#region SetValue
		public void SetValue(string address, string caption)
		{
			var matchEmailAddress = EmailAddressRegex.Match(address ?? string.Empty);

			if (matchEmailAddress.Success)
			{
				Address = string.Format("{0}@{1}", matchEmailAddress.Groups["EmailName"], matchEmailAddress.Groups["EmailDomain"]).Trim();

				if (matchEmailAddress.Groups.TryGetValue("EmailCaption", out var emailCaptionGroup))
				{
					Caption = (emailCaptionGroup.Value ?? string.Empty).Trim();
					if (!string.IsNullOrEmpty(caption))
					{
						Caption = caption;
					}
				}
				else
				{
					Caption = caption ?? string.Empty;
				}
			}
			else
			{
				Address = string.Empty;
				Caption = string.Empty;
			}
		}
		#endregion

		#region Value
		public string Value
		{
			get => this.Formatted();
			set => SetValue(value, string.Empty);
		}
		#endregion

		public bool IsValid()
		{
			return EmailAddressRegex.Match(Address ?? string.Empty).Success;
		}

		public IEmailAddress Clone()
		{
			return new EmailAddress()
			{
				Address = _address,
				Caption = _caption
			};
		}

		public override string ToString() => this.Formatted();

		public System.Net.Mail.MailAddress ToMailAddress()
		{
			return new System.Net.Mail.MailAddress(Address, Caption);
		}

		public static IEmailAddress Create(string address, string caption)
		{
			if (!string.IsNullOrWhiteSpace(address))
			{
				return new EmailAddress(address, caption);
			}

			return null;
		}

		public static bool TryCreateEmailAddress(string value, out IEmailAddress emailAddress)
		{
			var matchEmailAddress = EmailAddressRegex.Match(value ?? string.Empty);

			if (matchEmailAddress.Success)
			{
				var address = string.Format("{0}@{1}", matchEmailAddress.Groups["EmailName"], matchEmailAddress.Groups["EmailDomain"]).Trim();

				if (matchEmailAddress.Groups.TryGetValue("EmailCaption", out var emailCaptionGroup))
				{
					emailAddress = new EmailAddress(address, (emailCaptionGroup.Value ?? string.Empty).Trim());
				}
				else
				{
					emailAddress = new EmailAddress(address, string.Empty);
				}

				return true;
			}

			emailAddress = null;
			return false;
		}

		public static EmailAddress Create(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return new EmailAddress(value);
			}

			return null;
		}

		public static EmailAddress Create(System.Net.Mail.MailAddress mailAddress)
		{
			if (mailAddress != null)
			{
				return new EmailAddress(mailAddress);
			}

			return null;
		}
		public static IEmailAddress Create(IEmailAddress emailAddress)
		{
			if (emailAddress != null)
			{
				return new EmailAddress(emailAddress);
			}

			return null;
		}

		public static bool IsEmailAddress(string value)
		{
			var matchEmailAddress = EmailAddressRegex.Match((value ?? string.Empty).Trim());

			return matchEmailAddress.Success;
		}

		public static string GetEmailAddress(string value)
		{
			var result = string.Empty;

			var matchEmailAddress = EmailAddressRegex.Match(value ?? string.Empty);

			if (matchEmailAddress.Success)
			{
				result = string.Format("{0}@{1}", matchEmailAddress.Groups["EmailName"], matchEmailAddress.Groups["EmailDomain"]);
			}

			return result;
		}

		public static bool operator ==(EmailAddress a, EmailAddress b)
		{
			a ??= new EmailAddress();
			b ??= new EmailAddress();

			return (a.Address ?? string.Empty).Trim().Equals((b.Address ?? string.Empty).Trim(), StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool operator !=(EmailAddress a, EmailAddress b) => !(a == b);
		public override bool Equals(object o) => (this == (EmailAddress)o);
		public override int GetHashCode() => Value.ToLower().GetHashCode();

		public static implicit operator EmailAddress(string value) => new(value);
	}
}
