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
	public partial class SqlServer_Tests
	{
		public class ContactWithDataRecordManager : ISI.Extensions.Repository.SqlServer.RecordManagerPrimaryKeyWithArchive<ContactWithData, Guid>
		{
			public ContactWithDataRecordManager(
				Microsoft.Extensions.Configuration.IConfiguration configuration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				string connectionString)
				: base(configuration, logger, dateTimeStamper, serializer, connectionString)
			{

			}
		}

		[Test]
		public void GetRecordDescription()
		{
			var recordPropertyDescription = ISI.Extensions.Repository.RecordDescription.GetRecordDescription<ContactWithData>();
		}

		[Test]
		public void CreateTableContactWithDataRecord()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable();
		}

		[Test]
		public void ContactWithDataV1_Get()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contactV1 = new ContactWithData()
			{
				ContactUuid = Guid.NewGuid(),
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTime.UtcNow,
			};

			contactV1.ContactData = new ContactDataV1()
			{
				ContactUuid = contactV1.ContactUuid,
				FirstName = contactV1.FirstName,
				LastName = contactV1.LastName,
				AddressLine1 = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				AddressLine2 = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				City = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				State = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				Zip = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
			};

			recordManager.InsertRecordAsync(contactV1).GetAwaiter().GetResult();

			var testContact = recordManager.GetRecordAsync(contactV1.ContactUuid).GetAwaiter().GetResult();

			Assert.That(contactV1.ContactUuid == testContact.ContactUuid);

			var noContacts = recordManager.GetRecordsAsync(Array.Empty<Guid>()).ToEnumerable();

			Assert.That(noContacts != null);

			Assert.That(!noContacts.Any());
		}

		[Test]
		public void ContactWithDataV2_Get()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contactV2 = new ContactWithData()
			{
				ContactUuid = Guid.NewGuid(),
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTime.UtcNow,
			};

			contactV2.ContactData = new ContactDataV2()
			{
				ContactUuid = contactV2.ContactUuid,
				FirstName = contactV2.FirstName,
				LastName = contactV2.LastName,
				Title = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				AddressLine1 = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				AddressLine2 = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				City = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				State = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				Zip = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				PhoneNumber = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
			};

			recordManager.InsertRecordAsync(contactV2).GetAwaiter().GetResult();

			var testContact = recordManager.GetRecordAsync(contactV2.ContactUuid).GetAwaiter().GetResult();

			Assert.That(contactV2.ContactUuid == testContact.ContactUuid);

			var noContacts = recordManager.GetRecordsAsync(Array.Empty<Guid>()).ToEnumerable();

			Assert.That(noContacts != null);

			Assert.That(!noContacts.Any());
		}

		[Test]
		public void ContactWithData_Insert()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new ContactWithData()
			{
				ContactUuid = Guid.NewGuid(),
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTime.UtcNow,
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			recordManager.InsertRecordAsync(null).GetAwaiter().GetResult();

			recordManager.InsertRecordsAsync(null).ToEnumerable();
		}

		[Test]
		public void ContactWithData_Update()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new ContactWithData()
			{
				ContactUuid = Guid.NewGuid(),
				FirstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTime.UtcNow,
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			contact.FirstName = "U1";
			recordManager.UpdateRecordAsync(contact).GetAwaiter().GetResult();
		}

		[Test]
		public void ContactWithData_List()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			foreach (var record in recordManager.ListRecordsAsync().ToEnumerable())
			{
				Console.WriteLine($"{record.FirstName} {record.LastName}");
			}
		}

		[Test]
		public void UpsertRecordsAsync_Test()
		{
			var recordManager = new ContactWithDataRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable(ISI.Extensions.Repository.CreateTableMode.DeleteAndCreateIfExists);

			var records = new List<ContactWithData>();

			for (var index = 0; index < 100; index++)
			{
				var contactUuid = Guid.NewGuid();
				var firstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
				var lastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
				
				records.Add(new()
				{
					ContactUuid = contactUuid,
					FirstName = firstName,
					LastName = lastName,
					TimeStamp = DateTime.UtcNow,
					ContactData = new ContactDataV1()
					{
						ContactUuid = contactUuid,
						FirstName = firstName,
						LastName = lastName,
					}
				});
			}

			var insertedRecords = recordManager.UpsertRecordsAsync(records).ToEnumerable();


		}
	}
}