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
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Repository.Oracle
{
	public partial class RepositorySetupApi
	{
		private string GetMasterConnectionStringWithMasterDatabase()
		{
			var connectionString = Configuration.GetConnectionString("master");

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				return "master";
			}

			try
			{
				var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
				{
					ConnectionString = connectionString,
				};

				if (dbConnectionStringBuilder.ContainsKey("schemaName"))
				{
					dbConnectionStringBuilder.Remove("schemaName");
					connectionString = dbConnectionStringBuilder.ConnectionString;
				}
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, $"ConnectString: {connectionString}");
				throw;
			}

			var masterConnectionStringBuilder = new global::Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(connectionString);
			var connectionStringBuilder = new global::Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(ConnectionString);

			masterConnectionStringBuilder.TryGetValue("SERVER", out var masterServerValue);
			var masterServer = $"{masterServerValue}";
			connectionStringBuilder.TryGetValue("SERVER", out var serverValue);
			var server = $"{serverValue}";

			if (!string.IsNullOrWhiteSpace(server) && !string.Equals(masterServer, server, StringComparison.InvariantCultureIgnoreCase))
			{
				masterServer = server;
			}

			return masterConnectionStringBuilder.ConnectionString;
		}

		private string GetMasterConnectionString()
		{
			var connectionString = Configuration.GetConnectionString("master");

			if (string.IsNullOrWhiteSpace(connectionString))
			{
				return "master";
			}

			try
			{
				var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
				{
					ConnectionString = connectionString,
				};

				if (dbConnectionStringBuilder.ContainsKey("schemaName"))
				{
					dbConnectionStringBuilder.Remove("schemaName");
					connectionString = dbConnectionStringBuilder.ConnectionString;
				}
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, $"ConnectString: {connectionString}");
				throw;
			}

			var masterConnectionStringBuilder = new global::Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(connectionString);
			var connectionStringBuilder = new global::Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(ConnectionString);

			masterConnectionStringBuilder.TryGetValue("SERVER", out var masterServerValue);
			var masterServer = $"{masterServerValue}";
			connectionStringBuilder.TryGetValue("SERVER", out var serverValue);
			var server = $"{serverValue}";

			if (!string.IsNullOrWhiteSpace(server) && !string.Equals(masterServer, server, StringComparison.InvariantCultureIgnoreCase))
			{
				masterServer = server;
			}

			if (!string.Equals(masterConnectionStringBuilder.DataSource, connectionStringBuilder.DataSource, StringComparison.InvariantCultureIgnoreCase))
			{
				masterConnectionStringBuilder.DataSource = connectionStringBuilder.DataSource;
			}

			return masterConnectionStringBuilder.ConnectionString;
		}
	}
}