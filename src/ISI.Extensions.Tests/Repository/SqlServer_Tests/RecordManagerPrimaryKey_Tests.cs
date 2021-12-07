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
using ISI.Extensions.Extensions;
using NUnit.Framework;

namespace ISI.Extensions.Tests.Repository
{
	[TestFixture]
	public partial class SqlServer_Tests
	{
		[ISI.Extensions.Repository.Record(Schema = "C2", TableName = "ContactWithUuid")]
		public class Contact : ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>, ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime, ISI.Extensions.Repository.IRecordIndexDescriptions<Contact>
		{
			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.Identity]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ContactUuid")]
			public Guid ContactUuid { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "FirstName")]
			public string FirstName { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "LastName")]
			public string LastName { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "TimeStamp")]
			public DateTime TimeStamp { get; set; }

			Guid ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>.PrimaryKey => ContactUuid;

			DateTime ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTime => TimeStamp;

			ISI.Extensions.Repository.RecordIndexCollection<Contact> ISI.Extensions.Repository.IRecordIndexDescriptions<Contact>.GetRecordIndexes()
			{
				return new ISI.Extensions.Repository.RecordIndexCollection<Contact>
				{
					{
						new ISI.Extensions.Repository.RecordIndexColumnCollection<Contact>()
						{
							{record => FirstName},
							{record => LastName},
						},
						true
					},
				};
			}
		}

		public class ContactRecordManager : ISI.Extensions.Repository.SqlServer.RecordManagerPrimaryKeyWithArchive<Contact, Guid>
		{
			public ContactRecordManager(
				Microsoft.Extensions.Configuration.IConfiguration configuration,
				ISI.Extensions.Repository.SqlServer.Configuration sqlServerConfiguration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				string connectionString)
				: base(configuration, sqlServerConfiguration, logger, dateTimeStamper, serializer, connectionString)
			{

			}
		}


		[Test]
		public void CreateTableContactRecord()
		{
			var recordManager = new ContactRecordManager(Configuration, SqlServerConfiguration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable();
		}

		[Test]
		public void Get()
		{
			var recordManager = new ContactRecordManager(Configuration, SqlServerConfiguration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new Contact()
			{
				//ContactUuid = Guid.NewGuid(),
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTimeStamper.CurrentDateTimeUtc(),
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			var testContact = recordManager.GetRecordAsync(contact.ContactUuid).GetAwaiter().GetResult();

			Assert.AreEqual(contact.ContactUuid, testContact.ContactUuid);

			var noContacts = recordManager.GetRecordsAsync(Array.Empty<Guid>()).ToEnumerable();

			Assert.True(noContacts != null);

			Assert.False(noContacts.Any());
		}

		[Test]
		public void Insert()
		{
			var recordManager = new ContactRecordManager(Configuration, SqlServerConfiguration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new Contact()
			{
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTimeStamper.CurrentDateTimeUtc(),
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			recordManager.InsertRecordAsync(null).GetAwaiter().GetResult();

			recordManager.InsertRecordsAsync(null).ToEnumerable();
		}

		[Test]
		public void Update()
		{
			var recordManager = new ContactRecordManager(Configuration, SqlServerConfiguration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new Contact()
			{
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTimeStamper.CurrentDateTimeUtc(),
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			contact.FirstName = "U1";
			recordManager.UpdateRecordAsync(contact).GetAwaiter().GetResult();
		}

		[Test]
		public void List()
		{
			var recordManager = new ContactRecordManager(Configuration, SqlServerConfiguration, Logger, DateTimeStamper, Serializer, ConnectionString);

			foreach (var record in recordManager.ListRecordsAsync().ToEnumerable())
			{
				Console.WriteLine($"{record.FirstName} {record.LastName}");
			}
		}
	}
}