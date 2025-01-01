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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class CronExpression
	{
		public class CronExpressionHours : CronExpressionElement
		{
			private const int maxHour = 24;

			public bool[] HourFlags { get; } = new bool[maxHour];

			public CronExpressionHours()
			{

			}

			public CronExpressionHours(string source)
			{
				Action throwError = () => throw new(string.Format("Hours: Cannot parse \"{0}\"", source));

				var parser = new ISI.Extensions.Parsers.DelimitedTextParser(',');

				var parserContext = parser.CreateTextParserContext();

				var values = parser.Read(parserContext, source.Replace("-", ",-,")).Record.Select(value => value.Trim());

				var lastHourIndex = -1;
				var inRange = false;
				foreach (var value in values)
				{
					var hourIndex = value.ToInt(-1);

					if (inRange)
					{
						if ((hourIndex < 0) || (hourIndex < lastHourIndex) || (hourIndex >= maxHour))
						{
							throwError();
						}

						for (var index = lastHourIndex; index < hourIndex; index++)
						{
							HourFlags[index] = true;
						}

						lastHourIndex = -1;
						inRange = false;
					}
					else if (string.Equals(value, "*"))
					{
						Any = true;
					}
					else if (string.Equals(value, "-"))
					{
						if (lastHourIndex < 0)
						{
							throwError();
						}

						inRange = true;
					}
					else if ((hourIndex < 0) || (hourIndex >= maxHour))
					{
						throwError();
					}
					else
					{
						HourFlags[hourIndex] = true;
						lastHourIndex = hourIndex;
					}
				}
			}

			private CronExpressionHours(bool[] hoursFlags)
			{
				for (var hourIndex = 0; hourIndex < maxHour; hourIndex++)
				{
					HourFlags[hourIndex] = hoursFlags[hourIndex];
				}
			}

			internal CronExpressionHours Clone()
			{
				return new(HourFlags);
			}

			internal CronExpressionHours ShiftHours(int hours)
			{
				var hourFlags = new bool[maxHour];

				for (var hourIndex = 0; hourIndex < maxHour; hourIndex++)
				{
					hourFlags[(hourIndex + maxHour + hours) % maxHour] = HourFlags[hourIndex];
				}

				return new(hourFlags);
			}

			public override string ToString()
			{
				return Formatted();
			}

			public string Formatted()
			{
				var ranges = new List<string>();

				var hourIndex = 0;
				while (hourIndex < maxHour)
				{
					if (HourFlags[hourIndex])
					{
						var hourEndRangeIndex = hourIndex + 1;
						if ((hourEndRangeIndex < maxHour) && HourFlags[hourEndRangeIndex++])
						{
							while ((hourEndRangeIndex < maxHour) && HourFlags[hourEndRangeIndex])
							{
								hourEndRangeIndex++;
							}

							ranges.Add(string.Format("{0}-{1}", hourIndex, (hourEndRangeIndex >= maxHour ? maxHour - 1 : hourEndRangeIndex)));

							hourIndex = hourEndRangeIndex - 1;
						}
						else
						{
							ranges.Add(string.Format("{0}", hourIndex));
						}
					}

					hourIndex++;
				}

				return Formatted(ranges);
			}
		}
	}
}
