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
		private static IDictionary<ISI.Extensions.SpreadSheets.NumberFormat, int> _toNumber = null;
		private static IDictionary<int, ISI.Extensions.SpreadSheets.NumberFormat> _toNumberFormat = null;

		static CellsExtensions()
		{
			_toNumber = new Dictionary<ISI.Extensions.SpreadSheets.NumberFormat, int>()
			{
				{ ISI.Extensions.SpreadSheets.NumberFormat.General, 0},
				{ ISI.Extensions.SpreadSheets.NumberFormat.Text,  49},
				{ ISI.Extensions.SpreadSheets.NumberFormat.Integer,  1},
				{ ISI.Extensions.SpreadSheets.NumberFormat.Decimal,  2},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedInteger,  3},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedDecimal,  4},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedCurrencyInteger,  5},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedCurrencyInteger,  6},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedCurrencyDecimal,  7},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedCurrencyDecimal,  8},
				{ ISI.Extensions.SpreadSheets.NumberFormat.PercentageInteger,  9},
				{ ISI.Extensions.SpreadSheets.NumberFormat.PercentageDecimal,  10},
				{ ISI.Extensions.SpreadSheets.NumberFormat.Scientific,  11},
				{ ISI.Extensions.SpreadSheets.NumberFormat.ShortDate,  14},
				{ ISI.Extensions.SpreadSheets.NumberFormat.DayMonthYear,  15},
				{ ISI.Extensions.SpreadSheets.NumberFormat.DayMonth,  16},
				{ ISI.Extensions.SpreadSheets.NumberFormat.MonthYear,  17},
				{ ISI.Extensions.SpreadSheets.NumberFormat.HourMinuteAMPM,  18},
				{ ISI.Extensions.SpreadSheets.NumberFormat.HourMinuteSecondAMPM,  19},
				{ ISI.Extensions.SpreadSheets.NumberFormat.HourMinute,  20},
				{ ISI.Extensions.SpreadSheets.NumberFormat.HourMinuteSecond,  21},
				{ ISI.Extensions.SpreadSheets.NumberFormat.MonthDayYearHourMinute,  22},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedMoneyInteger,  37},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedMoneyInteger,  38},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedMoneyDecimal,  39},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedMoneyDecimal,  40},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedAccountantCurrencyInteger,  41},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedAccountantMoneyInteger,  42},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedAccountantCurrencyDecimal,  43},
				{ ISI.Extensions.SpreadSheets.NumberFormat.FormattedRedAccountantMoneyDecimal,  44},
			};

			_toNumberFormat = _toNumber.ToDictionary(_ => _.Value, _ => _.Key);
		}


		public static int? ToNumberFormat(this ISI.Extensions.SpreadSheets.NumberFormat? format)
		{
			return (format.HasValue ? ToNumberFormat(format.GetValueOrDefault()) : (int?) null);
		}

		public static int ToNumberFormat(this ISI.Extensions.SpreadSheets.NumberFormat format)
		{
			return _toNumber[format];
		}

		public static ISI.Extensions.SpreadSheets.NumberFormat ToNumberFormat(int format)
		{
			return _toNumberFormat[format];
		}
	}
}
