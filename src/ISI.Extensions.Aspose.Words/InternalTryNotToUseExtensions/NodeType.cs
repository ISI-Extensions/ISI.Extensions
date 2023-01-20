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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.NodeType ToNodeType(this global::Aspose.Words.NodeType nodeType)
		{
			switch (nodeType)
			{
				case global::Aspose.Words.NodeType.Any: return ISI.Extensions.Documents.NodeType.Any;
				case global::Aspose.Words.NodeType.Document: return ISI.Extensions.Documents.NodeType.Document;
				case global::Aspose.Words.NodeType.Section: return ISI.Extensions.Documents.NodeType.Section;
				case global::Aspose.Words.NodeType.Body: return ISI.Extensions.Documents.NodeType.Body;
				case global::Aspose.Words.NodeType.HeaderFooter: return ISI.Extensions.Documents.NodeType.HeaderFooter;
				case global::Aspose.Words.NodeType.Table: return ISI.Extensions.Documents.NodeType.Table;
				case global::Aspose.Words.NodeType.Row: return ISI.Extensions.Documents.NodeType.Row;
				case global::Aspose.Words.NodeType.Cell: return ISI.Extensions.Documents.NodeType.Cell;
				case global::Aspose.Words.NodeType.Paragraph: return ISI.Extensions.Documents.NodeType.Paragraph;
				case global::Aspose.Words.NodeType.BookmarkStart: return ISI.Extensions.Documents.NodeType.BookmarkStart;
				case global::Aspose.Words.NodeType.BookmarkEnd: return ISI.Extensions.Documents.NodeType.BookmarkEnd;
				case global::Aspose.Words.NodeType.EditableRangeStart: return ISI.Extensions.Documents.NodeType.EditableRangeStart;
				case global::Aspose.Words.NodeType.EditableRangeEnd: return ISI.Extensions.Documents.NodeType.EditableRangeEnd;
				case global::Aspose.Words.NodeType.MoveFromRangeStart: return ISI.Extensions.Documents.NodeType.MoveFromRangeStart;
				case global::Aspose.Words.NodeType.MoveFromRangeEnd: return ISI.Extensions.Documents.NodeType.MoveFromRangeEnd;
				case global::Aspose.Words.NodeType.MoveToRangeStart: return ISI.Extensions.Documents.NodeType.MoveToRangeStart;
				case global::Aspose.Words.NodeType.MoveToRangeEnd: return ISI.Extensions.Documents.NodeType.MoveToRangeEnd;
				case global::Aspose.Words.NodeType.GroupShape: return ISI.Extensions.Documents.NodeType.GroupShape;
				case global::Aspose.Words.NodeType.Shape: return ISI.Extensions.Documents.NodeType.Shape;
				case global::Aspose.Words.NodeType.Comment: return ISI.Extensions.Documents.NodeType.Comment;
				case global::Aspose.Words.NodeType.Footnote: return ISI.Extensions.Documents.NodeType.Footnote;
				case global::Aspose.Words.NodeType.Run: return ISI.Extensions.Documents.NodeType.Run;
				case global::Aspose.Words.NodeType.FieldStart: return ISI.Extensions.Documents.NodeType.FieldStart;
				case global::Aspose.Words.NodeType.FieldSeparator: return ISI.Extensions.Documents.NodeType.FieldSeparator;
				case global::Aspose.Words.NodeType.FieldEnd: return ISI.Extensions.Documents.NodeType.FieldEnd;
				case global::Aspose.Words.NodeType.FormField: return ISI.Extensions.Documents.NodeType.FormField;
				case global::Aspose.Words.NodeType.SpecialChar: return ISI.Extensions.Documents.NodeType.SpecialChar;
				case global::Aspose.Words.NodeType.SmartTag: return ISI.Extensions.Documents.NodeType.SmartTag;
				case global::Aspose.Words.NodeType.StructuredDocumentTag: return ISI.Extensions.Documents.NodeType.StructuredDocumentTag;
				case global::Aspose.Words.NodeType.GlossaryDocument: return ISI.Extensions.Documents.NodeType.GlossaryDocument;
				case global::Aspose.Words.NodeType.BuildingBlock: return ISI.Extensions.Documents.NodeType.BuildingBlock;
				case global::Aspose.Words.NodeType.CommentRangeStart: return ISI.Extensions.Documents.NodeType.CommentRangeStart;
				case global::Aspose.Words.NodeType.CommentRangeEnd: return ISI.Extensions.Documents.NodeType.CommentRangeEnd;
				case global::Aspose.Words.NodeType.OfficeMath: return ISI.Extensions.Documents.NodeType.OfficeMath;
				case global::Aspose.Words.NodeType.SubDocument: return ISI.Extensions.Documents.NodeType.SubDocument;
				case global::Aspose.Words.NodeType.System: return ISI.Extensions.Documents.NodeType.System;
				case global::Aspose.Words.NodeType.Null: return ISI.Extensions.Documents.NodeType.Null;
				default:
					throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
			}
		}

		public static global::Aspose.Words.NodeType ToNodeType(this ISI.Extensions.Documents.NodeType nodeType)
		{
			switch (nodeType)
			{
				case ISI.Extensions.Documents.NodeType.Any: return global::Aspose.Words.NodeType.Any;
				case ISI.Extensions.Documents.NodeType.Document: return global::Aspose.Words.NodeType.Document;
				case ISI.Extensions.Documents.NodeType.Section: return global::Aspose.Words.NodeType.Section;
				case ISI.Extensions.Documents.NodeType.Body: return global::Aspose.Words.NodeType.Body;
				case ISI.Extensions.Documents.NodeType.HeaderFooter: return global::Aspose.Words.NodeType.HeaderFooter;
				case ISI.Extensions.Documents.NodeType.Table: return global::Aspose.Words.NodeType.Table;
				case ISI.Extensions.Documents.NodeType.Row: return global::Aspose.Words.NodeType.Row;
				case ISI.Extensions.Documents.NodeType.Cell: return global::Aspose.Words.NodeType.Cell;
				case ISI.Extensions.Documents.NodeType.Paragraph: return global::Aspose.Words.NodeType.Paragraph;
				case ISI.Extensions.Documents.NodeType.BookmarkStart: return global::Aspose.Words.NodeType.BookmarkStart;
				case ISI.Extensions.Documents.NodeType.BookmarkEnd: return global::Aspose.Words.NodeType.BookmarkEnd;
				case ISI.Extensions.Documents.NodeType.EditableRangeStart: return global::Aspose.Words.NodeType.EditableRangeStart;
				case ISI.Extensions.Documents.NodeType.EditableRangeEnd: return global::Aspose.Words.NodeType.EditableRangeEnd;
				case ISI.Extensions.Documents.NodeType.MoveFromRangeStart: return global::Aspose.Words.NodeType.MoveFromRangeStart;
				case ISI.Extensions.Documents.NodeType.MoveFromRangeEnd: return global::Aspose.Words.NodeType.MoveFromRangeEnd;
				case ISI.Extensions.Documents.NodeType.MoveToRangeStart: return global::Aspose.Words.NodeType.MoveToRangeStart;
				case ISI.Extensions.Documents.NodeType.MoveToRangeEnd: return global::Aspose.Words.NodeType.MoveToRangeEnd;
				case ISI.Extensions.Documents.NodeType.GroupShape: return global::Aspose.Words.NodeType.GroupShape;
				case ISI.Extensions.Documents.NodeType.Shape: return global::Aspose.Words.NodeType.Shape;
				case ISI.Extensions.Documents.NodeType.Comment: return global::Aspose.Words.NodeType.Comment;
				case ISI.Extensions.Documents.NodeType.Footnote: return global::Aspose.Words.NodeType.Footnote;
				case ISI.Extensions.Documents.NodeType.Run: return global::Aspose.Words.NodeType.Run;
				case ISI.Extensions.Documents.NodeType.FieldStart: return global::Aspose.Words.NodeType.FieldStart;
				case ISI.Extensions.Documents.NodeType.FieldSeparator: return global::Aspose.Words.NodeType.FieldSeparator;
				case ISI.Extensions.Documents.NodeType.FieldEnd: return global::Aspose.Words.NodeType.FieldEnd;
				case ISI.Extensions.Documents.NodeType.FormField: return global::Aspose.Words.NodeType.FormField;
				case ISI.Extensions.Documents.NodeType.SpecialChar: return global::Aspose.Words.NodeType.SpecialChar;
				case ISI.Extensions.Documents.NodeType.SmartTag: return global::Aspose.Words.NodeType.SmartTag;
				case ISI.Extensions.Documents.NodeType.StructuredDocumentTag: return global::Aspose.Words.NodeType.StructuredDocumentTag;
				case ISI.Extensions.Documents.NodeType.GlossaryDocument: return global::Aspose.Words.NodeType.GlossaryDocument;
				case ISI.Extensions.Documents.NodeType.BuildingBlock: return global::Aspose.Words.NodeType.BuildingBlock;
				case ISI.Extensions.Documents.NodeType.CommentRangeStart: return global::Aspose.Words.NodeType.CommentRangeStart;
				case ISI.Extensions.Documents.NodeType.CommentRangeEnd: return global::Aspose.Words.NodeType.CommentRangeEnd;
				case ISI.Extensions.Documents.NodeType.OfficeMath: return global::Aspose.Words.NodeType.OfficeMath;
				case ISI.Extensions.Documents.NodeType.SubDocument: return global::Aspose.Words.NodeType.SubDocument;
				case ISI.Extensions.Documents.NodeType.System: return global::Aspose.Words.NodeType.System;
				case ISI.Extensions.Documents.NodeType.Null: return global::Aspose.Words.NodeType.Null;
				default:
					throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
			}
		}
	}
}