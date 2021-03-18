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
using System.Threading.Tasks;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.LoadFormat ToLoadFormat(this ISI.Extensions.SpreadSheets.LoadFormat loadFormat)
		{
			switch (loadFormat)
			{
				case ISI.Extensions.SpreadSheets.LoadFormat.Auto: return global::Aspose.Cells.LoadFormat.Auto;
				case ISI.Extensions.SpreadSheets.LoadFormat.CommaSeparatedValues: return global::Aspose.Cells.LoadFormat.CSV;
				case ISI.Extensions.SpreadSheets.LoadFormat.Excel97To2003: return global::Aspose.Cells.LoadFormat.Excel97To2003;
				case ISI.Extensions.SpreadSheets.LoadFormat.Xlsx: return global::Aspose.Cells.LoadFormat.Xlsx;
				case ISI.Extensions.SpreadSheets.LoadFormat.TabDelimited: return global::Aspose.Cells.LoadFormat.TabDelimited;
				case ISI.Extensions.SpreadSheets.LoadFormat.Html: return global::Aspose.Cells.LoadFormat.Html;
				case ISI.Extensions.SpreadSheets.LoadFormat.MHtml: return global::Aspose.Cells.LoadFormat.MHtml;
				case ISI.Extensions.SpreadSheets.LoadFormat.SpreadsheetML: return global::Aspose.Cells.LoadFormat.SpreadsheetML;
				case ISI.Extensions.SpreadSheets.LoadFormat.Xlsb: return global::Aspose.Cells.LoadFormat.Xlsb;
			}

			return global::Aspose.Cells.LoadFormat.Unknown;
		}

		public static ISI.Extensions.SpreadSheets.LoadFormat ToLoadFormat(this global::Aspose.Cells.LoadFormat gradientStyle)
		{
			switch (gradientStyle)
			{
				case global::Aspose.Cells.LoadFormat.Auto: return ISI.Extensions.SpreadSheets.LoadFormat.Auto;
				case global::Aspose.Cells.LoadFormat.CSV: return ISI.Extensions.SpreadSheets.LoadFormat.CommaSeparatedValues;
				case global::Aspose.Cells.LoadFormat.Excel97To2003: return ISI.Extensions.SpreadSheets.LoadFormat.Excel97To2003;
				case global::Aspose.Cells.LoadFormat.Xlsx: return ISI.Extensions.SpreadSheets.LoadFormat.Xlsx;
				case global::Aspose.Cells.LoadFormat.TabDelimited: return ISI.Extensions.SpreadSheets.LoadFormat.TabDelimited;
				case global::Aspose.Cells.LoadFormat.Html: return ISI.Extensions.SpreadSheets.LoadFormat.Html;
				case global::Aspose.Cells.LoadFormat.MHtml: return ISI.Extensions.SpreadSheets.LoadFormat.MHtml;
				case global::Aspose.Cells.LoadFormat.SpreadsheetML: return ISI.Extensions.SpreadSheets.LoadFormat.SpreadsheetML;
				case global::Aspose.Cells.LoadFormat.Xlsb: return ISI.Extensions.SpreadSheets.LoadFormat.Xlsb;
			}

			return ISI.Extensions.SpreadSheets.LoadFormat.Auto;
		}
	}
}
