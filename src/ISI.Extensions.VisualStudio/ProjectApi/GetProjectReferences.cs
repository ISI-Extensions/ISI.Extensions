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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.ProjectApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class ProjectApi
	{
		public DTOs.GetProjectReferencesResponse GetProjectReferences(DTOs.GetProjectReferencesRequest request)
		{
			var response = new DTOs.GetProjectReferencesResponse();

			var csProjXml = System.Xml.Linq.XElement.Load(request.ProjectFileName);

			var projectReferences = new List<ProjectReference>();

			foreach (var itemGroup in csProjXml.GetElementsByLocalName("ItemGroup"))
			{
				foreach (var projectReferenceElement in itemGroup.GetElementsByLocalName("ProjectReference"))
				{
					var path = projectReferenceElement.GetAttributeByLocalName("Include").Value;
					var rootPath = System.IO.Path.GetDirectoryName(request.ProjectFileName);
					while (path.StartsWith("..\\", StringComparison.InvariantCultureIgnoreCase))
					{
						rootPath = System.IO.Path.GetDirectoryName(rootPath);
						path = path.Substring(3);
					}
					path = System.IO.Path.Combine(rootPath, path);

					var name = projectReferenceElement.GetElementByLocalName("Name")?.Value ?? System.IO.Path.GetFileNameWithoutExtension(path);
					var projectUuid = projectReferenceElement.GetElementByLocalName("Project")?.Value?.ToGuidNullable();

					projectReferences.Add(new()
					{
						Name = name,
						Path = path,
						ProjectUuid = projectUuid,
					});
				}
			}

			response.ProjectReferences = projectReferences;

			return response;
		}
	}
}