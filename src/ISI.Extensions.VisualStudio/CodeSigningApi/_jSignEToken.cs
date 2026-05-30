#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		private bool jSignEToken(Microsoft.Extensions.Logging.ILogger logger, DTOs.ISignRequest request, string[] fileNames)
		{
			var arguments = new List<string>();

			arguments.Add("--storetype ETOKEN");

			if (!string.IsNullOrWhiteSpace(request.CertificatePassword))
			{
				arguments.Add($"--storepass \"{request.CertificatePassword}\"");
			}

			if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha256)
			{
				arguments.Add("--alg SHA-256");
			}
			else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha384)
			{
				arguments.Add("--alg SHA-384");
			}
			else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha512)
			{
				arguments.Add("--alg SHA-512");
			}

			if (request.TimeStampUri != null)
			{
				arguments.Add($"--tsaurl \"{request.TimeStampUri}\"");
			}

			var workingDirectories = new HashSet<string>(fileNames.Select(System.IO.Path.GetDirectoryName), StringComparer.CurrentCultureIgnoreCase);

			if (workingDirectories.Count == 1)
			{
				arguments.AddRange(fileNames.Select(fileName => $"\"{System.IO.Path.GetFileName(fileName)}\""));
			}
			else
			{
				arguments.AddRange(fileNames.Select(fileName => $"\"{fileName}\""));
			}

			var processRequest = new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "jsign",
				Arguments = arguments,
				Logger = (fileNames.NullCheckedCount() == 1 ? null : logger),
			};

			if (workingDirectories.Count == 1)
			{
				processRequest.WorkingDirectory = workingDirectories.First();
			}

			var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(processRequest);

			if (waitForProcessResponse.Errored)
			{
				Logger.LogError(waitForProcessResponse.Output);

				return false;
			}

			logger.LogInformation($"Signed files: {string.Join(", ", fileNames.Select(System.IO.Path.GetFileName))}");

			return true;
		}
	}
}
