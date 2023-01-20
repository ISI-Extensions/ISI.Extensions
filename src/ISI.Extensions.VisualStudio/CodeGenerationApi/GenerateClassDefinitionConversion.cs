#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public DTOs.GenerateClassDefinitionConversionResponse GenerateClassDefinitionConversion(DTOs.GenerateClassDefinitionConversionRequest request)
		{
			var response = new DTOs.GenerateClassDefinitionConversionResponse();

			var formattedText = new System.Text.StringBuilder();

			var sourceEntityName = (string.IsNullOrWhiteSpace(request.SourceEntityName.NullCheckedTrimEnd('.')) ? string.Empty : string.Format("{0}.", request.SourceEntityName.TrimEnd('.')));
			var targetEntityName = (string.IsNullOrWhiteSpace(request.TargetEntityName.NullCheckedTrimEnd('.')) ? string.Empty : string.Format("{0}.", request.TargetEntityName.TrimEnd('.')));


			foreach (var property in request.ClassDefinition.Properties)
			{
				if (property.IsPropertyIEnumerable)
				{
					var propertyType = property.PropertyType
						.TrimStart("IEnumerable<")
						.TrimStart("Enumerable<")
						.TrimEnd('[', ']', '>');

					if (string.Equals(propertyType, "string", StringComparison.InvariantCultureIgnoreCase) ||
							string.Equals(propertyType, "int", StringComparison.InvariantCultureIgnoreCase) ||
							string.Equals(propertyType, "long", StringComparison.InvariantCultureIgnoreCase) ||
							string.Equals(propertyType, "Guid", StringComparison.InvariantCultureIgnoreCase))
					{
						formattedText.AppendFormat("{3}{1} = {2}{1}.ToNullCheckedArray(){4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), sourceEntityName, targetEntityName, request.ConversionSeparator);
					}
					else
					{
						formattedText.AppendFormat("{3}{1} = {2}{1}.ToNullCheckedArray(Convert){4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), sourceEntityName, targetEntityName, request.ConversionSeparator);
					}
				}
				else if (property.IsPropertyCollection)
				{
					//1 -> propertyName
					//2 -> propertyType
					//3 -> propertyTypeCollection
					//4 -> source
					formattedText.AppendFormat("{5}{1} = {4}{1}.ToNullCheckedCollection<{2}, {2}, {3}>(Convert),{0}", Environment.NewLine,
						/* 1 */ FormatString(request.FormatPropertyName, property.PropertyName),
						/* 2 */ property.PropertyType,
						/* 3 */ property.PropertyType.Substring(0, property.PropertyType.Length - "Collection".Length),
						/* 4 */ sourceEntityName,
						/* 5 */ targetEntityName);
				}
				else
				{
					formattedText.AppendFormat("{3}{1} = {2}{1}{4}{0}", Environment.NewLine, FormatString(request.FormatPropertyName, property.PropertyName), sourceEntityName, targetEntityName, request.ConversionSeparator);
				}
			}

			response.Content = formattedText.ToString();

			return response;
		}
	}
}