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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.NupkgPackResponse NupkgPack(DTOs.NupkgPackRequest request)
		{
			var response = new DTOs.NupkgPackResponse();
			
			Logger.LogInformation(string.Format("Packing \"{0}\"", System.IO.Path.GetFileName(request.NuspecFullName)));

			if (string.IsNullOrWhiteSpace(request.OutputDirectory))
			{
				request.OutputDirectory = System.IO.Path.GetDirectoryName(request.NuspecFullName);
			}

			if ((System.Environment.GetEnvironmentVariable("NUGET_ENABLE_LEGACY_CSPROJ_PACK") ?? string.Empty).ToBoolean())
			{
				var arguments = new List<string>();
				arguments.Add("pack");
				arguments.Add(string.Format("\"{0}\"", request.NuspecFullName));
				arguments.Add(string.Format("-OutputDirectory \"{0}\"", request.OutputDirectory));
				if (request.IncludeSymbols)
				{
					arguments.Add("--include-symbols");
				}

				if (request.IncludeSource)
				{
					arguments.Add(string.Format("--include-source \"{0}\"", request.WorkingDirectory));
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
			else
			{
				if (string.IsNullOrWhiteSpace(request.CsProjFullName))
				{
					request.CsProjFullName = string.Format("{0}.csproj", request.NuspecFullName.TrimEnd(".nuspec"));
				}
				
				//dotnet pack  .\sample.csproj  -p:NuspecFile=.\nuget\check.nuspec  -p:NuspecBasePath=.\temp /p:Outputpath=package /p:PackageVersion=2.0.0-Dev
				var arguments = new List<string>();
				arguments.Add("pack");
				arguments.Add(string.Format("\"{0}\"", request.CsProjFullName));
				arguments.Add("--configuration Release");
				arguments.Add("--no-build");
				arguments.Add(string.Format("-p:NuspecFile=\"{0}\"", request.NuspecFullName));
				arguments.Add(string.Format("-p:Outputpath=\"{0}\"", request.OutputDirectory));
				if (request.IncludeSymbols)
				{
					arguments.Add("--include-symbols");
				}

				if (request.IncludeSource)
				{
					arguments.Add(string.Format("--include-source \"{0}\"", request.WorkingDirectory));
				}

				ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = Logger, //new NullLogger(),
					WorkingDirectory = request.WorkingDirectory,
					ProcessExeFullName = "dotnet",
					Arguments = arguments,
				});
			}

			Logger.LogInformation(string.Format("Packed \"{0}\"", System.IO.Path.GetFileName(request.NuspecFullName)));

			return response;
		}
	}
}