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
		public DTOs.SetStepResponse SetStep(int stepId)
		{
			var response = new DTOs.SetStepResponse();

			var success = false;

			foreach (var connectionString in new[] { MasterConnectionString, ConnectionString })
			{
				if (!success)
				{
					if (ISI.Extensions.Oracle.OracleConnection.TryGetOracleConnection(connectionString, false, out var connection))
					{
						connection.Open();

						var sql = new StringBuilder();

						var tableExists = DoesDatabaseMigrationStepTableExist(connection);

						if (!tableExists)
						{
							sql.Clear();
							sql.Append("CREATE TABLE \"DatabaseMigrationStep\"\n");
							sql.Append("(\n");
							sql.Append("	\"StepId\" INT NOT NULL CONSTRAINT \"PK_DatabaseMigrationStep\" PRIMARY KEY,\n");
							sql.Append("	\"CompletedDateTimeUtc\" timestamp NOT NULL,\n");
							sql.Append("	\"CompletedByKey\" VARCHAR2(255) NULL\n");
							sql.Append(")\n");
							connection.ExecuteNonQueryAsync(sql.ToString()).Wait();
						}

						sql.Clear();
						sql.Append("INSERT INTO \"DatabaseMigrationStep\" (\"StepId\", \"CompletedDateTimeUtc\", \"CompletedByKey\")\n");
						sql.Append("SELECT :StepId, sysdate, :CompletedByKey FROM DUAL\n");

						using (var command = new global::Oracle.ManagedDataAccess.Client.OracleCommand(sql.ToString(), connection))
						{
							command.AddParameter("StepId", stepId);
							command.AddParameter("CompletedByKey", (string.IsNullOrWhiteSpace(CompletedBy) ? (object)DBNull.Value : (object)CompletedBy));
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						connection.Dispose();
						connection = null;

						success = true;
					}
				}
			}

			return response;
		}
	}
}