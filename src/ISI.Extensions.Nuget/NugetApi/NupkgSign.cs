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
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.NupkgSignResponse NupkgSign(DTOs.NupkgSignRequest request)
		{
			var response = new DTOs.NupkgSignResponse();
			
			var arguments = new List<string>();

			arguments.Add(string.Format(" -Timestamper \"{0}\"", request.TimeStampUri));
			arguments.Add(string.Format(" -TimestampHashAlgorithm \"{0}\"", request.TimeStampDigestAlgorithm.GetAbbreviation()));

			if (!string.IsNullOrWhiteSpace(request.OutputDirectory))
			{
				arguments.Add(string.Format(" -OutputDirectory \"{0}\"", request.OutputDirectory));
			}

			if (string.IsNullOrWhiteSpace(request.CertificatePath))
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
				arguments.Add(string.Format(" -CertificatePath \"{0}\"", request.CertificatePath));
				arguments.Add(string.Format(" -CertificatePassword \"{0}\"", request.CertificatePassword));
			}
			arguments.Add(string.Format(" -HashAlgorithm \"{0}\"", request.DigestAlgorithm.GetAbbreviation()));

			if (request.OverwriteAnyExistingSignature)
			{
				arguments.Append(" -Overwrite");
			}

			arguments.Add(string.Format(" -Verbosity \"{0}\"", request.Verbosity));

			var processExeFullName = GetNugetExeFullName(new DTOs.GetNugetExeFullNameRequest()).NugetExeFullName;

			foreach (var nupkgFullName in request.NupkgFullNames)
			{
				var processRequest = new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = Logger, //new NullLogger(),
					WorkingDirectory = request.WorkingDirectory,
					ProcessExeFullName = processExeFullName,
					Arguments = new[]
					{
						string.Format("sign \"{0}\"", nupkgFullName),
						string.Join(" ", arguments),
					}};

				if (request.Verbosity == ISI.Extensions.Nuget.DataTransferObjects.NugetApi.NupkgSignVerbosity.Detailed)
				{
					Logger.LogInformation(processRequest.ToString());
				}

				ISI.Extensions.Process.WaitForProcessResponse(processRequest);
			}

			return response;
		}
	}
}