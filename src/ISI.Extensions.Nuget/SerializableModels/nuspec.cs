﻿#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.8.3928.0.
// 
namespace ISI.Extensions.Nuget.SerializableModels
{
	using System.Xml.Serialization;


	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	[XmlRoot(Namespace = "{0}", IsNullable = false)]
	public partial class package
	{

		private packageMetadata metadataField;

		private packageFile[] filesField;

		/// <remarks/>
		public packageMetadata metadata
		{
			get
			{
				return this.metadataField;
			}
			set
			{
				this.metadataField = value;
			}
		}

		/// <remarks/>
		[XmlArray(IsNullable = true)]
		[XmlArrayItem("file", IsNullable = false)]
		public packageFile[] files
		{
			get
			{
				return this.filesField;
			}
			set
			{
				this.filesField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadata
	{

		private string idField;

		private string versionField;

		private string titleField;

		private string authorsField;

		private string ownersField;

		private string licenseUrlField;

		private string projectUrlField;

		private string iconUrlField;

		private bool requireLicenseAcceptanceField;

		private bool requireLicenseAcceptanceFieldSpecified;

		private bool developmentDependencyField;

		private bool developmentDependencyFieldSpecified;

		private string descriptionField;

		private string summaryField;

		private string releaseNotesField;

		private string copyrightField;

		private string languageField;

		private string tagsField;

		private bool serviceableField;

		private bool serviceableFieldSpecified;

		private string iconField;

		private string readmeField;

		private packageMetadataRepository repositoryField;

		private packageMetadataLicense licenseField;

		private packageMetadataPackageType[] packageTypesField;

		private packageMetadataDependencies dependenciesField;

		private packageMetadataFrameworkAssembly[] frameworkAssembliesField;

		private frameworkReferenceGroup[] frameworkReferencesField;

		private packageMetadataReferences referencesField;

		private packageMetadataContentFiles contentFilesField;

		private string minClientVersionField;

		public packageMetadata()
		{
			this.languageField = "en-US";
		}

		/// <remarks/>
		public string id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public string authors
		{
			get
			{
				return this.authorsField;
			}
			set
			{
				this.authorsField = value;
			}
		}

		/// <remarks/>
		public string owners
		{
			get
			{
				return this.ownersField;
			}
			set
			{
				this.ownersField = value;
			}
		}

		/// <remarks/>
		[XmlElement(DataType = "anyURI")]
		public string licenseUrl
		{
			get
			{
				return this.licenseUrlField;
			}
			set
			{
				this.licenseUrlField = value;
			}
		}

		/// <remarks/>
		[XmlElement(DataType = "anyURI")]
		public string projectUrl
		{
			get
			{
				return this.projectUrlField;
			}
			set
			{
				this.projectUrlField = value;
			}
		}

		/// <remarks/>
		[XmlElement(DataType = "anyURI")]
		public string iconUrl
		{
			get
			{
				return this.iconUrlField;
			}
			set
			{
				this.iconUrlField = value;
			}
		}

		/// <remarks/>
		public bool requireLicenseAcceptance
		{
			get
			{
				return this.requireLicenseAcceptanceField;
			}
			set
			{
				this.requireLicenseAcceptanceField = value;
			}
		}

		/// <remarks/>
		[XmlIgnore()]
		public bool requireLicenseAcceptanceSpecified
		{
			get
			{
				return this.requireLicenseAcceptanceFieldSpecified;
			}
			set
			{
				this.requireLicenseAcceptanceFieldSpecified = value;
			}
		}

		/// <remarks/>
		public bool developmentDependency
		{
			get
			{
				return this.developmentDependencyField;
			}
			set
			{
				this.developmentDependencyField = value;
			}
		}

		/// <remarks/>
		[XmlIgnore()]
		public bool developmentDependencySpecified
		{
			get
			{
				return this.developmentDependencyFieldSpecified;
			}
			set
			{
				this.developmentDependencyFieldSpecified = value;
			}
		}

		/// <remarks/>
		public string description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				this.descriptionField = value;
			}
		}

		/// <remarks/>
		public string summary
		{
			get
			{
				return this.summaryField;
			}
			set
			{
				this.summaryField = value;
			}
		}

		/// <remarks/>
		public string releaseNotes
		{
			get
			{
				return this.releaseNotesField;
			}
			set
			{
				this.releaseNotesField = value;
			}
		}

		/// <remarks/>
		public string copyright
		{
			get
			{
				return this.copyrightField;
			}
			set
			{
				this.copyrightField = value;
			}
		}

		/// <remarks/>
		[System.ComponentModel.DefaultValueAttribute("en-US")]
		public string language
		{
			get
			{
				return this.languageField;
			}
			set
			{
				this.languageField = value;
			}
		}

		/// <remarks/>
		public string tags
		{
			get
			{
				return this.tagsField;
			}
			set
			{
				this.tagsField = value;
			}
		}

		/// <remarks/>
		public bool serviceable
		{
			get
			{
				return this.serviceableField;
			}
			set
			{
				this.serviceableField = value;
			}
		}

		/// <remarks/>
		[XmlIgnore()]
		public bool serviceableSpecified
		{
			get
			{
				return this.serviceableFieldSpecified;
			}
			set
			{
				this.serviceableFieldSpecified = value;
			}
		}

		/// <remarks/>
		public string icon
		{
			get
			{
				return this.iconField;
			}
			set
			{
				this.iconField = value;
			}
		}

		/// <remarks/>
		public string readme
		{
			get
			{
				return this.readmeField;
			}
			set
			{
				this.readmeField = value;
			}
		}

		/// <remarks/>
		public packageMetadataRepository repository
		{
			get
			{
				return this.repositoryField;
			}
			set
			{
				this.repositoryField = value;
			}
		}

		/// <remarks/>
		public packageMetadataLicense license
		{
			get
			{
				return this.licenseField;
			}
			set
			{
				this.licenseField = value;
			}
		}

		/// <remarks/>
		[XmlArrayItem("packageType", IsNullable = false)]
		public packageMetadataPackageType[] packageTypes
		{
			get
			{
				return this.packageTypesField;
			}
			set
			{
				this.packageTypesField = value;
			}
		}

		/// <remarks/>
		public packageMetadataDependencies dependencies
		{
			get
			{
				return this.dependenciesField;
			}
			set
			{
				this.dependenciesField = value;
			}
		}

		/// <remarks/>
		[XmlArrayItem("frameworkAssembly", IsNullable = false)]
		public packageMetadataFrameworkAssembly[] frameworkAssemblies
		{
			get
			{
				return this.frameworkAssembliesField;
			}
			set
			{
				this.frameworkAssembliesField = value;
			}
		}

		/// <remarks/>
		[XmlArrayItem("group", IsNullable = false)]
		public frameworkReferenceGroup[] frameworkReferences
		{
			get
			{
				return this.frameworkReferencesField;
			}
			set
			{
				this.frameworkReferencesField = value;
			}
		}

		/// <remarks/>
		public packageMetadataReferences references
		{
			get
			{
				return this.referencesField;
			}
			set
			{
				this.referencesField = value;
			}
		}

		/// <remarks/>
		public packageMetadataContentFiles contentFiles
		{
			get
			{
				return this.contentFilesField;
			}
			set
			{
				this.contentFilesField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string minClientVersion
		{
			get
			{
				return this.minClientVersionField;
			}
			set
			{
				this.minClientVersionField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataRepository
	{

		private string typeField;

		private string urlField;

		private string branchField;

		private string commitField;

		/// <remarks/>
		[XmlAttribute()]
		public string type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute(DataType = "anyURI")]
		public string url
		{
			get
			{
				return this.urlField;
			}
			set
			{
				this.urlField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string branch
		{
			get
			{
				return this.branchField;
			}
			set
			{
				this.branchField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string commit
		{
			get
			{
				return this.commitField;
			}
			set
			{
				this.commitField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class contentFileEntries
	{

		private string includeField;

		private string excludeField;

		private string buildActionField;

		private bool copyToOutputField;

		private bool copyToOutputFieldSpecified;

		private bool flattenField;

		private bool flattenFieldSpecified;

		/// <remarks/>
		[XmlAttribute()]
		public string include
		{
			get
			{
				return this.includeField;
			}
			set
			{
				this.includeField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string exclude
		{
			get
			{
				return this.excludeField;
			}
			set
			{
				this.excludeField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string buildAction
		{
			get
			{
				return this.buildActionField;
			}
			set
			{
				this.buildActionField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public bool copyToOutput
		{
			get
			{
				return this.copyToOutputField;
			}
			set
			{
				this.copyToOutputField = value;
			}
		}

		/// <remarks/>
		[XmlIgnore()]
		public bool copyToOutputSpecified
		{
			get
			{
				return this.copyToOutputFieldSpecified;
			}
			set
			{
				this.copyToOutputFieldSpecified = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public bool flatten
		{
			get
			{
				return this.flattenField;
			}
			set
			{
				this.flattenField = value;
			}
		}

		/// <remarks/>
		[XmlIgnore()]
		public bool flattenSpecified
		{
			get
			{
				return this.flattenFieldSpecified;
			}
			set
			{
				this.flattenFieldSpecified = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class referenceGroup
	{

		private reference[] referenceField;

		private string targetFrameworkField;

		/// <remarks/>
		[XmlElement("reference")]
		public reference[] reference
		{
			get
			{
				return this.referenceField;
			}
			set
			{
				this.referenceField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string targetFramework
		{
			get
			{
				return this.targetFrameworkField;
			}
			set
			{
				this.targetFrameworkField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class reference
	{

		private string fileField;

		/// <remarks/>
		[XmlAttribute()]
		public string file
		{
			get
			{
				return this.fileField;
			}
			set
			{
				this.fileField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class frameworkReference
	{

		private string nameField;

		/// <remarks/>
		[XmlAttribute()]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class frameworkReferenceGroup
	{

		private frameworkReference[] frameworkReferenceField;

		private string targetFrameworkField;

		/// <remarks/>
		[XmlElement("frameworkReference")]
		public frameworkReference[] frameworkReference
		{
			get
			{
				return this.frameworkReferenceField;
			}
			set
			{
				this.frameworkReferenceField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string targetFramework
		{
			get
			{
				return this.targetFrameworkField;
			}
			set
			{
				this.targetFrameworkField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class dependencyGroup
	{

		private dependency[] dependencyField;

		private string targetFrameworkField;

		/// <remarks/>
		[XmlElement("dependency")]
		public dependency[] dependency
		{
			get
			{
				return this.dependencyField;
			}
			set
			{
				this.dependencyField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string targetFramework
		{
			get
			{
				return this.targetFrameworkField;
			}
			set
			{
				this.targetFrameworkField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(Namespace = "{0}")]
	public partial class dependency
	{

		private string idField;

		private string versionField;

		private string includeField;

		private string excludeField;

		/// <remarks/>
		[XmlAttribute()]
		public string id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string include
		{
			get
			{
				return this.includeField;
			}
			set
			{
				this.includeField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string exclude
		{
			get
			{
				return this.excludeField;
			}
			set
			{
				this.excludeField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataLicense
	{

		private string typeField;

		private string versionField;

		private string valueField;

		/// <remarks/>
		[XmlAttribute()]
		public string type
		{
			get
			{
				return this.typeField;
			}
			set
			{
				this.typeField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}

		/// <remarks/>
		[XmlText()]
		public string Value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataPackageType
	{

		private string nameField;

		private string versionField;

		/// <remarks/>
		[XmlAttribute()]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string version
		{
			get
			{
				return this.versionField;
			}
			set
			{
				this.versionField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataDependencies
	{

		private object[] itemsField;

		/// <remarks/>
		[XmlElement("dependency", typeof(dependency))]
		[XmlElement("group", typeof(dependencyGroup))]
		public object[] Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataFrameworkAssembly
	{

		private string assemblyNameField;

		private string targetFrameworkField;

		/// <remarks/>
		[XmlAttribute()]
		public string assemblyName
		{
			get
			{
				return this.assemblyNameField;
			}
			set
			{
				this.assemblyNameField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string targetFramework
		{
			get
			{
				return this.targetFrameworkField;
			}
			set
			{
				this.targetFrameworkField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataReferences
	{

		private object[] itemsField;

		/// <remarks/>
		[XmlElement("group", typeof(referenceGroup))]
		[XmlElement("reference", typeof(reference))]
		public object[] Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageMetadataContentFiles
	{

		private contentFileEntries[] itemsField;

		/// <remarks/>
		[XmlElement("files")]
		public contentFileEntries[] Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[XmlType(AnonymousType = true, Namespace = "{0}")]
	public partial class packageFile
	{

		private string srcField;

		private string targetField;

		private string excludeField;

		/// <remarks/>
		[XmlAttribute()]
		public string src
		{
			get
			{
				return this.srcField;
			}
			set
			{
				this.srcField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string target
		{
			get
			{
				return this.targetField;
			}
			set
			{
				this.targetField = value;
			}
		}

		/// <remarks/>
		[XmlAttribute()]
		public string exclude
		{
			get
			{
				return this.excludeField;
			}
			set
			{
				this.excludeField = value;
			}
		}
	}
}
