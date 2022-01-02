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

namespace ISI.Extensions.Scm.DataTransferObjects.JenkinsServiceApi
{
	public partial class UpdateNugetPackagesRequest
	{
		public string SettingsFullName { get; set; }

		public string JenkinsServiceUrl { get; set; }
		public string JenkinsServicePassword { get; set; }

		public string JenkinsUrl { get; set; }
		public string JenkinsUserName { get; set; }
		public string JenkinsApiToken { get; set; }

		public bool PauseJenkins { get; set; }
		public string PauseJenkinsReason { get; set; } = "Updating Nuget Packages";

		public bool FinishAllJobsBeforeProcessing { get; set; }
		public string FinishAllJobsBeforeProcessingFilterByJobIdPrefix { get; set; }
		public string FinishAllJobsBeforeProcessingFilterByJobIdSuffix { get; set; }
		public string[] FinishAllJobsBeforeProcessingExceptForJobIds { get; set; }

		public string[] JobIds { get; set; }
		public string FilterByJobIdPrefix { get; set; }
		public string FilterByJobIdSuffix { get; set; }

		public string[] IgnorePackageIds { get; set; }
		public NugetPackageKey[] NugetPackageKeys { get; set; }
		public NugetPackageKey[] UpsertAssemblyRedirectsNugetPackageKeys { get; set; }
	}
}