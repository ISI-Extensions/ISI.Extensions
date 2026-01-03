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

namespace ISI.Extensions.Repository.Extensions
{
	public static partial class DataReaderByOrdinalExtensions
	{
		public static bool GetBoolean(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is int @int)
			{
				return (@int != 0);
			}

			if (value is long @long)
			{
				return (@long != 0);
			}

			if (value is short @short)
			{
				return (@short != 0);
			}

			return value.ToString().ToBoolean();
		}

		public static bool? GetBooleanNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (bool?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is int @int)
			{
				return (@int != 0);
			}

			if (value is long @long)
			{
				return (@long != 0);
			}

			if (value is short @short)
			{
				return (@short != 0);
			}

			return value.ToString().ToBooleanNullable();
		}

		public static byte[] GetBytes(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return null;
			}

			var value = dbDataReader[ordinal];

			if (value is byte[] @bytes)
			{
				return @bytes;
			}

			throw new NotImplementedException();
		}

		public static Guid GetGuid(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is Guid @guid)
			{
				return @guid;
			}

			return value.ToString().ToGuid();
		}

		public static Guid? GetGuidNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (Guid?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is Guid @guid)
			{
				return @guid;
			}

			return value.ToString().ToGuidNullable();
		}

		public static short GetShort(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToShort();
		}

		public static short? GetShortNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (short?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToShortNullable();
		}

		public static int GetInt(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is int @int)
			{
				return @int;
			}

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToInt();
		}

		public static int? GetIntNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (int?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is int @int)
			{
				return @int;
			}

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToIntNullable();
		}

		public static long GetLong(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is long @long)
			{
				return @long;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToLong();
		}

		public static long? GetLongNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (long?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is long @long)
			{
				return @long;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is short @short)
			{
				return @short;
			}

			return value.ToString().ToLongNullable();
		}

		public static double GetDouble(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is double @double)
			{
				return @double;
			}

			if (value is float @float)
			{
				return @float;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToDouble();
		}

		public static double? GetDoubleNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (double?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is double @double)
			{
				return @double;
			}

			if (value is float @float)
			{
				return @float;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToDoubleNullable();
		}

		public static decimal GetDecimal(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is decimal @decimal)
			{
				return @decimal;
			}

			if (value is double @double)
			{
				return (decimal)@double;
			}

			if (value is float @float)
			{
				return (decimal)@float;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToDecimal();
		}

		public static decimal? GetDecimalNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (decimal?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is decimal @decimal)
			{
				return @decimal;
			}

			if (value is double @double)
			{
				return (decimal)@double;
			}

			if (value is float @float)
			{
				return (decimal)@float;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToDecimalNullable();
		}

		public static float GetFloat(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is float @float)
			{
				return @float;
			}

			if (value is double @double)
			{
				return (float)@double;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToFloat();
		}

		public static float? GetFloatNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (float?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is float @float)
			{
				return @float;
			}

			if (value is double @double)
			{
				return (float)@double;
			}

			if (value is short @short)
			{
				return @short;
			}

			if (value is int @int)
			{
				return @int;
			}

			if (value is long @long)
			{
				return @long;
			}

			return value.ToString().ToFloatNullable();
		}

		public static DateTime GetDateTime(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is DateTime time)
			{
				return time;
			}

			return value.ToString().ToDateTime();
		}

		public static DateTime? GetDateTimeNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (DateTime?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is DateTime time)
			{
				return time;
			}

			return value.ToString().ToDateTimeNullable();
		}

		public static TimeSpan GetTimeSpan(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			var value = dbDataReader[ordinal];

			if (value is TimeSpan span)
			{
				return span;
			}

			if (value is long @long)
			{
				return new(@long);
			}

			if (value is int @int)
			{
				return new((long)@int);
			}

			if (value is short @short)
			{
				return new((long)@short);
			}

			return value.ToString().ToTimeSpan();
		}

		public static TimeSpan? GetTimeSpanNullable(this System.Data.Common.DbDataReader dbDataReader, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return (TimeSpan?)null;
			}

			var value = dbDataReader[ordinal];

			if (value is TimeSpan span)
			{
				return span;
			}

			if (value is long @long)
			{
				return new TimeSpan(@long);
			}

			if (value is int @int)
			{
				return new TimeSpan((long)@int);
			}

			if (value is short @short)
			{
				return new TimeSpan((long)@short);
			}

			return value.ToString().ToTimeSpanNullable();
		}

		public static object GetSerialized(this System.Data.Common.DbDataReader dbDataReader, ISI.Extensions.JsonSerialization.IJsonSerializer serializer, Type type, int ordinal)
		{
			if (dbDataReader.IsDBNull(ordinal))
			{
				return null;
			}

			var value = dbDataReader[ordinal].ToString();

			return serializer.Deserialize(type, value);
		}
	}
}
