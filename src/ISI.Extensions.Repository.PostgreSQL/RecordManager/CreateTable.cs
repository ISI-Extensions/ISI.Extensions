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
		public override void CreateTable(CreateTableMode createTableMode = CreateTableMode.ErrorIfExists)
		{
			using (var connection = GetSqlConnection())
			{
				CreateTable(connection, createTableMode, false);
			}
		}

		public virtual void CreateTable(Npgsql.NpgsqlConnection connection, CreateTableMode createTableMode = CreateTableMode.ErrorIfExists)
		{
			CreateTable(connection, createTableMode, false);
		}

		internal class HasLocalClusteringIndexRecord
		{
			[ISI.Extensions.Repository.PrimaryKey, ISI.Extensions.Repository.Identity]
			[ISI.Extensions.Repository.RecordProperty(ColumnName = "LocalClusteringIndexId")]
			public int LocalClusteringIndexId { get; set; }
		}

		private IRecordDescription<TRecord> GetRecordDescription()
		{
			var recordDescription = RecordDescription.GetRecordDescription<TRecord>();

			if (recordDescription.HasLocalClusteringIndex)
			{
				var recordDescriptionIndexes = recordDescription.Indexes.ToList();
				var recordDescriptionPropertyDescriptions = recordDescription.PropertyDescriptions.ToList();

				System.Attribute[] GetAttributes(IRecordPropertyDescription recordPropertyDescription, bool includePrimaryKeyAttribute = true)
				{
					var attributes = new List<System.Attribute>();
					if (recordPropertyDescription.DataMemberAttribute != null)
					{
						attributes.Add(recordPropertyDescription.DataMemberAttribute);
					}
					if (recordPropertyDescription.RecordPropertyAttribute != null)
					{
						attributes.Add(recordPropertyDescription.RecordPropertyAttribute);
					}
					if (includePrimaryKeyAttribute && (recordPropertyDescription.PrimaryKeyAttribute != null))
					{
						attributes.Add(recordPropertyDescription.PrimaryKeyAttribute);
					}
					if (recordPropertyDescription.RepositoryAssignedValueAttribute != null)
					{
						attributes.Add(recordPropertyDescription.RepositoryAssignedValueAttribute);
					}

					return attributes.ToArray();
				}

				if (recordDescription.PrimaryKeyPropertyDescriptions.NullCheckedAny())
				{
					var recordIndexColumns = new ISI.Extensions.Repository.RecordIndexColumnCollection<TRecord>();

					foreach (var primaryKeyPropertyDescription in recordDescription.PrimaryKeyPropertyDescriptions.OrderBy(propertyDescription => propertyDescription.PrimaryKeyAttribute.Order).Cast<IRecordPropertyDescription>())
					{
						recordIndexColumns.Add(new()
						{
							RecordPropertyDescription = new RecordPropertyDescription<TRecord>(primaryKeyPropertyDescription.PropertyInfo, GetAttributes(primaryKeyPropertyDescription), primaryKeyPropertyDescription.CanBeSerialized),
							AscendingOrder = true,
						});
					}

					recordDescriptionIndexes.Insert(0, new()
					{
						Columns = recordIndexColumns.ToArray(),
						Name = "PrimaryKeyIndex",
						Unique = true,
					});

					for (var propertyDescriptionIndex = 0; propertyDescriptionIndex < recordDescriptionPropertyDescriptions.Count; propertyDescriptionIndex++)
					{
						var recordDescriptionPropertyDescription = recordDescriptionPropertyDescriptions[propertyDescriptionIndex];

						if (recordDescriptionPropertyDescription.PrimaryKeyAttribute != null)
						{
							recordDescriptionPropertyDescriptions[propertyDescriptionIndex] = new RecordPropertyDescription<TRecord>(recordDescriptionPropertyDescription.PropertyInfo, GetAttributes(recordDescriptionPropertyDescription, false), recordDescriptionPropertyDescription.CanBeSerialized);
						}
					}
				}

				var localClusteringIndexIdPropertyDescription = RecordDescription.GetRecordDescription<HasLocalClusteringIndexRecord>().PropertyDescriptions.FirstOrDefault() as IRecordPropertyDescription;
				recordDescriptionPropertyDescriptions.Insert(0, new RecordPropertyDescription<TRecord>(localClusteringIndexIdPropertyDescription.PropertyInfo, GetAttributes(localClusteringIndexIdPropertyDescription), localClusteringIndexIdPropertyDescription.CanBeSerialized));

				recordDescription = new RecordDescription<TRecord>(recordDescription.Schema, recordDescription.TableName, recordDescription.HasLocalClusteringIndex, recordDescriptionPropertyDescriptions.ToArray(), recordDescriptionIndexes.ToArray());
			}

			return recordDescription;
		}

		public virtual void CreateTable(Npgsql.NpgsqlConnection connection, CreateTableMode createTableMode, bool hasArchiveTable)
		{
			var sql = new StringBuilder();

			sql.Clear();
			sql.Append("SELECT COUNT(1) FROM pg_tables WHERE schemaname = @SchemaName AND tablename = @TableName\n");
			var tableExists = connection.ExecuteScalarAsync<int>(sql.ToString(), [
				new KeyValuePair<string, object>("SchemaName", Schema),
				new KeyValuePair<string, object>("TableName", TableName)
			]).GetAwaiter().GetResult() != 0;

			sql.Clear();

			var recordDescription = GetRecordDescription();

			var tableName = FormatTableName(TableName, null, false);

			var primaryKeyName = $"\"pk_{TableName}\"";

			if (!recordDescription.PrimaryKeyPropertyDescriptions.NullCheckedAny())
			{
				primaryKeyName = null;
			}
			else
			{
				var primaryKeyColumnsWithPrimaryKeyAttributes = recordDescription.PrimaryKeyPropertyDescriptions.Where(column => column.PrimaryKeyAttribute != null).ToArray();
				if (primaryKeyColumnsWithPrimaryKeyAttributes.Any())
				{
					var firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName = primaryKeyColumnsWithPrimaryKeyAttributes.FirstOrDefault(column => !string.IsNullOrEmpty(column.PrimaryKeyAttribute.Name));
					if (firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName != null)
					{
						primaryKeyName = FormatColumnName(firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName.PrimaryKeyAttribute.Name);
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(Schema))
			{
				connection.ExecuteNonQueryAsync($"CREATE SCHEMA IF NOT EXISTS {Schema.PostgreSQLFormatName()};").Wait();
			}

			if (tableExists)
			{
				switch (createTableMode)
				{
					case CreateTableMode.DeleteAndCreateIfExists:
						connection.ExecuteNonQueryAsync($"DROP TABLE IF EXISTS {tableName};").Wait();
						tableExists = false;
						break;

					case CreateTableMode.TruncateIfExists:
						connection.ExecuteNonQueryAsync($"TRUNCATE TABLE {tableName};").Wait();
						break;
				}
			}

			if (!tableExists)
			{
				sql.AppendFormat("CREATE TABLE {0}\n", tableName);
				sql.Append("  (\n");
				sql.AppendFormat("{0}{1}\n", string.Join(",\n", recordDescription.PropertyDescriptions.OrderBy(propertyDescription => propertyDescription.Order).Select(propertyDescription => $"  {propertyDescription.GetColumnDefinition(FormatColumnName)}")), (string.IsNullOrWhiteSpace(primaryKeyName) ? string.Empty : ","));

				if (!string.IsNullOrWhiteSpace(primaryKeyName))
				{
					sql.AppendFormat("    CONSTRAINT {0} PRIMARY KEY ({1})\n", primaryKeyName, string.Join(", ", recordDescription.PrimaryKeyPropertyDescriptions.OrderBy(propertyDescription => propertyDescription.PrimaryKeyAttribute.Order).Select(column => FormatColumnName(column.ColumnName))));
				}

				sql.Append("  );\n");

				sql.Append(GetCreateIndexesSql(tableName, recordDescription));

				ExecuteCreateTable(connection, sql.ToString());

				if (hasArchiveTable)
				{
					CreateArchiveTable(connection, createTableMode);
				}
			}
		}




		protected virtual void ExecuteCreateTable(Npgsql.NpgsqlConnection connection, string sql)
		{
			ExecuteNonQueryAsync(connection, sql).Wait();
		}

		public virtual void CreateArchiveTable(Npgsql.NpgsqlConnection connection, CreateTableMode createTableMode = CreateTableMode.ErrorIfExists)
		{
			var sql = new StringBuilder();

			sql.Clear();
			sql.Append("SELECT COUNT(1) FROM pg_tables WHERE schemaname = @SchemaName AND tablename = @TableName\n");
			var tableExists = connection.ExecuteScalarAsync<int>(sql.ToString(), [
				new KeyValuePair<string, object>("SchemaName", Schema),
				new KeyValuePair<string, object>("TableName", $"{TableName}{ArchiveTableSuffix}")
			]).GetAwaiter().GetResult() != 0;

			sql.Clear();

			var recordDescription = RecordDescription.GetRecordDescription<TRecord>();

			var tableName = FormatArchiveTableName($"{TableName}{ArchiveTableSuffix}", null, false);

			var primaryKeyName = $"\"{RecordDescription.GetRecordDescription<TRecord>().TableName}{ArchiveTableSuffix}Index\"";

			if (!recordDescription.PrimaryKeyPropertyDescriptions.NullCheckedAny())
			{
				primaryKeyName = null;
			}
			else
			{
				var primaryKeyColumnsWithPrimaryKeyAttributes = recordDescription.PrimaryKeyPropertyDescriptions.Where(column => column.PrimaryKeyAttribute != null).ToArray();
				if (primaryKeyColumnsWithPrimaryKeyAttributes.Any())
				{
					var firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName = primaryKeyColumnsWithPrimaryKeyAttributes.FirstOrDefault(column => !string.IsNullOrEmpty(column.PrimaryKeyAttribute.Name));
					if (firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName != null)
					{
						primaryKeyName = $"\"{firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName.PrimaryKeyAttribute.Name}{ArchiveTableSuffix}\"";
					}
				}
			}

			if (tableExists)
			{
				switch (createTableMode)
				{
					case CreateTableMode.DeleteAndCreateIfExists:
						connection.ExecuteNonQueryAsync($"DROP TABLE IF EXISTS {tableName};").Wait();
						tableExists = false;
						break;
					case CreateTableMode.TruncateIfExists:
						connection.ExecuteNonQueryAsync($"TRUNCATE TABLE {tableName};").Wait();
						break;
				}
			}

			if (!tableExists)
			{
				sql.AppendFormat("CREATE TABLE {0}\n", tableName);
				sql.Append("  (\n");
				sql.AppendFormat("    {0} timestamp without time zone not null,\n", FormatColumnName(ArchiveTableArchiveDateTimeColumnName));
				sql.AppendFormat("{0}\n", string.Join(",\n", recordDescription.PropertyDescriptions.OrderBy(propertyDescription => propertyDescription.Order).Select(propertyDescription => $"  {propertyDescription.GetColumnDefinition(FormatColumnName)}")));
				sql.Append("  );\n");

				if (!string.IsNullOrWhiteSpace(primaryKeyName))
				{
					sql.AppendFormat("  CREATE INDEX {0} ON {1} ({2})\n", primaryKeyName, tableName, string.Join(", ", recordDescription.PrimaryKeyPropertyDescriptions.OrderBy(propertyDescription => propertyDescription.PrimaryKeyAttribute.Order).Select(column => FormatColumnName(column.ColumnName))));
				}

				ExecuteCreateArchiveTable(connection, sql.ToString());
			}
		}

		protected virtual void ExecuteCreateArchiveTable(Npgsql.NpgsqlConnection connection, string sql)
		{
			ExecuteNonQueryAsync(connection, sql).Wait();
		}
	}
}