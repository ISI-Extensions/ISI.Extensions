#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
		public DTOs.SignVsixesResponse SignVsixes(DTOs.ISignVsixesRequest request)
		{
			var response = new DTOs.SignVsixesResponse();

			using (new ISI.Extensions.Windows.ScreenSaverDisabler())
			{
				InitializeCodeSigningCertificateToken(request);

				var logger = new AddToLogLogger(request.AddToLog, Logger);

				var certificate = (string.IsNullOrWhiteSpace(request.CertificateFileName) ? GetCertificateFromCertificateStore(request.CertificateStoreName, request.CertificateStoreLocation, request.CertificateSubjectName, request.CertificateFingerprint) : GetCertificateFromPfx(request.CertificateFileName, request.CertificatePassword));

				var signingKey = GetSigningKeyFromCertificate(certificate);

				var vsixFullNames = new List<string>();

				switch (request)
				{
					case DTOs.SignVsixesInDirectoryRequest signVsixesInDirectoryRequest:
						vsixFullNames.AddRange(System.IO.Directory.GetFiles(signVsixesInDirectoryRequest.VsixesDirectory));
						break;

					case DTOs.SignVsixesRequest signVsixesRequest:
						vsixFullNames.AddRange(signVsixesRequest.VsixFullNames);
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(request));
				}

				if (vsixFullNames.Any())
				{
					using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
					{
						var tempVsixFullNames = vsixFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							for (var fileIndex = 0; fileIndex < tempVsixFullNames.Length; fileIndex++)
							{
								var tempVsixFullName = System.IO.Path.Combine(tempDirectory.FullName, System.IO.Path.GetFileName(tempVsixFullNames[fileIndex]));

								if (System.IO.File.Exists(tempVsixFullName))
								{
									System.IO.File.Delete(tempVsixFullName);
								}

								System.IO.File.Copy(tempVsixFullNames[fileIndex], tempVsixFullName);

								tempVsixFullNames[fileIndex] = tempVsixFullName;
							}
						}

						var vsixSigntoolExeFullName = VsixSigntoolApi.GetVsixSigntoolExeFullName(new()).VsixSigntoolExeFullName;

						void sign(string[] fileNames)
						{
							var arguments = new List<string>();
							arguments.Add("sign");

							if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha256)
							{
								arguments.Add("/fd SHA256");
							}
							else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha384)
							{
								arguments.Add("/fd SHA384");
							}
							else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha512)
							{
								arguments.Add("/fd SHA512");
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
								var certificateFileName = request.CertificateFileName;

								if (certificateFileName.EndsWith(".cer", StringComparison.InvariantCultureIgnoreCase))
								{
									certificateFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(certificateFileName), string.Format("{0}.pfx", System.IO.Path.GetFileNameWithoutExtension(certificateFileName)));

									if (!System.IO.File.Exists(certificateFileName))
									{
										certificateFileName = request.CertificateFileName;
									}
								}

								arguments.Add(string.Format("/f \"{0}\"", certificateFileName));

								if (!string.IsNullOrWhiteSpace(request.CertificatePassword))
								{
									arguments.Add(string.Format("/p \"{0}\"", request.CertificatePassword));
								}
							}

							if (!string.IsNullOrWhiteSpace(request.CertificateFingerprint))
							{
								arguments.Add(string.Format("/sha1 \"{0}\"", request.CertificateFingerprint));
							}

							//if (!string.IsNullOrWhiteSpace(request.CertificateSubjectName))
							//{
							//	arguments.Add(string.Format("/n \"{0}\"", request.CertificateSubjectName));
							//}

							//if (request.OverwriteAnyExistingSignature)
							//{
							//	arguments.Add("/as");
							//}

							switch (request.Verbosity)
							{
								case DTOs.CodeSigningVerbosity.Normal:
									break;
								case DTOs.CodeSigningVerbosity.Quiet:
									arguments.Add("/q");
									break;
								case DTOs.CodeSigningVerbosity.Detailed:
									arguments.Add("/v");
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}

							arguments.AddRange(fileNames.Select(fileName => string.Format("\"{0}\"", fileName)));

							var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
							{
								ProcessExeFullName = vsixSigntoolExeFullName,
								Arguments = arguments,
								Logger = (fileNames.NullCheckedCount() == 1 ? null : logger),
							});

							if (fileNames.NullCheckedCount() == 1)
							{
								if (waitForProcessResponse.Errored)
								{
									Logger.LogError(waitForProcessResponse.Output);
								}

								logger.LogInformation(string.Format("Signed vsix \"{0}\"", System.IO.Path.GetFileName(fileNames.First())));
							}
						}


						//void signX(string fileName)
						//{
						//	logger.LogInformation(string.Format("Signing vsix package \"{0}\"", System.IO.Path.GetFileName(fileName)));

						//	using (var package = OpenVsixSignTool.Core.OpcPackage.Open(fileName, OpenVsixSignTool.Core.OpcPackageFileMode.ReadWrite))
						//	{
						//		if (package.GetSignatures().Any() && !request.OverwriteAnyExistingSignature)
						//		{
						//			throw new("The VSIX is already signed.");
						//		}

						//		var signBuilder = package.CreateSignatureBuilder();

						//		signBuilder.EnqueueNamedPreset<OpenVsixSignTool.Core.VSIXSignatureBuilderPreset>();

						//		var hashAlgorithmName = GetHashAlgorithmName(request.DigestAlgorithm);

						//		var signingConfiguration = new OpenVsixSignTool.Core.SignConfigurationSet(hashAlgorithmName, hashAlgorithmName, signingKey, certificate);

						//		var signature = signBuilder.Sign(signingConfiguration);

						//		if (request.TimeStampUri != null)
						//		{
						//			var timestampBuilder = signature.CreateTimestampBuilder();

						//			var signResponse = timestampBuilder.SignAsync(request.TimeStampUri, GetHashAlgorithmName(request.TimeStampDigestAlgorithm)).GetAwaiter().GetResult();

						//			if (signResponse == OpenVsixSignTool.Core.TimestampResult.Failed)
						//			{
						//				throw new("TimeStamping Signature Failed");
						//			}
						//		}
						//	}

						//	logger.LogInformation(string.Format("Signed vsix package \"{0}\"", System.IO.Path.GetFileName(fileName)));
						//}

						if (request.RunAsync)
						{
							logger.LogInformation("Running Async");
							sign(vsixFullNames.ToArray());
							//Parallel.ForEach(vsixFullNames, vsixFullName => sign(vsixFullName));
						}
						else
						{
							sign(vsixFullNames.ToArray());
							//foreach (var vsixFullName in vsixFullNames)
							//{
							//	sign(vsixFullName);
							//}
						}

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							foreach (var vsixFullName in tempVsixFullNames)
							{
								var newVsixFullName = System.IO.Path.Combine(request.OutputDirectory, System.IO.Path.GetFileName(vsixFullName));

								if (System.IO.File.Exists(newVsixFullName))
								{
									System.IO.File.Delete(newVsixFullName);
								}

								System.IO.File.Copy(vsixFullName, newVsixFullName);
							}
						}
					}
				}
			}
			
			return response;
		}
	}
}