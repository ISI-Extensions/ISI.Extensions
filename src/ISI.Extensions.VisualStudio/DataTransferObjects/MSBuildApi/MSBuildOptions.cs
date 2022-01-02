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
using System.Text;

namespace ISI.Extensions.VisualStudio.DataTransferObjects.MSBuildApi
{
	public class MSBuildOptions
	{
		public HashSet<string> Targets { get; } = new(StringComparer.InvariantCultureIgnoreCase);
		public System.Collections.Specialized.NameValueCollection Properties { get; } = new(StringComparer.InvariantCultureIgnoreCase);

		public string Configuration { get; set; }

		public int? MaxCpuCount { get; set; }
		public bool? NodeReuse { get; set; }
		public bool? DetailedSummary { get; set; }
		public bool? NoConsoleLogger { get; set; }
		public bool? NoLogo { get; set; }

		public string Version
		{
			get => Properties.Get(nameof(Version));
			set => Properties.Add(nameof(Version), value);
		}

		public string VersionPrefix
		{
			get => Properties.Get(nameof(VersionPrefix));
			set => Properties.Add(nameof(VersionPrefix), value);
		}

		public string VersionSuffix
		{
			get => Properties.Get(nameof(VersionSuffix));
			set => Properties.Add(nameof(VersionSuffix), value);
		}

		public string FileVersion
		{
			get => Properties.Get(nameof(FileVersion));
			set => Properties.Add(nameof(FileVersion), value);
		}

		public string AssemblyVersion
		{
			get => Properties.Get(nameof(AssemblyVersion));
			set => Properties.Add(nameof(AssemblyVersion), value);
		}

		public string InformationalVersion
		{
			get => Properties.Get(nameof(InformationalVersion));
			set => Properties.Add(nameof(InformationalVersion), value);
		}

		public string PackageVersion
		{
			get => Properties.Get(nameof(PackageVersion));
			set => Properties.Add(nameof(PackageVersion), value);
		}

		public string PackageReleaseNotes
		{
			get => Properties.Get(nameof(PackageReleaseNotes));
			set => Properties.Add(nameof(PackageReleaseNotes), value);
		}

		public bool? ContinuousIntegrationBuild { get; set; }

		public bool? NoImplicitTarget { get; set; }

		public MSBuildVerbosity Verbosity { get; set; }

		public bool? IncludeSymbols { get; set; }
		public string SymbolPackageFormat { get; set; }

		public bool Restore { get; set; }
		public bool? RestoreLockedMode { get; set; }
	}
}
