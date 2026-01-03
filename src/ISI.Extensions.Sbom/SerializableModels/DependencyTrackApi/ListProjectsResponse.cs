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
using System.Runtime.Serialization;

namespace ISI.Extensions.Sbom.SerializableModels.DependencyTrackApi
{
	[DataContract]
	public class ListProjectsResponse
	{
		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public ListProjectsResponseAuthor[] Authors { get; set; }

		[DataMember(Name = "publisher", EmitDefaultValue = false)]
		public string Publisher { get; set; }

		[DataMember(Name = "manufacturer", EmitDefaultValue = false)]
		public ListProjectsResponseManufacturer Manufacturer { get; set; }

		[DataMember(Name = "supplier", EmitDefaultValue = false)]
		public ListProjectsResponseSupplier Supplier { get; set; }

		[DataMember(Name = "group", EmitDefaultValue = false)]
		public string Group { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string ProjectName { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "classifier", EmitDefaultValue = false)]
		public string Classifier { get; set; }

		[DataMember(Name = "collectionLogic", EmitDefaultValue = false)]
		public string CollectionLogic { get; set; }

		[DataMember(Name = "collectionTag", EmitDefaultValue = false)]
		public ListProjectsResponseTag CollectionTag { get; set; }

		[DataMember(Name = "cpe", EmitDefaultValue = false)]
		public string Cpe { get; set; }

		[DataMember(Name = "purl", EmitDefaultValue = false)]
		public string Purl { get; set; }

		[DataMember(Name = "swidTagId", EmitDefaultValue = false)]
		public string SwidTagId { get; set; }

		[DataMember(Name = "directDependencies", EmitDefaultValue = false)]
		public string DirectDependencies { get; set; }

		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public Guid ProjectUuid { get; set; }

		[DataMember(Name = "parent", EmitDefaultValue = false)]
		public Guid? ParentProjectUuid { get; set; }

		[DataMember(Name = "children", EmitDefaultValue = false)]
		public string[] Children { get; set; }

		[DataMember(Name = "properties", EmitDefaultValue = false)]
		public ListProjectsResponseProperty[] Properties { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public ListProjectsResponseTag[] Tags { get; set; }

		[DataMember(Name = "lastBomImport", EmitDefaultValue = false)]
		public int LastBomImport { get; set; }

		[DataMember(Name = "lastBomImportFormat", EmitDefaultValue = false)]
		public string LastBomImportFormat { get; set; }

		[DataMember(Name = "lastInheritedRiskScore", EmitDefaultValue = false)]
		public int LastInheritedRiskScore { get; set; }

		[DataMember(Name = "lastVulnerabilityAnalysis", EmitDefaultValue = false)]
		public int LastVulnerabilityAnalysis { get; set; }

		[DataMember(Name = "active", EmitDefaultValue = false)]
		public bool Active { get; set; }

		[DataMember(Name = "isLatest", EmitDefaultValue = false)]
		public bool IsLatest { get; set; }

		[DataMember(Name = "externalReferences", EmitDefaultValue = false)]
		public ListProjectsResponseExternalReference[] ExternalReferences { get; set; }

		[DataMember(Name = "metadata", EmitDefaultValue = false)]
		public ListProjectsResponseMetadata Metadata { get; set; }

		[DataMember(Name = "versions", EmitDefaultValue = false)]
		public ListProjectsResponseVersion[] Versions { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public string Author { get; set; }

		[DataMember(Name = "metrics", EmitDefaultValue = false)]
		public ListProjectsResponseMetrics Metrics { get; set; }

		[DataMember(Name = "bomRef", EmitDefaultValue = false)]
		public string BomRef { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseManufacturer
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "urls", EmitDefaultValue = false)]
		public string[] Urls { get; set; }

		[DataMember(Name = "contacts", EmitDefaultValue = false)]
		public ListProjectsResponseContact[] Contacts { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseContact
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "phone", EmitDefaultValue = false)]
		public string Phone { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseSupplier
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "urls", EmitDefaultValue = false)]
		public string[] Urls { get; set; }

		[DataMember(Name = "contacts", EmitDefaultValue = false)]
		public ListProjectsResponseContact[] Contacts { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseMetadata
	{
		[DataMember(Name = "supplier", EmitDefaultValue = false)]
		public ListProjectsResponseSupplier Supplier { get; set; }

		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public ListProjectsResponseAuthor[] Authors { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseAuthor
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "phone", EmitDefaultValue = false)]
		public string Phone { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseMetrics
	{
		[DataMember(Name = "critical", EmitDefaultValue = false)]
		public int Critical { get; set; }

		[DataMember(Name = "high", EmitDefaultValue = false)]
		public int High { get; set; }

		[DataMember(Name = "medium", EmitDefaultValue = false)]
		public int Medium { get; set; }

		[DataMember(Name = "low", EmitDefaultValue = false)]
		public int Low { get; set; }

		[DataMember(Name = "unassigned", EmitDefaultValue = false)]
		public int Unassigned { get; set; }

		[DataMember(Name = "vulnerabilities", EmitDefaultValue = false)]
		public int Vulnerabilities { get; set; }

		[DataMember(Name = "vulnerableComponents", EmitDefaultValue = false)]
		public int VulnerableComponents { get; set; }

		[DataMember(Name = "components", EmitDefaultValue = false)]
		public int Components { get; set; }

		[DataMember(Name = "suppressed", EmitDefaultValue = false)]
		public int Suppressed { get; set; }

		[DataMember(Name = "findingsTotal", EmitDefaultValue = false)]
		public int FindingsTotal { get; set; }

		[DataMember(Name = "findingsAudited", EmitDefaultValue = false)]
		public int FindingsAudited { get; set; }

		[DataMember(Name = "findingsUnaudited", EmitDefaultValue = false)]
		public int FindingsUnaudited { get; set; }

		[DataMember(Name = "inheritedRiskScore", EmitDefaultValue = false)]
		public int InheritedRiskScore { get; set; }

		[DataMember(Name = "policyViolationsFail", EmitDefaultValue = false)]
		public int PolicyViolationsFail { get; set; }

		[DataMember(Name = "policyViolationsWarn", EmitDefaultValue = false)]
		public int PolicyViolationsWarn { get; set; }

		[DataMember(Name = "policyViolationsInfo", EmitDefaultValue = false)]
		public int PolicyViolationsInfo { get; set; }

		[DataMember(Name = "policyViolationsTotal", EmitDefaultValue = false)]
		public int PolicyViolationsTotal { get; set; }

		[DataMember(Name = "policyViolationsAudited", EmitDefaultValue = false)]
		public int PolicyViolationsAudited { get; set; }

		[DataMember(Name = "policyViolationsUnaudited", EmitDefaultValue = false)]
		public int PolicyViolationsUnaudited { get; set; }

		[DataMember(Name = "policyViolationsSecurityTotal", EmitDefaultValue = false)]
		public int PolicyViolationsSecurityTotal { get; set; }

		[DataMember(Name = "policyViolationsSecurityAudited", EmitDefaultValue = false)]
		public int PolicyViolationsSecurityAudited { get; set; }

		[DataMember(Name = "policyViolationsSecurityUnaudited", EmitDefaultValue = false)]
		public int PolicyViolationsSecurityUnaudited { get; set; }

		[DataMember(Name = "policyViolationsLicenseTotal", EmitDefaultValue = false)]
		public int PolicyViolationsLicenseTotal { get; set; }

		[DataMember(Name = "policyViolationsLicenseAudited", EmitDefaultValue = false)]
		public int PolicyViolationsLicenseAudited { get; set; }

		[DataMember(Name = "policyViolationsLicenseUnaudited", EmitDefaultValue = false)]
		public int PolicyViolationsLicenseUnaudited { get; set; }

		[DataMember(Name = "policyViolationsOperationalTotal", EmitDefaultValue = false)]
		public int PolicyViolationsOperationalTotal { get; set; }

		[DataMember(Name = "policyViolationsOperationalAudited", EmitDefaultValue = false)]
		public int PolicyViolationsOperationalAudited { get; set; }

		[DataMember(Name = "policyViolationsOperationalUnaudited", EmitDefaultValue = false)]
		public int PolicyViolationsOperationalUnaudited { get; set; }

		[DataMember(Name = "collectionLogic", EmitDefaultValue = false)]
		public string CollectionLogic { get; set; }

		[DataMember(Name = "collectionLogicChanged", EmitDefaultValue = false)]
		public bool CollectionLogicChanged { get; set; }

		[DataMember(Name = "firstOccurrence", EmitDefaultValue = false)]
		public int FirstOccurrence { get; set; }

		[DataMember(Name = "lastOccurrence", EmitDefaultValue = false)]
		public int LastOccurrence { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseProperty
	{
		[DataMember(Name = "groupName", EmitDefaultValue = false)]
		public string GroupName { get; set; }

		[DataMember(Name = "propertyName", EmitDefaultValue = false)]
		public string PropertyName { get; set; }

		[DataMember(Name = "propertyValue", EmitDefaultValue = false)]
		public string PropertyValue { get; set; }

		[DataMember(Name = "propertyType", EmitDefaultValue = false)]
		public string PropertyType { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseTag
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseExternalReference
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }
	}

	[DataContract]
	public class ListProjectsResponseVersion
	{
		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "active", EmitDefaultValue = false)]
		public bool Active { get; set; }
	}
}
