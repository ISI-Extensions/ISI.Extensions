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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.SpreadSheets
{
	public interface ISetColumnOffset
	{
		int ColumnOffset { set; }
	}

	public interface IAddRecordsColumn<TRecord>
	{
		int ColumnOffset { get; }
		Type ColumnType { get; }
		Func<TRecord, bool> IsNull { get; }
		object GetValue(TRecord record);
		AddRecordsColumnOptions ColumnOptions { get; }
	}

	public class AddRecordsColumn<TRecord> : IAddRecordsColumn<TRecord>, ISetColumnOffset
	{
		public int ColumnOffset { get; private set; }
		public Type ColumnType { get; }
		public Func<TRecord, bool> IsNull { get; }
		public Func<TRecord, object> GetValue { get; }

		public AddRecordsColumnOptions ColumnOptions { get; }

		int ISetColumnOffset.ColumnOffset { set => ColumnOffset = value; }

		public AddRecordsColumn(int columnOffset, Type propertyType, Func<TRecord, object> getValue, AddRecordsColumnOptions columnOptions)
		{
			ColumnOffset = columnOffset;
			ColumnType = propertyType;
			var isNullable = (ColumnType.IsGenericType && (ColumnType.GetGenericTypeDefinition() == typeof(Nullable<>)));

			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};

			GetValue = getValue;

			ColumnOptions = columnOptions ?? new AddRecordsColumnOptions();
		}

		object IAddRecordsColumn<TRecord>.GetValue(TRecord record)
		{
			return GetValue(record);
		}
	}

	public class AddRecordsColumn<TRecord, TProperty> : IAddRecordsColumn<TRecord>, ISetColumnOffset
	{
		public int ColumnOffset { get; private set; }
		public Type ColumnType => typeof(TProperty);
		public Func<TRecord, bool> IsNull { get; }
		public Func<TRecord, TProperty> GetValue { get; }
		public Func<TProperty, object> Transform { get; }
		public string Formula { get; set; }

		public AddRecordsColumnOptions ColumnOptions { get; }

		int ISetColumnOffset.ColumnOffset { set => ColumnOffset = value; }

		public AddRecordsColumn(int columnOffset, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TProperty, object> transform, AddRecordsColumnOptions columnOptions)
		{
			ColumnOffset = columnOffset;
			var columnType = typeof(TProperty);
			var isNullable = (columnType.IsGenericType && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)));

			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};

			GetValue = property.Compile();
			Transform = transform ?? (_ => _);

			var memberExpression = (System.Linq.Expressions.MemberExpression)property.Body;

			ColumnOptions = columnOptions ?? (memberExpression.Member is System.Reflection.PropertyInfo propertyInfo ? AddRecordsColumnOptions.GetAddRecordsColumnOptions(propertyInfo).AddRecordsColumnOptions : new());

			if (string.IsNullOrWhiteSpace(ColumnOptions.ColumnName))
			{
				ColumnOptions.ColumnName = (memberExpression.Member is System.Reflection.PropertyInfo ? memberExpression.Member.Name : ColumnOptions.ColumnName);
			}
		}

		object IAddRecordsColumn<TRecord>.GetValue(TRecord record)
		{
			return Transform(GetValue(record));
		}
	}
}
