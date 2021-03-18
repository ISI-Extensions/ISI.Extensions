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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetCells : ISI.Extensions.SpreadSheets.IWorksheetCells
		{
			internal readonly ISI.Extensions.Aspose.Cells.Worksheet _worksheet = null;
			internal readonly global::Aspose.Cells.Cells _worksheetCells = null;

			internal WorksheetCells(ISI.Extensions.Aspose.Cells.Worksheet worksheet, global::Aspose.Cells.Cells worksheetCells)
			{
				_worksheet = worksheet;
				_worksheetCells = worksheetCells;
			}

			public ISI.Extensions.SpreadSheets.IWorksheetCell this[int row, int column] => new WorksheetCell(this, _worksheetCells[row, column]);
			public ISI.Extensions.SpreadSheets.IWorksheetCell this[string cellName] => new WorksheetCell(this, _worksheetCells[cellName]);

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection Rows => new ISI.Extensions.Aspose.Cells.WorksheetCellsRowCollection(this, _worksheetCells.Rows);
			public ISI.Extensions.SpreadSheets.IWorksheetCellsColumnCollection Columns => new ISI.Extensions.Aspose.Cells.WorksheetCellsColumnCollection(this, _worksheetCells.Columns);

			public ISI.Extensions.SpreadSheets.IWorksheetCell LastCell => new WorksheetCell(this, _worksheetCells.LastCell);

			public void DeleteBlankRows()
			{
				_worksheetCells.DeleteBlankRows();
			}

			public void InsertRow(int rowIndex) => _worksheetCells.InsertRow(rowIndex);
			public void InsertRows(int rowIndex, int totalRows) => _worksheetCells.InsertRows(rowIndex, totalRows);
			public void InsertRows(int rowIndex, int totalRows, bool updateReference) => _worksheetCells.InsertRows(rowIndex, totalRows, updateReference);
			public void DeleteRow(int rowIndex) => _worksheetCells.DeleteRow(rowIndex);
			public void DeleteRows(int rowIndex, int totalRows) => _worksheetCells.DeleteRows(rowIndex, totalRows);
			public void DeleteRows(int rowIndex, int totalRows, bool updateReference) => _worksheetCells.DeleteRows(rowIndex, totalRows, updateReference);

			public void InsertColumn(int columnIndex) => _worksheetCells.InsertColumn(columnIndex);
			public void InsertColumns(int columnIndex, int totalColumns) => _worksheetCells.InsertColumns(columnIndex, totalColumns);
			public void InsertColumns(int columnIndex, int totalColumns, bool updateReference) => _worksheetCells.InsertColumns(columnIndex, totalColumns, updateReference);
			public void DeleteColumn(int columnIndex) => _worksheetCells.DeleteColumn(columnIndex);
			public void DeleteColumns(int columnIndex, int totalColumns) => _worksheetCells.DeleteColumns(columnIndex, totalColumns, true);
			public void DeleteColumns(int columnIndex, int totalColumns, bool updateReference) => _worksheetCells.DeleteColumns(columnIndex, totalColumns, updateReference);

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(string upperLeftCell, string lowerRightCell) => new WorksheetCellsRange(_worksheet._worksheets, _worksheetCells.CreateRange(upperLeftCell, lowerRightCell));
			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(int firstRow, int firstColumn, int totalRows, int totalColumns) => new WorksheetCellsRange(_worksheet._worksheets, _worksheetCells.CreateRange(firstRow, firstColumn, totalRows, totalColumns));
			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(string address) => new WorksheetCellsRange(_worksheet._worksheets, _worksheetCells.CreateRange(address));
			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(int firstIndex, int number, bool isVertical) => new WorksheetCellsRange(_worksheet._worksheets, _worksheetCells.CreateRange(firstIndex, number, isVertical));
		}
	}
}