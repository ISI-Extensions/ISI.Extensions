#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public static ISI.Extensions.Documents.LineSpacingRule ToLineSpacingRule(this global::Aspose.Words.LineSpacingRule lineSpacingRule)
		{
			switch (lineSpacingRule)
			{
				case global::Aspose.Words.LineSpacingRule.AtLeast: return ISI.Extensions.Documents.LineSpacingRule.AtLeast;
				case global::Aspose.Words.LineSpacingRule.Exactly: return ISI.Extensions.Documents.LineSpacingRule.Exactly;
				case global::Aspose.Words.LineSpacingRule.Multiple: return ISI.Extensions.Documents.LineSpacingRule.Multiple;
				default:
					throw new ArgumentOutOfRangeException(nameof(lineSpacingRule), lineSpacingRule, null);
			}
		}

		public static global::Aspose.Words.LineSpacingRule ToLineSpacingRule(this ISI.Extensions.Documents.LineSpacingRule lineSpacingRule)
		{
			switch (lineSpacingRule)
			{
				case ISI.Extensions.Documents.LineSpacingRule.AtLeast: return global::Aspose.Words.LineSpacingRule.AtLeast;
				case ISI.Extensions.Documents.LineSpacingRule.Exactly: return global::Aspose.Words.LineSpacingRule.Exactly;
				case ISI.Extensions.Documents.LineSpacingRule.Multiple: return global::Aspose.Words.LineSpacingRule.Multiple;
				default:
					throw new ArgumentOutOfRangeException(nameof(lineSpacingRule), lineSpacingRule, null);
			}
		}
	}
}