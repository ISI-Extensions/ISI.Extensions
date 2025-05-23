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
using System.Linq.Expressions;
using ISI.Extensions.Oracle.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.Oracle.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.Oracle
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual string GetCreateIndexesSql(string tableName, ISI.Extensions.Repository.IRecordDescription<TRecord> recordDescription)
		{
			var sql = new StringBuilder();

			foreach (var recordIndex in recordDescription.Indexes)
			{
				var columnIssues = new List<string>();
				columnIssues.AddRange(recordIndex.Columns.Where(column => (column.RecordPropertyDescription.ValueType.GetDbType() == System.Data.DbType.String) && (column.RecordPropertyDescription.PropertySize <= 0)).Select(column => string.Format("Column \"{0}\" is of type varchar(max) which cannot be used in an index", column.RecordPropertyDescription.ColumnName)));
				if (columnIssues.Any())
				{
					throw new(string.Format("Cannot create index: \"{0}\"\n  {1}\n", recordIndex.CalculatedName, string.Join("\n  ", columnIssues)));
				}

				sql.Append("\n");

				var recordIndexName = recordIndex.CalculatedName;
				var uniqueIndexNameSuffix = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36);
				if (recordIndexName.Length > (128 - uniqueIndexNameSuffix.Length))
				{
					recordIndexName = recordIndexName.Substring(0, (128 - uniqueIndexNameSuffix.Length));
				}
				recordIndexName = $"{recordIndexName}{uniqueIndexNameSuffix}";

				sql.AppendFormat("  CREATE{3} INDEX \"{0}\" ON {1} ({2}){4};\n", recordIndexName, tableName, string.Join(", ", recordIndex.Columns.Select(column => string.Format("{0}{1}", FormatColumnName(column.RecordPropertyDescription.ColumnName), column.AscendingOrder ? string.Empty : " DESC"))), (recordIndex.Unique ? " UNIQUE" : string.Empty), (recordIndex.Clustered ? " CLUSTER" : string.Empty));
			}

			return sql.ToString();
		}
	}
}