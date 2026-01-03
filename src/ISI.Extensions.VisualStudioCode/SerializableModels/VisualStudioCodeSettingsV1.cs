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
using LOCALENTITIES = ISI.Extensions.VisualStudioCode;

namespace ISI.Extensions.VisualStudioCode.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("20deed3d-5e96-45e1-9767-b0126f9796e4")]
	public class VisualStudioCodeSettingsV1 : IVisualStudioCodeSettings
	{
		public static IVisualStudioCodeSettings ToSerializable(LOCALENTITIES.VisualStudioCodeSettings source)
		{
			return new VisualStudioCodeSettingsV1()
			{
				FormLocationAndSizes = source.FormLocationAndSizes.ToNullCheckedArray(VisualStudioCodeSettingsFormLocationAndSizeV1.ToSerializable),
				MaxCheckDirectoryDepth = source.MaxCheckDirectoryDepth,
				DefaultExcludePathFilters = source.DefaultExcludePathFilters.ToNullCheckedArray(),
				PreviouslySelectedSolutionFilterKeys = source.PreviouslySelectedSolutionFilterKeys.ToNullCheckedArray(),
				RefreshSolutionsPreviouslySelectedSolutions = source.RefreshSolutionsPreviouslySelectedSolutions.ToNullCheckedArray(),
				UpgradeNodeModulesPreviouslySelectedProjectKeys = source.UpgradeNodeModulesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.VisualStudioCodeSettings Export()
		{
			return new LOCALENTITIES.VisualStudioCodeSettings()
			{
				FormLocationAndSizes = FormLocationAndSizes.ToNullCheckedArray(formLocationAndSize => formLocationAndSize.Export()),
				MaxCheckDirectoryDepth = MaxCheckDirectoryDepth,
				DefaultExcludePathFilters = DefaultExcludePathFilters.ToNullCheckedArray(),
				PreviouslySelectedSolutionFilterKeys = PreviouslySelectedSolutionFilterKeys.ToNullCheckedArray(),
				RefreshSolutionsPreviouslySelectedSolutions = RefreshSolutionsPreviouslySelectedSolutions.ToNullCheckedArray(),
				UpgradeNodeModulesPreviouslySelectedProjectKeys = UpgradeNodeModulesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "formLocationAndSizes", EmitDefaultValue = false)]
		public IVisualStudioCodeSettingsFormLocationAndSize[] FormLocationAndSizes { get; set; }

		[DataMember(Name = "maxCheckDirectoryDepth", EmitDefaultValue = false)]
		public int MaxCheckDirectoryDepth { get; set; } = 5;

		[DataMember(Name = "defaultExcludePathFilters", EmitDefaultValue = false)]
		public string[] DefaultExcludePathFilters { get; set; }

		[DataMember(Name = "previouslySelectedSolutionFilterKeys", EmitDefaultValue = false)]
		public string[] PreviouslySelectedSolutionFilterKeys { get; set; }

		[DataMember(Name = "refreshSolutionsPreviouslySelectedSolutions", EmitDefaultValue = false)]
		public string[] RefreshSolutionsPreviouslySelectedSolutions { get; set; }

		[DataMember(Name = "upgradeNodeModulesPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] UpgradeNodeModulesPreviouslySelectedProjectKeys { get; set; }
	}
}
