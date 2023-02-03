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

			return new();
		}

		public DTOs.UpdateServicesManagerResponse UpdateServicesManagerV3(DTOs.UpdateServicesManagerRequest request)
		{
			var response = new DTOs.UpdateServicesManagerResponse();

			Logger.LogInformation(string.Format("UpdateServicesManager, ServicesManagerUrl: {0}", request.ServicesManagerUrl));

			var artifactVersion = new DateTimeStampVersion(ISI.Extensions.WebClient.Rest.ExecuteTextGet(request.ArtifactDateTimeStampVersionUrl, GetHeaders(null), true)).Version;

			Logger.LogInformation(string.Format("Artifact Version: {0}", artifactVersion));

			Version GetInstalledVersion()
			{
				try
				{
					var uri = new UriBuilder(request.ServicesManagerUrl);
					uri.SetPathAndQueryString(string.Empty);

					var restResponse = ISI.Extensions.WebClient.Rest.ExecuteGet<ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, new(), false);

					var version = (string)null;
					if ((restResponse?.ResponseHeaders?.TryGetValue(HeaderKey.ServicesManagerVersion, out version)).GetValueOrDefault())
					{
						response.CurrentVersion = version;

						return new(version);
					}
				}
				catch
				{
				}

				return null;
			}

			var installedVersion = GetInstalledVersion();

			if (installedVersion != null)
			{
				Logger.LogInformation(string.Format("Current Version: {0}", installedVersion));
			}

			if ((installedVersion == null) || (installedVersion != artifactVersion))
			{
				var uri = new UriBuilder(request.ServicesManagerUrl);
				uri.SetPathAndQueryString("rest/manager/v3/update-services-manager");

				var restRequest = new SerializableDTOs.UpdateServicesManagerRequest()
				{
					ArtifactDateTimeStampVersionUrl = request.ArtifactDateTimeStampVersionUrl,
					ArtifactDownloadUrl = request.ArtifactDownloadUrl,
				};

#if DEBUG
				var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

				var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.UpdateServicesManagerRequest, SerializableDTOs.UpdateServicesManagerResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request.Password), restRequest, false);

				response.CurrentVersion = restResponse?.Response?.CurrentVersion;
				response.Log = restResponse?.Response?.Log ?? restResponse?.Error?.Content;
				response.NewVersion = restResponse?.Response?.NewVersion;
				response.SameVersion = restResponse?.Response?.SameVersion;

				if (restResponse?.Error?.Exception != null)
				{
					response.Log = string.Format("{0}{1}Exception: {2}", response.Log, Environment.NewLine, restResponse.Error.Exception.ErrorMessageFormatted());
				}
				else
				{
					Logger.LogInformation(string.Format("Waiting to do Verification, Sleeping for {0} seconds", request.VerificationWaitInSeconds));

					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(request.VerificationWaitInSeconds));

					var tryAttemptsLeft = request.VerificationMaxTries;
					while (tryAttemptsLeft > 0)
					{
						installedVersion = GetInstalledVersion();

						if (installedVersion == null)
						{
							tryAttemptsLeft--;

							if (tryAttemptsLeft > 0)
							{
								Logger.LogError(string.Format("Error verifying, Sleeping for {0} seconds", request.VerificationExceptionSleepForInSeconds));

								System.Threading.Thread.Sleep(TimeSpan.FromSeconds(request.VerificationExceptionSleepForInSeconds));
							}
							else
							{
								response.WouldNotStart = true;

								Logger.LogError("Would Not Start");
							}
						}
						else
						{
							Logger.LogInformation(string.Format("Installed Version: {0}", installedVersion));

							tryAttemptsLeft = -1;
						}
					}

					if (!response.WouldNotStart)
					{
						if (installedVersion != artifactVersion)
						{
							Logger.LogInformation("New Version NOT Installed");
						}
						else
						{
							Logger.LogInformation("New Version Installed");
						}
					}
				}
			}
			else
			{
				response.SameVersion = true;
			}

			return response;
		}
	}
}