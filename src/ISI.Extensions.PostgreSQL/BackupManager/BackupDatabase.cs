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
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.Extensions;
using DTOs = ISI.Extensions.PostgreSQL.DataTransferObjects.BackupManager;

namespace ISI.Extensions.PostgreSQL
{
	public partial class BackupManager
	{
		public DTOs.BackupDatabaseResponse BackupDatabase(DTOs.IBackupDatabaseRequest request)
		{
			var response = new DTOs.BackupDatabaseResponse();

			var sql = new StringBuilder();

			var tempTableName = $"dump-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

			var fileName = $"{request.Database}.{DateTimeStamper.CurrentDateTimeUtc().Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortablePrecise)}";

			var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
			{
				ConnectionString = (request as DTOs.BackupDatabaseUsingConnectionStringRequest)?.ConnectionString,
			};

			var userName = (request as DTOs.BackupDatabaseRequest)?.UserName ?? dbConnectionStringBuilder.GetUserName();
			var password = (request as DTOs.BackupDatabaseRequest)?.Password ?? dbConnectionStringBuilder.GetPassword();
			var host = (request as DTOs.BackupDatabaseRequest)?.Host ?? dbConnectionStringBuilder.GetServerName();
			var port = (request as DTOs.BackupDatabaseRequest)?.Port ?? dbConnectionStringBuilder.GetServerPort() ?? 5432;


			sql.AppendLine($"DROP TABLE IF EXISTS \"{tempTableName}\";");
			sql.AppendLine($"CREATE TABLE \"{tempTableName}\" (str text);");
			sql.AppendLine($"COPY \"{tempTableName}\" FROM PROGRAM 'pg_dump --dbname=postgresql://{userName}:{password}@127.0.0.1:{port}/{request.Database}  --file={request.BackupDirectory}/{fileName}.dumping';");
			sql.AppendLine($"COPY \"{tempTableName}\" FROM PROGRAM 'mv {request.BackupDirectory}/{fileName}.dumping {request.BackupDirectory}/{fileName}.sql';");
			sql.AppendLine($"DROP TABLE IF EXISTS \"{tempTableName}\";");

			var connectionString = $"Host={host};Username={userName};Password={password};Database=postgres;";

			using (var connection = ISI.Extensions.PostgreSQL.SqlConnection.GetSqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
				{
					command.CommandTimeout = TimeSpan.FromHours(3).Seconds;
					command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
				}
			}

			response.FileName = $"{request.BackupDirectory}/{fileName}.sql";
			
			return response;
		}
	}
}