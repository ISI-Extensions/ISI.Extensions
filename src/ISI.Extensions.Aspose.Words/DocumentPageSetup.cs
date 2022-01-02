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
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentPageSetup : ISI.Extensions.Documents.IDocumentPageSetup
		{
			internal global::Aspose.Words.PageSetup _pageSetup = null;

			public DocumentPageSetup(global::Aspose.Words.PageSetup pageSetup)
			{
				_pageSetup = pageSetup;
			}

			public void ClearFormatting()
			{
				_pageSetup.ClearFormatting();
			}

			public bool OddAndEvenPagesHeaderFooter { get => _pageSetup.OddAndEvenPagesHeaderFooter; set => _pageSetup.OddAndEvenPagesHeaderFooter = value; }
			public bool DifferentFirstPageHeaderFooter { get => _pageSetup.DifferentFirstPageHeaderFooter; set => _pageSetup.DifferentFirstPageHeaderFooter = value; }
			public ISI.Extensions.Documents.MultiplePagesType MultiplePages { get => _pageSetup.MultiplePages.ToMultiplePagesType(); set => _pageSetup.MultiplePages = value.ToMultiplePagesType(); }
			public int SheetsPerBooklet { get => _pageSetup.SheetsPerBooklet; set => _pageSetup.SheetsPerBooklet = value; }
			public ISI.Extensions.Documents.SectionStart SectionStart { get => _pageSetup.SectionStart.ToSectionStart(); set => _pageSetup.SectionStart = value.ToSectionStart(); }
			public bool SuppressEndnotes { get => _pageSetup.SuppressEndnotes; set => _pageSetup.SuppressEndnotes = value; }
			public ISI.Extensions.Documents.PageVerticalAlignment VerticalAlignment { get => _pageSetup.VerticalAlignment.ToPageVerticalAlignment(); set => _pageSetup.VerticalAlignment = value.ToPageVerticalAlignment(); }
			public bool Bidi { get => _pageSetup.Bidi; set => _pageSetup.Bidi = value; }
			public double PageWidth { get => _pageSetup.PageWidth; set => _pageSetup.PageWidth = value; }
			public double PageHeight { get => _pageSetup.PageHeight; set => _pageSetup.PageHeight = value; }
			public ISI.Extensions.Documents.PaperSize PaperSize { get => _pageSetup.PaperSize.ToPaperSize(); set => _pageSetup.PaperSize = value.ToPaperSize(); }
			public ISI.Extensions.Documents.PageOrientation Orientation { get => _pageSetup.Orientation.ToPageOrientation(); set => _pageSetup.Orientation = value.ToPageOrientation(); }
			public double LeftMargin { get => _pageSetup.LeftMargin; set => _pageSetup.LeftMargin = value; }
			public double RightMargin { get => _pageSetup.RightMargin; set => _pageSetup.RightMargin = value; }
			public double TopMargin { get => _pageSetup.TopMargin; set => _pageSetup.TopMargin = value; }
			public double BottomMargin { get => _pageSetup.BottomMargin; set => _pageSetup.BottomMargin = value; }
			public double HeaderDistance { get => _pageSetup.HeaderDistance; set => _pageSetup.HeaderDistance = value; }
			public double FooterDistance { get => _pageSetup.FooterDistance; set => _pageSetup.FooterDistance = value; }
			public double Gutter { get => _pageSetup.Gutter; set => _pageSetup.Gutter = value; }
			public int FirstPageTray { get => _pageSetup.FirstPageTray; set => _pageSetup.FirstPageTray = value; }
			public int OtherPagesTray { get => _pageSetup.OtherPagesTray; set => _pageSetup.OtherPagesTray = value; }
			public ISI.Extensions.Documents.NumberStyle PageNumberStyle { get => _pageSetup.PageNumberStyle.ToNumberStyle(); set => _pageSetup.PageNumberStyle = value.ToNumberStyle(); }
			public bool RestartPageNumbering { get => _pageSetup.RestartPageNumbering; set => _pageSetup.RestartPageNumbering = value; }
			public int PageStartingNumber { get => _pageSetup.PageStartingNumber; set => _pageSetup.PageStartingNumber = value; }
			public ISI.Extensions.Documents.LineNumberRestartMode LineNumberRestartMode { get => _pageSetup.LineNumberRestartMode.ToLineNumberRestartMode(); set => _pageSetup.LineNumberRestartMode = value.ToLineNumberRestartMode(); }
			public int LineNumberCountBy { get => _pageSetup.LineNumberCountBy; set => _pageSetup.LineNumberCountBy = value; }
			public double LineNumberDistanceFromText { get => _pageSetup.LineNumberDistanceFromText; set => _pageSetup.LineNumberDistanceFromText = value; }
			public int LineStartingNumber { get => _pageSetup.LineStartingNumber; set => _pageSetup.LineStartingNumber = value; }
			public ISI.Extensions.Documents.IDocumentTextColumnCollection TextColumns => new DocumentTextColumnCollection(_pageSetup.TextColumns);
			public bool RtlGutter { get => _pageSetup.RtlGutter; set => _pageSetup.RtlGutter = value; }
			public bool BorderAlwaysInFront { get => _pageSetup.BorderAlwaysInFront; set => _pageSetup.BorderAlwaysInFront = value; }
			public ISI.Extensions.Documents.PageBorderDistanceFrom BorderDistanceFrom { get => _pageSetup.BorderDistanceFrom.ToPageBorderDistanceFrom(); set => _pageSetup.BorderDistanceFrom = value.ToPageBorderDistanceFrom(); }
			public ISI.Extensions.Documents.PageBorderAppliesTo BorderAppliesTo { get => _pageSetup.BorderAppliesTo.ToPageBorderAppliesTo(); set => _pageSetup.BorderAppliesTo = value.ToPageBorderAppliesTo(); }
			public bool BorderSurroundsHeader { get => _pageSetup.BorderSurroundsHeader; set => _pageSetup.BorderSurroundsHeader = value; }
			public bool BorderSurroundsFooter { get => _pageSetup.BorderSurroundsFooter; set => _pageSetup.BorderSurroundsFooter = value; }
			public ISI.Extensions.Documents.IDocumentBorderCollection Borders => new DocumentBorderCollection(_pageSetup.Borders);
			public ISI.Extensions.Documents.IDocumentFootnoteOptions FootnoteOptions => new DocumentFootnoteOptions(_pageSetup.FootnoteOptions);
			public ISI.Extensions.Documents.IDocumentEndnoteOptions EndnoteOptions => new DocumentEndnoteOptions(_pageSetup.EndnoteOptions);
			public ISI.Extensions.Documents.TextOrientation TextOrientation { get => _pageSetup.TextOrientation.ToTextOrientation(); set => _pageSetup.TextOrientation = value.ToTextOrientation(); }
		}
	}
}