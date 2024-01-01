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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository
{
	public class RecordWhereColumnCollection<TRecord> : List<IRecordWhereColumn<TRecord>>, IRecordWhereColumn<TRecord>, IRecordWhereColumnCollection<TRecord>
	{
		public WhereClauseOperator WhereClauseOperator { get; set; }

		public RecordWhereColumnCollection()
		 : this(WhereClauseOperator.And)
		{

		}
		public RecordWhereColumnCollection(WhereClauseOperator whereClauseOperator)
		{
			WhereClauseOperator = whereClauseOperator;
		}
		public RecordWhereColumnCollection(IEnumerable<IRecordWhereColumn<TRecord>> recordWhereColumns)
			: this(recordWhereColumns, WhereClauseOperator.And)
		{

		}
		public RecordWhereColumnCollection(IEnumerable<IRecordWhereColumn<TRecord>> recordWhereColumns, WhereClauseOperator whereClauseOperator)
			: base(recordWhereColumns)
		{
			WhereClauseOperator = whereClauseOperator;
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseNullOperator nullOperator)
		{
			Add<TProperty>(property, null, null, nullOperator, null, false, null, null);
		}


		public void Add(Expression<Func<TRecord, bool>> property)
		{
			Add(property, WhereClauseEqualityOperator.NotEqual, false);
		}

		public void Add(Expression<Func<TRecord, bool>> property, bool value)
		{
			Add(property, value ? WhereClauseEqualityOperator.NotEqual : WhereClauseEqualityOperator.Equal, false);
		}

		public void Add(Expression<Func<TRecord, int>> property, bool value)
		{
			Add(property, value ? WhereClauseEqualityOperator.NotEqual : WhereClauseEqualityOperator.Equal, 0);
		}

		public void Add(Expression<Func<TRecord, string>> property, WhereClauseStringComparisonOperator comparisonOperator, string value)
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			Add(new RecordWhereColumn<TRecord>()
			{
				RecordPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptionLookup[propertyInfo.Name],
				StringComparisonOperator = comparisonOperator,
				Values = new[] { value },
			});
		}

		public void Add<TProperty, TEntity>(Expression<Func<TRecord, TProperty>> property, WhereClauseStringComparisonOperator comparisonOperator, string value)
			where TProperty : ISI.Extensions.Converters.IExportTo<TEntity>
			where TEntity : class
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			Add(new RecordWhereColumn<TRecord>()
			{
				RecordPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptionLookup[propertyInfo.Name],
				StringComparisonOperator = comparisonOperator,
				Values = new[] { value },
			});
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseComparisonOperator comparisonOperator, TProperty value)
		{
			Add<TProperty>(property, comparisonOperator, null, null, new[] { value }, false, null, null);
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseEqualityOperator equalityOperator, IEnumerable<TProperty> values)
		{
			Add<TProperty>(property, null, equalityOperator, null, values, false, null, null);
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty?>> property, WhereClauseEqualityOperator equalityOperator, IEnumerable<TProperty> values)
			where TProperty : struct
		{
			Add<TProperty>(property, null, equalityOperator, null, values, false, null, null);
		}

		public void Add(Expression<Func<TRecord, string>> property, WhereClauseStringComparisonOperator comparisonOperator, IEnumerable<string> values)
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			var filters = new RecordWhereColumnCollection<TRecord>(WhereClauseOperator.Or);

			foreach (var value in values)
			{
				filters.Add(new RecordWhereColumn<TRecord>()
				{
					RecordPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptionLookup[propertyInfo.Name],
					StringComparisonOperator = comparisonOperator,
					Values = new[] { value },
				});
			}

			if (filters.Any())
			{
				Add(filters);
			}
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseEqualityOperator equalityOperator, TProperty value)
		{
			Add<TProperty>(property, null, equalityOperator, null, new[] { value }, false, null, null);
		}

		//Note: Between operator is totally for code readability
		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseBetweenOperator betweenOperator, TProperty lesserBetweenValue, TProperty greaterBetweenValue)
		{
			Add<TProperty>(property, null, null, null, null, true, lesserBetweenValue, greaterBetweenValue);
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseEqualityOperator equalityOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				Add<TProperty>(property, null, equalityOperator, null, new[] { value.Value }, false, null, null);
			}
			else
			{
				Add<TProperty>(property, null, null, equalityOperator == WhereClauseEqualityOperator.Equal ? WhereClauseNullOperator.IsNull : WhereClauseNullOperator.IsNotNull, null, false, null, null);
			}
		}

		public void Add<TProperty>(Expression<Func<TRecord, TProperty?>> property, WhereClauseEqualityOperator equalityOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				Add<TProperty>(property, null, equalityOperator, null, new[] { value.Value }, false, null, null);
			}
			else
			{
				Add<TProperty>(property, null, null, equalityOperator == WhereClauseEqualityOperator.Equal ? WhereClauseNullOperator.IsNull : WhereClauseNullOperator.IsNotNull, null, false, null, null);
			}
		}


		private void Add<TProperty>(Expression<Func<TRecord, TProperty>> property, WhereClauseComparisonOperator? comparisonOperator, WhereClauseEqualityOperator? equalityOperator, WhereClauseNullOperator? nullOperator, IEnumerable<TProperty> values, bool isBetween, object lesserBetweenValue, object greaterBetweenValue)
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			Add(new RecordWhereColumn<TRecord>()
			{
				RecordPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptionLookup[propertyInfo.Name],
				ComparisonOperator = comparisonOperator,
				EqualityOperator = equalityOperator,
				NullOperator = nullOperator,
				Values = values.NullCheckedSelect(value => (object)value),
				IsBetween = isBetween,
				LesserBetweenValue = lesserBetweenValue,
				GreaterBetweenValue = greaterBetweenValue,
			});
		}

		private void Add<TProperty>(Expression<Func<TRecord, TProperty?>> property, WhereClauseComparisonOperator? comparisonOperator, WhereClauseEqualityOperator? equalityOperator, WhereClauseNullOperator? nullOperator, IEnumerable<TProperty> values, bool isBetween, object lesserBetweenValue, object greaterBetweenValue)
			where TProperty : struct
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			Add(new RecordWhereColumn<TRecord>()
			{
				RecordPropertyDescription = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptionLookup[propertyInfo.Name],
				ComparisonOperator = comparisonOperator,
				EqualityOperator = equalityOperator,
				NullOperator = nullOperator,
				Values = values.NullCheckedSelect(value => (object)value),
				IsBetween = isBetween,
				LesserBetweenValue = lesserBetweenValue,
				GreaterBetweenValue = greaterBetweenValue,
			});
		}
	}
}
