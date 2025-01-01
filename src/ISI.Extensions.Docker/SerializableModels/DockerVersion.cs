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

namespace ISI.Extensions.Docker.SerializableModels
{
	[DataContract]
	public class DockerVersion
	{
		[DataMember(Name = "Client", EmitDefaultValue = false)]
		public DockerVersionClient Client { get; set; }

		[DataMember(Name = "Server", EmitDefaultValue = false)]
		public DockerVersionServer Server { get; set; }
	}

	[DataContract]
	public class DockerVersionClient
	{
		[DataMember(Name = "Version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "ApiVersion", EmitDefaultValue = false)]
		public string ApiVersion { get; set; }

		[DataMember(Name = "DefaultApiversion", EmitDefaultValue = false)]
		public string DefaultAPIVersion { get; set; }

		[DataMember(Name = "GitCommit", EmitDefaultValue = false)]
		public string GitCommit { get; set; }

		[DataMember(Name = "GoVersion", EmitDefaultValue = false)]
		public string GoVersion { get; set; }

		[DataMember(Name = "Os", EmitDefaultValue = false)]
		public string Os { get; set; }

		[DataMember(Name = "Arch", EmitDefaultValue = false)]
		public string Arch { get; set; }

		[DataMember(Name = "BuildTime", EmitDefaultValue = false)]
		public string BuildTime { get; set; }

		[DataMember(Name = "Context", EmitDefaultValue = false)]
		public string Context { get; set; }
	}

	[DataContract]
	public class DockerVersionServer
	{
		[DataMember(Name = "Platform", EmitDefaultValue = false)]
		public DockerVersionServerPlatform Platform { get; set; }

		[DataMember(Name = "Components", EmitDefaultValue = false)]
		public DockerVersionServerComponent[] Components { get; set; }

		[DataMember(Name = "Version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "ApiVersion", EmitDefaultValue = false)]
		public string ApiVersion { get; set; }

		[DataMember(Name = "MinApiversion", EmitDefaultValue = false)]
		public string MinAPIVersion { get; set; }

		[DataMember(Name = "GitCommit", EmitDefaultValue = false)]
		public string GitCommit { get; set; }

		[DataMember(Name = "GoVersion", EmitDefaultValue = false)]
		public string GoVersion { get; set; }

		[DataMember(Name = "Os", EmitDefaultValue = false)]
		public string Os { get; set; }

		[DataMember(Name = "Arch", EmitDefaultValue = false)]
		public string Arch { get; set; }

		[DataMember(Name = "KernelVersion", EmitDefaultValue = false)]
		public string KernelVersion { get; set; }

		[DataMember(Name = "BuildTime", EmitDefaultValue = false)]
		public string BuildTime { get; set; }
	}

	[DataContract]
	public class DockerVersionServerPlatform
	{
		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class DockerVersionServerComponent
	{
		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "Version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "Details", EmitDefaultValue = false)]
		public DockerVersionServerComponentDetails Details { get; set; }
	}

	[DataContract]
	public class DockerVersionServerComponentDetails
	{
		[DataMember(Name = "ApiVersion", EmitDefaultValue = false)]
		public string ApiVersion { get; set; }

		[DataMember(Name = "Arch", EmitDefaultValue = false)]
		public string Arch { get; set; }

		[DataMember(Name = "BuildTime", EmitDefaultValue = false)]
		public string BuildTime { get; set; }

		[DataMember(Name = "Experimental", EmitDefaultValue = false)]
		public string Experimental { get; set; }

		[DataMember(Name = "GitCommit", EmitDefaultValue = false)]
		public string GitCommit { get; set; }

		[DataMember(Name = "GoVersion", EmitDefaultValue = false)]
		public string GoVersion { get; set; }

		[DataMember(Name = "KernelVersion", EmitDefaultValue = false)]
		public string KernelVersion { get; set; }

		[DataMember(Name = "MinApiversion", EmitDefaultValue = false)]
		public string MinAPIVersion { get; set; }

		[DataMember(Name = "Os", EmitDefaultValue = false)]
		public string Os { get; set; }
	}
}