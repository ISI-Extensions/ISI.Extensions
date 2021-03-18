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

namespace ISI.Extensions.DataReader
{
	public partial class EnumerableDataReader<TRecord>
	{
		public class ColumnInfo<TProperty> : IColumnInfo
		{
			public string ColumnName { get; }
			public Func<TRecord, TProperty> GetValue { get; }
			public Func<TRecord, bool> IsNull { get; }

			public ColumnInfo(
				string columnName,
				System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
			{
				var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);
				ColumnName = (string.IsNullOrEmpty(columnName) ? propertyInfo.Name : columnName);
				var columnType = typeof(TProperty);
				var isNullable = (columnType.IsGenericType && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)));
				GetValue = property.Compile();

				IsNull = record =>
				{
					if (isNullable)
					{
						return (GetValue(record) == null);
					}

					return false;
				};
			}

			public ColumnInfo(
				string columnName,
				TProperty value)
			{
				ColumnName = columnName;
				var columnType = typeof(TProperty);
				var isNullable = (columnType.IsGenericType && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)));
				GetValue = record => value;

				IsNull = record =>
				{
					if (isNullable)
					{
						return (GetValue(record) == null);
					}

					return false;
				};
			}

			Type IColumnInfo.PropertyType => typeof(TProperty);

			object IColumnInfo.GetValue(TRecord record)
			{
				return GetValue(record);
			}
		}

		public class ColumnInfo : IColumnInfo
		{
			public string ColumnName { get; }
			public Func<TRecord, bool> IsNull { get; }
			public Func<TRecord, object> GetValue { get; }

			public ColumnInfo(
				string columnName)
				: this(columnName, record => true, record => null)
			{
			}

			public ColumnInfo(
				string columnName,
				Func<TRecord, bool> isNull,
				Func<TRecord, object> getValue)
			{
				ColumnName = columnName;
				IsNull = isNull;
				GetValue = getValue;
			}

			Type IColumnInfo.PropertyType => typeof(object);

			object IColumnInfo.GetValue(TRecord record)
			{
				return GetValue(record);
			}
		}
	}
}