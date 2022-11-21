#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using SqlServerDTOs = ISI.Extensions.Repository.SqlServer.DataTransferObjects.RepositorySetupApi;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.SqlServer
{
	public partial class RepositorySetupApi
	{
		public SqlServerDTOs.CreateUserResponse CreateUser(string userName, string password = null)
		{
			using (var connection = SqlConnection.GetSqlConnection(MasterConnectionString))
			{
				return CreateUser(connection, userName, password);
			}
		}

		public SqlServerDTOs.CreateUserResponse CreateUser(Microsoft.Data.SqlClient.SqlConnection connection, string userName, string password = null)
		{
			var response = new SqlServerDTOs.CreateUserResponse();

			var sql = new StringBuilder();

			if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
			{
				sql.Clear();
				sql.AppendFormat("IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = '{0}')\n", userName);
				sql.Append("begin\n");
				sql.AppendFormat("  CREATE LOGIN [{0}] WITH PASSWORD = N'{1}';\n", userName, password);
				sql.Append("end\n");
				connection.ExecuteNonQueryAsync(sql.ToString()).Wait();
			}

			if (!string.IsNullOrEmpty(userName))
			{
				sql.Clear();
				sql.AppendFormat("use [{0}];\n", DatabaseName);
				sql.AppendFormat("IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '{0}')\n", userName);
				sql.Append("begin\n");
				sql.AppendFormat("  CREATE USER [{0}] FOR LOGIN [{0}];\n", userName);
				sql.Append("end\n");
				connection.ExecuteNonQueryAsync(sql.ToString()).Wait();
			}

			return response;
		}
	}
}