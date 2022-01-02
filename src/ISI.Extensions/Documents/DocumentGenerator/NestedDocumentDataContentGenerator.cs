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
	public abstract class NestedDocumentDataContentGenerator<TModel> : ContentGenerator<TModel>
		where TModel : class, IModel
	{
		protected NestedDocumentDataContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger)
			: base(logger)
		{
		}
		protected NestedDocumentDataContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger, 
			ISI.Extensions.TemplateProviders.ITemplateProvider templateProvider)
			: base(logger, templateProvider)
		{
		}
		protected NestedDocumentDataContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger,
			Func<ISI.Extensions.TemplateProviders.ITemplateProvider> getTemplateProvider)
			: base(logger, getTemplateProvider)
		{
		}

		protected virtual ISI.Extensions.Documents.IDocumentDataSourceRoot GetDocumentDataSourceRoot(IEnumerable<TModel> models)
		{
			return new ISI.Extensions.Documents.DocumentDataSourceRoot<TModel>(models);
		}
		protected virtual ISI.Extensions.Documents.IDocumentDataSourceRoot GetDocumentDataSourceRoot(TModel model)
		{
			return new ISI.Extensions.Documents.DocumentDataSourceRoot<TModel>(model);
		}

		public override void GenerateDocument(System.IO.Stream templateStream, IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			GenerateDocument(templateStream, GetDocumentDataSourceRoot(models), documentProperties, printerName, documentStream, fileFormat);
		}

		public override void GenerateDocument(System.IO.Stream templateStream, TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			GenerateDocument(templateStream, GetDocumentDataSourceRoot(model), documentProperties, printerName, documentStream, fileFormat);
		}

		public abstract void GenerateDocument(System.IO.Stream templateStream, ISI.Extensions.Documents.IDocumentDataSourceRoot documentDataSourceRoot, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat);
	}
}
