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
		public static global::Aspose.Words.BorderType ToBorderType(this ISI.Extensions.Documents.BorderType borderType)
		{
			switch (borderType)
			{
				case ISI.Extensions.Documents.BorderType.None: return global::Aspose.Words.BorderType.None;
				case ISI.Extensions.Documents.BorderType.Bottom: return global::Aspose.Words.BorderType.Bottom;
				case ISI.Extensions.Documents.BorderType.Left: return global::Aspose.Words.BorderType.Left;
				case ISI.Extensions.Documents.BorderType.Right: return global::Aspose.Words.BorderType.Right;
				case ISI.Extensions.Documents.BorderType.Top: return global::Aspose.Words.BorderType.Top;
				case ISI.Extensions.Documents.BorderType.Horizontal: return global::Aspose.Words.BorderType.Horizontal;
				case ISI.Extensions.Documents.BorderType.Vertical: return global::Aspose.Words.BorderType.Vertical;
				case ISI.Extensions.Documents.BorderType.DiagonalDown: return global::Aspose.Words.BorderType.DiagonalDown;
				case ISI.Extensions.Documents.BorderType.DiagonalUp: return global::Aspose.Words.BorderType.DiagonalUp;
				default:
					throw new ArgumentOutOfRangeException(nameof(borderType), borderType, null);
			}
		}

		public static ISI.Extensions.Documents.BorderType ToBorderType(this global::Aspose.Words.BorderType borderType)
		{
			switch (borderType)
			{
				case global::Aspose.Words.BorderType.None: return ISI.Extensions.Documents.BorderType.None;
				case global::Aspose.Words.BorderType.Bottom: return ISI.Extensions.Documents.BorderType.Bottom;
				case global::Aspose.Words.BorderType.Left: return ISI.Extensions.Documents.BorderType.Left;
				case global::Aspose.Words.BorderType.Right: return ISI.Extensions.Documents.BorderType.Right;
				case global::Aspose.Words.BorderType.Top: return ISI.Extensions.Documents.BorderType.Top;
				case global::Aspose.Words.BorderType.Horizontal: return ISI.Extensions.Documents.BorderType.Horizontal;
				case global::Aspose.Words.BorderType.Vertical: return ISI.Extensions.Documents.BorderType.Vertical;
				case global::Aspose.Words.BorderType.DiagonalDown: return ISI.Extensions.Documents.BorderType.DiagonalDown;
				case global::Aspose.Words.BorderType.DiagonalUp: return ISI.Extensions.Documents.BorderType.DiagonalUp;
				default:
					throw new ArgumentOutOfRangeException(nameof(borderType), borderType, null);
			}
		}
	}
}