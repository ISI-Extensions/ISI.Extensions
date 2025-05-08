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

namespace ISI.Extensions.Images
{
	public enum ImageFormat
	{
		[ISI.Extensions.EnumGuid("6f37ab79-1f70-433c-a65c-b8d53ed63175", "Bitmap", "bmp")] Bmp,
		[ISI.Extensions.EnumGuid("fe0710e2-9600-4f62-9850-1e598f220de8", "Jpeg", "jpg")] Jpeg,
		[ISI.Extensions.EnumGuid("461129b8-d2cf-4bec-9d0b-30540be99fec", "Png", "png")] Png,
		[ISI.Extensions.EnumGuid("fa96b179-b5ec-4b19-9058-b9acb52fcec3", "Gif", "gif")] Gif,
		[ISI.Extensions.EnumGuid("e322f909-c206-4c4c-a2bb-6275e407c1f5", "Tiff", "tif")] Tiff,
		[ISI.Extensions.EnumGuid("26db2f4e-556c-43ca-aac7-924a6e67c2f7", "Pdf", "pdf")] Pdf,
		[ISI.Extensions.EnumGuid("f79abb4a-95a9-414c-948a-7e589a58b974", "Hiec", "hiec")] Hiec,
		[ISI.Extensions.EnumGuid("f84a6207-ad02-4eee-a2e9-5cd5ffb95c6a", "Hief", "heif")] Hief,
	}
}
