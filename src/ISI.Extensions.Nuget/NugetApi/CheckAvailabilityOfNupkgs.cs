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
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.CheckAvailabilityOfNupkgsResponse CheckAvailabilityOfNupkgs(DTOs.CheckAvailabilityOfNupkgsRequest request)
		{
			var response = new DTOs.CheckAvailabilityOfNupkgsResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			if (string.IsNullOrWhiteSpace(request.Source))
			{
				logger.LogInformation("Check Availability Of Nupkgs");
			}
			else
			{
				logger.LogInformation($"Check Availability Of Nupkgs using Source: {request.Source}");
			}

			var nupkgAvailabilities = new List<(NugetPackageKey NugetPackageKey, bool Available)>();

			foreach (var nugetPackageKey in request.NugetPackageKeys)
			{
				logger.LogInformation($"Checking for {nugetPackageKey.Package} {nugetPackageKey.Version}");

				var available = false;

				var foundNugetPackageKeys = SearchNugetPackageKeys(new()
				{
					Search = nugetPackageKey.Package,
					ExactMatchOnly = true,
					Source = request.Source,
				}).NugetPackageKeys ?? [];

				foreach (var foundNugetPackageKey in foundNugetPackageKeys)
				{
					if (string.Equals(foundNugetPackageKey.Package, nugetPackageKey.Package, StringComparison.InvariantCultureIgnoreCase))
					{
						if (string.Equals(foundNugetPackageKey.Version, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
						{
							available = true;

							logger.LogInformation($"  {nugetPackageKey.Package} {nugetPackageKey.Version} available");
						}
					}
				}

				nupkgAvailabilities.Add((NugetPackageKey: nugetPackageKey, Available: available));
			}

			response.NupkgAvailabilities = nupkgAvailabilities.ToArray();

			return response;
		}
	}
}