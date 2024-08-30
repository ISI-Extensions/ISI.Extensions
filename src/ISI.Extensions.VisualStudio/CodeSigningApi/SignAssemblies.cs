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
		public DTOs.SignAssembliesResponse SignAssemblies(DTOs.ISignAssembliesRequest request)
		{
			var response = new DTOs.SignAssembliesResponse();

			using (new ISI.Extensions.Windows.ScreenSaverDisabler())
			{
				InitializeCodeSigningCertificateToken(request);

				var logger = new AddToLogLogger(request.AddToLog, Logger);

				var assemblyFullNames = new List<string>();

				switch (request)
				{
					case DTOs.SignAssembliesInDirectoryRequest signAssembliesInDirectoryRequest:
						assemblyFullNames.AddRange(System.IO.Directory.GetFiles(signAssembliesInDirectoryRequest.AssembliesDirectory));
						break;

					case DTOs.SignAssembliesRequest signAssembliesRequest:
						assemblyFullNames.AddRange(signAssembliesRequest.AssemblyFullNames);
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(request));
				}

				if (assemblyFullNames.Any())
				{
					using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
					{
						var tempAssemblyFullNames = assemblyFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							for (var fileIndex = 0; fileIndex < tempAssemblyFullNames.Length; fileIndex++)
							{
								var tempAssemblyFullName = System.IO.Path.Combine(tempDirectory.FullName, System.IO.Path.GetFileName(tempAssemblyFullNames[fileIndex]));

								if (System.IO.File.Exists(tempAssemblyFullName))
								{
									System.IO.File.Delete(tempAssemblyFullName);
								}

								System.IO.File.Copy(tempAssemblyFullNames[fileIndex], tempAssemblyFullName);

								tempAssemblyFullNames[fileIndex] = tempAssemblyFullName;
							}
						}

						var signtoolExeFullName = GetSigntoolExeFullName(new()).SigntoolExeFullName;

						void sign(string[] fileNames)
						{
							var arguments = GetSignAssemblyCommandArguments(request);

							arguments.AddRange(fileNames.Select(fileName => string.Format("\"{0}\"", fileName)));

							var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
							{
								ProcessExeFullName = signtoolExeFullName,
								Arguments = arguments,
								Logger = (fileNames.NullCheckedCount() == 1 ? null : logger),
							});

							if (fileNames.NullCheckedCount() == 1)
							{
								if (waitForProcessResponse.Errored)
								{
									Logger.LogError(waitForProcessResponse.Output);
								}

								logger.LogInformation(string.Format("Signed assembly \"{0}\"", System.IO.Path.GetFileName(fileNames.First())));
							}
						}

						if (request.RunAsync)
						{
							logger.LogInformation("Running Async");
							Parallel.ForEach(tempAssemblyFullNames, assemblyFullName => sign(new[] { assemblyFullName }));
						}
						else
						{
							sign(tempAssemblyFullNames);
						}

						if (!string.IsNullOrWhiteSpace(request.OutputDirectory) && System.IO.Directory.Exists(request.OutputDirectory))
						{
							foreach (var assemblyFullName in tempAssemblyFullNames)
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
			}

			return response;
		}
	}
}