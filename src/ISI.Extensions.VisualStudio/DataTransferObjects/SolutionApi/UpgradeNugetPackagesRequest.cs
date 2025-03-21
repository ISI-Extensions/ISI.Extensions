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

namespace ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi
{
	public delegate IDisposable GetBuildServiceSolutionLockDelegate(string solutionFullName, ISI.Extensions.StatusTrackers.AddToLog addToLog);

	public delegate void UpgradeNugetPackagesPreActionDelegate(string solutionFullName);
	public delegate void UpgradeNugetPackagesSetStatusDelegate(string solutionFullName, string description);
	public delegate void UpgradeNugetPackagesPostActionDelegate(string solutionFullName);
	public delegate void UpgradeNugetPackagesBuildScriptErrorDelegate(string solutionFullName);

	public class UpgradeNugetPackagesRequest
	{
		public bool UpdateWorkingCopyFromSourceControl { get; set; } = true;
		public bool CommitWorkingCopyToSourceControl { get; set; } = true;

		public IEnumerable<string> SolutionFullNames { get; set; }

		public bool ConvertToPackageReferences { get; set; }

		public IEnumerable<string> IgnorePackageIds { get; set; }

		public ISI.Extensions.Nuget.NugetPackageKeyDictionary NugetPackageKeys { get; set; }

		public IEnumerable<ISI.Extensions.Nuget.NugetPackageKey> UpsertAssemblyRedirectsNugetPackageKeys { get; set; }

		public IEnumerable<string> RemoveAssemblyRedirects { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }

		public UpgradeNugetPackagesPreActionDelegate PreAction { get; set; }
		public UpgradeNugetPackagesSetStatusDelegate SetStatus { get; set; }
		public UpgradeNugetPackagesPostActionDelegate PostAction { get; set; }

		public GetBuildServiceSolutionLockDelegate GetBuildServiceSolutionLock { get; set; } = null;

		public UpgradeNugetPackagesBuildScriptErrorDelegate BuildScriptError { get; set; }
		public bool ContinueOnBuildScriptError { get; set; }

		public System.Threading.CancellationToken CancellationToken { get; set; } = default;
	}
}