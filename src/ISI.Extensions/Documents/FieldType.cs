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
	public enum FieldType
	{
		FieldNone = 0,
		FieldCannotParse = 1,
		FieldRefNoKeyword = 2,
		FieldRef = 3,
		FieldIndexEntry = 4,
		FieldFootnoteRef = 5,
		FieldSet = 6,
		FieldIf = 7,
		FieldIndex = 8,
		FieldTOCEntry = 9,
		FieldStyleRef = 10,
		FieldRefDoc = 11,
		FieldSequence = 12,
		FieldTOC = 13,
		FieldInfo = 14,
		FieldTitle = 15,
		FieldSubject = 16,
		FieldAuthor = 17,
		FieldKeyword = 18,
		FieldComments = 19,
		FieldLastSavedBy = 20,
		FieldCreateDate = 21,
		FieldSaveDate = 22,
		FieldPrintDate = 23,
		FieldRevisionNum = 24,
		FieldEditTime = 25,
		FieldNumPages = 26,
		FieldNumWords = 27,
		FieldNumChars = 28,
		FieldFileName = 29,
		FieldTemplate = 30,
		FieldDate = 31,
		FieldTime = 32,
		FieldPage = 33,
		FieldFormula = 34,
		FieldQuote = 35,
		FieldInclude = 36,
		FieldPageRef = 37,
		FieldAsk = 38,
		FieldFillIn = 39,
		FieldData = 40,
		FieldNext = 41,
		FieldNextIf = 42,
		FieldSkipIf = 43,
		FieldMergeRec = 44,
		FieldDDE = 45,
		FieldDDEAuto = 46,
		FieldGlossary = 47,
		FieldPrint = 48,
		FieldEquation = 49,
		FieldGoToButton = 50,
		FieldMacroButton = 51,
		FieldAutoNumOutline = 52,
		FieldAutoNumLegal = 53,
		FieldAutoNum = 54,
		FieldImport = 55,
		FieldLink = 56,
		FieldSymbol = 57,
		FieldEmbed = 58,
		FieldMergeField = 59,
		FieldUserName = 60,
		FieldUserInitials = 61,
		FieldUserAddress = 62,
		FieldBarcode = 63,
		FieldDocVariable = 64,
		FieldSection = 65,
		FieldSectionPages = 66,
		FieldIncludePicture = 67,
		FieldIncludeText = 68,
		FieldFileSize = 69,
		FieldFormTextInput = 70,
		FieldFormCheckBox = 71,
		FieldNoteRef = 72,
		FieldTOA = 73,
		FieldTOAEntry = 74,
		FieldMergeSeq = 75,
		FieldPrivate = 77,
		FieldDatabase = 78,
		FieldAutoText = 79,
		FieldCompare = 80,
		FieldAddin = 81,
		FieldFormDropDown = 83,
		FieldAdvance = 84,
		FieldDocProperty = 85,
		FieldOcx = 87,
		FieldHyperlink = 88,
		FieldAutoTextList = 89,
		FieldListNum = 90,
		FieldHtmlActiveX = 91,
		FieldBidiOutline = 92,
		FieldAddressBlock = 93,
		FieldGreetingLine = 94,
		FieldShape = 95,
		FieldCitation = 1980,
		FieldDisplayBarcode = 6301,
		FieldMergeBarcode = 6302,
		FieldBibliography = 100500,
	}
}
