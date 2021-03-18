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
		public static global::Aspose.Cells.Tables.TableStyleType? ToTableStyle(this ISI.Extensions.SpreadSheets.TableStyle? tableStyle)
		{
			return (tableStyle.HasValue ? ToTableStyle(tableStyle.GetValueOrDefault()) : (global::Aspose.Cells.Tables.TableStyleType?) null);
		}

		public static global::Aspose.Cells.Tables.TableStyleType ToTableStyle(this ISI.Extensions.SpreadSheets.TableStyle tableStyle)
		{
			switch (tableStyle)
			{
				case ISI.Extensions.SpreadSheets.TableStyle.Light1: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight1;
				case ISI.Extensions.SpreadSheets.TableStyle.Light2: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight2;
				case ISI.Extensions.SpreadSheets.TableStyle.Light3: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight3;
				case ISI.Extensions.SpreadSheets.TableStyle.Light4: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight4;
				case ISI.Extensions.SpreadSheets.TableStyle.Light5: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight5;
				case ISI.Extensions.SpreadSheets.TableStyle.Light6: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight6;
				case ISI.Extensions.SpreadSheets.TableStyle.Light7: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight7;
				case ISI.Extensions.SpreadSheets.TableStyle.Light8: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight8;
				case ISI.Extensions.SpreadSheets.TableStyle.Light9: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight9;
				case ISI.Extensions.SpreadSheets.TableStyle.Light10: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight10;
				case ISI.Extensions.SpreadSheets.TableStyle.Light11: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight11;
				case ISI.Extensions.SpreadSheets.TableStyle.Light12: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight12;
				case ISI.Extensions.SpreadSheets.TableStyle.Light13: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight13;
				case ISI.Extensions.SpreadSheets.TableStyle.Light14: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight14;
				case ISI.Extensions.SpreadSheets.TableStyle.Light15: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight15;
				case ISI.Extensions.SpreadSheets.TableStyle.Light16: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight16;
				case ISI.Extensions.SpreadSheets.TableStyle.Light17: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight17;
				case ISI.Extensions.SpreadSheets.TableStyle.Light18: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight18;
				case ISI.Extensions.SpreadSheets.TableStyle.Light19: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight19;
				case ISI.Extensions.SpreadSheets.TableStyle.Light20: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight20;
				case ISI.Extensions.SpreadSheets.TableStyle.Light21: return global::Aspose.Cells.Tables.TableStyleType.TableStyleLight21;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium1: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium1;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium2: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium2;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium3: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium3;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium4: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium4;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium5: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium5;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium6: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium6;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium7: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium7;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium8: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium8;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium9: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium9;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium10: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium10;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium11: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium11;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium12: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium12;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium13: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium13;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium14: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium14;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium15: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium15;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium16: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium16;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium17: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium17;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium18: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium18;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium19: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium19;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium20: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium20;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium21: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium21;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium22: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium22;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium23: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium23;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium24: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium24;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium25: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium25;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium26: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium26;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium27: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium27;
				case ISI.Extensions.SpreadSheets.TableStyle.Medium28: return global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium28;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark1: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark1;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark2: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark2;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark3: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark3;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark4: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark4;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark5: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark5;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark6: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark6;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark7: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark7;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark8: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark8;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark9: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark9;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark10: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark10;
				case ISI.Extensions.SpreadSheets.TableStyle.Dark11: return global::Aspose.Cells.Tables.TableStyleType.TableStyleDark11;
			}

			return global::Aspose.Cells.Tables.TableStyleType.None;
		}

		public static ISI.Extensions.SpreadSheets.TableStyle? ToTableStyle(this global::Aspose.Cells.Tables.TableStyleType? tableStyle)
		{
			return (tableStyle.HasValue ? ToTableStyle(tableStyle.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.TableStyle?)null);
		}

		public static ISI.Extensions.SpreadSheets.TableStyle ToTableStyle(this global::Aspose.Cells.Tables.TableStyleType tableStyle)
		{
			switch (tableStyle)
			{
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight1: return ISI.Extensions.SpreadSheets.TableStyle.Light1;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight2: return ISI.Extensions.SpreadSheets.TableStyle.Light2;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight3: return ISI.Extensions.SpreadSheets.TableStyle.Light3;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight4: return ISI.Extensions.SpreadSheets.TableStyle.Light4;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight5: return ISI.Extensions.SpreadSheets.TableStyle.Light5;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight6: return ISI.Extensions.SpreadSheets.TableStyle.Light6;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight7: return ISI.Extensions.SpreadSheets.TableStyle.Light7;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight8: return ISI.Extensions.SpreadSheets.TableStyle.Light8;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight9: return ISI.Extensions.SpreadSheets.TableStyle.Light9;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight10: return ISI.Extensions.SpreadSheets.TableStyle.Light10;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight11: return ISI.Extensions.SpreadSheets.TableStyle.Light11;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight12: return ISI.Extensions.SpreadSheets.TableStyle.Light12;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight13: return ISI.Extensions.SpreadSheets.TableStyle.Light13;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight14: return ISI.Extensions.SpreadSheets.TableStyle.Light14;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight15: return ISI.Extensions.SpreadSheets.TableStyle.Light15;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight16: return ISI.Extensions.SpreadSheets.TableStyle.Light16;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight17: return ISI.Extensions.SpreadSheets.TableStyle.Light17;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight18: return ISI.Extensions.SpreadSheets.TableStyle.Light18;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight19: return ISI.Extensions.SpreadSheets.TableStyle.Light19;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight20: return ISI.Extensions.SpreadSheets.TableStyle.Light20;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleLight21: return ISI.Extensions.SpreadSheets.TableStyle.Light21;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium1: return ISI.Extensions.SpreadSheets.TableStyle.Medium1;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium2: return ISI.Extensions.SpreadSheets.TableStyle.Medium2;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium3: return ISI.Extensions.SpreadSheets.TableStyle.Medium3;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium4: return ISI.Extensions.SpreadSheets.TableStyle.Medium4;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium5: return ISI.Extensions.SpreadSheets.TableStyle.Medium5;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium6: return ISI.Extensions.SpreadSheets.TableStyle.Medium6;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium7: return ISI.Extensions.SpreadSheets.TableStyle.Medium7;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium8: return ISI.Extensions.SpreadSheets.TableStyle.Medium8;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium9: return ISI.Extensions.SpreadSheets.TableStyle.Medium9;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium10: return ISI.Extensions.SpreadSheets.TableStyle.Medium10;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium11: return ISI.Extensions.SpreadSheets.TableStyle.Medium11;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium12: return ISI.Extensions.SpreadSheets.TableStyle.Medium12;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium13: return ISI.Extensions.SpreadSheets.TableStyle.Medium13;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium14: return ISI.Extensions.SpreadSheets.TableStyle.Medium14;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium15: return ISI.Extensions.SpreadSheets.TableStyle.Medium15;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium16: return ISI.Extensions.SpreadSheets.TableStyle.Medium16;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium17: return ISI.Extensions.SpreadSheets.TableStyle.Medium17;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium18: return ISI.Extensions.SpreadSheets.TableStyle.Medium18;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium19: return ISI.Extensions.SpreadSheets.TableStyle.Medium19;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium20: return ISI.Extensions.SpreadSheets.TableStyle.Medium20;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium21: return ISI.Extensions.SpreadSheets.TableStyle.Medium21;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium22: return ISI.Extensions.SpreadSheets.TableStyle.Medium22;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium23: return ISI.Extensions.SpreadSheets.TableStyle.Medium23;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium24: return ISI.Extensions.SpreadSheets.TableStyle.Medium24;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium25: return ISI.Extensions.SpreadSheets.TableStyle.Medium25;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium26: return ISI.Extensions.SpreadSheets.TableStyle.Medium26;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium27: return ISI.Extensions.SpreadSheets.TableStyle.Medium27;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleMedium28: return ISI.Extensions.SpreadSheets.TableStyle.Medium28;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark1: return ISI.Extensions.SpreadSheets.TableStyle.Dark1;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark2: return ISI.Extensions.SpreadSheets.TableStyle.Dark2;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark3: return ISI.Extensions.SpreadSheets.TableStyle.Dark3;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark4: return ISI.Extensions.SpreadSheets.TableStyle.Dark4;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark5: return ISI.Extensions.SpreadSheets.TableStyle.Dark5;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark6: return ISI.Extensions.SpreadSheets.TableStyle.Dark6;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark7: return ISI.Extensions.SpreadSheets.TableStyle.Dark7;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark8: return ISI.Extensions.SpreadSheets.TableStyle.Dark8;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark9: return ISI.Extensions.SpreadSheets.TableStyle.Dark9;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark10: return ISI.Extensions.SpreadSheets.TableStyle.Dark10;
				case global::Aspose.Cells.Tables.TableStyleType.TableStyleDark11: return ISI.Extensions.SpreadSheets.TableStyle.Dark11;
			}

			return ISI.Extensions.SpreadSheets.TableStyle.None;
		}
	}
}
