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

namespace ISI.Extensions.Emails
{
	public enum EmailSenderSentStatus
	{
		[ISI.Extensions.EnumGuid("486ef4c6-6f3c-4e9b-833e-077e313e82ff")] Sent,
		[ISI.Extensions.EnumGuid("e36a0f98-cc1b-4df2-8e98-8182aba03d85")] Queued,
		[ISI.Extensions.EnumGuid("b31e5859-1281-475b-938d-db8a9c4c9792")] Scheduled,
		[ISI.Extensions.EnumGuid("66aaa47e-8313-4670-bf1e-b60c3c62defe")] Rejected,
		[ISI.Extensions.EnumGuid("6be21027-797e-4a74-b7ad-745295d6932b")] Invalid,
		[ISI.Extensions.EnumGuid("c0010e7d-9a78-4223-ba14-99c23791e92b")] GeneralError,
		[ISI.Extensions.EnumGuid("877281e0-80b5-4049-81e9-e765515784eb")] AuthenticationError,
		[ISI.Extensions.EnumGuid("9fa45ed5-cd63-45e9-86f1-d45f123cdafe")] AccountError,
		[ISI.Extensions.EnumGuid("5c910d8e-a60f-47bd-a9fb-106175473b40")] ValidationError,
	}
}
