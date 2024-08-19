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
			return new SqlConnectionWhereClause();
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

					filterParameters.Add(string.Format("@{0}{1}FilterValue_{2}", filterValueNamePrefix, columnName, filterValueIndex++), filterValuesEnumerator.Current);
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
							sqlFilters.Add(string.Format("{0}({1} = {2})", indent, FormatColumnName(columnName), filterParameters.First().Key));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}({1} != {2})", indent, FormatColumnName(columnName), filterParameters.First().Key));
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
							sqlFilters.Add(string.Format("{0}({1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys)));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(NOT {1} IN ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys)));
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

					var usedFilterValues = filterParameters.Values;

					var sqlConnectionWhereClause = whereClause as SqlConnectionWhereClause;

					sqlConnectionWhereClause.InitializeActions.Add((sqlServerConfiguration, connection) =>
					{
						var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<ISI.Extensions.DataReader.ValueWrapper<object>>(new[]
						{
							usedFilterValues.Select(filterValue => new ISI.Extensions.DataReader.ValueWrapper<object>()
							{
								Value = filterValue,
							}),
							new ISI.Extensions.ContinueReadingEnumerable<object>(filter.Values, filterValuesEnumerator).Select(filterValue => new ISI.Extensions.DataReader.ValueWrapper<object>()
							{
								Value = filterValue,
							})
						}, null, null);

						connection.EnsureConnectionIsOpenAsync().Wait();

						var createTempTableSql = string.Format(@"
CREATE TEMP TABLE {0}
(
	{1}
)", filterValueTempTableName, filter.RecordPropertyDescription.GetColumnDefinition(FormatColumnName));

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

					sqlConnectionWhereClause.FinalizeActions.Add((connection) =>
					{
						var dropTempTableSql = string.Format("DROP TABLE {0}", filterValueTempTableName);

						using (var command = new Npgsql.NpgsqlCommand(dropTempTableSql, connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					});
				}
			}
		}
	}
}
