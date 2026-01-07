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
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels.Nuget;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.NupkgPackResponse NupkgPack(DTOs.NupkgPackRequest request)
		{
			var response = new DTOs.NupkgPackResponse();
			
			Logger.LogInformation($"Packing \"{System.IO.Path.GetFileName(request.NuspecFullName)}\"");
			//Logger.LogInformation(string.Format("  WorkingDirectory = \"{0}\"", request.WorkingDirectory));
			//Logger.LogInformation(string.Format("  OutputDirectory = \"{0}\"", request.OutputDirectory));

			var nuspecXml = System.Xml.Linq.XElement.Load(request.NuspecFullName);
			var metadataXml = nuspecXml.GetElementByLocalName("metadata");
			var package = metadataXml.GetElementByLocalName("id")?.Value;
			var version = metadataXml.GetElementByLocalName("version")?.Value;
			var nupkgFileName = $"{package}.{version}.nupkg";
			var nupkgFullName = System.IO.Path.Combine(request.OutputDirectory, nupkgFileName);
			
			if (string.IsNullOrWhiteSpace(request.OutputDirectory))
			{
				request.OutputDirectory = System.IO.Path.GetDirectoryName(request.NuspecFullName);
			}

			if (string.IsNullOrWhiteSpace(request.CsProjFullName))
			{
				request.CsProjFullName = $"{request.NuspecFullName.TrimEnd(".nuspec")}.csproj";
			}

			var useLegacyNuget = (System.Environment.GetEnvironmentVariable("NUGET_ENABLE_LEGACY_CSPROJ_PACK") ?? string.Empty).ToBoolean();

			if (!useLegacyNuget)
			{
				var csProjXml = System.Xml.Linq.XElement.Parse(System.IO.File.ReadAllText(request.CsProjFullName));

				var targetFramework = GetTargetFrameworkVersionFromCsProjXml(csProjXml);

				if (targetFramework.StartsWith("net4", StringComparison.InvariantCultureIgnoreCase))
				{
					useLegacyNuget = true;
				}
			}

			void legacyNugetPack()
			{
				var arguments = new List<string>();
				arguments.Add("pack");
				arguments.Add($"\"{request.NuspecFullName}\"");
				arguments.Add($"-OutputDirectory \"{request.OutputDirectory}\"");

				if (request.IncludeSymbols)
				{
					arguments.Add("--include-symbols");
				}

				if (request.IncludeSource)
				{
					arguments.Add($"--include-source \"{request.WorkingDirectory}\"");
				}

				ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = Logger, //new NullLogger(),
					WorkingDirectory = request.WorkingDirectory,
					EnvironmentVariables = new Dictionary<string, string>()
					{
						{ "NUGET_ENABLE_LEGACY_CSPROJ_PACK", "true" }
					},
					ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
					Arguments = arguments,
				});
			}

			if (useLegacyNuget)
			{
				legacyNugetPack();
			}
			else
			{
				//dotnet pack  .\sample.csproj  -p:NuspecFile=.\nuget\check.nuspec  -p:NuspecBasePath=.\temp /p:Outputpath=package /p:PackageVersion=2.0.0-Dev
				var arguments = new List<string>();
				arguments.Add("pack");
				arguments.Add($"\"{request.CsProjFullName}\"");
				//arguments.Add("--verbosity Diagnostic");
				arguments.Add("--configuration Release");
				arguments.Add("--no-build");
				arguments.Add($"-p:NuspecFile=\"{request.NuspecFullName}\"");
				arguments.Add($"-p:Outputpath=\"{request.OutputDirectory}\"");

				if (request.IncludeSymbols)
				{
					arguments.Add("--include-symbols");
				}

				if (request.IncludeSource)
				{
					arguments.Add($"--include-source \"{request.WorkingDirectory}\"");
				}

				ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = Logger, //new NullLogger(),
					WorkingDirectory = request.WorkingDirectory,
					ProcessExeFullName = "dotnet",
					Arguments = arguments,
				});

				if (!System.IO.File.Exists(nupkgFullName))
				{
					legacyNugetPack();
				}
			}

			if (System.IO.File.Exists(nupkgFullName))
			{
				Logger.LogInformation($"Packed \"{System.IO.Path.GetFileName(request.NuspecFullName)}\"");
			}

			return response;
		}
	}
}