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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RemoteCodeSigning;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.RemoteCodeSigning;

namespace ISI.Extensions.Scm
{
	public partial class RemoteCodeSigning
	{
		public DTOs.SignVsixesResponse SignVsixes(DTOs.SignVsixesRequest request)
		{
			var response = new DTOs.SignVsixesResponse()
			{
				Success = true,
			};

			var SignVsixesBatchUuid = Guid.NewGuid();

			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString("api/create-sign-vsixes-batch");

				var createSignVsixesBatchRequest = new SerializableDTOs.CreateSignVsixesBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignVsixesBatchUuid = SignVsixesBatchUuid,
					OverwriteAnyExistingSignature = request.OverwriteAnyExistingSignature,
				};

				try
				{
					var createSignVsixesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.CreateSignVsixesBatchRequest, SerializableDTOs.CreateSignVsixesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), createSignVsixesBatchRequest, false);

					if (createSignVsixesBatchResponse.Error != null)
					{
						throw createSignVsixesBatchResponse.Error.Exception;
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			if (response.Success)
			{
				foreach (var vsixFullName in request.VsixFullNames)
				{
					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString("api/add-vsix-to-sign-vsixes-batch");
					uri.AddQueryStringParameter("SignVsixesBatchUuid", SignVsixesBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						using (var stream = System.IO.File.OpenRead(vsixFullName))
						{
							ISI.Extensions.WebClient.Upload.UploadFile(uri.Uri, null, stream, System.IO.Path.GetFileName(vsixFullName), "vsix");
						}
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			if (response.Success)
			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString("api/execute-sign-vsixes-batch");

				var executeSignVsixesBatchRequest = new SerializableDTOs.ExecuteSignVsixesBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignVsixesBatchUuid = SignVsixesBatchUuid,
				};

				try
				{
					var executeSignVsixesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.ExecuteSignVsixesBatchRequest, SerializableDTOs.ExecuteSignVsixesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), executeSignVsixesBatchRequest, false);

					if (executeSignVsixesBatchResponse.Error != null)
					{
						throw executeSignVsixesBatchResponse.Error.Exception;
					}

					Logger.LogInformation(executeSignVsixesBatchResponse.Response.StatusTrackerKey);

					response.Success = Watch(request.RemoteCodeSigningServiceUrl, request.RemoteCodeSigningServicePassword, executeSignVsixesBatchResponse.Response.StatusTrackerKey);
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			return response;
		}
	}
}