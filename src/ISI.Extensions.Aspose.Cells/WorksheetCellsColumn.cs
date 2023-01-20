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
		public class WorksheetCellsColumn : ISI.Extensions.SpreadSheets.IWorksheetCellsColumn
		{
			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCellsColumnCollection _worksheetColumns = null;
			internal readonly global::Aspose.Cells.Column _column = null;

			internal WorksheetCellsColumn(ISI.Extensions.Aspose.Cells.WorksheetCellsColumnCollection worksheetColumns, global::Aspose.Cells.Column column)
			{
				_worksheetColumns = worksheetColumns;
				_column = column;
			}

			public void ApplyStyle(ISI.Extensions.SpreadSheets.ICellStyle style, ISI.Extensions.SpreadSheets.SetStyleFlag flag)
			{
				_column.ApplyStyle(style.GetAsposeStyle(), flag.ToSetStyleFlag());
			}

			public int Index => _column.Index;

			public double Width
			{
				get => _column.Width;
				set => _column.Width = value;
			}

			public bool IsHidden
			{
				get => _column.IsHidden;
				set => _column.IsHidden = value;
			}

			public ISI.Extensions.SpreadSheets.ICellStyle Style => _column.Style.NullCheckedConvert(style => new ISI.Extensions.Aspose.Cells.CellStyle(_worksheetColumns._worksheetCells._worksheet._worksheets._workbook, _column.Style));
		}
	}
}