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

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class ListRepositoryChangeSetsResponse
	{
		[DataMember(Name = "page", EmitDefaultValue = false)]
		public int Page { get; set; }

		[DataMember(Name = "pageTotal", EmitDefaultValue = false)]
		public int PageTotal { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public ListRepositoryChangeSetsResponseLinks Links { get; set; }

		[DataMember(Name = "_embedded", EmitDefaultValue = false)]
		public ListRepositoryChangeSetsResponseEmbedded Embedded { get; set; }
	}

	[DataContract]
	public class ListRepositoryChangeSetsResponseLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public Link Self { get; set; }

		[DataMember(Name = "first", EmitDefaultValue = false)]
		public Link First { get; set; }

		[DataMember(Name = "next", EmitDefaultValue = false)]
		public Link Next { get; set; }

		[DataMember(Name = "last", EmitDefaultValue = false)]
		public Link Last { get; set; }

		[DataMember(Name = "create", EmitDefaultValue = false)]
		public Link Create { get; set; }
	}

	[DataContract]
	public class ListRepositoryChangeSetsResponseEmbedded
	{
		[DataMember(Name = "changesets", EmitDefaultValue = false)]
		public RepositoryChangeSet[] Changesets { get; set; }

		[DataMember(Name = "branch", EmitDefaultValue = false)]
		public RepositoryBranch Branch { get; set; }
	}

	[DataContract]
	public class RepositoryBranch
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public Link Links { get; set; }
	}

	[DataContract]
	public class RepositoryChangeSet
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public Person Author { get; set; }

		[DataMember(Name = "date", EmitDefaultValue = false)]
		public string __date { get => Date.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => Date = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Date { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "contributors", EmitDefaultValue = false)]
		public Contributor[] Contributors { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public ChangeSetLinks Links { get; set; }

		[DataMember(Name = "_embedded", EmitDefaultValue = false)]
		public ChangeSetEmbedded Embedded { get; set; }
	}

	[DataContract]
	public class ChangeSetLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public Link Self { get; set; }

		[DataMember(Name = "diff", EmitDefaultValue = false)]
		public Link Diff { get; set; }

		[DataMember(Name = "sources", EmitDefaultValue = false)]
		public Link Sources { get; set; }

		[DataMember(Name = "modifications", EmitDefaultValue = false)]
		public Link Modifications { get; set; }

		[DataMember(Name = "tag", EmitDefaultValue = false)]
		public Link Tag { get; set; }

		[DataMember(Name = "containedInTags", EmitDefaultValue = false)]
		public Link ContainedInTags { get; set; }

		[DataMember(Name = "diffParsed", EmitDefaultValue = false)]
		public Link DiffParsed { get; set; }
	}

	[DataContract]
	public class ChangeSetEmbedded
	{
		[DataMember(Name = "parents", EmitDefaultValue = false)]
		public Parent[] Parents { get; set; }
	}

	[DataContract]
	public class Parent
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public ParentLinks Links { get; set; }
	}

	[DataContract]
	public class ParentLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public Link Self { get; set; }

		[DataMember(Name = "diff", EmitDefaultValue = false)]
		public Link Diff { get; set; }
	}

	[DataContract]
	public class Contributor
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "person", EmitDefaultValue = false)]
		public Person Person { get; set; }
	}

	[DataContract]
	public class Person
	{
		[DataMember(Name = "mail", EmitDefaultValue = false)]
		public string Mail { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}
}