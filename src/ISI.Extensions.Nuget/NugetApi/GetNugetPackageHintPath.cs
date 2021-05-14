#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
		public DTOs.GetNugetPackageHintPathResponse GetNugetPackageHintPath(DTOs.GetNugetPackageHintPathRequest request)
		{
			var response = new DTOs.GetNugetPackageHintPathResponse();

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				var arguments = new List<string>();
				arguments.Add("install");
				arguments.Add(request.PackageId);
				arguments.Add("-DependencyVersion ignore");
				arguments.Add(string.Format("-Version {0}", request.PackageVersion));
				arguments.AddRange(GetConfigFileArguments(request.NugetConfigFullNames));

				var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = Logger,
					ProcessExeFullName = "nuget",
					Arguments = arguments.ToArray(),
					WorkingDirectory = tempDirectory.FullName,
				});

				if (!nugetResponse.Errored)
				{
					//var packageFullName = System.IO.Path.Combine(tempDirectory.FullName, string.Format("{0}.{1}", request.PackageId, request.PackageVersion));
					var packageFullName = System.IO.Directory.GetDirectories(tempDirectory.FullName).First();

					var dlls = System.IO.Directory.GetFiles(packageFullName, "*.dll", System.IO.SearchOption.AllDirectories)
						.Select(fullName => fullName.Substring(packageFullName.Length).Trim('\\'));

					foreach (var dllPrefix in new[]
					{
						"lib\\net48\\",
						"lib\\net4.8\\",
						"lib\\net472\\",
						"lib\\net4.72\\",
						"lib\\net461\\",
						"lib\\net4.61\\",
						"lib\\net46\\",
						"lib\\net4.6\\",
						"lib\\net40\\",
						"lib\\net4.0\\",
						"lib\\net35\\",
						"lib\\net3.5\\",
						"lib\\net20\\",
						"lib\\net2.0\\",
						"lib\\netstandard2.0\\",
					})
					{
						foreach (var dll in dlls)
						{
							if (string.IsNullOrWhiteSpace(response.HintPath) && dll.StartsWith(dllPrefix, StringComparison.InvariantCultureIgnoreCase))
							{
								response.HintPath = string.Format("{0}\\{1}", System.IO.Path.GetFileName(packageFullName), dll.Replace("/", "\\"));
							}
						}
					}
				}
			}

			return response;
		}
	}
}