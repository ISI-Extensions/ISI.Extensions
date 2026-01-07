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
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.SerializableModels.NugetSearchService
{
	[DataContract]
	public class Package
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public string Type { get; set; } = "Package";

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public string Registration { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "summary", EmitDefaultValue = false)]
		public string Summary { get; set; }

		[DataMember(Name = "title", EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(Name = "iconUrl", EmitDefaultValue = false)]
		public string IconUrl { get; set; }

		[DataMember(Name = "licenseUrl", EmitDefaultValue = false)]
		public string LicenseUrl { get; set; }

		[DataMember(Name = "projectUrl", EmitDefaultValue = false)]
		public string ProjectUrl { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		public string[] Tags { get; set; }

		[DataMember(Name = "authors", EmitDefaultValue = false)]
		public string[] Authors { get; set; }

		[DataMember(Name = "totalDownloads", EmitDefaultValue = false)]
		public long TotalDownloads { get; set; }

		[DataMember(Name = "verified", EmitDefaultValue = false)]
		public bool Verified { get; set; }

		[DataMember(Name = "packageTypes", EmitDefaultValue = false)]
		public PackageType[] PackageTypes { get; set; }

		[DataMember(Name = "versions", EmitDefaultValue = false)]
		public PackageVersion[] Versions { get; set; }
	}

	[DataContract]
	public class PackageType
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }
	}

	[DataContract]
	public class PackageVersion
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string RegistrationLeafUrl { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "downloads", EmitDefaultValue = false)]
		public long Downloads { get; set; }
	}
}
