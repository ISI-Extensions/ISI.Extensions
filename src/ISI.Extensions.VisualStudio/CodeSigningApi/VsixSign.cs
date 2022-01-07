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
		public DTOs.VsixSignResponse VsixSign(DTOs.VsixSignRequest request)
		{
			var response = new DTOs.VsixSignResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var certificate = (string.IsNullOrWhiteSpace(request.CertificatePath) ? GetCertificateFromCertificateStore(request.CertificateStoreName, request.CertificateStoreLocation, request.CertificateSubjectName, request.CertificateFingerprint) : GetCertificateFromPfx(request.CertificatePath, request.CertificatePassword));

			var signingKey = GetSigningKeyFromCertificate(certificate);

			using (var package = OpenVsixSignTool.Core.OpcPackage.Open(request.VsixFullName, OpenVsixSignTool.Core.OpcPackageFileMode.ReadWrite))
			{
				if (package.GetSignatures().Any() && !request.OverwriteAnyExistingSignature)
				{
					throw new Exception("The VSIX is already signed.");
				}

				var signBuilder = package.CreateSignatureBuilder();

				signBuilder.EnqueueNamedPreset<OpenVsixSignTool.Core.VSIXSignatureBuilderPreset>();

				var hashAlgorithmName = GetHashAlgorithmName(request.DigestAlgorithm);

				var signingConfiguration = new OpenVsixSignTool.Core.SignConfigurationSet(hashAlgorithmName, hashAlgorithmName, signingKey, certificate);

				var signature = signBuilder.Sign(signingConfiguration);

				if (request.TimeStampUri != null)
				{
					var timestampBuilder = signature.CreateTimestampBuilder();

					var signResponse = timestampBuilder.SignAsync(request.TimeStampUri, GetHashAlgorithmName(request.TimeStampDigestAlgorithm)).GetAwaiter().GetResult();

					if (signResponse == OpenVsixSignTool.Core.TimestampResult.Failed)
					{
						throw new Exception("TimeStamping Signature Failed");
					}
				}

				logger.LogInformation("{0} has been signed", request.VsixFullName);
			}

			return response;
		}
	}
}