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
using Microsoft.Extensions.Configuration;
using DTOs = ISI.Extensions.Repository.DataTransferObjects.RepositorySetupApi;

namespace ISI.Extensions.Repository.Cosmos
{
	public partial class RepositorySetupApi : ISI.Extensions.Repository.IRepositorySetupApiWithConfigurationLoggerDateTimeStamper
	{
		public const string DatabaseMigrationStepTableName = "DatabaseMigrationStep";

		public Microsoft.Extensions.Logging.ILogger Logger { get; }
		public ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

		public string ConnectionString { get; }
		public string DatabaseName { get; }
		public string AccountEndpoint { get; }
		public string AccountKey { get; }
		public string CompletedBy { get; }

		public RepositorySetupApi(
			Microsoft.Extensions.Configuration.IConfiguration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			string connectionString,
			string databaseName = null,
			string completedBy = null)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;

			ConnectionString = Configuration.GetConnectionString(connectionString) ?? connectionString;

			var connectionStringBuilder = new CosmosConnectionStringBuilder(ConnectionString);

			AccountEndpoint = connectionStringBuilder.AccountEndpoint;
			AccountKey = connectionStringBuilder.AccountKey;

			DatabaseName = (string.IsNullOrWhiteSpace(databaseName) ? connectionStringBuilder.DatabaseName : databaseName);
			CompletedBy = completedBy;
		}

		public DTOs.DeleteRepositoryResponse DeleteRepository()
		{
			throw new NotImplementedException();
		}

		public DTOs.ExecuteScriptResponse ExecuteScript(string script, IDictionary<string, object> parameters = null)
		{
			throw new NotImplementedException();
		}
	}
}