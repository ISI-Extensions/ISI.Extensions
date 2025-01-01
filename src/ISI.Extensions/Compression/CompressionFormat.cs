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

namespace ISI.Extensions.Compression
{
	public enum CompressionFormat
	{
		[ISI.Extensions.EnumGuid("00caf8a1-12e0-4eb9-a941-5b18777fb797", "SevenZip", "7z")] SevenZip,
		[ISI.Extensions.EnumGuid("821341a0-4373-466c-82c7-3f5c8400841d", "Zip", "zip")] Zip,
		[ISI.Extensions.EnumGuid("ac871406-666b-4410-9f34-74cbe309ecd7", "GZip", "gzip")] GZip,
		[ISI.Extensions.EnumGuid("9fb6fa23-f5ba-4f39-abd5-d9044fa8f4c3", "Tar", "tar")] Tar,
		[ISI.Extensions.EnumGuid("04c18db1-10fb-4c4a-be88-ffec14459a72", "BZip2", "bz2")] BZip2,
		[ISI.Extensions.EnumGuid("4c739664-2aad-419a-8cb5-9b85eda1a983", "XZ", "xz")] XZ,
	}
}
