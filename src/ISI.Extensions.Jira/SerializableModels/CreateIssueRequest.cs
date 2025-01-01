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

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public class CreateIssueRequest
	{
		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public CreateIssueFields Fields { get; set; }
	}

	[DataContract]
	public class CreateIssueFields
	{
		[DataMember(Name = "project", EmitDefaultValue = false)]
		public CreateIssueProject Project { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "issuetype", EmitDefaultValue = false)]
		public CreateIssueIssueType IssueType { get; set; }

		[DataMember(Name = "assignee", EmitDefaultValue = false)]
		public CreateIssueUser Assignee { get; set; }

		[DataMember(Name = "reporter", EmitDefaultValue = false)]
		public CreateIssueUser Reporter { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public CreateIssuePriority Priority { get; set; }

		[DataMember(Name = "labels", EmitDefaultValue = false)]
		public string[] Labels { get; set; }

		[DataMember(Name = "security", EmitDefaultValue = false)]
		public CreateIssueSecurity Security { get; set; }

		[DataMember(Name = "versions", EmitDefaultValue = false)]
		public CreateIssueVersion[] Versions { get; set; }

		[DataMember(Name = "environment", EmitDefaultValue = false)]
		public string Environment { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "duedate", EmitDefaultValue = false)]
		public string __DueDate { get => (DueDate.HasValue ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffzz}00", (DueDate?.Kind == DateTimeKind.Local ? DueDate : DueDate?.ToLocalTime())) : string.Empty); set => DueDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? DueDate { get; set; }

		[DataMember(Name = "fixVersions", EmitDefaultValue = false)]
		public CreateIssueFixVersion[] FixVersions { get; set; }

		[DataMember(Name = "components", EmitDefaultValue = false)]
		public CreateIssueComponent[] Components { get; set; }
	}

	[DataContract]
	public class CreateIssueProject
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long ProjectId { get; set; }
	}

	[DataContract]
	public class CreateIssueIssueType
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long IssueTypeId { get; set; }
	}

	[DataContract]
	public class CreateIssueUser
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class CreateIssuePriority
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long PriorityId { get; set; }
	}

	[DataContract]
	public class CreateIssueSecurity
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long SecurityId { get; set; }
	}

	[DataContract]
	public class CreateIssueVersion
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long VersionId { get; set; }
	}

	[DataContract]
	public class CreateIssueFixVersion
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long FixVersionId { get; set; }
	}

	[DataContract]
	public class CreateIssueComponent
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long ComponentId { get; set; }
	}
}