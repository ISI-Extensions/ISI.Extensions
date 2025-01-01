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
	public class Email : IEmail
	{
		public Guid EmailUuid { get; set; }
		public EmailStatus EmailStatus { get; set; }
		public IEmailMailMessage EmailMailMessage { get; set; }

		public Guid? EmailProviderUuid { get; set; }
		public string EmailProviderDeliveryChannelName { get; set; }
		private InvariantCultureIgnoreCaseStringDictionary<string> _emailProviderMetadata = null;
		public IDictionary<string, string> EmailProviderMetadata { get => _emailProviderMetadata ??= new(); set => _emailProviderMetadata = new(value.ToNullCheckedDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value, NullCheckDictionaryResult.Empty)); }
		public string BillingAccountNumber { get; set; }

		public bool Track { get; set; }
		public bool TrackOpens { get; set; }
		public bool TrackClicks { get; set; }
		public string TrackingDomain { get; set; }
		public string TrackingSigningDomain { get; set; }
		public string TrackingReturnPathDomain { get; set; }
		public string[] TrackingTags { get; set; }
		public string[] TrackingGoogleAnalyticsDomains { get; set; }
		public string[] TrackingGoogleAnalyticsCampaigns { get; set; }

		public DateTime? ScheduledSendDateTimeUtc { get; set; }
		public DateTime? LastAttemptSendDateTimeUtc { get; set; }
		public int? SendAttemptCount { get; set; }
		public DateTime? SentDateTimeUtc { get; set; }

		public IEmailSenderResponse EmailSenderResponse { get; set; }

		public ISI.Extensions.UserKey CreateUserKey { get; set; }
		public DateTime CreateDateTimeUtc { get; set; }
		public ISI.Extensions.UserKey ModifyUserKey { get; set; }
		public DateTime ModifyDateTimeUtc { get; set; }

		public static string EncodeToSubjectLineQuotedPrintable(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}

			var quotedPrintable = new StringBuilder();

			var toEncode = new List<byte>();

			void AddEncoded()
			{
				if (toEncode.Any())
				{
					quotedPrintable.AppendFormat("=?utf-8?Q?={0}?=", string.Join("=", toEncode.Select(v => string.Format("{0:X2}", v))));
					toEncode.Clear();
				}
			}

			var bytes = Encoding.UTF8.GetBytes(value);
			foreach (var v in bytes)
			{
				// The following are not required to be encoded:
				// - Tab (ASCII 9)
				// - Space (ASCII 32)
				// - Characters 33 to 126, except for the equal sign (61).

				if ((v == 9) || ((v >= 32) && (v <= 60)) || ((v >= 62) && (v <= 126)))
				{
					AddEncoded();
					quotedPrintable.Append(Convert.ToChar(v));
				}
				else
				{
					toEncode.Add(v);
				}
			}

			AddEncoded();

			return quotedPrintable.ToString();
		}
	}
}
