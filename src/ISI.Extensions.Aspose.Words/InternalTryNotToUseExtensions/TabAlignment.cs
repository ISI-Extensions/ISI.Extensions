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
		public static ISI.Extensions.Documents.TabAlignment ToTabAlignment(this global::Aspose.Words.TabAlignment tabAlignment)
		{
			switch (tabAlignment)
			{
				case global::Aspose.Words.TabAlignment.Left: return ISI.Extensions.Documents.TabAlignment.Left;
				case global::Aspose.Words.TabAlignment.Center: return ISI.Extensions.Documents.TabAlignment.Center;
				case global::Aspose.Words.TabAlignment.Right: return ISI.Extensions.Documents.TabAlignment.Right;
				case global::Aspose.Words.TabAlignment.Decimal: return ISI.Extensions.Documents.TabAlignment.Decimal;
				case global::Aspose.Words.TabAlignment.Bar: return ISI.Extensions.Documents.TabAlignment.Bar;
				case global::Aspose.Words.TabAlignment.List: return ISI.Extensions.Documents.TabAlignment.List;
				case global::Aspose.Words.TabAlignment.Clear: return ISI.Extensions.Documents.TabAlignment.Clear;
				default:
					throw new ArgumentOutOfRangeException(nameof(tabAlignment), tabAlignment, null);
			}
		}

		public static global::Aspose.Words.TabAlignment ToTabAlignment(this ISI.Extensions.Documents.TabAlignment tabAlignment)
		{
			switch (tabAlignment)
			{
				case ISI.Extensions.Documents.TabAlignment.Left: return global::Aspose.Words.TabAlignment.Left;
				case ISI.Extensions.Documents.TabAlignment.Center: return global::Aspose.Words.TabAlignment.Center;
				case ISI.Extensions.Documents.TabAlignment.Right: return global::Aspose.Words.TabAlignment.Right;
				case ISI.Extensions.Documents.TabAlignment.Decimal: return global::Aspose.Words.TabAlignment.Decimal;
				case ISI.Extensions.Documents.TabAlignment.Bar: return global::Aspose.Words.TabAlignment.Bar;
				case ISI.Extensions.Documents.TabAlignment.List: return global::Aspose.Words.TabAlignment.List;
				case ISI.Extensions.Documents.TabAlignment.Clear: return global::Aspose.Words.TabAlignment.Clear;
				default:
					throw new ArgumentOutOfRangeException(nameof(tabAlignment), tabAlignment, null);
			}
		}
	}
}