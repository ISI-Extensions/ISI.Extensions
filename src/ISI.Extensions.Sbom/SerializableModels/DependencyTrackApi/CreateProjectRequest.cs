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
using System.Runtime.Serialization;

namespace ISI.Extensions.Sbom.SerializableModels.DependencyTrackApi
{
	[DataContract]
	public class CreateProjectRequest
	{
		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public CreateProjectRequestAuthor[] Authors { get; set; }

		[DataMember(Name = "publisher", EmitDefaultValue = false)]
		public string Publisher { get; set; }

		[DataMember(Name = "manufacturer", EmitDefaultValue = false)]
		public CreateProjectRequestManufacturer Manufacturer { get; set; }

		[DataMember(Name = "supplier", EmitDefaultValue = false)]
		public CreateProjectRequestSupplier Supplier { get; set; }

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
		public CreateProjectRequestTag CollectionTag { get; set; }

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
		public CreateProjectRequestProperty[] Properties { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public CreateProjectRequestTag[] Tags { get; set; }

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

		[DataMember(Name = "accessTeams", EmitDefaultValue = false)]
		public CreateProjectRequestAccessTeam[] AccessTeams { get; set; }

		[DataMember(Name = "externalReferences", EmitDefaultValue = false)]
		public CreateProjectRequestExternalReference[] ExternalReferences { get; set; }

		[DataMember(Name = "versions", EmitDefaultValue = false)]
		public CreateProjectRequestVersion[] Versions { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public string Author { get; set; }

		[DataMember(Name = "metrics", EmitDefaultValue = false)]
		public CreateProjectRequestMetrics Metrics { get; set; }

		[DataMember(Name = "bomRef", EmitDefaultValue = false)]
		public string BomRef { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestManufacturer
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "urls", EmitDefaultValue = false)]
		public string[] Urls { get; set; }

		[DataMember(Name = "contacts", EmitDefaultValue = false)]
		public CreateProjectRequestContact[] Contacts { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestContact
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "phone", EmitDefaultValue = false)]
		public string Phone { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestSupplier
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "urls", EmitDefaultValue = false)]
		public string[] Urls { get; set; }

		[DataMember(Name = "contacts", EmitDefaultValue = false)]
		public CreateProjectRequestContact[] Contacts { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestMetrics
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
	public class CreateProjectRequestAuthor
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "phone", EmitDefaultValue = false)]
		public string Phone { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestProperty
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
	public class CreateProjectRequestTag
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestAccessTeam
	{
		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "apiKeys", EmitDefaultValue = false)]
		public CreateProjectRequestApiKey[] ApiKeys { get; set; }

		[DataMember(Name = "ldapUsers", EmitDefaultValue = false)]
		public CreateProjectRequestLdapUser[] LdapUsers { get; set; }

		[DataMember(Name = "managedUsers", EmitDefaultValue = false)]
		public CreateProjectRequestManagedUser[] ManagedUsers { get; set; }

		[DataMember(Name = "oidcUsers", EmitDefaultValue = false)]
		public CreateProjectRequestOidcUser[] OidcUsers { get; set; }

		[DataMember(Name = "mappedLdapGroups", EmitDefaultValue = false)]
		public CreateProjectRequestMappedLdapGroup[] MappedLdapGroups { get; set; }

		[DataMember(Name = "mappedOidcGroups", EmitDefaultValue = false)]
		public CreateProjectRequestMappedOidcGroup[] MappedOidcGroups { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public CreateProjectRequestPermission[] Permissions { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestApiKey
	{
		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }

		[DataMember(Name = "created", EmitDefaultValue = false)]
		public string _Created { get => Created.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => Created = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime Created { get; set; }

		[DataMember(Name = "lastUsed", EmitDefaultValue = false)]
		public string _LastUsed { get => LastUsed.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => LastUsed = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime LastUsed { get; set; }

		[DataMember(Name = "publicId", EmitDefaultValue = false)]
		public string PublicId { get; set; }

		[DataMember(Name = "key", EmitDefaultValue = false)]
		public string Key { get; set; }

		[DataMember(Name = "maskedKey", EmitDefaultValue = false)]
		public string MaskedKey { get; set; }

		[DataMember(Name = "legacy", EmitDefaultValue = false)]
		public bool Legacy { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestLdapUser
	{
		[DataMember(Name = "username", EmitDefaultValue = false)]
		public string Username { get; set; }

		[DataMember(Name = "dn", EmitDefaultValue = false)]
		public string Dn { get; set; }

		[DataMember(Name = "teams", EmitDefaultValue = false)]
		public string[] Teams { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public CreateProjectRequestPermission[] Permissions { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestPermission
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestManagedUser
	{
		[DataMember(Name = "username", EmitDefaultValue = false)]
		public string Username { get; set; }

		[DataMember(Name = "lastPasswordChange", EmitDefaultValue = false)]
		public string _LastPasswordChange { get => LastPasswordChange.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise); set => LastPasswordChange = value.ToDateTime(); }
		[IgnoreDataMember]
		public DateTime LastPasswordChange { get; set; }

		[DataMember(Name = "fullname", EmitDefaultValue = false)]
		public string Fullname { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "suspended", EmitDefaultValue = false)]
		public bool Suspended { get; set; }

		[DataMember(Name = "forcePasswordChange", EmitDefaultValue = false)]
		public bool ForcePasswordChange { get; set; }

		[DataMember(Name = "nonExpiryPassword", EmitDefaultValue = false)]
		public bool NonExpiryPassword { get; set; }

		[DataMember(Name = "teams", EmitDefaultValue = false)]
		public string[] Teams { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public CreateProjectRequestPermission[] Permissions { get; set; }

		[DataMember(Name = "newPassword", EmitDefaultValue = false)]
		public string NewPassword { get; set; }

		[DataMember(Name = "confirmPassword", EmitDefaultValue = false)]
		public string ConfirmPassword { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestOidcUser
	{
		[DataMember(Name = "username", EmitDefaultValue = false)]
		public string Username { get; set; }

		[DataMember(Name = "subjectIdentifier", EmitDefaultValue = false)]
		public string SubjectIdentifier { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string Email { get; set; }

		[DataMember(Name = "teams", EmitDefaultValue = false)]
		public string[] Teams { get; set; }

		[DataMember(Name = "permissions", EmitDefaultValue = false)]
		public CreateProjectRequestPermission[] Permissions { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestMappedLdapGroup
	{
		[DataMember(Name = "dn", EmitDefaultValue = false)]
		public string Dn { get; set; }

		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestMappedOidcGroup
	{
		[DataMember(Name = "group", EmitDefaultValue = false)]
		public CreateProjectRequestGroup Group { get; set; }

		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestGroup
	{
		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestExternalReference
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "comment", EmitDefaultValue = false)]
		public string Comment { get; set; }
	}

	[DataContract]
	public class CreateProjectRequestVersion
	{
		[DataMember(Name = "uuid", EmitDefaultValue = false)]
		public string Uuid { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "active", EmitDefaultValue = false)]
		public bool Active { get; set; }
	}
}
