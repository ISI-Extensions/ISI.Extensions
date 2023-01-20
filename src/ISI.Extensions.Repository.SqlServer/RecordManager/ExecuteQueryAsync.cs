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
		public delegate void ExecuteQuery_ProcessResults(Microsoft.Data.SqlClient.SqlDataReader dataReader);

		protected virtual async Task ExecuteQueryAsync(ExecuteQuery_ProcessResults processResults, ISelectClause selectClause, IWhereClause whereClause = null, IOrderByClause orderByClause = null, int? commandTimeout = null)
		{
			using (var connection = GetSqlConnection())
			{
				var sql = new StringBuilder();

				var sqlConnectionWhereClause = whereClause as ISqlConnectionWhereClause;

				var sqlServerCapabilities = await connection.GetSqlServerCapabilitiesAsync();

				sqlConnectionWhereClause?.Initialize(SqlServerConfiguration, connection, sqlServerCapabilities);

				var whereSql = (whereClause as IWhereClauseWithGetSql)?.GetSql();

				sql.AppendFormat("select {0}\n", selectClause.GetSql());
				sql.AppendFormat("{0}\n", GetFromClause());

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

				await ExecuteQueryAsync(processResults, connection, sql.ToString(), (whereClause as IWhereClauseWithGetParameters)?.GetParameters(), commandTimeout);
			}
		}

		protected virtual async Task ExecuteQueryAsync(ExecuteQuery_ProcessResults processResults, string sql, IDictionary<string, object> parameters = null, int? commandTimeout = null)
		{
			using (var connection = GetSqlConnection())
			{
				await ExecuteQueryAsync(processResults, connection, sql, parameters, commandTimeout);
			}
		}

		protected virtual async Task ExecuteQueryAsync(ExecuteQuery_ProcessResults processResults, Microsoft.Data.SqlClient.SqlConnection connection, string sql, IDictionary<string, object> parameters = null, int? commandTimeout = null)
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
					processResults(dataReader);
				}
			}
		}
	}
}
