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

namespace ISI.Extensions.Emails
{
	public delegate string EmailMessageContentFormatter<TModel>(string view, TModel model)
		where TModel : class, ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel;

	public abstract class EmailMessageContentGenerator<TModel> : ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGenerator<TModel>
		where TModel : class, ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel
	{
		protected virtual Microsoft.Extensions.Logging.ILogger Logger { get; }

		private ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider _templateProvider = null;
		protected ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider TemplateProvider => (_templateProvider ??= ISI.Extensions.TemplateProviders.TemplateProviderFactory.GetTemplateProvider<ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageTemplateProvider>(this, true));

		public virtual Type ModelType => typeof(TModel);

		protected EmailMessageContentGenerator(Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		protected virtual async Task<System.IO.Stream> GetTemplateStreamAsync(string templateCacheKey, System.Threading.CancellationToken cancellationToken = default)
		{
			return await TemplateProvider.GetTemplateStreamAsync(templateCacheKey, cancellationToken);
		}

		protected virtual async Task<string> GetTemplateCacheKeyAsync(string templateKey, ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			return await TemplateProvider.GetTemplateCacheKeyAsync(this, templateKey, model, cancellationToken);
		}

		#region Subject
		protected virtual async Task<string> GetSubjectTemplateKeyAsync(ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			return await TemplateProvider.GetSubjectTemplateKeyAsync(this, model, cancellationToken);
		}

		public virtual EmailMessageContentFormatter<TModel> SubjectContentFormatter { get; } = (content, model) => content;

		public virtual async Task<string> GetSubjectContentAsync(TModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			var templateKey = (await GetSubjectTemplateKeyAsync(model, cancellationToken));

			var templateCacheKey = (await GetTemplateCacheKeyAsync(templateKey, model, cancellationToken));

			using (var templateStream = (await GetTemplateStreamAsync(templateCacheKey, cancellationToken)))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion

		#region PlainText
		protected virtual async Task<string> GetPlainTextTemplateKeyAsync(ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			return await TemplateProvider.GetPlainTextTemplateKeyAsync(this, model, cancellationToken);
		}

		public virtual EmailMessageContentFormatter<TModel> PlainTextContentFormatter { get; } = (content, model) => content;

		public virtual async Task<string> GetPlainTextContentAsync(TModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			var templateKey = (await GetPlainTextTemplateKeyAsync(model, cancellationToken));

			var templateCacheKey = (await GetTemplateCacheKeyAsync(templateKey, model, cancellationToken));

			using (var templateStream = (await GetTemplateStreamAsync(templateCacheKey, cancellationToken)))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion

		#region Mhtml
		protected virtual async Task<string> GetMhtmlTemplateKeyAsync(ISI.Extensions.Emails.EmailMessageGenerator.IEmailMessageContentGeneratorModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			return await TemplateProvider.GetMhtmlTemplateKeyAsync(this, model, cancellationToken);
		}

		public virtual EmailMessageContentFormatter<TModel> MhtmlContentFormatter { get; } = (content, model) => content;

		public virtual async Task<string> GetMhtmlContentAsync(TModel model, System.Threading.CancellationToken cancellationToken = default)
		{
			var templateKey = (await GetMhtmlTemplateKeyAsync(model, cancellationToken));

			var templateCacheKey = (await GetTemplateCacheKeyAsync(templateKey, model, cancellationToken));

			using (var templateStream = (await GetTemplateStreamAsync(templateCacheKey, cancellationToken)))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion

		public virtual async Task<TResult> GenerateEmailMessageAsync<TResult>(TModel model, TResult emailMailMessage, System.Threading.CancellationToken cancellationToken = default)
			where TResult : ISI.Extensions.Emails.IEmailMailMessage
		{
			if (string.IsNullOrWhiteSpace(model.Subject))
			{
				var subjectView = (await GetSubjectContentAsync(model, cancellationToken));

				subjectView = SubjectContentFormatter(subjectView, model);

				model.Subject = subjectView;
			}

			emailMailMessage.Subject = model.Subject.Replace("\r", string.Empty).Replace("\n", string.Empty);

			if (model.FromEmailAddress != null)
			{
				emailMailMessage.From = model.FromEmailAddress;
			}

			if (model.ToEmailAddresses.NullCheckedAny())
			{
				emailMailMessage.To = model.ToEmailAddresses;
			}

			if (model.CcEmailAddresses.NullCheckedAny())
			{
				emailMailMessage.CC = model.CcEmailAddresses;
			}

			if (model.BccEmailAddresses.NullCheckedAny())
			{
				emailMailMessage.Bcc = model.BccEmailAddresses;
			}

			emailMailMessage.Priority = model.Priority;

			if (model.Attachments != null)
			{
				var attachments = new List<IEmailMailMessageAttachment>(emailMailMessage.Attachments ?? []);

				attachments.AddRange(model.Attachments);

				emailMailMessage.Attachments = attachments.ToArray();
			}

			//must be in this order re: http://stackoverflow.com/questions/5188605/gmail-displays-plain-text-email-instead-html
			var alternateViews = new List<IEmailMailMessageAlternateView>(emailMailMessage.AlternateViews ?? []);

			var plainView = (await GetPlainTextContentAsync(model, cancellationToken));
			if (!string.IsNullOrWhiteSpace(plainView))
			{
				plainView = PlainTextContentFormatter(plainView, model as TModel);

				if (!string.IsNullOrWhiteSpace(plainView))
				{
					alternateViews.Add(new EmailMailMessageAlternateView(plainView, null, ISI.Extensions.Emails.EmailMailMessageAlternateView.PlainTextMediaType));
				}
			}

			var mhtmlView = (await GetMhtmlContentAsync(model, cancellationToken));
			if (!string.IsNullOrWhiteSpace(mhtmlView))
			{
				mhtmlView = MhtmlContentFormatter(mhtmlView, model as TModel);

				if (!string.IsNullOrWhiteSpace(mhtmlView))
				{
					alternateViews.Add(new EmailMailMessageAlternateView(mhtmlView, null, ISI.Extensions.Emails.EmailMailMessageAlternateView.MthmlMediaType));
				}
			}

			emailMailMessage.AlternateViews = alternateViews.ToArray();

			return emailMailMessage;
		}
	}
}
