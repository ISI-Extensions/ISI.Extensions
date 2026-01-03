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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class CronExpression
	{
		public class CronExpressionMonths : CronExpressionElement
		{
			private const int maxMonth = 13;

			public bool[] MonthFlags { get; } = new bool[maxMonth];

			public CronExpressionMonths()
			{

			}

			public CronExpressionMonths(string source)
			{
				Action throwError = () => throw new($"Months: Cannot parse \"{source}\"");

				var parser = new ISI.Extensions.Parsers.DelimitedTextParser(',');

				var parserContext = parser.CreateTextParserContext();

				var values = parser.Read(parserContext, source.Replace("-", ",-,")).Record.Select(value => value.Trim());

				var lastMonthIndex = -1;
				var inRange = false;
				foreach (var value in values)
				{
					var monthIndex = value.ToInt(-1);

					if (monthIndex < 0)
					{
						var dow = value.ToMonthNullable();

						if (dow.HasValue)
						{
							monthIndex = (int)dow.GetValueOrDefault();
						}
					}

					if (inRange)
					{
						if ((monthIndex < 0) || (monthIndex < lastMonthIndex) || (monthIndex >= maxMonth))
						{
							throwError();
						}

						for (var index = lastMonthIndex; index < monthIndex; index++)
						{
							MonthFlags[index] = true;
						}

						lastMonthIndex = -1;
						inRange = false;
					}
					else if (string.Equals(value, "*"))
					{
						Any = true;
					}
					else if (string.Equals(value, "-"))
					{
						if (lastMonthIndex < 0)
						{
							throwError();
						}

						inRange = true;
					}
					else if ((monthIndex < 0) || (monthIndex >= maxMonth))
					{
						throwError();
					}
					else
					{
						MonthFlags[monthIndex] = true;
						lastMonthIndex = monthIndex;
					}
				}
			}

			private CronExpressionMonths(bool[] monthsFlags)
			{
				for (var monthIndex = 0; monthIndex < maxMonth; monthIndex++)
				{
					MonthFlags[monthIndex] = monthsFlags[monthIndex];
				}
			}

			internal CronExpressionMonths Clone()
			{
				return new(MonthFlags);
			}

			public override string ToString()
			{
				return Formatted();
			}

			public string Formatted()
			{
				var ranges = new List<string>();

				var MonthIndex = 0;
				while (MonthIndex < maxMonth)
				{
					if (MonthFlags[MonthIndex])
					{
						var MonthEndRangeIndex = MonthIndex + 1;
						if ((MonthEndRangeIndex < maxMonth) && MonthFlags[MonthEndRangeIndex++])
						{
							while ((MonthEndRangeIndex < maxMonth) && MonthFlags[MonthEndRangeIndex])
							{
								MonthEndRangeIndex++;
							}

							ranges.Add($"{MonthIndex}-{(MonthEndRangeIndex >= maxMonth ? maxMonth - 1 : MonthEndRangeIndex)}");

							MonthIndex = MonthEndRangeIndex - 1;
						}
						else
						{
							ranges.Add($"{MonthIndex}");
						}
					}

					MonthIndex++;
				}

				return Formatted(ranges);
			}
		}
	}
}
