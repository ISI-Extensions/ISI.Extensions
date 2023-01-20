#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Scm.SerializableModels.DeploymentManagerApi
{
	[DataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("f55216f7-6435-4603-8fa3-ec280b56e4a2")]
	public class DeployWebSiteResponse : IDeployComponentResponse
	{
		public ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.IDeployComponentResponse Export()
		{
			return new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployWebSiteResponse()
			{
				DeployToSubfolder = DeployToSubfolder,
				PackageFolder = PackageFolder,
				SameVersion = SameVersion,
				InUse = InUse,
				NewInstall = NewInstall,
				Installed = Installed,
				Success = Success,
				Log = Log,
			};
		}

		[DataMember(Name = "deployToSubfolder", EmitDefaultValue = false)]
		public string DeployToSubfolder { get; set; }

		[DataMember(Name = "packageFolder", EmitDefaultValue = false)]
		public string PackageFolder { get; set; }

		[DataMember(Name = "sameVersion", EmitDefaultValue = false)]
		public bool SameVersion { get; set; }

		[DataMember(Name = "inUse", EmitDefaultValue = false)]
		public bool InUse { get; set; }

		[DataMember(Name = "newInstall", EmitDefaultValue = false)]
		public bool NewInstall { get; set; }

		[DataMember(Name = "installed", EmitDefaultValue = false)]
		public bool Installed { get; set; }

		[DataMember(Name = "success", EmitDefaultValue = false)]
		public bool Success { get; set; }

		[DataMember(Name = "log", EmitDefaultValue = false)]
		public string Log { get; set; }
	}
}
