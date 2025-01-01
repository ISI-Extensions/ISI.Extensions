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

namespace ISI.Extensions.SpreadSheets
{
	public interface IWorksheetCells
	{
		ISI.Extensions.SpreadSheets.IWorksheetCell this[int row, int column] { get; }
		ISI.Extensions.SpreadSheets.IWorksheetCell this[string cellName] { get; }
		ISI.Extensions.SpreadSheets.IWorksheetCellsRowCollection Rows { get; }
		ISI.Extensions.SpreadSheets.IWorksheetCellsColumnCollection Columns { get; }

		ISI.Extensions.SpreadSheets.AddRecordsResponse AddRecords<TRecord>(IEnumerable<TRecord> records, int startRow, int startColumn, ISI.Extensions.SpreadSheets.AddRecordsColumnCollection<TRecord> columnDefinitions = null);
		ISI.Extensions.SpreadSheets.AddSummaryResponse AddSummary<TSummary>(TSummary summary, int startRow, int startColumn, int valueColumnOffset = 1, ISI.Extensions.SpreadSheets.AddSummaryRowCollection<TSummary> rowDefinitions = null);

		ISI.Extensions.SpreadSheets.IWorksheetCell Find(object what, ISI.Extensions.SpreadSheets.IWorksheetCell previousCell, ISI.Extensions.SpreadSheets.FindOptions findOptions);
		void Merge(int startRow, int startColumn, int rowCount, int columnCount);

		ISI.Extensions.SpreadSheets.IWorksheetCell LastCell { get; }
		void DeleteBlankRows();
		void InsertRow(int rowIndex);
		void InsertRows(int rowIndex, int totalRows);
		void InsertRows(int rowIndex, int totalRows, bool updateReference);
		void DeleteRow(int rowIndex);
		void DeleteRows(int rowIndex, int totalRows);
		void DeleteRows(int rowIndex, int totalRows, bool updateReference);
		void InsertColumn(int columnIndex);
		void InsertColumns(int columnIndex, int totalColumns);
		void InsertColumns(int columnIndex, int totalColumns, bool updateReference);
		void DeleteColumn(int columnIndex);
		void DeleteColumns(int columnIndex, int totalColumns);
		void DeleteColumns(int columnIndex, int totalColumns, bool updateReference);

		ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(string upperLeftCell, string lowerRightCell);
		ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(int firstRow, int firstColumn, int totalRows, int totalColumns);
		ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(string address);
		ISI.Extensions.SpreadSheets.IWorksheetCellsRange CreateRange(int firstIndex, int number, bool isVertical);
	}
}
