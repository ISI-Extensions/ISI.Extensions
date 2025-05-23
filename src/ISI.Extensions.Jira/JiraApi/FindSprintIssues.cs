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
using DTOs = ISI.Extensions.Jira.DataTransferObjects.JiraApi;
using SERIALIZABLE = ISI.Extensions.Jira.SerializableModels;

namespace ISI.Extensions.Jira
{
	public partial class JiraApi
	{
		public DTOs.FindSprintIssuesResponse FindSprintIssues(DTOs.FindSprintIssuesRequest request)
		{
			var response = new DTOs.FindSprintIssuesResponse();
			
			var boardId = GetBoardId(request, null, request.BoardIdOrName);
			if (!boardId.HasValue)
			{
				throw new Exception(string.Format("Cannot find BoardName: \"{0}\"", request.BoardIdOrName));
			}

			var sprintId = GetSprintId(request, boardId, request.SprintIdOrName);
			if (!sprintId.HasValue)
			{
				throw new Exception(string.Format("Cannot find SprintName: \"{0}\"", request.SprintIdOrName));
			}

			var uri = GetJiraApiUri(request);
			uri.SetPathAndQueryString(UrlPathFormat.FindSprintIssues.Replace(new Dictionary<string, string>()
			{
				{ "{boardId}", string.Format("{0}", boardId) },
				{ "{sprintId}", string.Format("{0}", sprintId) },
			}, StringComparer.InvariantCultureIgnoreCase));
			if (request.Skip > 0)
			{
				uri.AddQueryStringParameter("startAt", request.Skip);
			}
			if (request.Take > 0)
			{
				uri.AddQueryStringParameter("maxResults", request.Take);
			}
			if (!string.IsNullOrWhiteSpace(request.Jql))
			{
				uri.AddQueryStringParameter("jql", request.Jql);
			}

			var jiraResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SERIALIZABLE.FindSprintIssuesResponse>(uri.Uri, GetHeaders(request), true, GetSslProtocols(request));

			response.Skip = jiraResponse.Skip;
			response.Take = jiraResponse.Take;
			response.Total = jiraResponse.Total;
			response.Issues = jiraResponse.Issues.ToNullCheckedArray(x => x?.Export());

			return response;
		}
	}
}