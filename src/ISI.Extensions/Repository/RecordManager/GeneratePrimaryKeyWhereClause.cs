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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository
{
	public abstract partial class RecordManager<TRecord>
	{
		protected class PrimaryKeyWhereClause<TPrimaryKey> : WhereClause, IWhereClause, IWhereClauseWithSql, IWhereClauseWithGetSql, IWhereClauseWithParameters, IWhereClauseWithGetParameters
		{
			public PrimaryKeyWhereClause(IEnumerable<TPrimaryKey> primaryKeyValues, Func<string, string> formatColumnName)
			{
				primaryKeyValues = primaryKeyValues.ToNullCheckedArray(NullCheckCollectionResult.Empty);

				if (primaryKeyValues.Any())
				{
					var primaryKeyColumns = RecordDescription.GetRecordDescription<TRecord>().PrimaryKeyPropertyDescriptions;

					if (!primaryKeyColumns.Any())
					{
						throw new("missing primary key column");
					}
					else if (primaryKeyColumns.Length == 1)
					{
						var primaryKeyColumnName = string.Format("{0}", formatColumnName(primaryKeyColumns.First().ColumnName));

						if (!primaryKeyValues.Any())
						{
							throw new("missing primary key");
						}
						else if (primaryKeyValues.Count() == 1)
						{
							var primaryKeyValue = primaryKeyValues.FirstOrDefault();

							var primaryKeyColumnAlias = "@primaryKey";

							Parameters.Add(primaryKeyColumnAlias, primaryKeyValue);

							Sql = string.Format("      {0} = {1}", formatColumnName(primaryKeyColumnName), primaryKeyColumnAlias);
						}
						else
						{
							var primaryKeyValueIndex = 1;
							foreach (var primaryKeyValue in primaryKeyValues)
							{
								var primaryKeyColumnAlias = string.Format("@primaryKey_{0}", primaryKeyValueIndex++);

								Parameters.Add(primaryKeyColumnAlias, primaryKeyValue);
							}

							Sql = string.Format("      {0} in ({1})", formatColumnName(primaryKeyColumnName), string.Join(", ", Parameters.Keys));
						}
					}
					else
					{
						throw new NotImplementedException("Haven't built multi-column primary key yet");

						//multi-column primary key
						if (!primaryKeyValues.Any())
						{
							throw new("missing primary key");
						}
						else if (primaryKeyValues.Count() == 1)
						{
							var primaryKeyValue = primaryKeyValues.FirstOrDefault();

							foreach (var primaryKeyColumn in primaryKeyColumns)
							{
								Parameters.Add(string.Format("@primaryKey_{0}", formatColumnName(primaryKeyColumn.ColumnName)), primaryKeyValue);
							}

							Sql = string.Format("      ({0})", string.Join(" and ", primaryKeyColumns.Select(primaryKeyColumn => string.Format("{0} = {1}", primaryKeyColumn.ColumnName, string.Format("@primaryKey_{0}", primaryKeyColumn.ColumnName)))));
						}
						else
						{
							var sql = new List<string>();
							var primaryKeyValueIndex = 1;
							foreach (var primaryKeyValue in primaryKeyValues)
							{
								foreach (var primaryKeyColumn in primaryKeyColumns)
								{
									Parameters.Add(string.Format("@primaryKey_{0}_{1}", primaryKeyValueIndex, primaryKeyColumn.ColumnName), primaryKeyValue);
								}

								sql.Add(string.Format("      ({0})", string.Join(" and ", primaryKeyColumns.Select(primaryKeyColumn => string.Format("{0} = {1}", primaryKeyColumn.ColumnName, string.Format("@primaryKey_{0}_{1}", primaryKeyValueIndex, primaryKeyColumn.ColumnName))))));
								primaryKeyValueIndex++;
							}

							Sql = string.Format("      ({0})", string.Join("\n or", sql));
						}
					}
				}
			}

			bool IWhereClause.IsFilter => false;
		}

		protected virtual IWhereClause GeneratePrimaryKeyWhereClause<TRecordPrimaryKey>(IEnumerable<TRecordPrimaryKey> primaryKeyValues)
		{
			return new PrimaryKeyWhereClause<TRecordPrimaryKey>(primaryKeyValues, FormatColumnName);
		}
	}
}