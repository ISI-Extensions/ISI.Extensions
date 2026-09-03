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
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels.Nuget;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.SearchNugetPackageKeysResponse SearchNugetPackageKeys(DTOs.SearchNugetPackageKeysRequest request)
		{
			var response = new DTOs.SearchNugetPackageKeysResponse();

			var cacheContext = new NuGet.Protocol.Core.Types.SourceCacheContext()
			{
				NoCache = true,
			};

			NugetPackageKey[] getNugetPackageKeys(NuGet.Protocol.Core.Types.SourceRepository sourceRepository, TimeSpan? cooldownTimeSpan)
			{
				var maxPublishDateTimeOffset = DateTimeOffset.Now - (cooldownTimeSpan ?? TimeSpan.Zero);

				var nugetPackageKeys = new List<NugetPackageKey>();

				void addNugetPackageKeys(IEnumerable<NuGet.Protocol.Core.Types.IPackageSearchMetadata> packageSearchMetadata)
				{
					foreach (var package in packageSearchMetadata)
					{
						if ((cooldownTimeSpan == null) || package.Published.GetValueOrDefault() < maxPublishDateTimeOffset)
						{
							nugetPackageKeys.Add(new NugetPackageKey()
							{
								Package = package.Identity.Id,
								Version = package.Identity.Version.ToString(),
							});
						}
					}
				}
				
				if (string.IsNullOrWhiteSpace(request.Search))
				{
					var searchResource = sourceRepository.GetResourceAsync<NuGet.Protocol.Core.Types.PackageSearchResource>().GetAwaiter().GetResult();

					var searchFilter = new NuGet.Protocol.Core.Types.SearchFilter(includePrerelease: false);
					var skip = 0;
					var take = 50;

					while (skip >= 0)
					{
						var packageSearchMetadata = searchResource.SearchAsync("", searchFilter, skip, take, new NuGet.Common.NullLogger(), System.Threading.CancellationToken.None).GetAwaiter().GetResult().ToNullCheckedArray();
						if (packageSearchMetadata.Any())
						{
							skip = (packageSearchMetadata.Length < take) ? -1 : skip + take;
							addNugetPackageKeys(packageSearchMetadata);
						}
						else
						{
							skip = -1;
						}
					}
				}
				else
				{
					var packageMetadataResource = sourceRepository.GetResource<NuGet.Protocol.Core.Types.PackageMetadataResource>();

					var packageSearchMetadata = packageMetadataResource.GetMetadataAsync(
							request.Search,
							includePrerelease: false,
							includeUnlisted: false,
							cacheContext,
							new NuGet.Common.NullLogger(),
							System.Threading.CancellationToken.None
						)
						.GetAwaiter()
						.GetResult()
						.ToNullCheckedArray();

					if (packageSearchMetadata.NullCheckedAny())
					{
						addNugetPackageKeys(packageSearchMetadata);
					}
				}

				return nugetPackageKeys.ToNullCheckedArray(NullCheckCollectionResult.Empty);
			}


			if (!response.NugetPackageKeys.NullCheckedAny() && !string.IsNullOrWhiteSpace(request.Source))
			{
				if (System.IO.Directory.Exists(request.Source))
				{
					var sourceRepository = NuGet.Protocol.Core.Types.Repository.Factory.GetCoreV2(new NuGet.Configuration.PackageSource(request.Source));

					response.NugetPackageKeys = getNugetPackageKeys(sourceRepository, null);
				}
				else
				{
					var nuGetSourceProviders = NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3();

					var sourceRepository = new NuGet.Protocol.Core.Types.SourceRepository(new NuGet.Configuration.PackageSource(request.Source), nuGetSourceProviders);

					response.NugetPackageKeys = getNugetPackageKeys(sourceRepository, null);
				}
			}

			if (!response.NugetPackageKeys.NullCheckedAny() && request.NugetConfigFullNames.NullCheckedAny())
			{
				var packageSources = GetPackageSourcesFromConfigFileArguments(request.NugetConfigFullNames);

				foreach (var packageSource in packageSources)
				{
					if (!response.NugetPackageKeys.NullCheckedAny())
					{
						var sourceRepository = (packageSource.ProtocolVersion.ToInt() == 3 ? new NuGet.Protocol.Core.Types.SourceRepository(new NuGet.Configuration.PackageSource(packageSource.Url, packageSource.Key), NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3()) : NuGet.Protocol.Core.Types.Repository.Factory.GetCoreV2(new NuGet.Configuration.PackageSource(packageSource.Url, packageSource.Key)));

						response.NugetPackageKeys = getNugetPackageKeys(sourceRepository, packageSource.CooldownTimeSpan);
					}
				}
			}

			if (!response.NugetPackageKeys.NullCheckedAny())
			{
				var nuGetSourceProviders = NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3();

				var sourceRepository = new NuGet.Protocol.Core.Types.SourceRepository(new NuGet.Configuration.PackageSource("https://api.nuget.org/v3/index.json"), nuGetSourceProviders);

				response.NugetPackageKeys = getNugetPackageKeys(sourceRepository, null);
			}

			response.NugetPackageKeys ??= [];

			return response;
		}
	}
}