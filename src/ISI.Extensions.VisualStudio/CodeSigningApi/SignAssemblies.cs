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
		public DTOs.SignAssembliesResponse SignAssemblies(DTOs.SignAssembliesRequest request)
		{
			var response = new DTOs.SignAssembliesResponse();

			using (new ISI.Extensions.Windows.ScreenSaverDisabler())
			{
				InitializeCodeSigningCertificateToken(request);

				var logger = new AddToLogLogger(request.AddToLog, Logger);

				using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
				{
					var assemblyFullNames = request.AssemblyFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

					if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
					{
						for (var fileIndex = 0; fileIndex < assemblyFullNames.Length; fileIndex++)
						{
							var tempAssemblyFullName = System.IO.Path.Combine(tempDirectory.FullName, System.IO.Path.GetFileName(assemblyFullNames[fileIndex]));

							if (System.IO.File.Exists(tempAssemblyFullName))
							{
								System.IO.File.Delete(tempAssemblyFullName);
							}

							System.IO.File.Copy(assemblyFullNames[fileIndex], tempAssemblyFullName);

							assemblyFullNames[fileIndex] = tempAssemblyFullName;
						}
					}

					var arguments = new List<string>();
					arguments.Add("sign");

					if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha256)
					{
						arguments.Add("/fd SHA256");
					}

					if (request.TimeStampUri != null)
					{
						if (request.TimeStampDigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha256)
						{
							arguments.Add("/td SHA256");
							arguments.Add(string.Format("/tr \"{0}\"", request.TimeStampUri));
						}
						else
						{
							arguments.Add(string.Format("/t \"{0}\"", request.TimeStampUri));
						}
					}

					if (!string.IsNullOrWhiteSpace(request.CertificateFileName))
					{
						arguments.Add(string.Format("/f \"{0}\"", request.CertificateFileName));

						if (!string.IsNullOrWhiteSpace(request.CertificatePassword))
						{
							arguments.Add(string.Format("/p \"{0}\"", request.CertificatePassword));
						}
					}

					if (!string.IsNullOrWhiteSpace(request.CertificateFingerprint))
					{
						arguments.Add(string.Format("/sha1 \"{0}\"", request.CertificateFingerprint));
					}

					if (!string.IsNullOrWhiteSpace(request.CertificateSubjectName))
					{
						arguments.Add(string.Format("/n \"{0}\"", request.CertificateSubjectName));
					}

					if (request.OverwriteAnyExistingSignature)
					{
						arguments.Add("/as");
					}

					switch (request.Verbosity)
					{
						case DTOs.CodeSigningVerbosity.Normal:
							break;
						case DTOs.CodeSigningVerbosity.Quiet:
							arguments.Add("/q");
							break;
						case DTOs.CodeSigningVerbosity.Detailed:
							arguments.Add("/debug");
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					foreach (var assemblyFullName in request.AssemblyFullNames)
					{
						arguments.Add(string.Format("\"{0}\"", assemblyFullName));
					}

					ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						ProcessExeFullName = "signtool.exe",
						Arguments = arguments,
						Logger = logger,
					});

					if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
					{
						foreach (var assemblyFullName in assemblyFullNames)
						{
							var newAssemblyFullName = System.IO.Path.Combine(request.OutputDirectory, System.IO.Path.GetFileName(assemblyFullName));

							if (System.IO.File.Exists(newAssemblyFullName))
							{
								System.IO.File.Delete(newAssemblyFullName);
							}

							System.IO.File.Copy(assemblyFullName, newAssemblyFullName);
						}
					}
				}
			}

			return response;
		}
	}
}