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
using LOCALENTITIES = ISI.Extensions.VisualStudio;

namespace ISI.Extensions.VisualStudio.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("9b824062-df6f-4a3b-9fe0-e6f80efe9263")]
	public class SolutionPreferencesV2 : ISolutionPreferences
	{
		public static ISolutionPreferences ToSerializable(LOCALENTITIES.SolutionPreferences source)
		{
			return new SolutionPreferencesV2()
			{
				UpgradeNugetPackagesPriority = source.UpgradeNugetPackagesPriority,
				ExecuteBuildScriptTargetAfterUpgradeNugetPackages = source.ExecuteBuildScriptTargetAfterUpgradeNugetPackages,
				DoNotUpgradePackages = source.DoNotUpgradePackages.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.SolutionPreferences Export()
		{
			return new LOCALENTITIES.SolutionPreferences()
			{
				UpgradeNugetPackagesPriority = UpgradeNugetPackagesPriority,
				ExecuteBuildScriptTargetAfterUpgradeNugetPackages = ExecuteBuildScriptTargetAfterUpgradeNugetPackages,
				DoNotUpgradePackages = DoNotUpgradePackages.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "upgradeNugetPackagesPriority", EmitDefaultValue = false)]
		public int? UpgradeNugetPackagesPriority { get; set; }

		[DataMember(Name = "executeBuildScriptTargetAfterUpgradeNugetPackages", EmitDefaultValue = false)]
		public string ExecuteBuildScriptTargetAfterUpgradeNugetPackages { get; set; }

		[DataMember(Name = "doNotUpgradePackages", EmitDefaultValue = false)]
		public string[] DoNotUpgradePackages { get; set; }
	}
}
