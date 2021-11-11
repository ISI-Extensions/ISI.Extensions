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

namespace ISI.Extensions.DataReader
{
	public abstract class AbstractDataReader : ISI.Extensions.DataReader.IDataReader
	{
		protected object[] Values = null;

		public int Depth { get; protected set; }
		public bool IsClosed { get; protected set; }
		public int RecordsAffected { get; protected set; }

		public int FieldCount { get; protected set; }

		public abstract bool NextResult();

		public abstract bool Read();

		public abstract void Close();

		public abstract System.Data.DataTable GetSchemaTable();



		public virtual string GetName(int columnIndex)
		{
			throw new NotImplementedException();
		}

		public virtual string GetDataTypeName(int columnIndex)
		{
			throw new NotImplementedException();
		}

		public virtual Type GetFieldType(int columnIndex)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsDBNull(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			return IsNull(columnIndex);
		}

		protected virtual bool IsNull(int columnIndex)
		{
			return Values[columnIndex] == null;
		}

		public virtual object this[int columnIndex] => GetValue(columnIndex);

		public virtual object this[string name] => GetValue(GetOrdinal(name));

		protected virtual void CheckColumnIndex(int columnIndex)
		{
			if ((columnIndex < 0) || (columnIndex >= FieldCount))
			{
				throw new System.IndexOutOfRangeException(string.Format("trying to access Column {0}", columnIndex));
			}
		}

		public virtual object GetValue(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			return (object)Values[columnIndex];
		}

		public virtual int GetValues(object[] values)
		{
			var columnIndex = 0;

			while ((columnIndex < FieldCount) && (columnIndex < values.Length))
			{
				values[columnIndex] = GetValue(columnIndex);
				columnIndex++;
			}

			return columnIndex;
		}

		public IDictionary<string, object> GetValuesDictionary()
		{
			var response = new Dictionary<string, object>();

			for (var columnIndex = 0; columnIndex < FieldCount; columnIndex++)
			{
				response.Add(GetName(columnIndex), GetValue(columnIndex));
			}

			return response;
		}

		public abstract int GetOrdinal(string name);

		public virtual TEnum GetEnum<TEnum>(int columnIndex, TEnum defaultValue = default)
		{
			CheckColumnIndex(columnIndex);

			return ISI.Extensions.Enum<TEnum>.Parse(GetString(columnIndex), defaultValue);
		}

		public virtual bool GetBoolean(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(bool))
			{
				return (bool)Values[columnIndex];
			}
			if (columnType == typeof(bool?))
			{
				return ((bool?)Values[columnIndex]).GetValueOrDefault();
			}
			if (columnType == typeof(int))
			{
				return ((int)Values[columnIndex] != 0);
			}

			return GetString(columnIndex).ToBoolean();
		}

		public virtual bool? GetBooleanNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			if (IsNull(columnIndex))
			{
				return null;
			}

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(bool))
			{
				return (bool)Values[columnIndex];
			}
			if (columnType == typeof(bool?))
			{
				return (bool?)Values[columnIndex];
			}
			if (columnType == typeof(int))
			{
				return ((int)Values[columnIndex] != 0);
			}

			return GetString(columnIndex).ToBooleanNullable();
		}

		public virtual byte GetByte(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(byte))
			{
				return (byte)Values[columnIndex];
			}
			return byte.Parse(GetString(columnIndex));
		}

		public virtual long GetBytes(int columnIndex, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual char GetChar(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			return GetString(columnIndex).FirstOrDefault();
		}

		public virtual long GetChars(int columnIndex, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual System.Data.IDataReader GetData(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual TimeSpan GetTimeSpan(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(TimeSpan))
			{
				return (TimeSpan)Values[columnIndex];
			}
			if (columnType == typeof(TimeSpan?))
			{
				return ((TimeSpan?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToTimeSpan();
		}

		public virtual TimeSpan? GetTimeSpanNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual DateTime GetDateTime(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(DateTime))
			{
				return (DateTime)Values[columnIndex];
			}
			if (columnType == typeof(DateTime?))
			{
				return ((DateTime?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToDateTime();
		}

		public virtual DateTime? GetDateTimeNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual DateTime GetDateTimeUtc(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(DateTime))
			{
				return DateTime.SpecifyKind((DateTime) Values[columnIndex], DateTimeKind.Utc);
			}
			if (columnType == typeof(DateTime?))
			{
				return DateTime.SpecifyKind(((DateTime?) Values[columnIndex]).GetValueOrDefault(), DateTimeKind.Utc);
			}

			return DateTime.SpecifyKind(GetString(columnIndex).ToDateTime(), DateTimeKind.Utc);
		}

		public virtual DateTime? GetDateTimeUtcNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			throw new NotImplementedException();
		}

		public virtual decimal GetDecimal(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(decimal))
			{
				return (decimal)Values[columnIndex];
			}
			if (columnType == typeof(decimal?))
			{
				return ((decimal?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToDecimal();
		}

		public virtual decimal? GetDecimalNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			if (IsNull(columnIndex))
			{
				return null;
			}

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(decimal))
			{
				return (decimal)Values[columnIndex];
			}
			if (columnType == typeof(decimal?))
			{
				return (decimal?) Values[columnIndex];
			}

			return GetString(columnIndex).ToDecimalNullable();
		}

		public virtual Guid GetGuid(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(Guid))
			{
				return (Guid)Values[columnIndex];
			}
			if (columnType == typeof(Guid?))
			{
				return ((Guid?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToGuid();
		}

		public virtual Guid? GetGuidNullable(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			if (IsNull(columnIndex))
			{
				return null;
			}

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(Guid))
			{
				return (Guid)Values[columnIndex];
			}
			if (columnType == typeof(Guid?))
			{
				return (Guid?) Values[columnIndex];
			}

			return GetString(columnIndex).ToGuidNullable();
		}

		public virtual short GetInt16(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(short))
			{
				return (short)Values[columnIndex];
			}
			if (columnType == typeof(short?))
			{
				return ((short?)Values[columnIndex]).GetValueOrDefault();
			}

			return short.Parse(GetString(columnIndex));
		}

		public virtual int GetInt(int columnIndex)
		{
			return GetInt32(columnIndex);
		}

		public virtual int? GetIntNullable(int columnIndex)
		{
			if (IsNull(columnIndex))
			{
				return null;
			}

			return GetInt32(columnIndex);
		}

		public virtual int GetInt32(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(int))
			{
				return (int)Values[columnIndex];
			}
			if (columnType == typeof(int?))
			{
				return ((int?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToInt();
		}

		public virtual long GetLong(int columnIndex)
		{
			return GetInt64(columnIndex);
		}

		public virtual long? GetLongNullable(int columnIndex)
		{
			if (IsNull(columnIndex))
			{
				return null;
			}

			return GetInt64(columnIndex);
		}

		public virtual long GetInt64(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(long))
			{
				return (long)Values[columnIndex];
			}
			if (columnType == typeof(long?))
			{
				return ((long?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToLong();
		}

		public virtual float GetFloat(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(float))
			{
				return (float)Values[columnIndex];
			}
			if (columnType == typeof(float?))
			{
				return ((float?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToFloat();
		}

		public virtual float? GetFloatNullable(int columnIndex)
		{
			if (IsNull(columnIndex))
			{
				return null;
			}

			return GetFloat(columnIndex);
		}

		public virtual double GetDouble(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(double))
			{
				return (double)Values[columnIndex];
			}
			if (columnType == typeof(double?))
			{
				return ((double?)Values[columnIndex]).GetValueOrDefault();
			}

			return GetString(columnIndex).ToDouble();
		}

		public virtual double? GetDoubleNullable(int columnIndex)
		{
			if (IsNull(columnIndex))
			{
				return null;
			}

			return GetDouble(columnIndex);
		}

		public virtual string GetString(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			var columnType = Values[columnIndex].GetType();

			if (columnType == typeof(string))
			{
				return (string)Values[columnIndex];
			}

			return string.Format("{0}", Values[columnIndex]);
		}

		public virtual void Dispose()
		{
			//throw new NotImplementedException();
		}
	}
}
