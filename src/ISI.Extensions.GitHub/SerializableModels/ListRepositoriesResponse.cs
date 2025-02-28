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
using System.Runtime.Serialization;

namespace ISI.Extensions.GitHub.SerializableModels
{
	[DataContract]
	public class ListRepositoriesResponseRepository
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "node_id", EmitDefaultValue = false)]
		public string NodeId { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "full_name", EmitDefaultValue = false)]
		public string FullName { get; set; }

		[DataMember(Name = "owner", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryOwner Owner { get; set; }

		[DataMember(Name = "_private", EmitDefaultValue = false)]
		public bool Private { get; set; }

		[DataMember(Name = "html_url", EmitDefaultValue = false)]
		public string HtmlUrl { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "fork", EmitDefaultValue = false)]
		public bool Fork { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "archive_url", EmitDefaultValue = false)]
		public string ArchiveUrl { get; set; }

		[DataMember(Name = "assignees_url", EmitDefaultValue = false)]
		public string AssigneesUrl { get; set; }

		[DataMember(Name = "blobs_url", EmitDefaultValue = false)]
		public string BlobsUrl { get; set; }

		[DataMember(Name = "branches_url", EmitDefaultValue = false)]
		public string BranchesUrl { get; set; }

		[DataMember(Name = "collaborators_url", EmitDefaultValue = false)]
		public string CollaboratorsUrl { get; set; }

		[DataMember(Name = "comments_url", EmitDefaultValue = false)]
		public string CommentsUrl { get; set; }

		[DataMember(Name = "commits_url", EmitDefaultValue = false)]
		public string CommitsUrl { get; set; }

		[DataMember(Name = "compare_url", EmitDefaultValue = false)]
		public string CompareUrl { get; set; }

		[DataMember(Name = "contents_url", EmitDefaultValue = false)]
		public string ContentsUrl { get; set; }

		[DataMember(Name = "contributors_url", EmitDefaultValue = false)]
		public string ContributorsUrl { get; set; }

		[DataMember(Name = "deployments_url", EmitDefaultValue = false)]
		public string DeploymentsUrl { get; set; }

		[DataMember(Name = "downloads_url", EmitDefaultValue = false)]
		public string DownloadsUrl { get; set; }

		[DataMember(Name = "events_url", EmitDefaultValue = false)]
		public string EventsUrl { get; set; }

		[DataMember(Name = "forks_url", EmitDefaultValue = false)]
		public string ForksUrl { get; set; }

		[DataMember(Name = "git_commits_url", EmitDefaultValue = false)]
		public string GitCommitsUrl { get; set; }

		[DataMember(Name = "git_refs_url", EmitDefaultValue = false)]
		public string GitRefsUrl { get; set; }

		[DataMember(Name = "git_tags_url", EmitDefaultValue = false)]
		public string GitTagsUrl { get; set; }

		[DataMember(Name = "git_url", EmitDefaultValue = false)]
		public string GitUrl { get; set; }

		[DataMember(Name = "issue_comment_url", EmitDefaultValue = false)]
		public string IssueCommentUrl { get; set; }

		[DataMember(Name = "issue_events_url", EmitDefaultValue = false)]
		public string IssueEventsUrl { get; set; }

		[DataMember(Name = "issues_url", EmitDefaultValue = false)]
		public string IssuesUrl { get; set; }

		[DataMember(Name = "keys_url", EmitDefaultValue = false)]
		public string KeysUrl { get; set; }

		[DataMember(Name = "labels_url", EmitDefaultValue = false)]
		public string LabelsUrl { get; set; }

		[DataMember(Name = "languages_url", EmitDefaultValue = false)]
		public string LanguagesUrl { get; set; }

		[DataMember(Name = "merges_url", EmitDefaultValue = false)]
		public string MergesUrl { get; set; }

		[DataMember(Name = "milestones_url", EmitDefaultValue = false)]
		public string MilestonesUrl { get; set; }

		[DataMember(Name = "notifications_url", EmitDefaultValue = false)]
		public string NotificationsUrl { get; set; }

		[DataMember(Name = "pulls_url", EmitDefaultValue = false)]
		public string PullsUrl { get; set; }

		[DataMember(Name = "releases_url", EmitDefaultValue = false)]
		public string ReleasesUrl { get; set; }

		[DataMember(Name = "ssh_url", EmitDefaultValue = false)]
		public string SshUrl { get; set; }

		[DataMember(Name = "stargazers_url", EmitDefaultValue = false)]
		public string StargazersUrl { get; set; }

		[DataMember(Name = "statuses_url", EmitDefaultValue = false)]
		public string StatusesUrl { get; set; }

		[DataMember(Name = "subscribers_url", EmitDefaultValue = false)]
		public string SubscribersUrl { get; set; }

		[DataMember(Name = "subscription_url", EmitDefaultValue = false)]
		public string SubscriptionUrl { get; set; }

		[DataMember(Name = "tags_url", EmitDefaultValue = false)]
		public string TagsUrl { get; set; }

		[DataMember(Name = "teams_url", EmitDefaultValue = false)]
		public string TeamsUrl { get; set; }

		[DataMember(Name = "trees_url", EmitDefaultValue = false)]
		public string TreesUrl { get; set; }

		[DataMember(Name = "clone_url", EmitDefaultValue = false)]
		public string CloneUrl { get; set; }

		[DataMember(Name = "mirror_url", EmitDefaultValue = false)]
		public string MirrorUrl { get; set; }

		[DataMember(Name = "hooks_url", EmitDefaultValue = false)]
		public string HooksUrl { get; set; }

		[DataMember(Name = "svn_url", EmitDefaultValue = false)]
		public string SvnUrl { get; set; }

		[DataMember(Name = "homepage", EmitDefaultValue = false)]
		public string Homepage { get; set; }

		[DataMember(Name = "language", EmitDefaultValue = false)]
		public object Language { get; set; }

		[DataMember(Name = "forks_count", EmitDefaultValue = false)]
		public int ForksCount { get; set; }

		[DataMember(Name = "stargazers_count", EmitDefaultValue = false)]
		public int StargazersCount { get; set; }

		[DataMember(Name = "watchers_count", EmitDefaultValue = false)]
		public int WatchersCount { get; set; }

		[DataMember(Name = "size", EmitDefaultValue = false)]
		public int Size { get; set; }

		[DataMember(Name = "default_branch", EmitDefaultValue = false)]
		public string DefaultBranch { get; set; }

		[DataMember(Name = "open_issues_count", EmitDefaultValue = false)]
		public int OpenIssuesCount { get; set; }

		[DataMember(Name = "is_template", EmitDefaultValue = false)]
		public bool IsTemplate { get; set; }

		[DataMember(Name = "topics", EmitDefaultValue = false)]
		public string[] Topics { get; set; }

		[DataMember(Name = "has_issues", EmitDefaultValue = false)]
		public bool HasIssues { get; set; }

		[DataMember(Name = "has_projects", EmitDefaultValue = false)]
		public bool HasProjects { get; set; }

		[DataMember(Name = "has_wiki", EmitDefaultValue = false)]
		public bool HasWiki { get; set; }

		[DataMember(Name = "has_pages", EmitDefaultValue = false)]
		public bool HasPages { get; set; }

		[DataMember(Name = "has_downloads", EmitDefaultValue = false)]
		public bool HasDownloads { get; set; }

		[DataMember(Name = "has_discussions", EmitDefaultValue = false)]
		public bool HasDiscussions { get; set; }

		[DataMember(Name = "archived", EmitDefaultValue = false)]
		public bool Archived { get; set; }

		[DataMember(Name = "disabled", EmitDefaultValue = false)]
		public bool Disabled { get; set; }

		[DataMember(Name = "visibility", EmitDefaultValue = false)]
		public string Visibility { get; set; }

		[DataMember(Name = "pushed_at", EmitDefaultValue = false)]
		public string __PushedAt { get => PushedAt.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => PushedAt = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? PushedAt { get; set; }

		[DataMember(Name = "created_at", EmitDefaultValue = false)]
		public string __CreatedAt { get => CreatedAt.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreatedAt = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CreatedAt { get; set; }

		[DataMember(Name = "updated_at", EmitDefaultValue = false)]
		public string __UpdatedAt { get => UpdatedAt.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => UpdatedAt = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? UpdatedAt { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryPermissions Permissions { get; set; }

		[DataMember(Name = "security_and_analysis", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositorySecurityAndAnalysis SecurityAndAnalysis { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryOwner
	{
		[DataMember(Name = "login", EmitDefaultValue = false)]
		public string Login { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "node_id", EmitDefaultValue = false)]
		public string NodeId { get; set; }

		[DataMember(Name = "avatar_url", EmitDefaultValue = false)]
		public string AvatarUrl { get; set; }

		[DataMember(Name = "gravatar_id", EmitDefaultValue = false)]
		public string GravatarId { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "html_url", EmitDefaultValue = false)]
		public string HtmlUrl { get; set; }

		[DataMember(Name = "followers_url", EmitDefaultValue = false)]
		public string FollowersUrl { get; set; }

		[DataMember(Name = "following_url", EmitDefaultValue = false)]
		public string FollowingUrl { get; set; }

		[DataMember(Name = "gists_url", EmitDefaultValue = false)]
		public string GistsUrl { get; set; }

		[DataMember(Name = "starred_url", EmitDefaultValue = false)]
		public string StarredUrl { get; set; }

		[DataMember(Name = "subscriptions_url", EmitDefaultValue = false)]
		public string SubscriptionsUrl { get; set; }

		[DataMember(Name = "organizations_url", EmitDefaultValue = false)]
		public string OrganizationsUrl { get; set; }

		[DataMember(Name = "repos_url", EmitDefaultValue = false)]
		public string ReposUrl { get; set; }

		[DataMember(Name = "events_url", EmitDefaultValue = false)]
		public string EventsUrl { get; set; }

		[DataMember(Name = "received_events_url", EmitDefaultValue = false)]
		public string ReceivedEventsUrl { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "site_admin", EmitDefaultValue = false)]
		public bool SiteAdmin { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryPermissions
	{
		[DataMember(Name = "admin", EmitDefaultValue = false)]
		public bool Admin { get; set; }

		[DataMember(Name = "push", EmitDefaultValue = false)]
		public bool Push { get; set; }

		[DataMember(Name = "pull", EmitDefaultValue = false)]
		public bool Pull { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositorySecurityAndAnalysis
	{
		[DataMember(Name = "advanced_security", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositorySecurityAndAnalysisStatus AdvancedSecurity { get; set; }

		[DataMember(Name = "secret_scanning", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositorySecurityAndAnalysisStatus SecretScanning { get; set; }

		[DataMember(Name = "secret_scanning_push_protection", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositorySecurityAndAnalysisStatus SecretScanningPushProtection { get; set; }

		[DataMember(Name = "secret_scanning_non_provider_patterns", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositorySecurityAndAnalysisStatus SecretScanningNonProviderPatterns { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositorySecurityAndAnalysisStatus
	{
		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }
	}
}
