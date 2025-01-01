#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.VisualStudio.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.GetActiveBuildConfigurationResponse GetActiveBuildConfiguration(DTOs.GetActiveBuildConfigurationRequest request)
		{
			var response = new DTOs.GetActiveBuildConfigurationResponse();

			response.SolutionDetails = GetSolutionDetails(new()
			{
				Solution = request.Solution,
			}).SolutionDetails;

			if (response.SolutionDetails != null)
			{
				var buildConfigurations = new List<BuildConfiguration>();

				if (string.Equals(System.IO.Path.GetExtension(response.SolutionDetails.SolutionFullName), ISI.Extensions.VisualStudio.Solution.SolutionExtensionX, StringComparison.InvariantCultureIgnoreCase))
				{
					var solutionXml = System.Xml.Linq.XElement.Load(response.SolutionDetails.SolutionFullName);
					foreach (var configurationElement in solutionXml.GetElementsByLocalName("Configurations"))
					{
						foreach (var platformElement in configurationElement.GetElementsByLocalName("Platform"))
						{
							var nameAttribute = platformElement.GetAttributeByLocalName("Name");
							if (nameAttribute != null)
							{
								buildConfigurations.Add(new (nameAttribute.Value));
							}
						}
					}
				}
				else if (string.Equals(System.IO.Path.GetExtension(response.SolutionDetails.SolutionFullName), ISI.Extensions.VisualStudio.Solution.SolutionExtension, StringComparison.InvariantCultureIgnoreCase))
				{
					var lines = System.IO.File.ReadAllLines(response.SolutionDetails.SolutionFullName);
					var inPreSolution = false;
					foreach (var line in lines)
					{
						if (inPreSolution && line.IndexOf("EndGlobalSection", StringComparison.CurrentCultureIgnoreCase) >= 0)
						{
							inPreSolution = false;
						}
						else if (inPreSolution)
						{
							buildConfigurations.Add(new(line.Trim().Split(['\t', '='], StringSplitOptions.RemoveEmptyEntries).Last().Trim()));
						}
						else if (line.IndexOf("GlobalSection(SolutionConfigurationPlatforms) = preSolution", StringComparison.CurrentCultureIgnoreCase) >= 0)
						{
							inPreSolution = true;
						}
					}
				}

				#region .vs
				{
					var visualStudioConfigDirectory = System.IO.Path.Combine(response.SolutionDetails.SolutionDirectory, ".vs", response.SolutionDetails.SolutionName);

					if (System.IO.Directory.Exists(visualStudioConfigDirectory))
					{
						var configurationFiles = ISI.Extensions.IO.Path.EnumerateFiles(visualStudioConfigDirectory, "*.suo", this.GetDefaultExcludePathFilters(), this.GetMaxCheckDirectoryDepth());

						if (configurationFiles.Any())
						{
							var configurationFileName = configurationFiles.First();

							var configuration = new OpenMcdf.CompoundFile(configurationFileName);

							var storageStream = configuration.RootStorage.GetStream("SolutionConfiguration");

							var data = storageStream.GetData();

							using (var stream = new System.IO.MemoryStream(data))
							{
								using (var streamReader = new System.IO.StreamReader(stream, Encoding.Unicode))
								{
									var config = streamReader.ReadToEnd();

									var configArray = config.ToCharArray();

									for (var i = 0; i < configArray.Length; i++)
									{
										if (configArray[i] < 32)
										{
											configArray[i] = ' ';
										}
									}

									config = new(configArray);

									var parameters = config.Split(new[] { ';' }).Select(item => item.Split(new[] { '=' }).Select(piece => piece.Trim()).ToArray()).ToArray().Where(item => item.Length == 2).ToDictionary(pieces => pieces[0], pieces => pieces[1]);

									if (parameters.TryGetValue("ActiveCfg", out var msBuildConfiguration))
									{
										var buildConfiguration = buildConfigurations.FirstOrDefault(buildConfiguration => string.Equals(buildConfiguration.ToString(), msBuildConfiguration, StringComparison.CurrentCultureIgnoreCase));

										if (buildConfiguration != null)
										{
											response.ActiveBuildConfiguration = buildConfiguration;
										}
									}
								}
							}
						}
					}
				}
				#endregion

				if (response.ActiveBuildConfiguration != null)
				{
					if (buildConfigurations.Any(buildConfiguration => string.Equals(buildConfiguration.Configuration, "Debug", StringComparison.CurrentCultureIgnoreCase)))
					{
						buildConfigurations.RemoveAll(buildConfiguration => !string.Equals(buildConfiguration.Configuration, "Debug", StringComparison.CurrentCultureIgnoreCase));
					}

					if (buildConfigurations.Any(buildConfiguration => buildConfiguration.MSBuildPlatform == MSBuildPlatform.x64))
					{
						buildConfigurations.RemoveAll(buildConfiguration => buildConfiguration.MSBuildPlatform != MSBuildPlatform.x64);
					}
				}

				response.ActiveBuildConfiguration = buildConfigurations.NullCheckedFirstOrDefault();
			}

			return response;
		}
	}
}