#region Copyright & License
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;
using LOCALENTITIES = ISI.Extensions.Nuget;

namespace ISI.Extensions.Nuget.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("3df16826-77a3-41ac-ad4e-2310f9cdea37")]
	public class NugetPackageKeyTargetFrameworkV1 : INugetPackageKeyTargetFramework
	{
		public static INugetPackageKeyTargetFramework ToSerializable(LOCALENTITIES.NugetPackageKeyTargetFramework source)
		{
			return new NugetPackageKeyTargetFrameworkV1()
			{
				TargetFramework = source.TargetFramework?.DotNetFrameworkName,
				Assemblies = source.Assemblies.ToNullCheckedArray(NugetPackageKeyTargetFrameworkAssemblyV1.ToSerializable),
			};
		}

		public LOCALENTITIES.NugetPackageKeyTargetFramework Export()
		{
			return new LOCALENTITIES.NugetPackageKeyTargetFramework()
			{
				TargetFramework = NuGet.Frameworks.NuGetFramework.Parse(TargetFramework),
				Assemblies = Assemblies.ToNullCheckedArray(assembly => assembly.Export()),
			};
		}

		[DataMember(Name = "targetFramework", EmitDefaultValue = false)]
		public string TargetFramework { get; set; }

		[DataMember(Name = "assemblies", EmitDefaultValue = false)]
		public INugetPackageKeyTargetFrameworkAssembly[] Assemblies { get; set; }
	}
}