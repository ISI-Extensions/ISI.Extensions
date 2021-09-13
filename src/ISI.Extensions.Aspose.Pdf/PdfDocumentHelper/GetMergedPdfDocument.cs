#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Aspose
{
	public partial class Pdf
	{
		public partial class PdfDocumentHelper
		{
			public ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName)
			{
				return GetMergedPdfDocument(documents, fileName, (Func<System.IO.Stream>)null);
			}

			public ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName, System.IO.Stream outputStream)
			{
				return GetMergedPdfDocument(documents, fileName, () => outputStream ??= new ISI.Extensions.Stream.TempFileStream());
			}

			public ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName, Func<System.IO.Stream> getOutputStream)
			{
				var pdfEditor = new global::Aspose.Pdf.Facades.PdfFileEditor();

				var outputStream = getOutputStream?.Invoke() ?? new ISI.Extensions.Stream.TempFileStream();

				var documentsToMerge = new List<ISI.Extensions.Documents.IDocument>();
				foreach (var document in documents)
				{
					var fileFormat = ISI.Extensions.Extensions.DocumentExtensions.ToFileFormat(document.FileName);

					switch (fileFormat)
					{
						//case ISI.Extensions.Documents.FileFormat.Doc:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Docx:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Rtf:
						//	break;
						case ISI.Extensions.Documents.FileFormat.Pdf:
							documentsToMerge.Add(document);
							break;
						//case ISI.Extensions.Documents.FileFormat.Xps:
						//	break;
						case ISI.Extensions.Documents.FileFormat.Tiff:
						case ISI.Extensions.Documents.FileFormat.Png:
						case ISI.Extensions.Documents.FileFormat.Bmp:
						case ISI.Extensions.Documents.FileFormat.Jpeg:
							{
								var pdfDocument = new ISI.Extensions.Documents.Document()
								{
									FileName = System.IO.Path.Combine(System.IO.Path.GetFullPath(document.FileName), string.Format("{0}.pdf", System.IO.Path.GetFileNameWithoutExtension(document.FileName))),
									Stream = new ISI.Extensions.Stream.TempFileStream(),
								};

								ConvertImageToPdf(document.Stream, pdfDocument.Stream);

								documentsToMerge.Add(pdfDocument);
								break;
							}
						//case ISI.Extensions.Documents.FileFormat.Xls:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Xlsx:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Pptx:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Ppt:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Ppsx:
						//	break;
						//case ISI.Extensions.Documents.FileFormat.Pps:
						//	break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				pdfEditor.Concatenate(documentsToMerge.Select(document => document.Stream).ToArray(), outputStream);

				return new ISI.Extensions.Documents.Document()
				{
					FileName = fileName,
					Stream = outputStream
				};
			}
		}
	}
}