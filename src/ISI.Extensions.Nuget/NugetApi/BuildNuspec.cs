#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.BuildNuspecResponse BuildNuspec(DTOs.BuildNuspecRequest request)
		{
			var response = new DTOs.BuildNuspecResponse();

			var package = new ISI.Extensions.Nuget.SerializableModels.package()
			{
				metadata = new(),
			};

			package.metadata.id = request.Nuspec.Package;
			package.metadata.version = request.Nuspec.Version;

			if (!string.IsNullOrWhiteSpace(request.Nuspec.IconName))
			{
				package.metadata.icon = request.Nuspec.IconName;
			}
			else if (request.Nuspec.IconUri != null)
			{
				//package.metadata.icon = request.Nuspec.IconUri.ToString();
				package.metadata.iconUrl = request.Nuspec.IconUri.ToString();
			}

			if (request.Nuspec.ProjectUri != null)
			{
				package.metadata.projectUrl = request.Nuspec.ProjectUri.ToString();
			}

			package.metadata.title = request.Nuspec.Title;
			package.metadata.description = request.Nuspec.Description;
			package.metadata.summary = request.Nuspec.Summary;

			package.metadata.authors = string.Join(", ", request.Nuspec.Authors.ToNullCheckedArray(NullCheckCollectionResult.Empty));
			package.metadata.owners = string.Join(", ", request.Nuspec.Owners.ToNullCheckedArray(NullCheckCollectionResult.Empty));
			package.metadata.copyright = request.Nuspec.Copyright;

			if (request.Nuspec.LicenseUri != null)
			{
				package.metadata.licenseUrl = request.Nuspec.LicenseUri.ToString();
			}
			package.metadata.license = request.Nuspec.License.NullCheckedConvert(license => new ISI.Extensions.Nuget.SerializableModels.packageMetadataLicense()
			{
				type = license.LicenseType,
				version = license.Version,
				Value = license.Value,
			});

			if (request.Nuspec.RequireLicenseAcceptance.HasValue)
			{
				package.metadata.requireLicenseAcceptance = request.Nuspec.RequireLicenseAcceptance.Value;
				package.metadata.requireLicenseAcceptanceSpecified = true;
			}
			package.metadata.releaseNotes = request.Nuspec.ReleaseNotes;
			package.metadata.readme = request.Nuspec.Readme;

			package.metadata.tags = string.Join(", ", request.Nuspec.Tags.ToNullCheckedArray(NullCheckCollectionResult.Empty));

			package.metadata.repository = request.Nuspec.Repository.NullCheckedConvert(repository => new ISI.Extensions.Nuget.SerializableModels.packageMetadataRepository()
			{
				type = repository.RepositoryType,
				url = repository.RepositoryUri.ToString(),
				branch = repository.Branch,
				commit = repository.Commit,
			});

			package.metadata.packageTypes = request.Nuspec.PackageTypes.ToNullCheckedArray(packageType => new ISI.Extensions.Nuget.SerializableModels.packageMetadataPackageType()
			{
				name = packageType.Name,
				version = packageType.Version,
			}, NullCheckCollectionResult.ReturnNull);

			package.metadata.dependencies = request.Nuspec.Dependencies.NullCheckedConvert(dependencies => new ISI.Extensions.Nuget.SerializableModels.packageMetadataDependencies()
			{
				Items = dependencies.ToNullCheckedArray(dependency =>
				{
					switch (dependency)
					{
						case NuspecDependency nuspecDependency:
							return new ISI.Extensions.Nuget.SerializableModels.dependency()
							{
								id = nuspecDependency.Package,
								version = nuspecDependency.Version,
								include = (nuspecDependency.Include.HasValue ? nuspecDependency.Include.TrueFalse() : null),
								exclude = (nuspecDependency.Exclude.HasValue ? nuspecDependency.Exclude.TrueFalse() : null),
							} as object;

						case NuspecDependencyGroup nuspecDependencyGroup:
							return new ISI.Extensions.Nuget.SerializableModels.dependencyGroup()
							{
								targetFramework = nuspecDependencyGroup.TargetFramework,
								dependency = nuspecDependencyGroup.Dependencies.ToNullCheckedArray(d => new ISI.Extensions.Nuget.SerializableModels.dependency()
								{
									id = d.Package,
									version = d.Version,
									include = (d.Include.HasValue ? d.Include.TrueFalse() : null),
									exclude = (d.Exclude.HasValue ? d.Exclude.TrueFalse() : null),
								})
							} as object;

						default:
							throw new ArgumentOutOfRangeException(nameof(dependency));
					}
				}, NullCheckCollectionResult.ReturnNull)
			});

			package.files = request.Nuspec.Files.ToNullCheckedArray(file => new ISI.Extensions.Nuget.SerializableModels.packageFile()
			{
				src = file.SourcePattern,
				target = (string.IsNullOrWhiteSpace(file.Target) ? null : file.Target),
				exclude = (string.IsNullOrWhiteSpace(file.ExcludePattern) ? null : file.ExcludePattern),
			}, NullCheckCollectionResult.ReturnNull);

			using (var stream = new System.IO.MemoryStream())
			{
				var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ISI.Extensions.Nuget.SerializableModels.package));

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