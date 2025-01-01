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

namespace ISI.Extensions.VisualStudio.SerializableModels
{
	[DataContract]
	public class VsixPublishManifest
	{
		[DataMember(Name = "$schema", EmitDefaultValue = false)]
		public string Schema { get; set; } = "http://json.schemastore.org/vsix-publish";

		[DataMember(Name = "categories", EmitDefaultValue = false)]
		public string[] Categories { get; set; }

		[DataMember(Name = "identity", EmitDefaultValue = false)]
		public VsixPublishManifestIdentity Identity { get; set; }

		[DataMember(Name = "overview", EmitDefaultValue = true)]
		public string Overview { get; set; }

		[DataMember(Name = "assetFiles", EmitDefaultValue = false)]
		public VsixPublishManifestAssetFile[] AssetFiles { get; set; }

		[DataMember(Name = "priceCategory", EmitDefaultValue = false)]
		public string PriceCategory { get; set; }

		[DataMember(Name = "publisher", EmitDefaultValue = false)]
		public string Publisher { get; set; }

		[DataMember(Name = "private", EmitDefaultValue = false)]
		public bool Private { get; set; }

		[DataMember(Name = "qna", EmitDefaultValue = false)]
		public bool QuestionAndAnswer { get; set; }

		[DataMember(Name = "repo", EmitDefaultValue = false)]
		public string RepositoryUrl { get; set; }
	}

	[DataContract]
	public class VsixPublishManifestIdentity
	{
		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "displayName", EmitDefaultValue = false)]
		public string DisplayName { get; set; }

		[DataMember(Name = "icon", EmitDefaultValue = false)]
		public string IconFileName { get; set; }

		[DataMember(Name = "installTargets", EmitDefaultValue = false)]
		public VsixPublishManifestIdentityInstallTarget[] InstallTargets { get; set; }

		[DataMember(Name = "internalName", EmitDefaultValue = false)]
		public string InternalName { get; set; }

		[DataMember(Name = "language", EmitDefaultValue = false)]
		public string Language { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "vsixId", EmitDefaultValue = false)]
		public string VsixId { get; set; }
	}

	[DataContract]
	public class VsixPublishManifestIdentityInstallTarget
	{
		[DataMember(Name = "sku", EmitDefaultValue = false)]
		public string SKU { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }
	}

	[DataContract]
	public class VsixPublishManifestAssetFile
	{
		[DataMember(Name = "pathOnDisk", EmitDefaultValue = false)]
		public string AssetFileName { get; set; }

		[DataMember(Name = "targetPath", EmitDefaultValue = false)]
		public string TargetPath { get; set; }
	}
}