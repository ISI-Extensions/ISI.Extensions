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
using ISI.Extensions.Repository.SqlServer.Extensions;

namespace ISI.Extensions.Repository.SqlServer
{
	public class TableColumnDefinition
	{
		public string ColumnName { get; set; }
		public string ColumnType { get; set; }
		public int Length { get; set; }
		public bool Nullable { get; set; }
		public bool IsIdentity { get; set; }
		public bool HasDefaultValue { get; set; }

		public static async Task<TableColumnDefinition[]> GetTableColumnDefinitionsAsync(string connectionString, string tableName)
		{
			var result = new List<TableColumnDefinition>();

			using (var connection = SqlConnection.GetSqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new Microsoft.Data.SqlClient.SqlCommand("sp_help", connection))
				{
					command.CommandType = System.Data.CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@objname", tableName);

					using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync())
					{
						if (await dataReader.NextResultAsync())
						{
							var columnNameIndex = -1;
							var columnTypeIndex = -1;
							var nullableIndex = -1;
							var computedIndex = -1;
							var lengthIndex = -1;
							var precisionIndex = -1;
							var scaleIndex = -1;

							while (await dataReader.ReadAsync())
							{
								if (columnNameIndex < 0)
								{
									columnNameIndex = dataReader.GetOrdinal("Column_name");
									columnTypeIndex = dataReader.GetOrdinal("Type");
									nullableIndex = dataReader.GetOrdinal("Nullable");
									computedIndex = dataReader.GetOrdinal("Computed");
									lengthIndex = dataReader.GetOrdinal("Length");
									precisionIndex = dataReader.GetOrdinal("Prec");
									scaleIndex = dataReader.GetOrdinal("Scale");
								}

								var computed = !(dataReader.GetString(computedIndex) ?? string.Empty).Equals("no", StringComparison.InvariantCultureIgnoreCase);
								if (!computed)
								{
									var columnDefinition = new TableColumnDefinition()
									{
										ColumnName = dataReader.GetString(columnNameIndex),
										ColumnType = dataReader.GetString(columnTypeIndex),
										Length = (dataReader.GetFieldType(lengthIndex) == typeof(int) ? dataReader.GetInt32(lengthIndex) : dataReader.GetInt16(lengthIndex)),
										Nullable = !(dataReader.GetString(nullableIndex) ?? string.Empty).Equals("no", StringComparison.InvariantCultureIgnoreCase)
									};

									var precision = (dataReader.GetFieldType(precisionIndex) == typeof(string) ? dataReader.GetString(precisionIndex).ToInt() : dataReader.GetInt32(precisionIndex));
									var scale = (dataReader.GetFieldType(precisionIndex) == typeof(string) ? dataReader.GetString(scaleIndex).ToInt() : dataReader.GetInt32(scaleIndex));

									switch (columnDefinition.ColumnType.ToLower())
									{
										case "decimal":
										case "numeric":
											columnDefinition.ColumnType = string.Format("{0}({1}, {2})", columnDefinition.ColumnType, precision, scale);
											break;

										case "char":
										case "varchar":
										case "varbinary":
											columnDefinition.ColumnType = string.Format("{0}({1})", columnDefinition.ColumnType, (columnDefinition.Length <= 0 ? "max" : string.Format("{0}", columnDefinition.Length)));
											break;

										case "nchar":
										case "nvarchar":
											columnDefinition.Length /= 2;
											columnDefinition.ColumnType = string.Format("{0}({1})", columnDefinition.ColumnType, (columnDefinition.Length <= 0 ? "max" : string.Format("{0}", columnDefinition.Length)));
											break;
									}

									result.Add(columnDefinition);
								}
							}

							if (await dataReader.NextResultAsync())
							{
								if (await dataReader.ReadAsync())
								{
									var columnName = dataReader.GetString(0);
									foreach (var column in result.Where(c => string.Equals(c.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase)))
									{
										column.IsIdentity = true;
									}
								}
							}

							if (await dataReader.NextResultAsync() && await dataReader.NextResultAsync() && await dataReader.NextResultAsync() && await dataReader.NextResultAsync())
							{
								const string defaultPrefix = "DEFAULT on column ";
								while (await dataReader.ReadAsync())
								{
									var constraintType = dataReader.GetString(0);

									if (constraintType.StartsWith(defaultPrefix, StringComparison.InvariantCultureIgnoreCase))
									{
										var columnName = constraintType.Substring(defaultPrefix.Length).Trim();
										foreach (var column in result.Where(c => string.Equals(c.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase)))
										{
											column.HasDefaultValue = true;
										}
									}
								}
							}
						}
					}
				}
			}

			return result.ToArray();
		}
	}
}
