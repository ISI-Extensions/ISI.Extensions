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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Telephony.MessageBus.Calls.VoiceCommands
{
	[DataContract]
	public class Connect
	{
		public Connect()
		{
			ActionHttpVerb = ISI.Extensions.Telephony.MessageBus.HttpVerb.Post;
			TimeoutInSeconds = 30;
			CallTimeLimitInSeconds = 14400;
			HangupOnStar = false;
			Record = RecordType.DoNotRecord;
			Trim = TrimType.DoNotTrim;
		}

		[DataMember(Name = "actionHttpVerb")]
		public ISI.Extensions.Telephony.MessageBus.HttpVerb ActionHttpVerb { get; set; }

		[DataMember(Name = "actionUrl")]
		public string ActionUrl { get; set; }

		[DataMember(Name = "timeoutInSeconds")]
		public int TimeoutInSeconds { get; set; }

		[DataMember(Name = "hangupOnStar")]
		public bool HangupOnStar { get; set; }

		[DataMember(Name = "timeLimitInSeconds")]
		public int CallTimeLimitInSeconds { get; set; }

		[DataMember(Name = "callerId")]
		public string CallerId { get; set; }

		[DataMember(Name = "record")]
		public RecordType Record { get; set; }

		[DataMember(Name = "trim")]
		public TrimType Trim { get; set; }

		[DataMember(Name = "number")]
		public string Number { get; set; }

		[DataMember(Name = "sendDigits")]
		public string SendDigits { get; set; }

		[DataMember(Name = "sipAddress")]
		public string SipAddress { get; set; }

		[DataMember(Name = "sipHeaders")]
		public SipHeader[] SipHeaders { get; set; }
	}
}
