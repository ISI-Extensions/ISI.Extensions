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

namespace ISI.Extensions.Columns
{
	public class ColumnInfo<TRecord, TProperty> : IColumnInfo<TRecord>
		where TRecord : class, new()
	{
		public Type PropertyType => typeof(TProperty);
		public string ColumnName { get; }
		public string[] ColumnNames { get; }
		public Func<TRecord, TProperty> GetValue { get; }
		public Action<TRecord, TProperty> SetValue { get; }
		public Func<TRecord, bool> IsNull { get; }
		public Func<object, TProperty> TransformValue { get; }
		public Func<TRecord, string> FormattedValue { get; }

		public ColumnInfo(
			IEnumerable<string> columnNames,
			System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue)
			: this(null, columnNames, ISI.Extensions.Reflection.GetPropertyInfo(property), transformValue, formattedValue)
		{
		}

		public ColumnInfo(
			string columnName,
			Func<TRecord, bool> isNull,
			Func<TRecord, object> getValue)
		{
			ColumnName = columnName;
			IsNull = isNull;
			GetValue = record => (TProperty)getValue(record);
		}

		public ColumnInfo(
			string columnName,
			System.Reflection.PropertyInfo propertyInfo,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue)
		: this(columnName, new[] { columnName }, propertyInfo, transformValue, formattedValue)
		{
		}

		public ColumnInfo(
			string columnName,
			IEnumerable<string> columnNames,
			System.Reflection.PropertyInfo propertyInfo,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue)
		{
			ColumnName = (string.IsNullOrWhiteSpace(columnName) ? propertyInfo.Name : columnName);
			ColumnNames = (columnNames ?? new[] { propertyInfo.Name }).ToArray();
			var columnType = typeof(TProperty);
			var isNullable = (columnType.IsGenericType && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)));
			GetValue = record => (TProperty)propertyInfo.GetValue(record);   // property.Compile();
			SetValue = (record, value) => propertyInfo.SetValue(record, value);


			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};


			if (transformValue == null)
			{
				var type = typeof(TProperty);

				if (type == typeof(string))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue : (TProperty)(object)string.Format("{0}", value));
				}
				else if (type == typeof(Guid))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToGuid() : (value is Guid @guid ? (TProperty)(object)@guid : (TProperty)(object)string.Format("{0}", value).ToGuid()));
				}
				else if (type == typeof(Guid?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToGuidNullable() : (value is Guid? ? (TProperty)(object)(Guid?)value : (TProperty)(object)string.Format("{0}", value).ToGuidNullable()));
				}
				else if (type == typeof(int))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToInt() : (value is int @int ? (TProperty)(object)@int : (TProperty)(object)string.Format("{0}", value).ToInt()));
				}
				else if (type == typeof(int?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToIntNullable() : (value is int? ? (TProperty)(object)(int?)value : (TProperty)(object)string.Format("{0}", value).ToIntNullable()));
				}
				else if (type == typeof(long))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToLong() : (value is long @long ? (TProperty)(object)@long : (TProperty)(object)string.Format("{0}", value).ToLong()));
				}
				else if (type == typeof(long?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToLongNullable() : (value is long? ? (TProperty)(object)(long?)value : (TProperty)(object)string.Format("{0}", value).ToLongNullable()));
				}
				else if (type == typeof(double))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDouble() : (value is double @double ? (TProperty)(object)@double : (TProperty)(object)string.Format("{0}", value).ToDouble()));
				}
				else if (type == typeof(double?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDoubleNullable() : (value is double? ? (TProperty)(object)(double?)value : (TProperty)(object)string.Format("{0}", value).ToDoubleNullable()));
				}
				else if (type == typeof(decimal))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDecimal() : (value is decimal @decimal ? (TProperty)(object)@decimal : (TProperty)(object)string.Format("{0}", value).ToDecimal()));
				}
				else if (type == typeof(decimal?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDecimalNullable() : (value is decimal? ? (TProperty)(object)(decimal?)value : (TProperty)(object)string.Format("{0}", value).ToDecimalNullable()));
				}
				else if (type == typeof(bool))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToBoolean() : (value is bool @bool ? (TProperty)(object)@bool : (TProperty)(object)string.Format("{0}", value).ToBoolean()));
				}
				else if (type == typeof(bool?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToBooleanNullable() : (value is bool? ? (TProperty)(object)(bool?)value : (TProperty)(object)string.Format("{0}", value).ToBooleanNullable()));
				}
				else if (type == typeof(DateTime))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDateTime() : (value is DateTime @DateTime ? (TProperty)(object)@DateTime : (TProperty)(object)string.Format("{0}", value).ToDateTime()));
				}
				else if (type == typeof(DateTime?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToDateTimeNullable() : (value is DateTime? ? (TProperty)(object)(DateTime?)value : (TProperty)(object)string.Format("{0}", value).ToDateTimeNullable()));
				}
				else if (type == typeof(TimeSpan))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToTimeSpan() : (value is TimeSpan @TimeSpan ? (TProperty)(object)@TimeSpan : (TProperty)(object)string.Format("{0}", value).ToTimeSpan()));
				}
				else if (type == typeof(TimeSpan?))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)stringValue.ToTimeSpanNullable() : (value is TimeSpan? ? (TProperty)(object)(TimeSpan?)value : (TProperty)(object)string.Format("{0}", value).ToTimeSpanNullable()));
				}
				else if (ISI.Extensions.Enum.IsEnum(type))
				{
					transformValue = value => (value is string stringValue ? (TProperty)(object)(ISI.Extensions.Enum.Parse(type, stringValue)) : (TProperty)(object)(ISI.Extensions.Enum.Parse(type, string.Format("{0}", value))));
				}
			}
			TransformValue = transformValue;

			FormattedValue = formattedValue ?? (record => string.Format("{0}", GetValue(record)));
		}

		Func<object, bool> IColumnInfo.IsNull { get; }

		object IColumnInfo.GetValue(object record) => GetValue(record as TRecord);

		object IColumnInfo<TRecord>.GetValue(TRecord record)
		{
			return GetValue(record);
		}

		void IColumnInfo<TRecord>.SetValue(TRecord record, object value)
		{
			SetValue(record, (TProperty)value);
		}

		object IColumnInfo<TRecord>.TransformValue(object value)
		{
			if (TransformValue == null)
			{
				return value;
			}

			return TransformValue(value);
		}

		string IColumnInfo<TRecord>.FormattedValue(TRecord record)
		{
			return FormattedValue(record);
		}
	}

	public class ColumnInfo : IColumnInfo
	{
		public string ColumnName { get; }
		public Func<object, bool> IsNull { get; }
		public Func<object, object> GetValue { get; }

		public ColumnInfo(
			string columnName)
			: this(columnName, record => true, record => null)
		{
		}

		public ColumnInfo(
			string columnName,
			Func<object, bool> isNull,
			Func<object, object> getValue)
		{
			ColumnName = columnName;
			IsNull = isNull;
			GetValue = getValue;
		}

		object IColumnInfo.GetValue(object record) => GetValue(record);
	}
}
