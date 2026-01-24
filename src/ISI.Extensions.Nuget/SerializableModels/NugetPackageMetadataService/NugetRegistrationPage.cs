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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.SerializableModels.NugetPackageMetadataService
{
	[DataContract]
	public class NugetRegistrationPage
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationPageUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public Guid CommitUuid { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __commitTimeStamp { get => CommitTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get; set; }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public NugetRegistrationPageItem[] Items { get; set; }

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public string NugetRegistrationIndexUrl { get; set; }

		[DataMember(Name = "lower", EmitDefaultValue = false)]
		public string LowerVersion { get; set; }

		[DataMember(Name = "upper", EmitDefaultValue = false)]
		public string UpperVersion { get; set; }

		[DataMember(Name = "@context", EmitDefaultValue = false)]
		public NugetRegistrationPageContext Context { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPageItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationDetailsUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public Guid CommitUuid { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __commitTimeStamp { get => CommitTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "catalogEntry", EmitDefaultValue = false)]
		public NugetRegistrationPageItemCatalogEntry CatalogEntry { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public string PackageContent { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public string Registration { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPageItemCatalogEntry
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationDetailsUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public string Authors { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public NugetRegistrationPageItemCatalogEntryDependencyGroup[] DependencyGroups { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "iconUrl", EmitDefaultValue = false)]
		public string IconUrl { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Package { get; set; }

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
		public string __published { get => Published.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => Published = value.ToDateTime(); }
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
	public class NugetRegistrationPageItemCatalogEntryDependencyGroup
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string DependencyGroupUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "targetFramework", EmitDefaultValue = false)]
		public string TargetFramework { get; set; }

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public NugetRegistrationPageItemCatalogEntryDependency[] Dependencies { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPageItemCatalogEntryDependency
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string DependencyUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Package { get; set; }

		[DataMember(Name = "range", EmitDefaultValue = false)]
		public string VersionRange { get; set; }

		//[DataMember(Name = "registration", EmitDefaultValue = false)]
		//public string Registration { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPageContext
	{
		public NugetRegistrationPageContext()
		{
			Vocab = "http://schema.nuget.org/schema#";
			Catalog = "http://schema.nuget.org/catalog#";
			Xsd = "http://www.w3.org/2001/XMLSchema#";
			Items = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "catalog:item",
				Container = "@set",
			};
			CommitTimeStamp = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "catalog:commitTimeStamp",
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			CommitId = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "catalog:commitId",
			};
			Count = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "catalog:count",
			};
			Parent = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "catalog:parent",
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Tags = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "tag",
				Container = "@set",
			};
			Reasons = new NugetRegistrationPageContextItem()
			{
				Container = "@set",
			};
			PackageTargetFrameworks = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "packageTargetFramework",
				Container = "@set",
			};
			DependencyGroups = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "dependencyGroup",
				Container = "@set",
			};
			Dependencies = new NugetRegistrationPageContextItem()
			{
				ContextItemKey = "dependency",
				Container = "@set",
			};
			PackageContent = new NugetRegistrationPageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Published = new NugetRegistrationPageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			Registration = new NugetRegistrationPageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
		}

		[DataMember(Name = "@vocab", EmitDefaultValue = false)]
		public string Vocab { get; set; }

		[DataMember(Name = "catalog", EmitDefaultValue = false)]
		public string Catalog { get; set; }

		[DataMember(Name = "xsd", EmitDefaultValue = false)]
		public string Xsd { get; set; }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Items { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem CommitTimeStamp { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem CommitId { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Count { get; set; }

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Parent { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Tags { get; set; }

		[DataMember(Name = "reasons", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Reasons { get; set; }

		[DataMember(Name = "packageTargetFrameworks", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem PackageTargetFrameworks { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem DependencyGroups { get; set; }

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Dependencies { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem PackageContent { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Published { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public NugetRegistrationPageContextItem Registration { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPageContextItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string ContextItemKey { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "@container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}
}