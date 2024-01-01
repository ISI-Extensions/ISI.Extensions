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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Telephony.CommunicationChannels
{
	public class IncomingPhoneNumber
	{
		public string PhoneNumberKey { get; set; }
		public string PhoneNumberAccountKey { get; set; }

		public string FriendlyName { get; set; }
		public string PhoneNumber { get; set; }

		public Origin Origin { get; set; }
		public string TrunkKey { get; set; }

		public bool VoiceCallerIdLookup { get; set; }

		public IncomingCallReceiveMode IncomingCallReceiveMode { get; set; }
		public string IncomingCallApplicationKey { get; set; }
		public ISI.Extensions.HttpVerb IncomingCallUrlMethod { get; set; }
		public string IncomingCallUrl { get; set; }
		public ISI.Extensions.HttpVerb IncomingCallFallbackUrlMethod { get; set; }
		public string IncomingCallFallbackUrl { get; set; }
		public ISI.Extensions.HttpVerb CallStatusUrlMethod { get; set; }
		public string CallStatusUrl { get; set; }

		public string MessagingApplicationKey { get; set; }
		public ISI.Extensions.HttpVerb IncomingMessagingUrlMethod { get; set; }
		public string IncomingMessagingUrl { get; set; }
		public ISI.Extensions.HttpVerb IncomingMessagingFallbackUrlMethod { get; set; }
		public string IncomingMessagingFallbackUrl { get; set; }

		public AddressRequirement AddressRequirements { get; set; }

		public bool VoiceEnabled { get; set; }
		public bool SmsEnabled { get; set; }
		public bool MmsEnabled { get; set; }
		public bool FaxEnabled { get; set; }

		public bool Beta { get; set; }

		public EmergencyStatus EmergencyStatus { get; set; }
		public string EmergencyAddressKey { get; set; }

		public DateTime? CreateDateTimeUtc { get; set; }
		public DateTime? ModifyDateTimeUtc { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", FriendlyName, PhoneNumber);
		}
	}
}
