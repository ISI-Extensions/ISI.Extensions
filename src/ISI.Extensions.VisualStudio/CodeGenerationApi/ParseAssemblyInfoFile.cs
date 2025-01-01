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
		private static readonly System.Text.RegularExpressions.Regex[] _assemblyInfoRegexes =
		[
			new System.Text.RegularExpressions.Regex(@"^\s*\[assembly: (?:System\.Reflection\.)?(?<attributeKey>.*) ?\((?<attributeValue>.*)\)", System.Text.RegularExpressions.RegexOptions.Multiline)
		];

		public DTOs.ParseAssemblyInfoFileResponse ParseAssemblyInfoFile(DTOs.ParseAssemblyInfoFileRequest request)
		{
			var response = new DTOs.ParseAssemblyInfoFileResponse();

			if (System.IO.File.Exists(request.AssemblyInfoFullName))
			{
				var content = System.IO.File.ReadAllText(request.AssemblyInfoFullName);

				var attributeValues = new Dictionary<string, HashSet<string>>(StringComparer.InvariantCultureIgnoreCase);

				foreach (var assemblyInfoRegex in _assemblyInfoRegexes)
				{
					foreach (System.Text.RegularExpressions.Match match in assemblyInfoRegex.Matches(content))
					{
						if (match.Groups.Count > 0)
						{
							var attributeKey = match.Groups["attributeKey"].Value.Trim().TrimStart("Assembly").TrimEnd("Attribute");
							var attributeValue = match.Groups["attributeValue"].Value.Trim('\"', ' ');
							if (!string.IsNullOrWhiteSpace(attributeKey) && !string.IsNullOrWhiteSpace(attributeValue))
							{
								if (!attributeValues.TryGetValue(attributeKey, out var values))
								{
									values = new(StringComparer.InvariantCultureIgnoreCase);
									attributeValues.Add(attributeKey, values);
								}

								values.Add(attributeValue);
							}
						}
					}
				}

				string readAttributeValue(string attributeName)
				{
					if (attributeValues.TryGetValue(attributeName, out var values))
					{
						return values.NullCheckedFirstOrDefault() ?? string.Empty;
					}

					return string.Empty;
				}

				response.Title = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Title));
				response.Description = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Description));
				response.Guid = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Guid));
				response.Product = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Product));
				response.Copyright = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Copyright));
				response.Trademark = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Trademark));
				response.Version = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Version));
				response.FileVersion = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.FileVersion));
				response.InformationalVersion = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.InformationalVersion));
				response.Company = readAttributeValue(nameof(DTOs.ParseAssemblyInfoFileResponse.Company));
			}

			return response;
		}
	}
}