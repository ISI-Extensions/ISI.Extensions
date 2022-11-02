#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Repository.SqlServer
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual async Task<IEnumerable<TRecord>> PersistConvertedRecordsAsync<TConvertedRecord>(IEnumerable<TRecord> records, PersistenceMethod persistenceMethod, bool hasArchiveTable, Action<TRecord> updateRecordProperties = null, UpdateRecordFilterColumnCollection<TRecord> updateRecordColumns = null, Func<TRecord, TConvertedRecord> recordToConvertedRecordConverter = null, Func<TConvertedRecord, TRecord> convertedRecordToRecordConverter = null, Func<TRecord, DateTime> getArchiveDateTime = null)
			where TConvertedRecord : class
		{
			object GetValue(IRecordPropertyDescription<TConvertedRecord> recordPropertyDescription, TConvertedRecord convertedRecord)
			{
				var value = recordPropertyDescription.GetValue(convertedRecord);

				if (recordPropertyDescription.CanBeSerialized && (value != null))
				{
					value = Serializer.Serialize(recordPropertyDescription.ValueType, value, true);
				}

				return value;
			}

			var persistedConvertedRecords = new List<TConvertedRecord>();

			var insertPropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().PropertyDescriptions.ToList();
			var updatePropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().PropertyDescriptions.ToList();
			var primaryKeyPropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().PrimaryKeyPropertyDescriptions;

			var archivePropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().PropertyDescriptions.ToList();

			var repositoryAssignedValuePropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().RepositoryAssignedValuePropertyDescriptions ?? Array.Empty<RecordPropertyDescription<TConvertedRecord>>();
			var insertPropertyIndexes = System.Linq.Enumerable.Range(1, insertPropertyDescriptions.Count - repositoryAssignedValuePropertyDescriptions.Length).ToArray();
			var archivePropertyIndexes = System.Linq.Enumerable.Range(1, archivePropertyDescriptions.Count + 1).ToArray();

			if ((persistenceMethod != PersistenceMethod.Insert) && !primaryKeyPropertyDescriptions.NullCheckedAny())
			{
				throw new("Cannot use PersistConvertedRecords on record with no known primary key columns");
			}

			var maxBatchSize = (int)(1500 / (insertPropertyDescriptions.Count + (hasArchiveTable ? 1 : 0)));
			
			var repositoryAssignedValueColumnDefinitions = new List<TableColumnDefinition>();
			foreach (var repositoryAssignedPropertyDescription in repositoryAssignedValuePropertyDescriptions)
			{
				var columnName = repositoryAssignedPropertyDescription.ColumnName;
				repositoryAssignedValueColumnDefinitions.Add(TableColumnDefinitions.FirstOrDefault(c => string.Equals(c.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase)));
			}

			if (updateRecordColumns.NullCheckedAny())
			{
				updatePropertyDescriptions.RemoveAll(property => !updateRecordColumns.Any(updateRecordColumn => string.Equals(property.ColumnName, updateRecordColumn.RecordPropertyDescription.ColumnName)));
			}

			updatePropertyDescriptions.RemoveAll(property => primaryKeyPropertyDescriptions.Any(primaryKeyPropertyDescription => string.Equals(property.ColumnName, primaryKeyPropertyDescription.ColumnName)));

			recordToConvertedRecordConverter ??= record => record as TConvertedRecord;

			convertedRecordToRecordConverter ??= record => record as TRecord;

			if (getArchiveDateTime == null)
			{
				var archiveDateTimeUtc = DateTimeStamper.CurrentDateTime();

				getArchiveDateTime = record => archiveDateTimeUtc;
			}

			if (repositoryAssignedValueColumnDefinitions.Any())
			{
				insertPropertyDescriptions.RemoveAll(property => repositoryAssignedValuePropertyDescriptions.Any(repositoryAssignedValuePropertyDescription => string.Equals(property.ColumnName, repositoryAssignedValuePropertyDescription.ColumnName)));
			}

			using (var insertConnection = GetSqlConnection())
			using (var updateConnection = GetSqlConnection())
			using (var archiveConnection = GetSqlConnection())
			{
				var persistedConvertedRecordIndex = 0;

				foreach (var recordBath in records.NullCheckedChunk(maxBatchSize))
				{
					var persistedRecordSets = new List<(TRecord Record, TConvertedRecord ConvertedRecord)>();

					var hasInserts = false;
					var insertSql = new StringBuilder();

					insertSql.Append("set nocount on\n");
					insertSql.Append("\n");

					if (repositoryAssignedValueColumnDefinitions.Any())
					{
						insertSql.Append("declare @RepositoryAssignedValues table\n");
						insertSql.Append("(\n");
						insertSql.Append("  InsertedRecordIndex int not null identity(0, 1),\n");
						insertSql.AppendFormat("{0}\n", string.Join(",\n", repositoryAssignedValueColumnDefinitions.Select(repositoryAssignedValueColumnDefinition => string.Format("  {0} {1} not null", FormatColumnName(repositoryAssignedValueColumnDefinition.ColumnName), repositoryAssignedValueColumnDefinition.ColumnType))));
						insertSql.Append(")\n");
						insertSql.Append("\n");
					}

					insertSql.AppendFormat("insert into {0} ({1})\n", GetTableName(addAlias: false), string.Join(", ", insertPropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

					if (repositoryAssignedValueColumnDefinitions.Any())
					{
						insertSql.AppendFormat("output {0}\n", string.Join(", ", repositoryAssignedValuePropertyDescriptions.Select(property => string.Format("INSERTED.{0}", FormatColumnName(property.ColumnName)))));
						insertSql.AppendFormat("into @RepositoryAssignedValues ({0})\n", string.Join(", ", repositoryAssignedValuePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));
					}

					var sqlSelects = new List<string>();
					var sqlValues = new Dictionary<string, object>();

					foreach (var record in recordBath)
					{
						updateRecordProperties?.Invoke(record);

						var convertedRecord = recordToConvertedRecordConverter(record);

						persistedRecordSets.Add((Record: record, ConvertedRecord: convertedRecord));
						persistedConvertedRecords.Add(convertedRecord);
						persistedConvertedRecordIndex++;

						var doInsert = (persistenceMethod == PersistenceMethod.Insert);

						var isConvertedRecordValidated = false;

						if (!doInsert)
						{
							isConvertedRecordValidated |= updatePropertyDescriptions.ValidateColumnData(convertedRecord, true);

							var updateSql = new StringBuilder();

							updateSql.Append("set nocount on\n");
							updateSql.Append("update updateTable\n");
							updateSql.Append("set\n");
							var columnIndex = 1;
							updateSql.AppendFormat("{0}\n", string.Join(",\n", updatePropertyDescriptions.Select(property => string.Format("    {0} = @value{1}", FormatColumnName(property.ColumnName), columnIndex++))));
							updateSql.AppendFormat("from {0}\n", GetTableName("updateTable"));
							updateSql.Append("where\n");
							var primaryKeyIndex = 1;
							updateSql.AppendFormat("      {0}\n", string.Join(" and\n", primaryKeyPropertyDescriptions.Select(property => string.Format("    {0} = @primaryKey{1}", FormatColumnName(property.ColumnName), primaryKeyIndex++))));
							updateSql.Append("select @@RowCount");

							using (var command = new Microsoft.Data.SqlClient.SqlCommand(updateSql.ToString(), updateConnection))
							{
								command.CommandType = System.Data.CommandType.Text;

								columnIndex = 1;
								foreach (var property in updatePropertyDescriptions)
								{
									command.AddParameter(string.Format("@value{0}", columnIndex++), (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
								}

								primaryKeyIndex = 1;
								foreach (var property in primaryKeyPropertyDescriptions)
								{
									command.AddParameter(string.Format("@primaryKey{0}", primaryKeyIndex++), (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
								}

								updateConnection.EnsureConnectionIsOpenAsync().Wait();

								doInsert = !(string.Format("{0}", await command.ExecuteScalarWithExceptionTracingAsync())).ToBoolean();
							}
						}

						if (doInsert)
						{
							isConvertedRecordValidated |= insertPropertyDescriptions.ValidateColumnData(convertedRecord, true);

							hasInserts = true;

							sqlSelects.Add(string.Format("select {0}", string.Join(", ", insertPropertyIndexes.Select(propertyIndex => string.Format("@value_{0}_{1}", persistedConvertedRecordIndex, propertyIndex)))));

							var valueIndex = 1;
							foreach (var property in insertPropertyDescriptions)
							{
								sqlValues.Add(string.Format("@value_{0}_{1}", persistedConvertedRecordIndex, valueIndex++), (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
							}
						}
					}

					if (hasInserts)
					{
						insertSql.AppendFormat("{0}\n", string.Join(" union all\n", sqlSelects));
						insertSql.Append("\n");

						if (repositoryAssignedValueColumnDefinitions.Any())
						{
							insertSql.AppendFormat("select InsertedRecordIndex, {0}\n", string.Join(", ", repositoryAssignedValuePropertyDescriptions.Select(property => string.Format("[{0}]", property.ColumnName))));
							insertSql.Append("from @RepositoryAssignedValues\n");
						}

						await insertConnection.EnsureConnectionIsOpenAsync();

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(insertSql.ToString(), insertConnection))
						{
							command.CommandType = System.Data.CommandType.Text;

							command.AddParameters(sqlValues);

							if (repositoryAssignedValueColumnDefinitions.Any())
							{
								using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync())
								{
									if (await dataReader.ReadAsync())
									{
										persistedConvertedRecordIndex = (int)System.Convert.ChangeType(dataReader.GetValue(0), TypeCode.Int32);
										var columnIndex = 1;
										foreach (var repositoryAssignedValuePropertyDescription in repositoryAssignedValuePropertyDescriptions)
										{
											repositoryAssignedValuePropertyDescription.SetValue(persistedConvertedRecords[persistedConvertedRecordIndex], System.Convert.ChangeType(dataReader.GetValue(columnIndex++), repositoryAssignedValuePropertyDescription.ValueType));
										}
									}
								}
							}
							else
							{
								await command.ExecuteNonQueryWithExceptionTracingAsync();
							}
						}
					}

					if (hasArchiveTable && persistedRecordSets.Any())
					{
						insertSql.Clear();

						insertSql.AppendFormat("insert into {0} ({1}, {2})\n", GetArchiveTableName(addAlias: false), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), string.Join(", ", archivePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

						sqlSelects.Clear();
						sqlValues.Clear();

						var selectIndex = 1;
						foreach (var persistedRecordSet in persistedRecordSets)
						{
							sqlSelects.Add(string.Format("select {0}", string.Join(", ", archivePropertyIndexes.Select(propertyIndex => string.Format("@value_{0}_{1}", selectIndex, propertyIndex)))));

							var valueIndex = 1;
							sqlValues.Add(string.Format("@value_{0}_{1}", selectIndex, valueIndex++), getArchiveDateTime(persistedRecordSet.Record));
							foreach (var property in archivePropertyDescriptions)
							{
								sqlValues.Add(string.Format("@value_{0}_{1}", selectIndex, valueIndex++), (property.IsNull(persistedRecordSet.ConvertedRecord) ? DBNull.Value : GetValue(property, persistedRecordSet.ConvertedRecord)));
							}

							selectIndex++;
						}

						insertSql.AppendFormat("{0}\n", string.Join(" union all\n", sqlSelects));
						insertSql.Append("\n");

						await archiveConnection.EnsureConnectionIsOpenAsync();

						using (var command = new Microsoft.Data.SqlClient.SqlCommand(insertSql.ToString(), archiveConnection))
						{
							command.CommandTimeout = SqlServerConfiguration.ArchiveTableCommandTimeout;
							
							command.CommandType = System.Data.CommandType.Text;

							command.AddParameters(sqlValues);

							await command.ExecuteNonQueryWithExceptionTracingAsync();
						}
					}
				}
			}

			return persistedConvertedRecords.Select(convertedRecordToRecordConverter);
		}
	}
}
