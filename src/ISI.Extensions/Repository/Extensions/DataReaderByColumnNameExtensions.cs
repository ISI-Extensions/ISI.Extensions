#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public static partial class DataReaderByColumnNameExtensions
	{
		public static TEnum GetEnum<TEnum>(this System.Data.Common.DbDataReader dbDataReader, string name, TEnum defaultValue = default)
		{
			var ordinal = dbDataReader.GetOrdinal(name);

			if (dbDataReader.IsDBNull(ordinal))
			{
				return defaultValue;
			}

			return ISI.Extensions.Enum<TEnum>.Parse(string.Format("{0}", dbDataReader.GetValue(ordinal)), defaultValue);
		}

		public static string GetString(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			var ordinal = dbDataReader.GetOrdinal(name);

			return (dbDataReader.IsDBNull(ordinal) ? (string)null : dbDataReader.GetString(ordinal));
		}

		public static bool GetBoolean(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetBoolean(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static bool? GetBooleanNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetBooleanNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static byte[] GetBytes(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetBytes(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static Guid GetGuid(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetGuid(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static Guid? GetGuidNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetGuidNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static int GetInt(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetInt(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static int? GetIntNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetIntNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static long GetLong(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetLong(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static long? GetLongNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetLongNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static double GetDouble(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDouble(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static double? GetDoubleNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDoubleNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static decimal GetDecimal(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDecimal(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static decimal? GetDecimalNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDecimalNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static float GetFloat(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetFloat(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static float? GetFloatNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetFloatNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static DateTime GetDateTime(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDateTime(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static DateTime? GetDateTimeNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetDateTimeNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static TimeSpan GetTimeSpan(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetTimeSpan(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static TimeSpan? GetTimeSpanNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DataReaderByOrdinalExtensions.GetTimeSpanNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}
	}
}