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
using System.Text;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class SlidesExtensions
	{
		/*
		 Pps = 0,
		Ppt = 0,
		Pdf = 1,
		Xps = 2,
		Pptx = 3,
		Ppsx = 4,
		Tiff = 5,
		Odp = 6,
		Pptm = 7,
		Ppsm = 9,
		Potx = 10,
		Potm = 11,
		PdfNotes = 12,
		Html = 13,
		TiffNotes = 14,
		Swf = 15,
		SwfNotes = 16,
		Otp = 17,
		*/
		public static global::Aspose.Slides.Export.SaveFormat ToSaveFormat(this ISI.Extensions.Presentations.FileFormat fileFormat)
		{
			var result = global::Aspose.Slides.Export.SaveFormat.Pptx;

			switch (fileFormat)
			{
				case ISI.Extensions.Presentations.FileFormat.Default:
					result = global::Aspose.Slides.Export.SaveFormat.Pptx;
					break;
				case ISI.Extensions.Presentations.FileFormat.Pptx:
					result = global::Aspose.Slides.Export.SaveFormat.Pptx;
					break;
				case ISI.Extensions.Presentations.FileFormat.Ppt:
					result = global::Aspose.Slides.Export.SaveFormat.Ppt;
					break;
				case ISI.Extensions.Presentations.FileFormat.Ppsx:
					result = global::Aspose.Slides.Export.SaveFormat.Ppsx;
					break;
				case ISI.Extensions.Presentations.FileFormat.Pps:
					result = global::Aspose.Slides.Export.SaveFormat.Pps;
					break;
				case ISI.Extensions.Presentations.FileFormat.Pdf:
					result = global::Aspose.Slides.Export.SaveFormat.Pdf;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}

			return result;
		}

		public static global::Aspose.Slides.Export.SaveFormat ToSaveFormat(this ISI.Extensions.Documents.FileFormat fileFormat)
		{
			var result = global::Aspose.Slides.Export.SaveFormat.Pptx;

			switch (fileFormat)
			{
				case ISI.Extensions.Documents.FileFormat.Default:
					result = global::Aspose.Slides.Export.SaveFormat.Pptx;
					break;
				case ISI.Extensions.Documents.FileFormat.Pptx:
					result = global::Aspose.Slides.Export.SaveFormat.Pptx;
					break;
				case ISI.Extensions.Documents.FileFormat.Ppt:
					result = global::Aspose.Slides.Export.SaveFormat.Ppt;
					break;
				case ISI.Extensions.Documents.FileFormat.Ppsx:
					result = global::Aspose.Slides.Export.SaveFormat.Ppsx;
					break;
				case ISI.Extensions.Documents.FileFormat.Pps:
					result = global::Aspose.Slides.Export.SaveFormat.Pps;
					break;
				case ISI.Extensions.Documents.FileFormat.Pdf:
					result = global::Aspose.Slides.Export.SaveFormat.Pdf;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}

			return result;
		}
	}
}