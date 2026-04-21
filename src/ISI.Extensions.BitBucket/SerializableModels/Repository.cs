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

namespace ISI.Extensions.BitBucket.SerializableModels
{
	[DataContract]
	public class Repository
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "links", EmitDefaultValue = false)]
		public RepositoryLinks Links { get; set; }

		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "full_name", EmitDefaultValue = false)]
		public string FullName { get; set; }

		[DataMember(Name = "is_private", EmitDefaultValue = false)]
		public bool IsPrivate { get; set; }

		[DataMember(Name = "scm", EmitDefaultValue = false)]
		public string Scm { get; set; }

		[DataMember(Name = "owner", EmitDefaultValue = false)]
		public RepositoryOwner Owner { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string RepositoryKey { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "created_on", EmitDefaultValue = false)]
		public string __creationDate { get => CreationDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => CreationDate = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CreationDate { get; set; }

		[DataMember(Name = "updated_on", EmitDefaultValue = false)]
		public string __lastModified { get => LastModified.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => LastModified = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? LastModified { get; set; }

		[DataMember(Name = "size", EmitDefaultValue = false)]
		public int Size { get; set; }

		[DataMember(Name = "language", EmitDefaultValue = false)]
		public string Language { get; set; }

		[DataMember(Name = "has_issues", EmitDefaultValue = false)]
		public bool HasIssues { get; set; }

		[DataMember(Name = "has_wiki", EmitDefaultValue = false)]
		public bool HasWiki { get; set; }

		[DataMember(Name = "fork_policy", EmitDefaultValue = false)]
		public string ForkPolicy { get; set; }

		[DataMember(Name = "project", EmitDefaultValue = false)]
		public RepositoryProject Project { get; set; }

		[DataMember(Name = "mainbranch", EmitDefaultValue = false)]
		public RepositoryMainBranch MainBranch { get; set; }
	}

	[DataContract]
	public class RepositoryLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public RepositoryLink Self { get; set; }

		[DataMember(Name = "html", EmitDefaultValue = false)]
		public RepositoryLink Html { get; set; }

		[DataMember(Name = "avatar", EmitDefaultValue = false)]
		public RepositoryLink Avatar { get; set; }

		[DataMember(Name = "pullrequests", EmitDefaultValue = false)]
		public RepositoryLink PullRequests { get; set; }

		[DataMember(Name = "commits", EmitDefaultValue = false)]
		public RepositoryLink Commits { get; set; }

		[DataMember(Name = "forks", EmitDefaultValue = false)]
		public RepositoryLink Forks { get; set; }

		[DataMember(Name = "watchers", EmitDefaultValue = false)]
		public RepositoryLink Watchers { get; set; }

		[DataMember(Name = "downloads", EmitDefaultValue = false)]
		public RepositoryLink Downloads { get; set; }

		[DataMember(Name = "clone", EmitDefaultValue = false)]
		public RepositoryLink[] Clone { get; set; }

		[DataMember(Name = "hooks", EmitDefaultValue = false)]
		public RepositoryLink Hooks { get; set; }
	}

	[DataContract]
	public class RepositoryLink
	{
		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class RepositoryOwner
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class RepositoryProject
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class RepositoryMainBranch
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}
}