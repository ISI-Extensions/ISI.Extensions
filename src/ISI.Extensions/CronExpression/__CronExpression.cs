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

namespace ISI.Extensions
{
	public partial class CronExpression
	{
		public CronExpressionMinutes Minutes { get; }
		public CronExpressionHours Hours { get; }
		public CronExpressionDaysOfMonth DaysOfMonth { get; }
		public CronExpressionMonths Months { get; }
		public CronExpressionDaysOfWeek DaysOfWeek { get; }

		public CronExpression()
		{
			Minutes = new();
			Hours = new();
			DaysOfMonth = new();
			Months = new();
			DaysOfWeek = new();
		}

		public CronExpression(string source)
		{
			var parser = new ISI.Extensions.Parsers.DelimitedTextParser(' ');

			var parserContext = parser.CreateTextParserContext();

			var values = parser.Read(parserContext, source).Record;

			if (values.Length < 5)
			{
				Array.Resize(ref values, 5);
			}

			Minutes = new(values[0]);
			Hours = new(values[1]);
			DaysOfMonth = new(values[2]);
			Months = new(values[3]);
			DaysOfWeek = new(values[4]);
		}

		private CronExpression(
			CronExpressionMinutes minutes,
			CronExpressionHours hours,
			CronExpressionDaysOfMonth daysOfMonth,
			CronExpressionMonths months,
			CronExpressionDaysOfWeek daysOfWeek)
		{
			Minutes = minutes.Clone();
			Hours = hours.Clone();
			DaysOfMonth = daysOfMonth.Clone();
			Months = months.Clone();
			DaysOfWeek = daysOfWeek.Clone();
		}

		internal CronExpression Clone()
		{
			return new(Minutes, Hours, DaysOfMonth, Months, DaysOfWeek);
		}

		public CronExpression ShiftHours(int hours)
		{
			if (hours == 0)
			{
				return Clone();
			}

			var direction = (hours < 0 ? -1 : 1);
			hours *= direction;
			var days = (hours - (hours % 24)) / 24;

			return new(Minutes, Hours.ShiftHours(hours * direction), DaysOfMonth.ShiftDays(days * direction), Months.Clone(), DaysOfWeek.ShiftDays(days * direction));
		}

		public bool TryGetNextOccurrence(out DateTime nextOccurrence)
		{
			return TryGetNextOccurrence(DateTime.UtcNow, TimeSpan.MaxValue, out nextOccurrence);
		}

		public bool TryGetNextOccurrence(DateTime startDateTime, out DateTime nextOccurrence)
		{
			return TryGetNextOccurrence(startDateTime, TimeSpan.MaxValue, out nextOccurrence);
		}

		public bool TryGetNextOccurrence(DateTime startDateTime, TimeSpan timePeriod, out DateTime nextOccurrence)
		{
			var foundNextOccurrence = false;

			nextOccurrence = startDateTime;

			while (!foundNextOccurrence && (nextOccurrence < DateTime.MaxValue) && (nextOccurrence - startDateTime < timePeriod))
			{
				try
				{
					if (!Months.Any && !Months.MonthFlags[nextOccurrence.Month])
					{
						nextOccurrence = nextOccurrence.AddMonths(1);
						nextOccurrence = new(nextOccurrence.Year, nextOccurrence.Month, 1, 0, 0, 0);
					}
					else if (!DaysOfMonth.Any && !DaysOfMonth.DaysOfMonthFlags[nextOccurrence.Day])
					{
						nextOccurrence = nextOccurrence.AddDays(1);
						nextOccurrence = new(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day, 0, 0, 0);
					}
					else if (!DaysOfWeek.Any && !DaysOfWeek.DaysOfWeekFlags[(int)nextOccurrence.DayOfWeek])
					{
						nextOccurrence = nextOccurrence.AddDays(1);
						nextOccurrence = new(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day, 0, 0, 0);
					}
					else if (!Hours.Any && !Hours.HourFlags[nextOccurrence.Hour])
					{
						nextOccurrence = nextOccurrence.AddHours(1);
						nextOccurrence = new(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day, nextOccurrence.Hour, 0, 0);
					}
					else if (!Minutes.Any && !Minutes.MinuteFlags[nextOccurrence.Minute])
					{
						nextOccurrence = nextOccurrence.AddMinutes(1);
						nextOccurrence = new(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day, nextOccurrence.Hour, nextOccurrence.Minute, 0);
					}
					else
					{
						foundNextOccurrence = true;
					}
				}
				catch
				{
					nextOccurrence = DateTime.MaxValue;
				}
			}

			return foundNextOccurrence;
		}

		public override string ToString()
		{
			return Formatted();
		}

		public string Formatted()
		{
			var values = new[]
			{
				Minutes.ToString(),
				Hours.ToString(),
				DaysOfMonth.ToString(),
				Months.ToString(),
				DaysOfWeek.ToString(),
			};

			return string.Join(" ", values);
		}
	}
}
