#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
		public DTOs.GetLatestStepResponse GetLatestStep()
		{
			var response = new DTOs.GetLatestStepResponse();

			var success = false;

			foreach (var connectionString in new[] { MasterConnectionString, ConnectionString })
			{
				if (!success)
				{
					try
					{
						if (ISI.Extensions.SqlServer.SqlConnection.TryGetSqlConnection(connectionString, false, out var connection))
						{
							connection.Open();

							var sql = new StringBuilder();

							sql.Append("set nocount on\n");
							sql.AppendFormat("if db_id('{0}') is null\n", DatabaseName);
							sql.Append("begin\n");
							sql.Append("	select 0 as StepId\n");
							sql.Append("end\n");
							sql.Append("else\n");
							sql.Append("begin\n");
							sql.AppendFormat("  if (exists (select 1 from [{0}].INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'DatabaseMigrationStep'))\n", DatabaseName);
							sql.Append("  begin\n");
							sql.Append("		select max(StepId) as StepId\n");
							sql.AppendFormat("		from [{0}].dbo.DatabaseMigrationStep\n", DatabaseName);
							sql.Append("  end\n");
							sql.Append("  else\n");
							sql.Append("  begin\n");
							sql.Append("	  select 0 as StepId\n");
							sql.Append("  end\n");
							sql.Append("end\n");

							using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), connection))
							{
								response.StepId = $"{command.ExecuteScalarWithExceptionTracingAsync().GetAwaiter().GetResult()}".ToInt();
								success = true;
							}

							connection.Dispose();
						}
					}
					catch (Exception exception)
					{

					}
				}
			}

			return response;
		}
	}
}