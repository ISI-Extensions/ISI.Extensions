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
using DTOs = ISI.Extensions.Sbom.DataTransferObjects.SbomApi;

namespace ISI.Extensions.Sbom
{
	public partial class SbomApi
	{
		public DTOs.GenerateCycloneDXResponse GenerateCycloneDX(DTOs.GenerateCycloneDxRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.GenerateCycloneDXResponse();

			var arguments = new List<string>();

			arguments.Add($"\"{request.FullName}\"");

			if (!string.IsNullOrWhiteSpace(request.Framework))
			{
				arguments.Add("--framework");
				arguments.Add($"\"{request.Framework}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.Runtime))
			{
				arguments.Add("--runtime");
				arguments.Add($"\"{request.Runtime}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.OutputDirectory))
			{
				arguments.Add("--output");
				arguments.Add($"\"{request.OutputDirectory}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.OutputFilename))
			{
				arguments.Add("--filename");
				arguments.Add($"\"{request.OutputFilename}\"");
			}

			if (request.OutputJson)
			{
				arguments.Add("--json");
			}

			if (request.ExcludeDependencies.NullCheckedAny())
			{
				arguments.Add("--exclude-filter");
				arguments.Add($"\"{string.Join(",", request.ExcludeDependencies.Select(dependency => $"{dependency.Package}@{dependency.Version}"))}\"");
			}

			if (request.ExcludeDevelopmentDependencies)
			{
				arguments.Add("--exclude-dev");
			}

			if (request.ExcludeTestProjects)
			{
				arguments.Add("--exclude-test-projects");
			}

			if (!string.IsNullOrWhiteSpace(request.AlternativeNugetUrl))
			{
				arguments.Add("--url");
				arguments.Add($"\"{request.AlternativeNugetUrl}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.AlternativeNugetUserName))
			{
				arguments.Add("--baseUrlUsername");
				arguments.Add($"\"{request.AlternativeNugetUserName}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.AlternativeNugetPasswordApiKey))
			{
				arguments.Add("--baseUrlUserPassword");
				arguments.Add($"\"{request.AlternativeNugetPasswordApiKey}\"");
			}

			if (request.AlternativeNugetPasswordIsClearText)
			{
				arguments.Add("--isBaseUrlPasswordClearText");
			}

			if (request.Recursive)
			{
				arguments.Add("--recursive");
			}

			if (request.OmitSerialNumber)
			{
				arguments.Add("--no-serial-number");
			}

			if (!string.IsNullOrWhiteSpace(request.GitHubUserName))
			{
				arguments.Add("--github-username");
				arguments.Add($"\"{request.GitHubUserName}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.GitHubToken))
			{
				arguments.Add("--github-token");
				arguments.Add($"\"{request.GitHubToken}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.GitHubBearerToken))
			{
				arguments.Add("--github-bearer-token");
				arguments.Add($"\"{request.GitHubBearerToken}\"");
			}

			if (request.GitHubEnableLicenses)
			{
				arguments.Add("--enable-github-licenses");
			}

			if (request.DisablePackageRestore)
			{
				arguments.Add("--disable-package-restore");
			}

			if (request.DisableHashComputation)
			{
				arguments.Add("--disable-hash-computation");
			}

			if (request.DotnetCommandTimeout != null)
			{
				arguments.Add("--dotnet-command-timeout");
				arguments.Add($"\"{request.DotnetCommandTimeout.Value.Milliseconds}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.BaseIntermediateOutputPath))
			{
				arguments.Add("--base-intermediate-output-path");
				arguments.Add($"\"{request.BaseIntermediateOutputPath}\"");
			}

			if (!string.IsNullOrWhiteSpace(request.ImportMetadataPath))
			{
				arguments.Add("--import-metadata-path");
				arguments.Add($"\"{request.ImportMetadataPath}\"");
			}

			if (request.IncludeProjectPeferences)
			{
				arguments.Add("--include-project-references");
			}

			if (!string.IsNullOrWhiteSpace(request.SetName))
			{
				arguments.Add("--set-name");
				arguments.Add($"\"{request.SetName}\"");
			}

			if ((request.SetVersion != null) && (request.SetVersion != Version.Parse("0.0.0.0")))
			{
				arguments.Add("--set-version");
				arguments.Add($"\"{request.SetVersion}\"");
			}

			if (request.SetComponentType != null)
			{
				arguments.Add("--set-type");

				switch (request.SetComponentType)
				{
					case ComponentType.Application:
						arguments.Add("Application");
						break;
					case ComponentType.Container:
						arguments.Add("Container");
						break;
					case ComponentType.Data:
						arguments.Add("Data");
						break;
					case ComponentType.Device:
						arguments.Add("Device");
						break;
					case ComponentType.DeviceDriver:
						arguments.Add("Device_Driver");
						break;
					case ComponentType.File:
						arguments.Add("File");
						break;
					case ComponentType.Firmware:
						arguments.Add("Firmware");
						break;
					case ComponentType.Framework:
						arguments.Add("Framework");
						break;
					case ComponentType.Library:
						arguments.Add("Library");
						break;
					case ComponentType.MachineLearningModel:
						arguments.Add("Machine_Learning_Model");
						break;
					case ComponentType.None:
						arguments.Add("Null");
						break;
					case ComponentType.OperatingSystem:
						arguments.Add("Operating_System");
						break;
					case ComponentType.Platform:
						arguments.Add("Platform");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			if (!string.IsNullOrWhiteSpace(request.SetNugetPurl))
			{
				arguments.Add("--set-nuget-purl");
				arguments.Add($"\"{request.SetNugetPurl}\"");
			}

			var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = logger,
				ProcessExeFullName = "dotnet-CycloneDX",
				Arguments = arguments.ToArray(),
			});

			response.Output = waitForProcessResponse.Output;

			response.Errored = waitForProcessResponse.Errored;

			return response;
		}
	}
}