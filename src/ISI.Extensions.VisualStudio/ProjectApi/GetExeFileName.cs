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
		public DTOs.GetExeFileNameResponse GetExeFileName(DTOs.GetExeFileNameRequest request)
		{
			var response = new DTOs.GetExeFileNameResponse();
			
			var assemblyName = System.IO.Path.GetFileNameWithoutExtension(request.ProjectFileName);
			var targetFramework = string.Empty;
			var outputType = string.Empty;
			var outputPath = string.Empty;

			var csProjXml = System.Xml.Linq.XElement.Load(request.ProjectFileName);

			var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;
			var isDotNet = sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase);

			foreach (var propertyGroup in csProjXml.GetElementsByLocalName("PropertyGroup"))
			{
				targetFramework = propertyGroup.GetElementByLocalName("TargetFrameworkVersion")?.Value ?? propertyGroup.GetElementByLocalName("TargetFramework")?.Value ?? targetFramework;
				outputType = propertyGroup.GetElementByLocalName("OutputType")?.Value ?? outputType;

				var conditionAttribute = propertyGroup.GetAttributeByLocalName("Condition");
				if ((conditionAttribute != null) && (conditionAttribute.Value.IndexOf(request.BuildConfiguration.ToString().Replace("Any CPU", "AnyCPU")) >= 0))
				{
					outputPath = propertyGroup.GetElementByLocalName("OutputPath")?.Value ?? outputPath;
				}
			}

			response.ExeFileName = System.IO.Path.GetDirectoryName(request.ProjectFileName);
			response.ExeFileName = System.IO.Path.Combine(response.ExeFileName, outputPath);
			if (isDotNet && !string.IsNullOrWhiteSpace(targetFramework))
			{
				response.ExeFileName = System.IO.Path.Combine(response.ExeFileName, "bin", request.BuildConfiguration.ToString().Split(new[] { '|' }).First(), targetFramework);
			}
			response.ExeFileName = System.IO.Path.Combine(response.ExeFileName, $"{assemblyName}.{outputType}");

			return response;
		}
	}
}