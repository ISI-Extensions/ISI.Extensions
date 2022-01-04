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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeGenerationApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeGenerationApi
	{
		public DTOs.GenerateClassDefinitionConversionResponse GenerateClassDefinitionConversion(DTOs.GenerateClassDefinitionConversionRequest request)
		{
			var response = new DTOs.GenerateClassDefinitionConversionResponse();
			
			var formattedText = new System.Text.StringBuilder();

			if (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Request)
			{
				request.ClassDefinitionConversionType = DTOs.ClassDefinitionConversionType.Default;
			}

			request.EntityNameSpace = (string.IsNullOrEmpty(request.EntityNameSpace) ? string.Empty : (request.EntityNameSpace.EndsWith(".") ? request.EntityNameSpace : string.Format("{0}.", request.EntityNameSpace)));
			request.LocalEntityNameSpace = (string.IsNullOrEmpty(request.LocalEntityNameSpace) ? string.Empty : (request.LocalEntityNameSpace.EndsWith(".") ? request.LocalEntityNameSpace : string.Format("{0}.", request.LocalEntityNameSpace)));
			request.DomainEntityNameSpace = (string.IsNullOrEmpty(request.DomainEntityNameSpace) ? string.Empty : (request.DomainEntityNameSpace.EndsWith(".") ? request.DomainEntityNameSpace : string.Format("{0}.", request.DomainEntityNameSpace)));
			request.ServiceBusNameSpace = (string.IsNullOrEmpty(request.ServiceBusNameSpace) ? string.Empty : (request.ServiceBusNameSpace.EndsWith(".") ? request.ServiceBusNameSpace : string.Format("{0}.", request.ServiceBusNameSpace)));
			request.RepositoryNameSpace = (string.IsNullOrEmpty(request.RepositoryNameSpace) ? string.Empty : (request.RepositoryNameSpace.EndsWith(".") ? request.RepositoryNameSpace : string.Format("{0}.", request.RepositoryNameSpace)));

			Action<string, string, string, string, string> createBody = (fromNameSpace, toNameSpace, source, destination, conversionName) =>
			{
				foreach (var property in request.ClassDefinition.Properties)
				{
					if (property.PropertyType.EndsWith("[]"))
					{
						var propertyType = property.PropertyType.Substring(0, property.PropertyType.Length - "[]".Length);

						if (string.Equals(propertyType, "string", StringComparison.InvariantCultureIgnoreCase) ||
								string.Equals(propertyType, "int", StringComparison.InvariantCultureIgnoreCase) ||
								string.Equals(propertyType, "long", StringComparison.InvariantCultureIgnoreCase) ||
								string.Equals(propertyType, "Guid", StringComparison.InvariantCultureIgnoreCase))
						{
							formattedText.AppendFormat("\t\t\t\t{3}{1} = {2}{1}.ToNullCheckedArray(){4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), source, destination, (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Assignment ? ";" : ","));
						}
						else
						{
							formattedText.AppendFormat("\t\t\t\t{3}{1} = {2}{1}.ToNullCheckedArray({5}){4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), source, destination, (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Assignment ? ";" : ","), conversionName.Replace("{propertyType}", propertyType).Replace("{camelCasePropertyName}", ISI.Extensions.StringFormat.CamelCase(property.PropertyName)));
						}
					}
					else if (property.PropertyType.EndsWith("Collection"))
					{
						//1 -> propertyName
						//2 -> propertyType
						//3 -> propertyTypeCollection
						//4 -> fromNameSpace
						//5 -> toNameSpace
						//6 -> source
						//7 -> conversionName
						formattedText.AppendFormat("\t\t\t\t{1} = {6}{1}.ToNullCheckedCollection<{4}{2}, {5}{2}, {5}{3}>({7}),{0}", Environment.NewLine,
							/* 1 */ FormatString(request.FormatPropertyName, property.PropertyName),
							/* 2 */ property.PropertyType,
							/* 3 */ property.PropertyType.Substring(0, property.PropertyType.Length - "Collection".Length),
							/* 4 */ fromNameSpace,
							/* 5 */ toNameSpace,
							/* 6 */ source,
							/* 7 */ conversionName);
					}
					else
					{
						formattedText.AppendFormat("\t\t\t\t{3}{1} = {2}{1}{4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), source, destination, (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Assignment ? ";" : ","));
					}
				}
			};

			#region Default
			if (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Default)
			{
				if (!string.IsNullOrEmpty(request.ClassDefinition.ClassName))
				{
					formattedText.AppendFormat("\t\tpublic static {1} Convert(SourceNamespace.{1} {2}){0}", Environment.NewLine, request.ClassDefinition.ClassName, request.SourceIteratorName);
					formattedText.AppendFormat("\t\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t\treturn {2}.NullCheckedConvert<SourceNamespace.{1}, {1}>(value => new {1}{0}", Environment.NewLine, request.ClassDefinition.ClassName, request.SourceIteratorName);
					formattedText.AppendFormat("\t\t\t{{{0}", Environment.NewLine);
				}

				createBody(request.EntityNameSpace, "", string.Format("{0}.", request.SourceIteratorName), "", "Convert");

				if (!string.IsNullOrEmpty(request.ClassDefinition.ClassName))
				{
					formattedText.AppendFormat("\t\t\t}});{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t}}{0}", Environment.NewLine);
				}
			}
			#endregion

			#region ServiceBusLocal
			if ((request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.ServiceBusLocal) || (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.ServiceBusDomain))
			{
				if (string.IsNullOrEmpty(request.ClassDefinition.ClassName))
				{
					createBody("SOURCE.", "TARGET.", string.Format("{0}.", request.SourceIteratorName), "", "Convert");
				}
				else
				{
					request.EntityNameSpace = (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.ServiceBusLocal ? request.LocalEntityNameSpace : request.DomainEntityNameSpace);

					var conversionName = "ToServiceBus";

					formattedText.AppendFormat("\t\tpublic {5}{1} {4}({3}{1} {2}){0}", Environment.NewLine, request.ClassDefinition.ClassName, ISI.Extensions.StringFormat.CamelCase(request.ClassDefinition.ClassName), request.EntityNameSpace, conversionName, request.ServiceBusNameSpace);
					formattedText.AppendFormat("\t\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t\treturn {2}.NullCheckedConvert({4} => new {3}{1}{0}", Environment.NewLine, request.ClassDefinition.ClassName, ISI.Extensions.StringFormat.CamelCase(request.ClassDefinition.ClassName), request.ServiceBusNameSpace, request.SourceIteratorName);
					formattedText.AppendFormat("\t\t\t{{{0}", Environment.NewLine);

					createBody(request.EntityNameSpace, request.ServiceBusNameSpace, string.Format("{0}.", request.SourceIteratorName), "", conversionName);

					formattedText.AppendFormat("\t\t\t}});{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t}}{0}", Environment.NewLine);

					formattedText.AppendLine();

					conversionName = (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.ServiceBusLocal ? "ToLocalDomain" : "ToDomainEntity");

					formattedText.AppendFormat("\t\tpublic {3}{1} {4}({5}{1} {2}){0}", Environment.NewLine, request.ClassDefinition.ClassName, ISI.Extensions.StringFormat.CamelCase(request.ClassDefinition.ClassName), request.EntityNameSpace, conversionName, request.ServiceBusNameSpace);
					formattedText.AppendFormat("\t\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t\treturn {2}.NullCheckedConvert({4} => new {3}{1}{0}", Environment.NewLine, request.ClassDefinition.ClassName, ISI.Extensions.StringFormat.CamelCase(request.ClassDefinition.ClassName), request.EntityNameSpace, request.SourceIteratorName);
					formattedText.AppendFormat("\t\t\t{{{0}", Environment.NewLine);

					createBody(request.ServiceBusNameSpace, request.EntityNameSpace, string.Format("{0}.", request.SourceIteratorName), "", conversionName);

					formattedText.AppendFormat("\t\t\t}});{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t}}{0}", Environment.NewLine);
				}
			}
			#endregion

			#region Repository
			if (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Repository)
			{
				var objectAlias = (string.IsNullOrEmpty(request.ClassDefinition.ClassName) ? "value" : ISI.Extensions.StringFormat.CamelCase(request.ClassDefinition.ClassName));

				if (string.IsNullOrEmpty(request.ClassDefinition.ClassName))
				{
					createBody(request.EntityNameSpace, request.RepositoryNameSpace, string.Format("{0}.", request.SourceIteratorName), "", "ToSerializable");
				}
				else
				{
					formattedText.AppendFormat("ISI.Libraries.Converters.IExportTo<{2}{1}>{0}", Environment.NewLine, request.ClassDefinition.ClassName, request.EntityNameSpace);
					formattedText.AppendFormat("\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\tpublic static {3}{1} ToSerializable({2}{1} {4}){0}", Environment.NewLine, request.ClassDefinition.ClassName, request.EntityNameSpace, request.RepositoryNameSpace, objectAlias);
					formattedText.AppendFormat("\t\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t\treturn {3}.NullCheckedConvert({3} => new {2}{1}{0}", Environment.NewLine, request.ClassDefinition.ClassName, request.RepositoryNameSpace, objectAlias, request.SourceIteratorName);
					formattedText.AppendFormat("\t\t\t{{{0}", Environment.NewLine);

					createBody(request.EntityNameSpace, request.RepositoryNameSpace, string.Format("{0}.", request.SourceIteratorName), "", "{propertyType}.ToSerializable");

					formattedText.AppendFormat("\t\t\t}});{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t}}{0}", Environment.NewLine);

					formattedText.Append(Environment.NewLine);

					formattedText.AppendFormat("\t\tpublic {2}{1} Export(){0}", Environment.NewLine, request.ClassDefinition.ClassName, request.EntityNameSpace);
					formattedText.AppendFormat("\t\t{{{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t\treturn new {2}{1}{0}", Environment.NewLine, request.ClassDefinition.ClassName, request.EntityNameSpace);
					formattedText.AppendFormat("\t\t\t{{{0}", Environment.NewLine);

					createBody(request.RepositoryNameSpace, request.EntityNameSpace, string.Empty, "", "{camelCasePropertyName} => {camelCasePropertyName}.Export()");

					formattedText.AppendFormat("\t\t\t}};{0}", Environment.NewLine);
					formattedText.AppendFormat("\t\t}}{0}", Environment.NewLine);
				}
			}
			#endregion

			#region Assignment
			if (request.ClassDefinitionConversionType == DTOs.ClassDefinitionConversionType.Assignment)
			{
				createBody(request.EntityNameSpace, "", string.Format("{0}.", request.SourceIteratorName), string.Format("{0}.", request.TargetIteratorName), "Convert");
			}
			#endregion

			response.ClassDefinitionConversion= formattedText.ToString();

			return response;
		}
	}
}