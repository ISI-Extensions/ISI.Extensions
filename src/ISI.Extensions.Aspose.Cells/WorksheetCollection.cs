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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetCollection : ISI.Extensions.SpreadSheets.IWorksheetCollection
		{
			internal readonly ISI.Extensions.Aspose.Cells.Workbook _workbook = null;
			internal readonly global::Aspose.Cells.WorksheetCollection _worksheets = null;

			internal WorksheetCollection(ISI.Extensions.Aspose.Cells.Workbook workbook, global::Aspose.Cells.WorksheetCollection worksheets)
			{
				_workbook = workbook;
				_worksheets = worksheets;
			}

			public ISI.Extensions.SpreadSheets.IWorksheet this[int sheetIndex] => new Worksheet(this, _worksheets[sheetIndex]);
			public ISI.Extensions.SpreadSheets.IWorksheet this[string name] => new Worksheet(this, _worksheets[name]);

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange[] GetNamedRanges()
			{
				return _worksheets.GetNamedRanges().ToNullCheckedArray(range => new WorksheetCellsRange(this, range), NullCheckCollectionResult.Empty);
			}

			public ISI.Extensions.SpreadSheets.IWorksheetCellsRange GetRangeByName(string rangeName)
			{
				var range = _worksheets.GetRangeByName(rangeName);

				return (range == null ? null : new WorksheetCellsRange(this, range));
			}

			public IEnumerator<ISI.Extensions.SpreadSheets.IWorksheet> GetEnumerator()
			{
				return _worksheets.Select(worksheet => new ISI.Extensions.Aspose.Cells.Worksheet(this, worksheet)).GetEnumerator();
			}

			public void Clear()
			{
				_worksheets.Clear();
			}

			public ISI.Extensions.SpreadSheets.IWorksheet Add(string sheetName = null)
			{
				if (string.IsNullOrWhiteSpace(sheetName))
				{
					var sheetNames = new HashSet<string>(_worksheets.Select(w => w.Name));

					var sheetIndex = sheetNames.Count + 1;

					while (sheetNames.Contains((sheetName = string.Format("sheet{0}", sheetIndex))))
					{
						sheetIndex++;
					}
				}

				var worksheet = _worksheets.Add(sheetName.Length > Worksheet.MaxSheetNameLength ? sheetName.Substring(0, Worksheet.MaxSheetNameLength) : sheetName);

				return new Worksheet(this, worksheet);
			}

			public void Remove(string name) => _worksheets.RemoveAt(name);
			public void RemoveAt(int sheetIndex) => _worksheets.RemoveAt(sheetIndex);

			public ISI.Extensions.SpreadSheets.IWorksheet ActiveWorksheet
			{
				get => this[_workbook.GetAsposeWorkbook().Worksheets.ActiveSheetIndex];
				set => _workbook.GetAsposeWorkbook().Worksheets.ActiveSheetIndex = ((ISI.Extensions.Aspose.Cells.Worksheet) value).Index;
			}
		}
	}
}