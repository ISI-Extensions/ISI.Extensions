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
using System.Linq.Expressions;
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual async IAsyncEnumerable<TRecord> PersistConvertedRecordsAsync<TConvertedRecord>(IEnumerable<TRecord> records, PersistenceMethod persistenceMethod, bool hasArchiveTable, Action<TRecord> updateRecordProperties = null, UpdateRecordFilterColumnCollection<TRecord> updateRecordColumns = null, Func<TRecord, TConvertedRecord> recordToConvertedRecordConverter = null, Func<TConvertedRecord, TRecord> convertedRecordToRecordConverter = null, GetRecordArchiveDateTimeUtcDelegate<TRecord> getArchiveDateTimeUtc = null, System.Threading.CancellationToken cancellationToken = default)
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

			var repositoryAssignedValuePropertyDescriptions = RecordDescription.GetRecordDescription<TConvertedRecord>().RepositoryAssignedValuePropertyDescriptions ?? [];
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

			if (getArchiveDateTimeUtc == null)
			{
				var archiveDateTimeUtc = DateTimeStamper.CurrentDateTime();

				getArchiveDateTimeUtc = record => archiveDateTimeUtc;
			}

			if (repositoryAssignedValueColumnDefinitions.Any())
			{
				insertPropertyDescriptions.RemoveAll(property => repositoryAssignedValuePropertyDescriptions.Any(repositoryAssignedValuePropertyDescription => string.Equals(property.ColumnName, repositoryAssignedValuePropertyDescription.ColumnName)));
			}

			using (var connection = GetSqlConnection())
			{
				var persistedConvertedRecordIndex = 0;

				await connection.EnsureConnectionIsOpenAsync(cancellationToken: cancellationToken);

				foreach (var recordBatch in records.NullCheckedChunk(maxBatchSize))
				{
					var persistedRecordSets = new List<(TRecord Record, TConvertedRecord ConvertedRecord)>();

					foreach (var record in recordBatch)
					{
						updateRecordProperties?.Invoke(record);

						var convertedRecord = recordToConvertedRecordConverter(record);

						persistedRecordSets.Add((Record: record, ConvertedRecord: convertedRecord));
						persistedConvertedRecords.Add(convertedRecord);
						persistedConvertedRecordIndex++;

						switch (persistenceMethod)
						{
							case PersistenceMethod.Insert:
								{
									var sql = new StringBuilder();
									var sqlValues = new Dictionary<string, object>();

									sql.AppendFormat("INSERT INTO {0} ({1})\n", GetTableName(addAlias: false), string.Join(", ", insertPropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

									sql.Append($"VALUES({string.Join(", ", insertPropertyIndexes.Select(propertyIndex => $"@value_{propertyIndex}"))})");

									var valueIndex = 1;
									foreach (var property in insertPropertyDescriptions)
									{
										sqlValues.Add($"@value_{valueIndex++}", (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
									}

									if (repositoryAssignedValueColumnDefinitions.Any())
									{
										sql.AppendFormat("RETURNING {0}\n", string.Join(", ", repositoryAssignedValuePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));
									}

									using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
									{
										command.CommandType = System.Data.CommandType.Text;

										command.AddParameters(sqlValues);

										if (repositoryAssignedValueColumnDefinitions.Any())
										{
											using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync(cancellationToken: cancellationToken))
											{
												if (await dataReader.ReadAsync(cancellationToken))
												{
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
											await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
										}
									}
								}
								break;

							case PersistenceMethod.Update:
								{
									var sql = new StringBuilder();

									sql.AppendFormat("UPDATE {0}\n", GetTableName(addAlias: false));
									sql.Append("SET\n");
									var columnIndex = 1;
									sql.AppendFormat("{0}\n", string.Join(",\n", updatePropertyDescriptions.Select(property => $"    {FormatColumnName(property.ColumnName)} = @value_{columnIndex++}")));
									sql.Append("WHERE\n");
									var primaryKeyIndex = 1;
									sql.AppendFormat("      {0};\n", string.Join(" AND\n", primaryKeyPropertyDescriptions.Select(property => $"    {FormatColumnName(property.ColumnName)} = @primaryKey_{primaryKeyIndex++}")));

									using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
									{
										command.CommandType = System.Data.CommandType.Text;

										columnIndex = 1;
										foreach (var property in updatePropertyDescriptions)
										{
											command.AddParameter($"@value_{columnIndex++}", (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
										}

										primaryKeyIndex = 1;
										foreach (var property in primaryKeyPropertyDescriptions)
										{
											command.AddParameter($"@primaryKey_{primaryKeyIndex++}", (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
										}

										await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
									}
								}
								break;

							case PersistenceMethod.Upsert:
								{
									var sql = new StringBuilder();
									var sqlValues = new Dictionary<string, object>();

									sql.AppendFormat("INSERT INTO {0} ({1})\n", GetTableName(addAlias: false), string.Join(", ", insertPropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

									sql.Append($"VALUES({string.Join(", ", insertPropertyIndexes.Select(propertyIndex => $"@value_{propertyIndex}"))})");

									var valueIndexByColumnName = new Dictionary<string, int>();
									var valueIndex = 1;
									foreach (var property in insertPropertyDescriptions)
									{
										valueIndexByColumnName.Add(property.ColumnName, valueIndex);
										sqlValues.Add($"@value_{valueIndex++}", (property.IsNull(convertedRecord) ? DBNull.Value : GetValue(property, convertedRecord)));
									}

									sql.AppendFormat("ON CONFLICT ({0})\n", string.Join(", ", primaryKeyPropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

									sql.Append("DO UPDATE\n");
									sql.Append("SET\n");
									sql.AppendFormat("{0}", string.Join(",\n", updatePropertyDescriptions.Select(property => $"    {FormatColumnName(property.ColumnName)} = @value_{valueIndexByColumnName[property.ColumnName]}")));
									
									if (repositoryAssignedValueColumnDefinitions.Any())
									{
										sql.AppendFormat("\nRETURNING {0}", string.Join(", ", repositoryAssignedValuePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));
									}
									sql.Append(";\n");

									using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
									{
										command.CommandType = System.Data.CommandType.Text;

										command.AddParameters(sqlValues);

										if (repositoryAssignedValueColumnDefinitions.Any())
										{
											using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync(cancellationToken: cancellationToken))
											{
												if (await dataReader.ReadAsync(cancellationToken))
												{
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
											await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
										}
									}
								}
								break;

							default:
								throw new ArgumentOutOfRangeException(nameof(persistenceMethod), persistenceMethod, null);
						}
					}

					if (hasArchiveTable && persistedRecordSets.Any())
					{
						var sql = new StringBuilder();
						var sqlValues = new Dictionary<string, object>();

						sql.AppendFormat("INSERT INTO {0} ({1}, {2})\n", GetArchiveTableName(addAlias: false), FormatColumnName(ArchiveTableArchiveDateTimeColumnName), string.Join(", ", archivePropertyDescriptions.Select(property => FormatColumnName(property.ColumnName))));

						var sqlSelects = new List<string>();
						sqlValues.Clear();

						var selectIndex = 1;
						foreach (var persistedRecordSet in persistedRecordSets)
						{
							sqlSelects.Add($"SELECT {string.Join(", ", archivePropertyIndexes.Select(propertyIndex => $"@value_{selectIndex}_{propertyIndex}"))}");

							var valueIndex = 1;
							sqlValues.Add($"@value_{selectIndex}_{valueIndex++}", getArchiveDateTimeUtc(persistedRecordSet.Record));
							foreach (var property in archivePropertyDescriptions)
							{
								sqlValues.Add($"@value_{selectIndex}_{valueIndex++}", (property.IsNull(persistedRecordSet.ConvertedRecord) ? DBNull.Value : GetValue(property, persistedRecordSet.ConvertedRecord)));
							}

							selectIndex++;
						}

						sql.AppendFormat("{0}\n", string.Join(" UNION ALL\n", sqlSelects));
						sql.Append(";\n");

						using (var command = new Npgsql.NpgsqlCommand(sql.ToString(), connection))
						{
							command.CommandTimeout = PostgreSQLConfiguration.ArchiveTableCommandTimeout;

							command.CommandType = System.Data.CommandType.Text;

							command.AddParameters(sqlValues);

							await command.ExecuteNonQueryWithExceptionTracingAsync(cancellationToken: cancellationToken);
						}
					}
				}
			}

			foreach (var record in persistedConvertedRecords.Select(convertedRecordToRecordConverter))
			{
				yield return record;
			}
		}
	}
}