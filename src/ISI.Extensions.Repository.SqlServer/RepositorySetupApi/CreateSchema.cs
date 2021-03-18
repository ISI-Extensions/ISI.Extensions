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
using ISI.Extensions.Repository.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public partial class RepositorySetupApi
	{
		public DTOs.CreateRepositoryResponse CreateSchema(string schema = null, string userRoleName = null, string additionalScript = null)
		{
			var response = new DTOs.CreateRepositoryResponse();

			using (var connection = new Microsoft.Data.SqlClient.SqlConnection(MasterConnectionString))
			{
				connection.Open();

				CreateSchema(connection, schema, userRoleName);

				ExecuteScript(connection, additionalScript);
			}

			return response;
		}

		public DTOs.CreateRepositoryResponse CreateSchema(Microsoft.Data.SqlClient.SqlConnection connection, string schema = null, string userRoleName = null)
		{
			var response = new DTOs.CreateRepositoryResponse();

			var sql = new StringBuilder();

			if (string.IsNullOrEmpty(schema))
			{
				schema = Schema;
			}

			#region Create schema
			if (!string.IsNullOrEmpty(schema))
			{
				sql.Clear();
				sql.AppendFormat("use [{0}];\n", DatabaseName);
				sql.AppendFormat("exec('CREATE SCHEMA [{0}]');\n", schema);
				connection.ExecuteNonQueryAsync(sql.ToString()).Wait();
			}
			#endregion

			#region Create userRoleName
			if (!string.IsNullOrEmpty(userRoleName))
			{
				sql.Clear();
				sql.AppendFormat("use [{0}];\n", DatabaseName);
				sql.AppendFormat("CREATE ROLE [{0}] AUTHORIZATION db_securityadmin;\n", userRoleName);
				sql.AppendFormat("GRANT CREATE SCHEMA TO [{0}];\n", userRoleName);
				if (!string.IsNullOrEmpty(schema))
				{
					sql.AppendFormat("GRANT ALTER, SELECT, INSERT, UPDATE, DELETE ON SCHEMA :: [{1}] TO [{0}];\n", userRoleName, schema);
				}
				sql.AppendFormat("GRANT CREATE TABLE TO [{0}];\n", userRoleName);
				connection.ExecuteNonQueryAsync(sql.ToString()).Wait();
			}
			#endregion

			return response;
		}
	}
}
