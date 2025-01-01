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
using System.Threading.Tasks;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentStyle : ISI.Extensions.Documents.IDocumentStyle
		{
			internal global::Aspose.Words.Style _style = null;

			public DocumentStyle(global::Aspose.Words.Style style)
			{
				_style = style;
			}
			
			public void Remove()
			{
				_style.Remove();
			}

			public string Name { get => _style.Name; set => _style.Name = value; }
			public ISI.Extensions.Documents.StyleIdentifier StyleIdentifier => _style.StyleIdentifier.ToStyleIdentifier();
			public string[] Aliases => _style.Aliases;
			public bool IsHeading => _style.IsHeading;
			public ISI.Extensions.Documents.StyleType Type => _style.Type.ToStyleType();
			public string LinkedStyleName => _style.LinkedStyleName;
			public string BaseStyleName { get => _style.BaseStyleName; set => _style.BaseStyleName = value; }
			public string NextParagraphStyleName { get => _style.NextParagraphStyleName; set => _style.NextParagraphStyleName = value; }
			public bool BuiltIn => _style.BuiltIn;
			public ISI.Extensions.Documents.IDocumentFont Font => new DocumentFont(_style.Font);
			public ISI.Extensions.Documents.IDocumentParagraphFormat ParagraphFormat => new DocumentParagraphFormat(_style.ParagraphFormat);
			public ISI.Extensions.Documents.IDocumentStyleCollection Styles => new DocumentStyleCollection(_style.Styles);
		}
	}
}