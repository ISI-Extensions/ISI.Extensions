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
		public static ISI.Extensions.Documents.SectionStart ToSectionStart(this global::Aspose.Words.SectionStart sectionStart)
		{
			switch (sectionStart)
			{
				case global::Aspose.Words.SectionStart.Continuous: return ISI.Extensions.Documents.SectionStart.Continuous;
				case global::Aspose.Words.SectionStart.NewColumn: return ISI.Extensions.Documents.SectionStart.NewColumn;
				case global::Aspose.Words.SectionStart.NewPage: return ISI.Extensions.Documents.SectionStart.NewPage;
				case global::Aspose.Words.SectionStart.EvenPage: return ISI.Extensions.Documents.SectionStart.EvenPage;
				case global::Aspose.Words.SectionStart.OddPage: return ISI.Extensions.Documents.SectionStart.OddPage;
				default:
					throw new ArgumentOutOfRangeException(nameof(sectionStart), sectionStart, null);
			}
		}

		public static global::Aspose.Words.SectionStart ToSectionStart(this ISI.Extensions.Documents.SectionStart sectionStart)
		{
			switch (sectionStart)
			{
				case ISI.Extensions.Documents.SectionStart.Continuous: return global::Aspose.Words.SectionStart.Continuous;
				case ISI.Extensions.Documents.SectionStart.NewColumn: return global::Aspose.Words.SectionStart.NewColumn;
				case ISI.Extensions.Documents.SectionStart.NewPage: return global::Aspose.Words.SectionStart.NewPage;
				case ISI.Extensions.Documents.SectionStart.EvenPage: return global::Aspose.Words.SectionStart.EvenPage;
				case ISI.Extensions.Documents.SectionStart.OddPage: return global::Aspose.Words.SectionStart.OddPage;
				default:
					throw new ArgumentOutOfRangeException(nameof(sectionStart), sectionStart, null);
			}
		}
	}
}