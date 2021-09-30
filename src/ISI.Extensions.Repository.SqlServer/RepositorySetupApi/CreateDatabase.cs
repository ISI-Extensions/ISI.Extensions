#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public partial class RepositorySetupApi
	{
		public ISI.Extensions.Repository.SqlServer.DataTransferObjects.RepositorySetupApi.CreateDatabaseResponse CreateDatabase(string dataFileDirectory = null, string logFileDirectory = null)
		{
			using (var connection = SqlConnection.GetSqlConnection(MasterConnectionString))
			{
				return CreateDatabase(connection, dataFileDirectory, logFileDirectory);
			}
		}
		
		public ISI.Extensions.Repository.SqlServer.DataTransferObjects.RepositorySetupApi.CreateDatabaseResponse CreateDatabase(Microsoft.Data.SqlClient.SqlConnection connection, string dataFileDirectory = null, string logFileDirectory = null)
		{
			var response = new ISI.Extensions.Repository.SqlServer.DataTransferObjects.RepositorySetupApi.CreateDatabaseResponse();

			var sql = new StringBuilder();

			if (string.IsNullOrEmpty(dataFileDirectory) || string.IsNullOrEmpty(logFileDirectory))
			{
				sql.Clear();
				sql.Append(@"
set nocount on

declare @defaultDataDirectory nvarchar(4000)
declare @defaultLogDirectory nvarchar(4000) 

exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultData', @defaultDataDirectory output, 'no_output'

if (@defaultDataDirectory is null) 
begin 
	exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\Setup', N'SQLDataRoot', @defaultDataDirectory output, 'no_output' 
	select @defaultDataDirectory = @defaultDataDirectory + N'\Data' 
end

exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultLog', @defaultLogDirectory output, 'no_output'

if (@defaultLogDirectory is null) 
begin 
	exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\Setup', N'SQLLogRoot', @defaultLogDirectory output, 'no_output' 
	select @defaultLogDirectory = @defaultLogDirectory + N'\Data' 
end

if (@defaultLogDirectory is null) 
begin 
	select @defaultLogDirectory = @defaultDataDirectory
end

select @defaultDataDirectory as DefaultDataDirectory, @defaultLogDirectory as DefaultLogDirectory
");

				using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), connection))
				{
					using (var dataReader = command.ExecuteReaderWithExceptionTracingAsync().GetAwaiter().GetResult())
					{
						if (dataReader.Read())
						{
							dataFileDirectory = dataReader.GetString("DefaultDataDirectory");
							logFileDirectory = dataReader.GetString("DefaultLogDirectory");
						}
					}
				}
			}

			sql.Clear();
			sql.AppendFormat("CREATE DATABASE [{0}]\n", DatabaseName);
			sql.AppendFormat("  ON PRIMARY (NAME = N'{0}.Data', FILENAME = N'{1}')\n", DatabaseName, System.IO.Path.Combine(dataFileDirectory, string.Format("{0}.mdf", DatabaseName)));
			sql.AppendFormat("  LOG ON (NAME = N'{0}.Log', FILENAME = N'{1}');\n", DatabaseName, System.IO.Path.Combine(logFileDirectory, string.Format("{0}.ldf", DatabaseName)));
			connection.ExecuteNonQueryAsync(sql.ToString()).Wait();

			return response;
		}
	}
}
