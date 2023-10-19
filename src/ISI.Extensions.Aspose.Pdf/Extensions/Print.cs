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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class PdfExtensions
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

		private static global::Aspose.Pdf.Printing.PrintRange ToPrintRange(this System.Drawing.Printing.PrintRange printRange)
		{
			switch (printRange)
			{
				case System.Drawing.Printing.PrintRange.AllPages:
					return global::Aspose.Pdf.Printing.PrintRange.AllPages;
				case System.Drawing.Printing.PrintRange.Selection:
					return global::Aspose.Pdf.Printing.PrintRange.Selection;
				case System.Drawing.Printing.PrintRange.SomePages:
					return global::Aspose.Pdf.Printing.PrintRange.SomePages;
				case System.Drawing.Printing.PrintRange.CurrentPage:
					return global::Aspose.Pdf.Printing.PrintRange.CurrentPage;
				default:
					throw new ArgumentOutOfRangeException(nameof(printRange), printRange, null);
			}
		}

		private static global::Aspose.Pdf.Printing.Duplex ToDuplex(this System.Drawing.Printing.Duplex duplex)
		{
			switch (duplex)
			{
				case System.Drawing.Printing.Duplex.Default:
					return global::Aspose.Pdf.Printing.Duplex.Default;
				case System.Drawing.Printing.Duplex.Simplex:
					return global::Aspose.Pdf.Printing.Duplex.Simplex;
				case System.Drawing.Printing.Duplex.Vertical:
					return global::Aspose.Pdf.Printing.Duplex.Vertical;
				case System.Drawing.Printing.Duplex.Horizontal:
					return global::Aspose.Pdf.Printing.Duplex.Horizontal;
				default:
					throw new ArgumentOutOfRangeException(nameof(duplex), duplex, null);
			}
		}

		private static global::Aspose.Pdf.Printing.PrinterSettings ToPrinterSettings(this System.Drawing.Printing.PrinterSettings printerSettings)
		{
			if (printerSettings == null)
			{
				return null;
			}

			return new global::Aspose.Pdf.Printing.PrinterSettings()
			{
				Copies = printerSettings.Copies,
				Collate = printerSettings.Collate,
				Duplex = printerSettings.Duplex.ToDuplex(),
				FromPage = printerSettings.FromPage,
				MaximumPage = printerSettings.MaximumPage,
				MinimumPage = printerSettings.MinimumPage,
				PrintFileName = printerSettings.PrintFileName,
				PrintRange = printerSettings.PrintRange.ToPrintRange(),
				PrintToFile = printerSettings.PrintToFile,
				PrinterName = printerSettings.PrinterName,
				//PrinterUri = printerSettings.PrinterUri,
				ToPage = printerSettings.ToPage,
			};
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

		private static global::Aspose.Pdf.Printing.PrinterResolutionKind ToPrinterResolutionKind(this System.Drawing.Printing.PrinterResolutionKind printerResolutionKind)
		{
			switch (printerResolutionKind)
			{
				case System.Drawing.Printing.PrinterResolutionKind.High:
					return global::Aspose.Pdf.Printing.PrinterResolutionKind.High;
				case System.Drawing.Printing.PrinterResolutionKind.Medium:
					return global::Aspose.Pdf.Printing.PrinterResolutionKind.Medium;
				case System.Drawing.Printing.PrinterResolutionKind.Low:
					return global::Aspose.Pdf.Printing.PrinterResolutionKind.Low;
				case System.Drawing.Printing.PrinterResolutionKind.Draft:
					return global::Aspose.Pdf.Printing.PrinterResolutionKind.Draft;
				case System.Drawing.Printing.PrinterResolutionKind.Custom:
					return global::Aspose.Pdf.Printing.PrinterResolutionKind.Custom;
				default:
					throw new ArgumentOutOfRangeException(nameof(printerResolutionKind), printerResolutionKind, null);
			}
		}

		private static global::Aspose.Pdf.Printing.PageSettings ToPageSettings(this System.Drawing.Printing.PageSettings pageSettings, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			if (pageSettings == null)
			{
				return null;
			}

			return new global::Aspose.Pdf.Printing.PageSettings(printerSettings.ToPrinterSettings())
			{
				Color = pageSettings.Color,
				Landscape = pageSettings.Landscape,
				Margins = pageSettings.Margins.NullCheckedConvert(margins => new global::Aspose.Pdf.Devices.Margins(margins.Left, margins.Right, margins.Top, margins.Bottom)),
				PaperSize = pageSettings.PaperSize.NullCheckedConvert(paperSize => new global::Aspose.Pdf.Printing.PaperSize(paperSize.PaperName, paperSize.Width, paperSize.Height)),
				PaperSource = pageSettings.PaperSource.NullCheckedConvert(paperSource => new global::Aspose.Pdf.Printing.PaperSource()
				{
					SourceName = paperSource.SourceName,
				}),
				PrinterResolution = pageSettings.PrinterResolution.NullCheckedConvert(printerResolution => new global::Aspose.Pdf.Printing.PrinterResolution()
				{
					Kind = printerResolution.Kind.ToPrinterResolutionKind(),
					X = printerResolution.X,
					Y = printerResolution.Y,
				}),
			};
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
