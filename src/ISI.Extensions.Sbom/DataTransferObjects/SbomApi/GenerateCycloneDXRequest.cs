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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Sbom.DataTransferObjects.SbomApi
{
	public class GenerateCycloneDxRequest
	{
		public string FullName { get; set; }

		public string Framework { get; set; }
		public string Runtime { get; set; }

		public string OutputDirectory { get; set; }
		public string OutputFilename { get; set; }
		public bool OutputJson { get; set; }

		public GenerateCycloneDxRequestDependency[] ExcludeDependencies { get; set; }

		public bool ExcludeDevelopmentDependencies { get; set; }
		public bool ExcludeTestProjects { get; set; }

		public string AlternativeNugetUrl { get; set; }
		public string AlternativeNugetUserName { get; set; }
		public string AlternativeNugetPasswordApiKey { get; set; }
		public bool AlternativeNugetPasswordIsClearText { get; set; }

		public bool Recursive { get; set; }

		public bool OmitSerialNumber { get; set; }

		public string GitHubUserName { get; set; }
		public string GitHubToken { get; set; }
		public string GitHubBearerToken { get; set; }
		public bool GitHubEnableLicenses { get; set; }

		public bool DisablePackageRestore { get; set; }
		public bool DisableHashComputation { get; set; }
		public TimeSpan? DotnetCommandTimeout { get; set; }

		public string BaseIntermediateOutputPath { get; set; }
		public string ImportMetadataPath { get; set; }
		public bool IncludeProjectPeferences { get; set; }
		public string SetName { get; set; }
		public Version? SetVersion { get; set; }
		public ComponentType? SetComponentType { get; set; }
		public string SetNugetPurl { get; set; }


		/*
				-tfm, --framework <framework>                                                The target framework to use. If not defined, all will be aggregated.
			-rt, --runtime <runtime>                                                     The runtime to use. If not defined, all will be aggregated.
			-o, --output <output>                                                        The directory to write the BOM
			-fn, --filename <filename>                                                   Optionally provide a filename for the BOM (default: bom.xml or bom.json)
			-j, --json                                                                   Produce a JSON BOM instead of XML
			-ef, --exclude-filter <exclude-filter>                                       A comma separated list of dependencies to exclude in form 'name1@version1,name2@version2'. Transitive dependencies will also be removed.
			-ed, --exclude-dev                                                           Exclude development dependencies from the BOM (see https://github.com/NuGet/Home/wiki/DevelopmentDependency-support-for-PackageReference)
			-t, --exclude-test-projects                                                  Exclude test projects from the BOM
			-u, --url <url>                                                              Alternative NuGet repository URL to https://<yoururl>/nuget/<yourrepository>/v3/index.json
			-us, --baseUrlUsername <baseUrlUsername>                                     Alternative NuGet repository username
			-usp, --baseUrlUserPassword <baseUrlUserPassword>                            Alternative NuGet repository username password/apikey
			-uspct, --isBaseUrlPasswordClearText                                         Alternative NuGet repository password is cleartext
			-rs, --recursive                                                             To be used with a single project file, it will recursively scan project references of the supplied project file
			-ns, --no-serial-number                                                      Optionally omit the serial number from the resulting BOM

				-gu, --github-username <github-username>                                     Optionally provide a GitHub username for license resolution. If set you also need to provide a GitHub personal access token
			-gt, --github-token <github-token>                                           Optionally provide a GitHub personal access token for license resolution. If set you also need to provide a GitHub username
			-gbt, --github-bearer-token <github-bearer-token>                            Optionally provide a GitHub bearer token for license resolution. This is useful in GitHub actions
			-egl, --enable-github-licenses                                               Enables GitHub license resolution

				-dpr, --disable-package-restore                                              Optionally disable package restore
			-dhc, --disable-hash-computation                                             Optionally disable hash computation for packages
			-dct, --dotnet-command-timeout <dotnet-command-timeout>                      dotnet command timeout in milliseconds (primarily used for long dotnet restore operations) [default: 300000]
			-biop, --base-intermediate-output-path <base-intermediate-output-path>       Optionally provide a folder for customized build environment. Required if folder 'obj' is relocated.
			-imp, --import-metadata-path <import-metadata-path>                          Optionally provide a metadata template which has project specific details.
			-ipr, --include-project-references                                           Include project references as components (can only be used with project files).
			-sn, --set-name <set-name>                                                   Override the autogenerated BOM metadata component name.
			-sv, --set-version <set-version>                                             Override the default BOM metadata component version (defaults to 0.0.0).
			-st, --set-type   <Application|Container|Data|Device|Device_Driver|          Override the default BOM metadata component type (defaults to application). [default: Application]
												 File|Firmware|Framework|Library|
												 Machine_Learning_Model|Null|Operating_System|Platform>                                                                
			--set-nuget-purl                                                             Override the default BOM metadata component bom ref and PURL as NuGet package.
		*/

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}

	public class GenerateCycloneDxRequestDependency
	{
		public string Package { get; set; }
		public string Version { get; set; }
	}
}