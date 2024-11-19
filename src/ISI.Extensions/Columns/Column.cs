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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;

namespace ISI.Extensions.Columns
{
	public class Column<TRecord, TProperty> : IColumn<TRecord>
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

		public Column(
			IEnumerable<string> columnNames,
			System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue)
			: this(null, columnNames, ISI.Extensions.Reflection.GetPropertyInfo(property), transformValue, formattedValue)
		{
		}

		public Column(
			string columnName,
			Func<TRecord, bool> isNull,
			Func<TRecord, object> getValue)
		{
			ColumnName = columnName;
			IsNull = isNull;
			GetValue = record => (TProperty)getValue(record);
		}

		public Column(
			string columnName,
			System.Reflection.PropertyInfo propertyInfo,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
		: this(columnName, [columnName], propertyInfo, transformValue, formattedValue, jsonSerializer)
		{
		}

		public Column(
			string columnName,
			IEnumerable<string> columnNames,
			System.Reflection.PropertyInfo propertyInfo,
			Func<object, TProperty> transformValue,
			Func<TRecord, string> formattedValue,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
		{
			ColumnName = (string.IsNullOrWhiteSpace(columnName) ? propertyInfo.Name : columnName);
			ColumnNames = (columnNames ?? [propertyInfo.Name]).ToArray();
			var columnType = typeof(TProperty);
			var isNullable = (columnType.IsGenericType && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)));

			var propertyValueGetterSetterAttribute = propertyInfo.GetCustomAttribute<ISI.Extensions.Extensions.Converters.PropertyValueGetterSetterAttribute>(true);

			if (propertyValueGetterSetterAttribute != null)
			{
				GetValue = record => (TProperty)propertyValueGetterSetterAttribute.GetPropertyValue(record);
			}
			else
			{
				GetValue = record => (TProperty)propertyInfo.GetValue(record);   // property.Compile();
			}

			SetValue = (record, value) => propertyInfo.SetValue(record, value);

			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};


			transformValue ??= GetTransformValue<TProperty>(jsonSerializer);

			TransformValue = transformValue;

			FormattedValue = formattedValue ?? (record => string.Format("{0}", GetValue(record)));
		}

		Func<object, bool> IColumn.IsNull { get; }

		object IColumn.GetValue(object record) => GetValue(record as TRecord);

		object IColumn<TRecord>.GetValue(TRecord record)
		{
			return GetValue(record);
		}

		void IColumn<TRecord>.SetValue(TRecord record, object value)
		{
			SetValue(record, (TProperty)value);
		}

		object IColumn<TRecord>.TransformValue(object value)
		{
			if (TransformValue == null)
			{
				return value;
			}

			return TransformValue(value);
		}

		private Func<object, TTransformValueProperty> GetTransformValue<TTransformValueProperty>(ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			Func<object, TTransformValueProperty> transformValue = null;

			var type = typeof(TTransformValueProperty);

			if ((jsonSerializer != null) && type.IsInterface && jsonSerializer.GetSerializableInterfaceTypes().Contains(type))
			{
				transformValue = value =>
				{
					if ((jsonSerializer != null) && (value is System.Text.Json.JsonElement jsonElement))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, jsonElement.ToString());
					}

					if ((jsonSerializer != null) && (value is string stringValue))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, stringValue);
					}

					return default;
				};
			}
			else if (type == typeof(string[]))
			{
				transformValue = value =>
				{
					if ((jsonSerializer != null) && (value is System.Text.Json.JsonElement jsonElement))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, jsonElement.ToString());
					}

					if ((jsonSerializer != null) && (value is string stringValue))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, stringValue);
					}

					var values = new List<string>();

					if (value is System.Collections.IEnumerable enumerable)
					{
						foreach (var item in enumerable)
						{
							values.Add(string.Format("{0}", item));
						}
					}
					else
					{
						values.Add(string.Format("{0}", value));
					}

					return (TTransformValueProperty)(object)(values.ToArray());
				};
			}
			else if (type == typeof(string))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue : (TTransformValueProperty)(object)string.Format("{0}", value));
			}
			else if (type == typeof(Guid))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToGuid() : (value is Guid @guid ? (TTransformValueProperty)(object)@guid : (TTransformValueProperty)(object)string.Format("{0}", value).ToGuid()));
			}
			else if (type == typeof(Guid?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToGuidNullable() : (value is Guid? ? (TTransformValueProperty)(object)(Guid?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToGuidNullable()));
			}
			else if (type == typeof(int))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToInt() : (value is int @int ? (TTransformValueProperty)(object)@int : (TTransformValueProperty)(object)string.Format("{0}", value).ToInt()));
			}
			else if (type == typeof(int?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToIntNullable() : (value is int? ? (TTransformValueProperty)(object)(int?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToIntNullable()));
			}
			else if (type == typeof(long))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToLong() : (value is long @long ? (TTransformValueProperty)(object)@long : (TTransformValueProperty)(object)string.Format("{0}", value).ToLong()));
			}
			else if (type == typeof(long?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToLongNullable() : (value is long? ? (TTransformValueProperty)(object)(long?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToLongNullable()));
			}
			else if (type == typeof(double))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDouble() : (value is double @double ? (TTransformValueProperty)(object)@double : (TTransformValueProperty)(object)string.Format("{0}", value).ToDouble()));
			}
			else if (type == typeof(double?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDoubleNullable() : (value is double? ? (TTransformValueProperty)(object)(double?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToDoubleNullable()));
			}
			else if (type == typeof(decimal))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDecimal() : (value is decimal @decimal ? (TTransformValueProperty)(object)@decimal : (TTransformValueProperty)(object)string.Format("{0}", value).ToDecimal()));
			}
			else if (type == typeof(decimal?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDecimalNullable() : (value is decimal? ? (TTransformValueProperty)(object)(decimal?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToDecimalNullable()));
			}
			else if (type == typeof(bool))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToBoolean() : (value is bool @bool ? (TTransformValueProperty)(object)@bool : (TTransformValueProperty)(object)string.Format("{0}", value).ToBoolean()));
			}
			else if (type == typeof(bool?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToBooleanNullable() : (value is bool? ? (TTransformValueProperty)(object)(bool?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToBooleanNullable()));
			}
			else if (type == typeof(DateTime))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDateTime() : (value is DateTime @DateTime ? (TTransformValueProperty)(object)@DateTime : (TTransformValueProperty)(object)string.Format("{0}", value).ToDateTime()));
			}
			else if (type == typeof(DateTime?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToDateTimeNullable() : (value is DateTime? ? (TTransformValueProperty)(object)(DateTime?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToDateTimeNullable()));
			}
			else if (type == typeof(TimeSpan))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToTimeSpan() : (value is TimeSpan @TimeSpan ? (TTransformValueProperty)(object)@TimeSpan : (TTransformValueProperty)(object)string.Format("{0}", value).ToTimeSpan()));
			}
			else if (type == typeof(TimeSpan?))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)stringValue.ToTimeSpanNullable() : (value is TimeSpan? ? (TTransformValueProperty)(object)(TimeSpan?)value : (TTransformValueProperty)(object)string.Format("{0}", value).ToTimeSpanNullable()));
			}
			else if (ISI.Extensions.Enum.IsEnum(type))
			{
				transformValue = value => (value is string stringValue ? (TTransformValueProperty)(object)(ISI.Extensions.Enum.Parse(type, stringValue)) : (TTransformValueProperty)(object)(ISI.Extensions.Enum.Parse(type, string.Format("{0}", value))));
			}
			else if (type.IsClass && (jsonSerializer != null) && IsElementTypeDeserializable(jsonSerializer, type))
			{
				transformValue = value =>
				{
					if (value is System.Text.Json.JsonElement jsonElement)
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, jsonElement.ToString());
					}

					if (value is string stringValue)
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, stringValue);
					}

					if (string.Equals(value.GetType().FullName, "Newtonsoft.Json.Linq.JArray", StringComparison.InvariantCultureIgnoreCase) ||
							string.Equals(value.GetType().FullName, "Newtonsoft.Json.Linq.JObject", StringComparison.InvariantCultureIgnoreCase))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, value.ToString());
					}

					return default;
				};
			}
			else if ((jsonSerializer != null) && TryGetElementTypeFromEnumerableType(type, out var elementType) && IsElementTypeDeserializable(jsonSerializer, elementType))
			{
				transformValue = value =>
				{
					if (value is string stringValue)
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, stringValue);
					}

					if (value is System.Text.Json.JsonElement jsonElement)
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, jsonElement.ToString());
					}

					if (string.Equals(value.GetType().FullName, "Newtonsoft.Json.Linq.JArray", StringComparison.InvariantCultureIgnoreCase) ||
							string.Equals(value.GetType().FullName, "Newtonsoft.Json.Linq.JObject", StringComparison.InvariantCultureIgnoreCase))
					{
						return (TTransformValueProperty)jsonSerializer.Deserialize(type, value.ToString());
					}

					return default;
				};
			}

			return transformValue;
		}

		private bool IsElementTypeDeserializable(ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer, Type elementType)
		{
			if (elementType.GetCustomAttribute<System.Runtime.Serialization.DataContractAttribute>() != null)
			{
				return true;
			}

			if (jsonSerializer.GetSerializableInterfaceTypes().Contains(elementType))
			{
				return true;
			}

			return false;
		}

		private bool TryGetElementTypeFromEnumerableType(Type type, out Type elementType)
		{
			elementType = type.GetInterfaces()
				.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				.Select(t => t.GetGenericArguments().NullCheckedFirstOrDefault()).NullCheckedFirstOrDefault();

			return (elementType != null);
		}

		string IColumn<TRecord>.FormattedValue(TRecord record)
		{
			return FormattedValue(record);
		}
	}

	public class Column : IColumn
	{
		public string ColumnName { get; }
		public Func<object, bool> IsNull { get; }
		public Func<object, object> GetValue { get; }

		public Column(
			string columnName)
			: this(columnName, record => true, record => null)
		{
		}

		public Column(
			ISI.Extensions.DataContract.DataMemberPropertyInfo property)
		{
			ColumnName = property.PropertyName;

			IsNull = record => (property.PropertyInfo.GetValue(record) == null);

			var propertyValueGetterSetterAttribute = property.PropertyInfo.GetCustomAttribute<ISI.Extensions.Extensions.Converters.PropertyValueGetterSetterAttribute>(true);

			if (propertyValueGetterSetterAttribute != null)
			{
				GetValue = propertyValueGetterSetterAttribute.GetPropertyValue;
			}
			else
			{
				GetValue = property.PropertyInfo.GetValue;
			}
		}

		public Column(
			string columnName,
			Func<object, bool> isNull,
			Func<object, object> getValue)
		{
			ColumnName = columnName;
			IsNull = isNull;
			GetValue = getValue;
		}

		object IColumn.GetValue(object record) => GetValue(record);
	}
}
