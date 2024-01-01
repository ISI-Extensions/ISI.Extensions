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
using System.Threading.Tasks;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentParagraphFormat : ISI.Extensions.Documents.IDocumentParagraphFormat
		{
			internal global::Aspose.Words.ParagraphFormat _paragraphFormat = null;

			public DocumentParagraphFormat(global::Aspose.Words.ParagraphFormat paragraphFormat)
			{
				_paragraphFormat = paragraphFormat;
			}


			public void ClearFormatting()
			{
				_paragraphFormat.ClearFormatting();
			}

			public ISI.Extensions.Documents.ParagraphAlignment Alignment { get => _paragraphFormat.Alignment.ToParagraphAlignment(); set => _paragraphFormat.Alignment = value.ToParagraphAlignment(); }
			public bool NoSpaceBetweenParagraphsOfSameStyle { get => _paragraphFormat.NoSpaceBetweenParagraphsOfSameStyle; set => _paragraphFormat.NoSpaceBetweenParagraphsOfSameStyle = value; }
			public bool KeepTogether { get => _paragraphFormat.KeepTogether; set => _paragraphFormat.KeepTogether = value; }
			public bool KeepWithNext { get => _paragraphFormat.KeepWithNext; set => _paragraphFormat.KeepWithNext = value; }
			public bool PageBreakBefore { get => _paragraphFormat.PageBreakBefore; set => _paragraphFormat.PageBreakBefore = value; }
			public bool SuppressLineNumbers { get => _paragraphFormat.SuppressLineNumbers; set => _paragraphFormat.SuppressLineNumbers = value; }
			public bool SuppressAutoHyphens { get => _paragraphFormat.SuppressAutoHyphens; set => _paragraphFormat.SuppressAutoHyphens = value; }
			public bool WidowControl { get => _paragraphFormat.WidowControl; set => _paragraphFormat.WidowControl = value; }
			public bool AddSpaceBetweenFarEastAndAlpha { get => _paragraphFormat.AddSpaceBetweenFarEastAndAlpha; set => _paragraphFormat.AddSpaceBetweenFarEastAndAlpha = value; }
			public bool AddSpaceBetweenFarEastAndDigit { get => _paragraphFormat.AddSpaceBetweenFarEastAndDigit; set => _paragraphFormat.AddSpaceBetweenFarEastAndDigit = value; }
			public bool Bidi { get => _paragraphFormat.Bidi; set => _paragraphFormat.Bidi = value; }
			public double LeftIndent { get => _paragraphFormat.LeftIndent; set => _paragraphFormat.LeftIndent = value; }
			public double RightIndent { get => _paragraphFormat.RightIndent; set => _paragraphFormat.RightIndent = value; }
			public double FirstLineIndent { get => _paragraphFormat.FirstLineIndent; set => _paragraphFormat.FirstLineIndent = value; }
			public bool SpaceBeforeAuto { get => _paragraphFormat.SpaceBeforeAuto; set => _paragraphFormat.SpaceBeforeAuto = value; }
			public bool SpaceAfterAuto { get => _paragraphFormat.SpaceAfterAuto; set => _paragraphFormat.SpaceAfterAuto = value; }
			public double SpaceBefore { get => _paragraphFormat.SpaceBefore; set => _paragraphFormat.SpaceBefore = value; }
			public double SpaceAfter { get => _paragraphFormat.SpaceAfter; set => _paragraphFormat.SpaceAfter = value; }
			public ISI.Extensions.Documents.LineSpacingRule LineSpacingRule { get => _paragraphFormat.LineSpacingRule.ToLineSpacingRule(); set => _paragraphFormat.LineSpacingRule = value.ToLineSpacingRule(); }
			public double LineSpacing { get => _paragraphFormat.LineSpacing; set => _paragraphFormat.LineSpacing = value; }
			public bool IsHeading => _paragraphFormat.IsHeading;
			public bool IsListItem => _paragraphFormat.IsListItem;
			public ISI.Extensions.Documents.OutlineLevel OutlineLevel { get => _paragraphFormat.OutlineLevel.ToOutlineLevel(); set => _paragraphFormat.OutlineLevel = value.ToOutlineLevel(); }
			public int LinesToDrop { get => _paragraphFormat.LinesToDrop; set => _paragraphFormat.LinesToDrop = value; }
			public ISI.Extensions.Documents.DropCapPosition DropCapPosition { get => _paragraphFormat.DropCapPosition.ToDropCapPosition(); set => _paragraphFormat.DropCapPosition = value.ToDropCapPosition(); }
			public ISI.Extensions.Documents.IDocumentShading Shading => new DocumentShading(_paragraphFormat.Shading);
			public ISI.Extensions.Documents.IDocumentBorderCollection Borders => new DocumentBorderCollection(_paragraphFormat.Borders);
			public ISI.Extensions.Documents.IDocumentStyle Style { get => new DocumentStyle(_paragraphFormat.Style); set => _paragraphFormat.Style = value.GetAsposeStyle(); }
			public string StyleName { get => _paragraphFormat.StyleName; set => _paragraphFormat.StyleName = value; }
			public ISI.Extensions.Documents.StyleIdentifier StyleIdentifier { get => _paragraphFormat.StyleIdentifier.ToStyleIdentifier(); set => _paragraphFormat.StyleIdentifier = value.ToStyleIdentifier(); }
			public ISI.Extensions.Documents.IDocumentTabStopCollection TabStops => new DocumentTabStopCollection(_paragraphFormat.TabStops);
		}
	}
}