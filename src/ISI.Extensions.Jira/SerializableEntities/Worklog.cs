#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Jira.SerializableEntities
{
	[DataContract]
	public class Worklog : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.Worklog>
	{
		public ISI.Extensions.Jira.Worklog Export()
		{
			return new ISI.Extensions.Jira.Worklog()
			{
				IssueId = IssueId,
				WorklogId = WorklogId,
				Self = Self,
				Author = Author?.Export(),
				UpdateAuthor = UpdateAuthor?.Export(),
				Comment = Comment,
				Created = Created.ToDateTime(),
				Updated = Updated.ToDateTime(),
				Started = Started.ToDateTime(),
				TimeSpent = TimeSpan.FromSeconds(TimeSpentSeconds),
				Visibility = Visibility?.Export(),
			};
		}

		[DataMember(Name = "issueId", EmitDefaultValue = false)]
		public int IssueId { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int WorklogId { get; set; }

		[DataMember(Name = "self", EmitDefaultValue = false)]
		public string Self { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public User Author { get; set; }

		[DataMember(Name = "updateAuthor", EmitDefaultValue = false)]
		public User UpdateAuthor { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string Created { get; set; }

		[DataMember(Name = "updated", EmitDefaultValue = false)]
		public string Updated { get; set; }

		[DataMember(Name = "started", EmitDefaultValue = false)]
		public string Started { get; set; }

		[DataMember(Name = "timeSpent", EmitDefaultValue = false)]
		public string TimeSpent { get; set; }

		[DataMember(Name = "timeSpentSeconds", EmitDefaultValue = false)]
		public int TimeSpentSeconds { get; set; }

		[DataMember(Name = "visibility", EmitDefaultValue = false)]
		public Visibility Visibility { get; set; }
	}
}
