#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
		public static ISI.Extensions.Documents.RelativeVerticalPosition ToRelativeVerticalPosition(this global::Aspose.Words.Drawing.RelativeVerticalPosition relativeVerticalPosition)
		{
			if (relativeVerticalPosition == global::Aspose.Words.Drawing.RelativeVerticalPosition.Margin)
			{
				return ISI.Extensions.Documents.RelativeVerticalPosition.Margin;
			}
			if (relativeVerticalPosition == global::Aspose.Words.Drawing.RelativeVerticalPosition.TableDefault)
			{
				return ISI.Extensions.Documents.RelativeVerticalPosition.TableDefault;
			}

			if (relativeVerticalPosition == global::Aspose.Words.Drawing.RelativeVerticalPosition.Paragraph)
			{
				return ISI.Extensions.Documents.RelativeVerticalPosition.Paragraph;
			}
			if (relativeVerticalPosition == global::Aspose.Words.Drawing.RelativeVerticalPosition.TextFrameDefault)
			{
				return ISI.Extensions.Documents.RelativeVerticalPosition.TextFrameDefault;
			}

			switch (relativeVerticalPosition)
			{
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.Page: return ISI.Extensions.Documents.RelativeVerticalPosition.Page;
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.Line: return ISI.Extensions.Documents.RelativeVerticalPosition.Line;
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.TopMargin: return ISI.Extensions.Documents.RelativeVerticalPosition.TopMargin;
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.BottomMargin: return ISI.Extensions.Documents.RelativeVerticalPosition.BottomMargin;
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.InsideMargin: return ISI.Extensions.Documents.RelativeVerticalPosition.InsideMargin;
				case global::Aspose.Words.Drawing.RelativeVerticalPosition.OutsideMargin: return ISI.Extensions.Documents.RelativeVerticalPosition.OutsideMargin;
				default:
					throw new ArgumentOutOfRangeException(nameof(relativeVerticalPosition), relativeVerticalPosition, null);
			}
		}

		public static global::Aspose.Words.Drawing.RelativeVerticalPosition ToRelativeVerticalPosition(this ISI.Extensions.Documents.RelativeVerticalPosition relativeVerticalPosition)
		{
			switch (relativeVerticalPosition)
			{
				case ISI.Extensions.Documents.RelativeVerticalPosition.Margin: return global::Aspose.Words.Drawing.RelativeVerticalPosition.Margin;
				case ISI.Extensions.Documents.RelativeVerticalPosition.TableDefault: return global::Aspose.Words.Drawing.RelativeVerticalPosition.TableDefault;
				case ISI.Extensions.Documents.RelativeVerticalPosition.Page: return global::Aspose.Words.Drawing.RelativeVerticalPosition.Page;
				case ISI.Extensions.Documents.RelativeVerticalPosition.Paragraph: return global::Aspose.Words.Drawing.RelativeVerticalPosition.Paragraph;
				case ISI.Extensions.Documents.RelativeVerticalPosition.TextFrameDefault: return global::Aspose.Words.Drawing.RelativeVerticalPosition.TextFrameDefault;
				case ISI.Extensions.Documents.RelativeVerticalPosition.Line: return global::Aspose.Words.Drawing.RelativeVerticalPosition.Line;
				case ISI.Extensions.Documents.RelativeVerticalPosition.TopMargin: return global::Aspose.Words.Drawing.RelativeVerticalPosition.TopMargin;
				case ISI.Extensions.Documents.RelativeVerticalPosition.BottomMargin: return global::Aspose.Words.Drawing.RelativeVerticalPosition.BottomMargin;
				case ISI.Extensions.Documents.RelativeVerticalPosition.InsideMargin: return global::Aspose.Words.Drawing.RelativeVerticalPosition.InsideMargin;
				case ISI.Extensions.Documents.RelativeVerticalPosition.OutsideMargin: return global::Aspose.Words.Drawing.RelativeVerticalPosition.OutsideMargin;
				default:
					throw new ArgumentOutOfRangeException(nameof(relativeVerticalPosition), relativeVerticalPosition, null);
			}
		}
	}
}