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
		public DTOs.GenerateClassDefinitionResponse GenerateClassDefinition(DTOs.GenerateClassDefinitionRequest request)
		{
			var response = new DTOs.GenerateClassDefinitionResponse();

			var formattedText = new System.Text.StringBuilder();

			if (!ISI.Extensions.VisualStudio.CodeExtensionProviders.TryGetCodeExtensionProvider(request.CodeExtensionProviderUuid, out var codeExtensionProvider))
			{
				throw new Exception("CodeExtensionProvider not found");
			}

			var extensionsNamespace = codeExtensionProvider.Namespace;

			var columnOffset = 0;

			if (!string.IsNullOrEmpty(request.ClassDefinition.ClassName))
			{
				switch (request.IncludeDataContractAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					case IncludePropertyAttribute.YesWithNoNaming:
						formattedText.AppendFormat("\t[DataContract]{0}", Environment.NewLine);
						break;
					default:
						formattedText.AppendFormat("\t[DataContract(Name = \"{1}\")]{0}", Environment.NewLine, FormatAttributeName(request.IncludeDataContractAttributes, request.ClassDefinition.ClassName, request.ClassDefinition.DataContractName));
						break;
				}

				switch (request.IncludeRepositoryAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					case IncludePropertyAttribute.YesWithNoNaming:
						formattedText.AppendFormat("\t[{1}.Repository.Record]{0}", Environment.NewLine, extensionsNamespace);
						break;
					default:
						formattedText.AppendFormat("\t[{1}.Repository.Record(TableName = \"{2}\")]{0}", Environment.NewLine, extensionsNamespace, FormatAttributeName(request.IncludeRepositoryAttributes, request.ClassDefinition.ClassName, request.ClassDefinition.DataContractName));
						break;
				}

				switch (request.IncludeDocumentDataAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					default:
						if (request.IncludeDocumentDataAttributes == IncludePropertyAttribute.No)
						{
							formattedText.AppendFormat("\t[{1}.Documents.DocumentData]{0}", Environment.NewLine, extensionsNamespace);
						}
						else
						{
							formattedText.AppendFormat("\t[{1}.Documents.DocumentData(\"{2}\")]{0}", Environment.NewLine, extensionsNamespace, FormatAttributeName(request.IncludeDocumentDataAttributes, request.ClassDefinition.ClassName, request.ClassDefinition.DataContractName));
						}
						break;
				}

				formattedText.AppendFormat("\t{1}{2}class {3}{0}", Environment.NewLine, (string.IsNullOrEmpty(request.ClassDefinition.AccessModifier) ? string.Empty : string.Format("{0} ", request.ClassDefinition.AccessModifier)), (string.IsNullOrEmpty(request.ClassDefinition.Accessor) ? string.Empty : string.Format("{0} ", request.ClassDefinition.Accessor)), request.ClassDefinition.ClassName);
				formattedText.AppendFormat("\t{{{0}", Environment.NewLine);
			}

			formattedText.AppendFormat("{1}{0}", Environment.NewLine, string.Join(string.Format("{0}{1}", Environment.NewLine, ((request.IncludeDataContractAttributes == IncludePropertyAttribute.No) && (request.IncludeRepositoryAttributes == IncludePropertyAttribute.No) && (request.IncludeDocumentDataAttributes == IncludePropertyAttribute.No) && !request.IncludeSpreadSheetsAttributes ? string.Empty : Environment.NewLine)), request.ClassDefinition.Properties.Select(property =>
			{
				var propertyFormattedText = new StringBuilder();

				switch (request.IncludeDataContractAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					case IncludePropertyAttribute.YesWithNoNaming:
						if (request.EmitDefaultValueFalse)
						{
							propertyFormattedText.AppendFormat("\t\t[DataMember(EmitDefaultValue = false)]{0}", Environment.NewLine);
						}
						else
						{
							propertyFormattedText.AppendFormat("\t\t[DataMember]{0}", Environment.NewLine);
						}

						break;
					default:
						propertyFormattedText.AppendFormat("\t\t[DataMember(Name = \"{1}\"{2})]{0}", Environment.NewLine, FormatAttributeName(request.IncludeDataContractAttributes, property.PropertyName, property.DataMemberName), (request.EmitDefaultValueFalse ? ", EmitDefaultValue = false" : string.Empty));
						if ((request.PreferredSerializer == PreferredSerializer.Json) && (string.Equals(property.PropertyType, "DateTime", StringComparison.InvariantCulture) || string.Equals(property.PropertyType, "DateTime?", StringComparison.InvariantCulture)))
						{
							propertyFormattedText.AppendFormat("\t\t{1}{2}string __{4} {{ get {{ return {4}.Formatted(Formatters.DateTimeFormat.DateTimeUniversalPrecise); }} set {{ {4} = value.{5}(DateTimeKind.Utc); }} }}{0}", Environment.NewLine, (string.IsNullOrEmpty(property.AccessModifier) ? string.Empty : string.Format("{0} ", property.AccessModifier)), (string.IsNullOrEmpty(property.Accessor) ? string.Empty : string.Format("{0} ", property.Accessor)), property.PropertyType, FormatString(StringCaseFormat.No, property.PropertyName), (string.Equals(property.PropertyType, "DateTime?", StringComparison.CurrentCulture) ? "ToDateTimeNullable" : "ToDateTime"));
							propertyFormattedText.AppendFormat("\t\t[IgnoreDataMember]{0}", Environment.NewLine);
						}

						break;
				}

				switch (request.IncludeRepositoryAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					case IncludePropertyAttribute.YesWithNoNaming:
						propertyFormattedText.AppendFormat("\t\t[{1}.Repository.RecordProperty]{0}", Environment.NewLine, extensionsNamespace);
						break;
					default:
						propertyFormattedText.AppendFormat("\t\t[{1}.Repository.RecordProperty(ColumnName = \"{2}\")]{0}", Environment.NewLine, extensionsNamespace, FormatAttributeName(request.IncludeRepositoryAttributes, property.PropertyName, property.DataMemberName));
						break;
				}

				switch (request.IncludeDocumentDataAttributes)
				{
					case IncludePropertyAttribute.No:
						break;
					case IncludePropertyAttribute.YesWithNoNaming:
						propertyFormattedText.AppendFormat("\t\t[{1}.Documents.DocumentDataValue]{0}", Environment.NewLine, extensionsNamespace);
						break;
					default:
						propertyFormattedText.AppendFormat("\t\t[{1}s.Documents.DocumentDataValue(\"{2}\")]{0}", Environment.NewLine, extensionsNamespace, FormatAttributeName(request.IncludeDocumentDataAttributes, property.PropertyName, property.DataMemberName));
						break;
				}

				if (request.IncludeSpreadSheetsAttributes)
				{
					propertyFormattedText.AppendFormat("\t\t[{1}.SpreadSheets.Column({2}, HeaderCaption = \"{3}\")]{0}", Environment.NewLine, extensionsNamespace, columnOffset++, ISI.Extensions.StringFormat.SplitCase(property.PropertyName));
				}

				propertyFormattedText.AppendFormat("\t\t{1}{2}{3} {4} {{ get; set; }}", Environment.NewLine, (string.IsNullOrEmpty(property.AccessModifier) ? string.Empty : string.Format("{0} ", property.AccessModifier)), (string.IsNullOrEmpty(property.Accessor) ? string.Empty : string.Format("{0} ", property.Accessor)), property.PropertyType, FormatString(request.FormatPropertyName, property.PropertyName));

				return propertyFormattedText.ToString();
			})));

			if (!string.IsNullOrEmpty(request.ClassDefinition.ClassName))
			{
				formattedText.AppendFormat("\t}}{0}", Environment.NewLine);
			}

			response.ClassDefinition = formattedText.ToString();

			return response;
		}
	}
}