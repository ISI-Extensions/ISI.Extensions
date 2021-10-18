#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class JiraApi_Tests
	{
		public string JiraUrl { get; set; }
		public string JiraApiUserName { get; set; }
		public string JiraApiToken { get; set; }

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration)

				//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
				.AddLogging(builder => builder
						.AddConsole()
					//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
				)
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.TextWriterLogger(TestContext.Progress))

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddConfigurationRegistrations(configuration)
				.ProcessServiceRegistrars()
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.SetServiceLocator();

			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "SitePro.keyValue");
			//var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "Tristar.keyValue");
			var settings = new ISI.Extensions.SimpleKeyValueStorage(settingsFullName);

			JiraUrl = settings.GetValue("JiraUrl");
			JiraApiUserName = settings.GetValue("JiraApiUserName");
			JiraApiToken = settings.GetValue("JiraApiToken");
		}

		[Test]
		public void FindUsers_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			var findUsersResponse = jiraApi.FindUsers(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.FindUsersRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				ImpersonatedUser = "rmuth",
				Take = 3000,
			});
		}

		[Test]
		public void GetIssueFilters_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			var getIssueFiltersResponse = jiraApi.GetIssueFilters(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.GetIssueFiltersRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				ImpersonatedUser = "rmuth",
			});
		}

		[Test]
		public void FindIssues_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			var findIssuesResponse = jiraApi.FindIssues(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.FindIssuesRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				//ImpersonatedUser = "rmuth",
				Jql = "assignee=currentuser() AND STATUS!=DONE ORDER BY created DESC",
			});
		}

		[Test]
		public void RemoveAttachments_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			var impersonatedUser = "ljones";

			var findIssuesResponse = jiraApi.FindIssues(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.FindIssuesRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				ImpersonatedUser = impersonatedUser, //user must have access to above project
				Jql = "project=CD AND status IN (Closed,Canceled,Resolved) AND attachments IS NOT EMPTY AND updatedDate < 2020-12-22",
				Fields = new[] { "attachment" },
				//Take = 1001, //1,000 is the max the API will return
			});

			if (findIssuesResponse.Issues.NullCheckedAny())
			{
				var issues = findIssuesResponse.Issues;

				foreach (var issue in issues)
				{
					var attachments = issue.Fields.Attachments;

					foreach (var attachment in attachments)
					{
						jiraApi.DeleteAttachment(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.DeleteAttachmentRequest()
						{
							AttachmentId = attachment.AttachmentId,
							ImpersonatedUser = impersonatedUser, //user must have 'delete all attachments project permission'
						});
					}
				}
			}
		}

		[Test]
		public void AddWorklog_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			jiraApi.AddIssueWorklog(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.AddIssueWorklogRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				ImpersonatedUser = "rmuth",
				IssueIdOrKey = "DEV-13",
				Comment = "Worked on this",
				StartedDateTime = DateTime.Now - TimeSpan.FromHours(5),
				TimeSpent = TimeSpan.FromHours(4),
			});
		}

		[Test]
		public void GetIssueWorklog_Tests()
		{
			var jiraApi = new ISI.Extensions.Jira.JiraApi();

			var getIssueWorklogsResponse = jiraApi.GetIssueWorklogs(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.GetIssueWorklogsRequest()
			{
				JiraApiUrl = JiraUrl,
				JiraApiUserName = JiraApiUserName,
				JiraApiToken = JiraApiToken,
				ImpersonatedUser = "rmuth",
				IssueIdOrKey = "CD-76",
			});

			var groupedWorklogs = getIssueWorklogsResponse.Worklogs.GroupBy(worklog => string.Format("{0}-{1}", worklog.Started.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise), worklog.TimeSpent));

			//foreach (var groupedWorklog in groupedWorklogs.Where(worklogs => worklogs.Count() > 1))
			//{
			//	var isFirst = true;
			//	foreach (var worklog in groupedWorklog)
			//	{
			//		if (isFirst)
			//		{
			//			isFirst = false;
			//		}
			//		else
			//		{
			//			jiraApi.DeleteIssueWorklog(new ISI.Extensions.Jira.DataTransferObjects.JiraApi.DeleteIssueWorklogRequest()
			//			{
			//				JiraApiUrl = JiraUrl,
			//				JiraApiUserName = JiraApiUserName,
			//				JiraApiToken = JiraApiToken,
			//				ImpersonatedUser = worklog.Author.UserKey,
			//				IssueIdOrKey = string.Format("{0}", worklog.IssueId),
			//				WorklogId = worklog.WorklogId,
			//			});
			//		}
			//	}
			//}
		}
	}
}