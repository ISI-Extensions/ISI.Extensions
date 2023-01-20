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
	public interface IDocumentEditor
	{
		void SetMargins(double? topInInches = null, double? rightInInches = null, double? bottomInInches = null, double? leftInInches = null);
		void MoveToDocumentStart();
		void MoveToDocumentEnd();
		void MoveToSection(int sectionIndex);
		void MoveToHeaderFooter(HeaderFooterType headerFooterType);
		bool MoveToMergeField(string fieldName);
		bool MoveToMergeField(string fieldName, bool isAfter, bool isDeleteField);
		void MoveToField(IDocumentField field, bool isAfter);
		bool MoveToBookmark(string bookmarkName);
		bool MoveToBookmark(string bookmarkName, bool isStart, bool isAfter);
		void MoveToParagraph(int paragraphIndex, int characterIndex);
		void MoveTo(IDocumentNode node);
		void Write(string text);
		void WriteLine(string text);
		void WriteLine();
		IDocumentParagraph InsertParagraph();
		void InsertBreak(BreakType breakType);
		IDocumentField InsertTableOfContents(string switches);
		IDocumentField InsertField(FieldType fieldType, bool updateField);
		IDocumentField InsertField(string fieldCode);
		IDocumentField InsertField(string fieldCode, string fieldValue);
		IDocumentFootnote InsertFootnote(FootnoteType footnoteType, string footnoteText);
		IDocumentFootnote InsertFootnote(FootnoteType footnoteType, string footnoteText, string referenceMark);
		IDocumentShape InsertImage(string fileName);
		IDocumentShape InsertImage(System.IO.Stream stream);
		IDocumentShape InsertImage(byte[] imageBytes);
		IDocumentShape InsertImage(string fileName, double width, double height);
		IDocumentShape InsertImage(System.IO.Stream stream, double width, double height);
		IDocumentShape InsertImage(byte[] imageBytes, double width, double height);
		IDocumentShape InsertImage(string fileName, RelativeHorizontalPosition relativeHorizontalPosition, double left, RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, WrapType wrapType);
		IDocumentShape InsertImage(System.IO.Stream stream, RelativeHorizontalPosition relativeHorizontalPosition, double left, RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, WrapType wrapType);
		IDocumentShape InsertImage(byte[] imageBytes, RelativeHorizontalPosition relativeHorizontalPosition, double left, RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, WrapType wrapType);
		void InsertHtml(string html);
		IDocumentBookmarkStart StartBookmark(string bookmarkName);
		IDocumentBookmarkEnd EndBookmark(string bookmarkName);
		void PushFont();
		void PopFont();
		void InsertNode(IDocumentNode node);
		IDocumentFont Font { get; }
		bool Bold { get; set; }
		bool Italic { get; set; }
		Underline Underline { get; set; }
		IDocumentParagraphFormat ParagraphFormat { get; }
		IDocumentPageSetup PageSetup { get; }
		bool IsAtStartOfParagraph { get; }
		bool IsAtEndOfParagraph { get; }
		IDocumentNode CurrentNode { get; }
		IDocumentParagraph CurrentParagraph { get; }
		IDocumentSection CurrentSection { get; }

		void MergeDocumentData(System.Data.IDataReader dataReader);
		void MergeDocumentData(ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot);
		void MergeDocumentData(ISI.Extensions.Documents.IDocumentDataSource documentDataSource);
		void DeleteMergeFields();

		void RemoveAllChildren();
		void AppendDocument(ISI.Extensions.Documents.IDocumentEditor document, ISI.Extensions.Documents.ImportFormatMode importFormatMode);

		void SetDocumentProperties(ISI.Extensions.Documents.IDocumentProperties documentProperties);

		void Print(string printerName);
		void Save(System.IO.Stream documentStream, string fileNameExtension);
		void Save(System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat documentFormat);
	}
}
