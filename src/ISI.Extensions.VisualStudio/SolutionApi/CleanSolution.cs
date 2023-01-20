#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.CleanSolutionResponse CleanSolution(DTOs.CleanSolutionRequest request)
		{
			var response = new DTOs.CleanSolutionResponse();

			try
			{
				var solutionSourceDirectory = GetSolutionDetails(new()
				{
					Solution = request.Solution,
				}).SolutionDetails?.SolutionDirectory;

				if (System.IO.Directory.Exists(solutionSourceDirectory))
				{
					if (!ISI.Extensions.IO.Path.CheckForExistence(solutionSourceDirectory, "*.sln", ISI.Extensions.VisualStudio.VisualStudioSettings.DefaultExcludePathFilters, VisualStudioSettings.GetMaxCheckDirectoryDepth()))
					{
						solutionSourceDirectory = System.IO.Path.Combine(solutionSourceDirectory, "src");
					}

					if (System.IO.Directory.Exists(solutionSourceDirectory))
					{
						var solutionFileName = ISI.Extensions.IO.Path.EnumerateFiles(solutionSourceDirectory, "*.sln", ISI.Extensions.VisualStudio.VisualStudioSettings.DefaultExcludePathFilters, VisualStudioSettings.GetMaxCheckDirectoryDepth()).FirstOrDefault();

						if (!string.IsNullOrEmpty(solutionFileName))
						{
							var resharperCacheDirectory = System.IO.Path.Combine(solutionSourceDirectory, "_ReSharper.Caches");

							if (System.IO.Directory.Exists(resharperCacheDirectory))
							{
								System.IO.Directory.Delete(resharperCacheDirectory, true);
							}

							var solutionContentLines = System.IO.File.ReadAllLines(solutionFileName).ToList();

							var projectFileNames = solutionContentLines.Where(l => l.StartsWith("Project(")).Select(l => l.Split(new[] { ',' }, StringSplitOptions.None)[1].Trim(' ', '\"')).Select(projectFileName => System.IO.Path.Combine(solutionSourceDirectory, projectFileName));

							var usedPackages = new HashSet<string>();

							foreach (var projectFileName in projectFileNames.Where(System.IO.File.Exists))
							{
								var projectContentLines = System.IO.File.ReadAllLines(projectFileName).ToList();

								var projectDirectory = System.IO.Path.GetDirectoryName(projectFileName);

								var binDirectory = System.IO.Path.Combine(projectDirectory, "bin");
								if (System.IO.Directory.Exists(binDirectory))
								{
									System.IO.Directory.Delete(binDirectory, true);
								}

								var objDirectory = System.IO.Path.Combine(projectDirectory, "obj");
								if (System.IO.Directory.Exists(objDirectory))
								{
									System.IO.Directory.Delete(objDirectory, true);
								}

								if (projectContentLines.Any(line => line.IndexOf("ISI.Libraries.ClearScript.dll", StringComparison.InvariantCultureIgnoreCase) >= 0))
								{
									foreach (var fileName in new[]
									{
										"ClearScriptV8-32.dll",
										"ClearScriptV8-64.dll",
										"v8-ia32.dll",
										"v8-ia64.dll"
									})
									{
										var fullName = System.IO.Path.Combine(projectDirectory, fileName);

										if (System.IO.File.Exists(fullName))
										{
											System.IO.File.Delete(fullName);
										}
									}
								}


								var packageFileName = System.IO.Path.Combine(projectDirectory, "packages.config");
								if (System.IO.File.Exists(packageFileName))
								{
									foreach (var line in System.IO.File.ReadAllLines(packageFileName))
									{
										if (line.Trim().StartsWith("<package ", StringComparison.InvariantCultureIgnoreCase))
										{
											var keyValues = line.Replace("<package ", string.Empty).Replace("/>", string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(new[] { "=\"", "\"" }, StringSplitOptions.None)).ToDictionary(item => item[0], item => item[1], StringComparer.InvariantCultureIgnoreCase);

											try
											{
												string package = keyValues["id"];
												string version = keyValues["version"];

												usedPackages.Add(string.Format("{0}.{1}", package, version));
											}
											catch (Exception exception)
											{

											}
										}
									}
								}
							}

							var packagesDirectory = System.IO.Path.Combine(solutionSourceDirectory, "packages");

							if (System.IO.Directory.Exists(packagesDirectory))
							{
								foreach (var packageDirectory in System.IO.Directory.GetDirectories(packagesDirectory))
								{
									var package = packageDirectory.Split(new[] { '\\' }).Last();

									if (!usedPackages.Contains(package))
									{
										System.IO.Directory.Delete(packageDirectory, true);
									}
								}
							}
						}
					}
				}

				response.Success = true;
			}
			catch (Exception exception)
			{
				response.Success = false;
			}

			return response;
		}
	}
}