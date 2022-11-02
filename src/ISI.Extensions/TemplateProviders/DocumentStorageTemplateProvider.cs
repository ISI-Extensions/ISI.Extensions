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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Documents.DocumentGenerator;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.TemplateProviders
{
	[ISI.Extensions.TemplateProviders.TemplateProvider]
	public class DocumentStorageTemplateProvider : ISI.Extensions.TemplateProviders.ITemplateProvider, ISI.Extensions.Emails.EmailGenerator.ITemplateProvider
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.IDocumentStorage DocumentStorage { get; }

		public DocumentStorageTemplateProvider(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.IDocumentStorage documentStorage)
		{
			Logger = logger;
			DocumentStorage = documentStorage;
		}

		public bool IsTemplateProviderFor(object contentGenerator)
		{
			return (contentGenerator is ISI.Extensions.TemplateProviders.IUsesDocumentStorageTemplateProvider) ||
			       (contentGenerator is ISI.Extensions.TemplateProviders.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid) ||
			       (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProvider) ||
			       (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid);
		}

		public string GetTemplateCacheKey(object contentGenerator, string templateKey, object model)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			{
				if (contentGenerator is ISI.Extensions.TemplateProviders.IUseGetTemplateCacheKey templateCacheKeyGenerator)
				{
					return templateCacheKeyGenerator.GetTemplateCacheKey(templateKey, model);
				}
			}

			return templateKey;
		}

		public System.IO.Stream GetTemplateStream(string templateCacheKey)
		{
			var templateDocumentUuid = templateCacheKey.ToGuid();

			var document = DocumentStorage.GetDocumentStreamAsync(templateDocumentUuid).GetAwaiter().GetResult();

			if (document == null)
			{
				throw new ISI.Extensions.DocumentStorageException(templateDocumentUuid);
			}

			return document.Stream;
		}
		
		string ISI.Extensions.TemplateProviders.ITemplateProvider.GetTemplateKey(object contentGenerator, object model)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateDocumentUuid = Guid.Empty;

			{
				if (contentGenerator is ISI.Extensions.TemplateProviders.IUsesDocumentStorageTemplateProvider templateDocumentUuidProvider)
				{

					templateDocumentUuid = templateDocumentUuidProvider.TemplateDocumentUuid;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.TemplateProviders.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid templateDocumentUuidProvider)
				{
					templateDocumentUuid = templateDocumentUuidProvider.GetTemplateDocumentUuid(model);
				}
			}

			if (templateDocumentUuid == Guid.Empty)
			{
				throw new("No TemplateDocumentUuid Provided");
			}

			return templateDocumentUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}
		
		string ISI.Extensions.Emails.EmailGenerator.ITemplateProvider.GetSubjectTemplateKey(object contentGenerator, object model)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateDocumentUuid = Guid.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProvider templateDocumentUuidProvider)
				{

					templateDocumentUuid = templateDocumentUuidProvider.SubjectTemplateDocumentUuid;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid templateDocumentUuidProvider)
				{
					templateDocumentUuid = templateDocumentUuidProvider.GetSubjectTemplateDocumentUuid(model);
				}
			}

			return templateDocumentUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}
		
		string ISI.Extensions.Emails.EmailGenerator.ITemplateProvider.GetPlainTextTemplateKey(object contentGenerator, object model)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateDocumentUuid = Guid.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProvider templateDocumentUuidProvider)
				{

					templateDocumentUuid = templateDocumentUuidProvider.PlainTextTemplateDocumentUuid;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid templateDocumentUuidProvider)
				{
					templateDocumentUuid = templateDocumentUuidProvider.GetPlainTextTemplateDocumentUuid(model);
				}
			}

			return templateDocumentUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}

		string ISI.Extensions.Emails.EmailGenerator.ITemplateProvider.GetMhtmlTemplateKey(object contentGenerator, object model)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateDocumentUuid = Guid.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProvider templateDocumentUuidProvider)
				{

					templateDocumentUuid = templateDocumentUuidProvider.MhtmlTemplateDocumentUuid;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailGenerator.IUsesDocumentStorageTemplateProviderWithGetTemplateDocumentUuid templateDocumentUuidProvider)
				{
					templateDocumentUuid = templateDocumentUuidProvider.GetMhtmlTemplateDocumentUuid(model);
				}
			}

			return templateDocumentUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}
	}
}