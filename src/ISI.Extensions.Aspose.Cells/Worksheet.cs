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
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class Worksheet : ISI.Extensions.SpreadSheets.IWorksheet
		{
			public const int MaxSheetNameLength = 31;

			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCollection _worksheets = null;
			internal readonly global::Aspose.Cells.Worksheet _worksheet = null;

			internal Worksheet(ISI.Extensions.Aspose.Cells.WorksheetCollection worksheets, global::Aspose.Cells.Worksheet worksheet)
			{
				_worksheets = worksheets;
				_worksheet = worksheet;
			}

			public string Name
			{
				get => _worksheet.Name;
				set => _worksheet.Name = (value.Length > MaxSheetNameLength ? value.Substring(0, MaxSheetNameLength) : value);
			}

			public int Index => _worksheet.Index;


			public void Copy(ISI.Extensions.SpreadSheets.IWorksheet worksheet) => _worksheet.Copy(worksheet.GetAsposeWorksheet());

			public void MoveTo(int index) => _worksheet.MoveTo(index);

			public ISI.Extensions.SpreadSheets.IWorksheetPageSetup PageSetup => new WorksheetPageSetup(this, _worksheet.PageSetup);

			public ISI.Extensions.SpreadSheets.IWorksheetCells Cells => new WorksheetCells(this, _worksheet.Cells);

			public ISI.Extensions.SpreadSheets.IWorksheetCommentCollection Comments => new WorksheetCommentCollection(this, _worksheet.Comments);

			public ISI.Extensions.SpreadSheets.IHyperlinkCollection Hyperlinks => new ISI.Extensions.Aspose.Cells.HyperlinkCollection(this, _worksheet.Hyperlinks);

			public ISI.Extensions.SpreadSheets.IWorksheetPivotTableCollection PivotTables => new ISI.Extensions.Aspose.Cells.WorksheetPivotTableCollection(this, _worksheet.PivotTables);

			public void FreezePanes(int startRow, int startColumn, int visibleTopRowCount, int visibleLeftColumnCount)
			{
				_worksheet.FreezePanes(startRow, startColumn, visibleTopRowCount, visibleLeftColumnCount);
			}

			public ISI.Extensions.SpreadSheets.IWorksheetAutoFilter AutoFilter => new ISI.Extensions.Aspose.Cells.WorksheetAutoFilter(this, _worksheet.AutoFilter);

			public void AutoFitColumns()
			{
				_worksheet.AutoFitColumns();
			}
			public void AutoFitColumns(int firstColumn, int lastColumn)
			{
				_worksheet.AutoFitColumns(firstColumn, lastColumn);
			}
			public void AutoFitColumns(int firstRow, int firstColumn, int lastRow, int lastColumn)
			{
				_worksheet.AutoFitColumns(firstRow, firstColumn, lastRow, lastColumn);
			}

			public void CovertToTable(int firstRow, int firstColumn, int lastRow, int lastColumn, bool hasHeader, ISI.Extensions.SpreadSheets.TableStyle tableStyle, string tableName = null)
			{
				var table = _worksheet.ListObjects[_worksheet.ListObjects.Add(firstRow, firstColumn, lastRow, lastColumn, hasHeader)];

				if (!string.IsNullOrEmpty(tableName))
				{
					table.DisplayName = tableName;
				}

				table.TableStyleType = tableStyle.ToTableStyle();
			}

			public void CovertToTable(ISI.Extensions.SpreadSheets.AddRecordsResponse addRecordsResponse, ISI.Extensions.SpreadSheets.TableStyle tableStyle, string tableName = null)
			{
				CovertToTable(addRecordsResponse.HeaderRow ?? addRecordsResponse.StartRow, addRecordsResponse.StartColumn, addRecordsResponse.StopRow, addRecordsResponse.StopColumn, addRecordsResponse.HeaderRow.HasValue, tableStyle, tableName);
			}

			public bool IsProtected => _worksheet.IsProtected;

			public ISI.Extensions.SpreadSheets.IProtection Protection => new Protection(this);

			public void Protect(ISI.Extensions.SpreadSheets.ProtectionType protectionType)
			{
				_worksheet.Protect(protectionType.ToProtectionType());
			}

			public void Protect(ISI.Extensions.SpreadSheets.ProtectionType protectionType, string password, string oldPassword = null)
			{
				_worksheet.Protect(protectionType.ToProtectionType(), password, oldPassword);
			}

			public ISI.Extensions.SpreadSheets.IWorksheetPictureCollection Pictures => new WorksheetPictureCollection(this, _worksheet.Pictures);

			public ISI.Extensions.SpreadSheets.IWorksheetValidationCollection Validations => new WorksheetValidationCollection(this, _worksheet.Validations);
		}
	}
}
