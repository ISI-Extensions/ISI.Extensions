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
		public static ISI.Extensions.Documents.Underline ToUnderline(this global::Aspose.Words.Underline underline)
		{
			switch (underline)
			{
				case global::Aspose.Words.Underline.None: return ISI.Extensions.Documents.Underline.None;
				case global::Aspose.Words.Underline.Single: return ISI.Extensions.Documents.Underline.Single;
				case global::Aspose.Words.Underline.Words: return ISI.Extensions.Documents.Underline.Words;
				case global::Aspose.Words.Underline.Double: return ISI.Extensions.Documents.Underline.Double;
				case global::Aspose.Words.Underline.Dotted: return ISI.Extensions.Documents.Underline.Dotted;
				case global::Aspose.Words.Underline.Thick: return ISI.Extensions.Documents.Underline.Thick;
				case global::Aspose.Words.Underline.Dash: return ISI.Extensions.Documents.Underline.Dash;
				case global::Aspose.Words.Underline.DotDash: return ISI.Extensions.Documents.Underline.DotDash;
				case global::Aspose.Words.Underline.DotDotDash: return ISI.Extensions.Documents.Underline.DotDotDash;
				case global::Aspose.Words.Underline.Wavy: return ISI.Extensions.Documents.Underline.Wavy;
				case global::Aspose.Words.Underline.DottedHeavy: return ISI.Extensions.Documents.Underline.DottedHeavy;
				case global::Aspose.Words.Underline.DashHeavy: return ISI.Extensions.Documents.Underline.DashHeavy;
				case global::Aspose.Words.Underline.DotDashHeavy: return ISI.Extensions.Documents.Underline.DotDashHeavy;
				case global::Aspose.Words.Underline.DotDotDashHeavy: return ISI.Extensions.Documents.Underline.DotDotDashHeavy;
				case global::Aspose.Words.Underline.WavyHeavy: return ISI.Extensions.Documents.Underline.WavyHeavy;
				case global::Aspose.Words.Underline.DashLong: return ISI.Extensions.Documents.Underline.DashLong;
				case global::Aspose.Words.Underline.WavyDouble: return ISI.Extensions.Documents.Underline.WavyDouble;
				case global::Aspose.Words.Underline.DashLongHeavy: return ISI.Extensions.Documents.Underline.DashLongHeavy;
				default:
					throw new ArgumentOutOfRangeException(nameof(underline), underline, null);
			}
		}

		public static global::Aspose.Words.Underline ToUnderline(this ISI.Extensions.Documents.Underline underline)
		{
			switch (underline)
			{
				case ISI.Extensions.Documents.Underline.None: return global::Aspose.Words.Underline.None;
				case ISI.Extensions.Documents.Underline.Single: return global::Aspose.Words.Underline.Single;
				case ISI.Extensions.Documents.Underline.Words: return global::Aspose.Words.Underline.Words;
				case ISI.Extensions.Documents.Underline.Double: return global::Aspose.Words.Underline.Double;
				case ISI.Extensions.Documents.Underline.Dotted: return global::Aspose.Words.Underline.Dotted;
				case ISI.Extensions.Documents.Underline.Thick: return global::Aspose.Words.Underline.Thick;
				case ISI.Extensions.Documents.Underline.Dash: return global::Aspose.Words.Underline.Dash;
				case ISI.Extensions.Documents.Underline.DotDash: return global::Aspose.Words.Underline.DotDash;
				case ISI.Extensions.Documents.Underline.DotDotDash: return global::Aspose.Words.Underline.DotDotDash;
				case ISI.Extensions.Documents.Underline.Wavy: return global::Aspose.Words.Underline.Wavy;
				case ISI.Extensions.Documents.Underline.DottedHeavy: return global::Aspose.Words.Underline.DottedHeavy;
				case ISI.Extensions.Documents.Underline.DashHeavy: return global::Aspose.Words.Underline.DashHeavy;
				case ISI.Extensions.Documents.Underline.DotDashHeavy: return global::Aspose.Words.Underline.DotDashHeavy;
				case ISI.Extensions.Documents.Underline.DotDotDashHeavy: return global::Aspose.Words.Underline.DotDotDashHeavy;
				case ISI.Extensions.Documents.Underline.WavyHeavy: return global::Aspose.Words.Underline.WavyHeavy;
				case ISI.Extensions.Documents.Underline.DashLong: return global::Aspose.Words.Underline.DashLong;
				case ISI.Extensions.Documents.Underline.WavyDouble: return global::Aspose.Words.Underline.WavyDouble;
				case ISI.Extensions.Documents.Underline.DashLongHeavy: return global::Aspose.Words.Underline.DashLongHeavy;
				default:
					throw new ArgumentOutOfRangeException(nameof(underline), underline, null);
			}
		}
	}
}