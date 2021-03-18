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
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.SaveFormat ToSaveFormat(this ISI.Extensions.SpreadSheets.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.SpreadSheets.FileFormat.Default: return global::Aspose.Cells.SaveFormat.Xlsx;
				case ISI.Extensions.SpreadSheets.FileFormat.CommaSeparatedValues: return global::Aspose.Cells.SaveFormat.CSV;
				case ISI.Extensions.SpreadSheets.FileFormat.TabDelimited: return global::Aspose.Cells.SaveFormat.TabDelimited;
				case ISI.Extensions.SpreadSheets.FileFormat.Pdf: return global::Aspose.Cells.SaveFormat.Pdf;
				case ISI.Extensions.SpreadSheets.FileFormat.Html: return global::Aspose.Cells.SaveFormat.Html;
				case ISI.Extensions.SpreadSheets.FileFormat.Xls: return global::Aspose.Cells.SaveFormat.Excel97To2003;
				case ISI.Extensions.SpreadSheets.FileFormat.Xlsx: return global::Aspose.Cells.SaveFormat.Xlsx;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}
		}

		public static global::Aspose.Cells.SaveFormat ToSaveFormat(this ISI.Extensions.Documents.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.Documents.FileFormat.Default: return global::Aspose.Cells.SaveFormat.Xlsx;
				case ISI.Extensions.Documents.FileFormat.CommaSeparatedValues: return global::Aspose.Cells.SaveFormat.CSV;
				case ISI.Extensions.Documents.FileFormat.TabDelimited: return global::Aspose.Cells.SaveFormat.TabDelimited;
				case ISI.Extensions.Documents.FileFormat.Xlsx: return global::Aspose.Cells.SaveFormat.Xlsx;
				case ISI.Extensions.Documents.FileFormat.Xls: return global::Aspose.Cells.SaveFormat.Excel97To2003;
				case ISI.Extensions.Documents.FileFormat.Pdf: return global::Aspose.Cells.SaveFormat.Pdf;
				case ISI.Extensions.Documents.FileFormat.Html: return global::Aspose.Cells.SaveFormat.Html;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat));
			}
		}
	}
}