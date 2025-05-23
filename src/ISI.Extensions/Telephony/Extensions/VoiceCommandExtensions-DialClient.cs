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

namespace ISI.Extensions.Telephony.Extensions
{
	public static partial class VoiceCommandExtensions
	{
		public static Calls.VoiceCommands.IHasVoiceCommands DialClient(this Calls.VoiceCommands.IHasVoiceCommands hasVoiceCommands, 
			string client,
		  ISI.Extensions.HttpVerb actionHttpVerb = ISI.Extensions.HttpVerb.Post,
	    string actionUrl = null,
		  Calls.StatusCallbackEvent? statusCallbackEvents = null,
		  ISI.Extensions.HttpVerb statusCallbackHttpVerb = ISI.Extensions.HttpVerb.Post,
		  string statusCallbackUrl = null)
		{
			var hasDials = GetOrCreateHasDialTos(hasVoiceCommands);

			var dialClient = new Calls.VoiceCommands.DialClientFluent(hasVoiceCommands.Commands, hasDials.Dials)
			{
				Client = client,
				ActionHttpVerb = actionHttpVerb,
				ActionUrl = actionUrl,
				StatusCallbackEvents = statusCallbackEvents,
				StatusCallbackHttpVerb = statusCallbackHttpVerb,
				StatusCallbackUrl = statusCallbackUrl,
			};

			hasDials.Dials.Add(dialClient);

			return dialClient;
		}

		public static Calls.VoiceCommands.IHasVoiceCommands DialClient(this Calls.VoiceCommands.IHasVoiceCommands hasVoiceCommands, Calls.VoiceCommands.DialClient client)
		{
			var hasDials = GetOrCreateHasDialTos(hasVoiceCommands);

			var dialClient = new Calls.VoiceCommands.DialClientFluent(hasVoiceCommands.Commands, hasDials.Dials)
			{
				Client = client.Client,
				ActionHttpVerb = client.ActionHttpVerb,
				ActionUrl = client.ActionUrl,
				StatusCallbackEvents = client.StatusCallbackEvents,
				StatusCallbackHttpVerb = client.StatusCallbackHttpVerb,
				StatusCallbackUrl = client.StatusCallbackUrl,
			};

			hasDials.Dials.Add(dialClient);

			return dialClient;
		}
	}
}