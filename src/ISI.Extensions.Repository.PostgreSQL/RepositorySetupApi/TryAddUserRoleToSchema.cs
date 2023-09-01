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
using System.Diagnostics;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using SqlServerDTOs = ISI.Extensions.Repository.PostgreSQL.DataTransferObjects.RepositorySetupApi;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public partial class RepositorySetupApi
	{
		public bool TryAddUserRoleToSchema(string userRole, string schema)
		{
			using (var connection = SqlConnection.GetSqlConnection(MasterConnectionString))
			{
				return TryAddUserRoleToSchema(connection, userRole, schema);
			}
		}

		public bool TryAddUserRoleToSchema(Npgsql.NpgsqlConnection connection, string userRole, string schema)
		{
			try
			{
				connection.EnsureConnectionIsOpenAsync().Wait();

				var sql = new StringBuilder();

				sql.Clear();
				sql.AppendFormat("GRANT {1} TO {0};\n", userRole.PostgreSQLFormatName(), schema.PostgreSQLFormatName());

				connection.ExecuteNonQueryAsync(sql.ToString()).Wait();

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}