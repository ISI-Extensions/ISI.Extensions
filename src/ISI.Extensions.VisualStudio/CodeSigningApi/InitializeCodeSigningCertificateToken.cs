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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		public DTOs.InitializeCodeSigningCertificateTokenResponse InitializeCodeSigningCertificateToken(DTOs.IInitializeCodeSigningCertificateTokenRequest request)
		{
			var response = new DTOs.InitializeCodeSigningCertificateTokenResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			if (!string.IsNullOrWhiteSpace(request.CodeSigningCertificateTokenCertificateFileName) && System.IO.File.Exists(request.CodeSigningCertificateTokenCertificateFileName))
			{
				using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
				{
					var dllSourceToSignFullName = this.GetType().Assembly.Location;

					var dllToSignFullName = System.IO.Path.Combine(tempDirectory.FullName, System.IO.Path.GetFileName(dllSourceToSignFullName));

					System.IO.File.Copy(dllSourceToSignFullName, dllToSignFullName);

					var arguments = new List<string>();
					arguments.Add("sign");
					arguments.Add("/as");
					arguments.Add("/force");
					arguments.Add("/fd SHA256");
					arguments.Add(string.Format("/f \"{0}\"", request.CodeSigningCertificateTokenCertificateFileName));
					arguments.Add(string.Format("/csp \"{0}\"", request.CodeSigningCertificateTokenCryptographicProvider));
					arguments.Add(string.Format("/k \"[{{{{{1}}}}}]={0}\"", request.CodeSigningCertificateTokenContainerName, request.CodeSigningCertificateTokenPassword));
					arguments.Add(string.Format("\"{0}\"", dllToSignFullName));

					ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						ProcessExeFullName = "signtool.exe",
						Arguments = arguments,
						Logger = new NullLogger(),
					});

					logger.LogInformation("Code Signing Certificate Token has been Initialized");
				}
			}

			return response;
		}
	}
}