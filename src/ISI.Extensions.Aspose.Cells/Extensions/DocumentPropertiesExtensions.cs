#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Aspose.Extensions
{
	public static class DocumentPropertiesExtensions
	{
		public static void SetDocumentProperties(this global::Aspose.Cells.Workbook workbook, ISI.Extensions.Documents.IDocumentProperties documentProperties)
		{
			if (documentProperties != null)
			{
				if (!(documentProperties is ISI.Extensions.Documents.ISpreadSheetDocumentProperties spreadSheetDocumentProperties))
				{
					workbook.BuiltInDocumentProperties.Subject = documentProperties.Subject ?? string.Empty;
					workbook.BuiltInDocumentProperties.Author = documentProperties.Author ?? string.Empty;
					workbook.BuiltInDocumentProperties.Keywords = documentProperties.Keywords ?? string.Empty;
					workbook.BuiltInDocumentProperties.Title = documentProperties.Title ?? string.Empty;
				}
				else
				{
					workbook.BuiltInDocumentProperties.Subject = spreadSheetDocumentProperties.Subject ?? string.Empty;
					workbook.BuiltInDocumentProperties.Author = spreadSheetDocumentProperties.Author ?? string.Empty;
					workbook.BuiltInDocumentProperties.Revision = spreadSheetDocumentProperties.Revision ?? string.Empty;
					workbook.BuiltInDocumentProperties.Keywords = spreadSheetDocumentProperties.Keywords ?? string.Empty;
					workbook.BuiltInDocumentProperties.Title = spreadSheetDocumentProperties.Title ?? string.Empty;
					workbook.BuiltInDocumentProperties.Comments = spreadSheetDocumentProperties.Comments ?? string.Empty;
					workbook.BuiltInDocumentProperties.Template = spreadSheetDocumentProperties.Template ?? string.Empty;
					workbook.BuiltInDocumentProperties.NameOfApplication = spreadSheetDocumentProperties.ApplicationName ?? string.Empty;
					workbook.BuiltInDocumentProperties.Category = spreadSheetDocumentProperties.Category ?? string.Empty;
					workbook.BuiltInDocumentProperties.Manager = spreadSheetDocumentProperties.Manager ?? string.Empty;
					workbook.BuiltInDocumentProperties.Company = spreadSheetDocumentProperties.Company ?? string.Empty;
				}
			}
		}

		public static ISI.Extensions.Documents.IDocumentProperties GetDocumentProperties(this global::Aspose.Cells.Workbook workbook)
		{
			return new ISI.Extensions.SpreadSheets.DocumentProperties()
			{
				Subject = workbook.BuiltInDocumentProperties.Subject ?? string.Empty,
				Author = workbook.BuiltInDocumentProperties.Author ?? string.Empty,
				Revision = workbook.BuiltInDocumentProperties.Revision ?? string.Empty,
				Keywords = workbook.BuiltInDocumentProperties.Keywords ?? string.Empty,
				Title = workbook.BuiltInDocumentProperties.Title ?? string.Empty,
				Comments = workbook.BuiltInDocumentProperties.Comments ?? string.Empty,
				Template = workbook.BuiltInDocumentProperties.Template ?? string.Empty,
				ApplicationName = workbook.BuiltInDocumentProperties.NameOfApplication ?? string.Empty,
				Category = workbook.BuiltInDocumentProperties.Category ?? string.Empty,
				Manager = workbook.BuiltInDocumentProperties.Manager ?? string.Empty,
				Company = workbook.BuiltInDocumentProperties.Company ?? string.Empty
			};
		}
	}
}
