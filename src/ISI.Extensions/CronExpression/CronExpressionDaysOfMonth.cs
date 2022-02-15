#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class CronExpression
	{
		public class CronExpressionDaysOfMonth : CronExpressionElement
		{
			private const int maxDayOfMonth = 32;

			public bool[] DaysOfMonthFlags { get; } = new bool[maxDayOfMonth];

			public CronExpressionDaysOfMonth()
			{

			}

			public CronExpressionDaysOfMonth(string source)
			{
				Action throwError = () => throw new System.Exception(string.Format("DaysOfMonth: Cannot parse \"{0}\"", source));

				var parser = new ISI.Extensions.Parsers.DelimitedTextParser(',');

				var parserContext = parser.CreateTextParserContext();

				var values = parser.Read(parserContext, source.Replace("-", ",-,")).Record.Select(value => value.Trim());

				var lastDayOfMonthIndex = -1;
				var inRange = false;
				foreach (var value in values)
				{
					var dayOfMonthIndex = value.ToInt(-1);

					if (inRange)
					{
						if ((dayOfMonthIndex < 0) || (dayOfMonthIndex < lastDayOfMonthIndex) || (dayOfMonthIndex >= maxDayOfMonth))
						{
							throwError();
						}

						for (var index = lastDayOfMonthIndex; index < dayOfMonthIndex; index++)
						{
							DaysOfMonthFlags[index] = true;
						}

						lastDayOfMonthIndex = -1;
						inRange = false;
					}
					else if (string.Equals(value, "*"))
					{
						Any = true;
					}
					else if (string.Equals(value, "-"))
					{
						if (lastDayOfMonthIndex < 0)
						{
							throwError();
						}

						inRange = true;
					}
					else if ((dayOfMonthIndex < 0) || (dayOfMonthIndex >= maxDayOfMonth))
					{
						throwError();
					}
					else
					{
						DaysOfMonthFlags[dayOfMonthIndex] = true;
						lastDayOfMonthIndex = dayOfMonthIndex;
					}
				}
			}

			private CronExpressionDaysOfMonth(bool[] daysOfMonthFlags)
			{
				for (var dayOfMonthIndex = 0; dayOfMonthIndex < maxDayOfMonth; dayOfMonthIndex++)
				{
					DaysOfMonthFlags[dayOfMonthIndex] = daysOfMonthFlags[dayOfMonthIndex];
				}
			}

			internal CronExpressionDaysOfMonth Clone()
			{
				return new CronExpressionDaysOfMonth(DaysOfMonthFlags);
			}

			internal CronExpressionDaysOfMonth ShiftDays(int days)
			{
				var daysOfMonthFlags = new bool[maxDayOfMonth];

				for (var dayOfMonthIndex = 0; dayOfMonthIndex < maxDayOfMonth; dayOfMonthIndex++)
				{
					daysOfMonthFlags[(dayOfMonthIndex + maxDayOfMonth + days) % maxDayOfMonth] = DaysOfMonthFlags[dayOfMonthIndex];
				}

				return new CronExpressionDaysOfMonth(daysOfMonthFlags);
			}

			public override string ToString()
			{
				return Formatted();
			}

			public string Formatted()
			{
				var ranges = new List<string>();

				var dayOfMonthIndex = 0;
				while (dayOfMonthIndex < maxDayOfMonth)
				{
					if (DaysOfMonthFlags[dayOfMonthIndex])
					{
						var dayOfMonthEndRangeIndex = dayOfMonthIndex + 1;
						if ((dayOfMonthEndRangeIndex < maxDayOfMonth) && DaysOfMonthFlags[dayOfMonthEndRangeIndex++])
						{
							while ((dayOfMonthEndRangeIndex < maxDayOfMonth) && DaysOfMonthFlags[dayOfMonthEndRangeIndex])
							{
								dayOfMonthEndRangeIndex++;
							}

							ranges.Add(string.Format("{0}-{1}", dayOfMonthIndex, (dayOfMonthEndRangeIndex >= maxDayOfMonth ? maxDayOfMonth - 1 : dayOfMonthEndRangeIndex)));

							dayOfMonthIndex = dayOfMonthEndRangeIndex - 1;
						}
						else
						{
							ranges.Add(string.Format("{0}", dayOfMonthIndex));
						}
					}

					dayOfMonthIndex++;
				}

				return Formatted(ranges);
			}
		}
	}
}
