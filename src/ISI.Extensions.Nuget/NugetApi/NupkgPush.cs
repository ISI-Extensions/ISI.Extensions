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
		public DTOs.NupkgPushResponse NupkgPush(DTOs.NupkgPushRequest request)
		{
			var response = new DTOs.NupkgPushResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			response.Success = true;

			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				var serviceLocatorDirectoryUrl = (string.IsNullOrWhiteSpace(request.RepositoryUri?.ToString()) ? request.RepositoryName : request.RepositoryUri?.ToString());

				var workingDirectory = string.IsNullOrWhiteSpace(request.WorkingDirectory) ? System.IO.Path.GetDirectoryName(request.NupkgFullNames.FirstOrDefault()) : request.WorkingDirectory;

				if(!Uri.TryCreate(serviceLocatorDirectoryUrl, UriKind.Absolute, out var serviceLocatorDirectoryUri))
				{
					var nugetConfigFullNames = GetNugetConfigFullNames(new()
					{
						WorkingCopyDirectory = workingDirectory,
					}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

					foreach (var nugetConfigFullName in nugetConfigFullNames)
					{
						if (serviceLocatorDirectoryUri == null)
						{
							var packageSources = GetPackageSourcesFromNugetConfig(new()
							{
								NugetConfigFullName = nugetConfigFullName,
							}).PackageSources.ToNullCheckedDictionary(packageSource => packageSource.Key, packageSource => packageSource.Url, StringComparer.InvariantCultureIgnoreCase, NullCheckDictionaryResult.Empty);

							if (packageSources.TryGetValue(serviceLocatorDirectoryUrl, out var packageSourceUrl))
							{
								Uri.TryCreate(packageSourceUrl, UriKind.Absolute, out serviceLocatorDirectoryUri);
							}
						}
					}
				}

				var serviceLocatorDirectoryJson = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<ISI.Extensions.WebClient.Rest.TextResponse>(serviceLocatorDirectoryUri, null, true);

				if (string.IsNullOrWhiteSpace(serviceLocatorDirectoryJson.Content))
				{
					throw new Exception("cannot download serviceLocatorDirectory");
				}

				var serviceLocatorDirectory = JsonSerializer.Deserialize<SerializableDTOs.ServiceLocatorDirectory>(serviceLocatorDirectoryJson.Content.Replace("\"@id\"", "\"url\"").Replace("\"@type\"", "\"resource\""));

				var packagePublishUrl = serviceLocatorDirectory.Resources.NullCheckedFirstOrDefault(resource => resource.Resource.StartsWith("PackagePublish", StringComparison.InvariantCultureIgnoreCase))?.Url;

				if (string.IsNullOrWhiteSpace(packagePublishUrl))
				{
					throw new Exception("cannot find packagePublishUrl");
				}

				foreach (var nupkgFullName in request.NupkgFullNames)
				{
					var nupkgFileName = System.IO.Path.GetFileName(nupkgFullName);

					logger.LogInformation(string.Format("Pushing \"{0}\" to \"{1}\"", nupkgFileName, serviceLocatorDirectoryUrl));

					using (var client = new System.Net.WebClient())
					{
						client.Headers.Add(NuGetHeaderName, request.NugetApiKey);
						client.UploadFile(packagePublishUrl, System.Net.WebRequestMethods.Http.Put, nupkgFullName);
					}

					logger.LogInformation(string.Format("Pushed \"{0}\" to \"{1}\"", System.IO.Path.GetFileName(nupkgFullName), serviceLocatorDirectoryUrl));
				}
			}
			else
			{
				foreach (var nupkgFullName in request.NupkgFullNames)
				{
					var source = (string.IsNullOrWhiteSpace(request.RepositoryUri?.ToString()) ? request.RepositoryName : request.RepositoryUri?.ToString());

					logger.LogInformation(string.Format("Pushing \"{0}\" to \"{1}\"", System.IO.Path.GetFileName(nupkgFullName), source));

					var arguments = new List<string>();
					arguments.Add("push");

					arguments.Add(string.Format("-Source \"{0}\"", source));

					if (!string.IsNullOrWhiteSpace(request.NugetApiKey))
					{
						arguments.Add(string.Format("-ApiKey \"{0}\"", request.NugetApiKey));
					}

					var workingDirectory = string.IsNullOrWhiteSpace(request.WorkingDirectory) ? System.IO.Path.GetDirectoryName(nupkgFullName) : request.WorkingDirectory;

					if (!string.IsNullOrWhiteSpace(workingDirectory))
					{
						var nugetConfigFullNames = GetNugetConfigFullNames(new()
						{
							WorkingCopyDirectory = workingDirectory,
						}).NugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						foreach (var nugetConfigFullName in nugetConfigFullNames)
						{
							if (System.IO.File.Exists(nugetConfigFullName))
							{
								arguments.Add("-ConfigFile");
								arguments.Add(string.Format("\"{0}\"", nugetConfigFullName));
							}
						}
					}

					arguments.Add(string.Format("\"{0}\"", nupkgFullName));

					var nugetResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger, //new NullLogger(),
						WorkingDirectory = workingDirectory,
						ProcessExeFullName = GetNugetExeFullName(new()).NugetExeFullName,
						Arguments = arguments.ToArray(),
					});

					if (nugetResponse.Errored)
					{
						logger.LogError(string.Format("Error pushing \"{0}\" to \"{1}\"\n{2}", System.IO.Path.GetFileName(nupkgFullName), source, nugetResponse.Output));

						response.Success = false;
					}
					else
					{
						logger.LogInformation(string.Format("Pushed \"{0}\" to \"{1}\"", System.IO.Path.GetFileName(nupkgFullName), source));
					}
				}
			}

			return response;
		}
	}
}