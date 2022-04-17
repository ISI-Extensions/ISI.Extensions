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

namespace ISI.Extensions.Ebcdic
{
	public partial class RecordDefinitionCollection<TIRecord>
	{
		public class RecordDefinition<TRecord> : IRecordDefinition
			where TRecord : class, TIRecord, new()
		{
			protected ColumnInfoCollection<TRecord>.IColumnInfo[] Columns { get; }
			public Type RecordType => typeof(TRecord);
			public int RecordSize { get; }
			public Func<byte[], bool> IsRecordType { get; }

			public RecordDefinition(ColumnInfoCollection<TRecord> columns, Func<byte[], bool> isRecordType)
			{
				Columns = columns.ToArray();
				RecordSize = columns.RecordSize;
				IsRecordType = isRecordType ?? (bytes => true) ;
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

						if (column.ColumnType == typeof(string))
						{
							column.SetValue(record, TranslateEbcdic.ToString(columnBuffer));
						}
						else if (column.ColumnType == typeof(int?))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.SignedIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromSigned(columnBuffer));
									break;
								case ColumnFormat.PackedIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromPacked(columnBuffer));
									break;
								case ColumnFormat.BinaryIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromBinary(columnBuffer));
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToIntNullable());
									break;
							}
						}
						else if (column.ColumnType == typeof(int))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.SignedIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromSigned(columnBuffer).GetValueOrDefault());
									break;
								case ColumnFormat.PackedIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromPacked(columnBuffer).GetValueOrDefault());
									break;
								case ColumnFormat.BinaryIntNullable:
									column.SetValue(record, TranslateEbcdic.ToIntNullableFromBinary(columnBuffer).GetValueOrDefault());
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToIntNullable().GetValueOrDefault());
									break;
							}
						}
						else if (column.ColumnType == typeof(long?))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.SignedLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromSigned(columnBuffer));
									break;
								case ColumnFormat.PackedLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromPacked(columnBuffer));
									break;
								case ColumnFormat.BinaryLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromBinary(columnBuffer));
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToLongNullable());
									break;
							}
						}
						else if (column.ColumnType == typeof(long))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.SignedLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromSigned(columnBuffer).GetValueOrDefault());
									break;
								case ColumnFormat.PackedLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromPacked(columnBuffer).GetValueOrDefault());
									break;
								case ColumnFormat.BinaryLongNullable:
									column.SetValue(record, TranslateEbcdic.ToLongNullableFromBinary(columnBuffer).GetValueOrDefault());
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToLongNullable().GetValueOrDefault());
									break;
							}
						}
						else if (column.ColumnType == typeof(decimal?))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.PackedDecimalNullable:
									column.SetValue(record, TranslateEbcdic.ToDecimalNullableFromPacked(columnBuffer, column.Scale.GetValueOrDefault()));
									break;
								case ColumnFormat.BinaryDecimalNullable:
									column.SetValue(record, TranslateEbcdic.ToDecimalNullableFromBinary(columnBuffer, column.Scale.GetValueOrDefault()));
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToDecimalNullable());
									break;
							}
						}
						else if (column.ColumnType == typeof(decimal))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.PackedDecimalNullable:
									column.SetValue(record, TranslateEbcdic.ToDecimalNullableFromPacked(columnBuffer, column.Scale.GetValueOrDefault()).GetValueOrDefault());
									break;
								case ColumnFormat.BinaryDecimalNullable:
									column.SetValue(record, TranslateEbcdic.ToDecimalNullableFromBinary(columnBuffer, column.Scale.GetValueOrDefault()).GetValueOrDefault());
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToString(columnBuffer).ToDecimalNullable().GetValueOrDefault());
									break;
							}
						}
						else if (column.ColumnType == typeof(DateTime?))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.PackedDateDateTimeNullable:
									column.SetValue(record, TranslateEbcdic.ToDateTimeNullableFromPacked(columnBuffer));
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToDateTimeNullable(columnBuffer));
									break;
							}
						}
						else if (column.ColumnType == typeof(DateTime))
						{
							switch (column.ColumnFormat)
							{
								case ColumnFormat.PackedDateDateTimeNullable:
									column.SetValue(record, TranslateEbcdic.ToDateTimeNullableFromPacked(columnBuffer).GetValueOrDefault());
									break;
								default:
									column.SetValue(record, TranslateEbcdic.ToDateTimeNullable(columnBuffer).GetValueOrDefault());
									break;
							}
						}
					}

					offset += column.ColumnSize;
				}

				return record;
			}
		}
	}
}
