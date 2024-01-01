#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using LOCALENTITIES = ISI.Extensions.Jenkins;

namespace ISI.Extensions.Jenkins.SerializableModels
{
	[DataContract]
	public class Build : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jenkins.Build>
	{
		public static Build ToSerializable(ISI.Extensions.Jenkins.Build source)
		{
			return source.NullCheckedConvert(value => new Build()
			{
				Actions = value.Actions.ToNullCheckedArray(BuildAction.ToSerializable),
				Number = value.Number,
				Url = value.Url,
				IsBuilding = value.IsBuilding,
				Duration = value.Duration,
				EstimatedDuration = value.EstimatedDuration,
				DisplayName = value.DisplayName,
				KeepLog = value.KeepLog,
				Id = value.Id,
				TimeStamp = value.TimeStamp,
				Result = value.Result,
				ChangeSet = value.ChangeSet.NullCheckedConvert(ChangeSet.ToSerializable),
				Artifacts = value.Artifacts.ToNullCheckedArray(Artifact.ToSerializable),
				Culprits = value.Culprits.ToNullCheckedArray(UserInformation.ToSerializable),
			});
		}

		public ISI.Extensions.Jenkins.Build Export()
		{
			return new()
			{
				Actions = Actions.ToNullCheckedArray(x => x.Export()),
				Number = Number,
				Url = Url,
				IsBuilding = IsBuilding,
				Duration = Duration,
				EstimatedDuration = EstimatedDuration,
				DisplayName = DisplayName,
				KeepLog = KeepLog,
				Id = Id,
				TimeStamp = TimeStamp,
				Result = Result,
				ChangeSet = ChangeSet.NullCheckedConvert(x => x.Export()),
				Artifacts = Artifacts.ToNullCheckedArray(x => x.Export()),
				Culprits = Culprits.ToNullCheckedArray(x => x.Export()),
			};
		}

		[DataMember(Name = "actions", EmitDefaultValue = false)]
		public BuildAction[] Actions { get; set; }

		[DataMember(Name = "number", EmitDefaultValue = false)]
		public int Number { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "building", EmitDefaultValue = false)]
		public bool IsBuilding { get; set; }

		[DataMember(Name = "duration", EmitDefaultValue = false)]
		public string __Duration { get => Duration.Formatted(TimeSpanExtensions.TimeSpanFormat.Default); set => Duration = value.ToTimeSpanNullable(); }
		[IgnoreDataMember]
		public TimeSpan? Duration { get; set; }

		[DataMember(Name = "estimatedDuration", EmitDefaultValue = false)]
		public string __EstimatedDuration { get => EstimatedDuration.Formatted(TimeSpanExtensions.TimeSpanFormat.Default); set => EstimatedDuration = value.ToTimeSpanNullable(); }
		[IgnoreDataMember]
		public TimeSpan? EstimatedDuration { get; set; }

		[DataMember(Name = "fullDisplayName", EmitDefaultValue = false)]
		public string DisplayName { get; set; }

		[DataMember(Name = "keepLog", EmitDefaultValue = false)]
		public bool KeepLog { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "timeStamp", EmitDefaultValue = false)]
		public string __TimeStamp { get => TimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => TimeStamp = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? TimeStamp { get; set; }

		[DataMember(Name = "result", EmitDefaultValue = false)]
		public string Result { get; set; }

		[DataMember(Name = "changeSet", EmitDefaultValue = false)]
		public ChangeSet ChangeSet { get; set; }

		[DataMember(Name = "artifacts", EmitDefaultValue = false)]
		public Artifact[] Artifacts { get; set; }

		[DataMember(Name = "culprits", EmitDefaultValue = false)]
		public UserInformation[] Culprits { get; set; }

		public string ConsoleTextUrl => this.Url + "consoleText";

		public string RestJsonUrl => this.Url + "api/json";

		public string StopRestUrl => this.Url + "stop";

		public string TestRestJsonUrl => this.Url + "testReport/api/json";
	}
}