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
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public abstract partial class RecordManager<TRecord>
	{
		protected override IWhereClause NewWhereClause()
		{
			return new NpgsqlConnectionWhereClause();
		}

		protected override string GenerateTopLevelWhereClauseFilters(IWhereClause whereClause, IRecordWhereColumnCollection<TRecord> filters, ref int filterIndex, string indent, string filterValueNamePrefix = "")
		{
			var sqlFilters = new List<string>();

			if ((filters.WhereClauseOperator == WhereClauseOperator.Or) && (filters.Count > 10) && filters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>)?.WhereClauseOperator == WhereClauseOperator.And))
			{
				var columnNamesHashCode = (filters.First() as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode();

				if (filters.All(subFilter => (subFilter as IRecordWhereColumnCollection<TRecord>).GetColumnNamesHashCode() == columnNamesHashCode))
				{
					GenerateJoinTableFilter(whereClause, filters as RecordWhereColumnCollection<TRecord>, ref filterIndex, sqlFilters, $"{indent}  ", filterValueNamePrefix);

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
							GenerateJoinTableFilter(whereClause, recordWhereColumnFilters as RecordWhereColumnCollection<TRecord>, ref filterIndex, sqlFilters, $"{indent}  ", filterValueNamePrefix);

							usedTempTable = true;
						}
					}

					if (!usedTempTable)
					{
						sqlFilters.Add(GenerateWhereClauseFilters(whereClause, recordWhereColumnFilters, ref filterIndex, $"{indent}  ", filterValueNamePrefix));
					}
				}
			}

			var @operator = filters.WhereClauseOperator == WhereClauseOperator.And ? " AND\n" : " OR\n";

			return $"{indent}({string.Join(@operator, sqlFilters).Trim()})\n";
		}

		protected override void GenerateWhereClauseFilter_EqualityOperator(IWhereClause whereClause, RecordWhereColumn<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			if (filter.Values != null)
			{
				var columnName = filter.RecordPropertyDescription.ColumnName;

				var filterValueMaxCount = PostgreSQLConfiguration.FilterValueMaxCount;
				var filterValueCount = 0;
				var filterValueIndex = filterIndex;
				var filterParameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

				var filterValuesEnumerator = filter.Values.GetEnumerator();
				while ((filterValueCount < filterValueMaxCount) && filterValuesEnumerator.MoveNext())
				{
					filterValueCount++;

					filterParameters.Add($"@{filterValueNamePrefix}{columnName}FilterValue_{filterValueIndex++}", filterValuesEnumerator.Current);
				}

				if (filterValueCount == 0)
				{
					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add($"{indent}({FormatColumnName(columnName)} IS NULL)");
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add($"{indent}(NOT {FormatColumnName(columnName)} IS NULL)");
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
							sqlFilters.Add($"{indent}({FormatColumnName(columnName)} = {filterParameters.First().Key})");
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add($"{indent}({FormatColumnName(columnName)} != {filterParameters.First().Key})");
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
							sqlFilters.Add($"{indent}({FormatColumnName(columnName)} IN ({string.Join(" ,", filterParameters.Keys)}))");
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add($"{indent}(NOT {FormatColumnName(columnName)} IN ({string.Join(" ,", filterParameters.Keys)}))");
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
					var filterValueTempTableName = $"FilterValues_{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

					var usedFilterValues = filterParameters.Values;

					var npgsqlConnectionWhereClause = whereClause as NpgsqlConnectionWhereClause;

					npgsqlConnectionWhereClause.InitializeActions.Add((postgresqlConfiguration, connection) =>
					{
						var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<ISI.Extensions.DataReader.ValueWrapper<object>>([
							usedFilterValues.Select(filterValue => new ISI.Extensions.DataReader.ValueWrapper<object>()
							{
								Value = filterValue,
							}),
							new ISI.Extensions.ContinueReadingEnumerable<object>(filter.Values, filterValuesEnumerator).Select(filterValue => new ISI.Extensions.DataReader.ValueWrapper<object>()
							{
								Value = filterValue,
							})
						], null, null);

						connection.EnsureConnectionIsOpenAsync().Wait();

						var createTempTableSql = @$"
CREATE TEMP TABLE {filterValueTempTableName}
(
	{filter.RecordPropertyDescription.GetColumnDefinition(FormatColumnName)}
)";

						using (var command = new Npgsql.NpgsqlCommand(createTempTableSql, connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						using (var binaryImporter = connection.BeginBinaryImport($"COPY {filterValueTempTableName} FROM STDIN (FORMAT BINARY)"))
						{
							while (dataReader.Read())
							{
								binaryImporter.StartRow();

								for (var columnIndex = 0; columnIndex < dataReader.FieldCount; columnIndex++)
								{
									binaryImporter.Write(dataReader.GetValue(columnIndex));
								}
							}

							binaryImporter.Complete();
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

					npgsqlConnectionWhereClause.FinalizeActions.Add((connection) =>
					{
						var dropTempTableSql = $"DROP TABLE {filterValueTempTableName}";

						using (var command = new Npgsql.NpgsqlCommand(dropTempTableSql, connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					});
				}
			}
		}

		
		protected virtual void GenerateJoinTableFilter(IWhereClause whereClause, RecordWhereColumnCollection<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			var tempTableName = $"TempTable_{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

			var propertyDescriptions = (filter.First() as RecordWhereColumnCollection<TRecord>).ToNullCheckedArray(recordWhereColumnFilter => (recordWhereColumnFilter as RecordWhereColumn<TRecord>).RecordPropertyDescription);

			var npgsqlConnectionWhereClause = whereClause as NpgsqlConnectionWhereClause;

			npgsqlConnectionWhereClause.InitializeActions.Add((postgresqlConfiguration, connection) =>
			{
				connection.EnsureConnectionIsOpenAsync().Wait();

				using (var command = connection.CreateCommand())
				{
					command.CommandText = @$"
CREATE TEMP TABLE {tempTableName}
(
{string.Join(",\n", propertyDescriptions.Select(propertyDescription => $"  {propertyDescription.GetColumnDefinition(FormatColumnName)}"))}
)";

					command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
				}

				var joinTableValues = new JoinTableValues(filter);

				using (var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<RecordWhereColumnCollection<TRecord>>(joinTableValues, null, null))
				{
					using (var binaryImporter = connection.BeginBinaryImport($"COPY {tempTableName} FROM STDIN (FORMAT BINARY)"))
					{
						while (dataReader.Read())
						{
							binaryImporter.StartRow();

							for (var columnIndex = 0; columnIndex < dataReader.FieldCount; columnIndex++)
							{
								binaryImporter.Write(dataReader.GetValue(columnIndex));
							}
						}

						binaryImporter.Complete();
					}
				}
			});

			npgsqlConnectionWhereClause.JoinCauseBuilders.Add(alias => $"    JOIN {tempTableName} ON ({string.Join(" AND\n         ", propertyDescriptions.Select(propertyDescription => $"{tempTableName}.{FormatColumnName(propertyDescription.ColumnName)} = {alias}.{FormatColumnName(propertyDescription.ColumnName)}"))})\n");
		}
	}
}
