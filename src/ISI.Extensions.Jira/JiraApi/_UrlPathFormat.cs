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
		private class UrlPathFormat
		{
			public static readonly string FindUsers = "/rest/api/2/user/search";
			public static readonly string CreateUser = "/rest/api/2/user";
			public static readonly string GetIssueFilters = "/rest/api/2/filter/favourite";
			public static readonly string CreateIssue = "/rest/api/2/issue/";
			public static readonly string AddIssueAttachment = "/rest/api/2/issue/{issueIdOrKey}/attachments";
			public static readonly string FindIssues = "/rest/api/2/search";
			public static readonly string ListIssueWorklogs = "/rest/api/2/issue/{issueIdOrKey}/worklog";
			public static readonly string AddIssueWorklog = "/rest/api/2/issue/{issueIdOrKey}/worklog";
			public static readonly string ListIssueComment = "/rest/api/2/issue/{issueIdOrKey}/comment";
			public static readonly string AddIssueComment = "/rest/api/2/issue/{issueIdOrKey}/comment";
			public static readonly string DeleteAttachment = "/rest/api/2/attachment/{id}";
			public static readonly string DeleteIssueWorklog = "/rest/api/2/issue/{issueIdOrKey}/worklog/{worklogId}";
			public static readonly string ListRoles = "/rest/api/2/role";
			public static readonly string CreateRole = "/rest/api/2/role";
			public static readonly string ListPriorities = "/rest/api/2/priority";
			public static readonly string ListIssueTypes = "/rest/api/2/issuetype";
			public static readonly string ListProjects = "/rest/api/2/project";
			public static readonly string ListProjectRoles = "/rest/api/2/project/{projectIdOrKey}/role";
			public static readonly string ListBoards = "/rest/agile/1.0/board";
			public static readonly string ListSprints = "/rest/agile/1.0/board/{boardId}/sprint";
			public static readonly string FindSprintIssues = "/rest/agile/1.0/board/{boardId}/sprint/{sprintId}/issue";
		}
	}
}