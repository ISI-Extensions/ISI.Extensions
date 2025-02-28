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

namespace ISI.Extensions.BitBucket.SerializableModels
{
	[DataContract]
	public class ListRepositoriesResponse
	{
		[DataMember(Name = "size", EmitDefaultValue = false)]
		public int Size { get; set; }

		[DataMember(Name = "page", EmitDefaultValue = false)]
		public int Page { get; set; }

		[DataMember(Name = "pagelen", EmitDefaultValue = false)]
		public int PageLen { get; set; }

		[DataMember(Name = "next", EmitDefaultValue = false)]
		public string Next { get; set; }

		[DataMember(Name = "previous", EmitDefaultValue = false)]
		public string Previous { get; set; }

		[DataMember(Name = "values", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepository[] Repositories { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepository
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "links", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLinks Links { get; set; }

		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "full_name", EmitDefaultValue = false)]
		public string FullName { get; set; }

		[DataMember(Name = "is_private", EmitDefaultValue = false)]
		public bool IsPrivate { get; set; }

		[DataMember(Name = "scm", EmitDefaultValue = false)]
		public string Scm { get; set; }

		[DataMember(Name = "owner", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryOwner Owner { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

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
		public ListRepositoriesResponseRepositoryProject Project { get; set; }

		[DataMember(Name = "mainbranch", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryMainBranch MainBranch { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Self { get; set; }

		[DataMember(Name = "html", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Html { get; set; }

		[DataMember(Name = "avatar", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Avatar { get; set; }

		[DataMember(Name = "pullrequests", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink PullRequests { get; set; }

		[DataMember(Name = "commits", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Commits { get; set; }

		[DataMember(Name = "forks", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Forks { get; set; }

		[DataMember(Name = "watchers", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Watchers { get; set; }

		[DataMember(Name = "downloads", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Downloads { get; set; }

		[DataMember(Name = "clone", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink[] Clone { get; set; }

		[DataMember(Name = "hooks", EmitDefaultValue = false)]
		public ListRepositoriesResponseRepositoryLink Hooks { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryLink
	{
		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryOwner
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryProject
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseRepositoryMainBranch
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}
}