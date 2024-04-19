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
using ISI.Extensions.Repository.SqlServer.Extensions;
using ISI.Extensions.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public abstract partial class RecordManagerPrimaryKey<TRecord, TRecordPrimaryKey>
	{
		public virtual async Task<int> UpdateRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, SetRecordColumnCollection<TRecord> setRecordColumns, System.Threading.CancellationToken cancellationToken = default)
		{
			return await UpdateRecordsAsync(primaryKeyValues, setRecordColumns, false, null, cancellationToken: cancellationToken);
		}

		protected virtual async Task<int> UpdateRecordsAsync(IEnumerable<TRecordPrimaryKey> primaryKeyValues, SetRecordColumnCollection<TRecord> setRecordColumns, bool hasArchiveTable, DateTime? archiveDateTime, int? commandTimeout = null, System.Threading.CancellationToken cancellationToken = default)
		{
			var updateCount = 0;

			var archivePropertyDescriptions = RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions.ToList();

			using (var updateConnection = GetSqlConnection())
			using (var archiveConnection = GetSqlConnection())
			{
				var updateWhereClause = GeneratePrimaryKeyWhereClause(primaryKeyValues);

				var sqlConnectionUpdateWhereClause = updateWhereClause as ISqlConnectionWhereClause;

				var updateConnectionSqlServerCapabilities = await updateConnection.GetSqlServerCapabilitiesAsync(cancellationToken: cancellationToken);

				sqlConnectionUpdateWhereClause?.Initialize(SqlServerConfiguration, updateConnection, updateConnectionSqlServerCapabilities);

				var updateSql = new StringBuilder();

				updateSql.Append("update updateTable\n");
				updateSql.Append("set\n");
				var columnIndex = 1;
				updateSql.AppendFormat("{0}\n", string.Join(",\n", setRecordColumns.Select(setRecordColumn => string.Format("    {0} = @value{1}", FormatColumnName(setRecordColumn.RecordPropertyDescription.ColumnName), columnIndex++))));
				updateSql.AppendFormat("from {0}\n", GetTableName("updateTable"));
				if (sqlConnectionUpdateWhereClause != null)
				{
					updateSql.Append(sqlConnectionUpdateWhereClause.GetJoinCause("updateTable"));
				}
				var updateWhereSql = (updateWhereClause as IWhereClauseWithGetSql)?.GetSql();
				if (!string.IsNullOrWhiteSpace(updateWhereSql))
				{
					updateSql.Append("where\n");
					updateSql.Append(updateWhereSql);
				}

				using (var command = new Microsoft.Data.SqlClient.SqlCommand(updateSql.ToString(), updateConnection))
				{
					command.CommandType = System.Data.CommandType.Text;
					if (commandTimeout.HasValue)
					{
						command.CommandTimeout = commandTimeout.Value;
					}

					columnIndex = 1;
					foreach (var setRecordColumn in setRecordColumns)
					{
						command.AddParameter(string.Format("@value{0}", columnIndex++), (setRecordColumn.Values == null ? DBNull.Value : setRecordColumn.Values.FirstOrDefault()));
					}

					command.AddParameters(updateWhereClause);

					await updateConnection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

					updateCount += await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
				}

				if (hasArchiveTable)
				{
					archiveDateTime ??= DateTimeStamper.CurrentDateTime();

					var archiveWhereClause = GeneratePrimaryKeyWhereClause(primaryKeyValues);

					var sqlConnectionArchiveWhereClause = archiveWhereClause as ISqlConnectionWhereClause;

					var archiveConnectionSqlServerCapabilities = await archiveConnection.GetSqlServerCapabilitiesAsync(cancellationToken: cancellationToken);
					sqlConnectionArchiveWhereClause?.Initialize(SqlServerConfiguration, archiveConnection, archiveConnectionSqlServerCapabilities);

					var archiveSql = new StringBuilder();

					archiveSql.AppendFormat("insert into {0} ({1}, {2})\n", GetArchiveTableName(addAlias: false), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), string.Join(", ", archivePropertyDescriptions.Select(property => string.Format("archiveTable.{0}", FormatColumnName(property.ColumnName)))));
					archiveSql.AppendFormat("select @ArchiveDateTime, {0}\n", string.Join(", ", archivePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));
					archiveSql.AppendFormat("from {0}\n", GetTableName("archiveTable"));
					if (sqlConnectionArchiveWhereClause != null)
					{
						archiveSql.Append(sqlConnectionArchiveWhereClause.GetJoinCause("archiveTable"));
					}
					var archiveWhereSql = (archiveWhereClause as IWhereClauseWithGetSql)?.GetSql();
					if (!string.IsNullOrWhiteSpace(archiveWhereSql))
					{
						archiveSql.Append("where\n");
						archiveSql.Append(archiveWhereSql);
					}

					await archiveConnection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

					using (var command = new Microsoft.Data.SqlClient.SqlCommand(archiveSql.ToString(), archiveConnection))
					{
						command.CommandType = System.Data.CommandType.Text;
						if (commandTimeout.HasValue)
						{
							command.CommandTimeout = commandTimeout.Value;
						}

						var parameters = (archiveWhereClause as IWhereClauseWithGetParameters)?.GetParameters() ?? new Dictionary<string, object>();
						parameters.Add("@ArchiveDateTime", archiveDateTime);

						command.AddParameters(parameters);

						await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
					}

					sqlConnectionArchiveWhereClause?.Finalize(archiveConnection, archiveConnectionSqlServerCapabilities);
				}

				sqlConnectionUpdateWhereClause?.Finalize(updateConnection, updateConnectionSqlServerCapabilities);
			}

			return updateCount;
		}
	}
}