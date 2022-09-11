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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class PackagerApi
	{
		public DTOs.PackageComponentsResponse PackageComponents(DTOs.PackageComponentsRequest request)
		{
			var response = new DTOs.PackageComponentsResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);

			if (string.IsNullOrWhiteSpace(request.PackageName))
			{
				request.PackageName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileNameWithoutExtension(request.PackageFullName)));
			}

			if (!string.IsNullOrWhiteSpace(request.Solution) && System.IO.Directory.Exists(request.Solution))
			{
				var possibleSolutionFullNames = System.IO.Directory.GetFiles(request.Solution, "*.sln", System.IO.SearchOption.AllDirectories);

				if (possibleSolutionFullNames.Length == 1)
				{
					request.Solution = possibleSolutionFullNames.First();
				}
				else if (possibleSolutionFullNames.Length > 1)
				{
					var possibleSolutionName = System.IO.Path.GetFileName(request.Solution);

					var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

					if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						request.Solution = possibleSolutionFullName;
					}
					else
					{
						throw new Exception(string.Format("Cannot determine which solution to update \"{0}\"", request.Solution));
					}
				}
				else
				{
					throw new Exception(string.Format("Cannot find a solution to update \"{0}\"", request.Solution));
				}
			}








			logger.LogInformation("Package Components");
			logger.LogInformation("  Configuration: {0}", request.Configuration);
			logger.LogInformation("  Build Platform: {0}", request.BuildPlatform.GetDescription());
			logger.LogInformation("  Platform Target: {0}", request.PlatformTarget.GetDescription());
			logger.LogInformation("  SubDirectory: {0}", request.SubDirectory);
			logger.LogInformation("  Package FullName: {0}", request.PackageFullName);
			logger.LogInformation("  Package Name: {0}", request.PackageName);
			logger.LogInformation("  Package Version: {0}", request.PackageVersion);
			logger.LogInformation("  Package BuildDateTimeStamp: {0}", request.PackageBuildDateTimeStamp);

			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				var packageComponentsDirectory = tempDirectory.FullName;

				if (!request.CleanupTempDirectories)
				{
					packageComponentsDirectory = ISI.Extensions.IO.Path.GetTempDirectoryName();
				}

				if (!string.IsNullOrWhiteSpace(request.SubDirectory))
				{
					packageComponentsDirectory = System.IO.Path.Combine(packageComponentsDirectory, request.SubDirectory);
				}

				foreach (var packageComponent in request.PackageComponents)
				{
					var solutionFullName = request.Solution;
					if (string.IsNullOrWhiteSpace(solutionFullName))
					{
						solutionFullName = System.IO.Path.GetDirectoryName(packageComponent.ProjectFullName);

						while (!string.IsNullOrWhiteSpace(solutionFullName) && !solutionFullName.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase))
						{
							var possibleSolutionFullName = System.IO.Directory.GetFiles(solutionFullName, "*.sln", System.IO.SearchOption.TopDirectoryOnly).NullCheckedFirstOrDefault();

							if (string.IsNullOrWhiteSpace(possibleSolutionFullName))
							{
								solutionFullName = System.IO.Path.GetDirectoryName(solutionFullName);
							}
							else
							{
								solutionFullName = possibleSolutionFullName;
							}
						}
					}


					switch (packageComponent)
					{
						case DTOs.PackageComponentWindowsApplication packageComponentWindowsApplication:
							BuildPackageComponentWindowsApplication(logger, request.Configuration, request.BuildVersion, request.BuildPlatform, request.PlatformTarget, request.BuildVerbosity, packageComponentsDirectory, solutionFullName, request.AssemblyVersionFiles, packageComponentWindowsApplication);
							break;

						case DTOs.PackageComponentConsoleApplication packageComponentConsoleApplication:
							BuildPackageComponentConsoleApplication(logger, request.Configuration, request.BuildVersion, request.BuildPlatform, request.PlatformTarget, request.BuildVerbosity, packageComponentsDirectory, solutionFullName, request.AssemblyVersionFiles, packageComponentConsoleApplication);
							break;

						case DTOs.PackageComponentWindowsService packageComponentWindowsService:
							BuildPackageComponentWindowsService(logger, request.Configuration, request.BuildVersion, request.BuildPlatform, request.PlatformTarget, request.BuildVerbosity, packageComponentsDirectory, solutionFullName, request.AssemblyVersionFiles, packageComponentWindowsService);
							break;

						case DTOs.PackageComponentWebSite packageComponentWebSite:
							BuildPackageComponentWebSite(logger, request.Configuration, request.BuildVersion, request.BuildPlatform, request.PlatformTarget, request.BuildVerbosity, packageComponentsDirectory, solutionFullName, request.AssemblyVersionFiles, packageComponentWebSite);
							break;

						default:
							throw new ArgumentOutOfRangeException(nameof(packageComponent));
					}
				}

				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(request.PackageFullName));

				System.IO.Compression.ZipFile.CreateFromDirectory(tempDirectory.FullName, request.PackageFullName);

				if (!string.IsNullOrWhiteSpace(request.PackageVersion) && !string.IsNullOrWhiteSpace(request.PackageBuildDateTimeStamp))
				{
					System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(request.PackageFullName), string.Format("{0}.Current.DateTimeStamp.Version.txt", request.PackageName)), string.Format("{0}|{1}", request.PackageBuildDateTimeStamp, request.PackageVersion));
				}
			}

			return response;
		}
	}
}