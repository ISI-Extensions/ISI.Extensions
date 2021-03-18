#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Repository.Extensions;

namespace ISI.Extensions.Repository.SqlServer.Extensions
{
	public static partial class RecordColumnDescriptionExtensions
	{
		public static string GetColumnDefinition(this ISI.Extensions.Repository.IColumnDescription columnDescription, Func<string, string> formatColumnName)
		{
			var dbType = (columnDescription is IRecordPropertyDescription recordColumnDescription && recordColumnDescription.CanBeSerialized) ? System.Data.DbType.String : columnDescription.ValueType.GetDbType();

			string NullClause(bool nullable) => (nullable ? "null" : "not null");

			switch (dbType)
			{
				case System.Data.DbType.Boolean:
					{
						return string.Format("  {0} int {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));
					}

				case System.Data.DbType.DateTime2:
					{
						return string.Format("  {0} datetime2 {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));
					}

				case System.Data.DbType.Decimal:
				case System.Data.DbType.Double:
					{
						if (columnDescription.Precision.HasValue)
						{
							if (columnDescription.Scale.HasValue)
							{
								return string.Format("  {0} numeric({1}, {2}) {3}", formatColumnName(columnDescription.ColumnName), columnDescription.Precision, columnDescription.Scale, NullClause(columnDescription.Nullable));
							}

							return string.Format("  {0} numeric({1}) {2}", formatColumnName(columnDescription.ColumnName), columnDescription.Precision, NullClause(columnDescription.Nullable));
						}

						return string.Format("  {0} numeric {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));
					}

				case System.Data.DbType.Guid:
					{
						var columnDefinition = string.Format("  {0} uniqueidentifier {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));

						if ((columnDescription.Default != null) && !columnDescription.Nullable)
						{
							if (columnDescription.Default is IdentityAttribute identityAttribute)
							{
								columnDefinition = string.Format("{0} default newid()", columnDefinition);
							}
						}

						return columnDefinition;
					}

				case System.Data.DbType.Int32:
					{
						var columnDefinition = string.Format("  {0} int {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));

						if ((columnDescription.Default != null) && !columnDescription.Nullable)
						{
							if (columnDescription.Default is IdentityAttribute identityAttribute)
							{
								columnDefinition = string.Format("{0} identity({1}, {2})", columnDefinition, identityAttribute.Seed, identityAttribute.Increment);
							}
						}

						return columnDefinition;
					}

				case System.Data.DbType.Int64:
					{
						var columnDefinition = string.Format("  {0} bigint {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));

						if ((columnDescription.Default != null) && !columnDescription.Nullable)
						{
							if (columnDescription.Default is IdentityAttribute identityAttribute)
							{
								columnDefinition = string.Format("{0} identity({1}, {2})", columnDefinition, identityAttribute.Seed, identityAttribute.Increment);
							}
						}

						return columnDefinition;
					}

				case System.Data.DbType.String:
				case System.Data.DbType.StringFixedLength:
					{
						if (columnDescription.PropertySize > 0)
						{
							return string.Format("  {0} varchar({1}) {2}", formatColumnName(columnDescription.ColumnName), columnDescription.PropertySize, NullClause(columnDescription.Nullable));
						}

						return string.Format("  {0} varchar(max) {1}", formatColumnName(columnDescription.ColumnName), NullClause(columnDescription.Nullable));
					}

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
