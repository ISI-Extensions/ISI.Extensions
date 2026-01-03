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
		protected class PrimaryKeyWhereClause<TRecordPrimaryKey> : WhereClause, INpgsqlConnectionWhereClause, IWhereClause, IWhereClauseWithSql, IWhereClauseWithGetSql, IWhereClauseWithParameters, IWhereClauseWithGetParameters
		{
			protected IEnumerable<TRecordPrimaryKey> PrimaryKeyValues { get; }
			private readonly Func<string, string> _formatColumnName;

			protected string PrimaryKeyTempTableName { get; private set; }
			protected string JoinClauseFormat { get; private set; }

			protected bool Initialized { get; private set; }

			protected bool HasValue { get; private set; }

			public string GetJoinCause(string tableNameAlias)
			{
				if (string.IsNullOrEmpty(JoinClauseFormat))
				{
					return string.Empty;
				}

				return string.Format(JoinClauseFormat, tableNameAlias);
			}


			public PrimaryKeyWhereClause(IEnumerable<TRecordPrimaryKey> primaryKeyValues, Func<string, string> formatColumnName)
			{
				PrimaryKeyValues = primaryKeyValues;
				JoinClauseFormat = string.Empty;
				_formatColumnName = formatColumnName;
			}

			protected override void Reset()
			{
				Initialized = false;
				HasValue = false;
				Sql = string.Empty;
				Parameters.Clear();

				base.Reset();
			}

			public void Initialize(ISI.Extensions.Repository.PostgreSQL.Configuration postgresqlConfiguration, Npgsql.NpgsqlConnection connection)
			{
				var primaryKeyColumns = RecordDescription.GetRecordDescription<TRecord>().PrimaryKeyPropertyDescriptions;

				var primaryKeyColumnCount = primaryKeyColumns.Length;

				if (primaryKeyColumnCount <= 0)
				{
					throw new("No primary key defined");
				}

				var primaryKeyMaxCount = postgresqlConfiguration.PrimaryKeyMaxCount;

				if (primaryKeyColumns.Length == 1)
				{
					var primaryKeyColumn = primaryKeyColumns.FirstOrDefault();

					var parameters = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

					var usedPrimaryKeyValues = new List<TRecordPrimaryKey>();

					var primaryKeyValuesEnumerator = PrimaryKeyValues.GetEnumerator();

					string GetPrimaryKeyValueName(int primaryKeyValueNameIndex) => $"@primaryKey_{primaryKeyValueNameIndex}";

					var primaryKeyValueCount = 0;

					while ((primaryKeyValueCount < primaryKeyMaxCount) && primaryKeyValuesEnumerator.MoveNext())
					{
						primaryKeyValueCount++;

						parameters.Add(GetPrimaryKeyValueName(primaryKeyValueCount), primaryKeyValuesEnumerator.Current);

						usedPrimaryKeyValues.Add(primaryKeyValuesEnumerator.Current);
					}

					if (primaryKeyValueCount == 1)
					{
						var primaryKeyColumnAlias = "@primaryKey";

						Parameters.Add(primaryKeyColumnAlias, parameters.FirstOrDefault().Value);

						Sql = $"      {_formatColumnName(primaryKeyColumn.ColumnName)} = {primaryKeyColumnAlias}";
					}
					else if (primaryKeyValueCount < primaryKeyMaxCount)
					{
						foreach (var parameter in parameters)
						{
							Parameters.Add(parameter);
						}

						Sql = $"      {_formatColumnName(primaryKeyColumn.ColumnName)} in ({string.Join(", ", parameters.Keys)})";
					}
					else
					{
						PrimaryKeyTempTableName = $"PrimaryKeys_{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}";

						var dataReader = new ISI.Extensions.DataReader.EnumerableDataReader<ISI.Extensions.DataReader.ValueWrapper<TRecordPrimaryKey>>([
							usedPrimaryKeyValues.Select(primaryKey => new ISI.Extensions.DataReader.ValueWrapper<TRecordPrimaryKey>()
							{
								Value = primaryKey
							}),
							new ISI.Extensions.ContinueReadingEnumerable<TRecordPrimaryKey>(PrimaryKeyValues, primaryKeyValuesEnumerator).Select(primaryKey => new ISI.Extensions.DataReader.ValueWrapper<TRecordPrimaryKey>()
							{
								Value = primaryKey
							})
						], null, null);

						connection.EnsureConnectionIsOpenAsync().Wait();

						var sourcePrimaryKeyTempTableName = $"Source{PrimaryKeyTempTableName}";

						using (var command = new Npgsql.NpgsqlCommand(@$"
CREATE TEMP TABLE {sourcePrimaryKeyTempTableName}
(
	{primaryKeyColumn.GetColumnDefinition(_formatColumnName)}
)
", connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						using (var binaryImporter = connection.BeginBinaryImport($"COPY {sourcePrimaryKeyTempTableName} FROM STDIN (FORMAT BINARY)"))
						{
							while (dataReader.Read())
							{
								binaryImporter.StartRow();

								for (var columnIndex = 0; columnIndex < dataReader.FieldCount; columnIndex++)
								{
									binaryImporter.Write(dataReader.GetValue(columnIndex));
								}
							}

							binaryImporter.Complete();
						}

						using (var command = new Npgsql.NpgsqlCommand(@$"
CREATE TEMP TABLE {PrimaryKeyTempTableName}
(
	{primaryKeyColumn.GetColumnDefinition(_formatColumnName)} PRIMARY KEY
)
", connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						using (var command = new Npgsql.NpgsqlCommand(@$"
INSERT INTO {PrimaryKeyTempTableName} ({_formatColumnName(primaryKeyColumn.ColumnName)})
SELECT DISTINCT {_formatColumnName(primaryKeyColumn.ColumnName)}
FROM {sourcePrimaryKeyTempTableName}", connection))
						{
							command.ExecuteNonQueryWithExceptionTracingAsync().Wait();
						}

						JoinClauseFormat = $"    JOIN {PrimaryKeyTempTableName} {PrimaryKeyTempTableName} ON ({PrimaryKeyTempTableName}.{_formatColumnName(primaryKeyColumn.ColumnName)} = {{0}}.{_formatColumnName(primaryKeyColumn.ColumnName)})\n";
					}

					HasValue = (primaryKeyValueCount > 0);
				}
				else
				{
					throw new NotImplementedException("Haven't built multi-column primary key yet");
				}

				Initialized = true;
			}

			public void Finalize(Npgsql.NpgsqlConnection connection)
			{

			}

			bool IWhereClause.IsFilter => false;
			bool IWhereClause.HasFilter
			{
				get
				{
					if (!Initialized)
					{
						throw new($"\"{nameof(Initialize)}\" Method must be called before checking \"HasFilter\"");
					}

					return HasValue;
				}
			}
		}

		protected override IWhereClause GeneratePrimaryKeyWhereClause<TPrimaryKey>(IEnumerable<TPrimaryKey> primaryKeyValues)
		{
			return new PrimaryKeyWhereClause<TPrimaryKey>(primaryKeyValues, FormatColumnName);
		}
	}
}