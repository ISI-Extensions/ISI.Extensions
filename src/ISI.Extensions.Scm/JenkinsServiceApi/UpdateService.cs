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
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.JenkinsServiceApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi;

namespace ISI.Extensions.Scm
{
	public partial class JenkinsServiceApi
	{
		public const string UpdateServiceUrlPath = "update-service";

		public DTOs.UpdateServiceResponse UpdateService(DTOs.UpdateServiceRequest request)
		{
			var response = new DTOs.UpdateServiceResponse();
						
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var uri = new UriBuilder(request.JenkinsServiceUrl);
			uri.SetPathAndQueryString(string.Format("api/{0}", UpdateServiceUrlPath));

			var updateServiceRequest = new SerializableDTOs.UpdateServiceRequest();

			try
			{
				var updateServiceResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.UpdateServiceRequest, SerializableDTOs.UpdateServiceResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), updateServiceRequest, false);

				if (updateServiceResponse.Error != null)
				{
					throw updateServiceResponse.Error.Exception;
				}

				response.CurrentVersion = updateServiceResponse.Response.CurrentVersion;
				response.NewVersion = updateServiceResponse.Response.NewVersion;
				response.SameVersion = updateServiceResponse.Response.SameVersion;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "UpdateService Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}