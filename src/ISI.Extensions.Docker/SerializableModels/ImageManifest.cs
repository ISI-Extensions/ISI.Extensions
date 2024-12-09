#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Docker.SerializableModels
{
	[DataContract]
	public class ImageManifest
	{
		[DataMember(Name = "Ref", EmitDefaultValue = false)]
		public string Ref { get; set; }

		[DataMember(Name = "Descriptor", EmitDefaultValue = false)]
		public ImageManifestDescriptor Descriptor { get; set; }

		[DataMember(Name = "Raw", EmitDefaultValue = false)]
		public string Raw { get; set; }

		[DataMember(Name = "SchemaV2Manifest", EmitDefaultValue = false)]
		public ImageManifestSchemaV2Manifest SchemaV2Manifest { get; set; }
	}

	[DataContract]
	public class ImageManifestDescriptor
	{
		[DataMember(Name = "mediaType", EmitDefaultValue = false)]
		public string MediaType { get; set; }

		[DataMember(Name = "digest", EmitDefaultValue = false)]
		public string Digest { get; set; }
		
		[DataMember(Name = "size", EmitDefaultValue = false)]
		public int Size { get; set; }
		
		[DataMember(Name = "platform", EmitDefaultValue = false)]
		public ImageManifestDescriptorPlatform Platform { get; set; }
	}

	[DataContract]
	public class ImageManifestDescriptorPlatform
	{
		[DataMember(Name = "architecture", EmitDefaultValue = false)]
		public string Architecture { get; set; }
		
		[DataMember(Name = "os", EmitDefaultValue = false)]
		public string OS { get; set; }
	}

	[DataContract]
	public class ImageManifestSchemaV2Manifest
	{
		[DataMember(Name = "schemaVersion", EmitDefaultValue = false)]
		public string SchemaVersion { get; set; }
		
		[DataMember(Name = "mediaType", EmitDefaultValue = false)]
		public string MediaType { get; set; }
		
		[DataMember(Name = "config", EmitDefaultValue = false)]
		public ImageManifestSchemaV2ManifestMedia Config { get; set; }
		
		[DataMember(Name = "layers", EmitDefaultValue = false)]
		public ImageManifestSchemaV2ManifestMedia[] Layers { get; set; }
	}

	[DataContract]
	public class ImageManifestSchemaV2ManifestMedia
	{
		[DataMember(Name = "mediaType", EmitDefaultValue = false)]
		public string MediaType { get; set; }
		
		[DataMember(Name = "size", EmitDefaultValue = false)]
		public double Size { get; set; }
		
		[DataMember(Name = "digest", EmitDefaultValue = false)]
		public string Digest { get; set; }
	}
}