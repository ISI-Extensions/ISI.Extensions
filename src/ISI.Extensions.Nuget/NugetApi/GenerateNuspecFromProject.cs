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
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.GenerateNuspecFromProjectResponse GenerateNuspecFromProject(DTOs.GenerateNuspecFromProjectRequest request)
		{
			var response = new DTOs.GenerateNuspecFromProjectResponse();

			response.Nuspec = new();

			var projectDirectory = System.IO.Path.GetDirectoryName(request.ProjectFullName);

			response.Nuspec.Package = System.IO.Path.GetFileNameWithoutExtension(request.ProjectFullName);

			var fullNames = new List<string>();
			fullNames.AddRange(System.IO.Directory.GetFiles(projectDirectory, "*.cs", System.IO.SearchOption.AllDirectories).Where(path => (path.IndexOf("\\obj\\") < 0) && (path.IndexOf("\\bin\\") < 0)));

			var projectLines = System.IO.File.ReadAllLines(request.ProjectFullName);

			foreach (var projectLine in projectLines)
			{
				if (projectLine.IndexOf("<Compile ", StringComparison.InvariantCulture) > 0)
				{
					var keyValues = projectLine.Replace("<Compile ", string.Empty).Replace("/>", string.Empty).Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(["=\"", "\""], StringSplitOptions.None)).ToDictionary(item => item[0], item => item[1], StringComparer.CurrentCultureIgnoreCase);

					if (keyValues.TryGetValue("Include", out var includedFile))
					{
						fullNames.Add(System.IO.Path.Combine(projectDirectory, includedFile));
					}
				}
			}

			foreach (var fullName in fullNames)
			{
				if (System.IO.File.Exists(fullName))
				{
					var lines = System.IO.File.ReadAllLines(fullName);

					foreach (var line in lines)
					{
						if (line.IndexOf("assembly: AssemblyCompany", StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							response.Nuspec.Owners = [line.Split(['\"'], StringSplitOptions.None)[1]];
						}
						else if (line.IndexOf("assembly: AssemblyCopyright", StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							response.Nuspec.Copyright = line.Split(['\"'], StringSplitOptions.None)[1];
						}
						else if (line.IndexOf("assembly: AssemblyVersion", StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							response.Nuspec.Version = line.Split(['\"'], StringSplitOptions.None)[1];
						}
					}
				}
			}

			var dependencies = GetProjectNugetPackageDependencies(new()
			{
				ProjectFullName = request.ProjectFullName,
				TryGetPackageVersion = request.TryGetPackageVersion,
				BuildTargetFrameworks = request.BuildTargetFrameworks,
			}).NugetPackageKeys;

			response.Nuspec.Dependencies = dependencies.ToNullCheckedArray(dependency => new NuspecDependency()
			{
				Package = dependency.Package,
				Version = dependency.Version,
			});

			var csProjXml = System.Xml.Linq.XElement.Parse(string.Join("\n", projectLines));
			var isNet4WebProject = projectLines.Any(projectLine => projectLine.IndexOf("Microsoft.WebApplication.targets", StringComparison.InvariantCultureIgnoreCase) >= 0);

			var targetFramework = GetTargetFrameworkVersionFromCsProjXml(csProjXml);

			var projectName = System.IO.Path.GetFileNameWithoutExtension(request.ProjectFullName);

			var possibleFiles = new Dictionary<string, string>();

			if (targetFramework.StartsWith("net4", StringComparison.InvariantCultureIgnoreCase))
			{
				possibleFiles.Add($"bin\\{request.Configuration}\\{projectName}.dll", "lib/net48");
				if (isNet4WebProject)
				{
					possibleFiles.Add($"bin\\{projectName}.dll", "lib/net48");
				}
			}
			else
			{
				possibleFiles.Add($"bin\\{request.Configuration}\\{targetFramework}\\{projectName}.dll", $"lib/{targetFramework}");
			}

			if (request.IncludePdb)
			{
				if (targetFramework.StartsWith("net4", StringComparison.InvariantCultureIgnoreCase))
				{
					possibleFiles.Add($"bin\\{request.Configuration}\\{projectName}.pdb", "lib/net48");
					if (isNet4WebProject)
					{
						possibleFiles.Add($"bin\\{projectName}.pdb", "lib/net48");
					}
				}
				else
				{
					possibleFiles.Add($"bin\\{request.Configuration}\\{targetFramework}\\{projectName}.pdb", $"lib/{targetFramework}");
				}
			}

			if (request.IncludeSBom)
			{
				foreach (var manifestRelativeDirectory in new[] { $"bin\\{request.Configuration}\\_manifest", $"bin\\_manifest", $"bin\\{request.Configuration}\\{targetFramework}\\_manifest" })
				{
					var manifestDirectory = System.IO.Path.Combine(projectDirectory, manifestRelativeDirectory);

					if (System.IO.Directory.Exists(manifestDirectory))
					{
						var manifestFileFullNames = System.IO.Directory.GetFiles(manifestDirectory, "*", System.IO.SearchOption.AllDirectories);

						foreach (var manifestFileFullName in manifestFileFullNames)
						{
							var manifestFileRelativeFileName = manifestFileFullName.TrimStart(manifestDirectory).TrimStart('/', '\\');
							var manifestFileRelativeDirectory = System.IO.Path.GetDirectoryName(manifestFileRelativeFileName);

							possibleFiles.Add(manifestFileFullName, $"_manifest\\{manifestFileRelativeDirectory}".Replace('\\', '/'));
						}
					}
				}
			}

			var files = new List<NuspecFile>();

			foreach (var possibleFile in possibleFiles)
			{
				var fileName = System.IO.Path.Combine(projectDirectory, possibleFile.Key);
				if (System.IO.File.Exists(fileName))
				{
					files.Add(new ISI.Extensions.Nuget.NuspecFile()
					{
						Target = possibleFile.Value,
						SourcePattern = fileName,
					});
				}
			}

			response.Nuspec.Files = files;

			return response;
		}
	}
}