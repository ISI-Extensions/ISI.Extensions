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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		public DTOs.SignNupkgsResponse SignNupkgs(DTOs.SignNupkgsRequest request)
		{
			var response = new DTOs.SignNupkgsResponse();

			using (new ISI.Extensions.Windows.ScreenSaverDisabler())
			{
				InitializeCodeSigningCertificateToken(request);

				var logger = new AddToLogLogger(request.AddToLog, Logger);

				var arguments = new List<string>();

				arguments.Add(string.Format(" -Timestamper \"{0}\"", request.TimeStampUri));
				arguments.Add(string.Format(" -TimestampHashAlgorithm \"{0}\"", request.TimeStampDigestAlgorithm.GetAbbreviation()));

				if (!string.IsNullOrWhiteSpace(request.OutputDirectory))
				{
					arguments.Add(string.Format(" -OutputDirectory \"{0}\"", request.OutputDirectory));
				}

				if (string.IsNullOrWhiteSpace(request.CertificateFileName))
				{
					arguments.Add(string.Format(" -CertificateStoreName \"{0}\"", request.CertificateStoreName));
					arguments.Add(string.Format(" -CertificateStoreLocation \"{0}\"", request.CertificateStoreLocation));
					if (!string.IsNullOrWhiteSpace(request.CertificateSubjectName))
					{
						arguments.Add(string.Format(" -CertificateSubjectName \"{0}\"", request.CertificateSubjectName));
					}

					if (!string.IsNullOrWhiteSpace(request.CertificateFingerprint))
					{
						arguments.Add(string.Format(" -CertificateFingerprint \"{0}\"", request.CertificateFingerprint));
					}
				}
				else
				{
					arguments.Add(string.Format(" -CertificatePath \"{0}\"", request.CertificateFileName));
					arguments.Add(string.Format(" -CertificatePassword \"{0}\"", request.CertificatePassword));
				}

				arguments.Add(string.Format(" -HashAlgorithm \"{0}\"", request.DigestAlgorithm.GetAbbreviation()));

				if (request.OverwriteAnyExistingSignature)
				{
					arguments.Append(" -Overwrite");
				}

				arguments.Add(string.Format(" -Verbosity \"{0}\"", request.Verbosity));

				foreach (var nupkgFullName in request.NupkgFullNames)
				{
					logger.LogInformation(string.Format("Signing nuget package \"{0}\"", System.IO.Path.GetFileName(nupkgFullName)));

					var processRequest = new ISI.Extensions.Process.ProcessRequest()
					{
						Logger = logger,
						WorkingDirectory = request.WorkingDirectory,
						ProcessExeFullName = "nuget.exe",
						Arguments = new[]
						{
							string.Format("sign \"{0}\"", nupkgFullName),
							string.Join(" ", arguments),
						}
					};

					if (request.Verbosity == ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningVerbosity.Detailed)
					{
						logger.LogInformation(processRequest.ToString());
					}

					ISI.Extensions.Process.WaitForProcessResponse(processRequest);

					logger.LogInformation(string.Format("Signed nuget package \"{0}\"", System.IO.Path.GetFileName(nupkgFullName)));
				}
			}

			return response;
		}
	}
}