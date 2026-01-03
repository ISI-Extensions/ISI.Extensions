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
using System.Threading.Tasks;

namespace ISI.Extensions.SpreadSheets.Extensions
{
	public static class SpreadSheetHelperExtensions
	{
		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, string fileName, ISI.Extensions.TextParserFactory.TextDelimiter textDelimiter)
		{
			switch (textDelimiter)
			{
				case TextParserFactory.TextDelimiter.CommaSeparatedValues:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case TextParserFactory.TextDelimiter.PipeDelimitedValues:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.Auto,
						Separator = '|',
						SeparatorString = "\"",
						CheckExcelRestriction = false,
					});

				case TextParserFactory.TextDelimiter.TabDelimitedValues:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});
				
				default:
					throw new ArgumentOutOfRangeException(nameof(textDelimiter), textDelimiter, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, System.IO.Stream stream, ISI.Extensions.TextParserFactory.TextDelimiter textDelimiter)
		{
			switch (textDelimiter)
			{
				case TextParserFactory.TextDelimiter.CommaSeparatedValues:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case TextParserFactory.TextDelimiter.PipeDelimitedValues:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.Auto,
						Separator = '|',
						SeparatorString = "\"",
						CheckExcelRestriction = false,
					});

				case TextParserFactory.TextDelimiter.TabDelimitedValues:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});
				
				default:
					throw new ArgumentOutOfRangeException(nameof(textDelimiter), textDelimiter, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, string fileName, ISI.Extensions.SpreadSheets.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case FileFormat.Default:
				case FileFormat.Xls:
				case FileFormat.Xlsx:
					return spreadSheetHelper.Open(fileName);

				case FileFormat.CommaSeparatedValues:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case FileFormat.TabDelimited:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});

				//case FileFormat.Pdf:
				//case FileFormat.Html:
				//	break;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, System.IO.Stream stream, ISI.Extensions.SpreadSheets.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case FileFormat.Default:
				case FileFormat.Xls:
				case FileFormat.Xlsx:
					return spreadSheetHelper.Open(stream);

				case FileFormat.CommaSeparatedValues:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case FileFormat.TabDelimited:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});

				//case FileFormat.Pdf:
				//case FileFormat.Html:
				//	break;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, string fileName, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.Documents.FileFormat.CommaSeparatedValues:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case ISI.Extensions.Documents.FileFormat.PipeDelimited:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.Auto,
						Separator = '|',
						SeparatorString = "\"",
						CheckExcelRestriction = false,
					});

				case ISI.Extensions.Documents.FileFormat.TabDelimited:
					return spreadSheetHelper.Open(fileName, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});

				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.IWorkbook Open(this ISpreadSheetHelper spreadSheetHelper, System.IO.Stream stream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.Documents.FileFormat.CommaSeparatedValues:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.CommaSeparatedValues,
						CheckExcelRestriction = false,
					});

				case ISI.Extensions.Documents.FileFormat.PipeDelimited:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.Auto,
						Separator = '|',
						SeparatorString = "\"",
						CheckExcelRestriction = false,
					});

				case ISI.Extensions.Documents.FileFormat.TabDelimited:
					return spreadSheetHelper.Open(stream, new TextLoadOptions()
					{
						LoadFormat = LoadFormat.TabDelimited,
						CheckExcelRestriction = false,
					});

				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
			}
		}
	}
}
