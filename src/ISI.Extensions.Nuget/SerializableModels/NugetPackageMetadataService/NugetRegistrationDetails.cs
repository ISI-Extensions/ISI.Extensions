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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.SerializableModels.NugetPackageMetadataService
{
	[DataContract]
	public class NugetRegistrationDetails
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationDetailsUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public string Authors { get; set; }

		[DataMember(Name = "catalog:commitId", EmitDefaultValue = false)]
		public Guid CatalogCommitUuid { get; set; }

		[DataMember(Name = "catalog:commitTimeStamp", EmitDefaultValue = false)]
		public string __catalogCommitTimeStamp { get => CatalogCommitTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => CatalogCommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CatalogCommitTimeStamp { get; set; }

		[DataMember(Name = "copyright", EmitDefaultValue = false)]
		public string Copyright { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string __created { get => Created.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => Created = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Created { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "iconFile", EmitDefaultValue = false)]
		public string IconFile { get; set; }

		[DataMember(Name = "iconUrl", EmitDefaultValue = false)]
		public string IconUrl { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Package { get; set; }

		[DataMember(Name = "isPrerelease", EmitDefaultValue = false)]
		public bool IsPrerelease { get; set; }

		[DataMember(Name = "lastEdited", EmitDefaultValue = false)]
		public string __lastEdited { get => LastEdited.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => LastEdited = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime LastEdited { get; set; }

		[DataMember(Name = "licenseFile", EmitDefaultValue = false)]
		public string LicenseFile { get; set; }

		[DataMember(Name = "licenseUrl", EmitDefaultValue = false)]
		public string LicenseUrl { get; set; }

		[DataMember(Name = "listed", EmitDefaultValue = false)]
		public bool Listed { get; set; }

		[DataMember(Name = "packageHash", EmitDefaultValue = false)]
		public string PackageHash { get; set; }

		[DataMember(Name = "packageHashAlgorithm", EmitDefaultValue = false)]
		public string PackageHashAlgorithm { get; set; }

		[DataMember(Name = "packageSize", EmitDefaultValue = false)]
		public int PackageSize { get; set; }

		[DataMember(Name = "projectUrl", EmitDefaultValue = false)]
		public string ProjectUrl { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public string __published { get => Published.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => Published = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? Published { get; set; }

		[DataMember(Name = "releaseNotes", EmitDefaultValue = false)]
		public string ReleaseNotes { get; set; }

		[DataMember(Name = "requireLicenseAcceptance", EmitDefaultValue = false)]
		public bool RequireLicenseAcceptance { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "title", EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(Name = "verbatimVersion", EmitDefaultValue = false)]
		public string VerbatimVersion { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public NugetRegistrationDetailsDependencyGroup[] DependencyGroups { get; set; }

		[DataMember(Name = "packageEntries", EmitDefaultValue = false)]
		public NugetRegistrationDetailsPackageEntry[] PackageEntries { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "context", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContext Context { get; set; }
	}

	[DataContract]
	public class NugetRegistrationDetailsDependencyGroup
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string RegistrationDetailsDependencyGroupUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "targetFramework", EmitDefaultValue = false)]
		public string TargetFramework { get; set; }

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public NugetRegistrationDetailsDependency[] Dependencies { get; set; }
	}

	[DataContract]
	public class NugetRegistrationDetailsDependency
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string RegistrationDetailsDependencyUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Package { get; set; }

		[DataMember(Name = "range", EmitDefaultValue = false)]
		public string Range { get; set; }
	}

	[DataContract]
	public class NugetRegistrationDetailsPackageEntry
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string PackageEntryUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "compressedLength", EmitDefaultValue = false)]
		public int CompressedLength { get; set; }

		[DataMember(Name = "fullName", EmitDefaultValue = false)]
		public string FullName { get; set; }

		[DataMember(Name = "length", EmitDefaultValue = false)]
		public int Length { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class NugetRegistrationDetailsContext
	{
		public NugetRegistrationDetailsContext()
		{
			Vocab = "http://schema.nuget.org/schema#";
			Catalog = "http://schema.nuget.org/catalog#";
			Xsd = "http://www.w3.org/2001/XMLSchema#";
			Dependencies = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "dependency",
				Container = "@set",
			};
			DependencyGroups = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "dependencyGroup",
				Container = "@set",
			};
			PackageEntries = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "packageEntry",
				Container = "@set",
			};
			PackageTypes = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "packageType",
				Container = "@set",
			};
			SupportedFrameworks = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "supportedFramework",
				Container = "@set",
			};
			Tags = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "tag",
				Container = "@set",
			};
			Vulnerabilities = new NugetRegistrationDetailsContextItem()
			{
				ContextItemKey = "vulnerability",
				Container = "@set",
			};
			Published = new NugetRegistrationDetailsContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			Created = new NugetRegistrationDetailsContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			LastEdited = new NugetRegistrationDetailsContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			CatalogCommitTimeStamp = new NugetRegistrationDetailsContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			Reasons = new NugetRegistrationDetailsContextItem()
			{
				Container = "@set",
			};
		}

		[DataMember(Name = "@vocab", EmitDefaultValue = false)]
		public string Vocab { get; set; }

		[DataMember(Name = "@catalog", EmitDefaultValue = false)]
		public string Catalog { get; set; }

		[DataMember(Name = "xsd", EmitDefaultValue = false)]
		public string Xsd { get; set; }

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Dependencies { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem DependencyGroups { get; set; }

		[DataMember(Name = "packageEntries", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem PackageEntries { get; set; }

		[DataMember(Name = "packageTypes", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem PackageTypes { get; set; }

		[DataMember(Name = "supportedFrameworks", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem SupportedFrameworks { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Tags { get; set; }

		[DataMember(Name = "vulnerabilities", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Vulnerabilities { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Published { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Created { get; set; }

		[DataMember(Name = "lastEdited", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem LastEdited { get; set; }

		[DataMember(Name = "catalog:commitTimeStamp", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem CatalogCommitTimeStamp { get; set; }

		[DataMember(Name = "reasons", EmitDefaultValue = false)]
		public NugetRegistrationDetailsContextItem Reasons { get; set; }
	}

	[DataContract]
	public class NugetRegistrationDetailsContextItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string ContextItemKey { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "@container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}
}