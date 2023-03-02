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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public class SprintIssueFields : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.SprintIssueFields>
	{
		public ISI.Extensions.Jira.SprintIssueFields Export()
		{
			return new ISI.Extensions.Jira.SprintIssueFields()
			{
				Flagged = Flagged,
				Sprint = Sprint?.Export(),
				ClosedSprints = ClosedSprints.ToNullCheckedArray(x => x.Export()),
				Description = Description,
				Summary = Summary,
				Project = Project?.Export(),
				Comment = Comments?.Comment.ToNullCheckedArray(x => x.Export()),
				Epic = Epic?.Export(),
				Worklog = Worklog?.Worklog.ToNullCheckedArray(x => x.Export()),
				Updated = Updated,
			};
		}

		[DataMember(Name = "flagged", EmitDefaultValue = false)]
		public bool Flagged { get; set; }

		[DataMember(Name = "sprint", EmitDefaultValue = false)]
		public SprintIssueSprint Sprint { get; set; }

		[DataMember(Name = "closedSprints", EmitDefaultValue = false)]
		public Sprint[] ClosedSprints { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "project", EmitDefaultValue = false)]
		public SprintIssueProject Project { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public Comments Comments { get; set; }

		[DataMember(Name = "epic", EmitDefaultValue = false)]
		public Epic Epic { get; set; }

		[DataMember(Name = "worklog", EmitDefaultValue = false)]
		public Worklogs Worklog { get; set; }

		[DataMember(Name = "updated", EmitDefaultValue = false)]
		public string __Updated { get => (Updated.HasValue ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffzz}00", (Updated?.Kind == DateTimeKind.Local ? Updated : Updated?.ToLocalTime())) : string.Empty); set => Updated = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? Updated { get; set; }
	}
}
