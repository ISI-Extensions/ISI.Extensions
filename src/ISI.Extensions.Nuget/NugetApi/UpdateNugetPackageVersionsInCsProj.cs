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
		public DTOs.UpdateNugetPackageVersionsInCsProjResponse UpdateNugetPackageVersionsInCsProj(DTOs.UpdateNugetPackageVersionsInCsProjRequest request)
		{
			var response = new DTOs.UpdateNugetPackageVersionsInCsProjResponse();

			request.TryGetPackageHintPath ??= (NugetPackageKey key, out string path) =>
			{
				path = string.Empty;
				return false;
			};

			var csProjXml = System.Xml.Linq.XElement.Parse(request.CsProjXml);

			var replacements = new Dictionary<string, string>();
			//replacements.Add("<Version xmlns=\"\">", "<Version>");
			replacements.Add(" xmlns=\"\"", string.Empty);

			var usedPackageReferences = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			if ((request.CsProjXml.IndexOf("Sdk=\"Microsoft.NET", StringComparison.InvariantCultureIgnoreCase) >= 0) || (request.CsProjXml.IndexOf("<PackageReference", StringComparison.InvariantCultureIgnoreCase) >= 0))
			{
				foreach (var itemGroup in csProjXml.Elements("ItemGroup"))
				{
					var packageReferences = itemGroup.Elements("PackageReference");

					foreach (var packageReference in packageReferences)
					{
						var packageId = (packageReference.Attributes("Include").FirstOrDefault() ?? packageReference.Attributes("Update").FirstOrDefault())?.Value ?? string.Empty;
						var packageVersion = (packageReference.Attributes("Version").FirstOrDefault()?.Value ?? packageReference.Elements("Version").FirstOrDefault()?.Value) ?? string.Empty;

						if (request.TryGetNugetPackageKey(packageId, out var nugetPackageKey) && !string.IsNullOrWhiteSpace(nugetPackageKey.Version) && !string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
						{
							var versionAttribute = packageReference.Attributes("Version").FirstOrDefault();
							if (versionAttribute != null)
							{
								versionAttribute.Value = nugetPackageKey.Version;
							}

							var versionElement = packageReference.Elements("Version").FirstOrDefault();
							if (versionElement != null)
							{
								versionElement.Value = nugetPackageKey.Version;
							}
						}
					}
				}
			}

			if (request.CsProjXml.IndexOf("Sdk=\"Microsoft.NET", StringComparison.InvariantCultureIgnoreCase) < 0)
			{
				foreach (var itemGroup in csProjXml.Elements().Where(e => string.Equals(e.Name.LocalName, "ItemGroup", StringComparison.InvariantCultureIgnoreCase)))
				{
					var references = itemGroup.Elements().Where(e => string.Equals(e.Name.LocalName, "Reference", StringComparison.InvariantCultureIgnoreCase)).ToArray();

					foreach (var reference in references)
					{
						var packageAttribute = reference.Attributes("Include").FirstOrDefault();

						if (packageAttribute != null)
						{
							var packageId = string.Empty;
							var packageVersion = string.Empty;

							var hintPathAttribute = reference.Elements().FirstOrDefault(e => string.Equals(e.Name.LocalName, "HintPath", StringComparison.InvariantCultureIgnoreCase));

							if (hintPathAttribute != null)
							{
								var hintPath = hintPathAttribute.Value;

								if (!string.IsNullOrWhiteSpace(hintPath))
								{
									var hintPathPieces = hintPath.Split(new[] { '\\' }).ToList();

									var packagesPath = string.Empty;
									while (string.Equals(hintPathPieces.First(), "..", StringComparison.InvariantCultureIgnoreCase) || string.Equals(hintPathPieces.First(), "packages", StringComparison.InvariantCultureIgnoreCase))
									{
										packagesPath = string.Format("{0}\\{1}", packagesPath, hintPathPieces.First());
										hintPathPieces.RemoveAt(0);
									}

									packagesPath = packagesPath.TrimStart("\\");

									hintPathPieces = hintPathPieces.First().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToList();

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
									if (string.IsNullOrWhiteSpace(packageId))
									{
										packageId = packageAttribute.Value;
									}
									
									packageVersion = packageVersion.TrimEnd('.');

									if (request.TryGetNugetPackageKey(packageId, out var nugetPackageKey) && !string.IsNullOrWhiteSpace(nugetPackageKey.Version) && !string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
									{
										if (string.IsNullOrWhiteSpace(nugetPackageKey.HintPath) && request.TryGetPackageHintPath(nugetPackageKey, out hintPath))
										{
											nugetPackageKey.HintPath = hintPath;
										}

										packageAttribute.Value = packageAttribute.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).First();
										hintPathAttribute.Value = string.Format("{0}\\{1}", packagesPath, nugetPackageKey.HintPath);
									}
								}
							}

							if (request.ConvertToPackageReferences)
							{
								if (!string.IsNullOrWhiteSpace(packageId))
								{
									if (!usedPackageReferences.Contains(packageId))
									{
										if (request.TryGetNugetPackageKey(packageId, out var nugetPackageKey) && !string.IsNullOrWhiteSpace(nugetPackageKey.Version))
										{
											packageVersion = nugetPackageKey.Version;
										}

										var packageReferenceElement = new System.Xml.Linq.XElement("PackageReference");

										var includeAttribute = new System.Xml.Linq.XAttribute("Include", packageId);
										packageReferenceElement.Add(includeAttribute);

										var versionElement = new System.Xml.Linq.XElement("Version");
										versionElement.Value = packageVersion;
										packageReferenceElement.Add(versionElement);

										reference.AddBeforeSelf(packageReferenceElement);

										usedPackageReferences.Add(packageId);
									}

									reference.Remove();
								}
							}
						}
					}

					var packageReferences = itemGroup.Elements().Where(e => string.Equals(e.Name.LocalName, "PackageReference", StringComparison.InvariantCultureIgnoreCase)).ToArray();

					foreach (var packageReference in packageReferences)
					{
						var packageId = (packageReference.Attributes("Include").FirstOrDefault() ?? packageReference.Attributes("Update").FirstOrDefault())?.Value ?? string.Empty;
						var packageVersionAttribute = packageReference.Attributes("Version").FirstOrDefault();
						var packageVersionElement = packageReference.Elements().FirstOrDefault(e => string.Equals(e.Name.LocalName, "Version", StringComparison.InvariantCultureIgnoreCase));
						var packageVersion = packageVersionAttribute?.Value ?? packageVersionElement?.Value ?? string.Empty;

						if (request.TryGetNugetPackageKey(packageId, out var nugetPackageKey) && !string.IsNullOrWhiteSpace(nugetPackageKey.Version) && !string.Equals(packageVersion, nugetPackageKey.Version, StringComparison.InvariantCultureIgnoreCase))
						{
							if (packageVersionAttribute != null)
							{
								packageVersionAttribute.Value = nugetPackageKey.Version;
							}

							if (packageVersionElement != null)
							{
								packageVersionElement.Value = nugetPackageKey.Version;
							}
						}
					}
				}
			}

			if (request.ConvertToPackageReferences)
			{
				var removeTargets = new[]
				{
					"NETStandard.Library.targets",
					"Microsoft.ApplicationInsights.DependencyCollector.targets",
					"Microsoft.Bcl.Build.targets",
					"Microsoft.Net.Compilers",
					"NUnit.props",
				};

				foreach (var importElement in csProjXml.Elements().Where(e => string.Equals(e.Name.LocalName, "Import", StringComparison.InvariantCultureIgnoreCase)))
				{
					var project = importElement.Attributes("Project").FirstOrDefault()?.Value ?? string.Empty;

					foreach (var removeTarget in removeTargets)
					{
						if (project.IndexOf(removeTarget, StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							importElement.Remove();
						}
					}
				}

				foreach (var targetElement in csProjXml.Elements().Where(e => string.Equals(e.Name.LocalName, "Target", StringComparison.InvariantCultureIgnoreCase)))
				{
					var errors = targetElement.Elements().Where(e => string.Equals(e.Name.LocalName, "Error", StringComparison.InvariantCultureIgnoreCase));

					foreach (var error in errors)
					{
						var condition = error.Attributes("Condition").FirstOrDefault()?.Value ?? string.Empty;

						foreach (var removeTarget in removeTargets)
						{
							if (condition.IndexOf(removeTarget, StringComparison.InvariantCultureIgnoreCase) >= 0)
							{
								error.Remove();
							}
						}
					}
				}

				foreach (var itemGroup in csProjXml.Elements().Where(e => string.Equals(e.Name.LocalName, "ItemGroup", StringComparison.InvariantCultureIgnoreCase)))
				{
					var references = itemGroup.Elements().Where(e => string.Equals(e.Name.LocalName, "None", StringComparison.InvariantCultureIgnoreCase));

					foreach (var reference in references)
					{
						var include = reference.Attributes("Include").FirstOrDefault()?.Value ?? string.Empty;

						if (string.Equals(include, "packages.config", StringComparison.InvariantCultureIgnoreCase))
						{
							reference.Remove();
						}
					}

					if (!itemGroup.Elements().Any())
					{
						itemGroup.Remove();
					}
				}
			}

			response.CsProjXml = csProjXml.ToString();

			foreach (var replacement in replacements)
			{
				response.CsProjXml = response.CsProjXml.Replace(replacement.Key, replacement.Value);
			}

			if (request.CsProjXml.IndexOf("Sdk=\"Microsoft.NET", StringComparison.InvariantCultureIgnoreCase) < 0)
			{
				response.CsProjXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{0}", response.CsProjXml);
			}

			return response;
		}
	}
}