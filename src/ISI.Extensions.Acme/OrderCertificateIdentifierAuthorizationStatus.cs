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
using System.Text;

namespace ISI.Extensions.Acme
{
	public enum OrderCertificateIdentifierAuthorizationStatus
	{
		[ISI.Extensions.EnumGuid("d109cf68-3862-42ec-a75c-953a4eb43a17", "Pending", "pending")] Pending,
		[ISI.Extensions.EnumGuid("a92f4443-df35-4177-8722-ce96ee2cc938", "Valid", "valid")] Valid,
		[ISI.Extensions.EnumGuid("d5ac03df-fe5b-4dc7-b12b-b3fc91fa35da", "Invalid", "invalid")] Invalid,
		[ISI.Extensions.EnumGuid("0cf6bc66-a35f-49f7-8fb7-002f9b131ae0", "Deactivated", "deactivated")] Deactivated,
		[ISI.Extensions.EnumGuid("944bce9a-eea4-44c3-b64d-d628df001af2", "Expired", "expired")] Expired,
		[ISI.Extensions.EnumGuid("ffc05286-29cf-4972-aa90-45844eb69c07", "Revoked", "revoked")] Revoked,
	}
}
