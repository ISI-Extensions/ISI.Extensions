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
using Microsoft.Extensions.Configuration;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;

namespace ISI.Extensions.Repository.SqlServer
{
	public partial class RepositorySetupApi : ISI.Extensions.Repository.IRepositorySetupApi
	{
		public Microsoft.Extensions.Logging.ILogger Logger { get; }
		public ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		public ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; }

		private string _masterConnectionString = null;
		public string MasterConnectionString => _masterConnectionString ??= GetMasterConnectionString();

		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

		public string ConnectionString { get; }
		public string DatabaseName { get; }
		public string Schema { get; }
		public string TableNamePrefix { get; }
		public string CompletedBy { get; }

		public RepositorySetupApi(
			Microsoft.Extensions.Configuration.IConfiguration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
			string connectionString,
			string databaseName = null,
			string schema = null,
			string tableNamePrefix = null,
			string completedBy = null,
			string masterConnectionString = null)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			Serializer = serializer;
			Schema = schema;
			TableNamePrefix = tableNamePrefix;

			ConnectionString = Configuration.GetConnectionString(connectionString) ?? connectionString;

			var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

			if (string.IsNullOrWhiteSpace(databaseName))
			{
				databaseName = connectionStringBuilder.InitialCatalog;
			}
			DatabaseName = databaseName.Replace("[", string.Empty).Replace("]", string.Empty);

			Schema = (schema ?? string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

			if (string.IsNullOrWhiteSpace(completedBy))
			{
				completedBy = connectionStringBuilder.UserID;
			}
			CompletedBy = completedBy;

			_masterConnectionString = masterConnectionString;
		}

		private string GetMasterConnectionString()
		{
			var masterConnectionString = Configuration.GetConnectionString("master");

			if (string.IsNullOrWhiteSpace(masterConnectionString))
			{
				return "master";
			}

			var masterConnectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(masterConnectionString);
			var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

			if (!string.Equals(masterConnectionStringBuilder.DataSource, connectionStringBuilder.DataSource, StringComparison.InvariantCultureIgnoreCase))
			{
				masterConnectionStringBuilder.DataSource = connectionStringBuilder.DataSource;
			}
			
			return masterConnectionStringBuilder.ConnectionString;
		}
	}
}
