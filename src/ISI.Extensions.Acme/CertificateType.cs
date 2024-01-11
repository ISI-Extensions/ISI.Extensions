﻿#region Copyright & License
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
using System.Text;

namespace ISI.Extensions.Acme
{
	public enum CertificateType
	{
		[ISI.Extensions.EnumGuid("495aedce-1aac-4fcd-8a38-f813380f17eb")] Csr,
		[ISI.Extensions.EnumGuid("8a40c916-f5d8-410e-be41-2123a3e386e7")] Key,
		[ISI.Extensions.EnumGuid("0f526b75-e068-4947-83ed-7fa3f6a16069")] KeyPassword,
		[ISI.Extensions.EnumGuid("ec4c381a-55f7-48dc-895f-4cd0d402336e")] Crt,
		[ISI.Extensions.EnumGuid("a03c5263-76d1-4b43-bd5b-676faaed27bc")] Pem,
		[ISI.Extensions.EnumGuid("615dd5cd-87da-4b3e-8f4a-2b550015d6cb")] Pfx,
		[ISI.Extensions.EnumGuid("e712a8cd-a7b4-4028-b9b0-cd72a54a2b54")] Jks,
	}
}