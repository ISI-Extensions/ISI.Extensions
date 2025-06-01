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
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.LocallyCacheNupkgsResponse LocallyCacheNupkgs(DTOs.LocallyCacheNupkgsRequest request)
		{
			var response = new DTOs.LocallyCacheNupkgsResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			logger.LogInformation("Locally Caching NugetPackages");

			var nugetGlobalPackagesDirectory = GetNugetGlobalPackagesDirectory();

			var cachedNugetPackageKeys = new List<NugetPackageKey>();

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				foreach (var nupkgFullName in request.NupkgFullNames)
				{
					logger.LogInformation(string.Format("Locally Caching \"{0}\"", System.IO.Path.GetFileName(nupkgFullName)));

					var arguments = new List<string>();
					arguments.Add("add");
					arguments.Add(string.Format("\"{0}\"", nupkgFullName));
					arguments.Add(string.Format("-Source \"{0}\"", tempDirectory.FullName));

					var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger, //new NullLogger(),
						ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
						Arguments = arguments.ToArray(),
					});

					var responsePieces = nugetResponse.Output.Split(' ');
					if ((responsePieces.Length >= 3) && string.Equals(responsePieces[0], "Installed", StringComparison.InvariantCultureIgnoreCase))
					{
						cachedNugetPackageKeys.Add(new()
						{
							Package = responsePieces[1],
							Version = responsePieces[2],
						});
					}

					if (!string.IsNullOrWhiteSpace(nugetGlobalPackagesDirectory) && System.IO.Directory.Exists(nugetGlobalPackagesDirectory))
					{
						foreach (var sourceFullName in System.IO.Directory.GetFiles(tempDirectory.FullName, "*", System.IO.SearchOption.AllDirectories))
						{
							var relativeName = sourceFullName.Substring(tempDirectory.FullName.Length);

							var targetFullName = System.IO.Path.Combine(nugetGlobalPackagesDirectory, relativeName);

							if (!System.IO.File.Exists(targetFullName))
							{
								System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetFullName));

								System.IO.File.Copy(sourceFullName, targetFullName, true);
							}
						}
					}
				}
			}

			response.CachedNugetPackageKeys = cachedNugetPackageKeys.ToArray();


			return response;
		}
	}
}