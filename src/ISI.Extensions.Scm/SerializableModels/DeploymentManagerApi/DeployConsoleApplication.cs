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
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Scm.SerializableModels.DeploymentManagerApi
{
	[DataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("7300740d-30db-4297-ab26-416cc6d043e0")]
	public class DeployConsoleApplication : IDeployComponent
	{
		[DataMember(Name = "pauseComponentUrl", EmitDefaultValue = false)]
		public string PauseComponentUrl { get; set; }

		[DataMember(Name = "checkComponentCanDeployStatusUrl", EmitDefaultValue = false)]
		public string CheckComponentCanDeployStatusUrl { get; set; }

		[DataMember(Name = "checkComponentCanDeployStatusIntervalInSeconds", EmitDefaultValue = false)]
		public double? __CheckComponentCanDeployStatusIntervalInSeconds { get => CheckComponentCanDeployStatusInterval?.TotalSeconds; set => CheckComponentCanDeployStatusInterval = (value > 0 ? TimeSpan.FromSeconds(value.Value) : null); }
		[IgnoreDataMember]
		public TimeSpan? CheckComponentCanDeployStatusInterval { get; set; }

		[DataMember(Name = "checkComponentCanDeployStatusTimeoutInSeconds", EmitDefaultValue = false)]
		public double? __CheckComponentCanDeployStatusTimeoutInSeconds { get => CheckComponentCanDeployStatusTimeout?.TotalSeconds; set => CheckComponentCanDeployStatusTimeout = (value > 0 ? TimeSpan.FromSeconds(value.Value) : null); }
		[IgnoreDataMember]
		public TimeSpan? CheckComponentCanDeployStatusTimeout { get; set; }

		[DataMember(Name = "deployToSubfolder", EmitDefaultValue = false)]
		public string DeployToSubfolder { get; set; }

		[DataMember(Name = "packageFolder", EmitDefaultValue = false)]
		public string PackageFolder { get; set; }

		[DataMember(Name = "excludeFiles", EmitDefaultValue = false)]
		public DeployComponentExcludeFile[] ExcludeFiles { get; set; }

		[DataMember(Name = "consoleApplicationExe", EmitDefaultValue = false)]
		public string ConsoleApplicationExe { get; set; }

		[DataMember(Name = "executeConsoleApplicationAfterInstall", EmitDefaultValue = false)]
		public bool ExecuteConsoleApplicationAfterInstall { get; set; }

		[DataMember(Name = "executeConsoleApplicationAfterInstallArguments", EmitDefaultValue = false)]
		public string ExecuteConsoleApplicationAfterInstallArguments { get; set; }
	}
}
