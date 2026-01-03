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
		public DTOs.GenerateNupkgSPDXResponse GenerateNupkgSPDX(DTOs.GenerateNupkgSPDXRequest request)
		{
			var response = new DTOs.GenerateNupkgSPDXResponse();
			
			var sBomToolExeFullName = GetSBomToolExeFullName(new()).SBomToolExeFullName;

			ISI.Extensions.Locks.FileLock.Lock(sBomToolExeFullName, () =>
			{
				var projectDirectory = System.IO.Path.GetDirectoryName(request.ProjectFullName);

				var projectLines = System.IO.File.ReadAllLines(request.ProjectFullName);

				var csProjXml = System.Xml.Linq.XElement.Parse(string.Join("\n", projectLines));

				var targetFramework = GetTargetFrameworkVersionFromCsProjXml(csProjXml);

				var nupgkComponentDirectory = System.IO.Path.Combine(projectDirectory, "bin", request.Configuration, targetFramework);
				if (targetFramework.StartsWith("net4", StringComparison.InvariantCultureIgnoreCase))
				{
					nupgkComponentDirectory = System.IO.Path.Combine(projectDirectory, "bin", request.Configuration);
				}

				var arguments = new List<string>();

				arguments.Add("/c");
				arguments.Add(sBomToolExeFullName);
				arguments.Add("generate");
				arguments.Add($"-b \"{nupgkComponentDirectory}\"");
				arguments.Add($"-bc \"{projectDirectory}\"");
				arguments.Add($"-pn \"{request.NupkgName}\"");
				arguments.Add($"-pv \"{request.NupkgVersion}\"");
				arguments.Add($"-ps \"{request.NupkgAuthor}\"");
				arguments.Add($"-nsb \"{request.NupkgNamespace}\"");

				var process = new System.Diagnostics.Process();
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.FileName = "cmd.exe";
				process.StartInfo.Arguments = string.Join(" ", arguments);
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.Start();
				process.WaitForExit();

				var output = $"{process.StandardOutput.ReadToEnd()}\n{process.StandardError.ReadToEnd()}";

				System.IO.Directory.CreateDirectory(System.IO.Path.Combine(nupgkComponentDirectory, "_manifest"));

				System.IO.File.WriteAllText(System.IO.Path.Combine(nupgkComponentDirectory, "_manifest", "tool-output.txt"), output);
			});

			return response;
		}

		public string GetTargetFrameworkVersionFromCsProjXml(System.Xml.Linq.XElement csProjXml)
		{
			var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

			if (sdkAttribute.StartsWith("Microsoft.NET"))
			{
				var targetFrameworkVersion = csProjXml
					.GetElementByLocalName("PropertyGroup")?
					.GetElementByLocalName("TargetFramework")?
					.Value;

				if (string.IsNullOrWhiteSpace(targetFrameworkVersion))
				{
					targetFrameworkVersion = "net5.0";
				}

				return targetFrameworkVersion;
			}
			else
			{
				var targetFrameworkVersion = csProjXml
					.GetElementByLocalName("PropertyGroup")?
					.GetElementByLocalName("TargetFrameworkVersion")?
					.Value;

				if (string.IsNullOrWhiteSpace(targetFrameworkVersion))
				{
					targetFrameworkVersion = "v4.8";
				}

				targetFrameworkVersion = $"net{ISI.Extensions.StringFormat.StringNumericOnly(targetFrameworkVersion).Replace(".", string.Empty)}";

				return targetFrameworkVersion;
			}
		}
	}
}