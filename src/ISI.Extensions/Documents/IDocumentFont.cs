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
using System.Threading.Tasks;

namespace ISI.Extensions.Documents
{
	public interface IDocumentFont
	{
		void ClearFormatting();
		string Name { get; set; }
		string NameAscii { get; set; }
		string NameBi { get; set; }
		string NameFarEast { get; set; }
		string NameOther { get; set; }
		double Size { get; set; }
		double SizeBi { get; set; }
		bool Bold { get; set; }
		bool BoldBi { get; set; }
		bool Italic { get; set; }
		bool ItalicBi { get; set; }
		System.Drawing.Color Color { get; set; }
		System.Drawing.Color AutoColor { get; }
		bool StrikeThrough { get; set; }
		bool DoubleStrikeThrough { get; set; }
		bool Shadow { get; set; }
		bool Outline { get; set; }
		bool Emboss { get; set; }
		bool Engrave { get; set; }
		bool Superscript { get; set; }
		bool Subscript { get; set; }
		bool SmallCaps { get; set; }
		bool AllCaps { get; set; }
		bool Hidden { get; set; }
		Underline Underline { get; set; }
		System.Drawing.Color UnderlineColor { get; set; }
		int Scaling { get; set; }
		double Spacing { get; set; }
		double Position { get; set; }
		double Kerning { get; set; }
		System.Drawing.Color HighlightColor { get; set; }
		TextEffect TextEffect { get; set; }
		bool Bidi { get; set; }
		bool ComplexScript { get; set; }
		bool NoProofing { get; set; }
		int LocaleId { get; set; }
		int LocaleIdBi { get; set; }
		int LocaleIdFarEast { get; set; }
		IDocumentBorder Border { get; }
		IDocumentShading Shading { get; }
		IDocumentStyle Style { get; set; }
		string StyleName { get; set; }
		StyleIdentifier StyleIdentifier { get; set; }
	}
}
