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
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Telephony.MessageBus.CommunicationChannels.DataTransferObjects
{
	[DataContract]
	public class AddIncomingPhoneNumberResponse
	{
		[DataMember(Name = "phoneNumberKey", EmitDefaultValue = false)]
		public string PhoneNumberKey { get; set; }

		[DataMember(Name = "dateCreated", EmitDefaultValue = false)]
		public DateTime? DateCreated { get; set; }

		[DataMember(Name = "dateUpdated", EmitDefaultValue = false)]
		public DateTime? DateUpdated { get; set; }

		[DataMember(Name = "friendlyName", EmitDefaultValue = false)]
		public string FriendlyName { get; set; }

		[DataMember(Name = "accountKey", EmitDefaultValue = false)]
		public string AccountKey { get; set; }

		[DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
		public string PhoneNumber { get; set; }


		[DataMember(Name = "voiceCallerIdLookup", EmitDefaultValue = false)]
		public bool VoiceCallerIdLookup { get; set; }

		[DataMember(Name = "voiceActionHttpVerb", EmitDefaultValue = false)]
		public HttpVerb VoiceActionHttpVerb { get; set; }

		[DataMember(Name = "voiceActionUrl", EmitDefaultValue = false)]
		public string VoiceActionUrl { get; set; }

		[DataMember(Name = "voiceFallbackActionHttpVerb", EmitDefaultValue = false)]
		public HttpVerb VoiceFallbackActionHttpVerb { get; set; }

		[DataMember(Name = "voiceFallbackActionUrl", EmitDefaultValue = false)]
		public string VoiceFallbackActionUrl { get; set; }

		[DataMember(Name = "voiceApplicationKey", EmitDefaultValue = false)]
		public string VoiceApplicationKey { get; set; }

		[DataMember(Name = "statusCallbackActionHttpVerb", EmitDefaultValue = false)]
		public HttpVerb StatusCallbackActionHttpVerb { get; set; }

		[DataMember(Name = "statusCallbackActionUrl", EmitDefaultValue = false)]
		public string StatusCallbackActionUrl { get; set; }

		[DataMember(Name = "smsActionHttpVerb", EmitDefaultValue = false)]
		public HttpVerb SmsActionHttpVerb { get; set; }

		[DataMember(Name = "smsActionUrl", EmitDefaultValue = false)]
		public string SmsActionUrl { get; set; }

		[DataMember(Name = "smsFallbackActionHttpVerb", EmitDefaultValue = false)]
		public HttpVerb SmsFallbackActionHttpVerb { get; set; }

		[DataMember(Name = "smsFallbackActionUrl", EmitDefaultValue = false)]
		public string SmsFallbackActionUrl { get; set; }

		[DataMember(Name = "smsApplicationKey", EmitDefaultValue = false)]
		public string SmsApplicationKey { get; set; }

		[DataMember(Name = "smsEnabled", EmitDefaultValue = false)]
		public bool SmsEnabled { get; set; }

		[DataMember(Name = "mmsEnabled", EmitDefaultValue = false)]
		public bool MmsEnabled { get; set; }

		[DataMember(Name = "voiceEnabled", EmitDefaultValue = false)]
		public bool VoiceEnabled { get; set; }

		[DataMember(Name = "success", EmitDefaultValue = false)]
		public bool Success { get; set; }
	}
}
