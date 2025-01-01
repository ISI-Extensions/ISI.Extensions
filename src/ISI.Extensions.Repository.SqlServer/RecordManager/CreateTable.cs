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

namespace ISI.Extensions.Repository.SqlServer
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

		public virtual void CreateTable(Microsoft.Data.SqlClient.SqlConnection connection, CreateTableMode createTableMode = CreateTableMode.ErrorIfExists)
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

		public virtual void CreateTable(Microsoft.Data.SqlClient.SqlConnection connection, CreateTableMode createTableMode, bool hasArchiveTable)
		{
			var sql = new StringBuilder();

			var recordDescription = GetRecordDescription();

			var tableName = FormatTableName(TableName, null, false);

			var primaryKeyName = string.Format("[pk_{0}]", TableName);

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
						primaryKeyName = string.Format("{0}", FormatColumnName(firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName.PrimaryKeyAttribute.Name));
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(Schema))
			{
				if (ExecuteScalarAsync<int>(connection, "select count(*) from sys.schemas with (NoLock) where name = @SchemaName", new Dictionary<string, object>()
				{
					{"@SchemaName", Schema}
				}).GetAwaiter().GetResult() < 1)
				{
					ExecuteNonQueryAsync(connection, string.Format("CREATE SCHEMA [{0}]", Schema)).Wait();
				}
			}

			switch (createTableMode)
			{
				case CreateTableMode.DeleteAndCreateIfExists:
					sql.AppendFormat("if OBJECT_ID('{0}') is not null\n", tableName);
					sql.Append("begin\n");
					sql.AppendFormat("  drop table {0}\n", tableName);
					sql.Append("end\n");
					break;
				case CreateTableMode.TruncateIfExists:
					sql.AppendFormat("if OBJECT_ID('{0}') is not null\n", tableName);
					sql.Append("begin\n");
					sql.AppendFormat("  truncate table {0}\n", tableName);
					sql.Append("end\n");
					sql.Append("else\n");
					break;
			}

			sql.Append("begin\n");
			sql.AppendFormat("  create table {0}\n", tableName);
			sql.Append("  (\n");
			sql.AppendFormat("{0}{1}\n", string.Join(",\n", recordDescription.PropertyDescriptions.OrderBy(propertyDescription => propertyDescription.Order).Select(propertyDescription => string.Format("  {0}", propertyDescription.GetColumnDefinition(FormatColumnName)))), (string.IsNullOrWhiteSpace(primaryKeyName) ? string.Empty : ","));

			if (!string.IsNullOrWhiteSpace(primaryKeyName))
			{
				sql.AppendFormat("    constraint {0} primary key ({1})\n", primaryKeyName, string.Join(", ", recordDescription.PrimaryKeyPropertyDescriptions.OrderBy(propertyDescription => propertyDescription.PrimaryKeyAttribute.Order).Select(column => string.Format("{0}{1}", FormatColumnName(column.ColumnName), column.PrimaryKeyAttribute.AscendingOrder ? string.Empty : " desc"))));
			}

			sql.Append("  )\n");

			sql.Append(GetCreateIndexesSql(tableName, recordDescription));

			sql.Append("end\n");

			ExecuteCreateTable(connection, sql.ToString());

			if (hasArchiveTable)
			{
				CreateArchiveTable(connection, createTableMode);
			}
		}




		protected virtual void ExecuteCreateTable(Microsoft.Data.SqlClient.SqlConnection connection, string sql)
		{
			ExecuteNonQueryAsync(connection, sql).Wait();
		}

		public virtual void CreateArchiveTable(Microsoft.Data.SqlClient.SqlConnection connection, CreateTableMode createTableMode = CreateTableMode.ErrorIfExists)
		{
			var sql = new StringBuilder();

			var recordDescription = RecordDescription.GetRecordDescription<TRecord>();

			var tableName = FormatArchiveTableName(string.Format("{0}{1}", TableName, ArchiveTableSuffix), null, false);

			var primaryKeyName = string.Format("[{0}]", string.Format("{0}{1}", RecordDescription.GetRecordDescription<TRecord>().TableName, ArchiveTableSuffix));

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
						primaryKeyName = string.Format("[{0}]", string.Format("{0}{1}", firstPrimaryKeyColumnWithPrimaryKeyAttributeAndPrimaryKeyName.PrimaryKeyAttribute.Name, ArchiveTableSuffix));
					}
				}
			}

			switch (createTableMode)
			{
				case CreateTableMode.DeleteAndCreateIfExists:
					sql.AppendFormat("if OBJECT_ID('{0}') is not null\n", tableName);
					sql.Append("begin\n");
					sql.AppendFormat("  drop table {0}\n", tableName);
					sql.Append("end\n");
					break;
				case CreateTableMode.TruncateIfExists:
					sql.AppendFormat("if OBJECT_ID('{0}') is not null\n", tableName);
					sql.Append("begin\n");
					sql.AppendFormat("  truncate table {0}\n", tableName);
					sql.Append("end\n");
					sql.Append("else\n");
					break;
			}

			sql.Append("begin\n");
			sql.AppendFormat("  create table {0}\n", tableName);
			sql.Append("  (\n");
			sql.AppendFormat("    {0} datetime2 not null,\n", FormatColumnName(ArchiveTableArchiveDateTimeColumnName));
			sql.AppendFormat("{0}\n", string.Join(",\n", recordDescription.PropertyDescriptions.OrderBy(propertyDescription => propertyDescription.Order).Select(propertyDescription => string.Format("  {0}", propertyDescription.GetColumnDefinition(FormatColumnName)))));
			sql.Append("  )\n");

			if (!string.IsNullOrWhiteSpace(primaryKeyName))
			{
				sql.AppendFormat("  create index {0} on {1} ({2})\n", primaryKeyName, tableName, string.Join(", ", recordDescription.PrimaryKeyPropertyDescriptions.OrderBy(propertyDescription => propertyDescription.PrimaryKeyAttribute.Order).Select(column => FormatColumnName(column.ColumnName))));
			}

			sql.Append("end\n");

			ExecuteCreateArchiveTable(connection, sql.ToString());
		}

		protected virtual void ExecuteCreateArchiveTable(Microsoft.Data.SqlClient.SqlConnection connection, string sql)
		{
			ExecuteNonQueryAsync(connection, sql).Wait();
		}
	}
}
