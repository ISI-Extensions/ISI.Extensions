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
		public DTOs.RemoveProjectNugetPackagesFromCsProjResponse RemoveProjectNugetPackagesFromCsProj(DTOs.RemoveProjectNugetPackagesFromCsProjRequest request)
		{
			var response = new DTOs.RemoveProjectNugetPackagesFromCsProjResponse();

			var csProjXml = System.Xml.Linq.XElement.Load(request.CsProjFullName);
			
			var sdkAttribute = csProjXml.GetAttributeByLocalName("Sdk")?.Value ?? string.Empty;

			var isDirty = false;

			var nugetPackageNames = new HashSet<string>(request.NugetPackageNames, StringComparer.InvariantCultureIgnoreCase);

			foreach (var itemGroup in csProjXml.GetElementsByLocalName("ItemGroup"))
			{
				var packageReferences = itemGroup.GetElementsByLocalName("PackageReference");

				foreach (var packageReference in packageReferences)
				{
					var packageId = (packageReference.GetAttributeByLocalName("Include") ?? packageReference.GetAttributeByLocalName("Update"))?.Value;

					if (nugetPackageNames.Contains(packageId))
					{
						packageReference.Remove();
						isDirty = true;
					}
				}
			}

			foreach (var itemGroup in csProjXml.GetElementsByLocalName("ItemGroup"))
			{
				var references = itemGroup.GetElementsByLocalName("Reference");

				foreach (var reference in references)
				{
					var packageId = reference.GetAttributeByLocalName("Include")?.Value;

					var keyValues = $"PackageId={packageId}".Split([','], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(["="], StringSplitOptions.None)).ToDictionary(item => item[0].Trim(), item => (item.Length >= 2 ? item[1].Trim() : string.Empty), StringComparer.CurrentCultureIgnoreCase);
					keyValues.TryGetValue("PackageId", out packageId);

					if (nugetPackageNames.Contains(packageId))
					{
						reference.Remove();
						isDirty = true;
					}
				}
			}

			if (isDirty)
			{
				var csProj = csProjXml.ToString();

				if (!sdkAttribute.StartsWith("Microsoft.NET", StringComparison.InvariantCultureIgnoreCase))
				{
					csProj = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n{csProj}";
				}
				
				System.IO.File.WriteAllText(request.CsProjFullName, csProj);
			}

			return response;
		}
	}
}