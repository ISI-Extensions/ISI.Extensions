#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
		public static ISI.Extensions.Documents.FileFormat ToSaveFormat(this global::Aspose.Words.SaveFormat fileFormat)
		{
			switch (fileFormat)
			{
				case global::Aspose.Words.SaveFormat.Doc: return ISI.Extensions.Documents.FileFormat.Doc;
				case global::Aspose.Words.SaveFormat.Dot: return ISI.Extensions.Documents.FileFormat.Dot;
				case global::Aspose.Words.SaveFormat.Docx: return ISI.Extensions.Documents.FileFormat.Docx;
				case global::Aspose.Words.SaveFormat.Docm: return ISI.Extensions.Documents.FileFormat.Docm;
				case global::Aspose.Words.SaveFormat.Dotx: return ISI.Extensions.Documents.FileFormat.Dotx;
				case global::Aspose.Words.SaveFormat.Dotm: return ISI.Extensions.Documents.FileFormat.Dotm;
				case global::Aspose.Words.SaveFormat.Rtf: return ISI.Extensions.Documents.FileFormat.Rtf;
				case global::Aspose.Words.SaveFormat.Pdf: return ISI.Extensions.Documents.FileFormat.Pdf;
				case global::Aspose.Words.SaveFormat.Xps: return ISI.Extensions.Documents.FileFormat.Xps;
				case global::Aspose.Words.SaveFormat.Html: return ISI.Extensions.Documents.FileFormat.Html;
				case global::Aspose.Words.SaveFormat.Tiff: return ISI.Extensions.Documents.FileFormat.Tiff;
				case global::Aspose.Words.SaveFormat.Png: return ISI.Extensions.Documents.FileFormat.Png;
				case global::Aspose.Words.SaveFormat.Bmp: return ISI.Extensions.Documents.FileFormat.Bmp;
				case global::Aspose.Words.SaveFormat.Jpeg: return ISI.Extensions.Documents.FileFormat.Jpeg;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}
		}

		public static global::Aspose.Words.SaveFormat ToSaveFormat(this ISI.Extensions.Documents.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.Documents.FileFormat.Default: return global::Aspose.Words.SaveFormat.Docx;
				case ISI.Extensions.Documents.FileFormat.Doc: return global::Aspose.Words.SaveFormat.Doc;
				case ISI.Extensions.Documents.FileFormat.Dot: return global::Aspose.Words.SaveFormat.Dot;
				case ISI.Extensions.Documents.FileFormat.Docx: return global::Aspose.Words.SaveFormat.Docx;
				case ISI.Extensions.Documents.FileFormat.Docm: return global::Aspose.Words.SaveFormat.Docm;
				case ISI.Extensions.Documents.FileFormat.Dotx: return global::Aspose.Words.SaveFormat.Dotx;
				case ISI.Extensions.Documents.FileFormat.Dotm: return global::Aspose.Words.SaveFormat.Dotm;
				case ISI.Extensions.Documents.FileFormat.Rtf: return global::Aspose.Words.SaveFormat.Rtf;
				case ISI.Extensions.Documents.FileFormat.Pdf: return global::Aspose.Words.SaveFormat.Pdf;
				case ISI.Extensions.Documents.FileFormat.Xps: return global::Aspose.Words.SaveFormat.Xps;
				case ISI.Extensions.Documents.FileFormat.Html: return global::Aspose.Words.SaveFormat.Html;
				case ISI.Extensions.Documents.FileFormat.Tiff: return global::Aspose.Words.SaveFormat.Tiff;
				case ISI.Extensions.Documents.FileFormat.Png: return global::Aspose.Words.SaveFormat.Png;
				case ISI.Extensions.Documents.FileFormat.Bmp: return global::Aspose.Words.SaveFormat.Bmp;
				case ISI.Extensions.Documents.FileFormat.Jpeg: return global::Aspose.Words.SaveFormat.Jpeg;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}
		}
	}
}