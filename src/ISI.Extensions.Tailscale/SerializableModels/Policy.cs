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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Tailscale.SerializableModels
{
	[DataContract]
	public class Policy
	{
		[DataMember(Name = "groups", EmitDefaultValue = false)]
		public Dictionary<string, string[]> Groups { get; set; }

		[DataMember(Name = "tagOwners", EmitDefaultValue = false)]
		public Dictionary<string, string[]> TagOwners { get; set; }

		[DataMember(Name = "hosts", EmitDefaultValue = false)]
		public Dictionary<string, string> Hosts { get; set; }

		[DataMember(Name = "acls", EmitDefaultValue = false)]
		public PolicyAccessControlList[] AccessControlLists { get; set; }
	}

	[DataContract]
	public class PolicyAccessControlList
	{
		[DataMember(Name = "action", EmitDefaultValue = false)]
		public string Action { get; set; }

		[DataMember(Name = "src", EmitDefaultValue = false)]
		public string[] Source { get; set; }

		[DataMember(Name = "dst", EmitDefaultValue = false)]
		public string[] Destination { get; set; }

		[DataMember(Name = "proto", EmitDefaultValue = false)]
		public string Proto { get; set; }
	}
}
