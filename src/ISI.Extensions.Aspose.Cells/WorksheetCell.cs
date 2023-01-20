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
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetCell : ISI.Extensions.SpreadSheets.IWorksheetCell
		{
			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCells _worksheetCells = null;
			internal readonly global::Aspose.Cells.Cell _worksheetCell = null;

			internal WorksheetCell(ISI.Extensions.Aspose.Cells.WorksheetCells worksheetCells, global::Aspose.Cells.Cell worksheetCell)
			{
				_worksheetCells = worksheetCells;
				_worksheetCell = worksheetCell;
			}

			public int Row => _worksheetCell.Row;
			public int Column => _worksheetCell.Column;

			public string Name => _worksheetCell.Name;

			public ISI.Extensions.SpreadSheets.ICellStyle Style
			{
				get => new ISI.Extensions.Aspose.Cells.CellStyle(_worksheetCells._worksheet._worksheets.  _workbook, _worksheetCell.GetStyle());
				set => _worksheetCell.SetStyle(value.GetAsposeStyle());
			}

			public void SetStyle(string styleName)
			{
				Style = (string.IsNullOrWhiteSpace(styleName) ? null : _worksheetCells._worksheet._worksheets._workbook.GetNamedStyle(styleName));
			}
			public void SetStyle(ISI.Extensions.SpreadSheets.NumberFormat numberFormat)
			{
				var style = _worksheetCell.GetStyle();
				style.Number = numberFormat.ToNumberFormat();
				_worksheetCell.SetStyle(style);
			}
			public void SetCustomStyle(string customStyle)
			{
				var style = _worksheetCell.GetStyle();
				style.Custom = customStyle;
				_worksheetCell.SetStyle(style);
			}

			public void SetStyleBold(bool bold)
			{
				var style = _worksheetCell.GetStyle();
				style.Font.IsBold = bold;
				_worksheetCell.SetStyle(style);
			}

			public object Value
			{
				get => _worksheetCell.Value;
				set => _worksheetCell.Value = value;
			}

			public string StringValue
			{
				get => _worksheetCell.StringValue;
			}

			public string StringValueWithoutFormat
			{
				get => _worksheetCell.StringValueWithoutFormat;
			}

			public string Formula
			{
				get => _worksheetCell.Formula;
				set => _worksheetCell.Formula = value;
			}

			public bool IsFormula => _worksheetCell.IsFormula;

			public void UpdateCell(ISI.Extensions.SpreadSheets.UpdateCellDelegate updateCell)
			{
				updateCell?.Invoke(this);
			}
		}
	}
}