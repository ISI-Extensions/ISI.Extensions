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
		public static ISI.Extensions.Documents.OutlineLevel ToOutlineLevel(this global::Aspose.Words.OutlineLevel outlineLevel)
		{
			switch (outlineLevel)
			{
				case global::Aspose.Words.OutlineLevel.Level1: return ISI.Extensions.Documents.OutlineLevel.Level1;
				case global::Aspose.Words.OutlineLevel.Level2: return ISI.Extensions.Documents.OutlineLevel.Level2;
				case global::Aspose.Words.OutlineLevel.Level3: return ISI.Extensions.Documents.OutlineLevel.Level3;
				case global::Aspose.Words.OutlineLevel.Level4: return ISI.Extensions.Documents.OutlineLevel.Level4;
				case global::Aspose.Words.OutlineLevel.Level5: return ISI.Extensions.Documents.OutlineLevel.Level5;
				case global::Aspose.Words.OutlineLevel.Level6: return ISI.Extensions.Documents.OutlineLevel.Level6;
				case global::Aspose.Words.OutlineLevel.Level7: return ISI.Extensions.Documents.OutlineLevel.Level7;
				case global::Aspose.Words.OutlineLevel.Level8: return ISI.Extensions.Documents.OutlineLevel.Level8;
				case global::Aspose.Words.OutlineLevel.Level9: return ISI.Extensions.Documents.OutlineLevel.Level9;
				case global::Aspose.Words.OutlineLevel.BodyText: return ISI.Extensions.Documents.OutlineLevel.BodyText;
				default:
					throw new ArgumentOutOfRangeException(nameof(outlineLevel), outlineLevel, null);
			}
		}

		public static global::Aspose.Words.OutlineLevel ToOutlineLevel(this ISI.Extensions.Documents.OutlineLevel outlineLevel)
		{
			switch (outlineLevel)
			{
				case ISI.Extensions.Documents.OutlineLevel.Level1: return global::Aspose.Words.OutlineLevel.Level1;
				case ISI.Extensions.Documents.OutlineLevel.Level2: return global::Aspose.Words.OutlineLevel.Level2;
				case ISI.Extensions.Documents.OutlineLevel.Level3: return global::Aspose.Words.OutlineLevel.Level3;
				case ISI.Extensions.Documents.OutlineLevel.Level4: return global::Aspose.Words.OutlineLevel.Level4;
				case ISI.Extensions.Documents.OutlineLevel.Level5: return global::Aspose.Words.OutlineLevel.Level5;
				case ISI.Extensions.Documents.OutlineLevel.Level6: return global::Aspose.Words.OutlineLevel.Level6;
				case ISI.Extensions.Documents.OutlineLevel.Level7: return global::Aspose.Words.OutlineLevel.Level7;
				case ISI.Extensions.Documents.OutlineLevel.Level8: return global::Aspose.Words.OutlineLevel.Level8;
				case ISI.Extensions.Documents.OutlineLevel.Level9: return global::Aspose.Words.OutlineLevel.Level9;
				case ISI.Extensions.Documents.OutlineLevel.BodyText: return global::Aspose.Words.OutlineLevel.BodyText;
				default:
					throw new ArgumentOutOfRangeException(nameof(outlineLevel), outlineLevel, null);
			}
		}
	}
}