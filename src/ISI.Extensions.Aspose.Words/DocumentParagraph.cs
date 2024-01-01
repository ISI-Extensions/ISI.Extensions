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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentParagraph : ISI.Extensions.Documents.IDocumentParagraph
		{
			internal global::Aspose.Words.Paragraph _paragraph = null;

			public DocumentParagraph(global::Aspose.Words.Paragraph paragraph)
			{
				_paragraph = paragraph;
			}

			public string GetText()
			{
				return _paragraph.GetText();
			}

			public ISI.Extensions.Documents.IDocumentTabStop[] GetEffectiveTabStops()
			{
				return _paragraph.GetEffectiveTabStops().ToNullCheckedArray(tabStop => new DocumentTabStop(tabStop));
			}

			public int JoinRunsWithSameFormatting()
			{
				return _paragraph.JoinRunsWithSameFormatting();
			}

			public ISI.Extensions.Documents.IDocumentField AppendField(ISI.Extensions.Documents.FieldType fieldType, bool updateField)
			{
				return new DocumentField(_paragraph.AppendField(fieldType.ToFieldType(), updateField));
			}

			public ISI.Extensions.Documents.IDocumentField AppendField(string fieldCode)
			{
				return new DocumentField(_paragraph.AppendField(fieldCode));
			}

			public ISI.Extensions.Documents.IDocumentField AppendField(string fieldCode, string fieldValue)
			{
				return new DocumentField(_paragraph.AppendField(fieldCode, fieldCode));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(ISI.Extensions.Documents.FieldType fieldType, bool updateField, ISI.Extensions.Documents.IDocumentNode refNode, bool isAfter)
			{
				return new DocumentField(_paragraph.InsertField(fieldType.ToFieldType(), updateField, refNode.GetAsposeNode(), isAfter));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(string fieldCode, ISI.Extensions.Documents.IDocumentNode refNode, bool isAfter)
			{
				return new DocumentField(_paragraph.InsertField(fieldCode, refNode.GetAsposeNode(), isAfter));
			}

			public ISI.Extensions.Documents.IDocumentField InsertField(string fieldCode, string fieldValue, ISI.Extensions.Documents.IDocumentNode refNode, bool isAfter)
			{
				return new DocumentField(_paragraph.InsertField(fieldCode, fieldValue, refNode.GetAsposeNode(), isAfter));
			}

			public ISI.Extensions.Documents.NodeType NodeType => _paragraph.NodeType.ToNodeType();
			public ISI.Extensions.Documents.IDocumentSection ParentSection => new DocumentSection(_paragraph.ParentSection);
			public bool IsEndOfSection => _paragraph.IsEndOfSection;
			public bool IsEndOfHeaderFooter => _paragraph.IsEndOfHeaderFooter;
			public bool IsEndOfDocument => _paragraph.IsEndOfDocument;
			public ISI.Extensions.Documents.IDocumentParagraphFormat ParagraphFormat { get; }
			public ISI.Extensions.Documents.IDocumentFont ParagraphBreakFont => new DocumentFont(_paragraph.ParagraphBreakFont);
			public bool IsInsertRevision => _paragraph.IsInsertRevision;
			public bool IsDeleteRevision => _paragraph.IsDeleteRevision;
			public bool IsFormatRevision => _paragraph.IsFormatRevision;
		}
	}
}