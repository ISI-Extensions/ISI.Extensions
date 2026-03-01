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
	public class SwitchDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		public SwitchDocumentDataPropertyBagFunctionCase[] SwitchCases { get; set; }

		public SwitchDocumentDataPropertyBagFunction()
		{

		}

		public SwitchDocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey, SwitchDocumentDataPropertyBagFunctionCase[] switchCases = null)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
			SwitchCases = switchCases;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => [SourceTemplateDataKey];

		private object GetValue(IDictionary<string, object> values, string key)
		{
			if (!string.IsNullOrWhiteSpace(key) && values.TryGetValue(key, out var value))
			{
				return value;
			}

			return null;
		}

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				var fieldValue = $"{GetValue(values, SourceTemplateDataKey)}";

				foreach (var switchCase in SwitchCases)
				{
					if (string.Equals(fieldValue, switchCase.IsEqualToValue, StringComparison.InvariantCultureIgnoreCase))
					{
						var matchValue = switchCase.MatchValue;

						if (string.IsNullOrWhiteSpace(switchCase.MatchValueSourceTemplateDataKey))
						{
							matchValue = $"{GetValue(values, switchCase.MatchValueSourceTemplateDataKey)}";
						}

						return (string.IsNullOrWhiteSpace(matchValue) ? null : new ISI.Extensions.Documents.DocumentDataValue(matchValue));
					}
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new SwitchDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
				SwitchCases = SwitchCases.ToNullCheckedArray(SwitchDocumentDataPropertyBagFunctionCaseSerializableDefinitionV1.ToSerializable),
			});
		}
	}

	public class SwitchDocumentDataPropertyBagFunctionCase
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Is Equal To Value")]
		public string IsEqualToValue { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("MatchValue")]
		public string MatchValue { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string MatchValueSourceTemplateDataKey { get; set; }
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("be6e5630-6a23-418f-a61f-e336fbdb1fc0")]
	[DataContract]
	public class SwitchDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new SwitchDocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey, SwitchCases.ToNullCheckedArray(switchCase => switchCase.Export()));

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }

		[DataMember(Name = "switchCases", EmitDefaultValue = false)]
		public SwitchDocumentDataPropertyBagFunctionCaseSerializableDefinitionV1[] SwitchCases { get; set; }
	}

	[DataContract]
	public class SwitchDocumentDataPropertyBagFunctionCaseSerializableDefinitionV1
	{
		public SwitchDocumentDataPropertyBagFunctionCase Export()
		{
			return new SwitchDocumentDataPropertyBagFunctionCase()
			{
				IsEqualToValue = IsEqualToValue,
				MatchValue = MatchValue,
				MatchValueSourceTemplateDataKey = MatchValueSourceTemplateDataKey,
			};
		}

		public static SwitchDocumentDataPropertyBagFunctionCaseSerializableDefinitionV1 ToSerializable(SwitchDocumentDataPropertyBagFunctionCase source)
		{
			return new SwitchDocumentDataPropertyBagFunctionCaseSerializableDefinitionV1()
			{
				IsEqualToValue = source.IsEqualToValue,
				MatchValue = source.MatchValue,
				MatchValueSourceTemplateDataKey = source.MatchValueSourceTemplateDataKey,
			};
		}

		[DataMember(Name = "isEqualToValue", EmitDefaultValue = false)]
		public string IsEqualToValue { get; set; }

		[DataMember(Name = "matchValue", EmitDefaultValue = false)]
		public string MatchValue { get; set; }

		[DataMember(Name = "matchValueSourceTemplateDataKey", EmitDefaultValue = false)]
		public string MatchValueSourceTemplateDataKey { get; set; }
	}
}