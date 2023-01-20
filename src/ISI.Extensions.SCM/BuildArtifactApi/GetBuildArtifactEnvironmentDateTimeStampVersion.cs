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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.BuildArtifactApi;

namespace ISI.Extensions.Scm
{
	public partial class BuildArtifactApi
	{
		public DTOs.GetBuildArtifactEnvironmentDateTimeStampVersionResponse GetBuildArtifactEnvironmentDateTimeStampVersion(DTOs.GetBuildArtifactEnvironmentDateTimeStampVersionRequest request)
		{
			var response = new DTOs.GetBuildArtifactEnvironmentDateTimeStampVersionResponse();
			
			Logger.LogInformation(string.Format("GetBuildArtifactEnvironmentDateTimeStampVersion, BuildArtifactManagementUrl: {0}", request.BuildArtifactManagementUrl));

			using (var remoteManagementClient = ISI.Extensions.Scm.ServiceReferences.Scm.RemoteManagementClient.GetClient(request.BuildArtifactManagementUrl))
			{
				response.DateTimeStampVersion = new(remoteManagementClient.GetBuildArtifactEnvironmentDateTimeStampVersionAsync(request.AuthenticationToken, request.ArtifactName, request.Environment).GetAwaiter().GetResult());
			}

			return response;
		}
	}
}