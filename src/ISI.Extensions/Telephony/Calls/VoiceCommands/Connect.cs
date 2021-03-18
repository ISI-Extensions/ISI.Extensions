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

namespace ISI.Extensions.Telephony.Calls.VoiceCommands
{
	public class Connect : IVoiceCommand, IHasDials
	{
		public string CallerId { get; set; }

		public ISI.Extensions.HttpVerb ActionHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string ActionUrl { get; set; }

		public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

		public bool HangupOnStar { get; set; } = false;

		public TimeSpan CallTimeLimit { get; set; } = TimeSpan.FromHours(4);

		public RecordType RecordingType { get; set; } = RecordType.DoNotRecord;
		public RecordingTrimType RecordingTrim { get; set; } = RecordingTrimType.DoNotTrim;
		public RecordingStatusCallbackEvent? RecordingStatusCallbackEvents { get; set; } = null;
		public ISI.Extensions.HttpVerb RecordingStatusCallbackHttpVerb { get; set; } = ISI.Extensions.HttpVerb.Post;
		public string RecordingStatusCallbackUrl { get; set; }

		public bool AnswerOnBridge { get; set; } = false;

		private IList<IDial> _dials = null;
		public IList<IDial> Dials
		{
			get => _dials ??= new List<IDial>();
			set => _dials = value;
		}
	}

	internal class ConnectFluent : Connect, IHasVoiceCommands
	{
		private IList<IVoiceCommand> _voiceCommands { get; }
		IList<IVoiceCommand> IHasVoiceCommands.Commands => _voiceCommands;

		internal ConnectFluent(IList<IVoiceCommand> voiceCommands)
		{
			_voiceCommands = voiceCommands;
		}
	}
}

