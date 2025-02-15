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
	public class HistoryMetadata : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.HistoryMetadata>
	{
		public ISI.Extensions.Jira.HistoryMetadata Export()
		{
			return new()
			{
				Type = Type,
				Description = Description,
				DescriptionKey = DescriptionKey,
				ActivityDescription = ActivityDescription,
				ActivityDescriptionKey = ActivityDescriptionKey,
				EmailDescription = EmailDescription,
				EmailDescriptionKey = EmailDescriptionKey,
				Actor = Actor?.Export(),
				Generator = Generator?.Export(),
				Cause = Cause?.Export(),
			};
		}

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "descriptionKey", EmitDefaultValue = false)]
		public string DescriptionKey { get; set; }

		[DataMember(Name = "activityDescription", EmitDefaultValue = false)]
		public string ActivityDescription { get; set; }

		[DataMember(Name = "activityDescriptionKey", EmitDefaultValue = false)]
		public string ActivityDescriptionKey { get; set; }

		[DataMember(Name = "emailDescription", EmitDefaultValue = false)]
		public string EmailDescription { get; set; }

		[DataMember(Name = "emailDescriptionKey", EmitDefaultValue = false)]
		public string EmailDescriptionKey { get; set; }

		[DataMember(Name = "actor", EmitDefaultValue = false)]
		public HistoryMetadataParticipant Actor { get; set; }

		[DataMember(Name = "generator", EmitDefaultValue = false)]
		public HistoryMetadataParticipant Generator { get; set; }

		[DataMember(Name = "cause", EmitDefaultValue = false)]
		public HistoryMetadataParticipant Cause { get; set; }
	}
}
