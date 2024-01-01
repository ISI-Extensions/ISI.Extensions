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
		public static ISI.Extensions.Documents.TextOrientation ToTextOrientation(this global::Aspose.Words.TextOrientation textOrientation)
		{
			switch (textOrientation)
			{
				case global::Aspose.Words.TextOrientation.Horizontal: return ISI.Extensions.Documents.TextOrientation.Horizontal;
				case global::Aspose.Words.TextOrientation.Downward: return ISI.Extensions.Documents.TextOrientation.Downward;
				case global::Aspose.Words.TextOrientation.Upward: return ISI.Extensions.Documents.TextOrientation.Upward;
				case global::Aspose.Words.TextOrientation.HorizontalRotatedFarEast: return ISI.Extensions.Documents.TextOrientation.HorizontalRotatedFarEast;
				case global::Aspose.Words.TextOrientation.VerticalFarEast: return ISI.Extensions.Documents.TextOrientation.VerticalFarEast;
				case global::Aspose.Words.TextOrientation.VerticalRotatedFarEast: return ISI.Extensions.Documents.TextOrientation.VerticalRotatedFarEast;
				default:
					throw new ArgumentOutOfRangeException(nameof(textOrientation), textOrientation, null);
			}
		}

		public static global::Aspose.Words.TextOrientation ToTextOrientation(this ISI.Extensions.Documents.TextOrientation textOrientation)
		{
			switch (textOrientation)
			{
				case ISI.Extensions.Documents.TextOrientation.Horizontal: return global::Aspose.Words.TextOrientation.Horizontal;
				case ISI.Extensions.Documents.TextOrientation.Downward: return global::Aspose.Words.TextOrientation.Downward;
				case ISI.Extensions.Documents.TextOrientation.Upward: return global::Aspose.Words.TextOrientation.Upward;
				case ISI.Extensions.Documents.TextOrientation.HorizontalRotatedFarEast: return global::Aspose.Words.TextOrientation.HorizontalRotatedFarEast;
				case ISI.Extensions.Documents.TextOrientation.VerticalFarEast: return global::Aspose.Words.TextOrientation.VerticalFarEast;
				case ISI.Extensions.Documents.TextOrientation.VerticalRotatedFarEast: return global::Aspose.Words.TextOrientation.VerticalRotatedFarEast;
				default:
					throw new ArgumentOutOfRangeException(nameof(textOrientation), textOrientation, null);
			}
		}
	}
}