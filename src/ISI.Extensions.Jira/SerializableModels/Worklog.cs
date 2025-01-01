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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public class Worklog : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.Worklog>
	{
		public ISI.Extensions.Jira.Worklog Export()
		{
			return new()
			{
				IssueId = IssueId,
				WorklogId = WorklogId,
				WorklogUrl = WorklogUrl,
				Author = Author?.Export(),
				UpdateAuthor = UpdateAuthor?.Export(),
				Comment = Comment,
				Created = Created,
				Updated = Updated,
				Started = Started.ToDateTime(),
				TimeSpent = TimeSpan.FromSeconds(TimeSpentSeconds),
				Visibility = Visibility?.Export(),
			};
		}

		[DataMember(Name = "issueId", EmitDefaultValue = false)]
		public string IssueId { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string WorklogId { get; set; }

		[DataMember(Name = "self", EmitDefaultValue = false)]
		public string WorklogUrl { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public User Author { get; set; }

		[DataMember(Name = "updateAuthor", EmitDefaultValue = false)]
		public User UpdateAuthor { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string __Created { get =>string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffzz}00", (Created.Kind == DateTimeKind.Local ? Created : Created.ToLocalTime())); set => Created = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Created { get; set; }

		[DataMember(Name = "updated", EmitDefaultValue = false)]
		public string __Updated { get => (Updated.HasValue ? string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffzz}00", (Updated?.Kind == DateTimeKind.Local ? Updated : Updated?.ToLocalTime())) : string.Empty); set => Updated = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? Updated { get; set; }

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
