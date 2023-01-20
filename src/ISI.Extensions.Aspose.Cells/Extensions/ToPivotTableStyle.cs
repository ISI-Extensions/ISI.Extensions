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
using System.Threading.Tasks;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.Pivot.PivotTableStyleType? ToPivotTableStyle(this ISI.Extensions.SpreadSheets.PivotTableStyle? tableStyle)
		{
			return (tableStyle.HasValue ? ToPivotTableStyle(tableStyle.GetValueOrDefault()) : (global::Aspose.Cells.Pivot.PivotTableStyleType?) null);
		}

		public static global::Aspose.Cells.Pivot.PivotTableStyleType ToPivotTableStyle(this ISI.Extensions.SpreadSheets.PivotTableStyle tableStyle)
		{
			switch (tableStyle)
			{
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light1: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight1;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light2: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight2;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light3: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight3;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light4: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight4;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light5: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight5;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light6: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight6;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light7: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight7;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light8: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight8;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light9: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight9;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light10: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight10;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light11: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight11;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light12: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight12;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light13: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight13;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light14: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight14;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light15: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight15;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light16: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight16;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light17: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight17;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light18: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight18;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light19: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight19;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light20: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight20;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light21: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight21;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light22: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight22;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light23: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight23;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light24: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight24;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light25: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight25;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light26: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight26;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light27: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight27;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Light28: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight28;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium1: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium1;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium2: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium2;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium3: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium3;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium4: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium4;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium5: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium5;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium6: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium6;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium7: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium7;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium8: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium8;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium9: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium9;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium10: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium10;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium11: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium11;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium12: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium12;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium13: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium13;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium14: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium14;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium15: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium15;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium16: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium16;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium17: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium17;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium18: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium18;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium19: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium19;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium20: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium20;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium21: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium21;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium22: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium22;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium23: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium23;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium24: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium24;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium25: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium25;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium26: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium26;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium27: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium27;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Medium28: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium28;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark1: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark1;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark2: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark2;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark3: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark3;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark4: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark4;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark5: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark5;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark6: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark6;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark7: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark7;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark8: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark8;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark9: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark9;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark10: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark10;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark11: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark11;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark12: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark12;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark13: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark13;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark14: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark14;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark15: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark15;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark16: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark16;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark17: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark17;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark18: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark18;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark19: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark19;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark20: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark20;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark21: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark21;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark22: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark22;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark23: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark23;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark24: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark24;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark25: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark25;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark26: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark26;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark27: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark27;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Dark28: return global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark28;
				case ISI.Extensions.SpreadSheets.PivotTableStyle.Custom: return global::Aspose.Cells.Pivot.PivotTableStyleType.Custom;

			}

			return global::Aspose.Cells.Pivot.PivotTableStyleType.None;
		}

		public static ISI.Extensions.SpreadSheets.PivotTableStyle? ToPivotTableStyle(this global::Aspose.Cells.Pivot.PivotTableStyleType? tableStyle)
		{
			return (tableStyle.HasValue ? ToPivotTableStyle(tableStyle.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.PivotTableStyle?)null);
		}

		public static ISI.Extensions.SpreadSheets.PivotTableStyle ToPivotTableStyle(this global::Aspose.Cells.Pivot.PivotTableStyleType tableStyle)
		{
			switch (tableStyle)
			{
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight1: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light1;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight2: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light2;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight3: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light3;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight4: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light4;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight5: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light5;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight6: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light6;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight7: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light7;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight8: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light8;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight9: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light9;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight10: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light10;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight11: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light11;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight12: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light12;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight13: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light13;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight14: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light14;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight15: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light15;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight16: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light16;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight17: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light17;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight18: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light18;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight19: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light19;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight20: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light20;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight21: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light21;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight22: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light22;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight23: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light23;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight24: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light24;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight25: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light25;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight26: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light26;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight27: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light27;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleLight28: return ISI.Extensions.SpreadSheets.PivotTableStyle.Light28;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium1: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium1;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium2: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium2;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium3: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium3;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium4: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium4;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium5: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium5;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium6: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium6;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium7: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium7;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium8: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium8;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium9: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium9;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium10: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium10;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium11: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium11;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium12: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium12;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium13: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium13;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium14: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium14;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium15: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium15;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium16: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium16;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium17: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium17;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium18: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium18;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium19: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium19;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium20: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium20;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium21: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium21;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium22: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium22;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium23: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium23;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium24: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium24;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium25: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium25;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium26: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium26;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium27: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium27;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleMedium28: return ISI.Extensions.SpreadSheets.PivotTableStyle.Medium28;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark1: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark1;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark2: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark2;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark3: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark3;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark4: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark4;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark5: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark5;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark6: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark6;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark7: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark7;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark8: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark8;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark9: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark9;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark10: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark10;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark11: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark11;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark12: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark12;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark13: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark13;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark14: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark14;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark15: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark15;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark16: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark16;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark17: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark17;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark18: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark18;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark19: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark19;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark20: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark20;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark21: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark21;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark22: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark22;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark23: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark23;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark24: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark24;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark25: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark25;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark26: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark26;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark27: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark27;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.PivotTableStyleDark28: return ISI.Extensions.SpreadSheets.PivotTableStyle.Dark28;
				case global::Aspose.Cells.Pivot.PivotTableStyleType.Custom: return ISI.Extensions.SpreadSheets.PivotTableStyle.Custom;
			}

			return ISI.Extensions.SpreadSheets.PivotTableStyle.None;
		}
	}
}
