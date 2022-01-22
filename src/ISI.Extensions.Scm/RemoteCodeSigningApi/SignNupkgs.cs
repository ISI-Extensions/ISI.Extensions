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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RemoteCodeSigningApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.RemoteCodeSigningApi;

namespace ISI.Extensions.Scm
{
	public partial class RemoteCodeSigningApi
	{
		public class SignNupkgsPath
		{
			public const string CreateSignNupkgsBatch = "create-sign-nupkgs-batch";
			public const string AddNupkgToSignNupkgsBatch = "add-nupkg-to-sign-nupkgs-batch";
			public const string ExecuteSignNupkgsBatch = "execute-sign-nupkgs-batch";
			public const string GetNupkgFromSignNupkgsBatch = "get-nupkg-from-sign-nupkgs-batch";
		}

		public class SignNupkgsQueryStringParameter
		{
			public const string SignNupkgsBatchUuid = "signNupkgsBatchUuid";
			public const string NupkgUuid = "nupkgUuid";
		}

		public DTOs.SignNupkgsResponse SignNupkgs(DTOs.SignNupkgsRequest request)
		{
			var response = new DTOs.SignNupkgsResponse()
			{
				Success = true,
			};

			var signNupkgsBatchUuid = Guid.NewGuid();

			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString(string.Format("api/{0}", SignNupkgsPath.CreateSignNupkgsBatch));

				var createSignNupkgsBatchRequest = new SerializableDTOs.CreateSignNupkgsBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignNupkgsBatchUuid = signNupkgsBatchUuid,
					OverwriteAnyExistingSignature = request.OverwriteAnyExistingSignature,
				};

				try
				{
					var createSignNupkgsBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.CreateSignNupkgsBatchRequest, SerializableDTOs.CreateSignNupkgsBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), createSignNupkgsBatchRequest, false);

					if (createSignNupkgsBatchResponse.Error != null)
					{
						throw createSignNupkgsBatchResponse.Error.Exception;
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignNupkgs Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			var nupkgs = new Dictionary<Guid, string>();

			if (response.Success)
			{
				foreach (var nupkgFullName in request.NupkgFullNames)
				{
					var nupkgUuid = Guid.NewGuid();

					nupkgs.Add(nupkgUuid, nupkgFullName);

					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString(string.Format("api/{0}", SignNupkgsPath.AddNupkgToSignNupkgsBatch));
					uri.AddQueryStringParameter(SignNupkgsQueryStringParameter.SignNupkgsBatchUuid, signNupkgsBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
					uri.AddQueryStringParameter(SignNupkgsQueryStringParameter.NupkgUuid, nupkgUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						using (var stream = System.IO.File.OpenRead(nupkgFullName))
						{
							ISI.Extensions.WebClient.Upload.UploadFile(uri.Uri, GetHeaders(request), stream, System.IO.Path.GetFileName(nupkgFullName), "nupkg");
						}
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SignNupkgs Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			if (response.Success)
			{
				var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
				uri.SetPathAndQueryString(string.Format("api/{0}", SignNupkgsPath.ExecuteSignNupkgsBatch));

				var executeSignNupkgsBatchRequest = new SerializableDTOs.ExecuteSignNupkgsBatchRequest()
				{
					Password = request.RemoteCodeSigningServicePassword,
					SignNupkgsBatchUuid = signNupkgsBatchUuid,
				};

				try
				{
					var executeSignNupkgsBatchResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.ExecuteSignNupkgsBatchRequest, SerializableDTOs.ExecuteSignNupkgsBatchResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), executeSignNupkgsBatchRequest, false);

					if (executeSignNupkgsBatchResponse.Error != null)
					{
						throw executeSignNupkgsBatchResponse.Error.Exception;
					}

					Logger.LogInformation(executeSignNupkgsBatchResponse.Response.StatusTrackerKey);

					response.Success = Watch(request.RemoteCodeSigningServiceUrl, request.RemoteCodeSigningServicePassword, executeSignNupkgsBatchResponse.Response.StatusTrackerKey);
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "SignNupkgs Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}
			
			if (response.Success)
			{
				foreach (var nupkg in nupkgs)
				{
					var uri = new UriBuilder(request.RemoteCodeSigningServiceUrl);
					uri.SetPathAndQueryString(string.Format("api/{0}", SignNupkgsPath.GetNupkgFromSignNupkgsBatch));
					uri.AddQueryStringParameter(SignNupkgsQueryStringParameter.SignNupkgsBatchUuid, signNupkgsBatchUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
					uri.AddQueryStringParameter(SignNupkgsQueryStringParameter.NupkgUuid, nupkg.Key.Formatted(GuidExtensions.GuidFormat.WithHyphens));

					try
					{
						System.IO.File.Delete(nupkg.Value);

						using (var stream = System.IO.File.OpenWrite(nupkg.Value))
						{
							ISI.Extensions.WebClient.Download.DownloadFile(uri.Uri, GetHeaders(request), stream);
						}
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SignNupkgs Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			return response;
		}
	}
}