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
		public static ISI.Extensions.Documents.BreakType ToBreakType(this global::Aspose.Words.BreakType breakType)
		{
			switch (breakType)
			{
				case global::Aspose.Words.BreakType.ParagraphBreak: return ISI.Extensions.Documents.BreakType.ParagraphBreak;
				case global::Aspose.Words.BreakType.PageBreak: return ISI.Extensions.Documents.BreakType.PageBreak;
				case global::Aspose.Words.BreakType.ColumnBreak: return ISI.Extensions.Documents.BreakType.ColumnBreak;
				case global::Aspose.Words.BreakType.SectionBreakContinuous: return ISI.Extensions.Documents.BreakType.SectionBreakContinuous;
				case global::Aspose.Words.BreakType.SectionBreakNewColumn: return ISI.Extensions.Documents.BreakType.SectionBreakNewColumn;
				case global::Aspose.Words.BreakType.SectionBreakNewPage: return ISI.Extensions.Documents.BreakType.SectionBreakNewPage;
				case global::Aspose.Words.BreakType.SectionBreakEvenPage: return ISI.Extensions.Documents.BreakType.SectionBreakEvenPage;
				case global::Aspose.Words.BreakType.SectionBreakOddPage: return ISI.Extensions.Documents.BreakType.SectionBreakOddPage;
				case global::Aspose.Words.BreakType.LineBreak: return ISI.Extensions.Documents.BreakType.LineBreak;
				default:
					throw new ArgumentOutOfRangeException(nameof(breakType), breakType, null);
			}
		}

		public static global::Aspose.Words.BreakType ToBreakType(this ISI.Extensions.Documents.BreakType breakType)
		{
			switch (breakType)
			{
				case ISI.Extensions.Documents.BreakType.ParagraphBreak: return global::Aspose.Words.BreakType.ParagraphBreak;
				case ISI.Extensions.Documents.BreakType.PageBreak: return global::Aspose.Words.BreakType.PageBreak;
				case ISI.Extensions.Documents.BreakType.ColumnBreak: return global::Aspose.Words.BreakType.ColumnBreak;
				case ISI.Extensions.Documents.BreakType.SectionBreakContinuous: return global::Aspose.Words.BreakType.SectionBreakContinuous;
				case ISI.Extensions.Documents.BreakType.SectionBreakNewColumn: return global::Aspose.Words.BreakType.SectionBreakNewColumn;
				case ISI.Extensions.Documents.BreakType.SectionBreakNewPage: return global::Aspose.Words.BreakType.SectionBreakNewPage;
				case ISI.Extensions.Documents.BreakType.SectionBreakEvenPage: return global::Aspose.Words.BreakType.SectionBreakEvenPage;
				case ISI.Extensions.Documents.BreakType.SectionBreakOddPage: return global::Aspose.Words.BreakType.SectionBreakOddPage;
				case ISI.Extensions.Documents.BreakType.LineBreak: return global::Aspose.Words.BreakType.LineBreak;
				default:
					throw new ArgumentOutOfRangeException(nameof(breakType), breakType, null);
			}
		}
	}
}