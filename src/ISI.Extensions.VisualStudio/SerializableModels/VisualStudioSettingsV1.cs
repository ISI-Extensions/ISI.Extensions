﻿#region Copyright & License
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
	[ISI.Extensions.Serialization.SerializerContractUuid("800ce58a-224a-4dbe-bb43-847a0feaab30")]
	public class VisualStudioSettingsV1 : IVisualStudioSettings
	{
		public static IVisualStudioSettings ToSerializable(LOCALENTITIES.VisualStudioSettings source)
		{
			return new VisualStudioSettingsV1()
			{
				FormLocationAndSizes = source.FormLocationAndSizes.ToNullCheckedArray(VisualStudioSettingsFormLocationAndSizeV1.ToSerializable),
				MaxCheckDirectoryDepth = source.MaxCheckDirectoryDepth,
				DefaultExcludePathFilters = source.DefaultExcludePathFilters.ToNullCheckedArray(),
				PreviouslySelectedSolutionFilterKeys = source.PreviouslySelectedSolutionFilterKeys.ToNullCheckedArray(),
				RefreshSolutionsExcludePathFilters = source.RefreshSolutionsExcludePathFilters.ToNullCheckedArray(),
				RefreshSolutionsPreviouslySelectedSolutions = source.RefreshSolutionsPreviouslySelectedSolutions.ToNullCheckedArray(),
				RunServicesExcludePathFilters = source.RunServicesExcludePathFilters.ToNullCheckedArray(),
				RunServicesPreviouslySelectedProjectKeys = source.RunServicesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
				UpgradeNugetPackagesExcludePathFilters = source.UpgradeNugetPackagesExcludePathFilters.ToNullCheckedArray(),
				UpgradeNugetPackagesPreviouslySelectedProjectKeys = source.UpgradeNugetPackagesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
			};
		}

		public LOCALENTITIES.VisualStudioSettings Export()
		{
			return new LOCALENTITIES.VisualStudioSettings()
			{
				FormLocationAndSizes = FormLocationAndSizes.ToNullCheckedArray(formLocationAndSize => formLocationAndSize.Export()),
				MaxCheckDirectoryDepth = MaxCheckDirectoryDepth,
				DefaultExcludePathFilters = DefaultExcludePathFilters.ToNullCheckedArray(),
				PreviouslySelectedSolutionFilterKeys = PreviouslySelectedSolutionFilterKeys.ToNullCheckedArray(),
				RefreshSolutionsExcludePathFilters = RefreshSolutionsExcludePathFilters.ToNullCheckedArray(),
				RefreshSolutionsPreviouslySelectedSolutions = RefreshSolutionsPreviouslySelectedSolutions.ToNullCheckedArray(),
				RunServicesExcludePathFilters = RunServicesExcludePathFilters.ToNullCheckedArray(),
				RunServicesPreviouslySelectedProjectKeys = RunServicesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
				UpgradeNugetPackagesExcludePathFilters = UpgradeNugetPackagesExcludePathFilters.ToNullCheckedArray(),
				UpgradeNugetPackagesPreviouslySelectedProjectKeys = UpgradeNugetPackagesPreviouslySelectedProjectKeys.ToNullCheckedArray(),
			};
		}

		[DataMember(Name = "formLocationAndSizes", EmitDefaultValue = false)]
		public IVisualStudioSettingsFormLocationAndSize[] FormLocationAndSizes { get; set; }

		[DataMember(Name = "maxCheckDirectoryDepth", EmitDefaultValue = false)]
		public int MaxCheckDirectoryDepth { get; set; } = 5;

		[DataMember(Name = "defaultExcludePathFilters", EmitDefaultValue = false)]
		public string[] DefaultExcludePathFilters { get; set; }

		[DataMember(Name = "previouslySelectedSolutionFilterKeys", EmitDefaultValue = false)]
		public string[] PreviouslySelectedSolutionFilterKeys { get; set; }

		[DataMember(Name = "refreshSolutionsExcludePathFilters", EmitDefaultValue = false)]
		public string[] RefreshSolutionsExcludePathFilters { get; set; }

		[DataMember(Name = "refreshSolutionsPreviouslySelectedSolutions", EmitDefaultValue = false)]
		public string[] RefreshSolutionsPreviouslySelectedSolutions { get; set; }

		[DataMember(Name = "runServicesExcludePathFilters", EmitDefaultValue = false)]
		public string[] RunServicesExcludePathFilters { get; set; }

		[DataMember(Name = "runServicesPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] RunServicesPreviouslySelectedProjectKeys { get; set; }

		[DataMember(Name = "upgradeNugetPackagesExcludePathFilters", EmitDefaultValue = false)]
		public string[] UpgradeNugetPackagesExcludePathFilters { get; set; }

		[DataMember(Name = "upgradeNugetPackagesPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] UpgradeNugetPackagesPreviouslySelectedProjectKeys { get; set; }
	}
}
