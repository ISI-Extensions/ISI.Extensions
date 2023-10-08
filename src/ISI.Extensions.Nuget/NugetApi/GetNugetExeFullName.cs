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
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		private static string _nugetExeFullName = string.Empty;

		public DTOs.GetNugetExeFullNameResponse GetNugetExeFullName(DTOs.GetNugetExeFullNameRequest request)
		{
			var response = new DTOs.GetNugetExeFullNameResponse();

			const string nugetExeFileName = "nuget.exe";
			const string NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe";

			if (string.IsNullOrWhiteSpace(_nugetExeFullName))
			{
				if (ISI.Extensions.IO.Path.IsInEnvironmentPath(nugetExeFileName, out var nugetExeFullName))
				{
					_nugetExeFullName = nugetExeFullName;
				}
			}

			if (string.IsNullOrWhiteSpace(_nugetExeFullName))
			{
				if (ISI.Extensions.IO.Path.IsInEnvironmentPath("choco.exe", out var chocoExeFullName))
				{
					var processResponse = ISI.Extensions.Process.WaitForProcessResponse(chocoExeFullName, new[] { "install", "nuget.commandline", "-y" });
				}
			}

			if (string.IsNullOrWhiteSpace(_nugetExeFullName))
			{
				if (ISI.Extensions.IO.Path.IsInEnvironmentPath(nugetExeFileName, out var nugetExeFullName))
				{
					_nugetExeFullName = nugetExeFullName;
				}
			}

			if (string.IsNullOrWhiteSpace(_nugetExeFullName))
			{
				var uriCodeBase = new UriBuilder(GetType().Assembly.CodeBase);

				var directory = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uriCodeBase.Path));

				var nugetExeFullName = System.IO.Path.Combine(directory, nugetExeFileName);

				if (!System.IO.File.Exists(nugetExeFullName))
				{
					using (var webClient = new System.Net.WebClient())
					{
						webClient.DownloadFile(NUGET_URL, directory);
					}

					ISI.Extensions.IO.FileZone.RemoveZone(nugetExeFullName);
				}

				if (!string.IsNullOrWhiteSpace(nugetExeFullName) && System.IO.File.Exists(nugetExeFullName))
				{
					var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(nugetExeFullName);
					var productVersion = new System.Version(versionInfo.ProductVersion);
					var minProductionVersion = new System.Version("5.8.1");

					if (productVersion < minProductionVersion)
					{
						var processResponse = ISI.Extensions.Process.WaitForProcessResponse(nugetExeFullName, new[] { "update", "-self" });
					}
				}
			}

			response.NugetExeFullName = _nugetExeFullName;

			return response;
		}
	}
}