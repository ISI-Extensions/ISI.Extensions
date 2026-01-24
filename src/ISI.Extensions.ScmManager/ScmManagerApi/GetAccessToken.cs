#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi;
using SerializableDTOs = ISI.Extensions.ScmManager.SerializableModels;

namespace ISI.Extensions.ScmManager
{
	public partial class ScmManagerApi
	{
		public DTOs.GetAccessTokenResponse GetAccessToken(DTOs.GetAccessTokenRequest request)
		{
			var response = new DTOs.GetAccessTokenResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString("api/v2/auth/access_token");

			var apiRequest = new SerializableDTOs.GetAccessTokenRequest()
			{
				Username = request.Username,
				Password = request.Password,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var headers = new ISI.Extensions.WebClient.HeaderCollection();
			headers.Accept = "*/*";

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
			{
				{ System.Net.HttpStatusCode.OK, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.NoContent, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.BadRequest, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.Unauthorized, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.InternalServerError, typeof(ISI.Extensions.WebClient.Rest.SerializedResponse<SerializableDTOs.SetRepositoryRoleErrorResponse>) },
			}, uri.Uri, headers, apiRequest, true);

			if ((apiResponse.StatusCode == System.Net.HttpStatusCode.OK) && (apiResponse.Response is ISI.Extensions.WebClient.Rest.TextResponse textResponse))
			{
				response.ScmManagerApiToken = textResponse.Content;
			}

			return response;
		}
	}
}