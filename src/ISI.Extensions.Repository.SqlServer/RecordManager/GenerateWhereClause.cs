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
using System.Reflection;
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

		protected virtual void GenerateJoinTableFilter(IWhereClause whereClause, RecordWhereColumnCollection<TRecord> filter, ref int filterIndex, List<string> sqlFilters, string indent, string filterValueNamePrefix)
		{
			var tempTableName = string.Format("TempTable_{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting));

			var propertyDescriptions = (filter.First() as RecordWhereColumnCollection<TRecord>).ToNullCheckedArray(recordWhereColumnFilter => (recordWhereColumnFilter as RecordWhereColumn<TRecord>).RecordPropertyDescription);

			var sqlConnectionWhereClause = whereClause as SqlConnectionWhereClause;

			sqlConnectionWhereClause.InitializeActions.Add((configuration, connection, capabilities) =>
			{
				connection.EnsureConnectionIsOpenAsync().Wait();

				using (var command = new Microsoft.Data.SqlClient.SqlCommand(@$"
CREATE TABLE #{tempTableName}
(
{string.Join(",\n", propertyDescriptions.Select(propertyDescription => $"  {propertyDescription.GetColumnDefinition(FormatColumnName)}"))}
)", connection))
				{
					command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
				}

				var joinTableValues = new JoinTableValues(filter);

				var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<RecordWhereColumnCollection<TRecord>>(joinTableValues, null, null);

				using (var target = new Microsoft.Data.SqlClient.SqlBulkCopy(connection, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, null))
				{
					target.DestinationTableName = $"#{tempTableName}";
					target.BulkCopyTimeout = 0; // 60 * 60;
					target.BatchSize = 1000;
					target.WriteToServer(dataReader);
				}
			});

			sqlConnectionWhereClause.JoinCauseBuilders.Add(alias => $"    JOIN #{tempTableName} {tempTableName} WITH (NOLOCK) ON ({string.Join(" AND\n         ", propertyDescriptions.Select(propertyDescription => $"{tempTableName}.{FormatColumnName(propertyDescription.ColumnName)} = {alias}.{FormatColumnName(propertyDescription.ColumnName)}"))})\n");
		}
	}
}

