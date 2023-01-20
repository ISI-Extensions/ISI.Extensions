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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RemoteCodeSigningApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.RemoteCodeSigningApi;

namespace ISI.Extensions.Scm
{
	public partial class RemoteCodeSigningApi
	{
		public const string CreateSignVsixesBatchUrlPath = "create-sign-vsixes-batch";
		public const string AddVsixToSignVsixesBatchUrlPath = "add-vsix-to-sign-vsixes-batch";
		public const string ExecuteSignVsixesBatchUrlPath = "execute-sign-vsixes-batch";
		public const string GetVsixFromSignVsixesBatchUrlPath = "get-vsix-from-sign-vsixes-batch";

		public const string SignVsixesBatchUuidQueryStringParameter = "signVsixesBatchUuid";
		public const string VsixUuidQueryStringParameter = "vsixUuid";
		public const string VsixFileQueryStringParameter = "vsix";

		public DTOs.SignVsixesResponse SignVsixes(DTOs.SignVsixesRequest request)
		{
			var response = new DTOs.SignVsixesResponse()
			{
				Success = true,
			};

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var signVsixesBatchUuid = Guid.NewGuid();

			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString(string.Format("api/{0}", CreateSignVsixesBatchUrlPath));

				var createSignVsixesBatchRequest = new SerializableDTOs.CreateSignVsixesBatchRequest()
				{
					SignVsixesBatchUuid = signVsixesBatchUuid,
					OverwriteAnyExistingSignature = request.OverwriteAnyExistingSignature,
					Verbosity = request.Verbosity,
				};

				try
				{
					var createSignVsixesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.CreateSignVsixesBatchRequest, SerializableDTOs.CreateSignVsixesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), createSignVsixesBatchRequest, false);

					if (createSignVsixesBatchResponse.Error != null)
					{
						throw createSignVsixesBatchResponse.Error.Exception;
					}
				}
				catch (Exception exception)
				{
					logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			var vsixes = new Dictionary<Guid, string>();

			if (response.Success)
			{
				foreach (var vsixFullName in request.VsixFullNames)
				{
					var vsixUuid = Guid.NewGuid();

					vsixes.Add(vsixUuid, vsixFullName);

					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString(string.Format("api/{0}", AddVsixToSignVsixesBatchUrlPath));
					uri.AddQueryStringParameter(SignVsixesBatchUuidQueryStringParameter, signVsixesBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
					uri.AddQueryStringParameter(VsixUuidQueryStringParameter, vsixUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						using (var stream = System.IO.File.OpenRead(vsixFullName))
						{
							ISI.Extensions.WebClient.Upload.UploadFile(uri.Uri, GetHeaders(request), stream, System.IO.Path.GetFileName(vsixFullName), VsixFileQueryStringParameter);
						}
					}
					catch (Exception exception)
					{
						logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			if (response.Success)
			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString(string.Format("api/{0}", ExecuteSignVsixesBatchUrlPath));

				var executeSignVsixesBatchRequest = new SerializableDTOs.ExecuteSignVsixesBatchRequest()
				{
					SignVsixesBatchUuid = signVsixesBatchUuid,
					RunAsync = request.RunAsync,
				};

				try
				{
					var executeSignVsixesBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.ExecuteSignVsixesBatchRequest, SerializableDTOs.ExecuteSignVsixesBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), executeSignVsixesBatchRequest, false);

					if (executeSignVsixesBatchResponse.Error != null)
					{
						throw executeSignVsixesBatchResponse.Error.Exception;
					}

					Logger.LogInformation(executeSignVsixesBatchResponse.Response.StatusTrackerKey);

					response.Success = Watch(request.RemoteCodeSigningServiceUrl, request.RemoteCodeSigningServicePassword, executeSignVsixesBatchResponse.Response.StatusTrackerKey, logger);
				}
				catch (Exception exception)
				{
					logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			if (response.Success)
			{
				foreach (var vsix in vsixes)
				{
					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString(string.Format("api/{0}", GetVsixFromSignVsixesBatchUrlPath));
					uri.AddQueryStringParameter(SignVsixesBatchUuidQueryStringParameter, signVsixesBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
					uri.AddQueryStringParameter(VsixUuidQueryStringParameter, vsix.Key.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						var outputFullName = vsix.Value;

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							outputFullName = System.IO.Path.Combine(request.OutputDirectory, System.IO.Path.GetFileName(outputFullName));
						}

						System.IO.File.Delete(outputFullName);

						using (var stream = System.IO.File.OpenWrite(outputFullName))
						{
							ISI.Extensions.WebClient.Download.DownloadFile(uri.Uri, GetHeaders(request), stream);
						}
					}
					catch (Exception exception)
					{
						logger.LogError(exception, "SignVsixes Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			return response;
		}
	}
}