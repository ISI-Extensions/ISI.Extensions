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
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Documents
{
	[ISI.Extensions.Documents.DocumentDataPropertyBagFunction]
	public class ConcatenationDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Delimiter")]
		public string Delimiter { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string[] SourceTemplateDataKeys { get; set; }

		public ConcatenationDocumentDataPropertyBagFunction()
		{

		}

		public ConcatenationDocumentDataPropertyBagFunction(string templateMergeKey, string delimiter, params string[] sourceTemplateDataKeys)
		{
			TemplateMergeKey = templateMergeKey;
			Delimiter = delimiter;
			SourceTemplateDataKeys = sourceTemplateDataKeys.ToNullCheckedArray(NullCheckCollectionResult.Empty);
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => SourceTemplateDataKeys ?? [];

		private string GetValue(IDictionary<string, object> values, string key)
		{
			if (!string.IsNullOrWhiteSpace(key) && values.TryGetValue(key, out var value) && (value != null))
			{
				if (value is string stringValue)
				{
					return stringValue;
				}

				return $"{value}";
			}

			return string.Empty;
		}

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				var formattedAddress = new StringBuilder();

				void addValue(string value)
				{
					if (!string.IsNullOrWhiteSpace(value))
					{
						if (formattedAddress.Length > 0)
						{
							formattedAddress.Append(Delimiter);
						}

						formattedAddress.Append(value);
					}
				}

				foreach (var sourceTemplateDataKey in SourceTemplateDataKeys)
				{
					addValue(GetValue(values, sourceTemplateDataKey));
				}

				if (formattedAddress.Length > 0)
				{
					return new ISI.Extensions.Documents.DocumentDataValue(formattedAddress.ToString());
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new ConcatenationDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				Delimiter = Delimiter,
				SourceTemplateDataKeys = SourceTemplateDataKeys,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("fbe9141d-b04a-4b22-a493-f3d3ce7d041a")]
	[DataContract]
	public class ConcatenationDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new ConcatenationDocumentDataPropertyBagFunction(TemplateMergeKey, Delimiter, SourceTemplateDataKeys);

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "delimiter", EmitDefaultValue = false)]
		public string Delimiter { get; set; }

		[DataMember(Name = "sourceTemplateDataKeys", EmitDefaultValue = false)]
		public string[] SourceTemplateDataKeys { get; set; }
	}
}