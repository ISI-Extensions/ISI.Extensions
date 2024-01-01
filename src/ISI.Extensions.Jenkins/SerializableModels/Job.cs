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
	public class Job : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jenkins.Job>
	{
		public static Job ToSerializable(ISI.Extensions.Jenkins.Job source)
		{
			return source.NullCheckedConvert(value => new Job()
			{
				Actions = value.Actions.ToNullCheckedArray(JobAction.ToSerializable),
				IsConcurrentBuild = value.IsConcurrentBuild,
				Properties = value.Properties.ToNullCheckedArray(JobProperty.ToSerializable),
				KeepDependencies = value.KeepDependencies,
				HealthReports = value.HealthReports.ToNullCheckedArray(HealthReport.ToSerializable),
				Name = value.Name,
				Url = value.Url,
				DisplayName = value.DisplayName,
				FirstBuild = value.FirstBuild.NullCheckedConvert(Build.ToSerializable),
				Description = value.Description,
				Buildable = value.Buildable,
				Builds = value.Builds.ToNullCheckedArray(Build.ToSerializable),
				IsInQueue = value.IsInQueue,
				Color = value.Color,
				LastBuild = value.LastBuild.NullCheckedConvert(Build.ToSerializable),
				LastCompletedBuild = value.LastCompletedBuild.NullCheckedConvert(Build.ToSerializable),
				LastFailedBuild = value.LastFailedBuild.NullCheckedConvert(Build.ToSerializable),
				LastStableBuild = value.LastStableBuild.NullCheckedConvert(Build.ToSerializable),
				LastSuccessfulBuild = value.LastSuccessfulBuild.NullCheckedConvert(Build.ToSerializable),
				LastUnstableBuild = value.LastUnstableBuild.NullCheckedConvert(Build.ToSerializable),
				LastUnsuccessfulBuild = value.LastUnsuccessfulBuild.NullCheckedConvert(Build.ToSerializable),
				NextBuildNumber = value.NextBuildNumber,
			});
		}

		public ISI.Extensions.Jenkins.Job Export()
		{
			return new()
			{
				Actions = Actions.ToNullCheckedArray(x => x.Export()),
				IsConcurrentBuild = IsConcurrentBuild,
				Properties = Properties.ToNullCheckedArray(x => x.Export()),
				KeepDependencies = KeepDependencies,
				HealthReports = HealthReports.ToNullCheckedArray(x => x.Export()),
				Name = Name,
				Url = Url,
				DisplayName = DisplayName,
				FirstBuild = FirstBuild.NullCheckedConvert(x => x.Export()),
				Description = Description,
				Buildable = Buildable,
				Builds = Builds.ToNullCheckedArray(x => x.Export()),
				IsInQueue = IsInQueue,
				Color = Color,
				LastBuild = LastBuild.NullCheckedConvert(x => x.Export()),
				LastCompletedBuild = LastCompletedBuild.NullCheckedConvert(x => x.Export()),
				LastFailedBuild = LastFailedBuild.NullCheckedConvert(x => x.Export()),
				LastStableBuild = LastStableBuild.NullCheckedConvert(x => x.Export()),
				LastSuccessfulBuild = LastSuccessfulBuild.NullCheckedConvert(x => x.Export()),
				LastUnstableBuild = LastUnstableBuild.NullCheckedConvert(x => x.Export()),
				LastUnsuccessfulBuild = LastUnsuccessfulBuild.NullCheckedConvert(x => x.Export()),
				NextBuildNumber = NextBuildNumber,
			};
		}

		[DataMember(Name = "_class", EmitDefaultValue = false)]
		public string _class { get; set; }

		[DataMember(Name = "actions", EmitDefaultValue = false)]
		public JobAction[] Actions { get; set; }

		[DataMember(Name = "concurrentBuild", EmitDefaultValue = false)]
		public bool IsConcurrentBuild { get; set; }

		[DataMember(Name = "property", EmitDefaultValue = false)]
		public JobProperty[] Properties { get; set; }

		[DataMember(Name = "keepDependencies", EmitDefaultValue = false)]
		public bool KeepDependencies { get; set; }

		[DataMember(Name = "healthReport", EmitDefaultValue = false)]
		public HealthReport[] HealthReports { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "displayName", EmitDefaultValue = false)]
		public string DisplayName { get; set; }

		[DataMember(Name = "firstBuild", EmitDefaultValue = false)]
		public Build FirstBuild { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "buildable", EmitDefaultValue = false)]
		public bool Buildable { get; set; }

		[DataMember(Name = "builds", EmitDefaultValue = false)]
		public Build[] Builds { get; set; }

		[DataMember(Name = "inQueue", EmitDefaultValue = false)]
		public bool IsInQueue { get; set; }

		[DataMember(Name = "color", EmitDefaultValue = false)]
		public string Color { get; set; }

		[DataMember(Name = "lastBuild", EmitDefaultValue = false)]
		public Build LastBuild { get; set; }

		[DataMember(Name = "lastCompletedBuild", EmitDefaultValue = false)]
		public Build LastCompletedBuild { get; set; }

		[DataMember(Name = "lastFailedBuild", EmitDefaultValue = false)]
		public Build LastFailedBuild { get; set; }

		[DataMember(Name = "lastStableBuild", EmitDefaultValue = false)]
		public Build LastStableBuild { get; set; }

		[DataMember(Name = "lastSuccessfulBuild", EmitDefaultValue = false)]
		public Build LastSuccessfulBuild { get; set; }

		[DataMember(Name = "lastUnstableBuild", EmitDefaultValue = false)]
		public Build LastUnstableBuild { get; set; }

		[DataMember(Name = "lastUnsuccessfulBuild", EmitDefaultValue = false)]
		public Build LastUnsuccessfulBuild { get; set; }

		[DataMember(Name = "nextBuildNumber", EmitDefaultValue = false)]
		public int NextBuildNumber { get; set; }

		public string BuildRestUrl => this.Url + "build";

		public string RestJsonUrl => this.Url + "api/json";
	}
}