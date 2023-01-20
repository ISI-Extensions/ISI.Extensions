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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.JenkinsServiceApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi;

namespace ISI.Extensions.Scm
{
	public partial class JenkinsServiceApi
	{
		public const string BackupJenkinsConfigsUrlPath = "backup-jenkins-configs";

		public DTOs.BackupJenkinsConfigsResponse BackupJenkinsConfigs(DTOs.BackupJenkinsConfigsRequest request)
		{
			var response = new DTOs.BackupJenkinsConfigsResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var uri = new UriBuilder(request.JenkinsServiceUrl);
			uri.SetPathAndQueryString(string.Format("api/{0}", BackupJenkinsConfigsUrlPath));

			var updateNugetPackagesRequest = new SerializableDTOs.BackupJenkinsConfigsRequest()
			{
				SettingsFullName = request.SettingsFullName,
				JenkinsUrl = request.JenkinsUrl,
				JenkinsUserName = request.JenkinsUserName,
				JenkinsApiToken = request.JenkinsApiToken,
				JobIds = request.JobIds.ToNullCheckedArray(),
				FilterByJobIdPrefix = request.FilterByJobIdPrefix,
				FilterByJobIdSuffix = request.FilterByJobIdSuffix,
			};

			try
			{
				var backupJenkinsConfigsResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.BackupJenkinsConfigsRequest, SerializableDTOs.BackupJenkinsConfigsResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), updateNugetPackagesRequest, false);

				if (backupJenkinsConfigsResponse.Error != null)
				{
					throw backupJenkinsConfigsResponse.Error.Exception;
				}

				logger.LogInformation(backupJenkinsConfigsResponse.Response.StatusTrackerKey);

				response.Success = Watch(request.JenkinsServiceUrl, request.JenkinsServicePassword, backupJenkinsConfigsResponse.Response.StatusTrackerKey, logger);
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "BackupJenkinsConfigs Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}