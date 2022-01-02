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
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentEditor : ISI.Extensions.Documents.IDocumentEditor
		{
			internal global::Aspose.Words.DocumentBuilder _documentBuilder = null;

			public DocumentEditor(global::Aspose.Words.Document document)
			{
				_documentBuilder = new global::Aspose.Words.DocumentBuilder(document);
			}

			public void SetMargins(double? topInInches = null, double? rightInInches = null, double? bottomInInches = null, double? leftInInches = null)
			{
				var pageSetup = _documentBuilder.PageSetup;

				if (topInInches.HasValue)
				{
					pageSetup.TopMargin = global::Aspose.Words.ConvertUtil.InchToPoint(topInInches.Value);
				}

				if (rightInInches.HasValue)
				{
					pageSetup.RightMargin = global::Aspose.Words.ConvertUtil.InchToPoint(rightInInches.Value);
				}

				if (bottomInInches.HasValue)
				{
					pageSetup.BottomMargin = global::Aspose.Words.ConvertUtil.InchToPoint(bottomInInches.Value);
				}

				if (leftInInches.HasValue)
				{
					pageSetup.LeftMargin = global::Aspose.Words.ConvertUtil.InchToPoint(leftInInches.Value);
				}
			}

			public void MoveToDocumentStart()
			{
				_documentBuilder.MoveToDocumentStart();
			}

			public void MoveToDocumentEnd()
			{
				_documentBuilder.MoveToDocumentEnd();
			}

			public void MoveToSection(int sectionIndex)
			{
				_documentBuilder.MoveToSection(sectionIndex);
			}

			public void MoveToHeaderFooter(ISI.Extensions.Documents.HeaderFooterType headerFooterType)
			{
				_documentBuilder.MoveToHeaderFooter(headerFooterType.ToHeaderFooterType());
			}

			public bool MoveToMergeField(string fieldName)
			{
				return _documentBuilder.MoveToMergeField(fieldName);
			}

			public bool MoveToMergeField(string fieldName, bool isAfter, bool isDeleteField)
			{
				return _documentBuilder.MoveToMergeField(fieldName, isAfter, isDeleteField);
			}

			public void MoveToField(ISI.Extensions.Documents.IDocumentField field, bool isAfter)
			{
				_documentBuilder.MoveToField(field.GetAsposeField(), isAfter);
			}

			public bool MoveToBookmark(string bookmarkName)
			{
				return _documentBuilder.MoveToBookmark(bookmarkName);
			}

			public bool MoveToBookmark(string bookmarkName, bool isStart, bool isAfter)
			{
				return _documentBuilder.MoveToBookmark(bookmarkName, isStart, isAfter);
			}

			public void MoveToParagraph(int paragraphIndex, int characterIndex)
			{
				_documentBuilder.MoveToParagraph(paragraphIndex, characterIndex);
			}

			public void MoveTo(ISI.Extensions.Documents.IDocumentNode node)
			{
				_documentBuilder.MoveTo(node.GetAsposeNode());
			}

			public void Write(string text)
			{
				_documentBuilder.Write(text);
			}

			public void WriteLine(string text)
			{
				_documentBuilder.Writeln(text);
			}

			public void WriteLine()
			{
				_documentBuilder.Writeln();
			}

			public ISI.Extensions.Documents.IDocumentParagraph InsertParagraph()
			{
				return new DocumentParagraph(_documentBuilder.InsertParagraph());
			}

			public void InsertBreak(ISI.Extensions.Documents.BreakType breakType)
			{
				_documentBuilder.InsertBreak(breakType.ToBreakType());
			}

			public ISI.Extensions.Documents.IDocumentField InsertTableOfContents(string switches)
			{
				return new DocumentField(_documentBuilder.InsertTableOfContents(switches));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(ISI.Extensions.Documents.FieldType fieldType, bool updateField)
			{
				return new DocumentField(_documentBuilder.InsertField(fieldType.ToFieldType(), updateField));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(string fieldCode)
			{
				return new DocumentField(_documentBuilder.InsertField(fieldCode));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(string fieldCode, string fieldValue)
			{
				return new DocumentField(_documentBuilder.InsertField(fieldCode, fieldValue));
			}

			public ISI.Extensions.Documents.IDocumentFootnote InsertFootnote(ISI.Extensions.Documents.FootnoteType footnoteType, string footnoteText)
			{
				return new DocumentFootnote(_documentBuilder.InsertFootnote(footnoteType.ToFootnoteType(), footnoteText));
			}

			public ISI.Extensions.Documents.IDocumentFootnote InsertFootnote(ISI.Extensions.Documents.FootnoteType footnoteType, string footnoteText, string referenceMark)
			{
				return new DocumentFootnote(_documentBuilder.InsertFootnote(footnoteType.ToFootnoteType(), footnoteText, referenceMark));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(string fileName)
			{
				return new DocumentShape(_documentBuilder.InsertImage(fileName));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(System.IO.Stream stream)
			{
				return new DocumentShape(_documentBuilder.InsertImage(stream));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(byte[] imageBytes)
			{
				return new DocumentShape(_documentBuilder.InsertImage(imageBytes));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(string fileName, double width, double height)
			{
				return new DocumentShape(_documentBuilder.InsertImage(fileName, width, height));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(System.IO.Stream stream, double width, double height)
			{
				return new DocumentShape(_documentBuilder.InsertImage(stream, width, height));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(byte[] imageBytes, double width, double height)
			{
				return new DocumentShape(_documentBuilder.InsertImage(imageBytes, width, height));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(string fileName, ISI.Extensions.Documents.RelativeHorizontalPosition relativeHorizontalPosition, double left, ISI.Extensions.Documents.RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, ISI.Extensions.Documents.WrapType wrapType)
			{
				return new DocumentShape(_documentBuilder.InsertImage(fileName, relativeHorizontalPosition.ToRelativeHorizontalPosition(), left, relativeVerticalPosition.ToRelativeVerticalPosition(), top, width, height, wrapType.ToWrapType()));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(System.IO.Stream stream, ISI.Extensions.Documents.RelativeHorizontalPosition relativeHorizontalPosition, double left, ISI.Extensions.Documents.RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, ISI.Extensions.Documents.WrapType wrapType)
			{
				return new DocumentShape(_documentBuilder.InsertImage(stream, relativeHorizontalPosition.ToRelativeHorizontalPosition(), left, relativeVerticalPosition.ToRelativeVerticalPosition(), top, width, height, wrapType.ToWrapType()));
			}

			public ISI.Extensions.Documents.IDocumentShape InsertImage(byte[] imageBytes, ISI.Extensions.Documents.RelativeHorizontalPosition relativeHorizontalPosition, double left, ISI.Extensions.Documents.RelativeVerticalPosition relativeVerticalPosition, double top, double width, double height, ISI.Extensions.Documents.WrapType wrapType)
			{
				return new DocumentShape(_documentBuilder.InsertImage(imageBytes, relativeHorizontalPosition.ToRelativeHorizontalPosition(), left, relativeVerticalPosition.ToRelativeVerticalPosition(), top, width, height, wrapType.ToWrapType()));
			}

			public void InsertHtml(string html)
			{
				_documentBuilder.InsertHtml(html);
			}

			public ISI.Extensions.Documents.IDocumentBookmarkStart StartBookmark(string bookmarkName)
			{
				return new DocumentBookmarkStart(_documentBuilder.StartBookmark(bookmarkName));
			}

			public ISI.Extensions.Documents.IDocumentBookmarkEnd EndBookmark(string bookmarkName)
			{
				return new DocumentBookmarkEnd(_documentBuilder.EndBookmark(bookmarkName));
			}

			public void PushFont()
			{
				_documentBuilder.PushFont();
			}

			public void PopFont()
			{
				_documentBuilder.PopFont();
			}

			public void InsertNode(ISI.Extensions.Documents.IDocumentNode node)
			{
				_documentBuilder.InsertNode(node.GetAsposeNode());
			}

			public ISI.Extensions.Documents.IDocumentFont Font => new DocumentFont(_documentBuilder.Font);
			public bool Bold { get => _documentBuilder.Bold; set => _documentBuilder.Bold = value; }
			public bool Italic { get => _documentBuilder.Italic; set => _documentBuilder.Italic = value; }
			public ISI.Extensions.Documents.Underline Underline { get => _documentBuilder.Underline.ToUnderline(); set => _documentBuilder.Underline = value.ToUnderline(); }
			public ISI.Extensions.Documents.IDocumentParagraphFormat ParagraphFormat => new DocumentParagraphFormat(_documentBuilder.ParagraphFormat);
			public ISI.Extensions.Documents.IDocumentPageSetup PageSetup => new DocumentPageSetup(_documentBuilder.PageSetup);
			public bool IsAtStartOfParagraph => _documentBuilder.IsAtStartOfParagraph;
			public bool IsAtEndOfParagraph => _documentBuilder.IsAtEndOfParagraph;
			public ISI.Extensions.Documents.IDocumentNode CurrentNode => new DocumentNode(_documentBuilder.CurrentNode);
			public ISI.Extensions.Documents.IDocumentParagraph CurrentParagraph => new DocumentParagraph(_documentBuilder.CurrentParagraph);
			public ISI.Extensions.Documents.IDocumentSection CurrentSection => new DocumentSection(_documentBuilder.CurrentSection);

			public void MergeDocumentData(System.Data.IDataReader dataReader)
			{
				_documentBuilder.Document.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				_documentBuilder.Document.MailMerge.Execute(dataReader); 
			}
			public void MergeDocumentData(ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot)
			{
				_documentBuilder.Document.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				_documentBuilder.Document.MailMerge.UseNonMergeFields = true;

				_documentBuilder.Document.MailMerge.ExecuteWithRegions(new MailMergeDataSourceRoot(documentDataSourceRoot));
			}
			public void MergeDocumentData(ISI.Extensions.Documents.IDocumentDataSource documentDataSource)
			{
				_documentBuilder.Document.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				_documentBuilder.Document.MailMerge.UseNonMergeFields = true;

				_documentBuilder.Document.MailMerge.Execute(new DocumentDataSource(documentDataSource));
			}

			public void DeleteMergeFields()
			{
				_documentBuilder.Document.MailMerge.DeleteFields();
			}

			public void SetDocumentProperties(ISI.Extensions.Documents.IDocumentProperties documentProperties)
			{
				_documentBuilder.Document.SetDocumentProperties(documentProperties);
			}


			public void RemoveAllChildren()
			{
				_documentBuilder.Document.RemoveAllChildren();
			}
			public void AppendDocument(ISI.Extensions.Documents.IDocumentEditor document, ISI.Extensions.Documents.ImportFormatMode importFormatMode)
			{
				_documentBuilder.Document.AppendDocument(document.GetAsposeDocument(), importFormatMode.ToImportFormatMode());
			}



			public void Print(string printerName)
			{
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Print(_documentBuilder.Document, printerName);
			}

			public void Save(System.IO.Stream documentStream, string fileNameExtension)
			{
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Save(_documentBuilder.Document, documentStream, fileNameExtension);
			}
			public void Save(System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat documentFormat)
			{
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Save(_documentBuilder.Document, documentStream, documentFormat);
			}
		}
	}
}