#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
	public interface IWorksheetValidation
	{
		ISI.Extensions.SpreadSheets.OperatorType OperatorType { get; set; }
		ISI.Extensions.SpreadSheets.ValidationAlertType AlertType { get; set; }
		ISI.Extensions.SpreadSheets.ValidationType Type { get; set; }
		string InputMessage { get; set; }
		string InputTitle { get; set; }
		string ErrorMessage { get; set; }
		string ErrorTitle { get; set; }
		bool ShowInput { get; set; }
		bool ShowError { get; set; }
		bool IgnoreBlank { get; set; }
		string Formula1 { get; set; }
		string Formula2 { get; set; }
		object Value1 { get; set; }
		object Value2 { get; set; }
		bool InCellDropDown { get; set; }
		ISI.Extensions.SpreadSheets.CellArea[] Areas { get; }
		string GetFormula1(bool isR1C1, bool isLocal);
		string GetFormula2(bool isR1C1, bool isLocal);
		string GetFormula1(bool isR1C1, bool isLocal, int row, int column);
		string GetFormula2(bool isR1C1, bool isLocal, int row, int column);
		void SetFormula1(string formula, bool isR1C1, bool isLocal);
		void SetFormula2(string formula, bool isR1C1, bool isLocal);
		object GetListValue(int row, int column);
		void AddArea(ISI.Extensions.SpreadSheets.CellArea cellArea);
		void AddArea(ISI.Extensions.SpreadSheets.CellArea cellArea, bool checkIntersection, bool checkEdge);
		void AddAreas(ISI.Extensions.SpreadSheets.CellArea[] areas, bool checkIntersection, bool checkEdge);
		void RemoveArea(ISI.Extensions.SpreadSheets.CellArea cellArea);
		void RemoveAreas(ISI.Extensions.SpreadSheets.CellArea[] areas);
		void RemoveACell(int row, int column);
		void Copy(ISI.Extensions.SpreadSheets.IWorksheetValidation source, ISI.Extensions.SpreadSheets.CopyOptions copyOptions);
	}
}
