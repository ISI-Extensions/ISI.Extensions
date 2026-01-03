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
using DTOs = ISI.Extensions.VisualStudioCode.DataTransferObjects.NodeModulesApi;

namespace ISI.Extensions.VisualStudioCode
{
	public partial class NodeModulesApi
	{
		public DTOs.CleanSolutionResponse CleanSolution(DTOs.CleanSolutionRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.CleanSolutionResponse()
			{
				Success = true,
			};

			void deleteDirectory(object directory)
			{
				if (directory is System.IO.DirectoryInfo directoryInfo)
				{
					directoryInfo.Attributes = System.IO.FileAttributes.Normal;

					foreach (var childDirectoryInfo in directoryInfo.GetDirectories())
					{
						deleteDirectory(childDirectoryInfo);
					}

					foreach (var fileInfo in directoryInfo.GetFiles())
					{
						try
						{
							fileInfo.Attributes = System.IO.FileAttributes.Normal;

							fileInfo.Delete();
						}
						catch (Exception exception)
						{
							Console.WriteLine($"Could not clear readOnly on {fileInfo.FullName}");
						}
					}

					var maxRetries = 10;
					var millisecondsDelay = 30;

					while (maxRetries > 0)
					{
						try
						{
							directoryInfo.Delete(true);
							maxRetries = -1;
						}
						catch (System.IO.IOException exception)
						{
							if (maxRetries-- < 0)
							{
								Console.WriteLine(exception);
								throw;
							}
							System.Threading.Thread.Sleep(millisecondsDelay);
						}
						catch (UnauthorizedAccessException exception)
						{
							if (maxRetries-- < 0)
							{
								Console.WriteLine(exception);
								throw;
							}
							System.Threading.Thread.Sleep(millisecondsDelay);
						}
						catch (Exception exception)
						{
							Console.WriteLine(exception);
							throw;
						}
					}
				}
				else if ((directory is string directoryName) && !string.IsNullOrWhiteSpace(directoryName) && System.IO.Directory.Exists(directoryName))
				{
					deleteDirectory(new System.IO.DirectoryInfo(directoryName));
				}
			}

			try
			{
				var solutionSourceDirectory = SolutionApi.GetSolutionDetails(new()
				{
					Solution = request.Solution,
				}).SolutionDetails?.SolutionDirectory;

				if (!string.IsNullOrWhiteSpace(solutionSourceDirectory) && System.IO.Directory.Exists(solutionSourceDirectory))
				{
					var nodeModulesDirectory = System.IO.Path.Combine(solutionSourceDirectory, "node_modules");

					if (System.IO.Directory.Exists(nodeModulesDirectory))
					{
						deleteDirectory(nodeModulesDirectory);
						//var processResponse = ISI.Extensions.Process.WaitForProcessResponse(new Process.ProcessRequest()
						//{
						//	ProcessExeFullName = @"cmd.exe",
						//	Arguments = new[] { "/c", "rmdir", "/S", "/Q", "node_modules", },
						//	WorkingDirectory = solutionSourceDirectory,
						//	Logger = logger,
						//});

						//response.Success = !processResponse.Errored;
					}
				}
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ErrorMessageFormatted());

				response.Success = false;
			}

			return response;
		}
	}
}