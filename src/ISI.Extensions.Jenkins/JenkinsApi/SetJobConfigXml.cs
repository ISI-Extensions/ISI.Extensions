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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi;

namespace ISI.Extensions.Jenkins
{
	public partial class JenkinsApi
	{
		public DTOs.SetJobConfigXmlResponse SetJobConfigXml(DTOs.SetJobConfigXmlRequest request)
		{
			var response = new DTOs.SetJobConfigXmlResponse();

			var jobIds = GetJobIds(new()
			{
				JenkinsUrl = request.JenkinsUrl,
				UserName = request.UserName,
				ApiToken = request.ApiToken,
				SslProtocols = request.SslProtocols,
			}).JobIds;

			var jobExists = false;

			if (jobIds != null)
			{
				jobExists = jobIds.Any(jobId => string.Equals(jobId, request.JobId, StringComparison.InvariantCultureIgnoreCase));
			}

			var uri = new UriBuilder(request.JenkinsUrl);

			if (jobExists)
			{
				uri.SetPathAndQueryString(UrlPathFormat.GetJobConfigXml.Replace(new Dictionary<string, string>()
				{
					{"{jobId}", request.JobId}
				}, StringComparer.InvariantCultureIgnoreCase));
			}
			else
			{
				uri.SetPathAndQueryString(UrlPathFormat.CreateJobConfigXml.Replace(new Dictionary<string, string>()
				{
					{"{jobId}", request.JobId}
				}, StringComparer.InvariantCultureIgnoreCase));
			}

			var headers = GetHeaders(request);

			var csrfHeader = GetCSRFTokenHeader(request);
			if (csrfHeader != null)
			{
				headers.Add(csrfHeader);
			}

			headers.ContentType = ISI.Extensions.WebClient.Rest.ContentTypeXmlHeaderValue;

#if DEBUG
			var eventHandler = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			try
			{
				ISI.Extensions.WebClient.Rest.ExecutePost(uri.Uri, headers, new ISI.Extensions.WebClient.Rest.TextRequest(request.ConfigXml), true, request.SslProtocols);
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "SetJobConfigXml Failed");
			}

			return response;
		}
	}
}