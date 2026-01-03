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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jira.SerializableModels
{
	[DataContract]
	public class Sprint : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.Sprint>
	{
		public ISI.Extensions.Jira.Sprint Export()
		{
			return new ISI.Extensions.Jira.Sprint()
			{
				SprintId = SprintId,
				State = State,
				Name = Name,
				Url = Url,
				StartDate = StartDate,
				EndDate = EndDate,
				CompleteDate = CompleteDate,
				OriginBoardId = OriginBoardId,
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public long SprintId { get; set; }

		[DataMember(Name = "state", EmitDefaultValue = false)]
		public string State { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "self", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "startDate", EmitDefaultValue = false)]
		public string __StartDate { get => (StartDate.HasValue ? $"{(StartDate.Value.Kind == DateTimeKind.Local ? StartDate : StartDate.Value.ToLocalTime()):yyyy-MM-ddTHH:mm:ss.fffzz}00" : string.Empty); set => StartDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? StartDate { get; set; }

		[DataMember(Name = "endDate", EmitDefaultValue = false)]
		public string __EndDate { get => (EndDate.HasValue ? $"{(EndDate.Value.Kind == DateTimeKind.Local ? EndDate : EndDate.Value.ToLocalTime()):yyyy-MM-ddTHH:mm:ss.fffzz}00" : string.Empty); set => EndDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? EndDate { get; set; }

		[DataMember(Name = "completeDate", EmitDefaultValue = false)]
		public string __CompleteDate { get => (CompleteDate.HasValue ? $"{(CompleteDate.Value.Kind == DateTimeKind.Local ? CompleteDate : CompleteDate.Value.ToLocalTime()):yyyy-MM-ddTHH:mm:ss.fffzz}00" : string.Empty); set => CompleteDate = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? CompleteDate { get; set; }

		[DataMember(Name = "originBoardId", EmitDefaultValue = false)]
		public long OriginBoardId { get; set; }
	}
}
