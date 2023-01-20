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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi 
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.Serialization.ISerialization Serialization { get; }
		protected VisualStudioSettings VisualStudioSettings { get; }
		protected ISI.Extensions.Scm.BuildScriptApi BuildScriptApi { get; }
		protected ISI.Extensions.Scm.SourceControlClientApi SourceControlClientApi { get; }
		protected ISI.Extensions.VisualStudio.CodeGenerationApi CodeGenerationApi { get; }
		protected ISI.Extensions.VisualStudio.ProjectApi ProjectApi { get; }
		protected ISI.Extensions.Nuget.NugetApi NugetApi { get; }

		public SolutionApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.Serialization.ISerialization serialization,
			VisualStudioSettings visualStudioSettings,
			ISI.Extensions.Scm.BuildScriptApi buildScriptApi,
			ISI.Extensions.Scm.SourceControlClientApi sourceControlClientApi,
			ISI.Extensions.VisualStudio.CodeGenerationApi codeGenerationApi,
			ISI.Extensions.VisualStudio.ProjectApi projectApi,
			ISI.Extensions.Nuget.NugetApi nugetApi)
		{
			Logger = logger;
			Serialization = serialization;
			VisualStudioSettings = visualStudioSettings;
			BuildScriptApi = buildScriptApi;
			SourceControlClientApi = sourceControlClientApi;
			CodeGenerationApi = codeGenerationApi;
			ProjectApi = projectApi;
			NugetApi = nugetApi;
		}
	}
}