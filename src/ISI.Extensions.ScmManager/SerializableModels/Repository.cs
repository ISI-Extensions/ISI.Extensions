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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class Repository
	{
		[DataMember(Name = "contact", EmitDefaultValue = false)]
		public string Contact { get; set; }

		[DataMember(Name = "creationDate", EmitDefaultValue = false)]
		public string __creationDate { get => CreationDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreationDate = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CreationDate { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "healthCheckFailures", EmitDefaultValue = false)]
		public object[] HealthCheckFailures { get; set; }

		[DataMember(Name = "namespace", EmitDefaultValue = false)]
		public string Namespace { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "archived", EmitDefaultValue = false)]
		public bool Archived { get; set; }

		[DataMember(Name = "exporting", EmitDefaultValue = false)]
		public bool Exporting { get; set; }

		[DataMember(Name = "healthCheckRunning", EmitDefaultValue = false)]
		public bool HealthCheckRunning { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public RepositoryLinks Links { get; set; }

		[DataMember(Name = "_embedded", EmitDefaultValue = false)]
		public RepositoryEmbedded Embedded { get; set; }

		[DataMember(Name = "lastModified", EmitDefaultValue = false)]
		public string __lastModified { get => LastModified.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => LastModified = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? LastModified { get; set; }
	}

	[DataContract]
	public class RepositoryLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public Link Self { get; set; }

		[DataMember(Name = "reindex", EmitDefaultValue = false)]
		public Link Reindex { get; set; }

		[DataMember(Name = "delete", EmitDefaultValue = false)]
		public Link Delete { get; set; }

		[DataMember(Name = "update", EmitDefaultValue = false)]
		public Link Update { get; set; }

		[DataMember(Name = "archive", EmitDefaultValue = false)]
		public Link Archive { get; set; }

		[DataMember(Name = "renameWithNamespace", EmitDefaultValue = false)]
		public Link RenameWithNamespace { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public Link Permissions { get; set; }

		[DataMember(Name = "protocol", EmitDefaultValue = false)]
		public RepositoryLinksProtocol[] Protocol { get; set; }

		[DataMember(Name = "export", EmitDefaultValue = false)]
		public Link Export { get; set; }

		[DataMember(Name = "fullExport", EmitDefaultValue = false)]
		public Link FullExport { get; set; }

		[DataMember(Name = "exportInfo", EmitDefaultValue = false)]
		public Link ExportInfo { get; set; }

		[DataMember(Name = "changesets", EmitDefaultValue = false)]
		public Link Changesets { get; set; }

		[DataMember(Name = "sources", EmitDefaultValue = false)]
		public Link Sources { get; set; }

		[DataMember(Name = "content", EmitDefaultValue = false)]
		public Link Content { get; set; }

		[DataMember(Name = "paths", EmitDefaultValue = false)]
		public Link Paths { get; set; }

		[DataMember(Name = "runHealthCheck", EmitDefaultValue = false)]
		public Link RunHealthCheck { get; set; }

		[DataMember(Name = "searchableTypes", EmitDefaultValue = false)]
		public Link SearchableTypes { get; set; }

		[DataMember(Name = "search", EmitDefaultValue = false)]
		public RepositoryLinksSearch[] Search { get; set; }

		[DataMember(Name = "updateAvatar", EmitDefaultValue = false)]
		public RepositoryLinksUpdateAvatar[] UpdateAvatar { get; set; }

		[DataMember(Name = "template", EmitDefaultValue = false)]
		public Link Template { get; set; }

		[DataMember(Name = "favorize", EmitDefaultValue = false)]
		public Link Favorize { get; set; }

		[DataMember(Name = "pathWpConfig", EmitDefaultValue = false)]
		public Link PathWpConfig { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public Link Tags { get; set; }

		[DataMember(Name = "branches", EmitDefaultValue = false)]
		public Link Branches { get; set; }

		[DataMember(Name = "branchDetailsCollection", EmitDefaultValue = false)]
		public Link BranchDetailsCollection { get; set; }

		[DataMember(Name = "incomingChangesets", EmitDefaultValue = false)]
		public Link IncomingChangesets { get; set; }

		[DataMember(Name = "incomingDiff", EmitDefaultValue = false)]
		public Link IncomingDiff { get; set; }

		[DataMember(Name = "incomingDiffParsed", EmitDefaultValue = false)]
		public Link IncomingDiffParsed { get; set; }

		[DataMember(Name = "branchWpConfig", EmitDefaultValue = false)]
		public Link BranchWpConfig { get; set; }

		[DataMember(Name = "defaultBranch", EmitDefaultValue = false)]
		public Link DefaultBranch { get; set; }

		[DataMember(Name = "pathWpConfigWithBranches", EmitDefaultValue = false)]
		public Link PathWpConfigWithBranches { get; set; }

		[DataMember(Name = "readme", EmitDefaultValue = false)]
		public Link Readme { get; set; }

		[DataMember(Name = "pullRequest", EmitDefaultValue = false)]
		public Link PullRequest { get; set; }

		[DataMember(Name = "pullRequestCheck", EmitDefaultValue = false)]
		public Link PullRequestCheck { get; set; }

		[DataMember(Name = "pullRequestConfig", EmitDefaultValue = false)]
		public Link PullRequestConfig { get; set; }

		[DataMember(Name = "pullRequestTemplate", EmitDefaultValue = false)]
		public Link PullRequestTemplate { get; set; }

		[DataMember(Name = "workflowConfig", EmitDefaultValue = false)]
		public Link WorkflowConfig { get; set; }

		[DataMember(Name = "configuration", EmitDefaultValue = false)]
		public Link Configuration { get; set; }
	}

	[DataContract]
	public class RepositoryLinksProtocol
	{
		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "profile", EmitDefaultValue = false)]
		public string Profile { get; set; }
	}

	[DataContract]
	public class RepositoryLinksSearch
	{
		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class RepositoryLinksUpdateAvatar
	{
		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class RepositoryEmbedded
	{
		[DataMember(Name = "avatar", EmitDefaultValue = false)]
		public Avatar Avatar { get; set; }
	}
}