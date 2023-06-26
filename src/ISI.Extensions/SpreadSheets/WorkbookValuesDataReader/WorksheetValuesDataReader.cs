#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.SpreadSheets
{
	public class WorksheetValuesDataReader : ISI.Extensions.DataReader.AbstractDataReader
	{
		public ISI.Extensions.Parsers.IRecordDepth Context { get; private set; }

		protected IEnumerable<ISI.Extensions.SpreadSheets.IWorksheetCellsRow> Rows { get; }
		protected System.Collections.IEnumerator RowEnumerator { get; set; }

		protected ISI.Extensions.Columns.IColumn[] Columns { get; }
		protected IDictionary<string, int> ColumnLookUp { get; }

		protected ISI.Extensions.DataReader.TransformRecord TransformRecord { get; }

		public WorksheetValuesDataReader(ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection rows, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(columns, transformRecord)
		{
			Rows = rows;
		}

		public WorksheetValuesDataReader(ISI.Extensions.SpreadSheets.IWorksheet worksheet, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
			: this(columns, transformRecord)
		{
			Rows = worksheet.Cells.Rows;
		}

		private WorksheetValuesDataReader(IEnumerable<ISI.Extensions.Columns.IColumn> columns, ISI.Extensions.DataReader.TransformRecord transformRecord)
		{
			Columns = columns.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			{
				ColumnLookUp = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
				var columnIndex = 0;
				foreach (var column in Columns)
				{
					if (!string.IsNullOrEmpty(column.ColumnName))
					{
						ColumnLookUp.Add(column.ColumnName, columnIndex);
					}
					columnIndex++;
				}
			}

			TransformRecord = transformRecord;

			Depth = 0;

			Reset();
		}

		public void Reset()
		{
			(RowEnumerator as IDisposable)?.Dispose();
			RowEnumerator = null;

			Context = new ISI.Extensions.Parsers.RecordContext()
			{
				Depth = 0
			};
		}

		public override int FieldCount
		{
			get => (Columns.NullCheckedAny() ? Columns.Length : Values.NullCheckedCount());
			protected set => _ = value;
		}

		public override string GetName(int columnIndex)
		{
			CheckColumnIndex(columnIndex);

			return Columns[columnIndex].ColumnName;
		}

		public override System.Data.DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public override int GetOrdinal(string name)
		{
			return ColumnLookUp[name];
		}

		public override bool NextResult()
		{
			Close();

			return false;
		}

		public override bool Read()
		{
			RowEnumerator ??= Rows.GetEnumerator();

			do
			{
				if (!RowEnumerator.MoveNext())
				{
					Values = null;

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

				TransformRecord?.Invoke(Depth, Columns, Source, ref values);

				if (values != null)
				{
					Values = values;

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
				else
				{
					Values = null;
				}
			} while (Values == null);

			return true;
		}

		public override void Close()
		{

		}
	}
}
