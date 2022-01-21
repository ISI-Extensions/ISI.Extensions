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
		public DTOs.SignAssembliesResponse SignAssemblies(DTOs.SignAssembliesRequest request)
		{
			var response = new DTOs.SignAssembliesResponse()
			{
				Success = true,
			};

			var signAssembliesBatchUuid = Guid.NewGuid();

			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString("api/create-sign-assemblies-batch");

				var createSignAssembliesBatchRequest = new SerializableDTOs.CreateSignAssembliesBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignAssembliesBatchUuid = signAssembliesBatchUuid,
					OverwriteAnyExistingSignature = request.OverwriteAnyExistingSignature,
				};

				try
				{
					var createSignAssembliesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.CreateSignAssembliesBatchRequest, SerializableDTOs.CreateSignAssembliesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), createSignAssembliesBatchRequest, false);

					if (createSignAssembliesBatchResponse.Error != null)
					{
						throw createSignAssembliesBatchResponse.Error.Exception;
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignAssemblies Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			if (response.Success)
			{
				foreach (var assemblyFullName in request.AssemblyFullNames)
				{
					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString("api/add-assembly-to-sign-assemblies-batch");
					uri.AddQueryStringParameter("signAssembliesBatchUuid", signAssembliesBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						using (var stream = System.IO.File.OpenRead(assemblyFullName))
						{
							ISI.Extensions.WebClient.Upload.UploadFile(uri.Uri, null, stream, System.IO.Path.GetFileName(assemblyFullName), "assembly");
						}
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SignAssemblies Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			if (response.Success)
			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString("api/execute-sign-assemblies-batch");

				var executeSignAssembliesBatchRequest = new SerializableDTOs.ExecuteSignAssembliesBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignAssembliesBatchUuid = signAssembliesBatchUuid,
				};

				try
				{
					var executeSignAssembliesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.ExecuteSignAssembliesBatchRequest, SerializableDTOs.ExecuteSignAssembliesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), executeSignAssembliesBatchRequest, false);

					if (executeSignAssembliesBatchResponse.Error != null)
					{
						throw executeSignAssembliesBatchResponse.Error.Exception;
					}

					Logger.LogInformation(executeSignAssembliesBatchResponse.Response.StatusTrackerKey);

					response.Success = Watch(request.RemoteCodeSigningServiceUrl, request.RemoteCodeSigningServicePassword, executeSignAssembliesBatchResponse.Response.StatusTrackerKey);
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignAssemblies Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			return response;
		}
	}
}