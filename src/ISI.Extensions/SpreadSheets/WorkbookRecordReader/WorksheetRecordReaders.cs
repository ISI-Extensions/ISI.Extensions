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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.SpreadSheets
{
	public class WorksheetRecordReader<TRecord> : IEnumerable<TRecord>, IDisposable
		where TRecord : class, new()
	{
		protected ISI.Extensions.SpreadSheets.IWorkbook Workbook { get; }
		protected bool DoWorkbookDisposable { get; }
		protected ISI.Extensions.SpreadSheets.IWorksheet Worksheet { get; }

		protected ISI.Extensions.Columns.IColumn<TRecord>[] Columns { get; }
		protected ISI.Extensions.Parsers.OnRead<TRecord>[] OnReads { get; }

		private class WorksheetRecordReaderEnumerator : IEnumerator<TRecord>
		{
			public ISI.Extensions.Parsers.IRecordDepth Context { get; private set; }

			protected IEnumerable<ISI.Extensions.SpreadSheets.IWorksheetCellsRow> Rows { get; }
			protected System.Collections.IEnumerator RowEnumerator { get; set; }

			protected ISI.Extensions.Columns.ColumnCollection<TRecord> Columns { get; }
			protected IDictionary<string, int> ColumnLookUp { get; }
			private int[] _columnIndexes;
			protected int[] ColumnIndexes => _columnIndexes;

			protected ISI.Extensions.Parsers.OnRead<TRecord>[] OnReads { get; }

			public WorksheetRecordReaderEnumerator(IEnumerable<ISI.Extensions.SpreadSheets.IWorksheetCellsRow> rows, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, ISI.Extensions.Parsers.OnRead<TRecord>[] onReads)
			{
				Rows = rows;

				Columns = new ISI.Extensions.Columns.ColumnCollection<TRecord>((columns ?? ISI.Extensions.Columns.ColumnCollection<TRecord>.GetDefault()));

				ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
				for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
				{
					foreach (var columnName in Columns[columnIndex].ColumnNames)
					{
						if (!string.IsNullOrEmpty(columnName))
						{
							ColumnLookUp.Add(columnName, columnIndex);
						}
					}
				}
				_columnIndexes = Enumerable.Range(0, Columns.Count - 1).ToArray();

				OnReads = onReads.ToNullCheckedArray(ISI.Extensions.Extensions.NullCheckCollectionResult.Empty);

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
				RowEnumerator ??= Rows.GetEnumerator();

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
						var value = row.GetCellOrNull(columnIndex)?.Value;

						values[columnIndex] = value;
						rawValues[columnIndex] = value;
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

		public WorksheetRecordReader(string fileName, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(fileName);
			DoWorkbookDisposable = true;
			Worksheet = Workbook.Worksheets.NullCheckedFirstOrDefault();
		}

		public WorksheetRecordReader(System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(stream);
			DoWorkbookDisposable = true;
			Worksheet = Workbook.Worksheets.NullCheckedFirstOrDefault();
		}

		public WorksheetRecordReader(ISI.Extensions.SpreadSheets.IWorksheet worksheet, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			Worksheet = worksheet;
		}

		private WorksheetRecordReader(IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
		{
			Columns = (columns ?? ISI.Extensions.Columns.ColumnCollection<TRecord>.GetDefault()).ToNullCheckedArray(NullCheckCollectionResult.Empty);
			OnReads = onReads.ToNullCheckedArray(NullCheckCollectionResult.Empty);
		}

		public IEnumerator<TRecord> GetEnumerator()
		{
			return new WorksheetRecordReaderEnumerator(Worksheet.Cells.Rows, Columns, OnReads);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			if (DoWorkbookDisposable)
			{
				Workbook?.Dispose();
			}
		}
	}
}
