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

namespace ISI.Extensions.SpreadSheets
{
	public enum NumberFormat
	{
		General, //General General
		Text, //@
		Integer, //Decimal 0
		Decimal, //Decimal 0.00
		FormattedInteger, //Decimal #,##0
		FormattedDecimal, //Decimal #,##0.00 
		FormattedCurrencyInteger, //Currency $#,##0;$-#,##0
		FormattedRedCurrencyInteger, //Currency $#,##0;[Red]$-#,##0
		FormattedCurrencyDecimal, //Currency $#,##0.00;$-#,##0.00
		FormattedRedCurrencyDecimal, //Currency $#,##0.00;[Red]$-#,##0.00

		PercentageInteger, //Percentage 0%
		PercentageDecimal, //Percentage 0.00%
		Scientific, //Scientific 0.00E+00
		//  12 Fraction # ?/?
		//  13 Fraction # /
		ShortDate, //Date m/d/yy
		DayMonthYear, //Date d-mmm-yy
		DayMonth, //Date d-mmm
		MonthYear, //Date mmm-yy 
		HourMinuteAMPM, //Time h:mm AM/PM
		HourMinuteSecondAMPM, //Time h:mm:ss AM/PM
		HourMinute, //Time h:mm
		HourMinuteSecond, //Time h:mm:ss
		MonthDayYearHourMinute, //Time m/d/yy h:mm

		FormattedMoneyInteger, //Currency #,##0;-#,##0
		FormattedRedMoneyInteger, //Currency $#,##0;[Red]$-#,##0
		FormattedMoneyDecimal, //Currency $#,##0.00;$-#,##0.00
		FormattedRedMoneyDecimal, //Currency $#,##0.00;[Red]$-#,##0.00

		FormattedAccountantCurrencyInteger, //Accounting _ * #,##0_ ;_ * "_ ;_ @_
		FormattedRedAccountantMoneyInteger, //Accounting _ $* #,##0_ ;_ $* "_ ;_ @_
		FormattedAccountantCurrencyDecimal, //Accounting _ * #,##0.00_ ;_ * "??_ ;_ @_
		FormattedRedAccountantMoneyDecimal, //Accounting _ $* #,##0.00_ ;_ $* "??_ ;_ @_ 
	}
}
