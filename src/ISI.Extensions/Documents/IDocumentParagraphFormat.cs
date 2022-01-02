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
using System.Threading.Tasks;

namespace ISI.Extensions.Documents
{
	public interface IDocumentParagraphFormat
	{
		void ClearFormatting();
		ParagraphAlignment Alignment { get; set; }
		bool NoSpaceBetweenParagraphsOfSameStyle { get; set; }
		bool KeepTogether { get; set; }
		bool KeepWithNext { get; set; }
		bool PageBreakBefore { get; set; }
		bool SuppressLineNumbers { get; set; }
		bool SuppressAutoHyphens { get; set; }
		bool WidowControl { get; set; }
		bool AddSpaceBetweenFarEastAndAlpha { get; set; }
		bool AddSpaceBetweenFarEastAndDigit { get; set; }
		bool Bidi { get; set; }
		double LeftIndent { get; set; }
		double RightIndent { get; set; }
		double FirstLineIndent { get; set; }
		bool SpaceBeforeAuto { get; set; }
		bool SpaceAfterAuto { get; set; }
		double SpaceBefore { get; set; }
		double SpaceAfter { get; set; }
		LineSpacingRule LineSpacingRule { get; set; }
		double LineSpacing { get; set; }
		bool IsHeading { get; }
		bool IsListItem { get; }
		OutlineLevel OutlineLevel { get; set; }
		int LinesToDrop { get; set; }
		DropCapPosition DropCapPosition { get; set; }
		IDocumentShading Shading { get; }
		IDocumentBorderCollection Borders { get; }
		IDocumentStyle Style { get; set; }
		string StyleName { get; set; }
		StyleIdentifier StyleIdentifier { get; set; }
		IDocumentTabStopCollection TabStops { get; }
	}
}
