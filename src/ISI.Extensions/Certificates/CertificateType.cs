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

namespace ISI.Extensions.Certificates
{
	public enum CertificateType
	{
		[ISI.Extensions.EnumGuid("495aedce-1aac-4fcd-8a38-f813380f17eb", "Csr", "csr")] Csr,
		[ISI.Extensions.EnumGuid("8a40c916-f5d8-410e-be41-2123a3e386e7", "Key", "key")] Key,
		[ISI.Extensions.EnumGuid("0f526b75-e068-4947-83ed-7fa3f6a16069", "Key Password", "key-password")] KeyPassword,
		[ISI.Extensions.EnumGuid("ec4c381a-55f7-48dc-895f-4cd0d402336e", "Crt", "crt")] Crt,
		[ISI.Extensions.EnumGuid("f8d7ab2e-a3ba-42db-bf16-95b088060154", "Ca Bundle Crt", "ca-bundle-crt")] CaBundleCrt,
		[ISI.Extensions.EnumGuid("a03c5263-76d1-4b43-bd5b-676faaed27bc", "Bundle Crt", "pem")] BundleCrt,
		[ISI.Extensions.EnumGuid("615dd5cd-87da-4b3e-8f4a-2b550015d6cb", "Pfx", "pfx")] Pfx,
		[ISI.Extensions.EnumGuid("a0665289-4ff8-44ea-8ed6-d1a85af31262", "Pfx Password", "pfx-password")] PfxPassword,
		[ISI.Extensions.EnumGuid("e712a8cd-a7b4-4028-b9b0-cd72a54a2b54", "Jks Keystore", "jks")] JksKeystore,
		[ISI.Extensions.EnumGuid("9a041f7b-3c2a-4a66-b74c-5a301c8ef3cc", "Jks Keystore Password", "jks-password")] JksKeystorePassword,
	}
}
