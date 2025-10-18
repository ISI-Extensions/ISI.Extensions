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
		public DTOs.CreateIssueResponse CreateIssue(DTOs.CreateIssueRequest request)
		{
			var response = new DTOs.CreateIssueResponse();

			var uri = GetJiraApiUri(request);
			uri.SetPathAndQueryString(UrlPathFormat.CreateIssue);

			var projectId = GetProjectId(request, request.ProjectIdOrKey);
			if (!projectId.HasValue)
			{
				throw new Exception($"Cannot find ProjectKey: \"{request.ProjectIdOrKey}\"");
			}

			var issueTypeId = GetIssueTypeId(request, request.IssueTypeIdOrName);
			if (!issueTypeId.HasValue)
			{
				throw new Exception($"Cannot find IssueTypeKey: \"{request.IssueTypeIdOrName}\"");
			}

			var reporter = request.Reporter;
			if (!string.IsNullOrWhiteSpace(reporter) && request.CreateReporterIfNotFound)
			{
				ISI.Extensions.Emails.EmailAddress.TryCreateEmailAddress(reporter, out var emailAddress);

				var findUsersResponse = FindUsers(new DTOs.FindUsersRequest()
				{
					ImpersonatedUser = request.ImpersonatedUser,
					UserName = (emailAddress == null ? reporter : emailAddress.Address),
				});

				if (!findUsersResponse.Users.NullCheckedAny())
				{
					var createUserResponse = CreateUser(MakeRequest<DTOs.CreateUserRequest>(request, newRequest =>
					{
						newRequest.UserName = (emailAddress == null ? reporter : emailAddress.Address);
						newRequest.DisplayName = (string.IsNullOrWhiteSpace(emailAddress?.Caption) ? emailAddress?.Address : emailAddress?.Caption) ?? reporter;
						newRequest.EmailAddress = emailAddress?.Address;
					}));

					reporter = createUserResponse.User.Name;
				}
			}

			var priorityId = GetPriorityId(request, request.PriorityIdOrName);

			var jiraRequest = new SERIALIZABLE.CreateIssueRequest()
			{
				Fields = new SERIALIZABLE.CreateIssueFields()
				{
					Project = new SERIALIZABLE.CreateIssueProject()
					{
						ProjectId = projectId.Value,
					},
					Summary = request.Summary,
					IssueType = new SERIALIZABLE.CreateIssueIssueType()
					{
						IssueTypeId = issueTypeId.Value,
					},
					Assignee = (string.IsNullOrWhiteSpace(request.Assignee) ? null : new SERIALIZABLE.CreateIssueUser()
					{
						Name = request.Assignee,
					}),
					Reporter = (string.IsNullOrWhiteSpace(request.Reporter) ? null : new SERIALIZABLE.CreateIssueUser()
					{
						Name = reporter,
					}),
					Priority = (!priorityId.HasValue ? null : new SERIALIZABLE.CreateIssuePriority()
					{
						PriorityId = priorityId.Value,
					}),
					Labels = request.Labels.ToNullCheckedArray(),
					Environment = request.Environment,
					Description = request.Description,
					DueDate = request.DueDate,
				}
			};

			var jiraResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SERIALIZABLE.CreateIssueRequest, SERIALIZABLE.CreateIssueResponse>(uri.Uri, GetHeaders(request), jiraRequest, true, GetSslProtocols(request));

			response.IssueId = jiraResponse.IssueId;
			response.IssueKey = jiraResponse.IssueKey;
			response.IssueUrl = jiraResponse.IssueUrl;

			return response;
		}
	}
}