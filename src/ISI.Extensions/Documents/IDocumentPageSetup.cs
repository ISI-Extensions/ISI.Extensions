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

namespace ISI.Extensions.Documents
{
	public interface IDocumentPageSetup
	{
		void ClearFormatting();
		bool OddAndEvenPagesHeaderFooter { get; set; }
		bool DifferentFirstPageHeaderFooter { get; set; }
		MultiplePagesType MultiplePages { get; set; }
		int SheetsPerBooklet { get; set; }
		SectionStart SectionStart { get; set; }
		bool SuppressEndnotes { get; set; }
		PageVerticalAlignment VerticalAlignment { get; set; }
		bool Bidi { get; set; }
		double PageWidth { get; set; }
		double PageHeight { get; set; }
		PaperSize PaperSize { get; set; }
		PageOrientation Orientation { get; set; }
		double LeftMargin { get; set; }
		double RightMargin { get; set; }
		double TopMargin { get; set; }
		double BottomMargin { get; set; }
		double HeaderDistance { get; set; }
		double FooterDistance { get; set; }
		double Gutter { get; set; }
		int FirstPageTray { get; set; }
		int OtherPagesTray { get; set; }
		NumberStyle PageNumberStyle { get; set; }
		bool RestartPageNumbering { get; set; }
		int PageStartingNumber { get; set; }
		LineNumberRestartMode LineNumberRestartMode { get; set; }
		int LineNumberCountBy { get; set; }
		double LineNumberDistanceFromText { get; set; }
		int LineStartingNumber { get; set; }
		IDocumentTextColumnCollection TextColumns { get; }
		bool RtlGutter { get; set; }
		bool BorderAlwaysInFront { get; set; }
		PageBorderDistanceFrom BorderDistanceFrom { get; set; }
		PageBorderAppliesTo BorderAppliesTo { get; set; }
		bool BorderSurroundsHeader { get; set; }
		bool BorderSurroundsFooter { get; set; }
		IDocumentBorderCollection Borders { get; }
		IDocumentFootnoteOptions FootnoteOptions { get; }
		IDocumentEndnoteOptions EndnoteOptions { get; }
		TextOrientation TextOrientation { get; set; }
	}
}
