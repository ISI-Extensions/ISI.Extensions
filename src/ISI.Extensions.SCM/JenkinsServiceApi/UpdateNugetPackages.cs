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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.JenkinsServiceApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi;

namespace ISI.Extensions.Scm
{
	public partial class JenkinsServiceApi
	{
		public DTOs.UpdateNugetPackagesResponse UpdateNugetPackages(DTOs.UpdateNugetPackagesRequest request)
		{
			var response = new DTOs.UpdateNugetPackagesResponse();

			var uri = new UriBuilder(request.JenkinsServiceUrl);
			uri.SetPathAndQueryString("api/update-nuget-packages");

			var updateNugetPackagesRequest = new SerializableDTOs.UpdateNugetPackagesRequest()
			{
				Password = request.JenkinsServicePassword,
				SettingsFullName = request.SettingsFullName,
				JenkinsUrl = request.JenkinsUrl,
				JenkinsUserName = request.JenkinsUserName,
				JenkinsApiToken = request.JenkinsApiToken,
				PauseJenkins = request.PauseJenkins,
				PauseJenkinsReason = request.PauseJenkinsReason,
				FinishAllJobsBeforeProcessing = request.FinishAllJobsBeforeProcessing,
				FinishAllJobsBeforeProcessingFilterByJobIdPrefix = request.FinishAllJobsBeforeProcessingFilterByJobIdPrefix,
				FinishAllJobsBeforeProcessingFilterByJobIdSuffix = request.FinishAllJobsBeforeProcessingFilterByJobIdSuffix,
				FinishAllJobsBeforeProcessingExceptForJobIds = request.FinishAllJobsBeforeProcessingExceptForJobIds.ToNullCheckedArray(),
				JobIds = request.JobIds.ToNullCheckedArray(),
				FilterByJobIdPrefix = request.FilterByJobIdPrefix,
				FilterByJobIdSuffix = request.FilterByJobIdSuffix,
				IgnorePackageIds = request.IgnorePackageIds.ToNullCheckedArray(),
				NugetPackageKeys = request.NugetPackageKeys.ToNullCheckedArray(source => new SerializableDTOs.NugetPackageKey()
				{
					PackageId = source.PackageId,
					PackageVersion = source.PackageVersion,
				}),
				UpsertAssemblyRedirectsNugetPackageKeys = request.UpsertAssemblyRedirectsNugetPackageKeys.ToNullCheckedArray(source => new SerializableDTOs.NugetPackageKey()
				{
					PackageId = source.PackageId,
					PackageVersion = source.PackageVersion,
				}),
			};

			try
			{
				var updateNugetPackagesResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.UpdateNugetPackagesRequest, SerializableDTOs.UpdateNugetPackagesResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), updateNugetPackagesRequest, false);

				if (updateNugetPackagesResponse.Error != null)
				{
					throw updateNugetPackagesResponse.Error.Exception;
				}

				Logger.LogInformation(updateNugetPackagesResponse.Response.StatusTrackerKey);

				response.Success = Watch(request.JenkinsServiceUrl, request.JenkinsServicePassword, updateNugetPackagesResponse.Response.StatusTrackerKey);
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "UpdateNugetPackages Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}