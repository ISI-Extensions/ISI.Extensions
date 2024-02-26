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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.Emails.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Mandrill.DataTransferObjects.MandrillMessagesApi;
using SerializableDTOs = ISI.Extensions.Mandrill.SerializableModels.MandrillMessagesApi;

namespace ISI.Extensions.Mandrill
{
	public partial class MandrillMessagesApi
	{
		public async Task<DTOs.SendEmailResponse> SendEmailAsync(DTOs.SendEmailRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.SendEmailResponse();

			if (MandrillProfilesApi.TryGetMandrillProfile(request.Email.EmailProviderUuid.GetValueOrDefault(), out var mandrillProfile))
			{
				var uri = GetMessageApiUri(mandrillProfile);
				uri.AddDirectoryToPath("messages/send.json");

				var restRequest = new SerializableDTOs.SendEmailRequest()
				{
					ApiKey = mandrillProfile.ApiKey,
					Message = new(),
					SendingPoolKey = (string.IsNullOrWhiteSpace(request.Email.EmailProviderDeliveryChannelName) ? mandrillProfile.DefaultSendingPoolKey : request.Email.EmailProviderDeliveryChannelName),
					ScheduledSendDateTimeUtc = request.ScheduledSendDateTimeUtc,
				};

				restRequest.Message.Subject = request.Email.EmailMailMessage.Subject;

				restRequest.Message.FromEmailAddress = request.Email.EmailMailMessage.From.Address;
				restRequest.Message.FromEmailAddressCaption = request.Email.EmailMailMessage.From.Caption;

				var emailAddresses = new List<SerializableDTOs.SendEmailRequestMessageEmailAddress>();

				emailAddresses.AddRange(request.Email.EmailMailMessage.To.ToNullCheckedArray(emailAddress => new SerializableDTOs.SendEmailRequestMessageEmailAddress()
				{
					AddressType = SerializableDTOs.SendEmailRequestEmailAddressType.To,
					EmailAddress = emailAddress.Address,
					EmailAddressCaption = emailAddress.Caption,
				}));

				emailAddresses.AddRange(request.Email.EmailMailMessage.CC.ToNullCheckedArray(emailAddress => new SerializableDTOs.SendEmailRequestMessageEmailAddress()
				{
					AddressType = SerializableDTOs.SendEmailRequestEmailAddressType.Cc,
					EmailAddress = emailAddress.Address,
					EmailAddressCaption = emailAddress.Caption,
				}));

				emailAddresses.AddRange(request.Email.EmailMailMessage.Bcc.ToNullCheckedArray(emailAddress => new SerializableDTOs.SendEmailRequestMessageEmailAddress()
				{
					AddressType = SerializableDTOs.SendEmailRequestEmailAddressType.Bcc,
					EmailAddress = emailAddress.Address,
					EmailAddressCaption = emailAddress.Caption,
				}));

				restRequest.Message.To = emailAddresses.ToArray();

				restRequest.Message.Important = (request.Email.EmailMailMessage.Priority == ISI.Extensions.Emails.EmailMessagePriority.High);

				var attachments = request.Email.EmailMailMessage.Attachments;
				if (attachments.Any())
				{
					restRequest.Message.Attachments = attachments.Select(attachment => new SerializableDTOs.SendEmailRequestMessageAttachment()
					{
						MimeType = attachment.ContentType.Name,
						FileName = attachment.Name,
						Content = Convert.ToBase64String(attachment.Content)
					}).ToArray();
				}

				if (request.Email.EmailMailMessage.AlternateViews.TryGetAlternateView(ISI.Extensions.Emails.EmailMailMessageAlternateView.MthmlMediaType, out var mhtmlAlternateView))
				{
					restRequest.Message.Mhtml = mhtmlAlternateView.Rendered();
				}

				if (request.Email.EmailMailMessage.AlternateViews.TryGetAlternateView(ISI.Extensions.Emails.EmailMailMessageAlternateView.PlainTextMediaType, out var plainTextAlternateView))
				{
					restRequest.Message.PlainText = plainTextAlternateView.Rendered();
				}

				var body = request.Email.EmailMailMessage.Body;
				if (!string.IsNullOrWhiteSpace(body))
				{
					if (request.Email.EmailMailMessage.IsBodyHtml)
					{
						restRequest.Message.Mhtml = body;
					}
					else
					{
						restRequest.Message.PlainText = body;
					}
				}

				restRequest.Message.Metadata = request.Email.EmailProviderMetadata?.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
				restRequest.Message.SubAccount = (string.IsNullOrWhiteSpace(mandrillProfile.ForceSubAccount) ? request.Email.BillingAccountNumber : mandrillProfile.ForceSubAccount);

				restRequest.Message.TrackOpens = request.Email.TrackOpens;
				restRequest.Message.TrackClicks = request.Email.TrackClicks;
				restRequest.Message.TrackingDomain = request.Email.TrackingDomain;
				restRequest.Message.SigningDomain = request.Email.TrackingSigningDomain;
				restRequest.Message.ReturnPathDomain = request.Email.TrackingReturnPathDomain;
				restRequest.Message.Tags = request.Email.TrackingTags;
				restRequest.Message.GoogleAnalyticsDomains = request.Email.TrackingGoogleAnalyticsDomains;
				restRequest.Message.GoogleAnalyticsCampaigns = request.Email.TrackingGoogleAnalyticsCampaigns;

				try
				{
					var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.SendEmailRequest, SerializableDTOs.SendEmailResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetWebClientHeaders(mandrillProfile), restRequest, false);

					if (restResponse?.Response != null)
					{
						response.SentStatus = ISI.Extensions.Emails.EmailSenderSentStatus.Sent;

						if (string.Equals(restResponse.Response.Status, "Error", StringComparison.InvariantCultureIgnoreCase))
						{
							if (string.Equals(restResponse.Response.Name, "PaymentRequired", StringComparison.InvariantCultureIgnoreCase))
							{
								response.SentStatus = ISI.Extensions.Emails.EmailSenderSentStatus.AccountError;
							}
							else if (string.Equals(restResponse.Response.Name, "Invalid_Key", StringComparison.InvariantCultureIgnoreCase))
							{
								response.SentStatus = ISI.Extensions.Emails.EmailSenderSentStatus.AuthenticationError;
							}
							else if (string.Equals(restResponse.Response.Name, "ValidationError", StringComparison.InvariantCultureIgnoreCase))
							{
								response.SentStatus = ISI.Extensions.Emails.EmailSenderSentStatus.ValidationError;
							}
							else
							{
								response.SentStatus = ISI.Extensions.Emails.EmailSenderSentStatus.GeneralError;
							}

							response.Message = restResponse.Response.Message;
						}

						var emailSenderResponseRecipients = new List<ISI.Extensions.Emails.IEmailSenderResponseRecipient>();

						foreach (var emailAddressResponse in restResponse.Response)
						{
							var emailSenderResponseRecipient = new ISI.Extensions.Emails.EmailSenderResponseRecipient()
							{
								EmailAddress = emailAddressResponse.EmailAddress,
								TrackingKey = emailAddressResponse.TrackingKey,
								SentStatus = ISI.Extensions.Enum<ISI.Extensions.Emails.EmailSenderSentStatus?>.Parse(emailAddressResponse.Status),
								RejectReason = ISI.Extensions.Enum<ISI.Extensions.Emails.EmailSenderRejectReason?>.Parse(emailAddressResponse.RejectReason),
								Message = emailAddressResponse.RejectReason
							};

							emailSenderResponseRecipients.Add(emailSenderResponseRecipient);
						}

						response.RecipientResponses = emailSenderResponseRecipients.ToArray();
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SendEmail Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			return response;
		}
	}
}