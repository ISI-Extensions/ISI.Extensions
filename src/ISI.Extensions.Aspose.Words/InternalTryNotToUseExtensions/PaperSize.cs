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
using System.Linq;
using System.Text;

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.PaperSize ToPaperSize(this global::Aspose.Words.PaperSize paperSize)
		{
			switch (paperSize)
			{
				case global::Aspose.Words.PaperSize.Custom: return ISI.Extensions.Documents.PaperSize.Custom;
				case global::Aspose.Words.PaperSize.A3: return ISI.Extensions.Documents.PaperSize.A3;
				case global::Aspose.Words.PaperSize.A4: return ISI.Extensions.Documents.PaperSize.A4;
				case global::Aspose.Words.PaperSize.A5: return ISI.Extensions.Documents.PaperSize.A5;
				case global::Aspose.Words.PaperSize.B4: return ISI.Extensions.Documents.PaperSize.B4;
				case global::Aspose.Words.PaperSize.B5: return ISI.Extensions.Documents.PaperSize.B5;
				case global::Aspose.Words.PaperSize.Executive: return ISI.Extensions.Documents.PaperSize.Executive;
				case global::Aspose.Words.PaperSize.Folio: return ISI.Extensions.Documents.PaperSize.Folio;
				case global::Aspose.Words.PaperSize.Ledger: return ISI.Extensions.Documents.PaperSize.Ledger;
				case global::Aspose.Words.PaperSize.Legal: return ISI.Extensions.Documents.PaperSize.Legal;
				case global::Aspose.Words.PaperSize.Letter: return ISI.Extensions.Documents.PaperSize.Letter;
				case global::Aspose.Words.PaperSize.EnvelopeDL: return ISI.Extensions.Documents.PaperSize.EnvelopeDL;
				case global::Aspose.Words.PaperSize.Quarto: return ISI.Extensions.Documents.PaperSize.Quarto;
				case global::Aspose.Words.PaperSize.Statement: return ISI.Extensions.Documents.PaperSize.Statement;
				case global::Aspose.Words.PaperSize.Tabloid: return ISI.Extensions.Documents.PaperSize.Tabloid;
				case global::Aspose.Words.PaperSize.Paper10x14: return ISI.Extensions.Documents.PaperSize.Paper10x14;
				case global::Aspose.Words.PaperSize.Paper11x17: return ISI.Extensions.Documents.PaperSize.Paper11x17;
				default:
					throw new ArgumentOutOfRangeException(nameof(paperSize), paperSize, null);
			}
		}

		public static global::Aspose.Words.PaperSize ToPaperSize(this ISI.Extensions.Documents.PaperSize paperSize)
		{
			switch (paperSize)
			{
				case ISI.Extensions.Documents.PaperSize.Custom: return global::Aspose.Words.PaperSize.Custom;
				case ISI.Extensions.Documents.PaperSize.A3: return global::Aspose.Words.PaperSize.A3;
				case ISI.Extensions.Documents.PaperSize.A4: return global::Aspose.Words.PaperSize.A4;
				case ISI.Extensions.Documents.PaperSize.A5: return global::Aspose.Words.PaperSize.A5;
				case ISI.Extensions.Documents.PaperSize.B4: return global::Aspose.Words.PaperSize.B4;
				case ISI.Extensions.Documents.PaperSize.B5: return global::Aspose.Words.PaperSize.B5;
				case ISI.Extensions.Documents.PaperSize.Executive: return global::Aspose.Words.PaperSize.Executive;
				case ISI.Extensions.Documents.PaperSize.Folio: return global::Aspose.Words.PaperSize.Folio;
				case ISI.Extensions.Documents.PaperSize.Ledger: return global::Aspose.Words.PaperSize.Ledger;
				case ISI.Extensions.Documents.PaperSize.Legal: return global::Aspose.Words.PaperSize.Legal;
				case ISI.Extensions.Documents.PaperSize.Letter: return global::Aspose.Words.PaperSize.Letter;
				case ISI.Extensions.Documents.PaperSize.EnvelopeDL: return global::Aspose.Words.PaperSize.EnvelopeDL;
				case ISI.Extensions.Documents.PaperSize.Quarto: return global::Aspose.Words.PaperSize.Quarto;
				case ISI.Extensions.Documents.PaperSize.Statement: return global::Aspose.Words.PaperSize.Statement;
				case ISI.Extensions.Documents.PaperSize.Tabloid: return global::Aspose.Words.PaperSize.Tabloid;
				case ISI.Extensions.Documents.PaperSize.Paper10x14: return global::Aspose.Words.PaperSize.Paper10x14;
				case ISI.Extensions.Documents.PaperSize.Paper11x17: return global::Aspose.Words.PaperSize.Paper11x17;
				default:
					throw new ArgumentOutOfRangeException(nameof(paperSize), paperSize, null);
			}
		}
	}
}