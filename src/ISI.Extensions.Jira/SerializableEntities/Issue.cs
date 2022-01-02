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

namespace ISI.Extensions.Jira.SerializableEntities
{
	[DataContract]
	public partial class Issue : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.Issue>
	{
		public ISI.Extensions.Jira.Issue Export()
		{
			return new ISI.Extensions.Jira.Issue()
			{
				IssueId = IssueId,
				IssueKey = IssueKey,
				Expand = Expand,
				IssueUrl = IssueUrl,
				Fields = Fields?.Export(),
				Transitions = Transitions.ToNullCheckedArray(x => x?.Export()),
				Operations = Operations?.Export(),
				Changelog = Changelog?.Export(),
				FieldsToInclude = FieldsToInclude,
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public int IssueId { get; set; }

		[DataMember(Name = "key", EmitDefaultValue = false)]
		public string IssueKey { get; set; }

		[DataMember(Name = "expand", EmitDefaultValue = false)]
		public string Expand { get; set; }

		[DataMember(Name = "self", EmitDefaultValue = false)]
		public string IssueUrl { get; set; }

		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public IssueFields Fields { get; set; }

		[DataMember(Name = "transitions", EmitDefaultValue = false)]
		public Transition[] Transitions { get; set; }

		[DataMember(Name = "operations", EmitDefaultValue = false)]
		public Operations Operations { get; set; }

		[DataMember(Name = "changelog", EmitDefaultValue = false)]
		public Changelog Changelog { get; set; }

		[DataMember(Name = "fieldsToInclude", EmitDefaultValue = false)]
		public string FieldsToInclude { get; set; }
	}
}