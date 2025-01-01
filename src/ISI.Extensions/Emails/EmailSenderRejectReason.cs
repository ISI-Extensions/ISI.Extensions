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

namespace ISI.Extensions.Emails
{
	public enum EmailSenderRejectReason
	{
		[ISI.Extensions.EnumGuid("7c64d5ad-483c-4839-b499-7a8f45a26a93", "Hard Bounce")] HardBounce,
		[ISI.Extensions.EnumGuid("0e927d0e-7f03-40a5-be59-22eb196c1c28", "Soft Bounce")] SoftBounce,
		[ISI.Extensions.EnumGuid("ab5996d2-ebd1-4fa2-a86b-e1b746f04a6a", "Spam")] Spam,
		[ISI.Extensions.EnumGuid("a42d792f-57fa-4e1d-b403-7ddefb603a74", "Unsubscribe")] Unsubscribe,
		[ISI.Extensions.EnumGuid("537ccbf3-fba7-4940-998d-afdf625ac673", "Custom")] Custom,
		[ISI.Extensions.EnumGuid("f694aa72-b7d0-4292-aab9-ea8f99dc15ac", "Invalid Sender")] InvalidSender,
		[ISI.Extensions.EnumGuid("558aea16-c37c-4a94-9cca-1784c3e48237", "Invalid")] Invalid,
		[ISI.Extensions.EnumGuid("c9464685-9d33-42ac-ba72-2ac254b199ba", "Test Mode Limit")] TestModeLimit,
		[ISI.Extensions.EnumGuid("810ad6ec-529d-461b-99c6-7f5cd22a7384", "Unsigned")] Unsigned,
		[ISI.Extensions.EnumGuid("7a2b3abb-eb20-4bb1-950c-82a1ebc8ec2c", "Rule")] Rule,
	}
}
