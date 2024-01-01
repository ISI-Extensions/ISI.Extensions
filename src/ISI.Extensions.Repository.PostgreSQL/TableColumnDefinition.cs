#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Repository.PostgreSQL.Extensions;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public class TableColumnDefinition
	{
		public string ColumnName { get; set; }
		public string ColumnType { get; set; }
		public bool Nullable { get; set; }
		public string DefaultValue { get; set; }

		public static async Task<TableColumnDefinition[]> GetTableColumnDefinitionsAsync(string connectionString, string schema, string tableName)
		{
			var result = new List<TableColumnDefinition>();

			using (var connection = SqlConnection.GetSqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new Npgsql.NpgsqlCommand($"\\d {schema.PostgreSQLFormatName()}.{tableName.PostgreSQLFormatName()}", connection))
				{
					using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync())
					{
						if (await dataReader.NextResultAsync())
						{
							var columnNameIndex = -1;
							var columnTypeIndex = -1;
							var nullableIndex = -1;
							var defaultValueIndex = -1;

							while (await dataReader.ReadAsync())
							{
								if (columnNameIndex < 0)
								{
									columnNameIndex = dataReader.GetOrdinal("Column");
									columnTypeIndex = dataReader.GetOrdinal("Type");
									nullableIndex = dataReader.GetOrdinal("Nullable");
									defaultValueIndex = dataReader.GetOrdinal("Default");
								}

								var columnDefinition = new TableColumnDefinition()
								{
									ColumnName = dataReader.GetString(columnNameIndex),
									ColumnType = dataReader.GetString(columnTypeIndex),
									Nullable = !(dataReader.GetString(nullableIndex) ?? string.Empty).Equals("not null", StringComparison.InvariantCultureIgnoreCase),
									DefaultValue = dataReader.GetString(defaultValueIndex),
								};

								result.Add(columnDefinition);
							}
						}
					}
				}
			}

			return result.ToArray();
		}
	}
}
