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
	public class ComposeFile
	{
		[YamlMember(Alias = "name", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Name { get; set; }

		[YamlMember(Alias = "version", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Version { get; set; }

		[YamlMember(Alias = "services", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileService> Services { get; set; }

		[YamlMember(Alias = "networks", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileNetwork> Networks { get; set; }

		[YamlMember(Alias = "volumes", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileVolume> Volumes { get; set; }

		[YamlMember(Alias = "secrets", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileSecret> Secrets { get; set; }

		[YamlMember(Alias = "configs", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileConfig> Configs { get; set; }

		[YamlMember(Alias = "extensions", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, object> Extensions { get; set; }
	}
}
