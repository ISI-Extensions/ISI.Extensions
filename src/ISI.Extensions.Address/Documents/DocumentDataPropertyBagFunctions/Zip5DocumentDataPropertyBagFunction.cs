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
using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Documents
{
	[ISI.Extensions.Documents.DocumentDataPropertyBagFunction]
	public class Zip5DocumentDataPropertyBagFunction : ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		public Zip5DocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => [SourceTemplateDataKey];

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				if (values.TryGetValue(SourceTemplateDataKey, out var value) && (value != null))
				{
					return new ISI.Extensions.Documents.DocumentDataValue(ISI.Extensions.Address.Zip5($"{value}"));
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDefinition(new Zip5DocumentDataDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("0d65c5e8-f60e-4f53-8598-f5fc1edfe34a")]
	[DataContract]
	public class Zip5DocumentDataDocumentDataPropertyBagFunctionSerializableDefinitionV1 : ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition Export() => new ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDefinition(this);

		public virtual ISI.Extensions.Documents.IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction()
		{
			return new Zip5DocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey);
		}

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }
	}
}