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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.ImageType ToImageType(this global::Aspose.Words.Drawing.ImageType imageType)
		{
			switch (imageType)
			{
				case global::Aspose.Words.Drawing.ImageType.NoImage: return ISI.Extensions.Documents.ImageType.NoImage;
				case global::Aspose.Words.Drawing.ImageType.Unknown: return ISI.Extensions.Documents.ImageType.Unknown;
				case global::Aspose.Words.Drawing.ImageType.Emf: return ISI.Extensions.Documents.ImageType.Emf;
				case global::Aspose.Words.Drawing.ImageType.Wmf: return ISI.Extensions.Documents.ImageType.Wmf;
				case global::Aspose.Words.Drawing.ImageType.Pict: return ISI.Extensions.Documents.ImageType.Pict;
				case global::Aspose.Words.Drawing.ImageType.Jpeg: return ISI.Extensions.Documents.ImageType.Jpeg;
				case global::Aspose.Words.Drawing.ImageType.Png: return ISI.Extensions.Documents.ImageType.Png;
				case global::Aspose.Words.Drawing.ImageType.Bmp: return ISI.Extensions.Documents.ImageType.Bmp;
				default:
					throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null);
			}
		}

		public static global::Aspose.Words.Drawing.ImageType ToImageType(this ISI.Extensions.Documents.ImageType imageType)
		{
			switch (imageType)
			{
				case ISI.Extensions.Documents.ImageType.NoImage: return global::Aspose.Words.Drawing.ImageType.NoImage;
				case ISI.Extensions.Documents.ImageType.Unknown: return global::Aspose.Words.Drawing.ImageType.Unknown;
				case ISI.Extensions.Documents.ImageType.Emf: return global::Aspose.Words.Drawing.ImageType.Emf;
				case ISI.Extensions.Documents.ImageType.Wmf: return global::Aspose.Words.Drawing.ImageType.Wmf;
				case ISI.Extensions.Documents.ImageType.Pict: return global::Aspose.Words.Drawing.ImageType.Pict;
				case ISI.Extensions.Documents.ImageType.Jpeg: return global::Aspose.Words.Drawing.ImageType.Jpeg;
				case ISI.Extensions.Documents.ImageType.Png: return global::Aspose.Words.Drawing.ImageType.Png;
				case ISI.Extensions.Documents.ImageType.Bmp: return global::Aspose.Words.Drawing.ImageType.Bmp;
				default:
					throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null);
			}
		}
	}
}