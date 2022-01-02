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
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class Workbook : ISI.Extensions.SpreadSheets.IWorkbook
		{
			public static ISI.Extensions.SpreadSheets.IWorkbook OpenWorkbook(string fileName) => new Workbook(fileName);
			public static ISI.Extensions.SpreadSheets.IWorkbook OpenWorkbook(System.IO.Stream stream) => new Workbook(stream);

			internal readonly global::Aspose.Cells.Workbook _workbook = null;

			private global::Aspose.Cells.WorkbookDesigner _workbookDesigner = null;
			protected global::Aspose.Cells.WorkbookDesigner WorkbookDesigner => (_workbookDesigner ??= new global::Aspose.Cells.WorkbookDesigner(_workbook));

			public Workbook()
			{
				_workbook = new global::Aspose.Cells.Workbook();
			}

			public Workbook(string fileName)
			{
				_workbook = new global::Aspose.Cells.Workbook(fileName);
			}

			public Workbook(string fileName, ISI.Extensions.SpreadSheets.ILoadOptions loadOptions)
			{
				_workbook = new global::Aspose.Cells.Workbook(fileName, loadOptions.ToLoadOptions());
			}

			public Workbook(System.IO.Stream stream)
			{
				_workbook = new global::Aspose.Cells.Workbook(stream);
			}

			public Workbook(System.IO.Stream stream, ISI.Extensions.SpreadSheets.ILoadOptions loadOptions)
			{
				_workbook = new global::Aspose.Cells.Workbook(stream, loadOptions.ToLoadOptions());
			}

			public void SetDocumentProperties(ISI.Extensions.Documents.IDocumentProperties documentProperties)
			{
				_workbook.SetDocumentProperties(documentProperties);
			}

			public ISI.Extensions.SpreadSheets.IWorksheetCollection Worksheets => new WorksheetCollection(this, _workbook.Worksheets);

			public void Save(System.IO.Stream documentStream, string fileNameExtension)
			{
				fileNameExtension = System.IO.Path.GetExtension(fileNameExtension);

				var fileFormat = ISI.Extensions.Extensions.SpreadSheetExtensions.ToFileFormat(fileNameExtension);

				_workbook.Save(documentStream, fileFormat);
			}

			public void Save(System.IO.Stream documentStream, ISI.Extensions.SpreadSheets.FileFormat fileFormat)
			{
				_workbook.Save(documentStream, fileFormat);
			}

			public void Save(System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				_workbook.Save(documentStream, fileFormat);
			}

			public void Print(string printerName)
			{
				_workbook.Print(printerName);
			}

			public void Print(System.Drawing.Printing.PrinterSettings printerSettings, string printerName)
			{
				_workbook.Print(printerSettings, printerName);
			}

			public void ProcessDataSources()
			{
				WorkbookDesigner.Process();
			}

			public void CalculateFormula()
			{
				_workbook.CalculateFormula();
			}



			public void SetDataSource(string variable, object data)
			{
				WorkbookDesigner.SetDataSource(variable, data);
			}

			public ISI.Extensions.SpreadSheets.ICellStyle CreateCellStyle(string name = null)
			{
				var style = _workbook.CreateStyle();

				if (!string.IsNullOrWhiteSpace(name))
				{
					style.Name = name;
				}

				return new ISI.Extensions.Aspose.Cells.CellStyle(this, style);
			}

			public ISI.Extensions.SpreadSheets.ICellStyle GetNamedStyle(string name)
			{
				return new ISI.Extensions.Aspose.Cells.CellStyle(this, _workbook.GetNamedStyle(name));
			}

			public void Dispose()
			{
				_workbook?.Dispose();
			}
		}
	}
}
