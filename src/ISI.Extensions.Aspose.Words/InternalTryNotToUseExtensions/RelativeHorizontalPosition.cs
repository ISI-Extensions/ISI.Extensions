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
		public static ISI.Extensions.Documents.RelativeHorizontalPosition ToRelativeHorizontalPosition(this global::Aspose.Words.Drawing.RelativeHorizontalPosition relativeHorizontalPosition)
		{
			if (relativeHorizontalPosition == global::Aspose.Words.Drawing.RelativeHorizontalPosition.Default)
			{
				return ISI.Extensions.Documents.RelativeHorizontalPosition.Default;
			}
			if (relativeHorizontalPosition == global::Aspose.Words.Drawing.RelativeHorizontalPosition.Column)
			{
				return ISI.Extensions.Documents.RelativeHorizontalPosition.Column;
			}

			switch (relativeHorizontalPosition)
			{
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.Margin: return ISI.Extensions.Documents.RelativeHorizontalPosition.Margin;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.Page: return ISI.Extensions.Documents.RelativeHorizontalPosition.Page;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.Character: return ISI.Extensions.Documents.RelativeHorizontalPosition.Character;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.RightMargin: return ISI.Extensions.Documents.RelativeHorizontalPosition.RightMargin;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.LeftMargin: return ISI.Extensions.Documents.RelativeHorizontalPosition.LeftMargin;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.InsideMargin: return ISI.Extensions.Documents.RelativeHorizontalPosition.InsideMargin;
				case global::Aspose.Words.Drawing.RelativeHorizontalPosition.OutsideMargin: return ISI.Extensions.Documents.RelativeHorizontalPosition.OutsideMargin;
				default:
					return ISI.Extensions.Documents.RelativeHorizontalPosition.Default;
			}
		}

		public static global::Aspose.Words.Drawing.RelativeHorizontalPosition ToRelativeHorizontalPosition(this ISI.Extensions.Documents.RelativeHorizontalPosition relativeHorizontalPosition)
		{
			switch (relativeHorizontalPosition)
			{
				case ISI.Extensions.Documents.RelativeHorizontalPosition.Default: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.Default;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.Margin: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.Margin;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.Page: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.Page;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.Column: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.Column;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.Character: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.Character;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.RightMargin: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.RightMargin;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.LeftMargin: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.LeftMargin;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.InsideMargin: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.InsideMargin;
				case ISI.Extensions.Documents.RelativeHorizontalPosition.OutsideMargin: return global::Aspose.Words.Drawing.RelativeHorizontalPosition.OutsideMargin;
				default:
					throw new ArgumentOutOfRangeException(nameof(relativeHorizontalPosition), relativeHorizontalPosition, null);
			}
		}
	}
}