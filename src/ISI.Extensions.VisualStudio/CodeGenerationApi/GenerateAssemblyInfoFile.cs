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
 
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeGenerationApi
	{
		public DTOs.GenerateAssemblyInfoFileResponse GenerateAssemblyInfoFile(DTOs.GenerateAssemblyInfoFileRequest request)
		{
			var response = new DTOs.GenerateAssemblyInfoFileResponse();

			var content = new List<string>();

			content.Add("//------------------------------------------------------------------------------");
			content.Add("// <auto-generated>");
			content.Add("//     This code was generated by ISI.Extensions.VisualStudio.");
			content.Add("// </auto-generated>");
			content.Add("//------------------------------------------------------------------------------");
			content.Add(string.Empty);

			foreach (var @using in request.Usings.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				content.Add(string.Format("using {0};", @using));
			}
			content.Add(string.Empty);

			void writeAttribute(string attributeName, string attributeValue)
			{
				if (!string.IsNullOrWhiteSpace(attributeValue))
				{
					content.Add(string.Format("[assembly: Assembly{0}(\"{1}\")]", attributeName, attributeValue));
				}
			}

			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Title), request.Title);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Description), request.Description);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Guid), request.Guid);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Product), request.Product);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Copyright), request.Copyright);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Trademark), request.Trademark);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Version), request.Version);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.FileVersion), request.FileVersion);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.InformationalVersion), request.InformationalVersion);
			writeAttribute(nameof(DTOs.GenerateAssemblyInfoFileRequest.Company), request.Company);

			System.IO.File.WriteAllLines(request.AssemblyInfoFullName, content);

			return response;
		}
	}
}