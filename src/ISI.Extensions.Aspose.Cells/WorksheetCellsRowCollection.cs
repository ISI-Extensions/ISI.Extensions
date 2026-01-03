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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.SpreadSheets;
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class WorksheetCellsRowCollection : ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection
		{
			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCells _worksheetCells = null;
			internal readonly global::Aspose.Cells.RowCollection _rows = null;

			internal WorksheetCellsRowCollection(ISI.Extensions.Aspose.Cells.WorksheetCells worksheetCells, global::Aspose.Cells.RowCollection rows)
			{
				_worksheetCells = worksheetCells;
				_rows = rows;
			}

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRow this[int rowIndex] => new ISI.Extensions.Aspose.Cells.WorksheetCellsRow(this, _rows[rowIndex]);

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRow Insert(int rowIndex)
			{
				_worksheetCells.GetAsposeCells().InsertRow(rowIndex);

				return this[rowIndex];
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

			public IEnumerator<ISI.Extensions.SpreadSheets.IWorksheetCellsRow> GetEnumerator() => new WorksheetCellsRowsEnumerator(this, _rows);

			public class WorksheetCellsRowsEnumerator : IEnumerator<ISI.Extensions.SpreadSheets.IWorksheetCellsRow>
			{
				internal ISI.Extensions.Aspose.Cells.WorksheetCellsRowCollection _worksheetCellsRows = null;
				internal global::Aspose.Cells.RowCollection _rows = null;

				internal WorksheetCellsRowsEnumerator(ISI.Extensions.Aspose.Cells.WorksheetCellsRowCollection worksheetCellsRows, global::Aspose.Cells.RowCollection rows)
				{
					_worksheetCellsRows = worksheetCellsRows;
					_rows = rows;
				}

				private IEnumerator _enumerator = null;

				public IWorksheetCellsRow Current { get; private set; }

				object IEnumerator.Current => Current;

				public void Reset()
				{
					(_enumerator as IDisposable)?.Dispose();
					_enumerator = null;
				}

				public bool MoveNext()
				{
					if (_enumerator == null)
					{
						_enumerator = _rows.GetEnumerator();
					}

					if (_enumerator.MoveNext())
					{
						Current = new ISI.Extensions.Aspose.Cells.WorksheetCellsRow(_worksheetCellsRows, _enumerator.Current as global::Aspose.Cells.Row);
						return true;
					}

					return false;
				}

				public void Dispose()
				{
					Reset();
					_worksheetCellsRows = null;
					_rows = null;
				}
			}
		}
	}
}