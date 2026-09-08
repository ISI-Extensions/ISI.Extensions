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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GitHub.SerializableModels
{
	[DataContract]
	public class RepositoryCommit
	{
		[DataMember(Name = "sha", EmitDefaultValue = false)]
		public string Sha { get; set; }

		[DataMember(Name = "node_id", EmitDefaultValue = false)]
		public string NodeId { get; set; }

		[DataMember(Name = "commit", EmitDefaultValue = false)]
		public RepositoryCommitCommit Commit { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "html_url", EmitDefaultValue = false)]
		public string HtmlUrl { get; set; }

		[DataMember(Name = "comments_url", EmitDefaultValue = false)]
		public string CommentsUrl { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public RepositoryCommitAuthor Author { get; set; }

		[DataMember(Name = "committer", EmitDefaultValue = false)]
		public RepositoryCommitCommitter Committer { get; set; }

		[DataMember(Name = "parents", EmitDefaultValue = false)]
		public RepositoryCommitParent[] Parents { get; set; }

		[DataMember(Name = "stats", EmitDefaultValue = false)]
		public RepositoryCommitStats Stats { get; set; }

		[DataMember(Name = "files", EmitDefaultValue = false)]
		public RepositoryCommitFile[] Files { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommit
	{
		[DataMember(Name = "author", EmitDefaultValue = false)]
		public RepositoryCommitCommitAuthor Author { get; set; }

		[DataMember(Name = "committer", EmitDefaultValue = false)]
		public RepositoryCommitCommitCommitter Committer { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }

		[DataMember(Name = "tree", EmitDefaultValue = false)]
		public RepositoryCommitCommitTree Tree { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "comment_count", EmitDefaultValue = false)]
		public int CommentCount { get; set; }

		[DataMember(Name = "verification", EmitDefaultValue = false)]
		public RepositoryCommitCommitVerification Verification { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommitAuthor
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "date", EmitDefaultValue = false)]
		public string __CommitDate { get => CommitDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CommitDate = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitDate { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommitCommitter
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "date", EmitDefaultValue = false)]
		public string __CommitDate { get => CommitDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CommitDate = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitDate { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommitTree
	{
		[DataMember(Name = "sha", EmitDefaultValue = false)]
		public string Sha { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommitVerification
	{
		[DataMember(Name = "verified", EmitDefaultValue = false)]
		public bool Verified { get; set; }

		[DataMember(Name = "reason", EmitDefaultValue = false)]
		public string Reason { get; set; }

		[DataMember(Name = "signature", EmitDefaultValue = false)]
		public object Signature { get; set; }

		[DataMember(Name = "payload", EmitDefaultValue = false)]
		public object Payload { get; set; }

		[DataMember(Name = "verified_at", EmitDefaultValue = false)]
		public object VerifiedAt { get; set; }
	}

	[DataContract]
	public class RepositoryCommitAuthor
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

		[DataMember(Name = "user_view_type", EmitDefaultValue = false)]
		public string UserViewType { get; set; }

		[DataMember(Name = "site_admin", EmitDefaultValue = false)]
		public bool SiteAdmin { get; set; }
	}

	[DataContract]
	public class RepositoryCommitCommitter
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

		[DataMember(Name = "user_view_type", EmitDefaultValue = false)]
		public string UserViewType { get; set; }

		[DataMember(Name = "site_admin", EmitDefaultValue = false)]
		public bool SiteAdmin { get; set; }
	}

	[DataContract]
	public class RepositoryCommitStats
	{
		[DataMember(Name = "total", EmitDefaultValue = false)]
		public int Total { get; set; }

		[DataMember(Name = "additions", EmitDefaultValue = false)]
		public int Additions { get; set; }

		[DataMember(Name = "deletions", EmitDefaultValue = false)]
		public int Deletions { get; set; }
	}

	[DataContract]
	public class RepositoryCommitParent
	{
		[DataMember(Name = "sha", EmitDefaultValue = false)]
		public string Sha { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "html_url", EmitDefaultValue = false)]
		public string HtmlUrl { get; set; }
	}

	[DataContract]
	public class RepositoryCommitFile
	{
		[DataMember(Name = "sha", EmitDefaultValue = false)]
		public string Sha { get; set; }

		[DataMember(Name = "filename", EmitDefaultValue = false)]
		public string Filename { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public string Status { get; set; }

		[DataMember(Name = "additions", EmitDefaultValue = false)]
		public int Additions { get; set; }

		[DataMember(Name = "deletions", EmitDefaultValue = false)]
		public int Deletions { get; set; }

		[DataMember(Name = "changes", EmitDefaultValue = false)]
		public int Changes { get; set; }

		[DataMember(Name = "blob_url", EmitDefaultValue = false)]
		public string BlobUrl { get; set; }

		[DataMember(Name = "raw_url", EmitDefaultValue = false)]
		public string RawUrl { get; set; }

		[DataMember(Name = "contents_url", EmitDefaultValue = false)]
		public string ContentsUrl { get; set; }

		[DataMember(Name = "patch", EmitDefaultValue = false)]
		public string Patch { get; set; }
	}
}