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
		private static System.Text.RegularExpressions.Regex _contractRegex = new(@"(?m-isnx:^(?:\s*(?:(?<dataContract>(?:\[DataContract)(?:(?:\()(?:Name)(?:\s*)?(?:=)(?:\s*)?(?:\"")(?<dataContractName>[\w\d_]+)(?:\"")(?:\))?)(?:\]))|(?<class>(?:(?<accessModifier>public|protected|private)\s+)?(?:(?<accessor>abstract)\s+)?class(?:\s+)(?<className>[\w\d_]+))|(?<dataMember>(?:\[DataMember)(?:(?:\()(?:Name)(?:\s*)?(?:=)(?:\s*)?(?:\"")(?<dataMemberName>[\w\d_]+)(?:\"")(?:.*)?(?:\))?)(?:\]))|(?<property>(?:(?<accessModifier>public|protected|private)\s+)?(?:(?<accessor>abstract|virtual|override)\s+)?(?:(?<propertyType>(?:[\w\d\._]+)(?:\?)?(?:<[\w\d\._,\s\[\]]+>)?(?:\[\])?)(?:\s+)(?<propertyName>[\w\d_]+)(?:(?:\s+)(?:\{)(?:\s+)?(?:(?<getset>get;|set;)(?<thirdSpace>\s+))*(?:\})(?:\s+)(?:\=)(?:\s+)(?<defaultValue>[^;]+))?))?))(?:.+)$)");

		public DTOs.ParseClassDefinitionResponse ParseClassDefinition(DTOs.ParseClassDefinitionRequest request)
		{
			var response = new DTOs.ParseClassDefinitionResponse();

			var contract = new ClassDefinition();
			var contractProperty = new ClassPropertyDefinition();

			foreach (var line in request.Definition.Replace(",", " \n").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
			{
				var match = _contractRegex.Match(line.Trim() + " ");

				while (match.Success)
				{
					if (!string.IsNullOrEmpty(match.Groups["dataContract"].Value))
					{
						contract.HasDataContract = true;
						contract.DataContractName = match.Groups["dataContractName"].Value;
					}

					if (!string.IsNullOrEmpty(match.Groups["class"].Value))
					{
						if (!string.IsNullOrEmpty(match.Groups["accessModifier"].Value))
						{
							contract.AccessModifier = match.Groups["accessModifier"].Value;
						}

						contract.Accessor = match.Groups["accessor"].Value;
						contract.ClassName = match.Groups["className"].Value;
					}

					if (!string.IsNullOrEmpty(match.Groups["dataMember"].Value))
					{
						contractProperty.HasDataMember = true;
						contractProperty.DataMemberName = match.Groups["dataMemberName"].Value;
					}

					if (!string.IsNullOrEmpty(match.Groups["property"].Value))
					{
						if (!string.IsNullOrEmpty(match.Groups["accessModifier"].Value))
						{
							contractProperty.AccessModifier = match.Groups["accessModifier"].Value;
						}

						contractProperty.Accessor = match.Groups["accessor"].Value;
						contractProperty.PropertyType = match.Groups["propertyType"].Value;
						contractProperty.PropertyName = match.Groups["propertyName"].Value;
						contractProperty.DefaultValue = match.Groups["defaultValue"].Value;

						if (contractProperty.PropertyType.EndsWith("[]"))
						{
							contractProperty.IsPropertyIEnumerable = true;
						}
						else if (contractProperty.PropertyType.IndexOf("Enumerable<", StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							contractProperty.IsPropertyIEnumerable = true;
						}
						else if (contractProperty.PropertyType.EndsWith("Collection"))
						{
							contractProperty.IsPropertyCollection = true;
						}

						contract.Properties.Add(contractProperty);

						contractProperty = new();
					}

					match = match.NextMatch();
				}
			}

			if (!contract.Properties.Any() && string.IsNullOrEmpty(contract.ClassName))
			{
				var propertyNames = request.Definition.Split(new[] { ",", "|", "\t", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToArray();

				var hasPropertyTypes = false; //propertyNames.All(propertyName => propertyName.IndexOf(" ", StringComparison.Ordinal) > 0);

				if (propertyNames.Any())
				{
					foreach (var propertyName in propertyNames)
					{
						contractProperty = new()
						{
							PropertyType = "string",
							PropertyName = ISI.Extensions.StringFormat.PascalCase(propertyName)
						};

						if (propertyName.EndsWith("Guid", StringComparison.InvariantCultureIgnoreCase) || propertyName.EndsWith("Uuid", StringComparison.InvariantCultureIgnoreCase))
						{
							contractProperty.PropertyType = "Guid";
						}
						else if (propertyName.EndsWith("DateTime", StringComparison.InvariantCultureIgnoreCase) || propertyName.EndsWith("Date", StringComparison.InvariantCultureIgnoreCase) || propertyName.EndsWith("Time", StringComparison.InvariantCultureIgnoreCase))
						{
							contractProperty.PropertyType = "DateTime";
						}
						else if (propertyName.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase))
						{
							contractProperty.PropertyType = "int";
						}

						if (hasPropertyTypes)
						{
							var parsedField = new Queue<string>(propertyName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

							contractProperty.PropertyType = parsedField.Dequeue();
							contractProperty.PropertyName = ISI.Extensions.StringFormat.PascalCase(string.Join(" ", parsedField));
						}

						contract.Properties.Add(contractProperty);
					}
				}
			}

			response.ClassDefinition = contract;

			return response;
		}
	}
}