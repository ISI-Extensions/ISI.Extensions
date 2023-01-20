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

namespace ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi
{
	public partial class PackageComponentsRequest
	{
		public string Configuration { get; set; }
		public MSBuildVersion BuildVersion { get; set; } = MSBuildVersion.Automatic;
		public MSBuildPlatform BuildPlatform { get; set; } = MSBuildPlatform.Automatic;
		public BuildPlatformTarget PlatformTarget { get; set; } = BuildPlatformTarget.MSIL;
		public MSBuildVerbosity BuildVerbosity { get; set; } = MSBuildVerbosity.Quiet;

		public string Solution { get; set; }
		public string SubDirectory { get; set; }
		public IPackageComponent[] PackageComponents { get; set; }

		public string PackageFullName { get; set; }
		public string PackageName { get; set; }
		public string PackageVersion { get; set; }
		public string PackageBuildDateTimeStamp { get; set; }

		public AssemblyVersionFileDictionary AssemblyVersionFiles { get; set; }

		public bool CleanupTempDirectories { get; set; } = true;

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}