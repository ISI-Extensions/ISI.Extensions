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
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.MSBuildApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class MSBuildApi
	{
		public DTOs.MSBuildResponse MSBuild(DTOs.MSBuildRequest request)
		{
			var response = new DTOs.MSBuildResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var arguments = new List<string>();

			arguments.Add(string.Format("\"{0}\"", request.FullName));

			if (request.Options.MaxCpuCount.HasValue)
			{
				arguments.Add(request.Options.MaxCpuCount > 0 ? string.Format("/m:{0}", request.Options.MaxCpuCount) : "/m");
			}

			if (request.Options.DetailedSummary.GetValueOrDefault())
			{
				arguments.Add("/ds");
			}

			if (request.Options.NoConsoleLogger.GetValueOrDefault())
			{
				arguments.Add("/noconlog");
			}

			if (request.Options.NoLogo.GetValueOrDefault())
			{
				arguments.Add("/nologo");
			}

			// Set the verbosity.
			if (request.Options.Verbosity.HasValue)
			{
				arguments.Add(string.Format("/v:{0}", request.Options.Verbosity.GetKey().ToLower()));
			}

			if (request.Options.NodeReuse.HasValue)
			{
				arguments.Add(string.Format("/nr:{0}", request.Options.NodeReuse.TrueFalse(textCase: BooleanExtensions.TextCase.Lower)));
			}

			// Got a specific configuration in mind?
			if (!string.IsNullOrWhiteSpace(request.Options.Configuration))
			{
				// Add the configuration as a property.
				arguments.Add(string.Format("/p:Configuration=\"{0}\"", request.Options.Configuration));
			}



			// Set include symbols?
			if (request.Options.IncludeSymbols.HasValue)
			{
				arguments.Add(string.Format("/p:IncludeSymbols={0}", request.Options.IncludeSymbols.TrueFalse(textCase: BooleanExtensions.TextCase.Lower)));
			}

			// Set symbol package format?
			if (!string.IsNullOrWhiteSpace(request.Options.SymbolPackageFormat))
			{
				arguments.Add(string.Format("/p:SymbolPackageFormat={0}", request.Options.SymbolPackageFormat));
			}

			// Set Continuous Integration Build?
			if (request.Options.ContinuousIntegrationBuild.HasValue)
			{
				arguments.Add(string.Format("/p:ContinuousIntegrationBuild={0}", request.Options.ContinuousIntegrationBuild.TrueFalse(textCase: BooleanExtensions.TextCase.Lower)));
			}

			// Got any properties?
			if (request.Options.Properties.Count > 0)
			{
				foreach (var propertyKey in request.Options.Properties.AllKeys)
				{
					arguments.Add(string.Format("/p:{0}={1}", propertyKey, string.Join(",", request.Options.Properties.GetValues(propertyKey))));
				}
			}

			// Got any targets?
			if (request.Options.Targets.Count > 0)
			{
				var targets = string.Join(";", request.Options.Targets);
				arguments.Add(string.Concat("/target:", targets));
			}
			else
			{
				// Should use implicit target?
				if (!request.Options.NoImplicitTarget.GetValueOrDefault())
				{
					// Use default target.
					arguments.Add("/target:Build");
				}
			}




			// Invoke restore target before any other target?
			if (request.Options.Restore)
			{
				arguments.Add("/restore");
			}

			// Set restore locked mode?
			if (request.Options.RestoreLockedMode.HasValue)
			{
				arguments.Add(string.Format("/p:RestoreLockedMode={0}", request.Options.RestoreLockedMode.Value.TrueFalse(textCase: BooleanExtensions.TextCase.Lower)));
			}
			



			var processRequest = new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = logger,
				ProcessExeFullName = GetMSBuildExeFullName(new()
				{
					MsBuildPlatform = request.MsBuildPlatform,
					MsBuildVersion = request.MsBuildVersion,
				}).MSBuildExeFullName,
				Arguments = arguments,
			};

			logger.Log(LogLevel.Information, string.Format("\"{0}\" {1}", processRequest.ProcessExeFullName, string.Join(" ", processRequest.Arguments)));

			var processResponse = ISI.Extensions.Process.WaitForProcessResponse(processRequest);

			if (processResponse.Errored)
			{
				throw new("Error Executing MSBuild.exe");
			}

			return response;
		}
	}
}