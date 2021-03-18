#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentFont : ISI.Extensions.Documents.IDocumentFont
		{
			internal global::Aspose.Words.Font _font = null;

			public DocumentFont(global::Aspose.Words.Font font)
			{
				_font = font;
			}

			public void ClearFormatting()
			{
				_font.ClearFormatting();
			}

			public string Name { get => _font.Name; set => _font.Name = value; }
			public string NameAscii { get => _font.NameAscii; set => _font.NameAscii = value; }
			public string NameBi { get => _font.NameBi; set => _font.NameBi = value; }
			public string NameFarEast { get => _font.NameFarEast; set => _font.NameFarEast = value; }
			public string NameOther { get => _font.NameOther; set => _font.NameOther = value; }
			public double Size { get => _font.Size; set => _font.Size = value; }
			public double SizeBi { get => _font.SizeBi; set => _font.SizeBi = value; }
			public bool Bold { get => _font.Bold; set => _font.Bold = value; }
			public bool BoldBi { get => _font.BoldBi; set => _font.BoldBi = value; }
			public bool Italic { get => _font.Italic; set => _font.Italic = value; }
			public bool ItalicBi { get => _font.ItalicBi; set => _font.ItalicBi = value; }
			public System.Drawing.Color Color { get => _font.Color; set => _font.Color = value; }
			public System.Drawing.Color AutoColor => _font.AutoColor; 
			public bool StrikeThrough { get => _font.StrikeThrough; set => _font.StrikeThrough = value; }
			public bool DoubleStrikeThrough { get => _font.DoubleStrikeThrough; set => _font.DoubleStrikeThrough = value; }
			public bool Shadow { get => _font.Shadow; set => _font.Shadow = value; }
			public bool Outline { get => _font.Outline; set => _font.Outline = value; }
			public bool Emboss { get => _font.Emboss; set => _font.Emboss = value; }
			public bool Engrave { get => _font.Engrave; set => _font.Engrave = value; }
			public bool Superscript { get => _font.Superscript; set => _font.Superscript = value; }
			public bool Subscript { get => _font.Subscript; set => _font.Subscript = value; }
			public bool SmallCaps { get => _font.SmallCaps; set => _font.SmallCaps = value; }
			public bool AllCaps { get => _font.AllCaps; set => _font.AllCaps = value; }
			public bool Hidden { get => _font.Hidden; set => _font.Hidden = value; }
			public ISI.Extensions.Documents.Underline Underline { get => _font.Underline.ToUnderline(); set => _font.Underline = value.ToUnderline(); }
			public System.Drawing.Color UnderlineColor { get => _font.UnderlineColor; set => _font.UnderlineColor = value; }
			public int Scaling { get => _font.Scaling;  set => _font.Scaling = value;}
			public double Spacing { get => _font.Spacing; set => _font.Spacing = value; }
			public double Position { get => _font.Position; set => _font.Position = value; }
			public double Kerning { get => _font.Kerning; set => _font.Kerning = value; }
			public System.Drawing.Color HighlightColor { get => _font.HighlightColor; set => _font.HighlightColor = value; }
			public ISI.Extensions.Documents.TextEffect TextEffect { get => _font.TextEffect.ToTextEffect(); set => _font.TextEffect = value.ToTextEffect(); }
			public bool Bidi { get => _font.Bidi; set => _font.Bidi = value; }
			public bool ComplexScript { get => _font.ComplexScript; set => _font.ComplexScript = value; }
			public bool NoProofing { get => _font.NoProofing; set => _font.NoProofing = value; }
			public int LocaleId { get => _font.LocaleId; set => _font.LocaleId = value; }
			public int LocaleIdBi { get => _font.LocaleIdBi; set => _font.LocaleIdBi = value; }
			public int LocaleIdFarEast { get => _font.LocaleIdFarEast; set => _font.LocaleIdFarEast = value; }
			public ISI.Extensions.Documents.IDocumentBorder Border => new DocumentBorder(_font.Border);
			public ISI.Extensions.Documents.IDocumentShading Shading => new DocumentShading(_font.Shading);
			public ISI.Extensions.Documents.IDocumentStyle Style { get => new DocumentStyle(_font.Style); set => _font.Style = value.GetAsposeStyle(); }
			public string StyleName { get => _font.StyleName; set => _font.StyleName = value; }
			public ISI.Extensions.Documents.StyleIdentifier StyleIdentifier { get => _font.StyleIdentifier.ToStyleIdentifier(); set => _font.StyleIdentifier = value.ToStyleIdentifier(); }
		}
	}
}
