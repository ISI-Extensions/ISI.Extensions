#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Repository.SqlServer.Extensions
{
	public static partial class DbDataReaderByColumnNameExtensions
	{
		public static TEnum GetEnum<TEnum>(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			var ordinal = dbDataReader.GetOrdinal(name);

			if (dbDataReader.IsDBNull(ordinal))
			{
				return default(TEnum);
			}

			return ISI.Extensions.Enum<TEnum>.Parse(string.Format("{0}", dbDataReader.GetValue(ordinal)));
		}

		public static string GetString(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			var ordinal = dbDataReader.GetOrdinal(name);

			return (dbDataReader.IsDBNull(ordinal) ? (string)null : dbDataReader.GetString(ordinal));
		}

		public static bool GetBoolean(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetBoolean(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static bool? GetBooleanNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetBooleanNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static byte[] GetBytes(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetBytes(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static Guid GetGuid(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetGuid(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static Guid? GetGuidNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetGuidNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static int GetInt(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetInt(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static int? GetIntNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetIntNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static long GetLong(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetLong(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static long? GetLongNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetLongNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static double GetDouble(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDouble(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static double? GetDoubleNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDoubleNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static decimal GetDecimal(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDecimal(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static decimal? GetDecimalNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDecimalNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static float GetFloat(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetFloat(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static float? GetFloatNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetFloatNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static DateTime GetDateTime(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDateTime(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static DateTime? GetDateTimeNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetDateTimeNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static TimeSpan GetTimeSpan(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetTimeSpan(dbDataReader, dbDataReader.GetOrdinal(name));
		}

		public static TimeSpan? GetTimeSpanNullable(this System.Data.Common.DbDataReader dbDataReader, string name)
		{
			return DbDataReaderByOrdinalExtensions.GetTimeSpanNullable(dbDataReader, dbDataReader.GetOrdinal(name));
		}
	}
}