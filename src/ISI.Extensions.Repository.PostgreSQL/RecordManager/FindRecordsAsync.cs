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
using ISI.Extensions.Repository.PostgreSQL.Extensions;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				return await FindRecordsAsync(connection, whereClause, orderByClause, skip, take, commandTimeout, cancellationToken);
			}
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(string fromClause, IWhereClause whereClause, IOrderByClause orderByClause = null, int skip = 0, int take = -1, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				return await FindRecordsAsync(connection, fromClause, whereClause, orderByClause, skip, take, commandTimeout, cancellationToken);
			}
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(Npgsql.NpgsqlConnection connection, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			return await FindRecordsAsync(connection, GetFromClause(), whereClause, orderByClause, skip, take, commandTimeout, cancellationToken);
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(Npgsql.NpgsqlConnection connection, string fromClause, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			return await FindRecordsAsync(connection, null, fromClause, whereClause, orderByClause, skip, take, commandTimeout, cancellationToken);
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(Npgsql.NpgsqlConnection connection, ISelectClause selectClause, string fromClause, IWhereClause whereClause, IOrderByClause orderByClause, int skip, int take, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			var sql = new StringBuilder();

			var sqlConnectionWhereClause = whereClause as ISqlConnectionWhereClause;

			sqlConnectionWhereClause?.Initialize(SqlServerConfiguration, connection);

			if ((whereClause != null) && !whereClause.IsFilter && !whereClause.HasFilter)
			{
				//return empty if not a filter (ex. GetRecords) and no primaryKeys
				//yield return new TRecord[0];
			}
			else
			{
				selectClause ??= GenerateSelectClause();
				var whereSql = (whereClause as IWhereClauseWithGetSql)?.GetSql();

				sql.AppendFormat("SELECT {0}\n", selectClause.GetSql());
				sql.AppendFormat("{0}\n", fromClause);

				if (sqlConnectionWhereClause != null)
				{
					sql.Append(sqlConnectionWhereClause.GetJoinCause(GetTableNameAlias(TableAlias)));
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

				var records = await FindRecordsAsync(connection, sql.ToString(), (whereClause as IWhereClauseWithGetParameters)?.GetParameters(), commandTimeout, cancellationToken);

				sqlConnectionWhereClause?.Finalize(connection);

				return records;
			}

			return Array.Empty<TRecord>();
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(string sql, IDictionary<string, object> parameters, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				return await FindRecordsAsync(connection, sql, parameters, null, cancellationToken);
			}
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(string sql, IDictionary<string, object> parameters, int commandTimeout, System.Threading.CancellationToken cancellationToken = default)
		{
			using (var connection = GetSqlConnection())
			{
				return await FindRecordsAsync(connection, sql, parameters, commandTimeout, cancellationToken);
			}
		}

		protected virtual async Task<IEnumerable<TRecord>> FindRecordsAsync(Npgsql.NpgsqlConnection connection, string sql, IDictionary<string, object> parameters, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			var records = new List<TRecord>();

			await connection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

			using (var command = new Npgsql.NpgsqlCommand(sql, connection))
			{
				if (commandTimeout.HasValue)
				{
					command.CommandTimeout = commandTimeout.Value;
				}
				command.AddParameters(parameters);

				using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync(cancellationToken: cancellationToken))
				{
					var reader = ExpressionBuilder.GetReader(dataReader, RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions, Serializer);

					while (await dataReader.ReadAsync(cancellationToken))
					{
						records.Add(reader(dataReader));
					}
				}
			}

			return records;
		}
	}
}
