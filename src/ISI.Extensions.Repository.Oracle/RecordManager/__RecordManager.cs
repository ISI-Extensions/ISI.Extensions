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
using System.Linq.Expressions;
using ISI.Extensions.Oracle.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.Oracle.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.Oracle
{
	public delegate global::Oracle.ManagedDataAccess.Client.OracleConnection GetOracleConnectionDelegate(bool enableMultipleActiveResultSets = false);

	public abstract partial class RecordManager<TRecord> : ISI.Extensions.Repository.RecordManager<TRecord>
		where TRecord : class, IRecordManagerRecord, new()
	{
		protected virtual string ArchiveTableSuffix => "Archive";
		protected virtual string ArchiveTableArchiveDateTimeColumnName => "ArchiveDateTimeUtc";

		protected ISI.Extensions.Repository.Oracle.Configuration OracleConfiguration { get; }

		protected string ConnectionString { get; }
		protected string Schema { get; }
		protected string TableNamePrefix { get; }
		protected string TableName { get; }
		protected string TableAlias { get; }

		protected GetOracleConnectionDelegate GetOracleConnection { get; }

		protected RecordManager(
			Microsoft.Extensions.Configuration.IConfiguration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
			string connectionString,
			string schema = null,
			string tableNamePrefix = null,
			string tableName = null,
			string tableAlias = null,
			GetOracleConnectionDelegate getConnection = null)
			: base(configuration, logger, dateTimeStamper, serializer)
		{
			OracleConfiguration = new ISI.Extensions.Repository.Oracle.Configuration();
			configuration.GetSection(ISI.Extensions.Repository.Oracle.Configuration.ConfigurationSectionName).Bind(OracleConfiguration);

			ConnectionString = Configuration.GetConnectionString(connectionString) ?? connectionString;

			Schema = (string.IsNullOrEmpty(schema) ? RecordDescription.GetRecordDescription<TRecord>().Schema : schema);
			TableNamePrefix = tableNamePrefix;
			TableName = (string.IsNullOrEmpty(tableName) ? RecordDescription.GetRecordDescription<TRecord>().TableName : tableName);
			TableAlias = tableAlias;

			GetOracleConnection = getConnection ?? (enableMultipleActiveResultSets => ISI.Extensions.Oracle.OracleConnection.GetOracleConnection(ConnectionString, enableMultipleActiveResultSets));
		}

		protected override string DefaultOrderByClause => "ORDER BY CURRENT_TIMESTAMP\n";
	}
}