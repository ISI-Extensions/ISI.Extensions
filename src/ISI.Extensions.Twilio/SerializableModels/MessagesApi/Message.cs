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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Twilio.SerializableModels.MessagesApi
{
	[DataContract]
	public class Message
	{
		public ISI.Extensions.Twilio.Message Export()
		{
			return new ISI.Extensions.Twilio.Message()
			{
				AccountKey = AccountSid,
				MessageKey = Sid,
				From = From,
				To = To,
				Body = Body,
				NumSegments = NumSegments.ToInt(),
				NumMedia = NumMedia.ToInt(),
				MessageStatus = ISI.Extensions.Enum<ISI.Extensions.Telephony.Messages.MessageStatus?>.Convert(Status),
				Direction = ISI.Extensions.Enum<ISI.Extensions.Telephony.Messages.Direction?>.Convert(Direction),
				Price = Price,
				Uri = Uri,
				CreatedDateTimeUtc = DateCreated,
				UpdatedDateTimeUtc = DateUpdated,
				SentDateTimeUtc = DateSent,
			};
		}

		[DataMember(Name = "body")]
		public string Body { get; set; }

		[DataMember(Name = "num_segments")]
		public string NumSegments { get; set; }


		[IgnoreDataMember]
		public Direction Direction { get; set; }
		[DataMember(Name = "direction")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string __Direction { get => Direction.GetAbbreviation(); set => Direction = ISI.Extensions.Enum<Direction>.Parse(value); }


		[DataMember(Name = "from")]
		public string From { get; set; }

		[DataMember(Name = "to")]
		public string To { get; set; }

		[IgnoreDataMember]
		public DateTime? DateUpdated { get; set; }
		[DataMember(Name = "date_updated")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string __DateUpdated { get => DateUpdated.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => DateUpdated = value.ToDateTimeNullable(); }


		[DataMember(Name = "price")]
		public string Price { get; set; }


		[DataMember(Name = "error_message")]
		public string ErrorMessage { get; set; }


		[DataMember(Name = "uri")]
		public string Uri { get; set; }


		[DataMember(Name = "account_sid")]
		public string AccountSid { get; set; }


		[DataMember(Name = "num_media")]
		public string NumMedia { get; set; }


		[IgnoreDataMember]
		public MessageStatus Status { get; set; }
		[DataMember(Name = "status")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string __Status { get => Status.GetAbbreviation(); set => Status = ISI.Extensions.Enum<MessageStatus>.Parse(value); }


		[DataMember(Name = "messaging_service_sid")]
		public string MessagingServiceSid { get; set; }


		[DataMember(Name = "sid")]
		public string Sid { get; set; }


		[IgnoreDataMember]
		public DateTime? DateSent { get; set; }
		[DataMember(Name = "date_sent")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string __DateSent { get => DateSent.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => DateSent = value.ToDateTimeNullable(); }


		[IgnoreDataMember]
		public DateTime? DateCreated { get; set; }
		[DataMember(Name = "date_created")]
		[System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public string __DateCreated { get => DateCreated.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => DateCreated = value.ToDateTimeNullable(); }


		[DataMember(Name = "error_code")]
		public int? ErrorCode { get; set; }


		[DataMember(Name = "price_unit")]
		public string PriceUnit { get; set; }


		[DataMember(Name = "api_version")]
		public string ApiVersion { get; set; }
	}
}