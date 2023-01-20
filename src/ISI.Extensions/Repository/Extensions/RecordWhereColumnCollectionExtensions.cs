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
	public static partial class RecordWhereColumnCollectionExtensions
	{
		public static void AddIfNotNull<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, DateTime>> property, WhereClauseComparisonOperator comparisonOperator, DateTime? value)
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add(property, comparisonOperator, value.Value);
			}
		}

		public static void AddIfNotNull<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, DateTime?>> property, WhereClauseComparisonOperator comparisonOperator, DateTime? value)
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add(property, comparisonOperator, value.Value);
			}
		}

		public static void AddIfNotNull<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, int>> property, bool? value)
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add(property, value.Value);
			}
		}

		public static void AddIfNotNull<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, DateTime?>> property, DateTime? startDateTime, DateTime? stopDateTime)
		{
			if (startDateTime.HasValue && stopDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseBetweenOperator.Between, startDateTime.Value, stopDateTime.Value);
			}
			else if (startDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseComparisonOperator.GreaterThanOrEqual, startDateTime.Value);
			}
			else if (stopDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseComparisonOperator.LessThanOrEqual, stopDateTime.Value);
			}
		}

		public static void AddArchiveDateTimeRange<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, IRecordPropertyDescription<TRecord> recordDescription, DateTime? startDateTime, DateTime? stopDateTime)
			where TRecord : ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime
		{
			if (startDateTime.HasValue && stopDateTime.HasValue)
			{
				recordWhereColumns.Add(new RecordWhereColumn<TRecord>()
				{
					RecordPropertyDescription = recordDescription,
					IsBetween = true,
					LesserBetweenValue = startDateTime.Value,
					GreaterBetweenValue = stopDateTime.Value,
				});
			}
			else if (startDateTime.HasValue)
			{
				recordWhereColumns.Add(new RecordWhereColumn<TRecord>()
				{
					RecordPropertyDescription = recordDescription,
					ComparisonOperator = WhereClauseComparisonOperator.GreaterThanOrEqual,
					Values = new[] { (object)startDateTime.Value },
				});
			}
			else if (stopDateTime.HasValue)
			{
				recordWhereColumns.Add(new RecordWhereColumn<TRecord>()
				{
					RecordPropertyDescription = recordDescription,
					ComparisonOperator = WhereClauseComparisonOperator.LessThanOrEqual,
					Values = new[] { (object)stopDateTime.Value },
				});
			}
		}

		public static void AddIfNotNull<TRecord>(this IRecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, DateTime>> property, DateTime? startDateTime, DateTime? stopDateTime)
		{
			if (startDateTime.HasValue && stopDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseBetweenOperator.Between, startDateTime.Value, stopDateTime.Value);
			}
			else if (startDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseComparisonOperator.GreaterThanOrEqual, startDateTime.Value);
			}
			else if (stopDateTime.HasValue)
			{
				recordWhereColumns.Add(property, WhereClauseComparisonOperator.LessThanOrEqual, stopDateTime.Value);
			}
		}

		public static void AddIfNotNull<TRecord>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, string>> property, WhereClauseStringComparisonOperator whereClauseStringComparisonOperator, string value)
		{
			if (value != null)
			{
				recordWhereColumns.Add(property, whereClauseStringComparisonOperator, value);
			}
		}

		public static void AddIfNotNullOrEmpty<TRecord>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, string>> property, WhereClauseStringComparisonOperator whereClauseStringComparisonOperator, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				recordWhereColumns.Add(property, whereClauseStringComparisonOperator, value);
			}
		}

		public static void AddIfNullCheckedAny<TRecord,TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, WhereClauseEqualityOperator equalityOperator, IEnumerable<TProperty> values)
		{
			if (values.NullCheckedAny())
			{
				recordWhereColumns.Add<TProperty>(property,  equalityOperator, values);
			}
		}

		public static void AddIfNullCheckedAny<TRecord, TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty?>> property, WhereClauseEqualityOperator equalityOperator, IEnumerable<TProperty> values)
			where TProperty : struct
		{
			if (values.NullCheckedAny())
			{
				recordWhereColumns.Add<TProperty>(property, equalityOperator, values);
			}
		}

		public static void AddIfNullCheckedAny<TRecord>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, string>> property, WhereClauseStringComparisonOperator comparisonOperator, IEnumerable<string> values)
		{
			if (values.NullCheckedAny())
			{
				recordWhereColumns.Add(property, comparisonOperator, values);
			}
		}

		public static void AddIfNotNull<TRecord, TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, WhereClauseEqualityOperator equalityOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add<TProperty>(property, equalityOperator, new[] { value.Value });
			}
		}

		public static void AddIfNotNull<TRecord, TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty?>> property, WhereClauseEqualityOperator equalityOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add<TProperty>(property, equalityOperator, new[] { value.Value });
			}
		}

		public static void AddIfNotNull<TRecord, TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, WhereClauseComparisonOperator comparisonOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add<TProperty>(property, comparisonOperator, value.Value);
			}
		}

		public static void AddIfNotNull<TRecord, TProperty>(this RecordWhereColumnCollection<TRecord> recordWhereColumns, System.Linq.Expressions.Expression<Func<TRecord, TProperty?>> property, WhereClauseComparisonOperator comparisonOperator, TProperty? value)
			where TProperty : struct
		{
			if (value.HasValue)
			{
				recordWhereColumns.Add<TProperty?>(property, comparisonOperator, value);
			}
		}
	}
}
