#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public partial class IssueFields : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.IssueFields>
	{
		public ISI.Extensions.Jira.IssueFields Export()
		{
			return new()
			{
				IssueType = IssueType?.Export(),
				Timespent = Timespent,
				Project = Project?.Export(),
				FixVersions = FixVersions.ToNullCheckedArray(x => x.Export()),
				AggregateTimespent = AggregateTimespent,
				Resolution = Resolution?.Export(),
				ResolutionDate = ResolutionDate.ToDateTimeNullable(),
				WorkRatio = WorkRatio,
				LastViewed = LastViewed.ToDateTimeNullable(),
				Watches = Watches?.Export(),
				Created = Created.ToDateTime(),
				Updated = Updated.ToDateTime(),
				Priority = Priority?.Export(),
				Labels = Labels.ToNullCheckedArray(),
				TimeEstimate = TimeEstimate,
				AggregateTimeOriginalEstimate = AggregateTimeOriginalEstimate,
				IssueLinks = IssueLinks.ToNullCheckedArray(x => x.Export()),
				Assignee = Assignee?.Export(),
				Status = Status?.Export(),
				Components = Components.ToNullCheckedArray(),
				TimeOriginalEstimate = TimeOriginalEstimate,
				Description = Description,
				AggregateTimeEstimate = AggregateTimeEstimate,
				Summary = Summary,
				Creator = Creator?.Export(),
				Reporter = Reporter?.Export(),
				AggregateProgress = AggregateProgress?.Export(),
				DueDate = DueDate,
				Progress = Progress?.Export(),
				Votes = Votes?.Export(),
				StatusCategoryChangeDate = StatusCategoryChangeDate,
				Attachments = Attachments.ToNullCheckedArray(x => x.Export()),
			};
		}

		[DataMember(Name = "issuetype", EmitDefaultValue = false)]
		public IssueType IssueType { get; set; }

		[DataMember(Name = "timespent", EmitDefaultValue = false)]
		public string Timespent { get; set; }

		[DataMember(Name = "project", EmitDefaultValue = false)]
		public Project Project { get; set; }

		[DataMember(Name = "fixVersions", EmitDefaultValue = false)]
		public IssueFixVersion[] FixVersions { get; set; }

		[DataMember(Name = "aggregatetimespent", EmitDefaultValue = false)]
		public string AggregateTimespent { get; set; }

		[DataMember(Name = "resolution", EmitDefaultValue = false)]
		public IssueResolution Resolution { get; set; }

		[DataMember(Name = "resolutiondate", EmitDefaultValue = false)]
		public string ResolutionDate { get; set; }

		[DataMember(Name = "workratio", EmitDefaultValue = false)]
		public int WorkRatio { get; set; }

		[DataMember(Name = "lastViewed", EmitDefaultValue = false)]
		public string LastViewed { get; set; }

		[DataMember(Name = "watches", EmitDefaultValue = false)]
		public InwardIssueFieldsWatches Watches { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string Created { get; set; }

		[DataMember(Name = "updated", EmitDefaultValue = false)]
		public string Updated { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public Priority Priority { get; set; }

		[DataMember(Name = "labels", EmitDefaultValue = false)]
		public string[] Labels { get; set; }

		[DataMember(Name = "timeestimate", EmitDefaultValue = false)]
		public string TimeEstimate { get; set; }

		[DataMember(Name = "aggregatetimeoriginalestimate", EmitDefaultValue = false)]
		public string AggregateTimeOriginalEstimate { get; set; }

		[DataMember(Name = "issuelinks", EmitDefaultValue = false)]
		public IssueLink[] IssueLinks { get; set; }

		[DataMember(Name = "assignee", EmitDefaultValue = false)]
		public User Assignee { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		public Status Status { get; set; }

		[DataMember(Name = "components", EmitDefaultValue = false)]
		public string[] Components { get; set; }

		[DataMember(Name = "timeoriginalestimate", EmitDefaultValue = false)]
		public string TimeOriginalEstimate { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "aggregatetimeestimate", EmitDefaultValue = false)]
		public string AggregateTimeEstimate { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "creator", EmitDefaultValue = false)]
		public User Creator { get; set; }

		[DataMember(Name = "reporter", EmitDefaultValue = false)]
		public User Reporter { get; set; }

		[DataMember(Name = "aggregateprogress", EmitDefaultValue = false)]
		public InwardIssueFieldsProgress AggregateProgress { get; set; }

		[DataMember(Name = "dueDate", EmitDefaultValue = false)]
		public string __DueDate { get => DueDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => DueDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? DueDate { get; set; }

		[DataMember(Name = "progress", EmitDefaultValue = false)]
		public InwardIssueFieldsProgress Progress { get; set; }

		[DataMember(Name = "votes", EmitDefaultValue = false)]
		public InwardIssueFieldsVotes Votes { get; set; }

		[DataMember(Name = "statuscategorychangedate", EmitDefaultValue = false)]
		public string __StatusCategoryChangeDate { get => StatusCategoryChangeDate.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => StatusCategoryChangeDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? StatusCategoryChangeDate { get; set; }

		//public object[] Versions { get; set; }
		//public object[] Subtasks { get; set; }
		//public object Security { get; set; }
		//public object Environment { get; set; }

		[DataMember(Name = "attachments", EmitDefaultValue = false)]
		public Attachment[] Attachments { get; set; }
	}
}



