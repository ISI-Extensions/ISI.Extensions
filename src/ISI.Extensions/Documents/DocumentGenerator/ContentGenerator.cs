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
using System.IO;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Documents.DocumentGenerator
{
	public abstract class ContentGenerator<TModel> : IContentGenerator<TModel>
		where TModel : class, IModel
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		private Func<ISI.Extensions.TemplateProviders.ITemplateProvider> _getTemplateProvider = null;
		private ISI.Extensions.TemplateProviders.ITemplateProvider _templateProvider = null;
		protected ISI.Extensions.TemplateProviders.ITemplateProvider TemplateProvider => (_templateProvider ??= _getTemplateProvider?.Invoke() ?? ISI.Extensions.TemplateProviders.TemplateProviderFactory.GetTemplateProvider<ISI.Extensions.TemplateProviders.ITemplateProvider>(this, false));

		public virtual Type ModelType => typeof(TModel);

		protected ContentGenerator(Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}
		protected ContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger, 
			ISI.Extensions.TemplateProviders.ITemplateProvider templateProvider)
			: this(logger)
		{
			_templateProvider = templateProvider;
		}
		protected ContentGenerator(
			Microsoft.Extensions.Logging.ILogger logger, 
			Func<ISI.Extensions.TemplateProviders.ITemplateProvider> getTemplateProvider)
			: this(logger)
		{
			_getTemplateProvider = getTemplateProvider;
		}

		protected virtual System.IO.Stream GetTemplateStream(string templateCacheKey)
		{
			return TemplateProvider?.GetTemplateStream(templateCacheKey);
		}

		protected virtual string GetTemplateCacheKey(string templateKey, IModel model)
		{
			return TemplateProvider?.GetTemplateCacheKey(this, templateKey, model);
		}

		protected virtual string GetTemplateKey(IModel model)
		{
			return TemplateProvider?.GetTemplateKey(this, model);
		}

		public virtual void GenerateDocument(TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			var templateKey = GetTemplateKey(model);

			var templateCacheKey = GetTemplateCacheKey(templateKey, model);

			using (var templateStream = GetTemplateStream(templateCacheKey))
			{
				GenerateDocument(templateStream, model, documentProperties, printerName, documentStream, fileFormat);
			}
		}

		public virtual void GenerateDocument(IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			var templateKey = GetTemplateKey(null);

			var templateCacheKey = GetTemplateCacheKey(templateKey, null);

			using (var templateStream = GetTemplateStream(templateCacheKey))
			{
				GenerateDocument(templateStream, models, documentProperties, printerName, documentStream, fileFormat);
			}
		}

		public virtual void GenerateDocument(System.IO.Stream templateStream, TModel model, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			GenerateDocument(templateStream, new[] { model }, documentProperties, printerName, documentStream, fileFormat);
		}

		public virtual void GenerateDocument(System.IO.Stream templateStream, IEnumerable<TModel> models, ISI.Extensions.Documents.IDocumentProperties documentProperties, string printerName, System.IO.Stream documentStream, ISI.Extensions.Documents.FileFormat fileFormat)
		{
			GenerateDocument(templateStream, models.FirstOrDefault(), documentProperties, printerName, documentStream, fileFormat);
		}
	}
}
