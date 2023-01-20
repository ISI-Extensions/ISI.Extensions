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
using ISI.Extensions.Repository.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, int? commandTimeout = null)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindRecordsAsync(connection, whereClause, orderByClause, skip, take, commandTimeout))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(string fromClause, IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, int? commandTimeout = null)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindRecordsAsync(connection, fromClause, whereClause, orderByClause, skip, take, commandTimeout))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null)
		{
			await foreach (var record in FindRecordsAsync(connection, GetFromClause(), whereClause, orderByClause, skip, take, commandTimeout))
			{
				yield return record;
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, string fromClause, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null)
		{
			await foreach (var record in FindRecordsAsync(connection, null, fromClause, whereClause, orderByClause, skip, take, commandTimeout))
			{
				yield return record;
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, ISelectClause selectClause, string fromClause, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null)
		{
			var sql = new StringBuilder();

			var sqlConnectionWhereClause = whereClause as ISqlConnectionWhereClause;

			var sqlServerCapabilities = await connection.GetSqlServerCapabilitiesAsync();

			sqlConnectionWhereClause?.Initialize(SqlServerConfiguration, connection, sqlServerCapabilities);

			if ((whereClause != null) && !whereClause.IsFilter && !whereClause.HasFilter)
			{
				//return empty if not a filter (ex. GetRecords) and no primaryKeys
				//yield return new TRecord[0];
			}
			else
			{
				selectClause ??= GenerateSelectClause();
				var whereSql = (whereClause as IWhereClauseWithGetSql)?.GetSql();
				if (sqlServerCapabilities.SupportsNativePaging)
				{
					// https://www.mssqltips.com/sqlservertip/2696/comparing-performance-for-different-sql-server-paging-methods/
					// don't use top at all - use offset / fetch next for consistency, even if offset is 0
					sql.AppendFormat("select {0}\n", selectClause.GetSql());
					sql.AppendFormat("{0}\n", fromClause);

					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetTableNameAlias(TableAlias)));
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("where\n");
						sql.Append(whereSql);
					}

					if (!string.IsNullOrWhiteSpace(orderByClause?.GetSql()))
					{
						if (!sql.ToString().EndsWith("\n", StringComparison.InvariantCultureIgnoreCase))
						{
							sql.AppendLine();
						}

						sql.Append(orderByClause.GetSql());
					}

					if (skip > 0 || take >= 0)
					{
						sql.AppendFormat("offset ({0}) rows\n", skip);
						if (take > 0)
						{
							sql.AppendFormat("fetch next ({0}) rows only\n", take);
						}
					}
				}
				else if (skip <= 0)
				{
					// pre 2012, no offset - use top if necessary
					if (take > 0)
					{
						sql.AppendFormat("select top ({0}) {1}\n", take, selectClause.GetSql());
					}
					else
					{
						sql.AppendFormat("select {0}\n", selectClause.GetSql());
					}

					sql.AppendFormat("{0}\n", fromClause);

					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetTableNameAlias(TableAlias)));
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("where\n");
						sql.Append(whereSql);
					}

					if (!string.IsNullOrWhiteSpace(orderByClause?.GetSql()))
					{
						if (!sql.ToString().EndsWith("\n", StringComparison.InvariantCultureIgnoreCase))
						{
							sql.AppendLine();
						}

						sql.Append(orderByClause.GetSql());
					}
				}
				else
				{
					// pre 2012, with offset. derived table approach
					var orderByClauseSql = orderByClause?.GetSql();
					var rowNumberName = FormatColumnName(string.Format("RowNumber{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)));
					var pagingQueryName = FormatColumnName(string.Format("PagingQuery{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)));

					sql.AppendFormat("select {0}\n", selectClause.GetSql(pagingQueryName));
					sql.AppendFormat("from (select {0},\n", selectClause.GetSql());
					sql.AppendFormat("      ROW_NUMBER() OVER ({0}) as {1} from {2} with (nolock)\n", orderByClauseSql, rowNumberName, GetTableName());
					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetTableNameAlias(TableAlias)));
						sql.AppendLine();
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("where\n");
						sql.Append(whereSql);
					}

					sql.AppendLine();
					sql.AppendFormat(") {0}\n", pagingQueryName);
					sql.Append("where\n");
					sql.AppendFormat("       {0} > {1} and\n", rowNumberName, skip);
					sql.AppendFormat("       {0} <= {1}\n", rowNumberName, skip + take);

					if (!string.IsNullOrWhiteSpace(orderByClauseSql))
					{
						sql.Append(orderByClause.GetSql());
					}
				}

				await foreach (var record in FindRecordsAsync(connection, sql.ToString(), (whereClause as IWhereClauseWithGetParameters)?.GetParameters(), commandTimeout))
				{
					yield return record;
				}

				sqlConnectionWhereClause?.Finalize(connection, sqlServerCapabilities);
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(string sql, IDictionary<string, object> parameters)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindRecordsAsync(connection, sql, parameters, null))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(string sql, IDictionary<string, object> parameters, int commandTimeout)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindRecordsAsync(connection, sql, parameters, commandTimeout))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<TRecord> FindRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, string sql, IDictionary<string, object> parameters, int? commandTimeout = null)
		{
			await connection.EnsureConnectionIsOpenAsync();

			using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
			{
				if (commandTimeout.HasValue)
				{
					command.CommandTimeout = commandTimeout.Value;
				}
				command.AddParameters(parameters);

				using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync())
				{
					var reader = ExpressionBuilder.GetReader(dataReader, RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions, Serializer);

					while (await dataReader.ReadAsync())
					{
						yield return reader(dataReader);
					}
				}
			}
		}
	}
}
