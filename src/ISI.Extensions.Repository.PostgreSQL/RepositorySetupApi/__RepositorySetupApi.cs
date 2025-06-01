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
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;
using SqlServerDTOs = ISI.Extensions.Repository.PostgreSQL.DataTransferObjects.RepositorySetupApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public partial class RepositorySetupApi : ISI.Extensions.Repository.IRepositorySetupApiWithConfigurationLoggerDateTimeStamper
	{
		public Microsoft.Extensions.Logging.ILogger Logger { get; }
		public ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; }

		private string _masterConnectionStringWithMasterDatabase = null;
		public string MasterConnectionStringWithMasterDatabase => _masterConnectionStringWithMasterDatabase ??= GetMasterConnectionStringWithMasterDatabase();

		private string _masterConnectionString = null;
		public string MasterConnectionString => _masterConnectionString ??= GetMasterConnectionString();

		public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

		public string ConnectionString { get; }
		public string DatabaseName { get; }

		private string _completedBy = null;
		public string CompletedBy => _completedBy ??= GetCompletedBy();

		public RepositorySetupApi(
			Microsoft.Extensions.Configuration.IConfiguration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
			string connectionString,
			string databaseName = null,
			string completedBy = null,
			string masterConnectionString = null)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			Serializer = serializer;

			ConnectionString = Configuration.GetConnectionString(connectionString) ?? connectionString;

			try
			{
				var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
				{
					ConnectionString = ConnectionString,
				};

				if (dbConnectionStringBuilder.ContainsKey("schemaName"))
				{
					dbConnectionStringBuilder.Remove("schemaName");
					ConnectionString = dbConnectionStringBuilder.ConnectionString;
				}
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, $"ConnectString: {connectionString}");
				throw;
			}

			var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(ConnectionString);

			DatabaseName = (string.IsNullOrWhiteSpace(databaseName) ? connectionStringBuilder.Database : databaseName);

			_completedBy = (string.IsNullOrWhiteSpace(completedBy) ? connectionStringBuilder.Username : completedBy);
			if (string.IsNullOrWhiteSpace(_completedBy))
			{
				_completedBy = null;
			}

			if (!string.IsNullOrWhiteSpace(masterConnectionString))
			{
				try
				{
					var dbConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder()
					{
						ConnectionString = masterConnectionString,
					};

					if (dbConnectionStringBuilder.ContainsKey("schemaName"))
					{
						dbConnectionStringBuilder.Remove("schemaName");
						masterConnectionString = dbConnectionStringBuilder.ConnectionString;
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, $"ConnectString: {masterConnectionString}");
					throw;
				}
			}

			_masterConnectionStringWithMasterDatabase = masterConnectionString;
		}
	}
}