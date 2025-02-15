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
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.SqlServer.Extensions;
using ISI.Extensions.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public abstract partial class RecordManager<TRecord>
	{
		public virtual void BulkInsertRecords(IEnumerable<TRecord> records, bool keepIdentities = false, int bulkCopyTimeoutInSeconds = 3600, int batchSize = 1000, Action<string> batchLogger = null)
		{
			using (var connection = GetSqlConnection())
			{
				BulkInsertRecords(connection, records, keepIdentities, false, bulkCopyTimeoutInSeconds, batchSize, batchLogger);
			}
		}

		public virtual void BulkInsertRecords(Microsoft.Data.SqlClient.SqlConnection connection, IEnumerable<TRecord> records, bool keepIdentities = false, int bulkCopyTimeoutInSeconds = 3600, int batchSize = 1000, Action<string> batchLogger = null)
		{
			BulkInsertRecords(connection, records, keepIdentities, false, bulkCopyTimeoutInSeconds, batchSize, batchLogger);
		}

		protected void BulkInsertRecords(Microsoft.Data.SqlClient.SqlConnection connection, IEnumerable<TRecord> records, bool keepIdentities, bool hasArchiveTable, int bulkCopyTimeoutInSeconds = 3600, int batchSize = 1000, Action<string> batchLogger = null)
		{
			if (hasArchiveTable && !keepIdentities)
			{
				throw new("Cannot ignore Identity on when there is an archive table");
			}

			var columns = new ISI.Extensions.Columns.ColumnCollection<TRecord>();

			if (RecordDescription.GetRecordDescription<TRecord>().HasLocalClusteringIndex)
			{
				var localClusteringIndexIdPropertyDescription = RecordDescription.GetRecordDescription<HasLocalClusteringIndexRecord>().PropertyDescriptions.FirstOrDefault() as IRecordPropertyDescription;

				columns.Add(new ISI.Extensions.Columns.Column<TRecord, object>(
					localClusteringIndexIdPropertyDescription.ColumnName,
					record => true,
					record => null
				));
			}

			foreach (var propertyDescription in RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions)
			{
				if (!keepIdentities && propertyDescription.RepositoryAssignedValueAttribute is IdentityAttribute)
				{
					columns.Add(new ISI.Extensions.Columns.Column<TRecord, object>(
						propertyDescription.ColumnName,
						record => true,
						record => null
					));
				}
				else
				{
					columns.Add(new ISI.Extensions.Columns.Column<TRecord, object>(
						propertyDescription.ColumnName,
						record => propertyDescription.PropertyInfo.GetValue(record) == null,
						record =>
						{
							var value = propertyDescription.GetValue(record);

							if (propertyDescription.CanBeSerialized && (value != null))
							{
								value = Serializer.Serialize(propertyDescription.ValueType, value, true);
							}

							return value;
						}
					));
				}
			}

			connection.EnsureConnectionIsOpen();

			{
				var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<TRecord>(records, columns, null);

				using (var target = new Microsoft.Data.SqlClient.SqlBulkCopy(connection, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, null))
				{
					target.DestinationTableName = GetTableName(addAlias: false);
					target.BulkCopyTimeout = bulkCopyTimeoutInSeconds;
					target.BatchSize = batchSize;

					target.NotifyAfter = batchSize;
					if (batchLogger != null)
					{
						target.SqlRowsCopied += (sender, args) => batchLogger(string.Format("{0} records inserted", args.RowsCopied));
					}

					target.WriteToServer(dataReader);

					batchLogger?.Invoke(string.Format("{0} records inserted", target.RowsCopied));
				}
			}

			if (hasArchiveTable)
			{
				if (RecordDescription.GetRecordDescription<TRecord>().HasLocalClusteringIndex)
				{
					columns.RemoveAt(0);
				}

				var archiveDateTimeUtc = DateTimeStamper.CurrentDateTimeUtc();
				
				columns.Insert(0, new ISI.Extensions.Columns.Column<TRecord, DateTime>(
					ArchiveTableArchiveDateTimeColumnName,
					record => false,
					record =>
					{
						if (record is ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime recordManagerRecordWithArchiveDateTime)
						{
							return recordManagerRecordWithArchiveDateTime.ArchiveDateTimeUtc;
						}

						return archiveDateTimeUtc;
					}));

				var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<TRecord>(records, columns, null);

				using (var target = new Microsoft.Data.SqlClient.SqlBulkCopy(connection, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, null))
				{
					target.DestinationTableName = GetArchiveTableName(addAlias: false);
					target.BulkCopyTimeout = bulkCopyTimeoutInSeconds;
					target.BatchSize = batchSize;

					target.NotifyAfter = batchSize;
					if (batchLogger != null)
					{
						target.SqlRowsCopied += (sender, args) => batchLogger(string.Format("{0} records inserted into archive", args.RowsCopied));
					}

					try
					{
						target.WriteToServer(dataReader);
					}
					catch (Microsoft.Data.SqlClient.SqlException sqlException)
					{
						//https://stackoverflow.com/questions/10442686/received-an-invalid-column-length-from-the-bcp-client-for-colid-6
						const string invalidColumnLengthExceptionMessage = "Received an invalid column length from the bcp client for colid";
						if (sqlException.Message.Contains(invalidColumnLengthExceptionMessage))
						{
							var columnIndex = sqlException.Message.Substring(invalidColumnLengthExceptionMessage.Length + 1).Trim().ToInt() - 1;

							var sortedColumnMappings = typeof(Microsoft.Data.SqlClient.SqlBulkCopy).GetField("_sortedColumnMappings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(target);
							var items = (object[])sortedColumnMappings.GetType().GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(sortedColumnMappings);

							var itemData = items[columnIndex].GetType().GetField("_metadata", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
							var metadata = itemData.GetValue(items[columnIndex]);

							var columnName = metadata.GetType().GetField("column", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(metadata);
							var columnLength = metadata.GetType().GetField("length", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(metadata);

							throw new Exception(string.Format("Column: \"{0}\" contains data with a length greater than: {1}", columnName, columnLength), sqlException);
						}
					}

					batchLogger?.Invoke(string.Format("{0} records inserted into archive", target.RowsCopied));
				}
			}
		}
	}
}