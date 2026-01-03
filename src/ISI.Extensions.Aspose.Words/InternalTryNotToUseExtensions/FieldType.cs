#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
		public static global::Aspose.Words.Fields.FieldType ToFieldType(this ISI.Extensions.Documents.FieldType fieldType)
		{
			switch (fieldType)
			{
				case ISI.Extensions.Documents.FieldType.FieldNone: return global::Aspose.Words.Fields.FieldType.FieldNone;
				case ISI.Extensions.Documents.FieldType.FieldCannotParse: return global::Aspose.Words.Fields.FieldType.FieldCannotParse;
				case ISI.Extensions.Documents.FieldType.FieldRefNoKeyword: return global::Aspose.Words.Fields.FieldType.FieldRefNoKeyword;
				case ISI.Extensions.Documents.FieldType.FieldRef: return global::Aspose.Words.Fields.FieldType.FieldRef;
				case ISI.Extensions.Documents.FieldType.FieldIndexEntry: return global::Aspose.Words.Fields.FieldType.FieldIndexEntry;
				case ISI.Extensions.Documents.FieldType.FieldFootnoteRef: return global::Aspose.Words.Fields.FieldType.FieldFootnoteRef;
				case ISI.Extensions.Documents.FieldType.FieldSet: return global::Aspose.Words.Fields.FieldType.FieldSet;
				case ISI.Extensions.Documents.FieldType.FieldIf: return global::Aspose.Words.Fields.FieldType.FieldIf;
				case ISI.Extensions.Documents.FieldType.FieldIndex: return global::Aspose.Words.Fields.FieldType.FieldIndex;
				case ISI.Extensions.Documents.FieldType.FieldTOCEntry: return global::Aspose.Words.Fields.FieldType.FieldTOCEntry;
				case ISI.Extensions.Documents.FieldType.FieldStyleRef: return global::Aspose.Words.Fields.FieldType.FieldStyleRef;
				case ISI.Extensions.Documents.FieldType.FieldRefDoc: return global::Aspose.Words.Fields.FieldType.FieldRefDoc;
				case ISI.Extensions.Documents.FieldType.FieldSequence: return global::Aspose.Words.Fields.FieldType.FieldSequence;
				case ISI.Extensions.Documents.FieldType.FieldTOC: return global::Aspose.Words.Fields.FieldType.FieldTOC;
				case ISI.Extensions.Documents.FieldType.FieldInfo: return global::Aspose.Words.Fields.FieldType.FieldInfo;
				case ISI.Extensions.Documents.FieldType.FieldTitle: return global::Aspose.Words.Fields.FieldType.FieldTitle;
				case ISI.Extensions.Documents.FieldType.FieldSubject: return global::Aspose.Words.Fields.FieldType.FieldSubject;
				case ISI.Extensions.Documents.FieldType.FieldAuthor: return global::Aspose.Words.Fields.FieldType.FieldAuthor;
				case ISI.Extensions.Documents.FieldType.FieldKeyword: return global::Aspose.Words.Fields.FieldType.FieldKeyword;
				case ISI.Extensions.Documents.FieldType.FieldComments: return global::Aspose.Words.Fields.FieldType.FieldComments;
				case ISI.Extensions.Documents.FieldType.FieldLastSavedBy: return global::Aspose.Words.Fields.FieldType.FieldLastSavedBy;
				case ISI.Extensions.Documents.FieldType.FieldCreateDate: return global::Aspose.Words.Fields.FieldType.FieldCreateDate;
				case ISI.Extensions.Documents.FieldType.FieldSaveDate: return global::Aspose.Words.Fields.FieldType.FieldSaveDate;
				case ISI.Extensions.Documents.FieldType.FieldPrintDate: return global::Aspose.Words.Fields.FieldType.FieldPrintDate;
				case ISI.Extensions.Documents.FieldType.FieldRevisionNum: return global::Aspose.Words.Fields.FieldType.FieldRevisionNum;
				case ISI.Extensions.Documents.FieldType.FieldEditTime: return global::Aspose.Words.Fields.FieldType.FieldEditTime;
				case ISI.Extensions.Documents.FieldType.FieldNumPages: return global::Aspose.Words.Fields.FieldType.FieldNumPages;
				case ISI.Extensions.Documents.FieldType.FieldNumWords: return global::Aspose.Words.Fields.FieldType.FieldNumWords;
				case ISI.Extensions.Documents.FieldType.FieldNumChars: return global::Aspose.Words.Fields.FieldType.FieldNumChars;
				case ISI.Extensions.Documents.FieldType.FieldFileName: return global::Aspose.Words.Fields.FieldType.FieldFileName;
				case ISI.Extensions.Documents.FieldType.FieldTemplate: return global::Aspose.Words.Fields.FieldType.FieldTemplate;
				case ISI.Extensions.Documents.FieldType.FieldDate: return global::Aspose.Words.Fields.FieldType.FieldDate;
				case ISI.Extensions.Documents.FieldType.FieldTime: return global::Aspose.Words.Fields.FieldType.FieldTime;
				case ISI.Extensions.Documents.FieldType.FieldPage: return global::Aspose.Words.Fields.FieldType.FieldPage;
				case ISI.Extensions.Documents.FieldType.FieldFormula: return global::Aspose.Words.Fields.FieldType.FieldFormula;
				case ISI.Extensions.Documents.FieldType.FieldQuote: return global::Aspose.Words.Fields.FieldType.FieldQuote;
				case ISI.Extensions.Documents.FieldType.FieldInclude: return global::Aspose.Words.Fields.FieldType.FieldInclude;
				case ISI.Extensions.Documents.FieldType.FieldPageRef: return global::Aspose.Words.Fields.FieldType.FieldPageRef;
				case ISI.Extensions.Documents.FieldType.FieldAsk: return global::Aspose.Words.Fields.FieldType.FieldAsk;
				case ISI.Extensions.Documents.FieldType.FieldFillIn: return global::Aspose.Words.Fields.FieldType.FieldFillIn;
				case ISI.Extensions.Documents.FieldType.FieldData: return global::Aspose.Words.Fields.FieldType.FieldData;
				case ISI.Extensions.Documents.FieldType.FieldNext: return global::Aspose.Words.Fields.FieldType.FieldNext;
				case ISI.Extensions.Documents.FieldType.FieldNextIf: return global::Aspose.Words.Fields.FieldType.FieldNextIf;
				case ISI.Extensions.Documents.FieldType.FieldSkipIf: return global::Aspose.Words.Fields.FieldType.FieldSkipIf;
				case ISI.Extensions.Documents.FieldType.FieldMergeRec: return global::Aspose.Words.Fields.FieldType.FieldMergeRec;
				case ISI.Extensions.Documents.FieldType.FieldDDE: return global::Aspose.Words.Fields.FieldType.FieldDDE;
				case ISI.Extensions.Documents.FieldType.FieldDDEAuto: return global::Aspose.Words.Fields.FieldType.FieldDDEAuto;
				case ISI.Extensions.Documents.FieldType.FieldGlossary: return global::Aspose.Words.Fields.FieldType.FieldGlossary;
				case ISI.Extensions.Documents.FieldType.FieldPrint: return global::Aspose.Words.Fields.FieldType.FieldPrint;
				case ISI.Extensions.Documents.FieldType.FieldEquation: return global::Aspose.Words.Fields.FieldType.FieldEquation;
				case ISI.Extensions.Documents.FieldType.FieldGoToButton: return global::Aspose.Words.Fields.FieldType.FieldGoToButton;
				case ISI.Extensions.Documents.FieldType.FieldMacroButton: return global::Aspose.Words.Fields.FieldType.FieldMacroButton;
				case ISI.Extensions.Documents.FieldType.FieldAutoNumOutline: return global::Aspose.Words.Fields.FieldType.FieldAutoNumOutline;
				case ISI.Extensions.Documents.FieldType.FieldAutoNumLegal: return global::Aspose.Words.Fields.FieldType.FieldAutoNumLegal;
				case ISI.Extensions.Documents.FieldType.FieldAutoNum: return global::Aspose.Words.Fields.FieldType.FieldAutoNum;
				case ISI.Extensions.Documents.FieldType.FieldImport: return global::Aspose.Words.Fields.FieldType.FieldImport;
				case ISI.Extensions.Documents.FieldType.FieldLink: return global::Aspose.Words.Fields.FieldType.FieldLink;
				case ISI.Extensions.Documents.FieldType.FieldSymbol: return global::Aspose.Words.Fields.FieldType.FieldSymbol;
				case ISI.Extensions.Documents.FieldType.FieldEmbed: return global::Aspose.Words.Fields.FieldType.FieldEmbed;
				case ISI.Extensions.Documents.FieldType.FieldMergeField: return global::Aspose.Words.Fields.FieldType.FieldMergeField;
				case ISI.Extensions.Documents.FieldType.FieldUserName: return global::Aspose.Words.Fields.FieldType.FieldUserName;
				case ISI.Extensions.Documents.FieldType.FieldUserInitials: return global::Aspose.Words.Fields.FieldType.FieldUserInitials;
				case ISI.Extensions.Documents.FieldType.FieldUserAddress: return global::Aspose.Words.Fields.FieldType.FieldUserAddress;
				case ISI.Extensions.Documents.FieldType.FieldBarcode: return global::Aspose.Words.Fields.FieldType.FieldBarcode;
				case ISI.Extensions.Documents.FieldType.FieldDocVariable: return global::Aspose.Words.Fields.FieldType.FieldDocVariable;
				case ISI.Extensions.Documents.FieldType.FieldSection: return global::Aspose.Words.Fields.FieldType.FieldSection;
				case ISI.Extensions.Documents.FieldType.FieldSectionPages: return global::Aspose.Words.Fields.FieldType.FieldSectionPages;
				case ISI.Extensions.Documents.FieldType.FieldIncludePicture: return global::Aspose.Words.Fields.FieldType.FieldIncludePicture;
				case ISI.Extensions.Documents.FieldType.FieldIncludeText: return global::Aspose.Words.Fields.FieldType.FieldIncludeText;
				case ISI.Extensions.Documents.FieldType.FieldFileSize: return global::Aspose.Words.Fields.FieldType.FieldFileSize;
				case ISI.Extensions.Documents.FieldType.FieldFormTextInput: return global::Aspose.Words.Fields.FieldType.FieldFormTextInput;
				case ISI.Extensions.Documents.FieldType.FieldFormCheckBox: return global::Aspose.Words.Fields.FieldType.FieldFormCheckBox;
				case ISI.Extensions.Documents.FieldType.FieldNoteRef: return global::Aspose.Words.Fields.FieldType.FieldNoteRef;
				case ISI.Extensions.Documents.FieldType.FieldTOA: return global::Aspose.Words.Fields.FieldType.FieldTOA;
				case ISI.Extensions.Documents.FieldType.FieldTOAEntry: return global::Aspose.Words.Fields.FieldType.FieldTOAEntry;
				case ISI.Extensions.Documents.FieldType.FieldMergeSeq: return global::Aspose.Words.Fields.FieldType.FieldMergeSeq;
				case ISI.Extensions.Documents.FieldType.FieldPrivate: return global::Aspose.Words.Fields.FieldType.FieldPrivate;
				case ISI.Extensions.Documents.FieldType.FieldDatabase: return global::Aspose.Words.Fields.FieldType.FieldDatabase;
				case ISI.Extensions.Documents.FieldType.FieldAutoText: return global::Aspose.Words.Fields.FieldType.FieldAutoText;
				case ISI.Extensions.Documents.FieldType.FieldCompare: return global::Aspose.Words.Fields.FieldType.FieldCompare;
				case ISI.Extensions.Documents.FieldType.FieldAddin: return global::Aspose.Words.Fields.FieldType.FieldAddin;
				case ISI.Extensions.Documents.FieldType.FieldFormDropDown: return global::Aspose.Words.Fields.FieldType.FieldFormDropDown;
				case ISI.Extensions.Documents.FieldType.FieldAdvance: return global::Aspose.Words.Fields.FieldType.FieldAdvance;
				case ISI.Extensions.Documents.FieldType.FieldDocProperty: return global::Aspose.Words.Fields.FieldType.FieldDocProperty;
				case ISI.Extensions.Documents.FieldType.FieldOcx: return global::Aspose.Words.Fields.FieldType.FieldOcx;
				case ISI.Extensions.Documents.FieldType.FieldHyperlink: return global::Aspose.Words.Fields.FieldType.FieldHyperlink;
				case ISI.Extensions.Documents.FieldType.FieldAutoTextList: return global::Aspose.Words.Fields.FieldType.FieldAutoTextList;
				case ISI.Extensions.Documents.FieldType.FieldListNum: return global::Aspose.Words.Fields.FieldType.FieldListNum;
				case ISI.Extensions.Documents.FieldType.FieldHtmlActiveX: return global::Aspose.Words.Fields.FieldType.FieldHtmlActiveX;
				case ISI.Extensions.Documents.FieldType.FieldBidiOutline: return global::Aspose.Words.Fields.FieldType.FieldBidiOutline;
				case ISI.Extensions.Documents.FieldType.FieldAddressBlock: return global::Aspose.Words.Fields.FieldType.FieldAddressBlock;
				case ISI.Extensions.Documents.FieldType.FieldGreetingLine: return global::Aspose.Words.Fields.FieldType.FieldGreetingLine;
				case ISI.Extensions.Documents.FieldType.FieldShape: return global::Aspose.Words.Fields.FieldType.FieldShape;
				case ISI.Extensions.Documents.FieldType.FieldCitation: return global::Aspose.Words.Fields.FieldType.FieldCitation;
				case ISI.Extensions.Documents.FieldType.FieldDisplayBarcode: return global::Aspose.Words.Fields.FieldType.FieldDisplayBarcode;
				case ISI.Extensions.Documents.FieldType.FieldMergeBarcode: return global::Aspose.Words.Fields.FieldType.FieldMergeBarcode;
				case ISI.Extensions.Documents.FieldType.FieldBibliography: return global::Aspose.Words.Fields.FieldType.FieldBibliography;
				default:
					throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null);
			}
		}

		public static ISI.Extensions.Documents.FieldType ToFieldType(this global::Aspose.Words.Fields.FieldType fieldType)
		{
			switch (fieldType)
			{
				case global::Aspose.Words.Fields.FieldType.FieldNone: return ISI.Extensions.Documents.FieldType.FieldNone;
				case global::Aspose.Words.Fields.FieldType.FieldCannotParse: return ISI.Extensions.Documents.FieldType.FieldCannotParse;
				case global::Aspose.Words.Fields.FieldType.FieldRefNoKeyword: return ISI.Extensions.Documents.FieldType.FieldRefNoKeyword;
				case global::Aspose.Words.Fields.FieldType.FieldRef: return ISI.Extensions.Documents.FieldType.FieldRef;
				case global::Aspose.Words.Fields.FieldType.FieldIndexEntry: return ISI.Extensions.Documents.FieldType.FieldIndexEntry;
				case global::Aspose.Words.Fields.FieldType.FieldFootnoteRef: return ISI.Extensions.Documents.FieldType.FieldFootnoteRef;
				case global::Aspose.Words.Fields.FieldType.FieldSet: return ISI.Extensions.Documents.FieldType.FieldSet;
				case global::Aspose.Words.Fields.FieldType.FieldIf: return ISI.Extensions.Documents.FieldType.FieldIf;
				case global::Aspose.Words.Fields.FieldType.FieldIndex: return ISI.Extensions.Documents.FieldType.FieldIndex;
				case global::Aspose.Words.Fields.FieldType.FieldTOCEntry: return ISI.Extensions.Documents.FieldType.FieldTOCEntry;
				case global::Aspose.Words.Fields.FieldType.FieldStyleRef: return ISI.Extensions.Documents.FieldType.FieldStyleRef;
				case global::Aspose.Words.Fields.FieldType.FieldRefDoc: return ISI.Extensions.Documents.FieldType.FieldRefDoc;
				case global::Aspose.Words.Fields.FieldType.FieldSequence: return ISI.Extensions.Documents.FieldType.FieldSequence;
				case global::Aspose.Words.Fields.FieldType.FieldTOC: return ISI.Extensions.Documents.FieldType.FieldTOC;
				case global::Aspose.Words.Fields.FieldType.FieldInfo: return ISI.Extensions.Documents.FieldType.FieldInfo;
				case global::Aspose.Words.Fields.FieldType.FieldTitle: return ISI.Extensions.Documents.FieldType.FieldTitle;
				case global::Aspose.Words.Fields.FieldType.FieldSubject: return ISI.Extensions.Documents.FieldType.FieldSubject;
				case global::Aspose.Words.Fields.FieldType.FieldAuthor: return ISI.Extensions.Documents.FieldType.FieldAuthor;
				case global::Aspose.Words.Fields.FieldType.FieldKeyword: return ISI.Extensions.Documents.FieldType.FieldKeyword;
				case global::Aspose.Words.Fields.FieldType.FieldComments: return ISI.Extensions.Documents.FieldType.FieldComments;
				case global::Aspose.Words.Fields.FieldType.FieldLastSavedBy: return ISI.Extensions.Documents.FieldType.FieldLastSavedBy;
				case global::Aspose.Words.Fields.FieldType.FieldCreateDate: return ISI.Extensions.Documents.FieldType.FieldCreateDate;
				case global::Aspose.Words.Fields.FieldType.FieldSaveDate: return ISI.Extensions.Documents.FieldType.FieldSaveDate;
				case global::Aspose.Words.Fields.FieldType.FieldPrintDate: return ISI.Extensions.Documents.FieldType.FieldPrintDate;
				case global::Aspose.Words.Fields.FieldType.FieldRevisionNum: return ISI.Extensions.Documents.FieldType.FieldRevisionNum;
				case global::Aspose.Words.Fields.FieldType.FieldEditTime: return ISI.Extensions.Documents.FieldType.FieldEditTime;
				case global::Aspose.Words.Fields.FieldType.FieldNumPages: return ISI.Extensions.Documents.FieldType.FieldNumPages;
				case global::Aspose.Words.Fields.FieldType.FieldNumWords: return ISI.Extensions.Documents.FieldType.FieldNumWords;
				case global::Aspose.Words.Fields.FieldType.FieldNumChars: return ISI.Extensions.Documents.FieldType.FieldNumChars;
				case global::Aspose.Words.Fields.FieldType.FieldFileName: return ISI.Extensions.Documents.FieldType.FieldFileName;
				case global::Aspose.Words.Fields.FieldType.FieldTemplate: return ISI.Extensions.Documents.FieldType.FieldTemplate;
				case global::Aspose.Words.Fields.FieldType.FieldDate: return ISI.Extensions.Documents.FieldType.FieldDate;
				case global::Aspose.Words.Fields.FieldType.FieldTime: return ISI.Extensions.Documents.FieldType.FieldTime;
				case global::Aspose.Words.Fields.FieldType.FieldPage: return ISI.Extensions.Documents.FieldType.FieldPage;
				case global::Aspose.Words.Fields.FieldType.FieldFormula: return ISI.Extensions.Documents.FieldType.FieldFormula;
				case global::Aspose.Words.Fields.FieldType.FieldQuote: return ISI.Extensions.Documents.FieldType.FieldQuote;
				case global::Aspose.Words.Fields.FieldType.FieldInclude: return ISI.Extensions.Documents.FieldType.FieldInclude;
				case global::Aspose.Words.Fields.FieldType.FieldPageRef: return ISI.Extensions.Documents.FieldType.FieldPageRef;
				case global::Aspose.Words.Fields.FieldType.FieldAsk: return ISI.Extensions.Documents.FieldType.FieldAsk;
				case global::Aspose.Words.Fields.FieldType.FieldFillIn: return ISI.Extensions.Documents.FieldType.FieldFillIn;
				case global::Aspose.Words.Fields.FieldType.FieldData: return ISI.Extensions.Documents.FieldType.FieldData;
				case global::Aspose.Words.Fields.FieldType.FieldNext: return ISI.Extensions.Documents.FieldType.FieldNext;
				case global::Aspose.Words.Fields.FieldType.FieldNextIf: return ISI.Extensions.Documents.FieldType.FieldNextIf;
				case global::Aspose.Words.Fields.FieldType.FieldSkipIf: return ISI.Extensions.Documents.FieldType.FieldSkipIf;
				case global::Aspose.Words.Fields.FieldType.FieldMergeRec: return ISI.Extensions.Documents.FieldType.FieldMergeRec;
				case global::Aspose.Words.Fields.FieldType.FieldDDE: return ISI.Extensions.Documents.FieldType.FieldDDE;
				case global::Aspose.Words.Fields.FieldType.FieldDDEAuto: return ISI.Extensions.Documents.FieldType.FieldDDEAuto;
				case global::Aspose.Words.Fields.FieldType.FieldGlossary: return ISI.Extensions.Documents.FieldType.FieldGlossary;
				case global::Aspose.Words.Fields.FieldType.FieldPrint: return ISI.Extensions.Documents.FieldType.FieldPrint;
				case global::Aspose.Words.Fields.FieldType.FieldEquation: return ISI.Extensions.Documents.FieldType.FieldEquation;
				case global::Aspose.Words.Fields.FieldType.FieldGoToButton: return ISI.Extensions.Documents.FieldType.FieldGoToButton;
				case global::Aspose.Words.Fields.FieldType.FieldMacroButton: return ISI.Extensions.Documents.FieldType.FieldMacroButton;
				case global::Aspose.Words.Fields.FieldType.FieldAutoNumOutline: return ISI.Extensions.Documents.FieldType.FieldAutoNumOutline;
				case global::Aspose.Words.Fields.FieldType.FieldAutoNumLegal: return ISI.Extensions.Documents.FieldType.FieldAutoNumLegal;
				case global::Aspose.Words.Fields.FieldType.FieldAutoNum: return ISI.Extensions.Documents.FieldType.FieldAutoNum;
				case global::Aspose.Words.Fields.FieldType.FieldImport: return ISI.Extensions.Documents.FieldType.FieldImport;
				case global::Aspose.Words.Fields.FieldType.FieldLink: return ISI.Extensions.Documents.FieldType.FieldLink;
				case global::Aspose.Words.Fields.FieldType.FieldSymbol: return ISI.Extensions.Documents.FieldType.FieldSymbol;
				case global::Aspose.Words.Fields.FieldType.FieldEmbed: return ISI.Extensions.Documents.FieldType.FieldEmbed;
				case global::Aspose.Words.Fields.FieldType.FieldMergeField: return ISI.Extensions.Documents.FieldType.FieldMergeField;
				case global::Aspose.Words.Fields.FieldType.FieldUserName: return ISI.Extensions.Documents.FieldType.FieldUserName;
				case global::Aspose.Words.Fields.FieldType.FieldUserInitials: return ISI.Extensions.Documents.FieldType.FieldUserInitials;
				case global::Aspose.Words.Fields.FieldType.FieldUserAddress: return ISI.Extensions.Documents.FieldType.FieldUserAddress;
				case global::Aspose.Words.Fields.FieldType.FieldBarcode: return ISI.Extensions.Documents.FieldType.FieldBarcode;
				case global::Aspose.Words.Fields.FieldType.FieldDocVariable: return ISI.Extensions.Documents.FieldType.FieldDocVariable;
				case global::Aspose.Words.Fields.FieldType.FieldSection: return ISI.Extensions.Documents.FieldType.FieldSection;
				case global::Aspose.Words.Fields.FieldType.FieldSectionPages: return ISI.Extensions.Documents.FieldType.FieldSectionPages;
				case global::Aspose.Words.Fields.FieldType.FieldIncludePicture: return ISI.Extensions.Documents.FieldType.FieldIncludePicture;
				case global::Aspose.Words.Fields.FieldType.FieldIncludeText: return ISI.Extensions.Documents.FieldType.FieldIncludeText;
				case global::Aspose.Words.Fields.FieldType.FieldFileSize: return ISI.Extensions.Documents.FieldType.FieldFileSize;
				case global::Aspose.Words.Fields.FieldType.FieldFormTextInput: return ISI.Extensions.Documents.FieldType.FieldFormTextInput;
				case global::Aspose.Words.Fields.FieldType.FieldFormCheckBox: return ISI.Extensions.Documents.FieldType.FieldFormCheckBox;
				case global::Aspose.Words.Fields.FieldType.FieldNoteRef: return ISI.Extensions.Documents.FieldType.FieldNoteRef;
				case global::Aspose.Words.Fields.FieldType.FieldTOA: return ISI.Extensions.Documents.FieldType.FieldTOA;
				case global::Aspose.Words.Fields.FieldType.FieldTOAEntry: return ISI.Extensions.Documents.FieldType.FieldTOAEntry;
				case global::Aspose.Words.Fields.FieldType.FieldMergeSeq: return ISI.Extensions.Documents.FieldType.FieldMergeSeq;
				case global::Aspose.Words.Fields.FieldType.FieldPrivate: return ISI.Extensions.Documents.FieldType.FieldPrivate;
				case global::Aspose.Words.Fields.FieldType.FieldDatabase: return ISI.Extensions.Documents.FieldType.FieldDatabase;
				case global::Aspose.Words.Fields.FieldType.FieldAutoText: return ISI.Extensions.Documents.FieldType.FieldAutoText;
				case global::Aspose.Words.Fields.FieldType.FieldCompare: return ISI.Extensions.Documents.FieldType.FieldCompare;
				case global::Aspose.Words.Fields.FieldType.FieldAddin: return ISI.Extensions.Documents.FieldType.FieldAddin;
				case global::Aspose.Words.Fields.FieldType.FieldFormDropDown: return ISI.Extensions.Documents.FieldType.FieldFormDropDown;
				case global::Aspose.Words.Fields.FieldType.FieldAdvance: return ISI.Extensions.Documents.FieldType.FieldAdvance;
				case global::Aspose.Words.Fields.FieldType.FieldDocProperty: return ISI.Extensions.Documents.FieldType.FieldDocProperty;
				case global::Aspose.Words.Fields.FieldType.FieldOcx: return ISI.Extensions.Documents.FieldType.FieldOcx;
				case global::Aspose.Words.Fields.FieldType.FieldHyperlink: return ISI.Extensions.Documents.FieldType.FieldHyperlink;
				case global::Aspose.Words.Fields.FieldType.FieldAutoTextList: return ISI.Extensions.Documents.FieldType.FieldAutoTextList;
				case global::Aspose.Words.Fields.FieldType.FieldListNum: return ISI.Extensions.Documents.FieldType.FieldListNum;
				case global::Aspose.Words.Fields.FieldType.FieldHtmlActiveX: return ISI.Extensions.Documents.FieldType.FieldHtmlActiveX;
				case global::Aspose.Words.Fields.FieldType.FieldBidiOutline: return ISI.Extensions.Documents.FieldType.FieldBidiOutline;
				case global::Aspose.Words.Fields.FieldType.FieldAddressBlock: return ISI.Extensions.Documents.FieldType.FieldAddressBlock;
				case global::Aspose.Words.Fields.FieldType.FieldGreetingLine: return ISI.Extensions.Documents.FieldType.FieldGreetingLine;
				case global::Aspose.Words.Fields.FieldType.FieldShape: return ISI.Extensions.Documents.FieldType.FieldShape;
				case global::Aspose.Words.Fields.FieldType.FieldCitation: return ISI.Extensions.Documents.FieldType.FieldCitation;
				case global::Aspose.Words.Fields.FieldType.FieldDisplayBarcode: return ISI.Extensions.Documents.FieldType.FieldDisplayBarcode;
				case global::Aspose.Words.Fields.FieldType.FieldMergeBarcode: return ISI.Extensions.Documents.FieldType.FieldMergeBarcode;
				case global::Aspose.Words.Fields.FieldType.FieldBibliography: return ISI.Extensions.Documents.FieldType.FieldBibliography;
				default:
					throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null);
			}
		}
	}
}