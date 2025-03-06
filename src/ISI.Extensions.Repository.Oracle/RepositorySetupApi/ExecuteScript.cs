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
using ISI.Extensions.Oracle.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.Oracle.Extensions;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using SqlServerDTOs = ISI.Extensions.Repository.Oracle.DataTransferObjects.RepositorySetupApi;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.Oracle
{
	public partial class RepositorySetupApi
	{
		public DTOs.ExecuteScriptResponse ExecuteScript(string script, IDictionary<string, object> parameters = null)
		{
			using (var connection = ISI.Extensions.Oracle.OracleConnection.GetOracleConnection(ConnectionString))
			{
				connection.Open();

				return ExecuteScript(connection, script, parameters);
			}
		}

		internal DTOs.ExecuteScriptResponse ExecuteScript(global::Oracle.ManagedDataAccess.Client.OracleConnection connection, string script, IDictionary<string, object> parameters = null)
		{
			var response = new DTOs.ExecuteScriptResponse();

			if (!string.IsNullOrEmpty(script))
			{
				var replacementValues = GetReplacementValues();

				script = script.Replace(replacementValues);

				var currentDatabase = ((new global::Oracle.ManagedDataAccess.Client.OracleCommand("SELECT current_database();", connection)).ExecuteScalar() as string).Trim();

				var sql = new StringBuilder();

				if (!string.Equals(DatabaseName, currentDatabase, StringComparison.InvariantCultureIgnoreCase))
				{
					sql.AppendFormat("\\connect {0};\n", DatabaseName.OracleFormatName());
				}
				sql.AppendFormat("{0}\n", script);

				using (var command = new global::Oracle.ManagedDataAccess.Client.OracleCommand(sql.ToString(), connection))
				{
					command.AddParameters(parameters);

					command.CommandTimeout = 60 * 10;
					command.ExecuteNonQueryWithExceptionTracingAsync().ContinueWith(executeResponse => response.RowAffected = executeResponse.Result).Wait();
				}
			}

			return response;
		}
	}
}