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

namespace ISI.Extensions.Telephony.Extensions
{
	public static partial class VoiceCommandExtensions
	{
		public static Calls.VoiceCommands.IHasVoiceCommands Connect(this Calls.VoiceResponse voiceResponse,
			string callerId,
			ISI.Extensions.HttpVerb actionHttpVerb = ISI.Extensions.HttpVerb.Post,
			string actionUrl = null,
			TimeSpan? timeout = null,
			bool hangupOnStar = false,
			TimeSpan? callTimeLimit = null,
			Calls.RecordType recordingType = Calls.RecordType.DoNotRecord,
			Calls.RecordingTrimType recordingTrim = Calls.RecordingTrimType.DoNotTrim,
			Calls.RecordingStatusCallbackEvent? recordingStatusCallbackEvents = null,
			ISI.Extensions.HttpVerb recordingStatusCallbackHttpVerb = ISI.Extensions.HttpVerb.Post,
			string recordingStatusCallbackUrl = null,
			bool answerOnBridge = false)
		{
			var connect = new Calls.VoiceCommands.ConnectFluent(new List<Calls.VoiceCommands.IVoiceCommand>())
			{
				CallerId = callerId,
				ActionHttpVerb = actionHttpVerb,
				ActionUrl = actionUrl,
				Timeout = timeout ?? (new Calls.VoiceCommands.Connect()).Timeout,
				HangupOnStar = hangupOnStar,
				CallTimeLimit = callTimeLimit ?? (new Calls.VoiceCommands.Connect()).CallTimeLimit,
				RecordingType = recordingType,
				RecordingTrim = recordingTrim,
				RecordingStatusCallbackEvents = recordingStatusCallbackEvents,
				RecordingStatusCallbackHttpVerb = recordingStatusCallbackHttpVerb,
				RecordingStatusCallbackUrl = recordingStatusCallbackUrl,
				AnswerOnBridge = answerOnBridge,
			};

			voiceResponse.Commands.Add(connect);

			return connect;
		}

		public static Calls.VoiceCommands.IHasVoiceCommands Connect(this Calls.VoiceResponse voiceResponse, Calls.VoiceCommands.Connect connect)
		{
			connect = new Calls.VoiceCommands.ConnectFluent(new List<Calls.VoiceCommands.IVoiceCommand>())
			{
				CallerId = connect.CallerId,
				ActionHttpVerb = connect.ActionHttpVerb,
				ActionUrl = connect.ActionUrl,
				Timeout = connect.Timeout,
				HangupOnStar = connect.HangupOnStar,
				CallTimeLimit = connect.CallTimeLimit,
				RecordingType = connect.RecordingType,
				RecordingTrim = connect.RecordingTrim,
				RecordingStatusCallbackEvents = connect.RecordingStatusCallbackEvents,
				RecordingStatusCallbackHttpVerb = connect.RecordingStatusCallbackHttpVerb,
				RecordingStatusCallbackUrl = connect.RecordingStatusCallbackUrl,
				AnswerOnBridge = connect.AnswerOnBridge,
			};

			voiceResponse.Commands.Add(connect);

			return (Calls.VoiceCommands.IHasVoiceCommands)connect;
		}
	}
}
