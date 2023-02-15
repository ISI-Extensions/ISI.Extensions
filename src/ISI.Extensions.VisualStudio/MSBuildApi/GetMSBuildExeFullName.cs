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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.MSBuildApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class MSBuildApi
	{
		public DTOs.GetMSBuildExeFullNameResponse GetMSBuildExeFullName(DTOs.GetMSBuildExeFullNameRequest request)
		{
			var response = new DTOs.GetMSBuildExeFullNameResponse();

			if (request.MsBuildVersion == MSBuildVersion.Automatic)
			{
				request.MsBuildVersion = MSBuildVersion.Latest;
			}
			if (request.MsBuildVersion == MSBuildVersion.Latest)
			{
				request.MsBuildVersion = MSBuildVersion.MSBuild17;
			}

			if (request.MsBuildPlatform == MSBuildPlatform.Automatic)
			{
				request.MsBuildPlatform = (System.Environment.Is64BitOperatingSystem ? MSBuildPlatform.x64 : MSBuildPlatform.x86);
			}


			var arguments = new[]
			{
					"-products *",
					"-requires Microsoft.Component.MSBuild",
					"-property installationPath",
				};

			var processResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				//Logger = logger,
				ProcessExeFullName = VsWhereApi.GetVsWhereExeFullName(new()).VsWhereExeFullName,
				Arguments = arguments,
			});

			if (processResponse.Errored)
			{
				throw new Exception("Cannot find or get VsWhere.exe");
			}

			var msBuildExeFullNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			void addIfExists(string msBuildExeFullName)
			{
				if (System.IO.File.Exists(msBuildExeFullName))
				{
					msBuildExeFullNames.Add(msBuildExeFullName);
				}
			}

			foreach (var visualStudioPath in processResponse.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
			{
				addIfExists(System.IO.Path.Combine(visualStudioPath, "MSBuild", "Current", "Bin", "MSBuild.exe"));
				addIfExists(System.IO.Path.Combine(visualStudioPath, "MSBuild", "Current", "Bin", "amd64", "MSBuild.exe"));
			}

			var vsVersions = new[]
			{
				"Enterprise",
				"Professional",
				"Community",
				"BuildTools"
			};

			if (request.MsBuildPlatform == MSBuildPlatform.x64)
			{
				msBuildExeFullNames.RemoveWhere(msBuildFullName => msBuildFullName.IndexOf(@"\amd64\", StringComparison.InvariantCultureIgnoreCase) < 0);
			}
			else
			{
				msBuildExeFullNames.RemoveWhere(msBuildFullName => msBuildFullName.IndexOf(@"\amd64\", StringComparison.InvariantCultureIgnoreCase) >= 0);
			}

			switch (request.MsBuildVersion)
			{
				case MSBuildVersion.MSBuild16:
					msBuildExeFullNames.RemoveWhere(msBuildFullName => msBuildFullName.IndexOf(@"\2019\", StringComparison.InvariantCultureIgnoreCase) < 0);
					foreach (var vsVersion in vsVersions)
					{
						if (string.IsNullOrWhiteSpace(response.MSBuildExeFullName))
						{
							response.MSBuildExeFullName = msBuildExeFullNames.FirstOrDefault(msBuildFullName => msBuildFullName.IndexOf(string.Format(@"\{0}\", vsVersion), StringComparison.InvariantCultureIgnoreCase) > 0);
						}
					}
					break;

				case MSBuildVersion.MSBuild17:
					msBuildExeFullNames.RemoveWhere(msBuildFullName => msBuildFullName.IndexOf(@"\2022\", StringComparison.InvariantCultureIgnoreCase) < 0);
					foreach (var vsVersion in vsVersions)
					{
						if (string.IsNullOrWhiteSpace(response.MSBuildExeFullName))
						{
							response.MSBuildExeFullName = msBuildExeFullNames.FirstOrDefault(msBuildFullName => msBuildFullName.IndexOf(string.Format(@"\{0}\", vsVersion), StringComparison.InvariantCultureIgnoreCase) > 0);
						}
					}
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			return response;
		}
	}
}