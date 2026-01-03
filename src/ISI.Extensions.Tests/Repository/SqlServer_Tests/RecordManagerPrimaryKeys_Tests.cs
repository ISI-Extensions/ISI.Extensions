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
using ISI.Extensions.Repository.Extensions;
using NUnit.Framework;

namespace ISI.Extensions.Tests.Repository
{
	[TestFixture]
	public partial class SqlServer_Tests
	{
		[ISI.Extensions.Repository.Record(Schema = "C3", TableName = "ShippingBatchItems")]
		public class ShippingBatchItemRecord : ISI.Extensions.Repository.IRecordManagerRecord, ISI.Extensions.Repository.IRecordIndexDescriptions<ShippingBatchItemRecord>
		{
			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ShippingBatchUuid")]
			public Guid ShippingBatchUuid { get; set; }

			[ISI.Extensions.Repository.PrimaryKey]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "TrackingNumber", PropertySize = 256)]
			public string TrackingNumber { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "SerialNumber", PropertySize = 256)]
			public string SerialNumber { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ModifyDateTimeUtc")]
			public DateTime ModifyDateTimeUtc { get; set; }

			[ISI.Extensions.Repository.RecordProperty(ColumnName = "ModifyUserKey", PropertySize = 256)]
			public string ModifyUserKey { get; set; }

			ISI.Extensions.Repository.RecordIndexCollection<ShippingBatchItemRecord> ISI.Extensions.Repository.IRecordIndexDescriptions<ShippingBatchItemRecord>.GetRecordIndexes()
			{
				return new ISI.Extensions.Repository.RecordIndexCollection<ShippingBatchItemRecord>()
				{
					new ISI.Extensions.Repository.RecordIndexColumnCollection<ShippingBatchItemRecord>()
					{
						{ record => record.TrackingNumber },
					},
					new ISI.Extensions.Repository.RecordIndexColumnCollection<ShippingBatchItemRecord>()
					{
						{ record => record.SerialNumber },
					},
				};
			}
		}

		public class ShippingBatchItemRecordManager : ISI.Extensions.Repository.SqlServer.RecordManager<ShippingBatchItemRecord>
		{
			public ShippingBatchItemRecordManager(
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
		public void CreateTableShippingBatchItemRecord()
		{
			var recordManager = new ShippingBatchItemRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);

			recordManager.CreateTable();
		}

		[Test]
		public void UpsertShippingBatchItemRecord()
		{
			var recordManager = new ShippingBatchItemRecordManager(Configuration, Logger, DateTimeStamper, Serializer, ConnectionString);


			var record = new ShippingBatchItemRecord()
			{
				ShippingBatchUuid = Guid.NewGuid(),
				TrackingNumber = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36),
				SerialNumber = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36),
				ModifyDateTimeUtc = DateTime.UtcNow,
				ModifyUserKey = "Ron",
			};

			recordManager.UpdateRecordsAsync([record]).GetAwaiter().GetResult();

			recordManager.UpdateRecordsAsync([record]).GetAwaiter().GetResult();

		}

	}
}