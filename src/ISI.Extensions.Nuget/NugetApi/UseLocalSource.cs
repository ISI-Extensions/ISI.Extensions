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
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.UseLocalSourceResponse UseLocalSource(DTOs.UseLocalSourceRequest request)
		{
			var response = new DTOs.UseLocalSourceResponse();

			request.AddLocalProjectToSolution ??= _ => { };

			var nugetPackageKeys = GetProjectNugetPackageDependencies(new DTOs.GetProjectNugetPackageDependenciesRequest()
			{
				ProjectFullName = request.ProjectFullName,
			}).NugetPackageKeys.ToArray();

			var localSourcesToAdd = new List<(string ProjectName, string ProjectFullName)>();

			foreach (var nugetPackageKey in nugetPackageKeys)
			{
				if (request.TryGetLocalProject(nugetPackageKey.Package, out var projectFullName))
				{
					localSourcesToAdd.Add((ProjectName: nugetPackageKey.Package, ProjectFullName: projectFullName));
				}
			}

			if (localSourcesToAdd.Any())
			{
				RemoveProjectNugetPackagesFromCsProj(new DTOs.RemoveProjectNugetPackagesFromCsProjRequest()
				{
					CsProjFullName = request.ProjectFullName,
					NugetPackageNames = localSourcesToAdd.ToNullCheckedArray(localSource => localSource.ProjectName),
				});

				var projectDirectory = System.IO.Path.GetDirectoryName(request.ProjectFullName);

				var csProjXml = System.Xml.Linq.XElement.Load(request.ProjectFullName);

				var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

				var isDirty = false;

				var itemGroup = new System.Xml.Linq.XElement("ItemGroup");

				foreach (var localSource in localSourcesToAdd)
				{
					var relativePath = ISI.Extensions.IO.Path.GetRelativePath(projectDirectory, localSource.ProjectFullName);

					var projectReference = new System.Xml.Linq.XElement("ProjectReference");
					projectReference.SetAttributeValue("Include", relativePath);
					if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
					{
						projectReference.Add(new System.Xml.Linq.XElement("Name", localSource.ProjectName));
					}

					itemGroup.Add(projectReference);
				}

				csProjXml.Add(itemGroup);

				var csProj = csProjXml.ToString();
				csProj = csProj.Replace("<ItemGroup xmlns=\"\">", "<ItemGroup>");

				if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
				{
					csProj = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{0}", csProj);
				}

				System.IO.File.WriteAllText(request.ProjectFullName, csProj);

				foreach (var localSource in localSourcesToAdd)
				{
					request.AddLocalProjectToSolution(localSource.ProjectFullName);
				}
			}

			return response;
		}
	}
}