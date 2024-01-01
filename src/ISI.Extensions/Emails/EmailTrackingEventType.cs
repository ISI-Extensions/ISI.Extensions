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

namespace ISI.Extensions.Emails
{
	public enum EmailTrackingEventType
	{
		[ISI.Extensions.EnumGuid("1cbc1782-c106-4278-9005-041220504178")] SendAttempt,
		[ISI.Extensions.EnumGuid("d727aa1a-658b-41fd-8eb9-073fe64bfeab", "Send", Aliases = new[] { "send" })] Send,
		[ISI.Extensions.EnumGuid("4d9bbd0d-89ef-4e68-8a1f-6d0228e33a6a", "Deferral", Aliases = new[] { "deferral" })] Deferral,
		[ISI.Extensions.EnumGuid("4e957079-b81a-43b9-96c3-2ba10274bf83", "Hard Bounce", Aliases = new[] { "hard_bounce", "hard-bounce" })] HardBounce,
		[ISI.Extensions.EnumGuid("bb4fa2c6-3de8-475d-be16-a88d6119e14f", "Soft Bounce", Aliases = new[] { "soft_bounce", "soft-bounce" })] SoftBounce,
		[ISI.Extensions.EnumGuid("33ade975-eeae-4050-9c0e-5c2ee65b0b90", "Open", Aliases = new[] { "open" })] Open,
		[ISI.Extensions.EnumGuid("410d5b5c-902b-49af-8b33-3567c07ebae8", "Click", Aliases = new[] { "click" })] Click,
		[ISI.Extensions.EnumGuid("ae1da485-2e46-447e-95db-0b8d4627c14f", "Spam", Aliases = new[] { "spam" })] Spam,
		[ISI.Extensions.EnumGuid("7f461a82-5185-4cb7-9390-fe16270006f6", "Unsubscribe", Aliases = new[] { "unsub" })] Unsubscribe,
		[ISI.Extensions.EnumGuid("a2e13ad7-c7bd-4aea-81ce-15dcedd32bb5", "Reject", Aliases = new[] { "reject" })] Reject,
		[ISI.Extensions.EnumGuid("2e43d8c1-27ea-4803-8c0a-a413d08c1d18")] HasBadEmailAddress,
	}
}
