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

namespace ISI.Extensions.SpreadSheets
{
	public interface IWorksheet
	{
		string Name { get; set; }
		int Index { get; }

		void Copy(ISI.Extensions.SpreadSheets.IWorksheet worksheet);
		void MoveTo(int index);

		ISI.Extensions.SpreadSheets.IWorksheetPageSetup PageSetup { get; }
		ISI.Extensions.SpreadSheets.IWorksheetCells Cells { get; }
		ISI.Extensions.SpreadSheets.IWorksheetCommentCollection Comments { get; }
		ISI.Extensions.SpreadSheets.IHyperlinkCollection Hyperlinks { get; }
		ISI.Extensions.SpreadSheets.IWorksheetPivotTableCollection PivotTables { get; }
		ISI.Extensions.SpreadSheets.IWorksheetAutoFilter AutoFilter { get; }

		void FreezePanes(int startRow, int startColumn, int visibleTopRowCount, int visibleLeftColumnCount);

		void AutoFitColumns();
		void AutoFitColumns(int firstColumn, int lastColumn);
		void AutoFitColumns(int firstRow, int firstColumn, int lastRow, int lastColumn);
		void CovertToTable(int firstRow, int firstColumn, int lastRow, int lastColumn, bool hasHeader, ISI.Extensions.SpreadSheets.TableStyle tableStyle, string tableName = null);
		void CovertToTable(ISI.Extensions.SpreadSheets.AddRecordsResponse addRecordsResponse, ISI.Extensions.SpreadSheets.TableStyle tableStyle, string tableName = null);

		bool IsProtected { get; }
		ISI.Extensions.SpreadSheets.IProtection Protection { get; }
		void Protect(ISI.Extensions.SpreadSheets.ProtectionType protectionType, string password, string oldPassword = null);
		void Protect(ISI.Extensions.SpreadSheets.ProtectionType protectionType);

		ISI.Extensions.SpreadSheets.IWorksheetPictureCollection Pictures { get; }

		ISI.Extensions.SpreadSheets.IWorksheetValidationCollection Validations { get; }
	}
}
