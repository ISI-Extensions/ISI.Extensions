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
		public class CronExpressionMinutes : CronExpressionElement
		{
			private const int maxMinute = 60;

			public bool[] MinuteFlags { get; } = new bool[maxMinute];

			public CronExpressionMinutes()
			{

			}

			public CronExpressionMinutes(string source)
			{
				Action throwError = () => throw new(string.Format("Minutes: Cannot parse \"{0}\"", source));

				var parser = new ISI.Extensions.Parsers.DelimitedTextParser(',');

				var parserContext = parser.CreateTextParserContext();

				var values = parser.Read(parserContext, source.Replace("-", ",-,")).Record.Select(value => value.Trim());

				var lastMinuteIndex = -1;
				var inRange = false;
				foreach (var value in values)
				{
					var minuteIndex = value.ToInt(-1);

					if (inRange)
					{
						if ((minuteIndex < 0) || (minuteIndex < lastMinuteIndex) || (minuteIndex >= maxMinute))
						{
							throwError();
						}

						for (var index = lastMinuteIndex; index < minuteIndex; index++)
						{
							MinuteFlags[index] = true;
						}

						lastMinuteIndex = -1;
						inRange = false;
					}
					else if (string.Equals(value, "*"))
					{
						Any = true;
					}
					else if (string.Equals(value, "-"))
					{
						if (lastMinuteIndex < 0)
						{
							throwError();
						}

						inRange = true;
					}
					else if ((minuteIndex < 0) || (minuteIndex >= maxMinute))
					{
						throwError();
					}
					else
					{
						MinuteFlags[minuteIndex] = true;
						lastMinuteIndex = minuteIndex;
					}
				}
			}

			private CronExpressionMinutes(bool[] minutesFlags)
			{
				for (var minuteIndex = 0; minuteIndex < maxMinute; minuteIndex++)
				{
					MinuteFlags[minuteIndex] = minutesFlags[minuteIndex];
				}
			}

			internal CronExpressionMinutes Clone()
			{
				return new(MinuteFlags);
			}

			public override string ToString()
			{
				return Formatted();
			}

			public string Formatted()
			{
				var ranges = new List<string>();

				var minuteIndex = 0;
				while (minuteIndex < maxMinute)
				{
					if (MinuteFlags[minuteIndex])
					{
						var minuteEndRangeIndex = minuteIndex + 1;
						if ((minuteEndRangeIndex < maxMinute) && MinuteFlags[minuteEndRangeIndex++])
						{
							while ((minuteEndRangeIndex < maxMinute) && MinuteFlags[minuteEndRangeIndex])
							{
								minuteEndRangeIndex++;
							}

							ranges.Add(string.Format("{0}-{1}", minuteIndex, (minuteEndRangeIndex >= maxMinute ? maxMinute - 1 : minuteEndRangeIndex)));

							minuteIndex = minuteEndRangeIndex - 1;
						}
						else
						{
							ranges.Add(string.Format("{0}", minuteIndex));
						}
					}

					minuteIndex++;
				}

				return Formatted(ranges);
			}
		}
	}
}
