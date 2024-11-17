#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.SqlServer.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.SqlServer.DataTransferObjects.BackupManager;

namespace ISI.Extensions.SqlServer
{
	public partial class BackupManager
	{
		public DTOs.ListDatabasesResponse ListDatabases(DTOs.IListDatabasesRequest request)
		{
			var response = new DTOs.ListDatabasesResponse();
			
			var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder();
			switch (request)
			{
				case DTOs.ListDatabasesRequest listDatabasesRequest:
					//connectionStringBuilder.DataSource = listDatabasesRequest.Port.HasValue ? $"{listDatabasesRequest.Host}:{listDatabasesRequest.Port}" : listDatabasesRequest.Host;
					connectionStringBuilder.DataSource = listDatabasesRequest.Host;
					connectionStringBuilder.UserID = listDatabasesRequest.UserName;
					connectionStringBuilder.Password = listDatabasesRequest.Password;
					break;

				case DTOs.ListDatabasesUsingConnectionStringRequest listDatabasesUsingConnectionStringRequest:
					connectionStringBuilder.ConnectionString = listDatabasesUsingConnectionStringRequest.ConnectionString;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			connectionStringBuilder.TrustServerCertificate = true;

			Logger.LogInformation($"ServerName: {connectionStringBuilder.GetServerName()}");
			//Logger.LogInformation($"ServerPort: {connectionStringBuilder.GetServerPort()}");
			//Logger.LogInformation($"ConnectionString: {connectionStringBuilder.ConnectionString}");

			using (var connection = ISI.Extensions.SqlServer.SqlConnection.GetSqlConnection(connectionStringBuilder.ConnectionString))
			{
				connection.Open();

				var sql = new StringBuilder();
				sql.AppendLine($"SELECT name FROM master.sys.databases");

				using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql.ToString(), connection))
				{
					using (var reader = command.ExecuteReader())
					{
						var databases = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet();
						
						while (reader.Read())
						{
							databases.Add(reader.GetString(0));
						}

						response.Databases = databases.ToArray();
					}
				}
			}

			return response;
		}
	}
}