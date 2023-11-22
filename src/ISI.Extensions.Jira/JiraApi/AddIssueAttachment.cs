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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Jira.DataTransferObjects.JiraApi;
using SERIALIZABLE = ISI.Extensions.Jira.SerializableModels;

namespace ISI.Extensions.Jira
{
	public partial class JiraApi
	{
		public DTOs.AddIssueAttachmentResponse AddIssueAttachment(DTOs.AddIssueAttachmentRequest request)
		{
			var response = new DTOs.AddIssueAttachmentResponse();

			var uri = GetJiraApiUri(request);
			uri.SetPathAndQueryString(UrlPathFormat.AddIssueAttachment.Replace(new Dictionary<string, string>()
			{
				{"{issueIdOrKey}", request.IssueIdOrKey.ToString()},
			}, StringComparer.InvariantCultureIgnoreCase));

			using (var httpClient = new System.Net.Http.HttpClient())
			{
				using (var httpRequestMessage = new System.Net.Http.HttpRequestMessage(new System.Net.Http.HttpMethod("POST"), uri.Uri))
				{
					httpRequestMessage.Headers.TryAddWithoutValidation(HeaderKeys.AtlassianToken, HeaderValues.AtlassianToken_NoCheck);

					if (!string.IsNullOrWhiteSpace(request?.ImpersonatedUser) && !string.Equals(request.JiraApiUserName, request.ImpersonatedUser, StringComparison.InvariantCulture))
					{
						httpRequestMessage.Headers.TryAddWithoutValidation(HeaderKeys.ImpersonatedUser, request.ImpersonatedUser);
					}

					var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", request.JiraApiUserName, request.JiraApiToken)));
					httpRequestMessage.Headers.TryAddWithoutValidation(ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization, $"Basic {base64authorization}");

					var multipartContent = new System.Net.Http.MultipartFormDataContent();

					var streamContent = new System.Net.Http.StreamContent(request.FileStream);
					streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
					streamContent.Headers.ContentDisposition.Name = "\"file\"";
					streamContent.Headers.ContentDisposition.FileName = string.Format("\"{0}\"", System.IO.Path.GetFileName(request.FileName));
					streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

					multipartContent.Add(streamContent);

					httpRequestMessage.Content = multipartContent;

					var httpResponse = httpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
				}
			}

			return response;
		}
	}
}