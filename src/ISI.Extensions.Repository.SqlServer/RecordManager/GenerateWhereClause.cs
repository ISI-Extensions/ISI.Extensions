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
using ISI.Extensions.Repository.SqlServer.Extensions;
using ISI.Extensions.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
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

				var filterValueMaxCount = SqlServerConfiguration.FilterValueMaxCount;
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
							sqlFilters.Add(string.Format("{0}({1} is null)", indent, FormatColumnName(columnName)));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(not {1} is null)", indent, FormatColumnName(columnName)));
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
							sqlFilters.Add(string.Format("{0}({1} in ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys)));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(not {1} in ({2}))", indent, FormatColumnName(columnName), string.Join(" ,", filterParameters.Keys)));
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

					sqlConnectionWhereClause.InitializeActions.Add((sqlServerConfiguration, connection, sqlServerCapabilities) =>
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

						using (var target = new Microsoft.Data.SqlClient.SqlBulkCopy(connection, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, null))
						{
							var createTempTableSql = string.Format(@"
create table #{0}
(
	{1}
)", filterValueTempTableName, filter.RecordPropertyDescription.GetColumnDefinition(FormatColumnName));

							using (var command = new Microsoft.Data.SqlClient.SqlCommand(createTempTableSql, connection))
							{
								command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
							}

							target.DestinationTableName = string.Format("#{0}", filterValueTempTableName);
							target.BulkCopyTimeout = 0; // 60 * 60;
							target.BatchSize = 1000;
							target.WriteToServer(dataReader);
						}
					});

					switch (filter.EqualityOperator)
					{
						case WhereClauseEqualityOperator.Equal:
							sqlFilters.Add(string.Format("{0}({1} in (select {1} from #{2} with (NoLock)))", indent, FormatColumnName(columnName), filterValueTempTableName));
							break;
						case WhereClauseEqualityOperator.NotEqual:
							sqlFilters.Add(string.Format("{0}(not {1} in (select {1} from #{2} with (NoLock)))", indent, FormatColumnName(columnName), filterValueTempTableName));
							break;
					}

					sqlConnectionWhereClause.FinalizeActions.Add((connection, sqlServerCapabilities) =>
					{
						var dropTempTableSql = string.Format("drop table #{0}", filterValueTempTableName);

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(dropTempTableSql, connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					});
				}
			}
		}
	}
}

