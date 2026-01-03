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
using ISI.Extensions.Repository.Extensions;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public delegate TRecord Reader<out TRecord>(System.Data.Common.DbDataReader dataReader);

	public class ExpressionBuilder
	{
		public static Action<System.Data.Common.DbDataReader, ISI.Extensions.JsonSerialization.IJsonSerializer, TRecord> GetBuilder<TRecord>(System.Data.Common.DbDataReader dbDataReader, IEnumerable<IRecordPropertyDescription<TRecord>> properties)
		{
			var constructor = typeof(TRecord).GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);

			if (constructor == null)
			{
				throw new("Needs a parameterless constructor");
			}

			var dbDataReaderExtensionsType = typeof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions);
			var getBoolean = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetBoolean), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getBooleanNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetBooleanNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getShort = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetShort), [typeof(System.Data.Common.DbDataReader), typeof(short)]);
			var getShortNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetShortNullable), [typeof(System.Data.Common.DbDataReader), typeof(short)]);
			var getInt = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetInt), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getIntNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetIntNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getLong = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetLong), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getLongNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetLongNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDouble = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDouble), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDoubleNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDoubleNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDecimal = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDecimal), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDecimalNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDecimalNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getFloat = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetFloat), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getFloatNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetFloatNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDateTime = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDateTime), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getDateTimeNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetDateTimeNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getTimeSpan = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetTimeSpan), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getTimeSpanNullable = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetTimeSpanNullable), [typeof(System.Data.Common.DbDataReader), typeof(int)]);
			var getSerialized = dbDataReaderExtensionsType.GetMethod(nameof(ISI.Extensions.Repository.Extensions.DataReaderByOrdinalExtensions.GetSerialized), [typeof(System.Data.Common.DbDataReader), typeof(ISI.Extensions.JsonSerialization.IJsonSerializer), typeof(Type), typeof(int)]);

			var dataReader = System.Linq.Expressions.Expression.Parameter(typeof(System.Data.Common.DbDataReader), "dataReader");
			var serializer = System.Linq.Expressions.Expression.Parameter(typeof(ISI.Extensions.JsonSerialization.IJsonSerializer), "serializer");
			var record = System.Linq.Expressions.Expression.Parameter(typeof(TRecord), "record");

			var expressions = new List<System.Linq.Expressions.Expression>();

			foreach (var property in properties.Where(property => property.PropertyInfo.CanWrite))
			{
				var columnIndex = System.Linq.Expressions.Expression.Constant(dbDataReader.GetOrdinal(property.ColumnName), typeof(int));

				var basePropertyType = property.ValueType;

				var isNullable = (basePropertyType.IsGenericType && (basePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)));

				if (isNullable)
				{
					basePropertyType = (new System.ComponentModel.NullableConverter(basePropertyType)).UnderlyingType;
				}

				System.Linq.Expressions.Expression assignExpression = null;

				if (basePropertyType == typeof(bool))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getBooleanNullable : getBoolean, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(short))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getShortNullable : getShort, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(int))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getIntNullable : getInt, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(long))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getLongNullable : getLong, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(double))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getDoubleNullable : getDouble, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(decimal))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getDecimalNullable : getDecimal, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(float))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getFloatNullable : getFloat, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(DateTime))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getDateTimeNullable : getDateTime, dataReader, columnIndex));
				}
				else if (basePropertyType == typeof(TimeSpan))
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(isNullable ? getTimeSpanNullable : getTimeSpan, dataReader, columnIndex));
				}
				else if (property.CanBeSerialized)
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Call(getSerialized, dataReader, serializer, System.Linq.Expressions.Expression.Constant(property.ValueType, typeof(Type)), columnIndex), property.ValueType));
				}
				else
				{
					assignExpression = System.Linq.Expressions.Expression.Assign(System.Linq.Expressions.Expression.PropertyOrField(record, property.PropertyName), System.Linq.Expressions.Expression.Call(dataReader, nameof(System.Data.Common.DbDataReader.GetFieldValue), [property.ValueType], columnIndex));
				}

				var testExpression = System.Linq.Expressions.Expression.IsFalse(System.Linq.Expressions.Expression.Call(dataReader, nameof(System.Data.Common.DbDataReader.IsDBNull), [], columnIndex));

				expressions.Add(System.Linq.Expressions.Expression.IfThen(testExpression, assignExpression));
			}

			return System.Linq.Expressions.Expression.Lambda<Action<System.Data.Common.DbDataReader, ISI.Extensions.JsonSerialization.IJsonSerializer, TRecord>>(System.Linq.Expressions.Expression.Block(expressions), new[] { dataReader, serializer, record }).Compile();
		}

		public static Reader<TRecord> GetReader<TRecord>(System.Data.Common.DbDataReader dbDataReader, IEnumerable<IRecordPropertyDescription<TRecord>> properties, ISI.Extensions.JsonSerialization.IJsonSerializer serializer)
		{
			var buildUp = GetBuilder(dbDataReader, properties);

			return dataReader =>
			{
				var value = Activator.CreateInstance<TRecord>();

				buildUp(dataReader, serializer, value);

				return value;
			};
		}
	}
}
