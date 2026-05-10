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
		public DTOs.BuildNuspecResponse BuildNuspec(DTOs.BuildNuspecRequest request)
		{
			var response = new DTOs.BuildNuspecResponse();

			var package = new SerializableDTOs.Nuspec()
			{
				Metadata = new(),
			};

			package.Metadata.Package = request.Nuspec.Package;
			package.Metadata.Version = request.Nuspec.Version;

			if (!string.IsNullOrWhiteSpace(request.Nuspec.IconName))
			{
				package.Metadata.Icon = request.Nuspec.IconName;
			}
			else if (request.Nuspec.IconUri != null)
			{
				//package.metadata.icon = request.Nuspec.IconUri.ToString();
				package.Metadata.IconUrl = request.Nuspec.IconUri.ToString();
			}

			if (request.Nuspec.ProjectUri != null)
			{
				package.Metadata.ProjectUrl = request.Nuspec.ProjectUri.ToString();
			}

			package.Metadata.Title = request.Nuspec.Title;
			package.Metadata.Description = request.Nuspec.Description;
			package.Metadata.Summary = request.Nuspec.Summary;

			package.Metadata.Authors = string.Join(", ", request.Nuspec.Authors.ToNullCheckedArray(NullCheckCollectionResult.Empty));
			package.Metadata.Owners = string.Join(", ", request.Nuspec.Owners.ToNullCheckedArray(NullCheckCollectionResult.Empty));
			package.Metadata.Copyright = request.Nuspec.Copyright;

			if (request.Nuspec.LicenseUri != null)
			{
				package.Metadata.LicenseUrl = request.Nuspec.LicenseUri.ToString();
			}
			package.Metadata.License = request.Nuspec.License.NullCheckedConvert(license => new SerializableDTOs.NuspecPackageMetadataLicense()
			{
				Type = license.LicenseType,
				Version = license.Version,
				Value = license.Value,
			});

			if (request.Nuspec.RequireLicenseAcceptance.HasValue)
			{
				package.Metadata.RequireLicenseAcceptance = request.Nuspec.RequireLicenseAcceptance.Value;
				//package.Metadata.requireLicenseAcceptanceSpecified = true;
			}
			package.Metadata.ReleaseNotes = request.Nuspec.ReleaseNotes;
			package.Metadata.Readme = request.Nuspec.Readme;

			package.Metadata.Tags = string.Join(" ", request.Nuspec.Tags.ToNullCheckedArray(NullCheckCollectionResult.Empty));

			package.Metadata.Repository = request.Nuspec.Repository.NullCheckedConvert(repository => new SerializableDTOs.NuspecPackageMetadataRepository()
			{
				Type = repository.RepositoryType,
				Url = repository.RepositoryUri.ToString(),
				Branch = repository.Branch,
				Commit = repository.Commit,
			});

			package.Metadata.PackageTypes = request.Nuspec.PackageTypes.ToNullCheckedArray(packageType => new SerializableDTOs.NuspecPackageMetadataType()
			{
				Name = packageType.Name,
				Version = packageType.Version,
			}, NullCheckCollectionResult.ReturnNull);

			package.Metadata.Dependencies = request.Nuspec.Dependencies.NullCheckedConvert(dependencies => new SerializableDTOs.NuspecPackageMetadataDependencies()
			{
				Items = dependencies.ToNullCheckedArray(dependency =>
				{
					switch (dependency)
					{
						case NuspecDependency nuspecDependency:
							return new SerializableDTOs.NuspecPackageMetadataDependency()
							{
								Package = nuspecDependency.Package,
								Version = nuspecDependency.Version,
								Include = (nuspecDependency.Include.HasValue ? nuspecDependency.Include.TrueFalse() : null),
								Exclude = (nuspecDependency.Exclude.HasValue ? nuspecDependency.Exclude.TrueFalse() : null),
							} as object;

						case NuspecDependencyGroup nuspecDependencyGroup:
							return new SerializableDTOs.NuspecPackageMetadataDependencyGroup()
							{
								TargetFramework = nuspecDependencyGroup.TargetFramework,
								Dependencies = nuspecDependencyGroup.Dependencies.ToNullCheckedArray(d => new SerializableDTOs.NuspecPackageMetadataDependency()
								{
									Package = d.Package,
									Version = d.Version,
									Include = (d.Include.HasValue ? d.Include.TrueFalse() : null),
									Exclude = (d.Exclude.HasValue ? d.Exclude.TrueFalse() : null),
								})
							} as object;

						default:
							throw new ArgumentOutOfRangeException(nameof(dependency));
					}
				}, NullCheckCollectionResult.ReturnNull)
			});

			package.Files = request.Nuspec.Files.ToNullCheckedArray(file => new SerializableDTOs.NuspecPackageFile()
			{
				Src = file.SourcePattern,
				Target = (string.IsNullOrWhiteSpace(file.Target) ? null : file.Target),
				Exclude = (string.IsNullOrWhiteSpace(file.ExcludePattern) ? null : file.ExcludePattern),
			}, NullCheckCollectionResult.ReturnNull);

			using (var stream = new System.IO.MemoryStream())
			{
				var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SerializableDTOs.Nuspec));

				var xmlSerializerNamespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
				xmlSerializerNamespaces.Add(string.Empty, string.Empty);

				using (var xmlTextWriter = new System.Xml.XmlTextWriter(stream, Encoding.UTF8)
				{
					Formatting = System.Xml.Formatting.Indented,
				})
				{
					xmlSerializer.Serialize(xmlTextWriter, package);

					stream.Flush();
					stream.Rewind();

					response.Nuspec = stream.TextReadToEnd();

					//Very, very hockie ....
					response.Nuspec = response.Nuspec.Replace("<package xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"{0}\">", "<package xmlns=\"http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd\">");
					response.Nuspec = response.Nuspec.Replace("<authors />", string.Empty);
					response.Nuspec = response.Nuspec.Replace("<owners />", string.Empty);
					response.Nuspec = response.Nuspec.Replace("<tags />", string.Empty);
					response.Nuspec = response.Nuspec.Replace("<files xsi:nil=\"true\" />", string.Empty);

					return response;
				}
			}
		}
	}
}