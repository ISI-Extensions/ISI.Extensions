#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Telephony.DataTransferObjects.CommunicationChannelsApi
{
	public class AddIncomingPhoneNumberRequest
	{
		public string OperationKey { get; set; }
		public string RequestKey { get; set; }

		public string PhoneNumber { get; set; }
		public string PhoneNumberAccountKey { get; set; }
		public string FriendlyName { get; set; }

		public string TrunkKey { get; set; }

		public bool? VoiceCallerIdLookup { get; set; }

		public ISI.Extensions.Telephony.CommunicationChannels.IncomingCallReceiveMode? IncomingCallReceiveMode { get; set; }
		public string IncomingCallApplicationKey { get; set; }

		public string IncomingCallHandlerKey { get; set; }

		public ISI.Extensions.HttpVerb? IncomingCallMethod { get; set; }
		public string IncomingCallUrl { get; set; }

		public ISI.Extensions.HttpVerb? IncomingCallFallbackMethod { get; set; }
		public string IncomingCallFallbackUrl { get; set; }

		public ISI.Extensions.HttpVerb? CallStatusUrlMethod { get; set; }
		public string CallStatusUrl { get; set; }

		public string IncomingMessageHandlerKey { get; set; }
		public string IncomingMessageApplicationKey { get; set; }
		public ISI.Extensions.HttpVerb? IncomingMessageMethod { get; set; }
		public string IncomingMessageUrl { get; set; }

		public ISI.Extensions.HttpVerb? IncomingMessageFallbackMethod { get; set; }
		public string IncomingMessageFallbackUrl { get; set; }
	}
}
