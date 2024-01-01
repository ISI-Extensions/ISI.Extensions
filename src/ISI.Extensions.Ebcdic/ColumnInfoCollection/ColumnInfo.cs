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
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Ebcdic
{
	public partial class ColumnInfoCollection<TRecord>
	{
		public class ColumnInfo<TProperty> : IColumnInfo
		{
			public ColumnFormat ColumnFormat { get; }
			public int? Scale { get; }
			public int ColumnSize { get; }
			public Type ColumnType { get; }
			public Type BaseColumnType { get; }
			public bool IsNullable { get; }
			public Func<TRecord, TProperty> GetValue { get; }
			public Action<TRecord, TProperty> SetValue { get; }
			public Func<TRecord, bool> IsNull { get; }
			public Func<TProperty, TProperty> TransformValue { get; }

			public ColumnInfo(
				System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property,
				int columnSize,
				ColumnFormat columnFormat,
				int? scale,
				Func<TProperty, TProperty> transformValue)
			{
				ColumnFormat = columnFormat;
				Scale = scale;
				ColumnSize = columnSize;
				var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);
				ColumnType = propertyInfo.PropertyType;
				IsNullable = (ColumnType.IsGenericType && (ColumnType.GetGenericTypeDefinition() == typeof (Nullable<>)));
				BaseColumnType = (IsNullable ? (new System.ComponentModel.NullableConverter(ColumnType)).UnderlyingType : ColumnType);
				GetValue = property.Compile();
				SetValue = (record, value) =>
				{
					if (TransformValue != null)
					{
						value = TransformValue(value);
					}

					propertyInfo.SetValue(record, value);
				};
				IsNull = record =>
				{
					if (IsNullable)
					{
						return (GetValue(record) == null);
					}

					return false;
				};
				TransformValue = transformValue;
			}

			object IColumnInfo.GetValue(TRecord record)
			{
				return GetValue(record);
			}

			void IColumnInfo.SetValue(TRecord record, object value)
			{
				SetValue(record, (TProperty) value);
			}
		}

		public class ColumnInfo : IColumnInfo
		{
			public ColumnFormat ColumnFormat { get; private set; }
			public int? Scale { get; private set; }
			public int ColumnSize { get; }
			public Type ColumnType { get; }
			public Type BaseColumnType { get; }
			public bool IsNullable { get; }
			public Func<TRecord, object> GetValue { get; }
			public Action<TRecord, object> SetValue { get; }
			public Func<TRecord, bool> IsNull { get; }

			public ColumnInfo(
				System.Reflection.PropertyInfo propertyInfo,
				int columnSize)
			{
				ColumnSize = columnSize;
				if (propertyInfo == null)
				{
					ColumnType = null;
					IsNullable = true;
					BaseColumnType = null;
					GetValue = record => null;
					SetValue = (record, value) => { };
					IsNull = record => true;
				}
				else
				{
					ColumnType = propertyInfo.PropertyType;
					IsNullable = (ColumnType.IsGenericType && (ColumnType.GetGenericTypeDefinition() == typeof (Nullable<>)));
					BaseColumnType = (IsNullable ? (new System.ComponentModel.NullableConverter(ColumnType)).UnderlyingType : ColumnType);
					GetValue = record => propertyInfo.GetValue(record);
					SetValue = (record, value) => propertyInfo.SetValue(record, value);
					IsNull = record =>
					{
						if (IsNullable)
						{
							return (GetValue(record) == null);
						}

						return false;
					};
				}
			}

			object IColumnInfo.GetValue(TRecord record)
			{
				return GetValue(record);
			}

			void IColumnInfo.SetValue(TRecord record, object value)
			{
				SetValue(record, value);
			}
		}
	}
}