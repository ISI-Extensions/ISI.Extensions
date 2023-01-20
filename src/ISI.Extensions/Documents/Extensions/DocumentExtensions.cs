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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Extensions
{
	public static class DocumentExtensions
	{
		private static readonly IDictionary<ISI.Extensions.Documents.FileFormat, string> _toFileNameExtension = null;
		private static readonly IDictionary<string, ISI.Extensions.Documents.FileFormat> _toFileFormat = null;

		static DocumentExtensions()
		{
			_toFileNameExtension = new Dictionary<ISI.Extensions.Documents.FileFormat, string>()
			{
				{ISI.Extensions.Documents.FileFormat.Default, "docx"},
				{ISI.Extensions.Documents.FileFormat.Doc, "doc"},
				{ISI.Extensions.Documents.FileFormat.Dot, "dot"},
				{ISI.Extensions.Documents.FileFormat.Docx, "docx"},
				{ISI.Extensions.Documents.FileFormat.Docm, "docm"},
				{ISI.Extensions.Documents.FileFormat.Dotx, "dotx"},
				{ISI.Extensions.Documents.FileFormat.Dotm, "dotm"},
				{ISI.Extensions.Documents.FileFormat.Rtf, "rtf"},

				{ISI.Extensions.Documents.FileFormat.Pdf, "pdf"},

				{ISI.Extensions.Documents.FileFormat.Xps, "xps"},
				{ISI.Extensions.Documents.FileFormat.Html, "html"},

				{ISI.Extensions.Documents.FileFormat.Xls, "xls"},
				{ISI.Extensions.Documents.FileFormat.Xlsx, "xlsx"},

				{ISI.Extensions.Documents.FileFormat.Tiff, "tiff"},
				{ISI.Extensions.Documents.FileFormat.Png, "png"},
				{ISI.Extensions.Documents.FileFormat.Bmp, "bmp"},
				{ISI.Extensions.Documents.FileFormat.Jpeg, "jpg"},

				{ISI.Extensions.Documents.FileFormat.Pptx, "pptx"},
				{ISI.Extensions.Documents.FileFormat.Ppt, "ppt"},
				{ISI.Extensions.Documents.FileFormat.Ppsx, "ppsx"},
				{ISI.Extensions.Documents.FileFormat.Pps, "pps"},

				{ISI.Extensions.Documents.FileFormat.CommaSeparatedValues, "csv"},
				{ISI.Extensions.Documents.FileFormat.TabDelimited, "tab"},
				{ISI.Extensions.Documents.FileFormat.PipeDelimited, "pipe"},
			};

			_toFileFormat = _toFileNameExtension.Where(_ => _.Key != ISI.Extensions.Documents.FileFormat.Default).ToDictionary(_ => _.Value, _ => _.Key, StringComparer.InvariantCultureIgnoreCase);
		}

		private static ISI.Extensions.Documents.Pdf.IPdfDocumentHelper _pdfDocumentHelper = null;
		internal static ISI.Extensions.Documents.Pdf.IPdfDocumentHelper PdfDocumentHelper => _pdfDocumentHelper ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Documents.Pdf.IPdfDocumentHelper>();

		private static ISI.Extensions.Documents.Doc.IDocDocumentHelper _docDocumentHelper = null;
		internal static ISI.Extensions.Documents.Doc.IDocDocumentHelper DocDocumentHelper => _docDocumentHelper ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Documents.Doc.IDocDocumentHelper>();

		private static ISI.Extensions.Documents.SpreadSheet.ISpreadSheetDocumentHelper _spreadSheetHelper = null;
		internal static ISI.Extensions.Documents.SpreadSheet.ISpreadSheetDocumentHelper SpreadSheetHelper => _spreadSheetHelper ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Documents.SpreadSheet.ISpreadSheetDocumentHelper>();

		public static ISI.Extensions.Documents.IDocument GetMergedPdfDocument(this IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName, System.IO.Stream outputStream)
		{
			return PdfDocumentHelper.GetMergedPdfDocument(documents, fileName, outputStream);
		}

		public static int GetPdfDocumentPageCount(this ISI.Extensions.Documents.IDocument document)
		{
			return PdfDocumentHelper.GetPdfDocumentPageCount(document);
		}

		public static void Print(this ISI.Extensions.Documents.IDocument document, string printerName)
		{
			if (PdfDocumentHelper.IsHelperFor(document))
			{
				PdfDocumentHelper.Print(document, printerName);
			}
			else if (DocDocumentHelper.IsHelperFor(document))
			{
				DocDocumentHelper.Print(document, printerName);
			}
			else if (SpreadSheetHelper.IsHelperFor(document))
			{
				SpreadSheetHelper.Print(document, printerName);
			}
		}

		public static ISI.Extensions.Documents.FileFormat ToFileFormat(string fileNameExtension, ISI.Extensions.Documents.FileFormat defaultFileFormat = ISI.Extensions.Documents.FileFormat.Default)
		{
			fileNameExtension = System.IO.Path.GetExtension(fileNameExtension);

			if (_toFileFormat.TryGetValue(fileNameExtension, out var fileFormat))
			{
				return fileFormat;
			}

			return defaultFileFormat;
		}

		public static string ToFileNameExtension(this ISI.Extensions.Documents.FileFormat fileFormat)
		{
			return _toFileNameExtension[fileFormat];
		}

		public static string ToFileName(this ISI.Extensions.Documents.FileFormat fileFormat, string fileName)
		{
			return string.Format("{0}.{1}", fileName, fileFormat.ToFileNameExtension());
		}
	}
}
