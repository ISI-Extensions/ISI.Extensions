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
		public DTOs.GetNugetServersResponse GetNugetServers(DTOs.GetNugetServersRequest request)
		{
			var response = new DTOs.GetNugetServersResponse();

			var sourcesResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new NullLogger(),
				WorkingDirectory = request.WorkingDirectory,
				ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
				Arguments =
				[
					"sources"
				]
			});

			if (!sourcesResponse.Errored)
			{
				var nugetServers = new List<NugetServer>();

				var lines = sourcesResponse.Output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();

				while (lines.Any())
				{
					var lineParts = lines.First().Split(['.'], 2);
					lines.RemoveAt(0);

					if (lineParts.First().Trim().ToInt() > 0)
					{
						var nameParts = lineParts[1].Trim().Split(['[', ']'], StringSplitOptions.RemoveEmptyEntries);
						var nugetServer = new NugetServer()
						{
							Name = nameParts.First().Trim(),
							Enabled = string.Equals(nameParts[1], "Enabled", StringComparison.InvariantCultureIgnoreCase),
						};

						var url = lines.First().Trim();
						lines.RemoveAt(0);

						nugetServer.Url = url;

						if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
						{
							if (url.IndexOf("/api/v2", StringComparison.InvariantCultureIgnoreCase) > 0)
							{
								nugetServer.DownloadNugetUrl = string.Format("{0}/package/", url.TrimEnd("/"));
							}
							else
							{
								var nugetServerIndex = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<ISI.Extensions.Nuget.SerializableModels.NugetServerIndexJson>(url, [], true);

								var v2Resource = nugetServerIndex.Resources.FirstOrDefault(resource => string.Equals(resource.Type, "LegacyGallery", StringComparison.InvariantCultureIgnoreCase));

								if (v2Resource != null)
								{
									nugetServer.DownloadNugetUrl = string.Format("{0}/package/", v2Resource.Url.TrimEnd("/"));
								}
							}
						}
						else
						{
							nugetServer.NugetCache = url;
						}

						nugetServers.Add(nugetServer);
					}
				}

				response.NugetServers = nugetServers.ToArray();
			}

			return response;
		}
	}
}