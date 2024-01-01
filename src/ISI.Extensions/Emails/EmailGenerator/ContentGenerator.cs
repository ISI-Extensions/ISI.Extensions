#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Emails.EmailGenerator
{
	public delegate string ContentFormatter<TModel>(string view, TModel model)
		where TModel : class, IModel;

	public abstract class ContentGenerator<TModel> : IContentGenerator<TModel>
		where TModel : class, IModel
	{
		protected virtual Microsoft.Extensions.Logging.ILogger Logger { get; }

		private ITemplateProvider _templateProvider = null;
		protected ITemplateProvider TemplateProvider => (_templateProvider ??= ISI.Extensions.TemplateProviders.TemplateProviderFactory.GetTemplateProvider<ITemplateProvider>(this, true));

		public virtual Type ModelType => typeof(TModel);

		protected ContentGenerator(Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}

		protected virtual System.IO.Stream GetTemplateStream(string templateCacheKey)
		{
			return TemplateProvider.GetTemplateStream(templateCacheKey);
		}

		protected virtual string GetTemplateCacheKey(string templateKey, IModel model)
		{
			return TemplateProvider.GetTemplateCacheKey(this, templateKey, model);
		}

		#region Subject
		protected virtual string GetSubjectTemplateKey(IModel model)
		{
			return TemplateProvider.GetSubjectTemplateKey(this, model);
		}

		public virtual ContentFormatter<TModel> SubjectContentFormatter { get; } = (content, model) => content;

		public virtual string GetSubjectContent(TModel model)
		{
			var templateKey = GetSubjectTemplateKey(model);

			var templateCacheKey = GetTemplateCacheKey(templateKey, model);

			using (var templateStream = GetTemplateStream(templateCacheKey))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion

		#region PlainText
		protected virtual string GetPlainTextTemplateKey(IModel model)
		{
			return TemplateProvider.GetPlainTextTemplateKey(this, model);
		}

		public virtual ContentFormatter<TModel> PlainTextContentFormatter { get; } = (content, model) => content;

		public virtual string GetPlainTextContent(TModel model)
		{
			var templateKey = GetPlainTextTemplateKey(model);

			var templateCacheKey = GetTemplateCacheKey(templateKey, model);

			using (var templateStream = GetTemplateStream(templateCacheKey))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion

		#region Mhtml
		protected virtual string GetMhtmlTemplateKey(IModel model)
		{
			return TemplateProvider.GetMhtmlTemplateKey(this, model);
		}

		public virtual ContentFormatter<TModel> MhtmlContentFormatter { get; } = (content, model) => content;

		public virtual string GetMhtmlContent(TModel model)
		{
			var templateKey = GetMhtmlTemplateKey(model);

			var templateCacheKey = GetTemplateCacheKey(templateKey, model);

			using (var templateStream = GetTemplateStream(templateCacheKey))
			{
				return (templateStream == null ? string.Empty : templateStream.TextReadToEnd());
			}
		}
		#endregion




		public virtual bool Track(TModel model)
		{
			var result = true;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.Track;
			}

			return result;
		}

		public virtual bool TrackOpens(TModel model)
		{
			var result = true;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.TrackOpens;
			}

			return result;
		}

		public virtual bool TrackClicks(TModel model)
		{
			var result = true;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.TrackClicks;
			}

			return result;
		}

		public virtual string TrackingDomain(TModel model)
		{
			string result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.TrackingDomain;
			}

			return result;
		}

		public virtual string SigningDomain(TModel model)
		{
			string result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.SigningDomain;
			}

			return result;
		}

		public virtual string ReturnPathDomain(TModel model)
		{
			string result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.ReturnPathDomain;
			}

			return result;
		}

		public virtual string[] TrackingTags(TModel model)
		{
			string[] result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.TrackingTags;
			}

			return result;
		}

		public virtual string[] GoogleAnalyticsDomains(TModel model)
		{
			string[] result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.GoogleAnalyticsDomains;
			}

			return result;
		}

		public virtual string[] GoogleAnalyticsCampaigns(TModel model)
		{
			string[] result = null;

			if (model is IModelHasTracking trackingModel)
			{
				result = trackingModel.GoogleAnalyticsCampaigns;
			}

			return result;
		}

		public virtual IDictionary<string, string> Metadata(TModel model)
		{
			IDictionary<string, string> result = null;

			if (model is IModelHasMetadataInformation metadataInformationModel)
			{
				result = metadataInformationModel.Metadata;
			}

			return result;
		}

		public virtual string BillingAccountNumber(TModel model)
		{
			string result = null;

			if (model is IModelHasBillingInformation billingInformationModel)
			{
				result = billingInformationModel.BillingAccountNumber;
			}

			return result;
		}

		public virtual string DeliveryChannelName(TModel model)
		{
			string result = null;

			if (model is IModelHasDeliveryInformation deliveryInformationModel)
			{
				result = deliveryInformationModel.DeliveryChannelName;
			}

			return result;
		}

		public virtual string ProfileKey(TModel model)
		{
			string result = null;

			if (model is IModelHasProviderProfileInformation providerProfileInformationModel)
			{
				result = providerProfileInformationModel.ProviderProfileKey;
			}

			return result;
		}





		public virtual TResult GenerateEmail<TResult>(TModel model, TResult emailMailMessage)
			where TResult : ISI.Extensions.Emails.IEmailMailMessage
		{
			if (string.IsNullOrWhiteSpace(model.Subject))
			{
				var subjectView = GetSubjectContent(model);

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
				var attachments = new List<IEmailMailMessageAttachment>(emailMailMessage.Attachments ?? Array.Empty<IEmailMailMessageAttachment>());

				attachments.AddRange(model.Attachments);

				emailMailMessage.Attachments = attachments.ToArray();
			}

			//must be in this order re: http://stackoverflow.com/questions/5188605/gmail-displays-plain-text-email-instead-html
			var alternateViews = new List<IEmailMailMessageAlternateView>(emailMailMessage.AlternateViews ?? Array.Empty<IEmailMailMessageAlternateView>());

			var plainView = GetPlainTextContent(model);
			if (!string.IsNullOrWhiteSpace(plainView))
			{
				plainView = PlainTextContentFormatter(plainView, model as TModel);

				if (!string.IsNullOrWhiteSpace(plainView))
				{
					alternateViews.Add(new EmailMailMessageAlternateView(plainView, null, ISI.Extensions.Emails.EmailMailMessageAlternateView.PlainTextMediaType));
				}
			}

			var mhtmlView = GetMhtmlContent(model);
			if (!string.IsNullOrWhiteSpace(mhtmlView))
			{
				mhtmlView = MhtmlContentFormatter(mhtmlView, model as TModel);

				if (!string.IsNullOrWhiteSpace(mhtmlView))
				{
					alternateViews.Add(new EmailMailMessageAlternateView(mhtmlView, null, ISI.Extensions.Emails.EmailMailMessageAlternateView.MthmlMediaType));
				}
			}

			emailMailMessage.AlternateViews = alternateViews.ToArray();

			if (emailMailMessage is IEmailMailMessageHasTrackingInformation trackingInformation)
			{
				trackingInformation.Track = Track(model);
				trackingInformation.TrackOpens = TrackOpens(model);
				trackingInformation.TrackClicks = TrackClicks(model);
				trackingInformation.TrackingDomain = TrackingDomain(model);
				trackingInformation.SigningDomain = SigningDomain(model);
				trackingInformation.ReturnPathDomain = ReturnPathDomain(model);
				trackingInformation.TrackingTags = (TrackingTags(model) ?? new string[] { }).ToArray();
				trackingInformation.GoogleAnalyticsDomains = (GoogleAnalyticsDomains(model) ?? new string[] { }).ToArray();
				trackingInformation.GoogleAnalyticsCampaigns = (GoogleAnalyticsCampaigns(model) ?? new string[] { }).ToArray();
			}

			if (emailMailMessage is IEmailMailMessageHasMetadataInformation metadataInformation)
			{
				metadataInformation.Metadata = Metadata(model);
			}

			if (emailMailMessage is IEmailMailMessageHasBillingInformation billingInformation)
			{
				billingInformation.BillingAccountNumber = BillingAccountNumber(model);
			}

			if (emailMailMessage is IEmailMailMessageHasDeliveryInformation deliveryInformation)
			{
				deliveryInformation.DeliveryChannelName = DeliveryChannelName(model);
			}

			if (emailMailMessage is IEmailMailMessageHasProviderProfileInformation providerProfileInformation)
			{
				providerProfileInformation.ProviderProfileKey = ProfileKey(model);
			}

			return emailMailMessage;
		}
	}
}
