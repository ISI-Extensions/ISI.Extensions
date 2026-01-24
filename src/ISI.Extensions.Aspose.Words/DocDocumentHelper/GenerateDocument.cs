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
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public partial class DocDocumentHelper
		{
			public virtual void GenerateDocument(System.IO.Stream templateStream, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, Action<ISI.Extensions.Documents.IDocumentEditor> beforeGeneration, Action<ISI.Extensions.Documents.IDocumentEditor> afterGeneration, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateStream);

				templateDocument.WarningCallback = WarningCallback;

				BeforeGeneration(templateDocument, beforeGeneration);

				templateDocument.FirstSection.PageSetup.PageStartingNumber = 1;
				templateDocument.FirstSection.PageSetup.RestartPageNumbering = true;

				templateDocument.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				templateDocument.MailMerge.CleanupOptions |= global::Aspose.Words.MailMerging.MailMergeCleanupOptions.RemoveUnusedFields;

				templateDocument.MailMerge.Execute(dataReader);

				templateDocument.UpdateFields();

				AfterGeneration(templateDocument, documentProperties, afterGeneration, printerName, documentStream, fileFormat);
			}

			public virtual void GenerateDocument(System.IO.Stream templateStream, ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot, ISI.Extensions.Documents.IDocumentProperties documentProperties, Action<ISI.Extensions.Documents.IDocumentEditor> beforeGeneration, Action<ISI.Extensions.Documents.IDocumentEditor> afterGeneration, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateStream);
				
				templateDocument.WarningCallback = WarningCallback;

				BeforeGeneration(templateDocument, beforeGeneration);

				templateDocument.FirstSection.PageSetup.PageStartingNumber = 1;
				templateDocument.FirstSection.PageSetup.RestartPageNumbering = true;

				templateDocument.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				templateDocument.MailMerge.UseNonMergeFields = true;

				templateDocument.MailMerge.CleanupOptions |= global::Aspose.Words.MailMerging.MailMergeCleanupOptions.RemoveUnusedFields;

				templateDocument.MailMerge.ExecuteWithRegions(new MailMergeDataSourceRoot(documentDataSourceRoot));

				templateDocument.UpdateFields();

				AfterGeneration(templateDocument, documentProperties, afterGeneration, printerName, documentStream, fileFormat);
			}

			public virtual void GenerateDocument(System.IO.Stream templateStream, ISI.Extensions.Documents.IDocumentDataSource documentDataSource, ISI.Extensions.Documents.IDocumentProperties documentProperties, Action<ISI.Extensions.Documents.IDocumentEditor> beforeGeneration, Action<ISI.Extensions.Documents.IDocumentEditor> afterGeneration, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateStream);
								
				templateDocument.WarningCallback = WarningCallback;

				BeforeGeneration(templateDocument, beforeGeneration);

				templateDocument.FirstSection.PageSetup.PageStartingNumber = 1;
				templateDocument.FirstSection.PageSetup.RestartPageNumbering = true;

				templateDocument.MailMerge.FieldMergingCallback = new ISI.Extensions.Aspose.Words.MailMergeFieldHandler();

				templateDocument.MailMerge.UseNonMergeFields = true;

				templateDocument.MailMerge.CleanupOptions |= global::Aspose.Words.MailMerging.MailMergeCleanupOptions.RemoveUnusedFields;

				templateDocument.MailMerge.Execute(new DocumentDataSource(documentDataSource));

				templateDocument.UpdateFields();

				AfterGeneration(templateDocument, documentProperties, afterGeneration, printerName, documentStream, fileFormat);
			}

			private void BeforeGeneration(global::Aspose.Words.Document document, Action<ISI.Extensions.Documents.IDocumentEditor> beforeGeneration)
			{
				if (beforeGeneration != null)
				{
					var documentEditor = new DocumentEditor(document);

					beforeGeneration(documentEditor);
				}
			}

			private void AfterGeneration(global::Aspose.Words.Document document, ISI.Extensions.Documents.IDocumentProperties documentProperties, Action<ISI.Extensions.Documents.IDocumentEditor> afterGeneration, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				document.SetDocumentProperties(documentProperties);

				if (afterGeneration != null)
				{
					var documentEditor = new DocumentEditor(document);

					afterGeneration(documentEditor);
				}

				ISI.Extensions.Aspose.Extensions.WordsExtensions.Print(document, printerName);
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Save(document, documentStream, fileFormat);
			}
		}
	}
}