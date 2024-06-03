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
using DTOs = ISI.Extensions.Twilio.DataTransferObjects.MessagesApi;
using SerializableModelsDTOs = ISI.Extensions.Twilio.SerializableModels.MessagesApi;

namespace ISI.Extensions.Twilio
{
	public partial class MessagesApi
	{
		public async Task<DTOs.SendMessageResponse> SendMessageAsync(DTOs.SendMessageRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.SendMessageResponse();

			var uri = new UriBuilder(GetUrl("Accounts/{accountKey}/Messages.json".Replace(new Dictionary<string, string>()
			{
				{ "{accountKey}", request.AccountKey },
			})));

			var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
			formData.Add("From", request.From);
			formData.Add("To", request.To);
			formData.ConditionalAdd(!string.IsNullOrEmpty(request.Body), nameof(SerializableModelsDTOs.CreateMessageOptions.Body), () => request.Body);
			formData.ConditionalAdd(request.Media.NullCheckedAny(), nameof(SerializableModelsDTOs.CreateMessageOptions.MediaUrl), () => request.Media.Select(media => media.Url));

			formData.ConditionalAdd(!string.IsNullOrEmpty(request.MessagingServiceKey), nameof(SerializableModelsDTOs.CreateMessageOptions.MessagingServiceSid), () => request.MessagingServiceKey);
			formData.ConditionalAdd(!string.IsNullOrEmpty(request.StatusCallbackUrl), nameof(SerializableModelsDTOs.CreateMessageOptions.StatusCallback), () => request.StatusCallbackUrl);
			formData.ConditionalAdd(!string.IsNullOrEmpty(request.ApplicationKey), nameof(SerializableModelsDTOs.CreateMessageOptions.ApplicationSid), () => request.ApplicationKey);

			formData.ConditionalAdd(request.MaxPrice.HasValue, nameof(SerializableModelsDTOs.CreateMessageOptions.MaxPrice), () => string.Format("{0}", request.MaxPrice));
			formData.ConditionalAdd(request.ProvideFeedback, nameof(SerializableModelsDTOs.CreateMessageOptions.ProvideFeedback), () => "true");
			formData.ConditionalAdd(request.ValidityPeriodInSeconds.HasValue, nameof(SerializableModelsDTOs.CreateMessageOptions.ValidityPeriod), () => string.Format("{0}", request.ValidityPeriodInSeconds));
			//formData.ConditionalAdd(!string.IsNullOrEmpty(request.MaxRate), nameof(SerializableModelsDTOs.CreateMessageOptions.MaxRate), () => request.MaxRate);
			formData.ConditionalAdd(request.ForceDelivery, nameof(SerializableModelsDTOs.CreateMessageOptions.ForceDelivery), () => "true");
			formData.ConditionalAdd(!string.IsNullOrEmpty(request.ProviderKey), nameof(SerializableModelsDTOs.CreateMessageOptions.PathAccountSid), () => request.ProviderKey);
			//formData.ConditionalAdd(request.ContentRetention.HasValue, nameof(SerializableModelsDTOs.CreateMessageOptions.ContentRetention), () => ISI.Libraries.Enum<ISI.Wrappers.Twilio.SerializableEntities.Messaging.ContentRetention>.Abbreviation(ISI.Libraries.Enum<ISI.Wrappers.Twilio.SerializableEntities.Messaging.ContentRetention>.Convert(request.ContentRetention)));
			//formData.ConditionalAdd(request.AddressRetention.HasValue, nameof(SerializableModelsDTOs.CreateMessageOptions.AddressRetention), () => ISI.Libraries.Enum<ISI.Wrappers.Twilio.SerializableEntities.Messaging.AddressRetention>.Abbreviation(ISI.Libraries.Enum<ISI.Wrappers.Twilio.SerializableEntities.Messaging.AddressRetention>.Convert(request.AddressRetention)));
			formData.ConditionalAdd(request.SmartEncoded, nameof(SerializableModelsDTOs.CreateMessageOptions.SmartEncoded), () => "true");

			var serviceResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<ISI.Extensions.WebClient.Rest.FormDataCollection, SerializableModelsDTOs.SendMessageResponse>(uri.Uri, GetHeaders(request), formData, true);

			response.Message = serviceResponse.Export();

			response.ErrorCode = serviceResponse.ErrorCode;
			response.ErrorMessage = serviceResponse.ErrorMessage;

			return response;
		}
	}
}