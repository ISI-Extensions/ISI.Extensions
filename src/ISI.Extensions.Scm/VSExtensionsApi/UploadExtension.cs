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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.VSExtensionsApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Scm
{
	public partial class VSExtensionsApi
	{
		public DTOs.UploadExtensionResponse UploadExtension(DTOs.UploadExtensionRequest request)
		{
			var response = new DTOs.UploadExtensionResponse();
			
			var repositoryUri = new UriBuilder(request.RepositoryUrl);
			repositoryUri.AddDirectoryToPath("gallery/upload");
			repositoryUri.AddQueryStringParameter("authenticationToken", request.AuthenticationToken);

			Logger.LogInformation(string.Format("UploadArtifact, RepositoryUrl: {0}", repositoryUri.Uri));
			Logger.LogInformation(string.Format("UploadArtifact, VsixFullName: {0}", request.VsixFullName));

			repositoryUri.AddQueryStringParameter("authenticationToken", request.AuthenticationToken);

			var formValues = new System.Collections.Specialized.NameValueCollection();

			var tryAttemptsLeft = request.MaxTries;
			while (tryAttemptsLeft > 0)
			{
				try
				{
					using (System.IO.Stream stream = System.IO.File.OpenRead(request.VsixFullName))
					{
						ISI.Extensions.WebClient.Upload.UploadFile(repositoryUri.Uri, new(), stream, request.VsixFullName, "uploadFile", formValues);
					}

					tryAttemptsLeft = 0;
				}
				catch (Exception exception)
				{
					Logger.LogInformation(exception.ErrorMessageFormatted());

					tryAttemptsLeft--;
					if (tryAttemptsLeft < 0)
					{
						throw;
					}

					System.Threading.Thread.Sleep(5000);
				}
			}

			return response;
		}
	}
}