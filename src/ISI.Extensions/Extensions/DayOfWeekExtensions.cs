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
using System.Text;

namespace ISI.Extensions.Extensions
{
	public static class DayOfWeekExtensions
	{
		private static IDictionary<string, DayOfWeek> DayOfWeekLookup = new Dictionary<string, DayOfWeek>(StringComparer.InvariantCultureIgnoreCase)
	{
		{ "Sunday", DayOfWeek.Sunday },
		{ "Sun", DayOfWeek.Sunday },
		{ "Su", DayOfWeek.Sunday },
		{ "Monday", DayOfWeek.Monday },
		{ "Mon", DayOfWeek.Monday },
		{ "Mo", DayOfWeek.Monday },
		{ "M", DayOfWeek.Monday },
		{ "Tuesday", DayOfWeek.Tuesday },
		{ "Tues", DayOfWeek.Tuesday },
		{ "Tue", DayOfWeek.Tuesday },
		{ "Tu", DayOfWeek.Tuesday },
		{ "Wednesday", DayOfWeek.Wednesday },
		{ "Wednes", DayOfWeek.Wednesday },
		{ "Weds", DayOfWeek.Wednesday },
		{ "Wed", DayOfWeek.Wednesday },
		{ "W", DayOfWeek.Wednesday },
		{ "Thursday", DayOfWeek.Thursday },
		{ "Thurs", DayOfWeek.Thursday },
		{ "Thur", DayOfWeek.Thursday },
		{ "Thu", DayOfWeek.Thursday },
		{ "Thr", DayOfWeek.Thursday },
		{ "Th", DayOfWeek.Thursday },
		{ "Friday", DayOfWeek.Friday },
		{ "Fri", DayOfWeek.Friday },
		{ "Fr", DayOfWeek.Friday },
		{ "F", DayOfWeek.Friday },
		{ "Saturday", DayOfWeek.Saturday },
		{ "Satur", DayOfWeek.Saturday },
		{ "Sat", DayOfWeek.Saturday },
		{ "Sa", DayOfWeek.Saturday },
	};

		public static DayOfWeek? ToDayOfWeekNullable(this string value)
		{
			if (DayOfWeekLookup.TryGetValue(value, out var dayOfWeek))
			{
				return dayOfWeek;
			}

			return null;
		}
	}
}
