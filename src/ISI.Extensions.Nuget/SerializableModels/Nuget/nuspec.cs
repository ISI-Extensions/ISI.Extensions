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

namespace ISI.Extensions.Nuget.SerializableModels.Nuget
{
	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "{0}", IsNullable = false, ElementName = "package")]
	public class Nuspec
	{
		[System.Xml.Serialization.XmlAttribute("metadata")]
		public NuspecPackageMetadata Metadata { get; set; }

		[System.Xml.Serialization.XmlArray(IsNullable = true)]
		[System.Xml.Serialization.XmlArrayItem("file", IsNullable = false)]
		public NuspecPackageFile[] Files { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadata
	{
		public NuspecPackageMetadata()
		{
			Language = "en-US";
		}

		[System.Xml.Serialization.XmlAttribute("id")]
		public string Package { get; set; }

		[System.Xml.Serialization.XmlAttribute("version")]
		public string Version { get; set; }

		[System.Xml.Serialization.XmlAttribute("title")]
		public string Title { get; set; }

		[System.Xml.Serialization.XmlAttribute("authors")]
		public string Authors { get; set; }

		[System.Xml.Serialization.XmlAttribute("owners")]
		public string Owners { get; set; }

		[System.Xml.Serialization.XmlElement(DataType = "anyURI", ElementName = "licenseUrl")]
		public string LicenseUrl { get; set; }

		[System.Xml.Serialization.XmlElement(DataType = "anyURI", ElementName = "projectUrl")]
		public string ProjectUrl { get; set; }

		[System.Xml.Serialization.XmlElement(DataType = "anyURI", ElementName = "iconUrl")]
		public string IconUrl { get; set; }

		[System.Xml.Serialization.XmlAttribute("requireLicenseAcceptance")]
		public bool RequireLicenseAcceptance { get; set; }

		[System.Xml.Serialization.XmlAttribute("developmentDependency")]
		public bool DevelopmentDependency { get; set; }

		[System.Xml.Serialization.XmlAttribute("description")]
		public string Description { get; set; }

		[System.Xml.Serialization.XmlAttribute("summary")]
		public string Summary { get; set; }

		[System.Xml.Serialization.XmlAttribute("releaseNotes")]
		public string ReleaseNotes { get; set; }

		[System.Xml.Serialization.XmlAttribute("copyright")]
		public string Copyright { get; set; }

		[System.Xml.Serialization.XmlAttribute("language")]
		[System.ComponentModel.DefaultValue("en-US")]
		public string Language { get; set; }

		[System.Xml.Serialization.XmlAttribute("tags")]
		public string Tags { get; set; }

		[System.Xml.Serialization.XmlAttribute("serviceable")]
		public bool Serviceable { get; set; }

		[System.Xml.Serialization.XmlAttribute("icon")]
		public string Icon { get; set; }

		[System.Xml.Serialization.XmlAttribute("readme")]
		public string Readme { get; set; }

		[System.Xml.Serialization.XmlElement("repository")]
		public NuspecPackageMetadataRepository Repository { get; set; }

		[System.Xml.Serialization.XmlElement("license")]
		public NuspecPackageMetadataLicense License { get; set; }

		[System.Xml.Serialization.XmlArrayItem("packageType", IsNullable = false)]
		public NuspecPackageMetadataType[] PackageTypes { get; set; }

		[System.Xml.Serialization.XmlElement("dependencies")]
		public NuspecPackageMetadataDependencies Dependencies { get; set; }

		[System.Xml.Serialization.XmlArrayItem("frameworkAssembly", IsNullable = false)]
		public NuspecPackageMetadataFrameworkAssembly[] FrameworkAssemblies { get; set; }

		[System.Xml.Serialization.XmlArrayItem("group", IsNullable = false)]
		public NuspecPackageMetadataFrameworkReferenceGroup[] FrameworkReferences { get; set; }

		[System.Xml.Serialization.XmlElement("references")]
		public NuspecPackageMetadataReferences References { get; set; }

		[System.Xml.Serialization.XmlElement("contentFiles")]
		public NuspecPackageMetadataContentFiles ContentFiles { get; set; }

		[System.Xml.Serialization.XmlAttribute("minClientVersion")]
		public string MinClientVersion { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataRepository
	{
		[System.Xml.Serialization.XmlAttribute("type")]
		public string Type { get; set; }

		[System.Xml.Serialization.XmlAttribute(DataType = "anyURI", AttributeName = "url")]
		public string Url { get; set; }

		[System.Xml.Serialization.XmlAttribute("branch")]
		public string Branch { get; set; }

		[System.Xml.Serialization.XmlAttribute("commit")]
		public string Commit { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataContentFile
	{
		[System.Xml.Serialization.XmlAttribute("include")]
		public string Include { get; set; }

		[System.Xml.Serialization.XmlAttribute("exclude")]
		public string Exclude { get; set; }

		[System.Xml.Serialization.XmlAttribute("buildAction")]
		public string BuildAction { get; set; }

		[System.Xml.Serialization.XmlAttribute("copyToOutput")]
		public bool CopyToOutput { get; set; }

		[System.Xml.Serialization.XmlAttribute("flatten")]
		public bool Flatten { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataReferenceGroup
	{
		[System.Xml.Serialization.XmlAttribute("reference")]
		public NuspecPackageMetadataReference[] References { get; set; }

		[System.Xml.Serialization.XmlAttribute("targetFramework")]
		public string TargetFramework { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataReference
	{
		[System.Xml.Serialization.XmlAttribute("file")]
		public string File { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataFrameworkReference
	{
		[System.Xml.Serialization.XmlAttribute("name")]
		public string Name { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataFrameworkReferenceGroup
	{
		[System.Xml.Serialization.XmlElement("frameworkReference")]
		public NuspecPackageMetadataFrameworkReference[] FrameworkReferences { get; set; }

		[System.Xml.Serialization.XmlAttribute("targetFramework")]
		public string TargetFramework { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataDependencyGroup
	{
		[System.Xml.Serialization.XmlElement("dependency")]
		public NuspecPackageMetadataDependency[] Dependencies { get; set; }

		[System.Xml.Serialization.XmlAttribute("targetFramework")]
		public string TargetFramework { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(Namespace = "{0}")]
	public class NuspecPackageMetadataDependency
	{
		[System.Xml.Serialization.XmlAttribute("id")]
		public string Package { get; set; }

		[System.Xml.Serialization.XmlAttribute("version")]
		public string Version { get; set; }

		[System.Xml.Serialization.XmlAttribute("include")]
		public string Include { get; set; }

		[System.Xml.Serialization.XmlAttribute("exclude")]
		public string Exclude { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataLicense
	{
		[System.Xml.Serialization.XmlAttribute("type")]
		public string Type { get; set; }

		[System.Xml.Serialization.XmlAttribute("version")]
		public string Version { get; set; }

		[System.Xml.Serialization.XmlTextAttribute()]
		public string Value { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataType
	{
		[System.Xml.Serialization.XmlAttribute("name")]
		public string Name { get; set; }

		[System.Xml.Serialization.XmlAttribute("version")]
		public string Version { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataDependencies
	{
		[System.Xml.Serialization.XmlElement("dependency", typeof(NuspecPackageMetadataDependency))]
		[System.Xml.Serialization.XmlElement("group", typeof(NuspecPackageMetadataDependencyGroup))]
		public object[] Items { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataFrameworkAssembly
	{
		[System.Xml.Serialization.XmlAttribute("assemblyName")]
		public string AssemblyName { get; set; }

		[System.Xml.Serialization.XmlAttribute("targetFramework")]
		public string TargetFramework { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataReferences
	{
		[System.Xml.Serialization.XmlElement("group", typeof(NuspecPackageMetadataReferenceGroup))]
		[System.Xml.Serialization.XmlElement("reference", typeof(NuspecPackageMetadataReference))]
		public object[] Items { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageMetadataContentFiles
	{
		[System.Xml.Serialization.XmlElement("files")]
		public NuspecPackageMetadataContentFile[] Items { get; set; }
	}

	[System.Serializable]
	[System.Diagnostics.DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("code")]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "{0}")]
	public class NuspecPackageFile
	{
		[System.Xml.Serialization.XmlAttribute("src")]
		public string Src { get; set; }

		[System.Xml.Serialization.XmlAttribute("target")]
		public string Target { get; set; }

		[System.Xml.Serialization.XmlAttribute("exclude")]
		public string Exclude { get; set; }
	}
}
