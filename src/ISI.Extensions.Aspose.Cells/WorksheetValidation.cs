#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class WorksheetValidation : ISI.Extensions.SpreadSheets.IWorksheetValidation
		{
			internal readonly ISI.Extensions.Aspose.Cells.Worksheet _worksheet = null;
			internal readonly global::Aspose.Cells.Validation _validation = null;

			internal WorksheetValidation(ISI.Extensions.Aspose.Cells.Worksheet worksheet, global::Aspose.Cells.Validation validation)
			{
				_worksheet = worksheet;
				_validation = validation;
			}


			public ISI.Extensions.SpreadSheets.OperatorType OperatorType
			{
				get => _validation.Operator.ToOperatorType();
				set => _validation.Operator = value.ToOperatorType();
			}

			public ISI.Extensions.SpreadSheets.ValidationAlertType AlertType
			{
				get => _validation.AlertStyle.ToValidationAlertType();
				set => _validation.AlertStyle = value.ToValidationAlertType();
			}


			public ISI.Extensions.SpreadSheets.ValidationType Type
			{
				get => _validation.Type.ToValidationType();
				set => _validation.Type = value.ToValidationType();
			}

			public string InputMessage
			{
				get => _validation.InputMessage;
				set => _validation.InputMessage = value;
			}

			public string InputTitle
			{
				get => _validation.InputTitle;
				set => _validation.InputTitle = value;
			}

			public string ErrorMessage
			{
				get => _validation.ErrorMessage;
				set => _validation.ErrorMessage = value;
			}

			public string ErrorTitle
			{
				get => _validation.ErrorTitle;
				set => _validation.ErrorTitle = value;
			}

			public bool ShowInput
			{
				get => _validation.ShowInput;
				set => _validation.ShowInput = value;
			}

			public bool ShowError
			{
				get => _validation.ShowError;
				set => _validation.ShowError = value;
			}

			public bool IgnoreBlank
			{
				get => _validation.IgnoreBlank;
				set => _validation.IgnoreBlank = value;
			}

			public string GetFormula1(bool isR1C1, bool isLocal) => _validation.GetFormula1(isR1C1, isLocal);
			public string GetFormula2(bool isR1C1, bool isLocal) => _validation.GetFormula2(isR1C1, isLocal);
			public string GetFormula1(bool isR1C1, bool isLocal, int row, int column) => _validation.GetFormula1(isR1C1, isLocal, row, column);
			public string GetFormula2(bool isR1C1, bool isLocal, int row, int column) => _validation.GetFormula2(isR1C1, isLocal, row, column);

			public void SetFormula1(string formula, bool isR1C1, bool isLocal) => _validation.SetFormula1(formula, isR1C1, isLocal);
			public void SetFormula2(string formula, bool isR1C1, bool isLocal) => _validation.SetFormula2(formula, isR1C1, isLocal);

			public string Formula1
			{
				get => _validation.Formula1;
				set => _validation.Formula1 = value;
			}

			public string Formula2
			{
				get => _validation.Formula2;
				set => _validation.Formula2 = value;
			}

			public object GetListValue(int row, int column) => _validation.GetListValue(row, column);

			public object Value1
			{
				get => _validation.Value1;
				set => _validation.Value1 = value;
			}

			public object Value2
			{
				get => _validation.Value2;
				set => _validation.Value2 = value;
			}

			public bool InCellDropDown
			{
				get => _validation.InCellDropDown;
				set => _validation.InCellDropDown = value;
			}

			public ISI.Extensions.SpreadSheets.CellArea[] Areas => _validation.Areas.ToNullCheckedArray(area => area.ToCellArea());

			public void AddArea(ISI.Extensions.SpreadSheets.CellArea cellArea) => _validation.AddArea(cellArea.ToCellArea());
			public void AddArea(ISI.Extensions.SpreadSheets.CellArea cellArea, bool checkIntersection, bool checkEdge) => _validation.AddArea(cellArea.ToCellArea(), checkIntersection, checkEdge);

			public void AddAreas(ISI.Extensions.SpreadSheets.CellArea[] areas, bool checkIntersection, bool checkEdge) => _validation.AddAreas(areas.ToNullCheckedArray(area => area.ToCellArea()), checkIntersection, checkEdge);

			public void RemoveArea(ISI.Extensions.SpreadSheets.CellArea cellArea) => _validation.RemoveArea(cellArea.ToCellArea());

			public void RemoveAreas(ISI.Extensions.SpreadSheets.CellArea[] areas) => _validation.RemoveAreas(areas.ToNullCheckedArray(area => area.ToCellArea()));

			public void RemoveACell(int row, int column) => _validation.RemoveACell(row, column);

			public void Copy(ISI.Extensions.SpreadSheets.IWorksheetValidation source, ISI.Extensions.SpreadSheets.CopyOptions copyOptions) => _validation.Copy(((WorksheetValidation) source)._validation, copyOptions.ToCopyOptions());
		}
	}
}