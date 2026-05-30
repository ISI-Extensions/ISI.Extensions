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
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeploy
	{
		[YamlMember(Alias = "replicas", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public int? Replicas { get; set; }

		[YamlMember(Alias = "mode", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Mode { get; set; }

		[YamlMember(Alias = "resources", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployResources Resources { get; set; }

		[YamlMember(Alias = "placement", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployPlacement Placement { get; set; }

		[YamlMember(Alias = "update_config", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployUpdateConfig UpdateConfig { get; set; }

		[YamlMember(Alias = "restart_policy", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployRestartPolicy RestartPolicy { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }
	}
}
