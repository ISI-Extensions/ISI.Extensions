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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.TemplateProviders
{
	[ISI.Extensions.TemplateProviders.TemplateProvider]
	public class FileTemplateProvider : ISI.Extensions.TemplateProviders.ITemplateProvider, ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected Microsoft.Extensions.FileProviders.IFileProvider FileProvider { get; }

		public FileTemplateProvider(
			Microsoft.Extensions.Logging.ILogger logger,
			Microsoft.Extensions.FileProviders.IFileProvider fileProvider)
		{
			Logger = logger;
			FileProvider = fileProvider;
		}

		public bool IsTemplateProviderFor(object contentGenerator)
		{
			return (contentGenerator is ISI.Extensions.TemplateProviders.IUsesFileTemplateProvider) ||
						 (contentGenerator is ISI.Extensions.TemplateProviders.IUsesFileTemplateProviderWithGetTemplateFileName) ||
						 (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileEmailMessageTemplateProvider) ||
						 (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileTemplateProviderWithGetTemplateFileName);
		}

		private string GenerateTemplateCacheKey(string templateKey, object model)
		{
			var potentialTemplateCacheKeys = new Stack<string>();

			var fileExtension = templateKey.Substring(templateKey.LastIndexOf(".") + 1);
			var fileNameWithoutExtension = templateKey.Substring(0, templateKey.LastIndexOf("."));
			var cultureKey = (model as ISI.Extensions.Culture.IHasCultureKey)?.CultureKey ?? string.Empty;
			var templateGroupKey = (model as ISI.Extensions.TemplateProviders.IModelWithTemplateGroupKey)?.TemplateGroupKey ?? string.Empty;

			if (!string.IsNullOrEmpty(cultureKey))
			{
				potentialTemplateCacheKeys.Push(string.Format("{0}-{2}.{1}", fileNameWithoutExtension, fileExtension, cultureKey));
			}
			if (!string.IsNullOrEmpty(templateGroupKey))
			{
				potentialTemplateCacheKeys.Push(string.Format("{0}-{2}.{1}", fileNameWithoutExtension, fileExtension, templateGroupKey));

				if (!string.IsNullOrEmpty(cultureKey))
				{
					potentialTemplateCacheKeys.Push(string.Format("{0}-{2}-{3}.{1}", fileNameWithoutExtension, fileExtension, templateGroupKey, cultureKey));
				}
			}

			var templateCacheKey = string.Empty;
			while (string.IsNullOrWhiteSpace(templateCacheKey) && potentialTemplateCacheKeys.Any())
			{
				templateCacheKey = potentialTemplateCacheKeys.Pop();

				if (!FileProvider.GetFileInfo(templateCacheKey).Exists)
				{
					templateCacheKey = null;
				}
			}

			if (string.IsNullOrWhiteSpace(templateCacheKey))
			{
				templateCacheKey = templateKey;
			}

			return templateCacheKey;
		}

		public async Task<string> GetTemplateCacheKeyAsync(object contentGenerator, string templateKey, object model, System.Threading.CancellationToken cancellationToken = default)
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

			return GenerateTemplateCacheKey(templateKey, model);
		}

		public async Task<System.IO.Stream> GetTemplateStreamAsync(string templateCacheKey, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!FileProvider.GetFileInfo(templateCacheKey).Exists)
			{
				throw new System.IO.FileNotFoundException(templateCacheKey);
			}

			return FileProvider.GetFileInfo(templateCacheKey).CreateReadStream();
		}

		async Task<string> ISI.Extensions.TemplateProviders.ITemplateProvider.GetTemplateKeyAsync(object contentGenerator, object model, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateFileName = string.Empty;

			{
				if (contentGenerator is ISI.Extensions.TemplateProviders.IUsesFileTemplateProvider templateFileNameProvider)
				{

					templateFileName = templateFileNameProvider.TemplateFileName;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.TemplateProviders.IUsesFileTemplateProviderWithGetTemplateFileName templateFileNameProvider)
				{
					templateFileName = templateFileNameProvider.GetTemplateFileName(model);
				}
			}

			if (string.IsNullOrWhiteSpace(templateFileName))
			{
				throw new("No TemplateFileName Provided");
			}

			return templateFileName;
		}

		async Task<string> ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider.GetSubjectTemplateKeyAsync(object contentGenerator, object model, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateFileName = string.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileEmailMessageTemplateProvider templateFileNameProvider)
				{

					templateFileName = templateFileNameProvider.SubjectTemplateFileName;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileTemplateProviderWithGetTemplateFileName templateFileNameProvider)
				{
					templateFileName = templateFileNameProvider.GetSubjectTemplateFileName(model);
				}
			}

			return templateFileName;
		}

		async Task<string> ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider.GetPlainTextTemplateKeyAsync(object contentGenerator, object model, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateFileName = string.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileEmailMessageTemplateProvider templateFileNameProvider)
				{
					templateFileName = templateFileNameProvider.PlainTextTemplateFileName;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileTemplateProviderWithGetTemplateFileName templateFileNameProvider)
				{
					templateFileName = templateFileNameProvider.GetPlainTextTemplateFileName(model);
				}
			}

			return templateFileName;
		}

		async Task<string> ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider.GetMhtmlTemplateKeyAsync(object contentGenerator, object model, System.Threading.CancellationToken cancellationToken = default)
		{
			if (!IsTemplateProviderFor(contentGenerator))
			{
				throw new("Wrong content generator template type");
			}

			var templateFileName = string.Empty;

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileEmailMessageTemplateProvider templateFileNameProvider)
				{

					templateFileName = templateFileNameProvider.MhtmlTemplateFileName;
				}
			}

			{
				if (contentGenerator is ISI.Extensions.Emails.EmailMessageGenerator.IUsesFileTemplateProviderWithGetTemplateFileName templateFileNameProvider)
				{
					templateFileName = templateFileNameProvider.GetMhtmlTemplateFileName(model);
				}
			}

			return templateFileName;
		}
	}
}
