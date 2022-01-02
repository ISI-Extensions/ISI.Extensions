#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Telephony.Calls.VoiceCommands
{
	public class DialConference : IDial
	{
		public string Conference { get; set; }

		public bool Muted { get; set; } = false;

		public PlayBeep PlayBeep { get; set; } = PlayBeep.NoBeep;

		public bool StartConferenceOnEnter { get; set; } = true;
		public bool EndConferenceOnExit { get; set; } = false;

		public ISI.Extensions.HttpVerb WaitHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string WaitUrl { get; set; }

		public int MaxParticipants { get; set; } = 250;

		public RecordType RecordingType { get; set; } = RecordType.DoNotRecord;
		public RecordingTrimType RecordingTrim { get; set; } = RecordingTrimType.DoNotTrim;
		public RecordingStatusCallbackEvent? RecordingStatusCallbackEvents { get; set; } = null;
		public ISI.Extensions.HttpVerb RecordingStatusCallbackHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string RecordingStatusCallbackUrl { get; set; }

		//public ISI.Extensions.HttpVerb EventCallbackHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string EventCallbackUrl { get; set; }

		public ConferenceStatusCallbackEvent? ConferenceStatusCallbackEvents { get; set; } = null;
		public ISI.Extensions.HttpVerb ConferenceStatusCallbackHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string ConferenceStatusCallbackUrl { get; set; }
	}

	internal class DialConferenceFluent : DialConference, IHasVoiceCommands, IHasDials
	{
		private IList<IVoiceCommand> _voiceCommands { get; }
		IList<IVoiceCommand> IHasVoiceCommands.Commands => _voiceCommands;

		private IList<IDial> _dialTos { get; }
		IList<IDial> IHasDials.Dials => _dialTos;

		internal DialConferenceFluent(IList<IVoiceCommand> voiceCommands, IList<IDial> dialTos)
		{
			_voiceCommands = voiceCommands;
			_dialTos = dialTos;
		}
	}
}
