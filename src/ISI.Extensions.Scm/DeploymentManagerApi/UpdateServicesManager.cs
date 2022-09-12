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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.DeploymentManagerApi;


namespace ISI.Extensions.Scm
{
	public partial class DeploymentManagerApi
	{
		public DTOs.UpdateServicesManagerResponse UpdateServicesManager(DTOs.UpdateServicesManagerRequest request)
		{
			var endPointVersion = GetEndpointVersion(request.ServicesManagerUrl);

			if (endPointVersion >= 3)
			{
				return UpdateServicesManagerV3(request);
			}

			return UpdateServicesManagerV1(request);
		}

		public DTOs.UpdateServicesManagerResponse UpdateServicesManagerV1(DTOs.UpdateServicesManagerRequest request)
		{
			var response = new DTOs.UpdateServicesManagerResponse();

			Logger.LogInformation(string.Format("UpdateServicesManager, ServicesManagerUrl: {0}", request.ServicesManagerUrl));

			using (var managerClient = ISI.Extensions.Scm.ServiceReferences.ServicesManager.ManagerClient.GetClient(request.ServicesManagerUrl))
			{
				managerClient.Endpoint.Binding.SendTimeout = TimeSpan.FromMinutes(15);
				managerClient.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMinutes(15);

				var updateResponse = managerClient.Update(request.Password);

				response.CurrentVersion = updateResponse.CurrentVersion;
				response.Log = updateResponse.Log;
				response.NewVersion = updateResponse.NewVersion;
				response.SameVersion = updateResponse.SameVersion;
			}

			return response;
		}

		public DTOs.UpdateServicesManagerResponse UpdateServicesManagerV3(DTOs.UpdateServicesManagerRequest request)
		{
			var response = new DTOs.UpdateServicesManagerResponse();

			Logger.LogInformation(string.Format("UpdateServicesManager, ServicesManagerUrl: {0}", request.ServicesManagerUrl));

			var uri = new UriBuilder(request.ServicesManagerUrl);
			uri.SetPathAndQueryString("rest/manager/v3/update-services-manager");

			var restRequest = new SerializableDTOs.UpdateServicesManagerRequest()
			{
				ArtifactVersionDateTimeStampUrl = request.ArtifactVersionDateTimeStampUrl,
				ArtifactDownloadUrl = request.ArtifactDownloadUrl,
			};

			var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.UpdateServicesManagerRequest, SerializableDTOs.UpdateServicesManagerResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request.Password), restRequest, false);

			response.CurrentVersion = restResponse?.Response?.CurrentVersion;
			response.Log = restResponse?.Response?.Log;
			response.NewVersion = restResponse?.Response?.NewVersion;
			response.SameVersion = restResponse?.Response?.SameVersion;

			return response;
		}
	}
}