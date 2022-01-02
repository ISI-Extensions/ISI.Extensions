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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Documents.DocumentGenerator
{
	public abstract class DataReaderContentGenerator<TModel> : ContentGenerator<TModel>
		where TModel : class, IModel
	{
		protected DataReaderContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger)
			: base(logger)
		{
		}
		protected DataReaderContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger, 
			ISI.Extensions.TemplateProviders.ITemplateProvider templateProvider)
			: base(logger, templateProvider)
		{
		}
		protected DataReaderContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger,
			Func<ISI.Extensions.TemplateProviders.ITemplateProvider> getTemplateProvider)
			: base(logger, getTemplateProvider)
		{
		}

		//public virtual System.Data.IDataReader GetDataReader(IEnumerable<TModel> model, Type mergeModelType)
		//{
		//	return Activator.CreateInstance(typeof(ISI.Extensions.DataReaderV2.EnumerableDataReader<>).MakeGenericType(mergeModelType), model, null) as System.Data.IDataReader;
		//}

		public virtual System.Data.IDataReader GetDataReader<TMergeModel>(IEnumerable<TModel> models, Func<TModel, TMergeModel> converter)
			where TMergeModel : class
		{
			return new ISI.Extensions.DataReader.EnumerableDataReader<TMergeModel>(models.Select(converter));
		}

		public virtual System.Data.IDataReader GetDataReader<TMergeModel>(TModel model, Func<TModel, TMergeModel> converter)
			where TMergeModel : class
		{
			return GetDataReader<TMergeModel>(new[] { model }, converter);
		}

		public virtual System.Data.IDataReader GetDataReader(IEnumerable<TModel> models)
		{
			return new ISI.Extensions.DataReader.EnumerableDataReader<TModel>(models);
		}

		public virtual System.Data.IDataReader GetDataReader(TModel model)
		{
			return GetDataReader(new[] { model });
		}

		//public virtual void GenerateDocument<TMailMergeModel>(System.IO.Stream templateStream, IEnumerable<TModel> models, Func<TModel, TMailMergeModel> converter, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		//	where TMailMergeModel : class
		//{
		//	var dataReader = GetDataReader(models);

		//	GenerateDocument(templateStream, dataReader, documentProperties, printerName, documentStream, fileFormat);
		//}

		public override void GenerateDocument(System.IO.Stream templateStream, IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			var dataReader = GetDataReader(models);

			GenerateDocument(templateStream, dataReader, documentProperties, printerName, documentStream, fileFormat);
		}

		public override void GenerateDocument(System.IO.Stream templateStream, TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			var dataReader = GetDataReader(model);

			GenerateDocument(templateStream, dataReader, documentProperties, printerName, documentStream, fileFormat);
		}

		protected abstract void GenerateDocument(System.IO.Stream templateStream, System.Data.IDataReader dataReader, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat);
	}
}
