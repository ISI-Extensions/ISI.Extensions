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
		public class WorksheetCellsRow : ISI.Extensions.SpreadSheets.IWorksheetCellsRow
		{
			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCellsRowCollection _worksheetRows = null;
			internal readonly global::Aspose.Cells.Row _row = null;

			internal WorksheetCellsRow(ISI.Extensions.Aspose.Cells.WorksheetCellsRowCollection worksheetRows, global::Aspose.Cells.Row row)
			{
				_worksheetRows = worksheetRows;
				_row = row;
			}

			public void ApplyStyle(ISI.Extensions.SpreadSheets.ICellStyle style, ISI.Extensions.SpreadSheets.SetStyleFlag flag)
			{
				_row.ApplyStyle(style.GetAsposeStyle(), flag.ToSetStyleFlag());
			}

			public int Index => _row.Index;

			public double Height
			{
				get => _row.Height;
				set => _row.Height = value;
			}

			public bool IsHidden
			{
				get => _row.IsHidden;
				set => _row.IsHidden = value;
			}

			public ISI.Extensions.SpreadSheets.ICellStyle Style => _row.Style.NullCheckedConvert(style => new ISI.Extensions.Aspose.Cells.CellStyle(_worksheetRows._worksheetCells._worksheet._worksheets._workbook, _row.Style));

			public ISI.Extensions.SpreadSheets.IWorksheetCell LastCell => new WorksheetCell(_worksheetRows._worksheetCells, _row.LastCell);

			public ISI.Extensions.SpreadSheets.IWorksheetCell GetCellOrNull(int columnIndex)
			{
				var cell = _row.GetCellOrNull(columnIndex);

				return (cell == null ? null : new WorksheetCell(_worksheetRows._worksheetCells, cell));
			}
		}
	}
}