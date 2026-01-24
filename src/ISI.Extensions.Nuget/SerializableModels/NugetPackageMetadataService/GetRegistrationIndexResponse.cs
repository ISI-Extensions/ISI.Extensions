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
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.SerializableModels.NugetPackageMetadataService
{
	[DataContract]
	public class GetRegistrationIndexResponse
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string RequestUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string[] Type { get; set; } =
		[
			"catalog:CatalogRoot",
			"PackageRegistration",
			"catalog:Permalink"
		];

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public string CommitId { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __CommitTimeStamp { get => $"{CommitTimeStamp:s}"; set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get => Items.NullCheckedCount(); set { var x = value; } }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public IndexResponseItem[] Items { get; set; }

		[DataMember(Name = "context", EmitDefaultValue = false)]
		public IndexResponseContext Context { get; set; }
	}

	[DataContract]
	public class IndexResponseContext
	{
		[DataMember(Name = "vocab", EmitDefaultValue = false)]
		public string Vocab { get; set; } = "http://schema.nuget.org/schema#";

		[DataMember(Name = "catalog", EmitDefaultValue = false)]
		public string Catalog { get; set; } = "http://schema.nuget.org/catalog#";

		[DataMember(Name = "xsd", EmitDefaultValue = false)]
		public string Xsd { get; set; } = "http://www.w3.org/2001/XMLSchema#";

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public IndexResponseIdContainer Items { get; set; } = new()
		{
			Id = "catalog:item",
			Container = "@set",
		};

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public IndexResponseIdType CommitTimeStamp { get; set; } = new()
		{
			Id = "catalog:commitTimeStamp",
			Type = "xsd:dateTime",
		};

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public IndexResponseId CommitId { get; set; } = new()
		{
			Id = "catalog:commitId",
		};

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public IndexResponseId Count { get; set; } = new()
		{
			Id = "catalog:count",
		};

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public IndexResponseIdType Parent { get; set; } = new()
		{
			Id = "catalog:parent",
			Type = "@id",
		};

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public IndexResponseIdContainer Tags { get; set; } = new()
		{
			Id = "tag",
			Container = "@set",
		};

		[DataMember(Name = "reasons", EmitDefaultValue = false)]
		public IndexResponseContainer Reasons { get; set; } = new()
		{
			Container = "@set",
		};

		[DataMember(Name = "packageTargetFrameworks", EmitDefaultValue = false)]
		public IndexResponseIdContainer PackageTargetFrameworks { get; set; } = new()
		{
			Id = "packageTargetFramework",
			Container = "@set",
		};

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public IndexResponseIdContainer DependencyGroups { get; set; } = new()
		{
			Id = "dependencyGroup",
			Container = "@set",
		};

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public IndexResponseIdContainer Dependencies { get; set; } = new()
		{
			Id = "dependency",
			Container = "@set",
		};

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public IndexResponseType PackageContent { get; set; } = new()
		{
			Type = "@id",
		};

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public IndexResponseType Published { get; set; } = new()
		{
			Type = "xsd:dateTime",
		};

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public IndexResponseType Registration { get; set; } = new()
		{
			Type = "@id",
		};
	}

	[DataContract]
	public class IndexResponseId
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Id { get; set; }
	}

	[DataContract]
	public class IndexResponseContainer
	{
		[DataMember(Name = "container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}

	[DataContract]
	public class IndexResponseType
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class IndexResponseIdContainer
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "@container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}

	[DataContract]
	public class IndexResponseIdType
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}

	[DataContract]
	public class IndexResponseItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string[] Type { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public string CommitId { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __CommitTimeStamp { get => $"{CommitTimeStamp:s}"; set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get => Items.NullCheckedCount(); set { var x = value; } }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public IndexResponseItemPackage[] Items { get; set; }

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public string Parent { get; set; }

		[DataMember(Name = "lower", EmitDefaultValue = false)]
		public string Lower { get; set; }

		[DataMember(Name = "upper", EmitDefaultValue = false)]
		public string Upper { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackage
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "Package";

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public string CommitId { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __CommitTimeStamp { get => $"{CommitTimeStamp:s}"; set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "catalogEntry", EmitDefaultValue = false)]
		public IndexResponseItemPackageDetails PackageDetails { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public string PackageContent { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public string Registration { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackageDetails
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string PackageDetailsId { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "PackageDetails";

		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public string Authors { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public IndexResponseItemPackageDependencyGroup[] DependencyGroups { get; set; }

		[DataMember(Name = "deprecation", EmitDefaultValue = false)]
		public IndexResponseItemPackageDeprecation Deprecation { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "iconUrl", EmitDefaultValue = false)]
		public string IconUrl { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string PackageId { get; set; }

		[DataMember(Name = "language", EmitDefaultValue = false)]
		public string Language { get; set; }

		[DataMember(Name = "licenseExpression", EmitDefaultValue = false)]
		public string LicenseExpression { get; set; }

		[DataMember(Name = "licenseUrl", EmitDefaultValue = false)]
		public string LicenseUrl { get; set; }

		[DataMember(Name = "listed", EmitDefaultValue = false)]
		public bool Listed { get; set; }

		[DataMember(Name = "minClientVersion", EmitDefaultValue = false)]
		public string MinClientVersion { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public string PackageContent { get; set; }

		[DataMember(Name = "projectUrl", EmitDefaultValue = false)]
		public string ProjectUrl { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public string __Published { get => $"{Published:s}"; set => Published = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Published { get; set; }

		[DataMember(Name = "requireLicenseAcceptance", EmitDefaultValue = false)]
		public bool RequireLicenseAcceptance { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "title", EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackageDeprecation
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string DeprecationId { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "deprecation";

		[DataMember(Name = "alternatePackage", EmitDefaultValue = false)]
		public IndexResponseItemPackageAlternatePackage AlternatePackage { get; set; }

		[DataMember(Name = "reasons", EmitDefaultValue = false)]
		public string[] Reasons { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackageAlternatePackage
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string AlternatePackageId { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "alternatePackage";

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string PackageId { get; set; }

		[DataMember(Name = "range", EmitDefaultValue = false)]
		public string Range { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackageDependencyGroup
	{
		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "DependencyGroup";

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public IndexResponseItemPackageDependency[] Dependencies { get; set; }

		[DataMember(Name = "targetFramework", EmitDefaultValue = false)]
		public string TargetFramework { get; set; }
	}

	[DataContract]
	public class IndexResponseItemPackageDependency
	{
		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string DependencyType { get; set; } = "PackageDependency";

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string PackageId { get; set; }

		[DataMember(Name = "range", EmitDefaultValue = false)]
		public string Range { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public string Registration { get; set; }
	}
}