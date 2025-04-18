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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class ProjectApi
	{
		public DTOs.SetDockerImageDetailsResponse SetDockerImageDetails(DTOs.SetDockerImageDetailsRequest request)
		{
			var response = new DTOs.SetDockerImageDetailsResponse();

			var projectDetails = GetProjectDetails(new()
			{
				Project = request.Project,
			}).ProjectDetails;

			if (projectDetails != null)
			{
				response.ProjectFullName = projectDetails.ProjectFullName;
				response.ProjectXml = System.IO.File.ReadAllText(response.ProjectFullName);

				var csProjXml = System.Xml.Linq.XElement.Parse(response.ProjectXml);

				var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

				if (sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
				{
					var propertyGroupElement = csProjXml.GetElementsByLocalName("PropertyGroup").NullCheckedFirstOrDefault();

					if (propertyGroupElement != null)
					{
						var isDirty = false;

						if (!string.IsNullOrWhiteSpace(request.TargetOperatingSystem))
						{
							propertyGroupElement.SetOrAddElementByLocalName("DockerDefaultTargetOS", request.TargetOperatingSystem);

							isDirty = true;
						}

						if (!string.IsNullOrWhiteSpace(request.ContainerRegistry))
						{
							propertyGroupElement.SetOrAddElementByLocalName("ContainerRegistry", request.ContainerRegistry);

							isDirty = true;
						}

						if (!string.IsNullOrWhiteSpace(request.ContainerRepository))
						{
							propertyGroupElement.SetOrAddElementByLocalName("ContainerRepository", request.ContainerRepository);

							isDirty = true;
						}

						if (request.ContainerImageTags.NullCheckedAny())
						{
							propertyGroupElement.GetElementByLocalName("ContainerImageTag")?.Remove();
							propertyGroupElement.GetElementByLocalName("ContainerImageTags")?.Remove();

							if (request.ContainerImageTags.Length == 1)
							{
								propertyGroupElement.SetOrAddElementByLocalName("ContainerImageTag", request.ContainerImageTags.First());
							}
							else
							{
								propertyGroupElement.SetOrAddElementByLocalName("ContainerImageTags", string.Join(";", request.ContainerImageTags));
							}

							isDirty = true;
						}
						
						if (!string.IsNullOrWhiteSpace(request.ContainerFamily))
						{
							propertyGroupElement.SetOrAddElementByLocalName("ContainerFamily", request.ContainerFamily);

							isDirty = true;
						}

						if (!string.IsNullOrWhiteSpace(request.ContainerBaseImage))
						{
							propertyGroupElement.SetOrAddElementByLocalName("ContainerBaseImage", request.ContainerBaseImage);

							isDirty = true;
						}

						if (!string.IsNullOrWhiteSpace(request.ContainerWorkingDirectory))
						{
							propertyGroupElement.SetOrAddElementByLocalName("ContainerWorkingDirectory", request.ContainerWorkingDirectory);

							isDirty = true;
						}

						if (isDirty)
						{
							System.IO.File.WriteAllText(response.ProjectFullName, csProjXml.ToString());
						}
					}
				}
			}

			return response;
		}
	}
}