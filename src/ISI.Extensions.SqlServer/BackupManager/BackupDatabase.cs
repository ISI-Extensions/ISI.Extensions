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
using ISI.Extensions.SqlServer.Extensions;
using ISI.Extensions.Repository.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.SqlServer.DataTransferObjects.BackupManager;

namespace ISI.Extensions.SqlServer
{
	public partial class BackupManager
	{
		public DTOs.BackupDatabaseResponse BackupDatabase(DTOs.IBackupDatabaseRequest request)
		{
			var response = new DTOs.BackupDatabaseResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			logger.LogInformation($"Backing up database {request.Database}");

			var fileNameDateTimeUtc = request.FileNameDateTimeUtc ?? DateTimeStamper.CurrentDateTimeUtc();

			var fileName = $"{request.Database}.{fileNameDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortablePrecise)}";

			var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder();
			switch (request)
			{
				case DTOs.BackupDatabaseRequest backupDatabaseRequest:
					//connectionStringBuilder.DataSource = backupDatabaseRequest.Port.HasValue ? $"{backupDatabaseRequest.Host}:{backupDatabaseRequest.Port}" : backupDatabaseRequest.Host;
					connectionStringBuilder.DataSource = backupDatabaseRequest.Host;
					connectionStringBuilder.UserID = backupDatabaseRequest.UserName;
					connectionStringBuilder.Password = backupDatabaseRequest.Password;
					break;

				case DTOs.BackupDatabaseUsingConnectionStringRequest backupDatabaseUsingConnectionStringRequest:
					connectionStringBuilder.ConnectionString = backupDatabaseUsingConnectionStringRequest.ConnectionString;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			connectionStringBuilder.TrustServerCertificate = true;

			Logger.LogInformation($"ServerName: {connectionStringBuilder.GetServerName()}");
			//Logger.LogInformation($"ServerPort: {connectionStringBuilder.GetServerPort()}");
			//Logger.LogInformation($"ConnectionString: {connectionStringBuilder.ConnectionString}");

			using (var connection = ISI.Extensions.SqlServer.SqlConnection.GetSqlConnection(connectionStringBuilder.ConnectionString))
			{
				connection.Open();

				var sql = new StringBuilder();
				sql.AppendLine($"BACKUP DATABASE [{request.Database}]");
				sql.AppendLine($"TO DISK = '{request.LocalBackupDirectory}\\{fileName}.bak';");

				using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), connection))
				{
					command.CommandTimeout = TimeSpan.FromHours(3).Seconds;
					command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
				}
			}

			response.FileName = $"{fileName}.bak";

			logger.LogInformation($"Backed up database {request.Database} to \"{response.FileName}\"");

			return response;
		}
	}
}