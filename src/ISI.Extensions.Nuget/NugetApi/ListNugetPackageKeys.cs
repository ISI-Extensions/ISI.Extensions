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
		public DTOs.ListNugetPackageKeysResponse ListNugetPackageKeys(DTOs.ListNugetPackageKeysRequest request)
		{
			var response = new DTOs.ListNugetPackageKeysResponse();

			var arguments = new List<string>();

			arguments.Add("list");

			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				arguments.Add(request.Search);
			}

			if (!string.IsNullOrWhiteSpace(request.Source))
			{
				arguments.Add($"-Source \"{request.Source}\"");
			}
			if (request.NugetConfigFullNames.NullCheckedAny())
			{
				arguments.AddRange(GetSourcesFromConfigFileArguments(request.NugetConfigFullNames));
			}

			arguments.Add("-NonInteractive");

			var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new NullLogger(),
				ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
				Arguments = arguments.ToArray(),
			});

			if (!nugetResponse.Errored)
			{
				var nugetPackageKeys = new List<NugetPackageKey>();

				var lines = nugetResponse.Output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

				foreach (var line in lines)
				{
					var linePieces = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

					if (linePieces.Length == 2)
					{
						var package = linePieces[0].Trim();

						if (!request.ExactMatchOnly || string.Equals(package, request.Search, StringComparison.InvariantCultureIgnoreCase))
						{
							var nugetPackageKey = GetNugetPackageKey(new()
							{
								Package = package,
								Version = linePieces[1].Trim(),
								Source = request.Source,
							}).NugetPackageKey;

							if (nugetPackageKey != null)
							{
								nugetPackageKeys.Add(nugetPackageKey);
							}
						}
					}
				}

				response.NugetPackageKeys = nugetPackageKeys.ToArray();
			}

			return response;
		}
	}
}