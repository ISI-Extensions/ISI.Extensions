#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
		public static ISI.Extensions.Documents.ParagraphAlignment ToParagraphAlignment(this global::Aspose.Words.ParagraphAlignment paragraphAlignment)
		{
			switch (paragraphAlignment)
			{
				case global::Aspose.Words.ParagraphAlignment.Left: return ISI.Extensions.Documents.ParagraphAlignment.Left;
				case global::Aspose.Words.ParagraphAlignment.Center: return ISI.Extensions.Documents.ParagraphAlignment.Center;
				case global::Aspose.Words.ParagraphAlignment.Right: return ISI.Extensions.Documents.ParagraphAlignment.Right;
				case global::Aspose.Words.ParagraphAlignment.Justify: return ISI.Extensions.Documents.ParagraphAlignment.Justify;
				case global::Aspose.Words.ParagraphAlignment.Distributed: return ISI.Extensions.Documents.ParagraphAlignment.Distributed;
				case global::Aspose.Words.ParagraphAlignment.ArabicMediumKashida: return ISI.Extensions.Documents.ParagraphAlignment.ArabicMediumKashida;
				case global::Aspose.Words.ParagraphAlignment.ArabicHighKashida: return ISI.Extensions.Documents.ParagraphAlignment.ArabicHighKashida;
				case global::Aspose.Words.ParagraphAlignment.ArabicLowKashida: return ISI.Extensions.Documents.ParagraphAlignment.ArabicLowKashida;
				case global::Aspose.Words.ParagraphAlignment.ThaiDistributed: return ISI.Extensions.Documents.ParagraphAlignment.ThaiDistributed;
				default:
					throw new ArgumentOutOfRangeException(nameof(paragraphAlignment), paragraphAlignment, null);
			}
		}

		public static global::Aspose.Words.ParagraphAlignment ToParagraphAlignment(this ISI.Extensions.Documents.ParagraphAlignment paragraphAlignment)
		{
			switch (paragraphAlignment)
			{
				case ISI.Extensions.Documents.ParagraphAlignment.Left: return global::Aspose.Words.ParagraphAlignment.Left;
				case ISI.Extensions.Documents.ParagraphAlignment.Center: return global::Aspose.Words.ParagraphAlignment.Center;
				case ISI.Extensions.Documents.ParagraphAlignment.Right: return global::Aspose.Words.ParagraphAlignment.Right;
				case ISI.Extensions.Documents.ParagraphAlignment.Justify: return global::Aspose.Words.ParagraphAlignment.Justify;
				case ISI.Extensions.Documents.ParagraphAlignment.Distributed: return global::Aspose.Words.ParagraphAlignment.Distributed;
				case ISI.Extensions.Documents.ParagraphAlignment.ArabicMediumKashida: return global::Aspose.Words.ParagraphAlignment.ArabicMediumKashida;
				case ISI.Extensions.Documents.ParagraphAlignment.ArabicHighKashida: return global::Aspose.Words.ParagraphAlignment.ArabicHighKashida;
				case ISI.Extensions.Documents.ParagraphAlignment.ArabicLowKashida: return global::Aspose.Words.ParagraphAlignment.ArabicLowKashida;
				case ISI.Extensions.Documents.ParagraphAlignment.ThaiDistributed: return global::Aspose.Words.ParagraphAlignment.ThaiDistributed;
				default:
					throw new ArgumentOutOfRangeException(nameof(paragraphAlignment), paragraphAlignment, null);
			}
		}
	}
}