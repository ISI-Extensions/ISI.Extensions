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
	public class NugetRegistrationIndex
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationIndexUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public Guid CommitUuid { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __commitTimeStamp { get => CommitTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get; set; }

		[DataMember(Name = "items", EmitDefaultValue = false)]
		public NugetRegistrationIndexItem[] Items { get; set; }

		[DataMember(Name = "itemContext", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContext ItemContext { get; set; }
	}

	[DataContract]
	public class NugetRegistrationIndexItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationPageUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public Guid CommitUuid { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public string __commitTimeStamp { get => CommitTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversal); set => CommitTimeStamp = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime CommitTimeStamp { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get; set; }

		[DataMember(Name = "lower", EmitDefaultValue = false)]
		public string LowerVersion { get; set; }

		[DataMember(Name = "upper", EmitDefaultValue = false)]
		public string UpperVersion { get; set; }
	}

	[DataContract]
	public class NugetRegistrationIndexItemContext
	{
		public NugetRegistrationIndexItemContext()
		{
			Vocab = "http://schema.nuget.org/schema#";
			Catalog = "http://schema.nuget.org/catalog#";
			Xsd = "http://www.w3.org/2001/XMLSchema#";
			Items = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "catalog:item",
				Container = "@set",
			};
			CommitTimeStamp = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "catalog:commitTimeStamp",
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			CommitId = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "catalog:commitId",
			};
			Count = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "catalog:count",
			};
			Parent = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "catalog:parent",
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Tags = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "tag",
				Container = "@set",
			};
			Reasons = new NugetRegistrationIndexItemContextItem()
			{
				Container = "@set",
			};
			PackageTargetFrameworks = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "packageTargetFramework",
				Container = "@set",
			};
			DependencyGroups = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "dependencyGroup",
				Container = "@set",
			};
			Dependencies = new NugetRegistrationIndexItemContextItem()
			{
				ContextItemKey = "dependency",
				Container = "@set",
			};
			PackageContent = new NugetRegistrationIndexItemContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Published = new NugetRegistrationIndexItemContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
			Registration = new NugetRegistrationIndexItemContextItem()
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
		public NugetRegistrationIndexItemContextItem Items { get; set; }

		[DataMember(Name = "commitTimeStamp", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem CommitTimeStamp { get; set; }

		[DataMember(Name = "commitId", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem CommitId { get; set; }

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Count { get; set; }

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Parent { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Tags { get; set; }

		[DataMember(Name = "reasons", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Reasons { get; set; }

		[DataMember(Name = "packageTargetFrameworks", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem PackageTargetFrameworks { get; set; }

		[DataMember(Name = "dependencyGroups", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem DependencyGroups { get; set; }

		[DataMember(Name = "dependencies", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Dependencies { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem PackageContent { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Published { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public NugetRegistrationIndexItemContextItem Registration { get; set; }
	}

	[DataContract]
	public class NugetRegistrationIndexItemContextItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string ContextItemKey { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "@container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}
}