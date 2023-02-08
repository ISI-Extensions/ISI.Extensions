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
using ISI.Extensions.Slack.Extensions;
using DTOs = ISI.Extensions.Slack.DataTransferObjects.SlackClient;

namespace ISI.Extensions.Slack
{
	public partial class SlackClient
	{
		public DTOs.SvnCommitHookResponse SvnCommitHook(DTOs.SvnCommitHookRequest request)
		{
			var response = new DTOs.SvnCommitHookResponse();

			var ignoreAuthors = Configuration.SvnCommitHook.GetIgnoreAuthors();

			if (!ignoreAuthors.Contains(request.Author))
			{
				var uri = new UriBuilder(Configuration.Url);
				uri.SetPathAndQueryString(UrlPathFormat.SvnCommitHook.Replace(new Dictionary<string, string>()
				{
					{"{token}", Configuration.SvnCommitHook.Token}
				}));

				var payloadRequest = new SerializableModels.SvnCommitHookPayload()
				{
					Revision = request.Revision,
					Author = request.Author,
					Log = string.Format("{0}\r\n{1}", request.Message, string.Join("\r\n", request.Directories ?? Array.Empty<string>()))
				};

				var revisionUrl = (string.IsNullOrEmpty(request.RevisionUrl) ? Configuration.SvnCommitHook.RevisionUrl : request.RevisionUrl);
				if (!string.IsNullOrWhiteSpace(revisionUrl))
				{
					var revisionUri = new UriBuilder(revisionUrl.Replace(new Dictionary<string, string>()
					{
						{"{revision}", string.Format("{0}", request.Revision)}
					}));

					payloadRequest.Url = revisionUri.Uri.ToString();
				}

				var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
				formData.Add("payload", Serialization.Serialize(payloadRequest, ISI.Extensions.Serialization.SerializationFormat.Json).SerializedValue);

				response.Content = ISI.Extensions.WebClient.Rest.ExecuteFormRequestPost<ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), formData, true)?.Content;
			}

			return response;
		}
	}
}