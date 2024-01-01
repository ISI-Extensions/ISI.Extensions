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
		protected virtual async Task<int> DeleteRecordsAsync(IWhereClause whereClause, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!(whereClause is IWhereClauseWithGetSql whereClauseWithGetSql))
			{
				throw new("Where clause must implement IWhereClauseWithGetSql");
			}

			if (!(whereClause is IWhereClauseWithGetParameters whereClauseWithGetParameters))
			{
				throw new("Where clause must implement IWhereClauseWithGetParameters");
			}

			var deleteCount = 0;

			using (var connection = GetSqlConnection())
			{
				var sql = new StringBuilder();

				var sqlConnectionWhereClause = whereClause as ISqlConnectionWhereClause;

				sqlConnectionWhereClause?.Initialize(SqlServerConfiguration, connection);

				var tableNameAlias = GetTableNameAlias(TableAlias);

				sql.AppendFormat("DELETE {0}\n", tableNameAlias);

				sql.AppendFormat("FROM {0}\n", GetTableName());
				if (sqlConnectionWhereClause != null)
				{
					sql.Append(sqlConnectionWhereClause.GetJoinCause(tableNameAlias));
				}

				var whereClauseSql = whereClauseWithGetSql.GetSql();
				if (!string.IsNullOrEmpty(whereClauseSql))
				{
					sql.Append("WHERE\n");
					sql.Append(whereClauseSql);
				}

				if (whereClause.IsFilter || whereClause.HasFilter)
				{
					await connection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

					using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
					{
						command.CommandTimeout = 0;
						command.AddParameters(whereClauseWithGetParameters.GetParameters());

						deleteCount = await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
					}
				}

				sqlConnectionWhereClause?.Finalize(connection);
			}

			return deleteCount;
		}
	}
}
