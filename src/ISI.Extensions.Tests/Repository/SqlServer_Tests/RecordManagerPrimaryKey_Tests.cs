#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "FirstName", PropertySize = 256)]
			public string FirstName { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "LastName", PropertySize = 256)]
			public string LastName { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "TimeStamp")]
			public DateTime TimeStamp { get; set; }

			Guid ISI.Extensions.Repository.IRecordManagerPrimaryKeyRecord<Guid>.PrimaryKey => ContactUuid;

			DateTime ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTime => TimeStamp;

			ISI.Extensions.Repository.RecordIndexCollection<Contact> ISI.Extensions.Repository.IRecordIndexDescriptions<Contact>.GetRecordIndexes()
			{
				return new()
				{
					{
						new()
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
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				string connectionString)
				: base(configuration, logger, dateTimeStamper, serializer, connectionString)
			{

			}

			public async IAsyncEnumerable<Contact> FindRecordsByFirstNameAsync(IEnumerable<string> firstNames, int skip = 0, int take = -1)
			{
				var filters = new ISI.Extensions.Repository.RecordWhereColumnCollection<Contact>();

				filters.Add(record => record.FirstName, ISI.Extensions.Repository.WhereClauseEqualityOperator.Equal, firstNames);

				var whereClause = GenerateWhereClause(filters);

				await foreach (var record in FindRecordsAsync(whereClause))
				{
					yield return record;
				}
			}
		}

		[ISI.Extensions.Repository.Record(Schema = "Nuget", TableName = "Nupkgs")]
		public class NupkgRecord : ISI.Extensions.Repository.IRecordManagerRecord, ISI.Extensions.Repository.IRecordIndexDescriptions<NupkgRecord>, ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime
		{
			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "Package", PropertySize = 256)]
			public string Package { get; set; }

			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "Version", PropertySize = 256)]
			public string Version { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "Description")]
			public string Description { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "IsActive")]
			public bool IsActive { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ModifyDateTimeUtc")]
			public DateTime ModifyDateTimeUtc { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "IsActiveVersion")]
			public bool IsActiveVersion { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "CommitUuid")]
			public Guid CommitUuid { get; set; }

			DateTime ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTime => ModifyDateTimeUtc;

			ISI.Extensions.Repository.RecordIndexCollection<NupkgRecord> ISI.Extensions.Repository.IRecordIndexDescriptions<NupkgRecord>.GetRecordIndexes()
			{
				return new()
				{
					{
						new ISI.Extensions.Repository.RecordIndexColumnCollection<NupkgRecord>()
						{
							{ record => record.Package },
							{ record => record.Version },
						},
						true
					},
					{
						new ISI.Extensions.Repository.RecordIndexColumnCollection<NupkgRecord>()
						{
							{ record => record.Package },
							{ record => record.IsActive },
						}
					},
					{
						new ISI.Extensions.Repository.RecordIndexColumnCollection<NupkgRecord>()
						{
							{ record => record.Package },
							{ record => record.IsActiveVersion },
						}
					},
					{
						new ISI.Extensions.Repository.RecordIndexColumnCollection<NupkgRecord>()
						{
							{ record => record.CommitUuid },
						}
					},
				};
			}
		}

		public class NupkgKey
		{
			public string Package { get; set; }
			public string Version { get; set; }
		}

		public partial class NupkgRecordManager : ISI.Extensions.Repository.SqlServer.RecordManagerWithArchive<NupkgRecord>
		{
			public NupkgRecordManager(
				Microsoft.Extensions.Configuration.IConfiguration configuration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				string connectionString)
				: base(configuration, logger, dateTimeStamper, serializer, connectionString)
			{

			}

			public async IAsyncEnumerable<NupkgRecord> GetRecordsAsync(IEnumerable<NupkgKey> nupkgKeys, int skip = 0, int take = -1)
			{
				var filters = new ISI.Extensions.Repository.RecordWhereColumnCollection<NupkgRecord>(ISI.Extensions.Repository.WhereClauseOperator.Or);

				if (nupkgKeys.NullCheckedAny())
				{
					foreach (var nupkgKey in nupkgKeys)
					{
						var nupkgKeyFilters = new ISI.Extensions.Repository.RecordWhereColumnCollection<NupkgRecord>();
						nupkgKeyFilters.Add(record => record.Package, ISI.Extensions.Repository.WhereClauseEqualityOperator.Equal, nupkgKey.Package);
						nupkgKeyFilters.Add(record => record.Version, ISI.Extensions.Repository.WhereClauseEqualityOperator.Equal, nupkgKey.Version);
						filters.Add(nupkgKeyFilters);
					}
				}

				var whereClause = GenerateWhereClause(filters);

				await foreach (var record in FindRecordsAsync(whereClause))
				{
					yield return record;
				}
			}

			public async IAsyncEnumerable<NupkgRecord> FindRecordsAsync(IEnumerable<string> packages, IEnumerable<string> packagePrefixes, IEnumerable<Guid> commitUuids, bool isActiveVersionOnly)
			{
				var filters = new ISI.Extensions.Repository.RecordWhereColumnCollection<NupkgRecord>();

				filters.AddIfNullCheckedAny(record => record.Package, ISI.Extensions.Repository.WhereClauseEqualityOperator.Equal, packages);

				if (packagePrefixes.NullCheckedAny())
				{
					var packageFilters = new ISI.Extensions.Repository.RecordWhereColumnCollection<NupkgRecord>(ISI.Extensions.Repository.WhereClauseOperator.Or);

					foreach (var packagePrefix in packagePrefixes)
					{
						packageFilters.Add(record => record.Package, ISI.Extensions.Repository.WhereClauseStringComparisonOperator.BeginsWith, packagePrefix);
					}

					filters.Add(packageFilters);
				}

				filters.AddIfNullCheckedAny(record => record.CommitUuid, ISI.Extensions.Repository.WhereClauseEqualityOperator.Equal, commitUuids);

				if (isActiveVersionOnly)
				{
					filters.Add(record => record.IsActiveVersion, ISI.Extensions.Repository.WhereClauseEqualityOperator.NotEqual, false);
				}

				var whereClause = GenerateWhereClause(filters);

				await foreach (var record in FindRecordsAsync(whereClause))
				{
					yield return record;
				}
			}
		}


		[Test]
		public void CreateTableNupkgRecord()
		{
			var recordManager = new NupkgRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable();
		}

		[Test]
		public void FindNupkg()
		{
			var recordManager = new NupkgRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var contact = new NupkgRecord()
			{
				Package = "ISI.Extensions.VisualStudio",
				Version = "10.0.8464.5254",
				Description = "ISI.Extensions.VisualStudio",
				IsActive = true,
				ModifyDateTimeUtc = DateTimeStamper.CurrentDateTimeUtc(),
				IsActiveVersion = true,
				CommitUuid = Guid.NewGuid(),
			};

			//recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			var testContact = recordManager.GetRecordsAsync(new[]
			{
				//new NupkgKey()
				//{
				//	Package = "accumailgoldconnections.nettoolkit",
				//	Version = "1.0.4.0",
				//}
				new NupkgKey()
				{
					Package = "ISI.Extensions.VisualStudio",
					Version = "10.0.8464.5254",
				}
			}).ToEnumerable().NullCheckedFirstOrDefault();

			Assert.AreEqual(testContact.Package, "ISI.Extensions.VisualStudio");
			Assert.AreEqual(testContact.Version, "10.0.8464.5254");
		}



		[Test]
		public void CreateTableContactRecord()
		{
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable();
		}

		[Test]
		public void Get()
		{
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

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
		public void FindFirstName()
		{
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			var firstName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);

			var contact = new Contact()
			{
				//ContactUuid = Guid.NewGuid(),
				FirstName = firstName,
				LastName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens),
				TimeStamp = DateTimeStamper.CurrentDateTimeUtc(),
			};

			recordManager.InsertRecordAsync(contact).GetAwaiter().GetResult();

			var testContact = recordManager.FindRecordsByFirstNameAsync(new[] { firstName }).ToEnumerable().NullCheckedFirstOrDefault();

			Assert.AreEqual(contact.ContactUuid, testContact.ContactUuid);

			var noContacts = recordManager.GetRecordsAsync(Array.Empty<Guid>()).ToEnumerable();

			Assert.True(noContacts != null);

			Assert.False(noContacts.Any());
		}

		[Test]
		public void Insert()
		{
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

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
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

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
			var recordManager = new ContactRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			foreach (var record in recordManager.ListRecordsAsync().ToEnumerable())
			{
				Console.WriteLine($"{record.FirstName} {record.LastName}");
			}
		}
	}
}