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
using NUnit.Framework;

namespace ISI.Extensions.Tests.Repository
{
	[TestFixture]
	public partial class PostgreSQL_Tests
	{
		[Test]
		public void CreateTable_Test()
		{
			var recordManager = new EmailRecordManager(ConfigurationRoot, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable(ISI.Extensions.Repository.CreateTableMode.ErrorIfExists);
		}

		public partial class EmailRecordManager : ISI.Extensions.Repository.PostgreSQL.RecordManagerPrimaryKeyWithArchive<EmailRecord, Guid>
		{
			public EmailRecordManager(
				Microsoft.Extensions.Configuration.IConfiguration configuration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				string connectionString)
				: base(configuration, logger, dateTimeStamper, serializer, connectionString)
			{
			}
		}


		[ISI.Extensions.Repository.Record(Schema = "EmailProcessor", TableName = "Emails")]
		public class EmailRecord : ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>, ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime, ISI.Extensions.Repository.IRecordIndexDescriptions<EmailRecord>
		{
			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "EmailUuid")]
			public Guid EmailUuid { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "EmailMailMessageHashCode")]
			public int EmailMailMessageHashCode { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "CreateNodeId")]
			public int? CreateNodeId { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "EmailStatusUuid")]
			public Guid EmailStatusUuid { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ScheduledSendDateTimeUtc")]
			public DateTime? ScheduledSendDateTimeUtc { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "LastAttemptSendDateTimeUtc")]
			public DateTime? LastAttemptSendDateTimeUtc { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ModifyDateTimeUtc")]
			public DateTime ModifyDateTimeUtc { get; set; }

			[ISI.Extensions.Repository.IgnoreRecordProperty]
			Guid ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>.PrimaryKey => EmailUuid;

			[ISI.Extensions.Repository.IgnoreRecordProperty]
			DateTime ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTimeUtc => ModifyDateTimeUtc;

			ISI.Extensions.Repository.RecordIndexCollection<EmailRecord> ISI.Extensions.Repository.IRecordIndexDescriptions<EmailRecord>.GetRecordIndexes()
			{
				return new ISI.Extensions.Repository.RecordIndexCollection<EmailRecord>()
				{
					new ISI.Extensions.Repository.RecordIndex<EmailRecord>()
					{
						Unique = true,
						Columns = (new ISI.Extensions.Repository.RecordIndexColumnCollection<EmailRecord>()
						{
							{ record => record.CreateNodeId },
						}).ToArray(),
					},
					new ISI.Extensions.Repository.RecordIndexColumnCollection<EmailRecord>()
					{
						{ record => record.EmailStatusUuid },
					},
					new ISI.Extensions.Repository.RecordIndexColumnCollection<EmailRecord>()
					{
						{ record => record.ScheduledSendDateTimeUtc },
						{ record => record.EmailStatusUuid },
						{ record => record.LastAttemptSendDateTimeUtc },
					},
				};
			}
		}
	}
}