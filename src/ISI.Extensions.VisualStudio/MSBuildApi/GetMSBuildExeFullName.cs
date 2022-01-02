#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

			var vsVersions = new[]
			{
				"Enterprise",
				"Professional",
				"Community",
				"BuildTools"
			};

			var msBuildFullNames = VsWhereApi.GetMSBuildExeFullNames(new ISI.Extensions.VisualStudio.DataTransferObjects.VsWhereApi.GetMSBuildExeFullNamesRequest()).MSBuildExeFullNames.ToList();

			if (request.MsBuildPlatform == MSBuildPlatform.x64)
			{
				msBuildFullNames.RemoveAll(msBuildFullName => msBuildFullName.IndexOf(@"\amd64\", StringComparison.InvariantCultureIgnoreCase) < 0);
			}
			else
			{
				msBuildFullNames.RemoveAll(msBuildFullName => msBuildFullName.IndexOf(@"\amd64\", StringComparison.InvariantCultureIgnoreCase) >= 0);
			}

			switch (request.MsBuildVersion)
			{
				case MSBuildVersion.MSBuild16:
					msBuildFullNames.RemoveAll(msBuildFullName => msBuildFullName.IndexOf(@"\2019\", StringComparison.InvariantCultureIgnoreCase) < 0);
					foreach (var vsVersion in vsVersions)
					{
						if (string.IsNullOrWhiteSpace(response.MSBuildExeFullName))
						{
							response.MSBuildExeFullName = msBuildFullNames.FirstOrDefault(msBuildFullName => msBuildFullName.IndexOf(string.Format(@"\{0}\", vsVersion), StringComparison.InvariantCultureIgnoreCase) > 0);
						}
					}
					break;
				case MSBuildVersion.MSBuild17:
					msBuildFullNames.RemoveAll(msBuildFullName => msBuildFullName.IndexOf(@"\2022\", StringComparison.InvariantCultureIgnoreCase) < 0);
					foreach (var vsVersion in vsVersions)
					{
						if (string.IsNullOrWhiteSpace(response.MSBuildExeFullName))
						{
							response.MSBuildExeFullName = msBuildFullNames.FirstOrDefault(msBuildFullName => msBuildFullName.IndexOf(string.Format(@"\{0}\", vsVersion), StringComparison.InvariantCultureIgnoreCase) > 0);
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