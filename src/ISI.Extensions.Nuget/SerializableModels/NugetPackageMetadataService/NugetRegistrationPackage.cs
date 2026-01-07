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
	public class NugetRegistrationPackage
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string NugetRegistrationPackageUrl { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "catalogEntry", EmitDefaultValue = false)]
		public string NugetRegistrationDetailsUrl { get; set; }

		[DataMember(Name = "listed", EmitDefaultValue = false)]
		public bool Listed { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public string DownloadUrl { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public string __published { get => Published.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise); set => Published = value.ToDateTimeNullable(); }
		[IgnoreDataMember]
		public DateTime? Published { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public string Registration { get; set; }

		[DataMember(Name = "@context", EmitDefaultValue = false)]
		public NugetRegistrationPackageContext Context { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPackageContext
	{
		public NugetRegistrationPackageContext()
		{
			Vocab = "http://schema.nuget.org/schema#";
			Xsd = "http://www.w3.org/2001/XMLSchema#";
			CatalogEntry = new NugetRegistrationPackageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Registration = new NugetRegistrationPackageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			PackageContent = new NugetRegistrationPackageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("@id"),
			};
			Published = new NugetRegistrationPackageContextItem()
			{
				Types = new ISI.Extensions.Serialization.SerializableEnumerableOrInstance<string>("xsd:dateTime"),
			};
		}

		[DataMember(Name = "vocab", EmitDefaultValue = false)]
		public string Vocab { get; set; }

		[DataMember(Name = "xsd", EmitDefaultValue = false)]
		public string Xsd { get; set; }

		[DataMember(Name = "catalogEntry", EmitDefaultValue = false)]
		public NugetRegistrationPackageContextItem CatalogEntry { get; set; }

		[DataMember(Name = "registration", EmitDefaultValue = false)]
		public NugetRegistrationPackageContextItem Registration { get; set; }

		[DataMember(Name = "packageContent", EmitDefaultValue = false)]
		public NugetRegistrationPackageContextItem PackageContent { get; set; }

		[DataMember(Name = "published", EmitDefaultValue = false)]
		public NugetRegistrationPackageContextItem Published { get; set; }
	}

	[DataContract]
	public class NugetRegistrationPackageContextItem
	{
		[DataMember(Name = "@id", EmitDefaultValue = false)]
		public string ContextItemKey { get; set; }

		[DataMember(Name = "@type", EmitDefaultValue = false)]
		public ISI.Extensions.Serialization.ISerializableEnumerableOrInstance<string> Types { get; set; }

		[DataMember(Name = "@container", EmitDefaultValue = false)]
		public string Container { get; set; }
	}
}