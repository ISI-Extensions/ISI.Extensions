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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Mandrill.SerializableModels.MandrillMessagesApi
{
	[DataContract]
	public class SendEmailRequestMessage
	{
		[DataMember(Name = "html", Order = 1)]
		public string Mhtml { get; set; }

		[DataMember(Name = "text", Order = 2)]
		public string PlainText { get; set; }

		[DataMember(Name = "subject", Order = 3)]
		public string __Subject { get => (ISI.Extensions.StringFormat.HasNonAsciiCharacters(Subject) ? ISI.Extensions.Emails.Email.EncodeToSubjectLineQuotedPrintable(Subject) : Subject); set => Subject = value; }
		[IgnoreDataMember]
		public string Subject { get; set; }

		[DataMember(Name = "from_email", Order = 4)]
		public string FromEmailAddress { get; set; }

		[DataMember(Name = "from_name", EmitDefaultValue = false, Order = 5)]
		public string FromEmailAddressCaption { get; set; }

		[DataMember(Name = "to", Order = 6)]
		public SendEmailRequestMessageEmailAddress[] To { get; set; }

		[DataMember(Name = "headers", Order = 7)]
		public SendEmailRequestMessageHeaders Headers { get; set; }

		[DataMember(Name = "important", EmitDefaultValue = false, Order = 8)]
		public bool Important { get; set; }

		[DataMember(Name = "track_opens", EmitDefaultValue = false, Order = 9)]
		public bool TrackOpens { get; set; }

		[DataMember(Name = "track_clicks", EmitDefaultValue = false, Order = 10)]
		public bool TrackClicks { get; set; }

		[DataMember(Name = "auto_text", EmitDefaultValue = false, Order = 11)]
		public bool AutoPlainText { get; set; }

		[DataMember(Name = "auto_html", EmitDefaultValue = false, Order = 12)]
		public bool AutoMhtml { get; set; }

		[DataMember(Name = "inline_css", EmitDefaultValue = false, Order = 13)]
		public bool InlineCss { get; set; }

		[DataMember(Name = "url_strip_qs", EmitDefaultValue = false, Order = 14)]
		public bool StripUrlQueryStringWhenAggregating { get; set; }

		[DataMember(Name = "preserve_recipients", EmitDefaultValue = false, Order = 15)]
		public bool PreserveRecipients { get; set; }

		[DataMember(Name = "bcc_address", EmitDefaultValue = false, Order = 16)]
		public string AuditCopyEmailAddress { get; set; }

		[DataMember(Name = "tracking_domain", EmitDefaultValue = false, Order = 17)]
		public string TrackingDomain { get; set; }

		[DataMember(Name = "signing_domain", EmitDefaultValue = false, Order = 18)]
		public string SigningDomain { get; set; }

		[DataMember(Name = "return_path_domain", EmitDefaultValue = false, Order = 19)]
		public string ReturnPathDomain { get; set; }

		[DataMember(Name = "merge", EmitDefaultValue = false, Order = 20)]
		public bool? IsMailMerge { get; set; }

		[DataMember(Name = "global_merge_vars", EmitDefaultValue = false, Order = 21)]
		public SendEmailRequestMessageMergeVariable[] GlobalMergeVariables { get; set; }

		[DataMember(Name = "merge_vars", EmitDefaultValue = false, Order = 22)]
		public SendEmailRequestMessageRecipientMergeVariables[] RecipientMergeVariables { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false, Order = 23)]
		public string[] Tags { get; set; }

		[DataMember(Name = "subaccount", EmitDefaultValue = false, Order = 24)]
		public string SubAccount { get; set; }

		[DataMember(Name = "google_analytics_domains", EmitDefaultValue = false, Order = 25)]
		public string[] GoogleAnalyticsDomains { get; set; }

		[DataMember(Name = "google_analytics_campaign", EmitDefaultValue = false, Order = 26)]
		public string[] GoogleAnalyticsCampaigns { get; set; }

		[DataMember(Name = "metadata", EmitDefaultValue = false, Order = 27)]
		public Dictionary<string, string> Metadata { get; set; }

		[DataMember(Name = "recipient_metadata", EmitDefaultValue = false, Order = 28)]
		public SendEmailRequestMessageRecipientMetadata[] RecipientMetadata { get; set; }

		[DataMember(Name = "attachments", EmitDefaultValue = false, Order = 29)]
		public SendEmailRequestMessageAttachment[] Attachments { get; set; }

		[DataMember(Name = "images", EmitDefaultValue = false, Order = 30)]
		public SendEmailRequestMessageAttachment[] Images { get; set; }
	}
}