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

namespace ISI.Extensions.SpreadSheets
{
	public partial class WorkbookRecordReader<TRecord>
	{
		public partial class WorksheetRecordReader
		{
			public partial class WorksheetRecords : IEnumerable<TRecord>
			{
				public class WorksheetRecordEnumerator : IEnumerator<TRecord>
				{
					public ISI.Extensions.Parsers.IRecordDepth Context { get; private set; }

					protected ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection Rows { get; }
					protected System.Collections.IEnumerator RowEnumerator { get; set; }

					protected ISI.Extensions.Parsers.IColumnInfo<TRecord>[] Columns { get; }
					protected IDictionary<string, int> ColumnLookUp { get; }
					private int[] _columnIndexes;
					protected int[] ColumnIndexes => _columnIndexes;

					protected ISI.Extensions.Parsers.OnRead<TRecord>[] OnReads { get; }

					public WorksheetRecordEnumerator(ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection rows, ISI.Extensions.Parsers.IColumnInfo<TRecord>[] columns, ISI.Extensions.Parsers.OnRead<TRecord>[] onReads)
					{
						Rows = rows;

						Columns = columns ?? ISI.Extensions.Parsers.ColumnInfoCollection<TRecord>.GetDefault().ToArray();

						ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
						for (var columnIndex = 0; columnIndex < Columns.Length; columnIndex++)
						{
							foreach (var columnName in Columns[columnIndex].ColumnNames)
							{
								if (!string.IsNullOrEmpty(columnName))
								{
									ColumnLookUp.Add(columnName, columnIndex);
								}
							}
						}
						_columnIndexes = Enumerable.Range(0, Columns.Length - 1).ToArray();

						OnReads = onReads.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						Reset();
					}

					public void Reset()
					{
						Current = null;
						(RowEnumerator as IDisposable)?.Dispose();
						RowEnumerator = null;

						Context = new ISI.Extensions.Parsers.RecordContext()
						{
							Depth = 0
						};
					}

					public bool MoveNext()
					{
						if (RowEnumerator == null)
						{
							RowEnumerator = Rows.GetEnumerator();
						}

						do
						{
							if (!RowEnumerator.MoveNext())
							{
								Current = null;
								return false;
							}


							if (!(RowEnumerator.Current is ISI.Extensions.SpreadSheets.IWorksheetCellsRow row))
							{
								return false;
							}

							var columnCount = row.LastCell.Column + 1;
							var values = new object[columnCount];
							var rawValues = new object[columnCount];

							for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
							{
								values[columnIndex] = row.GetCellOrNull(columnIndex)?.Value;
								rawValues[columnIndex] = values[columnIndex];
							}

							foreach (var onRead in OnReads)
							{
								onRead(Context, null, ColumnLookUp, Columns, ref _columnIndexes, ref values);
							}

							if (values.NullCheckedAny())
							{
								var record = new TRecord();

								for (var valueIndex = 0; ((valueIndex < values.Length) && (valueIndex < ColumnIndexes.Length)); valueIndex++)
								{
									var columnIndex = ColumnIndexes[valueIndex];
									if (columnIndex >= 0)
									{
										Columns[columnIndex].SetValue(record, Columns[columnIndex].TransformValue(values[valueIndex]));
									}
								}

								Current = record;

								Context.Depth++;

								if (RowEnumerator.Current is ISI.Extensions.DataReader.IHasRowNumber hasRowNumber)
								{
									hasRowNumber.RowNumber = Context.Depth;
								}

								if (RowEnumerator.Current is ISI.Extensions.DataReader.IAcceptsRawValues acceptsRawValues)
								{
									acceptsRawValues.SetRawValues(rawValues);
								}

							}
						} while (Current == null);

						return true;
					}

					public TRecord Current { get; private set; }

					object System.Collections.IEnumerator.Current => Current;

					public void Dispose()
					{

					}
				}

				protected ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection Rows { get; }
				protected ISI.Extensions.Parsers.IColumnInfo<TRecord>[] Columns { get; }
				protected ISI.Extensions.Parsers.OnRead<TRecord>[] OnReads { get; }

				public WorksheetRecords(ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection rows, ISI.Extensions.Parsers.IColumnInfo<TRecord>[] columns, ISI.Extensions.Parsers.OnRead<TRecord>[] onReads)
				{
					Rows = rows;
					Columns = columns;
					OnReads = onReads;
				}

				public IEnumerator<TRecord> GetEnumerator()
				{
					return new WorksheetRecordEnumerator(Rows, Columns, OnReads);
				}

				System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}
			}
		}
	}
}
