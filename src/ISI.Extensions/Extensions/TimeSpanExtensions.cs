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
using System.Text;

namespace ISI.Extensions.Extensions
{
	public static class TimeSpanExtensions
	{
		public enum TimeSpanFormat
		{
			Default = 0,
			Short = 1,
			Precise = 2
		}
		public static string Formatted(this TimeSpan? value, TimeSpanFormat format)
		{
			return Formatted(value.GetValueOrDefault(TimeSpan.MinValue), format);
		}
		public static string Formatted(this TimeSpan value, TimeSpanFormat format)
		{
			var result = string.Empty;

			if (value != TimeSpan.MinValue)
			{
				switch (format)
				{
					case TimeSpanFormat.Default:
						result = string.Format("{0}.{1:00}:{2:00}:{3:00}", value.Days, value.Hours, value.Minutes, value.Seconds);
						break;
					case TimeSpanFormat.Short:
						if (value.Days > 0)
						{
							if (value.Milliseconds > 0)
							{
								result = string.Format("{0}.{1:00}:{2:00}:{3:00}.{4:000}", value.Days, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
							}
							else if (value.Seconds > 0)
							{
								result = string.Format("{0}.{1:00}:{2:00}:{3:00}", value.Days, value.Hours, value.Minutes, value.Seconds);
							}
							else if (value.Minutes > 0)
							{
								result = string.Format("{0}.{1:00}:{2:00}", value.Days, value.Hours, value.Minutes);
							}
							else if (value.Hours > 0)
							{
								result = string.Format("{0}.{1:00}", value.Days, value.Hours);
							}
							else
							{
								result = string.Format("{0}", value.Days);
							}
						}
						else
						{
							if (value.Milliseconds > 0)
							{
								result = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
							}
							else if (value.Seconds > 0)
							{
								result = string.Format("{0:00}:{1:00}:{2:00}", value.Hours, value.Minutes, value.Seconds);
							}
							else if (value.Minutes > 0)
							{
								result = string.Format("{0:00}:{1:00}", value.Hours, value.Minutes);
							}
							else
							{
								result = string.Format("{0:00}", value.Hours);
							}
						}
						break;
					case TimeSpanFormat.Precise:
						result = string.Format("{0}.{1:00}:{2:00}:{3:00}.{4:000}", value.Days, value.Hours, value.Minutes, value.Seconds, value.Milliseconds);
						break;
				}
			}

			return result;
		}

		public static TimeSpan ToTimeSpan(this string value)
		{
			return ToTimeSpan(value, TimeSpan.Zero);
		}
		public static TimeSpan ToTimeSpan(this string value, TimeSpan defaultValue)
		{
			if (TimeSpan.TryParse(value ?? string.Empty, out var parsedValue))
			{
				return parsedValue;
			}

			return defaultValue;
		}

		public static TimeSpan? ToTimeSpanNullable(this string value, TimeSpan? defaultValue = null)
		{
			if (TimeSpan.TryParse(value, out var parsedValue))
			{
				return parsedValue;
			}

			return defaultValue;
		}
	}
}
