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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual IWhereClause NewWhereClause()
		{
			return new WhereClause();
		}

		protected virtual IWhereClause GenerateWhereClause(IRecordWhereColumnCollection<TRecord> filters, string filterValueNamePrefix = "")
		{
			var whereClause = NewWhereClause() as WhereClause;

			if (filters.NullCheckedAny())
			{
				var filterIndex = 1;
				whereClause.Sql = GenerateTopLevelWhereClauseFilters(whereClause, filters, ref filterIndex, "      ", filterValueNamePrefix);
			}

			return whereClause;
		}

		protected virtual string GenerateTopLevelWhereClauseFilters(IWhereClause whereClause, IRecordWhereColumnCollection<TRecord> filters, ref int filterIndex, string indent, string filterValueNamePrefix = "")
		{
			return GenerateWhereClauseFilters(whereClause, filters, ref filterIndex, indent, filterValueNamePrefix);
		}

		protected virtual string GenerateWhereClauseFilters(IWhereClause whereClause, IRecordWhereColumnCollection<TRecord> filters, ref int filterIndex, string indent, string filterValueNamePrefix = "")
		{
			var sqlFilters = new List<string>();

			foreach (var filter in filters)
			{
				if (filter is RecordWhereColumn<TRecord> recordWhereColumnFilter)
				{
					sqlFilters.AddRange(GenerateWhereClauseFilter(whereClause, recordWhereColumnFilter, ref filterIndex, indent, filterValueNamePrefix));
				}
				else if (filter is IRecordWhereColumnCollection<TRecord> recordWhereColumnFilters)
				{
					sqlFilters.Add(GenerateWhereClauseFilters(whereClause, recordWhereColumnFilters, ref filterIndex, string.Format("{0}  ", indent), filterValueNamePrefix));
				}
			}

			var @operator = filters.WhereClauseOperator == WhereClauseOperator.And ? " AND\n" : " OR\n";

			return string.Format("{0}({1})\n", indent, string.Join(@operator, sqlFilters).Trim());
		}

		protected virtual IList<string> GenerateWhereClauseFilter(IWhereClause whereClause, RecordWhereColumn<TRecord> filter, ref int filterIndex, string indent, string filterValueNamePrefix = "")
		{
			var sqlFilters = new List<string>();

			var columnName = filter.RecordPropertyDescription.ColumnName;

			if (filter.NullOperator.HasValue)
			{
				switch (filter.NullOperator.Value)
				{
					case WhereClauseNullOperator.IsNull:
						sqlFilters.Add(string.Format("{0}({1} IS NULL)", indent, FormatColumnName(columnName)));
						break;
					case WhereClauseNullOperator.IsNotNull:
						sqlFilters.Add(string.Format("{0}(NOT {1} IS NULL)", indent, FormatColumnName(columnName)));
						break;
				}
			}
			else if (filter.IsBetween)
			{
				if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
				{
					throw new("Where clause must implement IWhereClauseWithParameters");
				}

				var filterLesserBetweenValueName = string.Format("@{0}Lesser{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex);
				whereClauseWithParameters.Parameters.Add(filterLesserBetweenValueName, filter.LesserBetweenValue);

				var filterGreaterBetweenValueName = string.Format("@{0}Greater{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex);
				whereClauseWithParameters.Parameters.Add(filterGreaterBetweenValueName, filter.GreaterBetweenValue);

				sqlFilters.Add(string.Format("{0}({1} BETWEEN {2} AND {3})", indent, FormatColumnName(columnName), filterLesserBetweenValueName, filterGreaterBetweenValueName));

				filterIndex++;
			}
			else if (filter.StringComparisonOperator.HasValue)
			{
				if (!(filter.Values.NullCheckedFirstOrDefault() is string value))
				{
					sqlFilters.Add(string.Format("{0}({1} IS NULL)", indent, FormatColumnName(columnName)));
				}
				else
				{
					if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
					{
						throw new("Where clause must implement IWhereClauseWithParameters");
					}

					value = value.Trim();

					var filterValueName = string.Format("@{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex++);

					if (filter.StringComparisonOperator == WhereClauseStringComparisonOperator.Wildcards)
					{
						filter.StringComparisonOperator = WhereClauseStringComparisonOperator.Equal;

						var beginsWith = value.EndsWith("*");
						if (beginsWith)
						{
							value = value.TrimEnd('*').Trim();
						}

						var endsWith = value.StartsWith("*");
						if (endsWith)
						{
							value = value.TrimStart('*').Trim();
						}

						if (beginsWith && endsWith)
						{
							filter.StringComparisonOperator = WhereClauseStringComparisonOperator.Contains;
						}
						else if (beginsWith)
						{
							filter.StringComparisonOperator = WhereClauseStringComparisonOperator.BeginsWith;
						}
						else if (endsWith)
						{
							filter.StringComparisonOperator = WhereClauseStringComparisonOperator.EndsWith;
						}
					}

					switch (filter.StringComparisonOperator)
					{
						case WhereClauseStringComparisonOperator.BeginsWith:
							whereClauseWithParameters.Parameters.Add(filterValueName, string.Format("{0}%", value));
							sqlFilters.Add(string.Format("{0}({1} LIKE {2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.Equal:
							whereClauseWithParameters.Parameters.Add(filterValueName, value);
							sqlFilters.Add(string.Format("{0}({1} = {2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.Contains:
							whereClauseWithParameters.Parameters.Add(filterValueName, string.Format("%{0}%", value));
							sqlFilters.Add(string.Format("{0}({1} LIKE {2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.EndsWith:
							whereClauseWithParameters.Parameters.Add(filterValueName, string.Format("%{0}", value));
							sqlFilters.Add(string.Format("{0}({1} LIKE {2})", indent, FormatColumnName(columnName), filterValueName));
							break;
					}
				}
			}
			else if (filter.ComparisonOperator.HasValue)
			{
				var value = filter.Values.NullCheckedFirstOrDefault();

				if (value == null)
				{
					throw new ArgumentOutOfRangeException();
				}

				if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
				{
					throw new("Where clause must implement IWhereClauseWithParameters");
				}

				var filterValueName = string.Format("@{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex++);

				whereClauseWithParameters.Parameters.Add(filterValueName, value);

				switch (filter.ComparisonOperator)
				{
					case WhereClauseComparisonOperator.LessThan:
						sqlFilters.Add(string.Format("{0}({1} < {2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.LessThanOrEqual:
						sqlFilters.Add(string.Format("{0}({1} <= {2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.GreaterThanOrEqual:
						sqlFilters.Add(string.Format("{0}({1} >= {2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.GreaterThan:
						sqlFilters.Add(string.Format("{0}({1} > {2})", indent, FormatColumnName(columnName), filterValueName));
						break;
				}
			}
			else if (filter.EqualityOperator.HasValue)
			{
				GenerateWhereClauseFilter_EqualityOperator(whereClause, filter, ref filterIndex, sqlFilters, indent, filterValueNamePrefix);
			}
			else
			{
				throw new("No Operator specified");
			}

			return sqlFilters;
		}

		public class JoinTableValues : IEnumerable<RecordWhereColumnCollection<TRecord>>, ISI.Extensions.Columns.IGetColumns<RecordWhereColumnCollection<TRecord>>
		{
			public class Column : ISI.Extensions.Columns.IColumn<RecordWhereColumnCollection<TRecord>>
			{
				public Type PropertyType { get; }
				public string ColumnName { get; }
				public string[] ColumnNames { get; }
				public Func<RecordWhereColumnCollection<TRecord>, object> GetValue { get; }
				public Func<RecordWhereColumnCollection<TRecord>, bool> IsNull { get; }
				public Func<RecordWhereColumnCollection<TRecord>, string> FormattedValue { get; }

				public Column(RecordWhereColumn<TRecord> recordWhereColumn)
				{
					PropertyType = recordWhereColumn.RecordPropertyDescription.ValueType;
					ColumnName = recordWhereColumn.RecordPropertyDescription.ColumnName;
					ColumnNames = [ColumnName];

					GetValue = record => ((record.FirstOrDefault(column => string.Equals((column as RecordWhereColumn<TRecord>)?.RecordPropertyDescription?.ColumnName ?? string.Empty, ColumnName, StringComparison.CurrentCulture)) as RecordWhereColumn<TRecord>)?.Values).NullCheckedFirstOrDefault();

					var isNullable = (PropertyType.IsGenericType && (PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)));

					IsNull = record =>
					{
						if (isNullable)
						{
							return (GetValue(record) == null);
						}

						return false;
					};

					FormattedValue = record => ($"{GetValue(record)}");
				}

				Func<object, bool> ISI.Extensions.Columns.IColumn.IsNull => record => IsNull(record as RecordWhereColumnCollection<TRecord>);

				object ISI.Extensions.Columns.IColumn.GetValue(object record) => GetValue(record as RecordWhereColumnCollection<TRecord>);

				object ISI.Extensions.Columns.IColumn<RecordWhereColumnCollection<TRecord>>.GetValue(RecordWhereColumnCollection<TRecord> record) => GetValue(record);

				void ISI.Extensions.Columns.IColumn<RecordWhereColumnCollection<TRecord>>.SetValue(RecordWhereColumnCollection<TRecord> record, object value)
				{
				}

				object ISI.Extensions.Columns.IColumn<RecordWhereColumnCollection<TRecord>>.TransformValue(object value) => value;

				string ISI.Extensions.Columns.IColumn<RecordWhereColumnCollection<TRecord>>.FormattedValue(RecordWhereColumnCollection<TRecord> record) => FormattedValue(record);
			}

			private readonly RecordWhereColumnCollection<TRecord>[] _recordWhereColumnFilters;
			private readonly ISI.Extensions.Columns.ColumnCollection<RecordWhereColumnCollection<TRecord>> _columns;

			public JoinTableValues()
			{

			}

			public JoinTableValues(RecordWhereColumnCollection<TRecord> recordWhereColumnFilters)
			{
				_recordWhereColumnFilters = recordWhereColumnFilters.ToNullCheckedArray(recordWhereColumnFilter => recordWhereColumnFilter as RecordWhereColumnCollection<TRecord>);
				_columns = new ISI.Extensions.Columns.ColumnCollection<RecordWhereColumnCollection<TRecord>>((_recordWhereColumnFilters.First() as RecordWhereColumnCollection<TRecord>).Select(recordWhereColumnFilter => new Column(recordWhereColumnFilter as RecordWhereColumn<TRecord>)));
			}

			public ISI.Extensions.Columns.IColumnCollection<RecordWhereColumnCollection<TRecord>> GetColumns() => _columns;

			public IEnumerator<RecordWhereColumnCollection<TRecord>> GetEnumerator()
			{
				return _recordWhereColumnFilters.ToList().GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return _recordWhereColumnFilters.ToList().GetEnumerator();
			}
		}

		protected virtual void GenerateWhereClauseFilter_EqualityOperator(IWhereClause whereClause, RecordWhereColumn<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			if (filter.Values.NullCheckedAny())
			{
				var columnName = filter.RecordPropertyDescription.ColumnName;

				var filterValueNames = new List<string>();

				foreach (var value in filter.Values)
				{
					if (value != null)
					{
						var filterValueName = string.Format("@{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex++);

						filterValueNames.Add(filterValueName);

						if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
						{
							throw new("Where clause must implement IWhereClauseWithParameters");
						}

						whereClauseWithParameters.Parameters.Add(filterValueName, value);
					}
				}

				if (filterValueNames.Count == 0)
				{
					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} IS NULL)", indent, FormatColumnName(columnName)));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(NOT {1} IS NULL)", indent, FormatColumnName(columnName)));
							break;
					}
				}
				else if (filterValueNames.Count == 1)
				{
					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} = {2})", indent, FormatColumnName(columnName), filterValueNames.First()));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}({1} != {2})", indent, FormatColumnName(columnName), filterValueNames.First()));
							break;
					}
				}
				else
				{
					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterValueNames)));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(NOT {1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterValueNames)));
							break;
					}
				}
			}
		}
	}
}