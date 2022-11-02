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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.JenkinsServiceApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Scm
{
	public partial class JenkinsServiceApi
	{
		public const string GetJenkinsJobIdsUrlPath = "get-jenkins-job-ids";

		public DTOs.GetJenkinsJobIdsResponse GetJenkinsJobIds(DTOs.GetJenkinsJobIdsRequest request)
		{
			var response = new DTOs.GetJenkinsJobIdsResponse();

			var uri = new UriBuilder(request.JenkinsServiceUrl);
			uri.SetPathAndQueryString(string.Format("api/{0}", GetJenkinsJobIdsUrlPath));
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.SettingsFullName), "settingsFullName", request.SettingsFullName);
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.JenkinsUrl), "jenkinsUrl", request.JenkinsUrl);
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.UserName), "userName", request.UserName);
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.ApiToken), "apiToken", request.ApiToken);
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.FilterByJobIdPrefix), "filterByJobIdPrefix", request.FilterByJobIdSuffix);
			uri.ConditionalAddQueryStringParameter(request.StripJobIdPrefix.HasValue, "stripJobIdPrefix", request.StripJobIdPrefix.TrueFalse());
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.FilterByJobIdSuffix), "filterByJobIdSuffix", request.FilterByJobIdSuffix);
			uri.ConditionalAddQueryStringParameter(request.StripJobIdSuffix.HasValue, "stripJobIdSuffix", request.StripJobIdSuffix.TrueFalse());
			uri.ConditionalAddQueryStringParameter(!string.IsNullOrWhiteSpace(request.InsertJobId), "insertJobId", request.InsertJobId);

			try
			{
				response.JobIds = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, new(), true).Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "GetJenkinsJobIds Failed");
			}

			return response;
		}
	}
}