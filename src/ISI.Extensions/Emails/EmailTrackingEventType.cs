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

namespace ISI.Extensions.Emails
{
	public enum EmailTrackingEventType
	{
		SendAttempt = 100,
		[ISI.Extensions.Enum("Send", Aliases = new string[] { "send" })] Send = 101,
		[ISI.Extensions.Enum("Deferral", Aliases = new string[] { "deferral" })] Deferral = 102,
		[ISI.Extensions.Enum("Hard Bounce", Aliases = new string[] { "hard_bounce", "hard-bounce" })] HardBounce = 103,
		[ISI.Extensions.Enum("Soft Bounce", Aliases = new string[] { "soft_bounce", "soft-bounce" })] SoftBounce = 104,
		[ISI.Extensions.Enum("Open", Aliases = new string[] { "open" })] Open = 105,
		[ISI.Extensions.Enum("Click", Aliases = new string[] { "click" })] Click = 106,
		[ISI.Extensions.Enum("Spam", Aliases = new string[] { "spam" })] Spam = 107, 
		[ISI.Extensions.Enum("Unsubscribe", Aliases = new string[] { "unsub" })] Unsubscribe = 108,
		[ISI.Extensions.Enum("Reject", Aliases = new string[] { "reject" })] Reject = 109,
		HasBadEmailAddress = 110
	}
}
