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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Pdf
	{
		public partial class PdfDocumentHelper
		{
			public virtual void GenerateDocument(System.IO.Stream templateStream, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var documentStreams = new List<System.IO.Stream>();

				templateStream = new ISI.Extensions.Stream.TempFileStream(templateStream);

				var obfuscateFieldNames = ((documentProperties as ISI.Extensions.Documents.IPdfDocumentProperties)?.ObfuscateFieldNames) ?? false;

				while (dataReader.Read())
				{
					var outputStream = new ISI.Extensions.Stream.TempFileStream();

					templateStream.Rewind();

					var document = new global::Aspose.Pdf.Facades.Form(templateStream);

					for (var i = 0; i < dataReader.FieldCount; i++)
					{
						var fieldName = dataReader.GetName(i);

						if (obfuscateFieldNames)
						{
							fieldName = Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting);
							document.RenameField(dataReader.GetName(i), fieldName);
						}

						document.FillField(fieldName, dataReader.GetValue(i).ToString());
					}

					document.FlattenAllFields();
					document.Save(outputStream);

					outputStream.Rewind();

					documentStreams.Add(outputStream);
				}

				var documentStreamsCount = documentStreams.Count;
				if (documentStreamsCount == 1)
				{
					var document = new global::Aspose.Pdf.Document(documentStreams.FirstOrDefault());

					ISI.Extensions.Aspose.Extensions.PdfExtensions.Print(document, printerName);
					ISI.Extensions.Aspose.Extensions.PdfExtensions.Save(document, documentStream, fileFormat);
				}
				else if (documentStreamsCount > 1)
				{
					using (var outputStream = new ISI.Extensions.Stream.TempFileStream())
					{
						var pdfEditor = new global::Aspose.Pdf.Facades.PdfFileEditor();

						pdfEditor.Concatenate(documentStreams.ToArray(), outputStream);

						var document = new global::Aspose.Pdf.Document(outputStream);

						ISI.Extensions.Aspose.Extensions.PdfExtensions.Print(document, printerName);
						ISI.Extensions.Aspose.Extensions.PdfExtensions.Save(document, documentStream, fileFormat);
					}
				}

				foreach (var stream in documentStreams)
				{
					stream?.Dispose();
				}

				templateStream.Dispose();
			}
		}
	}
}