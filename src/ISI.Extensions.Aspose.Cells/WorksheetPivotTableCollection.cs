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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetPivotTableCollection : ISI.Extensions.SpreadSheets.IWorksheetPivotTableCollection
		{
			internal readonly ISI.Extensions.Aspose.Cells.Worksheet _worksheet = null;
			internal readonly global::Aspose.Cells.Pivot.PivotTableCollection _pivotTables = null;

			internal WorksheetPivotTableCollection(ISI.Extensions.Aspose.Cells.Worksheet worksheet, global::Aspose.Cells.Pivot.PivotTableCollection pivotTables)
			{
				_worksheet = worksheet;
				_pivotTables = pivotTables;
			}

			public ISI.Extensions.SpreadSheets.IWorksheetPivotTable this[string pivotTableName] => new ISI.Extensions.Aspose.Cells.WorksheetPivotTable(this, _pivotTables[pivotTableName]);
			public ISI.Extensions.SpreadSheets.IWorksheetPivotTable this[int startRow, int startColumn] => new ISI.Extensions.Aspose.Cells.WorksheetPivotTable(this, _pivotTables[startRow, startColumn]);

			public ISI.Extensions.SpreadSheets.IWorksheetPivotTable Add(string sourceData, int startRow, int startColumn, string tableName, bool useSameSource = true)
			{
				var pivotTableIndex = _pivotTables.Add(sourceData, startRow, startColumn, tableName, useSameSource);

				return new ISI.Extensions.Aspose.Cells.WorksheetPivotTable(this, _pivotTables[pivotTableIndex]);
			}
		}
	}
}