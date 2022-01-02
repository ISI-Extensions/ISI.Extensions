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
		public class CronExpressionDaysOfWeek : CronExpressionElement
		{
			private const int maxDayOfWeek = 7;

			public bool[] DaysOfWeekFlags { get; } = new bool[maxDayOfWeek];

			public CronExpressionDaysOfWeek()
			{

			}

			public CronExpressionDaysOfWeek(string source)
			{
				Action throwError = () => throw new System.Exception(string.Format("DaysOfWeek: Cannot parse \"{0}\"", source));

				var parser = new ISI.Extensions.Parsers.DelimitedTextParser(',');

				var values = parser.Read(source.Replace("-", ",-,")).Record.Select(value => value.Trim());

				var lastDayOfWeekIndex = -1;
				var inRange = false;
				foreach (var value in values)
				{
					var dayOfWeekIndex = value.ToInt(-1);

					if (dayOfWeekIndex < 0)
					{
						var dow = value.ToDayOfWeekNullable();

						if (dow.HasValue)
						{
							dayOfWeekIndex = (int)dow.GetValueOrDefault();
						}
					}

					if (inRange)
					{
						if ((dayOfWeekIndex < 0) || (dayOfWeekIndex < lastDayOfWeekIndex) || (dayOfWeekIndex >= maxDayOfWeek))
						{
							throwError();
						}

						for (var index = lastDayOfWeekIndex; index < dayOfWeekIndex; index++)
						{
							DaysOfWeekFlags[index] = true;
						}

						lastDayOfWeekIndex = -1;
						inRange = false;
					}
					else if (string.Equals(value, "*"))
					{
						Any = true;
					}
					else if (string.Equals(value, "-"))
					{
						if (lastDayOfWeekIndex < 0)
						{
							throwError();
						}

						inRange = true;
					}
					else if ((dayOfWeekIndex < 0) || (dayOfWeekIndex >= maxDayOfWeek))
					{
						throwError();
					}
					else
					{
						DaysOfWeekFlags[dayOfWeekIndex] = true;
						lastDayOfWeekIndex = dayOfWeekIndex;
					}
				}
			}

			private CronExpressionDaysOfWeek(bool[] daysOfWeekFlags)
			{
				for (var dayOfWeekIndex = 0; dayOfWeekIndex < maxDayOfWeek; dayOfWeekIndex++)
				{
					DaysOfWeekFlags[dayOfWeekIndex] = daysOfWeekFlags[dayOfWeekIndex];
				}
			}

			internal CronExpressionDaysOfWeek Clone()
			{
				return new CronExpressionDaysOfWeek(DaysOfWeekFlags);
			}

			internal CronExpressionDaysOfWeek ShiftDays(int days)
			{
				var daysOfWeekFlags = new bool[maxDayOfWeek];

				for (var dayOfWeekIndex = 0; dayOfWeekIndex < maxDayOfWeek; dayOfWeekIndex++)
				{
					daysOfWeekFlags[(dayOfWeekIndex + maxDayOfWeek + days) % maxDayOfWeek] = DaysOfWeekFlags[dayOfWeekIndex];
				}

				return new CronExpressionDaysOfWeek(daysOfWeekFlags);
			}

			public override string ToString()
			{
				return Formatted();
			}

			public string Formatted()
			{
				var ranges = new List<string>();

				var dayOfWeekIndex = 0;
				while (dayOfWeekIndex < maxDayOfWeek)
				{
					if (DaysOfWeekFlags[dayOfWeekIndex])
					{
						var dayOfWeekEndRangeIndex = dayOfWeekIndex + 1;
						if ((dayOfWeekEndRangeIndex < maxDayOfWeek) && DaysOfWeekFlags[dayOfWeekEndRangeIndex++])
						{
							while ((dayOfWeekEndRangeIndex < maxDayOfWeek) && DaysOfWeekFlags[dayOfWeekEndRangeIndex])
							{
								dayOfWeekEndRangeIndex++;
							}

							ranges.Add(string.Format("{0}-{1}", dayOfWeekIndex, (dayOfWeekEndRangeIndex >= maxDayOfWeek ? maxDayOfWeek - 1 : dayOfWeekEndRangeIndex)));

							dayOfWeekIndex = dayOfWeekEndRangeIndex - 1;
						}
						else
						{
							ranges.Add(string.Format("{0}", dayOfWeekIndex));
						}
					}

					dayOfWeekIndex++;
				}

				return Formatted(ranges);
			}
		}
	}
}
