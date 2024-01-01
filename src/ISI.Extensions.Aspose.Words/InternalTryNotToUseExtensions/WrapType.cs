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
		public static ISI.Extensions.Documents.WrapType ToWrapType(this global::Aspose.Words.Drawing.WrapType wrapType)
		{
			switch (wrapType)
			{
				case global::Aspose.Words.Drawing.WrapType.None: return ISI.Extensions.Documents.WrapType.None;
				case global::Aspose.Words.Drawing.WrapType.Inline: return ISI.Extensions.Documents.WrapType.Inline;
				case global::Aspose.Words.Drawing.WrapType.TopBottom: return ISI.Extensions.Documents.WrapType.TopBottom;
				case global::Aspose.Words.Drawing.WrapType.Square: return ISI.Extensions.Documents.WrapType.Square;
				case global::Aspose.Words.Drawing.WrapType.Tight: return ISI.Extensions.Documents.WrapType.Tight;
				case global::Aspose.Words.Drawing.WrapType.Through: return ISI.Extensions.Documents.WrapType.Through;
				default:
					throw new ArgumentOutOfRangeException(nameof(wrapType), wrapType, null);
			}
		}

		public static global::Aspose.Words.Drawing.WrapType ToWrapType(this ISI.Extensions.Documents.WrapType wrapType)
		{
			switch (wrapType)
			{
				case ISI.Extensions.Documents.WrapType.None: return global::Aspose.Words.Drawing.WrapType.None;
				case ISI.Extensions.Documents.WrapType.Inline: return global::Aspose.Words.Drawing.WrapType.Inline;
				case ISI.Extensions.Documents.WrapType.TopBottom: return global::Aspose.Words.Drawing.WrapType.TopBottom;
				case ISI.Extensions.Documents.WrapType.Square: return global::Aspose.Words.Drawing.WrapType.Square;
				case ISI.Extensions.Documents.WrapType.Tight: return global::Aspose.Words.Drawing.WrapType.Tight;
				case ISI.Extensions.Documents.WrapType.Through: return global::Aspose.Words.Drawing.WrapType.Through;
				default:
					throw new ArgumentOutOfRangeException(nameof(wrapType), wrapType, null);
			}
		}
	}
}