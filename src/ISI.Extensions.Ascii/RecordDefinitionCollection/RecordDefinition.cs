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

namespace ISI.Extensions.Ascii
{
	public delegate bool IsRecordTypeDelegate(byte[] buffer);

	public partial class RecordDefinitionCollection<TIRecord>
	{
		public class RecordDefinition<TRecord> : IRecordDefinition
			where TRecord : class, TIRecord, new()
		{
			protected ColumnInfoCollection<TRecord>.IColumnInfo[] Columns { get; }
			public Type RecordType => typeof(TRecord);
			public int RecordSize { get; }
			public IsRecordTypeDelegate IsRecordType { get; }

			public RecordDefinition(ColumnInfoCollection<TRecord> columns, IsRecordTypeDelegate isRecordType)
			{
				Columns = columns.ToArray();
				RecordSize = columns.RecordSize;
				IsRecordType = isRecordType ?? (bytes => true);
			}

			public TIRecord GetRecord(byte[] recordBuffer)
			{
				var record = new TRecord();

				var offset = 0;
				foreach (var column in Columns)
				{
					if (column.ColumnType != null)
					{
						var columnBuffer = new byte[column.ColumnSize];
						for (var columnIndex = 0; columnIndex < column.ColumnSize; columnIndex++)
						{
							columnBuffer[columnIndex] = recordBuffer[offset + columnIndex];
						}

						var value = System.Text.Encoding.Default.GetString(columnBuffer);

						if (column.ColumnType == typeof(string))
						{
							column.SetValue(record, value);
						}
						else if (column.ColumnType == typeof(int?))
						{
							column.SetValue(record, value.ToIntNullable());
						}
						else if (column.ColumnType == typeof(int))
						{
							column.SetValue(record, value.ToInt());
						}
						else if (column.ColumnType == typeof(long?))
						{
							column.SetValue(record, value.ToLongNullable());
						}
						else if (column.ColumnType == typeof(long))
						{
							column.SetValue(record, value.ToLong());
						}
						else if (column.ColumnType == typeof(decimal?))
						{
							column.SetValue(record, value.ToDecimalNullable());
						}
						else if (column.ColumnType == typeof(decimal))
						{
							column.SetValue(record, value.ToDecimal());
						}
						else if (column.ColumnType == typeof(DateTime?))
						{
							column.SetValue(record, value.ToDateTimeNullable());
						}
						else if (column.ColumnType == typeof(DateTime))
						{
							column.SetValue(record, value.ToDateTime());
						}
					}

					offset += column.ColumnSize;
				}

				return record;
			}
		}
	}
}
