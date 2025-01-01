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
		public DTOs.ExtractProjectNugetPackageDependenciesFromCsProjResponse ExtractProjectNugetPackageDependenciesFromCsProj(DTOs.ExtractProjectNugetPackageDependenciesFromCsProjRequest request)
		{
			var response = new DTOs.ExtractProjectNugetPackageDependenciesFromCsProjResponse();

			request.TryGetPackageVersion ??= (string _, out string version) =>
			{
				version = string.Empty;
				return false;
			};

			var nugetPackageKeys = new NugetPackageKeyDictionary();

			var csProjXml = System.Xml.Linq.XElement.Load(request.CsProjFullName);

			foreach (var itemGroup in csProjXml.GetElementsByLocalName("ItemGroup"))
			{
				var packageReferences = itemGroup.GetElementsByLocalName("PackageReference");

				foreach (var packageReference in packageReferences)
				{
					var packageId = (packageReference.GetAttributeByLocalName("Include") ?? packageReference.GetAttributeByLocalName("Update"))?.Value;
					var packageVersion = packageReference.GetAttributeByLocalName("Version")?.Value;
					if (string.IsNullOrWhiteSpace(packageVersion))
					{
						packageVersion = packageReference.GetElementByLocalName("Version")?.Value;
					}

					if (!string.IsNullOrWhiteSpace(packageVersion))
					{
						if (nugetPackageKeys.TryGetValue(packageId, out var nugetPackageKey))
						{
							if (!request.DoNotCheckForDifferentVersions && !string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
							{
								throw new(string.Format("Multiple versions of {0} found in {1}", packageId, request.CsProjFullName));
							}
						}
						else
						{
							nugetPackageKeys.TryAdd(packageId, packageVersion);
						}
					}
				}

				var projectReferences = itemGroup.GetElementsByLocalName("ProjectReference");

				foreach (var projectReference in projectReferences)
				{
					var projectPath = projectReference.GetAttributeByLocalName("Include")?.Value;

					if (!string.IsNullOrWhiteSpace(projectPath))
					{
						var packageId = System.IO.Path.GetFileNameWithoutExtension(projectPath);

						if (!request.TryGetPackageVersion(packageId, out var packageVersion))
						{
							packageVersion = string.Empty;
						}

						if (!string.IsNullOrWhiteSpace(packageVersion))
						{
							if (nugetPackageKeys.TryGetValue(packageId, out var nugetPackageKey))
							{
								if (!string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
								{
									throw new(string.Format("Multiple versions of {0} found in {1}", packageId, request.CsProjFullName));
								}
							}
							else
							{
								nugetPackageKeys.TryAdd(packageId, packageVersion);
							}
						}
					}
				}
			}

			foreach (var itemGroup in csProjXml.GetElementsByLocalName("ItemGroup"))
			{
				var references = itemGroup.GetElementsByLocalName("Reference");

				foreach (var reference in references)
				{
					var packageId = reference.GetAttributeByLocalName("Include")?.Value;
					var packageVersion = string.Empty;
					var hintPath = reference.GetElementByLocalName("HintPath")?.Value;

					var keyValues = string.Format("PackageId={0}", packageId).Split([','], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(["="], StringSplitOptions.None)).ToDictionary(item => item[0].Trim(), item => (item.Length >= 2 ? item[1].Trim() : string.Empty), StringComparer.CurrentCultureIgnoreCase);
					keyValues.TryGetValue("PackageId", out packageId);
					keyValues.TryGetValue("Version", out packageVersion);

					if (!string.IsNullOrWhiteSpace(hintPath))
					{
						var hintPathPieces = hintPath.Split(new[] { '\\' }).ToList();
						while (string.Equals(hintPathPieces.First(), "..", StringComparison.InvariantCultureIgnoreCase) || string.Equals(hintPathPieces.First(), "packages", StringComparison.InvariantCultureIgnoreCase))
						{
							hintPathPieces.RemoveAt(0);
						}

						hintPath = string.Join("\\", hintPathPieces);

						hintPathPieces = hintPathPieces.First().Split(['.'], StringSplitOptions.RemoveEmptyEntries).Reverse().ToList();

						packageId = string.Empty;
						packageVersion = string.Empty;

						var inVersion = true;
						while (hintPathPieces.Any())
						{
							var pathPiece = hintPathPieces.First();
							hintPathPieces.RemoveAt(0);

							if (inVersion && pathPiece.ToIntNullable().HasValue)
							{
								packageVersion = string.Format("{0}.{1}", pathPiece, packageVersion);
							}
							else
							{
								inVersion = false;

								packageId = string.Format("{0}.{1}", pathPiece, packageId);
							}
						}

						packageId = packageId.TrimEnd('.');
						packageVersion = packageVersion.TrimEnd('.');
					}

					if (!string.IsNullOrWhiteSpace(packageVersion) && !string.IsNullOrWhiteSpace(hintPath))
					{
						if (nugetPackageKeys.TryGetValue(packageId, out var nugetPackageKey))
						{
							if (!string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
							{
								throw new(string.Format("Multiple versions of {0} found in {1}", packageId, request.CsProjFullName));
							}
						}
						else
						{
							try
							{
								nugetPackageKey = new()
								{
									Package = packageId,
									Version = packageVersion,
								};

								if (request.BuildTargetFrameworks)
								{
									nugetPackageKey.TargetFrameworks = GetTargetFrameworksFromHintPath(hintPath);
								}

								nugetPackageKeys.TryAdd(nugetPackageKey);
							}
							catch
							{
							}
						}
					}
				}
			}

			response.NugetPackageKeys = nugetPackageKeys.ToArray();

			return response;
		}
	}
}