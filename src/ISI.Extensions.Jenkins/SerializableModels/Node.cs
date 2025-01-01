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
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;
using LOCALENTITIES = ISI.Extensions.Jenkins;

namespace ISI.Extensions.Jenkins.SerializableModels
{
	[DataContract]
	public class Node : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jenkins.Node>
	{
		public static Node ToSerializable(ISI.Extensions.Jenkins.Node source)
		{
			return source.NullCheckedConvert(value => new Node()
			{
				Mode = value.Mode,
				SlaveAgentPort = value.SlaveAgentPort,
				NodeDescription = value.NodeDescription,
				NodeName = value.NodeName,
				NumExecutors = value.NumExecutors,
				Description = value.Description,
				Jobs = value.Jobs.ToNullCheckedArray(Job.ToSerializable),
				IsQuietingDown = value.IsQuietingDown,
				PrimaryView = value.PrimaryView.NullCheckedConvert(View.ToSerializable),
				UseSecurity = value.UseSecurity,
				UseCrumbs = value.UseCrumbs,
				Views = value.Views.ToNullCheckedArray(View.ToSerializable),
				AssignedLabels = value.AssignedLabels.ToNullCheckedArray(AssignedLabel.ToSerializable),
			});
		}

		public ISI.Extensions.Jenkins.Node Export()
		{
			return new()
			{
				Mode = Mode,
				SlaveAgentPort = SlaveAgentPort,
				NodeDescription = NodeDescription,
				NodeName = NodeName,
				NumExecutors = NumExecutors,
				Description = Description,
				Jobs = Jobs.ToNullCheckedArray(x => x.Export()),
				IsQuietingDown = IsQuietingDown,
				PrimaryView = PrimaryView.NullCheckedConvert(x => x.Export()),
				UseSecurity = UseSecurity,
				UseCrumbs = UseCrumbs,
				Views = Views.ToNullCheckedArray(x => x.Export()),
				AssignedLabels = AssignedLabels.ToNullCheckedArray(x => x.Export()),
			};
		}

		[DataMember(Name = "_class", EmitDefaultValue = false)]
		public string _class { get; set; }

		[DataMember(Name = "mode", EmitDefaultValue = false)]
		public string Mode { get; set; }

		[DataMember(Name = "slaveAgentPort", EmitDefaultValue = false)]
		public int SlaveAgentPort { get; set; }

		[DataMember(Name = "nodeDescription", EmitDefaultValue = false)]
		public string NodeDescription { get; set; }

		[DataMember(Name = "nodeName", EmitDefaultValue = false)]
		public string NodeName { get; set; }

		[DataMember(Name = "numExecutors", EmitDefaultValue = false)]
		public int NumExecutors { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "jobs", EmitDefaultValue = false)]
		public Job[] Jobs { get; set; }

		[DataMember(Name = "quietingdown", EmitDefaultValue = false)]
		public bool IsQuietingDown { get; set; }

		[DataMember(Name = "primaryView", EmitDefaultValue = false)]
		public View PrimaryView { get; set; }

		[DataMember(Name = "useSecurity", EmitDefaultValue = false)]
		public bool UseSecurity { get; set; }

		[DataMember(Name = "useCrumbs", EmitDefaultValue = false)]
		public bool UseCrumbs { get; set; }

		[DataMember(Name = "views", EmitDefaultValue = false)]
		public View[] Views { get; set; }

		[DataMember(Name = "assignedLabels", EmitDefaultValue = false)]
		public AssignedLabel[] AssignedLabels { get; set; }
	}
}
