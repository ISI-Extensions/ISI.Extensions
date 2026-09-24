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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class ProjectApi
	{
		public DTOs.GetTargetFrameworkVersionsResponse GetTargetFrameworkVersions(DTOs.GetTargetFrameworkVersionsRequest request)
		{
			var response = new DTOs.GetTargetFrameworkVersionsResponse();

			var projectDetails = GetProjectDetails(new()
			{
				Project = request.Project,
			}).ProjectDetails;

			if (projectDetails != null)
			{
				var csProjXml = System.Xml.Linq.XElement.Parse(System.IO.File.ReadAllText(projectDetails.ProjectFullName));

				var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

				if (sdkAttribute.StartsWith("Microsoft.NET"))
				{
					var targetFrameworkVersions = csProjXml
						.GetElementByLocalName("PropertyGroup")?
						.GetElementByLocalName("TargetFrameworks")?
						.Value;

					if (!string.IsNullOrWhiteSpace(targetFrameworkVersions))
					{
						response.TargetFrameworkVersions = targetFrameworkVersions.Split(';').Select(targetFrameworkVersion => targetFrameworkVersion.Trim()).ToNullCheckedArray(NullCheckCollectionResult.Empty);
					}
					else
					{
						var targetFrameworkVersion = csProjXml
							.GetElementByLocalName("PropertyGroup")?
							.GetElementByLocalName("TargetFramework")?
							.Value;

						if (string.IsNullOrWhiteSpace(targetFrameworkVersion))
						{
							targetFrameworkVersion = "net10.0";
						}

						response.TargetFrameworkVersions = [targetFrameworkVersion];
					}
				}
				else
				{
					var targetFrameworkVersion = csProjXml
						.GetElementByLocalName("PropertyGroup")?
						.GetElementByLocalName("TargetFrameworkVersion")?
						.Value;

					if (string.IsNullOrWhiteSpace(targetFrameworkVersion))
					{
						targetFrameworkVersion = "v4.8";
					}

					targetFrameworkVersion = $"net{ISI.Extensions.StringFormat.StringNumericOnly(targetFrameworkVersion).Replace(".", string.Empty)}";

					response.TargetFrameworkVersions = [targetFrameworkVersion];
				}
			}

			return response;
		}
	}
}