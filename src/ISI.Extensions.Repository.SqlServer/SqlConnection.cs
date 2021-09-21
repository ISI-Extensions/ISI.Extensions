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
using System.Text;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public class SqlConnection
	{
		public static Microsoft.Data.SqlClient.SqlConnection GetSqlConnection(string connectionString, bool enableMultipleActiveResultSets = false)
		{
			var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
			{
				ConnectionString = connectionString,	
			};

			var appRoleName = dbConnectionStringBuilder.GetValue(ISI.Extensions.Repository.ConnectionStringParameterName.AppRoleName, true) ?? string.Empty;
			var appRolePassword = dbConnectionStringBuilder.GetValue(ISI.Extensions.Repository.ConnectionStringParameterName.AppRolePassword, true) ?? string.Empty;

			if (enableMultipleActiveResultSets)
			{
				dbConnectionStringBuilder.Add("MultipleActiveResultSets", "true");
			}

			var connection = new Microsoft.Data.SqlClient.SqlConnection(dbConnectionStringBuilder.ConnectionString);

			connection.StateChange += (sender, args) =>
			{
				if ((args.OriginalState == System.Data.ConnectionState.Closed) ||
				    (args.OriginalState == System.Data.ConnectionState.Connecting) &&
				    (args.CurrentState == System.Data.ConnectionState.Open))
				{
					if (!string.IsNullOrWhiteSpace(appRoleName) && !string.IsNullOrWhiteSpace(appRolePassword))
					{
						using (var command = new Microsoft.Data.SqlClient.SqlCommand("sp_setapprole", connection))
						{
							command.CommandType = System.Data.CommandType.StoredProcedure;

							command.AddParameter("@rolename", appRoleName);
							command.AddParameter("@password", appRolePassword);

							command.CommandTimeout = 0;
							command.ExecuteNonQuery();
						}
					}
				}
			};

			return connection;
		}
	}
}
