#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Documents.Pdf
{
	public interface IPdfDocumentHelper : ISI.Extensions.Documents.IDocumentHelper
	{
		System.IO.Stream[] ConvertToImage(System.IO.Stream pdfStream, ISI.Extensions.Documents.ImageType imageType, Func<System.IO.Stream> getOutputStream = null);
		ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName);
		ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName, System.IO.Stream outputStream);
		ISI.Extensions.Documents.IDocument GetMergedPdfDocument(IEnumerable<ISI.Extensions.Documents.IDocument> documents, string fileName, Func<System.IO.Stream> getOutputStream);
		int GetPdfDocumentPageCount(ISI.Extensions.Documents.IDocument document);
		void GenerateDocument(System.IO.Stream templateStream, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat);
		void ConvertImageToPdf(System.IO.Stream imageStream, System.IO.Stream pdfStream);
		System.IO.Stream ConvertColorSpaceToGreyScale(ISI.Extensions.Documents.IDocument document, Func<System.IO.Stream> getOutputStream = null);
	}
}
