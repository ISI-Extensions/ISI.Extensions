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
		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindArchiveRecordsAsync(connection, minArchiveDateTime, maxArchiveDateTime, whereClause, orderByClause, skip, take, cancellationToken))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(string fromClause, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindArchiveRecordsAsync(connection, fromClause, minArchiveDateTime, maxArchiveDateTime, whereClause, orderByClause, skip, take, cancellationToken))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, System.Threading.CancellationToken cancellationToken = default)
		{
			await foreach (var record in FindArchiveRecordsAsync(connection, string.Format("FROM {0} WITH (NOLOCK)", GetArchiveTableName(TableAlias)), minArchiveDateTime, maxArchiveDateTime, whereClause, orderByClause, skip, take, cancellationToken))
			{
				yield return record;
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, string fromClause, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, System.Threading.CancellationToken cancellationToken = default)
		{
			await foreach (var record in FindArchiveRecordsAsync(connection, null, fromClause, minArchiveDateTime, maxArchiveDateTime, whereClause, orderByClause, skip, take, cancellationToken))
			{
				yield return record;
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, ISelectClause selectClause, string fromClause, DateTime? minArchiveDateTime, DateTime? maxArchiveDateTime, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, System.Threading.CancellationToken cancellationToken = default)
		{
			var sql = new StringBuilder();

			var sqlConnectionWhereClause = whereClause as ISqlConnectionWhereClause;

			var sqlServerCapabilities = await connection.GetSqlServerCapabilitiesAsync(cancellationToken: cancellationToken);

			sqlConnectionWhereClause?.Initialize(SqlServerConfiguration, connection, sqlServerCapabilities);

			if ((whereClause != null) && !whereClause.IsFilter && !whereClause.HasFilter)
			{
				//return empty if not a filter (ex. GetRecords) and no primaryKeys
				//return new ISI.Extensions.Repository.ArchiveRecord<TRecord>[0];
			}
			else
			{
				selectClause ??= GenerateSelectClause(GetArchiveTableNameAlias(TableAlias));
				var whereSql = (whereClause as IWhereClauseWithGetSql)?.GetSql();

				whereClause ??= new WhereClause();
				if (minArchiveDateTime.HasValue)
				{
					(whereClause as IWhereClauseWithParameters)?.Parameters?.Add("@minArchiveDateTime", minArchiveDateTime.Value);
				}

				if (maxArchiveDateTime.HasValue)
				{
					(whereClause as IWhereClauseWithParameters)?.Parameters?.Add("@maxArchiveDateTime", maxArchiveDateTime.Value);
				}

				if (minArchiveDateTime.HasValue && maxArchiveDateTime.HasValue)
				{
					whereSql = string.Format("      {0}.{1} BETWEEN @minArchiveDateTime AND @maxArchiveDateTime{2}{3}", GetArchiveTableNameAlias(TableAlias), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), (string.IsNullOrWhiteSpace(whereSql) ? string.Empty : " AND\n"), whereSql);
				}
				else if (minArchiveDateTime.HasValue)
				{
					whereSql = string.Format("      {0}.{1} >= @minArchiveDateTime{2}{3}", GetArchiveTableNameAlias(TableAlias), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), (string.IsNullOrWhiteSpace(whereSql) ? string.Empty : " AND\n"), whereSql);
				}
				else if (maxArchiveDateTime.HasValue)
				{
					whereSql = string.Format("      {0}.{1} <= @maxArchiveDateTime{2}{3}", GetArchiveTableNameAlias(TableAlias), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), (string.IsNullOrWhiteSpace(whereSql) ? string.Empty : " AND\n"), whereSql);
				}

				if (sqlServerCapabilities.SupportsNativePaging)
				{
					// https://www.mssqltips.com/sqlservertip/2696/comparing-performance-for-different-sql-server-paging-methods/
					// don't use top at all - use offset / fetch next for consistency, even if offset is 0
					sql.AppendFormat("SELECT {0}.{1}, {2}\n", GetArchiveTableNameAlias(TableAlias), ArchiveTableArchiveDateTimeColumnName, selectClause.GetSql());
					sql.AppendFormat("{0}\n", fromClause);

					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetArchiveTableNameAlias(TableAlias)));
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("WHERE\n");
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
						sql.AppendFormat("OFFSET ({0}) ROWS\n", skip);
						if (take > 0)
						{
							sql.AppendFormat("FETCH NEXT ({0}) ROWS ONLY\n", take);
						}
					}
				}
				else if (skip <= 0)
				{
					// pre 2012, no offset - use top if necessary
					if (take > 0)
					{
						sql.AppendFormat("SELECT top ({0}) {1}.{2}, {3}\n", take, GetArchiveTableNameAlias(TableAlias), ArchiveTableArchiveDateTimeColumnName, selectClause.GetSql());
					}
					else
					{
						sql.AppendFormat("SELECT {0}.{1}, {2}\n", GetArchiveTableNameAlias(TableAlias), ArchiveTableArchiveDateTimeColumnName, selectClause.GetSql());
					}

					sql.AppendFormat("{0}\n", fromClause);

					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetArchiveTableNameAlias(TableAlias)));
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("WHERE\n");
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

					sql.AppendFormat("SELECT {0}.{1}, {2}\n", GetArchiveTableNameAlias(TableAlias), ArchiveTableArchiveDateTimeColumnName, selectClause.GetSql(pagingQueryName));
					sql.AppendFormat("FROM (SELECT {0},\n", selectClause.GetSql());
					sql.AppendFormat("      ROW_NUMBER() OVER ({0}) AS {1} FROM {2} WITH (nolock)\n", orderByClauseSql, rowNumberName, GetTableName());
					if (sqlConnectionWhereClause != null)
					{
						sql.Append(sqlConnectionWhereClause.GetJoinCause(GetArchiveTableNameAlias(TableAlias)));
						sql.AppendLine();
					}

					if (!string.IsNullOrWhiteSpace(whereSql))
					{
						sql.Append("WHERE\n");
						sql.Append(whereSql);
					}

					sql.AppendLine();
					sql.AppendFormat(") {0}\n", pagingQueryName);
					sql.Append("WHERE\n");
					sql.AppendFormat("       {0} > {1} AND\n", rowNumberName, skip);
					sql.AppendFormat("       {0} <= {1}\n", rowNumberName, skip + take);

					if (!string.IsNullOrWhiteSpace(orderByClauseSql))
					{
						sql.Append(orderByClause.GetSql());
					}
				}

				await foreach (var record in FindArchiveRecordsAsync(connection, sql.ToString(), (whereClause as IWhereClauseWithGetParameters)?.GetParameters(), cancellationToken))
				{
					yield return record;
				}

				sqlConnectionWhereClause?.Finalize(connection, sqlServerCapabilities);
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(string sql, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				await foreach (var record in FindArchiveRecordsAsync(connection, sql, parameters, cancellationToken))
				{
					yield return record;
				}
			}
		}

		protected virtual async IAsyncEnumerable<ISI.Extensions.Repository.ArchiveRecord<TRecord>> FindArchiveRecordsAsync(Microsoft.Data.SqlClient.SqlConnection connection, string sql, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken = default)
		{
			await connection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

			using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
			{
				command.AddParameters(parameters);

				using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync(cancellationToken: cancellationToken))
				{
					var reader = ExpressionBuilder.GetReader(dataReader, RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions, Serializer);

					while (await dataReader.ReadAsync(cancellationToken))
					{
						yield return new()
						{
							ArchiveDateTime = dataReader.GetDateTime(0),
							Record = reader(dataReader),
						};
					}
				}
			}
		}
	}
}
