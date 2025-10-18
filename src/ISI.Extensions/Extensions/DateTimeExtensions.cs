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
	public static class DateTimeExtensions
	{
		#region Formats
		public const string FormatDateTime = @"MM/dd/yyyy HH:mm:ss";
		public const string FormatDateTimeStandard = @"MM/dd/yyyy h:mm tt";
		public const string FormatDateTimeShort = @"M/d/yy h:mm tt";
		public const string FormatDateTimePrecise = @"yyyy-MM-dd HH:mm:ss.fff";
		public const string FormatDateTimeUniversal = @"yyyy-MM-ddTHH:mm:ss.fffK";
		public const string FormatDateTimeUniversalPrecise = @"yyyy-MM-ddTHH:mm:ss.fffffffK";
		public const string FormatDateTimeSortable = @"yyyyMMdd.HHmmss";
		public const string FormatDateTimeSortablePrecise = @"yyyyMMdd.HHmmssfff";

		public const string FormatDate = @"MM/dd/yyyy";
		public const string FormatDateShort = @"M/d/yy";
		public const string FormatDateSortable = @"yyyyMMdd";

		public const string FormatTime = @"hh:mm tt";
		public const string FormatTimeShort = @"h:mm tt";
		public const string FormatTimePrecise = @"HH:mm:ss.fff";
		public const string FormatTimeSortable = @"HHmmss";
		public const string FormatTimeSortablePrecise = @"HHmmssfff";

		public const string FormatYearMonthDay = @"yyyyMMdd";
		public const string FormatWeekDayName = @"dddd";
		#endregion

		public enum DateTimeFormat
		{
			DateTime = 0,
			DateTimeStandard = 17,
			DateTimeShort = 1,
			DateTimePrecise = 2,
			DateTimeUniversal = 10,
			DateTimeUniversalPrecise = 16,
			DateTimeSortable = 11,
			DateTimeSortablePrecise = 12,

			Date = 3,
			DateShort = 4,
			DateSortable = 13,

			Time = 6,
			TimeShort = 7,
			TimePrecise = 8,
			TimeSortable = 14,
			TimeSortablePrecise = 15,

			YearMonthDay = 5,
			WeekDayName = 9
		}

		private static readonly IDictionary<DateTimeFormat, string> _dateFormats = new System.Collections.Concurrent.ConcurrentDictionary<DateTimeFormat, string>([
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTime, FormatDateTime),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeStandard, FormatDateTimeStandard),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeShort, FormatDateTimeShort),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimePrecise, FormatDateTimePrecise),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeUniversal, FormatDateTimeUniversal),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeUniversalPrecise, FormatDateTimeUniversalPrecise),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeSortable, FormatDateTimeSortable),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateTimeSortablePrecise, FormatDateTimeSortablePrecise),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.Date, FormatDate),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateShort, FormatDateShort),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.DateSortable, FormatDateSortable),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.Time, FormatTime),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.TimeShort, FormatTimeShort),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.TimePrecise, FormatTimePrecise),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.TimeSortable, FormatTimeSortable),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.TimeSortablePrecise, FormatTimeSortablePrecise),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.YearMonthDay, FormatYearMonthDay),
			new KeyValuePair<DateTimeFormat, string>(DateTimeFormat.WeekDayName, FormatWeekDayName)
		]);

		private static DateTime ToDateTimeUtc(this DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new(dateTime.Ticks, DateTimeKind.Utc);
			}

			return dateTime;
		}

		public static DateTime ToDateTimeUtc(this string value) => ToDateTimeUtc(value, DateTime.MinValue);

		public static DateTime ToDateTimeUtc(this string value, DateTime defaultValue)
		{
			value ??= string.Empty;

			if (DateTime.TryParse(value, out var parsedValue))
			{
				return parsedValue.ToDateTimeUtc();
			}

			foreach (var dateFormat in _dateFormats.Values)
			{
				if (DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out parsedValue))
				{
					return parsedValue.ToDateTimeUtc();
				}
			}

			return defaultValue;
		}

		public static DateTime ToDateTime(this string value) => ToDateTime(value, DateTime.MinValue);

		public static DateTime ToDateTime(this string value, DateTime defaultValue)
		{
			value ??= string.Empty;

			if (DateTime.TryParse(value, out var parsedValue))
			{
				return parsedValue;
			}

			foreach (var dateFormat in _dateFormats.Values)
			{
				if (DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out parsedValue))
				{
					return parsedValue;
				}
			}

			return defaultValue;
		}

		public static DateTime? ToDateTimeUtcNullable(this string value, DateTime? defaultValue = null)
		{
			value ??= string.Empty;

			if (DateTime.TryParse(value, out var parsedValue))
			{
				return parsedValue.ToDateTimeUtc();
			}

			foreach (var dateFormat in _dateFormats.Values)
			{
				if (DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out parsedValue))
				{
					return parsedValue.ToDateTimeUtc();
				}
			}

			return defaultValue;
		}

		public static DateTime? ToDateTimeNullable(this string value, DateTime? defaultValue = null)
		{
			value ??= string.Empty;

			if (DateTime.TryParse(value, out var parsedValue))
			{
				return parsedValue;
			}

			foreach (var dateFormat in _dateFormats.Values)
			{
				if (DateTime.TryParseExact(value, dateFormat, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out parsedValue))
				{
					return parsedValue;
				}
			}

			return defaultValue;
		}









		public static string Formatted(this DateTime? utcDateTime, DateTimeFormat format, TimeSpan timeZoneOffset)
		{
			return Formatted(utcDateTime.GetValueOrDefault(DateTime.MinValue), format, timeZoneOffset);
		}
		public static string Formatted(this DateTime utcDateTime, DateTimeFormat format, TimeSpan timeZoneOffset)
		{
			if (utcDateTime != DateTime.MinValue)
			{
				return Formatted(utcDateTime + timeZoneOffset, format);
			}

			return string.Empty;
		}

		public static string Formatted(this DateTime? utcDateTime, DateTimeFormat format, string timeZoneName, bool showTimeZone = true)
		{
			return Formatted(utcDateTime.GetValueOrDefault(DateTime.MinValue), format, timeZoneName, showTimeZone);
		}
		public static string Formatted(this DateTime utcDateTime, DateTimeFormat format, string timeZoneName, bool showTimeZone = true)
		{
			if ((utcDateTime == DateTime.MinValue) || (showTimeZone && _dateFormats.ContainsKey(format)))
			{
				showTimeZone = false;
			}

			var timeZone = TimeZoneInfoHelper.GetTimeZone(timeZoneName);

			if (string.IsNullOrEmpty(timeZone))
			{
				return Formatted(utcDateTime, format);
			}

			var timeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById(timeZone);

			var datetime = System.TimeZoneInfo.ConvertTimeFromUtc(new(utcDateTime.Ticks, DateTimeKind.Utc), timeZoneInfo);

			return $"{Formatted(datetime, format)}{(showTimeZone ? $" {timeZoneInfo.TimeZoneShortName(datetime)}" : string.Empty)}";
		}

		public static string Formatted(this DateTime? utcDateTime, DateTimeFormat format, System.TimeZoneInfo timeZoneInfo, bool showTimeZone = true)
		{
			return Formatted(utcDateTime.GetValueOrDefault(DateTime.MinValue), format, timeZoneInfo, showTimeZone);
		}
		public static string Formatted(this DateTime utcDateTime, DateTimeFormat format, System.TimeZoneInfo timeZoneInfo, bool showTimeZone = true)
		{
			if ((utcDateTime == DateTime.MinValue) || (showTimeZone && _dateFormats.ContainsKey(format)))
			{
				showTimeZone = false;
			}

			var datetime = System.TimeZoneInfo.ConvertTimeFromUtc(new(utcDateTime.Ticks, DateTimeKind.Utc), timeZoneInfo);

			return $"{Formatted(datetime, format)}{(showTimeZone ? $" {timeZoneInfo.TimeZoneShortName(datetime)}" : string.Empty)}";
		}

		public static string Formatted(this DateTime? value, DateTimeFormat format)
		{
			return Formatted(value.GetValueOrDefault(DateTime.MinValue), format);
		}
		public static string Formatted(this DateTime value, DateTimeFormat format)
		{
			if ((value != DateTime.MinValue) && _dateFormats.TryGetValue(format, out string formatString))
			{
				return value.ToString(formatString);
			}

			return string.Empty;
		}

		public static string Formatted(this DateTimeOffset? value, DateTimeFormat format)
		{
			return Formatted(value.GetValueOrDefault(DateTime.MinValue), format);
		}
		public static string Formatted(this DateTimeOffset value, DateTimeFormat format)
		{
			if ((value != DateTime.MinValue) && _dateFormats.TryGetValue(format, out string formatString))
			{
				return value.ToString(formatString);
			}

			return string.Empty;
		}
	}
}