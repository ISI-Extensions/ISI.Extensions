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
using System.Diagnostics;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.SqlServer.Extensions;
using ISI.Extensions.SqlServer.Extensions;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using SqlServerDTOs = ISI.Extensions.Repository.SqlServer.DataTransferObjects.RepositorySetupApi;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.SqlServer
{
	public partial class RepositorySetupApi
	{
		public void RenameRepository(string newRepositoryName, string connectionString = null)
		{
			Func<string, string> fileNameConverter = fullName => fullName;

			connectionString = (string.IsNullOrWhiteSpace(connectionString) ? ConnectionString : connectionString);
			var databaseName = DatabaseName;

			{
				var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

				databaseName = connectionStringBuilder.InitialCatalog;
				databaseName = databaseName.Replace("[", string.Empty).Replace("]", string.Empty);

				var databaseServer = connectionStringBuilder["SERVER"] as string;

				if (!string.Equals(databaseServer, "(local)", StringComparison.CurrentCultureIgnoreCase))
				{
					fileNameConverter = fullName =>
					{
						var pieces = fullName.Split(new[] { ':' });

						var drive = pieces[0];
						var relativeName = pieces[1];

						return string.Format("\\\\{0}\\{1}${2}", databaseServer, drive, relativeName);
					};
				}
			}

			var databaseFileNames = new List<string>();

			{
				using (var connection = ISI.Extensions.SqlServer.SqlConnection.GetSqlConnection(MasterConnectionString))
				{
					connection.Open();

					{
						var sql = string.Format(@"
select MasterFiles.type_desc, MasterFiles.name, MasterFiles.physical_name
from sys.master_files MasterFiles with (NoLock)
		inner join sys.databases Databases with (NoLock) on (Databases.database_id = MasterFiles.database_id)
where Databases.name = '{0}'
order by MasterFiles.type, MasterFiles.file_id
", databaseName);

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
						{
							using (var dataReader = command.ExecuteReaderWithExceptionTracingAsync().GetAwaiter().GetResult())
							{
								while (dataReader.Read())
								{
									databaseFileNames.Add(dataReader.GetString(2));
								}
							}
						}
					}

					{
						var sql = string.Format(@"EXEC master.dbo.sp_detach_db @dbname = N'{0}'", databaseName);

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
						{
							command.CommandTimeout = 60 * 10;
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					}

					for (var databaseFileNamesIndex = 0; databaseFileNamesIndex < databaseFileNames.Count; databaseFileNamesIndex++)
					{
						var databaseFileName = databaseFileNames[databaseFileNamesIndex];

						if (System.IO.Path.GetFileNameWithoutExtension(databaseFileName).IndexOf(databaseName, StringComparison.CurrentCultureIgnoreCase) >= 0)
						{
							var sourceFileName = fileNameConverter(databaseFileName);

							databaseFileName = string.Format("{0}\\{1}{2}", System.IO.Path.GetDirectoryName(databaseFileName), System.IO.Path.GetFileNameWithoutExtension(databaseFileName).Replace(databaseName, newRepositoryName), System.IO.Path.GetExtension(databaseFileName));

							var targetFileName = fileNameConverter(databaseFileName);

							System.IO.File.Move(sourceFileName, targetFileName);

							databaseFileNames[databaseFileNamesIndex] = databaseFileName;
						}
					}

					{
						var sql = string.Format(@"CREATE DATABASE [{0}] ON {1} FOR ATTACH", newRepositoryName, string.Join(", ", databaseFileNames.Select(databaseFileName => string.Format(@"( FILENAME = N'{0}' )", databaseFileName))));

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
						{
							command.CommandTimeout = 60 * 10;
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}
					}
				}
			}
		}
	}
}