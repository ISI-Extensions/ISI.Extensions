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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.VisualStudio.SerializableModels
{
	[DataContract]
	public class VisualStudioSettings
	{
		[DataMember(Name = "formLocationAndSizes", EmitDefaultValue = false)]
		public VisualStudioSettingsFormLocationAndSize[] FormLocationAndSizes { get; set; }

		[DataMember(Name = "maxCheckDirectoryDepth", EmitDefaultValue = false)]
		public int MaxCheckDirectoryDepth { get; set; } = 5;

		[DataMember(Name = "previouslySelectedSolutionFilterKeys", EmitDefaultValue = false)]
		public string[] PreviouslySelectedSolutionFilterKeys { get; set; }

		[DataMember(Name = "refreshSolutionsExcludePathFilters", EmitDefaultValue = false)]
		public string[] RefreshSolutionsExcludePathFilters { get; set; }

		[DataMember(Name = "refreshSolutionsPreviouslySelectedSolutions", EmitDefaultValue = false)]
		public string[] RefreshSolutionsPreviouslySelectedSolutions { get; set; }

		[DataMember(Name = "runMigrationToolsExcludePathFilters", EmitDefaultValue = false)]
		public string[] RunMigrationToolsExcludePathFilters { get; set; }

		[DataMember(Name = "runMigrationToolsPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] RunMigrationToolsPreviouslySelectedProjectKeys { get; set; }

		[DataMember(Name = "runMicroServicesExcludePathFilters", EmitDefaultValue = false)]
		public string[] RunMicroServicesExcludePathFilters { get; set; }

		[DataMember(Name = "runMicroServicesPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] RunMicroServicesPreviouslySelectedProjectKeys { get; set; }

		[DataMember(Name = "runWindowsServicesExcludePathFilters", EmitDefaultValue = false)]
		public string[] RunWindowsServicesExcludePathFilters { get; set; }

		[DataMember(Name = "runWindowsServicesPreviouslySelectedProjectKeys", EmitDefaultValue = false)]
		public string[] RunWindowsServicesPreviouslySelectedProjectKeys { get; set; }
	}
}
