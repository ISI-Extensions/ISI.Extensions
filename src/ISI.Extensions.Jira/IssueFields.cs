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

namespace ISI.Extensions.Jira
{
	public class IssueFields
	{
		public IssueType IssueType { get; set; }
		public string Timespent { get; set; }
		public Project Project { get; set; }
		public IssueFixVersion[] FixVersions { get; set; }
		public string AggregateTimespent { get; set; }
		public IssueResolution Resolution { get; set; }
		public DateTime? ResolutionDate { get; set; }
		public long WorkRatio { get; set; }
		public DateTime? LastViewed { get; set; }
		public InwardIssueFieldsWatches Watches { get; set; }
		public DateTime Created { get; set; }
		public Priority Priority { get; set; }
		public string[] Labels { get; set; }
		public string TimeEstimate { get; set; }
		public string AggregateTimeOriginalEstimate { get; set; }
		public IssueLink[] IssueLinks { get; set; }
		public User Assignee { get; set; }
		public DateTime? Updated { get; set; }
		public Status Status { get; set; }
		public IssueComponent[] Components { get; set; }
		public string TimeOriginalEstimate { get; set; }
		public string Description { get; set; }
		public string AggregateTimeEstimate { get; set; }
		public string Summary { get; set; }
		public User Creator { get; set; }
		public User Reporter { get; set; }
		public InwardIssueFieldsProgress AggregateProgress { get; set; }
		public DateTime? DueDate { get; set; }
		public InwardIssueFieldsProgress Progress { get; set; }
		public InwardIssueFieldsVotes Votes { get; set; }
		public DateTime? StatusCategoryChangeDate { get; set; }
		public Attachment[] Attachments { get; set; }
	}
}



