#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.PostgreSQL
{
	public partial class BackupManager
	{
		public DTOs.FixCollationVersionResponse FixCollationVersion(DTOs.IFixCollationVersionRequest request)
		{
			var response = new DTOs.FixCollationVersionResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder();
			switch (request)
			{
				case DTOs.FixCollationVersionRequest fixCollationVersionRequest:
					connectionStringBuilder.Host = fixCollationVersionRequest.Host;
					connectionStringBuilder.Port = fixCollationVersionRequest.Port ?? 5432;
					connectionStringBuilder.Username = fixCollationVersionRequest.UserName;
					connectionStringBuilder.Password = fixCollationVersionRequest.Password;
					break;

				case DTOs.FixCollationVersionUsingConnectionStringRequest fixCollationVersionUsingConnectionStringRequest:
					connectionStringBuilder.ConnectionString = fixCollationVersionUsingConnectionStringRequest.ConnectionString;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			logger.LogInformation($"ServerName: {connectionStringBuilder.GetServerName()}");

			var databases = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet();

			using (var connection = ISI.Extensions.PostgreSQL.NpgsqlConnection.GetNpgsqlConnection(connectionStringBuilder.ConnectionString))
			{
				connection.Open();

				var sql = new StringBuilder();
				sql.AppendLine($"SELECT datname AS \"DatabaseName\", datcollversion AS \"CollationVersion\", pg_database_collation_actual_version(oid) AS \"ShouldBeCollationVersion\" FROM pg_database;");

				using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var collationVersion = (reader.GetString("CollationVersion") ?? string.Empty).Trim();
							if (!string.IsNullOrWhiteSpace(collationVersion))
							{
								var shouldBeCollationVersion = (reader.GetString("ShouldBeCollationVersion") ?? string.Empty).Trim();

								if (!string.Equals(collationVersion, shouldBeCollationVersion, StringComparison.InvariantCultureIgnoreCase))
								{
									databases.Add(reader.GetString("DatabaseName"));
								}
							}
						}
					}
				}
			}

			foreach (var database in databases)
			{
				logger.LogInformation($"Fixing Collation Version For: {database}");

				var tempTableName = $"fix-collation-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

				using (var connection = ISI.Extensions.PostgreSQL.NpgsqlConnection.GetNpgsqlConnection(connectionStringBuilder.ConnectionString))
				{
					connection.Open();

					var sql = new StringBuilder();
					sql.AppendLine($"DROP TABLE IF EXISTS \"{tempTableName}\";");
					sql.AppendLine($"CREATE TABLE \"{tempTableName}\" (str text);");
					sql.AppendLine($"COPY \"{tempTableName}\" FROM PROGRAM 'psql --username=postgres  --command=\"REINDEX DATABASE \\\"{database}\\\";\" {database}';");
					sql.AppendLine($"COPY \"{tempTableName}\" FROM PROGRAM 'psql --username=postgres  --command=\"ALTER DATABASE \\\"{database}\\\" REFRESH COLLATION VERSION;\" {database}';");
					sql.AppendLine($"DROP TABLE IF EXISTS \"{tempTableName}\";");

					using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
					{
						command.CommandTimeout = TimeSpan.FromHours(3).Seconds;
						command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
					}
				}
			}

			return response;
		}
	}
}