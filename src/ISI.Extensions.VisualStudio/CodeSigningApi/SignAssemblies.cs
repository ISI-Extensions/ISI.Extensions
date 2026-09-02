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
		public DTOs.SignAssembliesResponse SignAssemblies(DTOs.ISignAssembliesRequest request)
		{
			var response = new DTOs.SignAssembliesResponse();

			using (new ISI.Extensions.Windows.ScreenSaverDisabler())
			{
				InitializeCodeSigningCertificateToken(request);

				var logger = new AddToLogLogger(request.AddToLog, Logger);

				var assemblyFullNames = Array.Empty<string>();

				switch (request)
				{
					case DTOs.SignAssembliesInDirectoryRequest signAssembliesInDirectoryRequest:
						assemblyFullNames = System.IO.Directory.GetFiles(signAssembliesInDirectoryRequest.AssembliesDirectory);
						break;

					case DTOs.SignAssembliesRequest signAssembliesRequest:
						assemblyFullNames = signAssembliesRequest.AssemblyFullNames ?? [];
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(request));
				}

				if (assemblyFullNames.Any())
				{
					using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
					{
						var signedAssemblyFullNames = new string[assemblyFullNames.Length];

						for (var fileIndex = 0; fileIndex < signedAssemblyFullNames.Length; fileIndex++)
						{
							var signedAssemblyDirectory = System.IO.Path.Combine(tempDirectory.FullName, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36));

							System.IO.Directory.CreateDirectory(signedAssemblyDirectory);

							var signedAssemblyFullName = System.IO.Path.Combine(signedAssemblyDirectory, System.IO.Path.GetFileName(assemblyFullNames[fileIndex]));

							System.IO.File.Copy(assemblyFullNames[fileIndex], signedAssemblyFullName);

							signedAssemblyFullNames[fileIndex] = signedAssemblyFullName;

							logger.LogInformation($"{assemblyFullNames[fileIndex]} => {signedAssemblyFullNames[fileIndex]}");
						}

						var signtoolExeFullName = GetSigntoolExeFullName(new()).SigntoolExeFullName;

						bool sign(string[] fileNames)
						{
							var arguments = GetSignAssemblyCommandArguments(request);

							arguments.AddRange(fileNames.Select(fileName => $"\"{fileName}\""));

							var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
							{
								ProcessExeFullName = signtoolExeFullName,
								Arguments = arguments,
								Logger = (fileNames.NullCheckedCount() == 1 ? null : logger),
							});

							if (waitForProcessResponse.Errored)
							{
								Logger.LogError(waitForProcessResponse.Output);
								return false;
							}

							return true;
						}

						var signedFilesSuccessfully = true;

						switch (request.CertificateType)
						{
							case DTOs.CodeSigningCertificateType.File:
								signedFilesSuccessfully = sign(signedAssemblyFullNames);
								break;

							case DTOs.CodeSigningCertificateType.JSignEToken:
								foreach (var extension in new[] { "*.exe", "*.dll", "*.msi", "*.cab", "*.cat", "*.appx", "*.msix", "*.navx", "*.efi" })
								{
									var filesToSign = System.IO.Directory.EnumerateFiles(tempDirectory.FullName, extension, System.IO.SearchOption.AllDirectories).ToArray();

									if (signedFilesSuccessfully && filesToSign.Any())
									{
										foreach (var chunkedFilesToSign in filesToSign.Chunker(10))
										{
											signedFilesSuccessfully = jSignEToken(logger, request, chunkedFilesToSign.ToArray());
										}
									}
								}
								break;

							default:
								throw new ArgumentOutOfRangeException();
						}

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							for (var fileIndex = 0; fileIndex < assemblyFullNames.Length; fileIndex++)
							{
								var signedAssemblyFullName = System.IO.Path.Combine(request.OutputDirectory, System.IO.Path.GetFileName(signedAssemblyFullNames[fileIndex]));

								if (System.IO.File.Exists(signedAssemblyFullName))
								{
									System.IO.File.Delete(signedAssemblyFullName);
								}

								System.IO.File.Copy(signedAssemblyFullNames[fileIndex], signedAssemblyFullName);
							}
						}
						else
						{
							for (var fileIndex = 0; fileIndex < assemblyFullNames.Length; fileIndex++)
							{
								var signedAssemblyFullName = assemblyFullNames[fileIndex];

								if (System.IO.File.Exists(signedAssemblyFullName))
								{
									System.IO.File.Delete(signedAssemblyFullName);
								}

								System.IO.File.Copy(signedAssemblyFullNames[fileIndex], signedAssemblyFullName);
							}
						}
					}
				}
			}

			return response;
		}
	}
}