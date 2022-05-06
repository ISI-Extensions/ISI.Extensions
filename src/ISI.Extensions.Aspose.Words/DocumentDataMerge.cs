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
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentDataMerge : ISI.Extensions.Documents.IDocumentDataMerge
		{
			static DocumentDataMerge()
			{
				(new ISI.Extensions.Aspose.Words.LicenseApplier()).ApplyLicense();
			}

			public void Execute<TModel>(string templateFileName, IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
				where TModel : class
			{
				var dataReader = Activator.CreateInstance(typeof(ISI.Extensions.DataReader.EnumerableDataReader<>).MakeGenericType(typeof(TModel)), models) as System.Data.IDataReader;

				Execute(templateFileName, dataReader, documentProperties, printerName, documentStream, fileFormat);
			}

			public void Execute<T>(System.IO.Stream templateStream, IEnumerable<T> data, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
				where T : class
			{
				var dataReader = Activator.CreateInstance(typeof(ISI.Extensions.DataReader.EnumerableDataReader<>).MakeGenericType(typeof(T)), new[] { data }) as System.Data.IDataReader;

				Execute(templateStream, dataReader, documentProperties, printerName, documentStream, fileFormat);
			}

			public void Execute(string templateFileName, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateFileName);

				Execute(templateDocument, dataReader, documentProperties, printerName, documentStream, fileFormat);
			}

			public void Execute(System.IO.Stream templateStream, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateStream);

				Execute(templateDocument, dataReader, documentProperties, printerName, documentStream, fileFormat);
			}

			public void Execute(global::Aspose.Words.Document templateDocument, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				templateDocument.MailMerge.FieldMergingCallback = new MailMergeFieldHandler();

				templateDocument.MailMerge.Execute(dataReader);

				templateDocument.SetDocumentProperties(documentProperties);

				ISI.Extensions.Aspose.Extensions.WordsExtensions.Print(templateDocument, printerName);
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Save(templateDocument, documentStream, fileFormat);
			}







			public void ExecuteWithRegions(string templateFileName, ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateFileName);

				ExecuteWithRegions(templateDocument, documentDataSourceRoot, documentProperties, printerName, documentStream, fileFormat);
			}

			public void ExecuteWithRegions(System.IO.Stream templateStream, ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				var templateDocument = new global::Aspose.Words.Document(templateStream);

				ExecuteWithRegions(templateDocument, documentDataSourceRoot, documentProperties, printerName, documentStream, fileFormat);
			}

			public void ExecuteWithRegions(global::Aspose.Words.Document templateDocument, ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
			{
				templateDocument.MailMerge.FieldMergingCallback = new MailMergeFieldHandler();

				templateDocument.MailMerge.ExecuteWithRegions(new MailMergeDataSourceRoot(documentDataSourceRoot));

				templateDocument.SetDocumentProperties(documentProperties);

				ISI.Extensions.Aspose.Extensions.WordsExtensions.Print(templateDocument, printerName);
				ISI.Extensions.Aspose.Extensions.WordsExtensions.Save(templateDocument, documentStream, fileFormat);
			}
		}
	}
}