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
using ISI.Extensions.Extensions;
using System.Linq.Expressions;
using ISI.Extensions.Oracle.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.Oracle.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.Oracle
{
	public abstract partial class RecordManager<TRecord>
	{
		protected override IWhereClause NewWhereClause()
		{
			return new OracleConnectionWhereClause();
		}

		protected override string GenerateTopLevelWhereClauseFilters(IWhereClause whereClause, IRecordWhereColumnCollection<TRecord> filters, ref int filterIndex, string indent, string filterValueNamePrefix = "")
		{
			var sqlFilters = new List<string>();

			if ((filters.WhereClauseOperator == WhereClauseOperator.Or) && (filters.Count > 10) && filters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>)?.WhereClauseOperator == WhereClauseOperator.And))
			{
				var columnNamesHashCode = (filters.First() as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode();

				if (filters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode() == columnNamesHashCode))
				{
					GenerateJoinTableFilter(whereClause, filters as RecordWhereColumnCollection<TRecord>, ref filterIndex, sqlFilters, string.Format("{0}  ", indent), filterValueNamePrefix);

					return string.Empty;
				}
			}

			foreach (var filter in filters)
			{
				if (filter is RecordWhereColumn<TRecord> recordWhereColumnFilter)
				{
					sqlFilters.AddRange(GenerateWhereClauseFilter(whereClause, recordWhereColumnFilter, ref filterIndex, indent, filterValueNamePrefix));
				}
				else if (filter is IRecordWhereColumnCollection<TRecord> recordWhereColumnFilters)
				{
					var usedTempTable = false;

					if ((filters.WhereClauseOperator == WhereClauseOperator.And) && (recordWhereColumnFilters.WhereClauseOperator == WhereClauseOperator.Or) && (recordWhereColumnFilters.Count > 10) && recordWhereColumnFilters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>)?.WhereClauseOperator == WhereClauseOperator.And))
					{
						var columnNamesHashCode = (recordWhereColumnFilters.First() as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode();

						if (recordWhereColumnFilters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode() == columnNamesHashCode))
						{
							GenerateJoinTableFilter(whereClause, recordWhereColumnFilters as RecordWhereColumnCollection<TRecord>, ref filterIndex, sqlFilters, string.Format("{0}  ", indent), filterValueNamePrefix);

							usedTempTable = true;
						}
					}

					if (!usedTempTable)
					{
						sqlFilters.Add(GenerateWhereClauseFilters(whereClause, recordWhereColumnFilters, ref filterIndex, string.Format("{0}  ", indent), filterValueNamePrefix));
					}
				}
			}

			var @operator = filters.WhereClauseOperator == WhereClauseOperator.And ? " AND\n" : " OR\n";

			return string.Format("{0}({1})\n", indent, string.Join(@operator, sqlFilters).Trim());
		}

		protected override IList<string> GenerateWhereClauseFilter(IWhereClause whereClause, RecordWhereColumn<TRecord> filter, ref int filterIndex, string indent, string filterValueNamePrefix = "")
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

				var filterLesserBetweenValueName = string.Format("{0}Lesser{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex);
				whereClauseWithParameters.Parameters.Add(filterLesserBetweenValueName, filter.LesserBetweenValue);

				var filterGreaterBetweenValueName = string.Format("{0}Greater{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex);
				whereClauseWithParameters.Parameters.Add(filterGreaterBetweenValueName, filter.GreaterBetweenValue);

				sqlFilters.Add(string.Format("{0}({1} BETWEEN :{2} AND :{3})", indent, FormatColumnName(columnName), filterLesserBetweenValueName, filterGreaterBetweenValueName));

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

					var filterValueName = string.Format("{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex++);

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
							sqlFilters.Add(string.Format("{0}({1} LIKE :{2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.Equal:
							whereClauseWithParameters.Parameters.Add(filterValueName, value);
							sqlFilters.Add(string.Format("{0}({1} = :{2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.Contains:
							whereClauseWithParameters.Parameters.Add(filterValueName, string.Format("%{0}%", value));
							sqlFilters.Add(string.Format("{0}({1} LIKE :{2})", indent, FormatColumnName(columnName), filterValueName));
							break;
						case WhereClauseStringComparisonOperator.EndsWith:
							whereClauseWithParameters.Parameters.Add(filterValueName, string.Format("%{0}", value));
							sqlFilters.Add(string.Format("{0}({1} LIKE :{2})", indent, FormatColumnName(columnName), filterValueName));
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

				var filterValueName = string.Format("{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterIndex++);

				whereClauseWithParameters.Parameters.Add(filterValueName, value);

				switch (filter.ComparisonOperator)
				{
					case WhereClauseComparisonOperator.LessThan:
						sqlFilters.Add(string.Format("{0}({1} < :{2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.LessThanOrEqual:
						sqlFilters.Add(string.Format("{0}({1} <= :{2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.GreaterThanOrEqual:
						sqlFilters.Add(string.Format("{0}({1} >= :{2})", indent, FormatColumnName(columnName), filterValueName));
						break;
					case WhereClauseComparisonOperator.GreaterThan:
						sqlFilters.Add(string.Format("{0}({1} > :{2})", indent, FormatColumnName(columnName), filterValueName));
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
		
		protected virtual void GenerateJoinTableFilter(IWhereClause whereClause, RecordWhereColumnCollection<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			var tempTableName = string.Format("TempTable_{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting));

			var propertyDescriptions = (filter.First() as RecordWhereColumnCollection<TRecord>).ToNullCheckedArray(recordWhereColumnFilter => (recordWhereColumnFilter as RecordWhereColumn<TRecord>).RecordPropertyDescription);

			var oracleConnectionWhereClause = whereClause as OracleConnectionWhereClause;

			oracleConnectionWhereClause.InitializeActions.Add((oracleConfiguration, oracleConnection) =>
			{
				oracleConnection.EnsureConnectionIsOpenAsync().Wait();

				using (var command = oracleConnection.CreateCommand())
				{
					command.CommandText = @$"
CREATE GLOBAL TEMPORARY TABLE {tempTableName}
(
{string.Join(",\n", propertyDescriptions.Select(propertyDescription => $"  {propertyDescription.GetColumnDefinition(FormatColumnName)}"))}
) ON COMMIT PRESERVE ROWS";

					command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
				}

				var joinTableValues = new JoinTableValues(filter);

				using (var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<RecordWhereColumnCollection<TRecord>>(joinTableValues, null, null))
				{
					var columnsLength = propertyDescriptions.Length;
					var records = new List<object>[columnsLength];
					for (var columnIndex = 0; columnIndex < columnsLength; columnIndex++)
					{
						records[columnIndex] = new List<object>();
					}

					while (dataReader.Read())
					{
						for (var columnIndex = 0; columnIndex < columnsLength; columnIndex++)
						{
							records[columnIndex].Add(dataReader.GetValue(columnIndex));
						}
					}

					using (var command = oracleConnection.CreateCommand())
					{
						command.CommandText = @$"INSERT INTO {tempTableName} ({string.Join(", ", propertyDescriptions.Select(propertyDescription => FormatColumnName(propertyDescription.ColumnName)))}) VALUES ({string.Join(", ", joinTableValues.GetColumns().Select(column => $":{column.ColumnName}"))})";
						command.BindByName = true;

						command.ArrayBindCount = records[0].Count;

						for (var columnIndex = 0; columnIndex < columnsLength; columnIndex++)
						{
							var column = joinTableValues.GetColumns()[columnIndex];

							var parameter = new global::Oracle.ManagedDataAccess.Client.OracleParameter(column.ColumnName, column.PropertyType.GetOracleDbType(), System.Data.ParameterDirection.Input);
							parameter.Value = records[columnIndex].ToArray();
							command.Parameters.Add(parameter);
						}

						command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
					}
				}
			});

			oracleConnectionWhereClause.JoinCauseBuilders.Add(alias => $"    JOIN {tempTableName} ON ({string.Join(" AND\n         ", propertyDescriptions.Select(propertyDescription => $"{tempTableName}.{FormatColumnName(propertyDescription.ColumnName)} = {alias}.{FormatColumnName(propertyDescription.ColumnName)}"))})\n");
		}

		protected override void GenerateWhereClauseFilter_EqualityOperator(IWhereClause whereClause, RecordWhereColumn<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			if (filter.Values != null)
			{
				var columnName = filter.RecordPropertyDescription.ColumnName;

				var filterValueMaxCount = OracleConfiguration.FilterValueMaxCount;
				var filterValueCount = 0;
				var filterValueIndex = filterIndex;
				var filterParameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

				var filterValuesEnumerator = filter.Values.GetEnumerator();
				while ((filterValueCount < filterValueMaxCount) && filterValuesEnumerator.MoveNext())
				{
					filterValueCount++;

					filterParameters.Add(string.Format("{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterValueIndex++), filterValuesEnumerator.Current);
				}

				if (filterValueCount == 0)
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
				else if (filterValueCount == 1)
				{
					if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
					{
						throw new("Where clause must implement IWhereClauseWithParameters");
					}

					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} = :{2})", indent, FormatColumnName(columnName), filterParameters.First().Key));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}({1} != :{2})", indent, FormatColumnName(columnName), filterParameters.First().Key));
							break;
					}

					foreach (var parameter in filterParameters)
					{
						whereClauseWithParameters.Parameters.Add(parameter.Key, parameter.Value);
					}

					filterIndex = filterValueIndex;
				}
				else if (filterValueCount < filterValueMaxCount)
				{
					if (!(whereClause is IWhereClauseWithParameters whereClauseWithParameters))
					{
						throw new("Where clause must implement IWhereClauseWithParameters");
					}

					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys.Select(filterParameterKey => $":{filterParameterKey}"))));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(NOT {1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys.Select(filterParameterKey => $":{filterParameterKey}"))));
							break;
					}

					foreach (var parameter in filterParameters)
					{
						whereClauseWithParameters.Parameters.Add(parameter.Key, parameter.Value);
					}

					filterIndex = filterValueIndex;
				}
				else
				{
					var filterValueTempTableName = string.Format("FilterValues_{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting));

					var usedFilterValues = filterParameters.Values.ToList();
					while (filterValuesEnumerator.MoveNext())
					{
						usedFilterValues.Add(filterValuesEnumerator.Current);
					}

					var filterValues = usedFilterValues.ToArray();

					var sqlConnectionWhereClause = whereClause as OracleConnectionWhereClause;

					sqlConnectionWhereClause.InitializeActions.Add((oracleConfiguration, connection) =>
					{
						var filterValuesParameter = new global::Oracle.ManagedDataAccess.Client.OracleParameter();
						filterValuesParameter.OracleDbType = filterValues.First().GetType().GetOracleDbType();
						filterValuesParameter.Value = filterValues;

						using (var command = connection.CreateCommand())
						{
							command.CommandText = @$"
CREATE GLOBAL TEMPORARY TABLE {filterValueTempTableName}
(
	{filter.RecordPropertyDescription.GetColumnDefinition(FormatColumnName)}
) ON COMMIT PRESERVE ROWS";

							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						using (var command = connection.CreateCommand())
						{
							command.CommandText = @$"INSERT INTO {filterValueTempTableName} ({filter.RecordPropertyDescription.GetColumnDefinition(FormatColumnName)}) VALUES (:1)";
							command.ArrayBindCount = filterValues.Length;
							command.Parameters.Add(filterValuesParameter);

							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					});

					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} IN (SELECT {1} FROM {2}))", indent, FormatColumnName(columnName), filterValueTempTableName));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(NOT {1} IN (SELECT {1} FROM {2}))", indent, FormatColumnName(columnName), filterValueTempTableName));
							break;
					}

					sqlConnectionWhereClause.FinalizeActions.Add((connection) =>
					{
						var dropTempTableSql = string.Format("DROP TABLE {0}", filterValueTempTableName);

						using (var command = new global::Oracle.ManagedDataAccess.Client.OracleCommand(dropTempTableSql, connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					});
				}
			}
		}
	}
}
