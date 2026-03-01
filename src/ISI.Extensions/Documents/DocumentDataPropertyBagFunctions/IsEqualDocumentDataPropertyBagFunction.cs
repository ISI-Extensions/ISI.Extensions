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
	public class IsEqualDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Is Equal To Value")]
		public string IsEqualToValue { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("TrueValue")]
		public string TrueValue { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("FalseValue")]
		public string FalseValue { get; set; }

		public IsEqualDocumentDataPropertyBagFunction()
		{

		}

		public IsEqualDocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey, string isEqualToValue, string trueValue, string falseValue = null)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
			IsEqualToValue = isEqualToValue;
			TrueValue = trueValue;
			FalseValue = falseValue;
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
				var fieldValue = GetValue(values, SourceTemplateDataKey);

				if (string.Equals($"{fieldValue}", IsEqualToValue, StringComparison.InvariantCultureIgnoreCase))
				{
					return (string.IsNullOrWhiteSpace(TrueValue) ? null : new ISI.Extensions.Documents.DocumentDataValue(TrueValue));
				}

				return (string.IsNullOrWhiteSpace(FalseValue) ? null : new ISI.Extensions.Documents.DocumentDataValue(FalseValue));
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new IsEqualDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
				IsEqualToValue = IsEqualToValue,
				TrueValue = TrueValue,
				FalseValue = FalseValue,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("0decc170-2501-415b-9017-1faabb494be7")]
	[DataContract]
	public class IsEqualDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new IsEqualDocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey, IsEqualToValue, TrueValue, FalseValue);

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }

		[DataMember(Name = "isEqualToValue", EmitDefaultValue = false)]
		public string IsEqualToValue { get; set; }

		[DataMember(Name = "trueValue", EmitDefaultValue = false)]
		public string TrueValue { get; set; }

		[DataMember(Name = "falseValue", EmitDefaultValue = false)]
		public string FalseValue { get; set; }
	}
}