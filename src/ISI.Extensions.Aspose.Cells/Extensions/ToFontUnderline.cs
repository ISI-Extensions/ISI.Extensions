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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.FontUnderlineType ToFontUnderline(this ISI.Extensions.SpreadSheets.FontUnderline fontUnderline)
		{
			switch (fontUnderline)
			{
				case ISI.Extensions.SpreadSheets.FontUnderline.Single: return global::Aspose.Cells.FontUnderlineType.Single; 
				case ISI.Extensions.SpreadSheets.FontUnderline.Double: return global::Aspose.Cells.FontUnderlineType.Double; 
				case ISI.Extensions.SpreadSheets.FontUnderline.Accounting: return global::Aspose.Cells.FontUnderlineType.Accounting;
				case ISI.Extensions.SpreadSheets.FontUnderline.DoubleAccounting: return global::Aspose.Cells.FontUnderlineType.DoubleAccounting;
				case ISI.Extensions.SpreadSheets.FontUnderline.Dash: return global::Aspose.Cells.FontUnderlineType.Dash;
				case ISI.Extensions.SpreadSheets.FontUnderline.DashDotDotHeavy: return global::Aspose.Cells.FontUnderlineType.DashDotDotHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.DashDotHeavy: return global::Aspose.Cells.FontUnderlineType.DashDotHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.DashedHeavy: return global::Aspose.Cells.FontUnderlineType.DashedHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.DashLong: return global::Aspose.Cells.FontUnderlineType.DashLong;
				case ISI.Extensions.SpreadSheets.FontUnderline.DashLongHeavy: return global::Aspose.Cells.FontUnderlineType.DashLongHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.DotDash: return global::Aspose.Cells.FontUnderlineType.DotDash;
				case ISI.Extensions.SpreadSheets.FontUnderline.DotDotDash: return global::Aspose.Cells.FontUnderlineType.DotDotDash;
				case ISI.Extensions.SpreadSheets.FontUnderline.Dotted: return global::Aspose.Cells.FontUnderlineType.Dotted;
				case ISI.Extensions.SpreadSheets.FontUnderline.DottedHeavy: return global::Aspose.Cells.FontUnderlineType.DottedHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.Heavy: return global::Aspose.Cells.FontUnderlineType.Heavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.Wave: return global::Aspose.Cells.FontUnderlineType.Wave;
				case ISI.Extensions.SpreadSheets.FontUnderline.WavyDouble: return global::Aspose.Cells.FontUnderlineType.WavyDouble;
				case ISI.Extensions.SpreadSheets.FontUnderline.WavyHeavy: return global::Aspose.Cells.FontUnderlineType.WavyHeavy;
				case ISI.Extensions.SpreadSheets.FontUnderline.Words: return global::Aspose.Cells.FontUnderlineType.Words;
			}

			return global::Aspose.Cells.FontUnderlineType.None;
		}

		public static ISI.Extensions.SpreadSheets.FontUnderline ToFontUnderline(this global::Aspose.Cells.FontUnderlineType fontUnderline)
		{
			switch (fontUnderline)
			{
				case global::Aspose.Cells.FontUnderlineType.Single: return ISI.Extensions.SpreadSheets.FontUnderline.Single; 
				case global::Aspose.Cells.FontUnderlineType.Double: return ISI.Extensions.SpreadSheets.FontUnderline.Double; 
				case global::Aspose.Cells.FontUnderlineType.Accounting: return ISI.Extensions.SpreadSheets.FontUnderline.Accounting; 
				case global::Aspose.Cells.FontUnderlineType.DoubleAccounting: return ISI.Extensions.SpreadSheets.FontUnderline.DoubleAccounting; 
				case global::Aspose.Cells.FontUnderlineType.Dash: return ISI.Extensions.SpreadSheets.FontUnderline.Dash; 
				case global::Aspose.Cells.FontUnderlineType.DashDotDotHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.DashDotDotHeavy; 
				case global::Aspose.Cells.FontUnderlineType.DashDotHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.DashDotHeavy; 
				case global::Aspose.Cells.FontUnderlineType.DashedHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.DashedHeavy; 
				case global::Aspose.Cells.FontUnderlineType.DashLong: return ISI.Extensions.SpreadSheets.FontUnderline.DashLong; 
				case global::Aspose.Cells.FontUnderlineType.DashLongHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.DashLongHeavy; 
				case global::Aspose.Cells.FontUnderlineType.DotDash: return ISI.Extensions.SpreadSheets.FontUnderline.DotDash; 
				case global::Aspose.Cells.FontUnderlineType.DotDotDash: return ISI.Extensions.SpreadSheets.FontUnderline.DotDotDash; 
				case global::Aspose.Cells.FontUnderlineType.Dotted: return ISI.Extensions.SpreadSheets.FontUnderline.Dotted; 
				case global::Aspose.Cells.FontUnderlineType.DottedHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.DottedHeavy; 
				case global::Aspose.Cells.FontUnderlineType.Heavy: return ISI.Extensions.SpreadSheets.FontUnderline.Heavy; 
				case global::Aspose.Cells.FontUnderlineType.Wave: return ISI.Extensions.SpreadSheets.FontUnderline.Wave; 
				case global::Aspose.Cells.FontUnderlineType.WavyDouble: return ISI.Extensions.SpreadSheets.FontUnderline.WavyDouble; 
				case global::Aspose.Cells.FontUnderlineType.WavyHeavy: return ISI.Extensions.SpreadSheets.FontUnderline.WavyHeavy; 
				case global::Aspose.Cells.FontUnderlineType.Words: return ISI.Extensions.SpreadSheets.FontUnderline.Words; 
			}

			return ISI.Extensions.SpreadSheets.FontUnderline.None;
		}
	}
}
