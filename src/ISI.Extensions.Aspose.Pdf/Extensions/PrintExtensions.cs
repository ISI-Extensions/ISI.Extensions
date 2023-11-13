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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class PrintExtensions
	{
		public static void Print(this global::Aspose.Pdf.Facades.Form form, string printerName)
		{
			if (!string.IsNullOrWhiteSpace(printerName))
			{
				using (var documentStream = new ISI.Extensions.Stream.TempFileStream())
				{
					form.Save(documentStream);

					documentStream.Rewind();

					var pdfViewer = new global::Aspose.Pdf.Facades.PdfViewer();

					pdfViewer.BindPdf(documentStream);

					Print(pdfViewer, printerName);
				}
			}
		}

		public static void Print(this global::Aspose.Pdf.Document document, string printerName)
		{
			Print(new global::Aspose.Pdf.Facades.PdfViewer(document), printerName);
		}

		public static void Print(this global::Aspose.Pdf.Facades.PdfViewer pdfViewer, string printerName)
		{
			if (!string.IsNullOrWhiteSpace(printerName))
			{
				var printerSettings = new global::Aspose.Pdf.Printing.PrinterSettings()
				{
					PrinterName = printerName
				};

				pdfViewer.PrintDocumentWithSettings(printerSettings);
			}
		}

		public static void Print(this global::Aspose.Pdf.Facades.Form form, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			using (var documentStream = new ISI.Extensions.Stream.TempFileStream())
			{
				form.Save(documentStream);

				documentStream.Rewind();

				var pdfViewer = new global::Aspose.Pdf.Facades.PdfViewer();

				pdfViewer.BindPdf(documentStream);

				pdfViewer.PrintDocumentWithSettings(printerSettings.ToPrinterSettings());
			}
		}

		public static void Print(this global::Aspose.Pdf.Document document, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			var pdfViewer = new global::Aspose.Pdf.Facades.PdfViewer(document);

			pdfViewer.PrintDocumentWithSettings(printerSettings.ToPrinterSettings());
		}

		public static void Print(this global::Aspose.Pdf.Facades.Form form, System.Drawing.Printing.PageSettings pageSettings, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			using (var documentStream = new ISI.Extensions.Stream.TempFileStream())
			{
				form.Save(documentStream);

				documentStream.Rewind();

				var pdfViewer = new global::Aspose.Pdf.Facades.PdfViewer();

				pdfViewer.BindPdf(documentStream);

				pdfViewer.PrintDocumentWithSettings(pageSettings.ToPageSettings(printerSettings), printerSettings.ToPrinterSettings());
			}
		}

		public static void Print(this global::Aspose.Pdf.Document document, System.Drawing.Printing.PageSettings pageSettings, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			var pdfViewer = new global::Aspose.Pdf.Facades.PdfViewer(document);

			pdfViewer.PrintDocumentWithSettings(pageSettings.ToPageSettings(printerSettings), printerSettings.ToPrinterSettings());
		}
	}
}
