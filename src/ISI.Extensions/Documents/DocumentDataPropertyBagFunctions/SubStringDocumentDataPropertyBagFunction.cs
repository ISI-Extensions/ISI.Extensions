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
	public class SubStringDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("StartIndex")]
		public int StartIndex { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Length")]
		public int? Length { get; set; }

		public SubStringDocumentDataPropertyBagFunction()
		{

		}

		public SubStringDocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey, int startIndex, int? length = null)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
			StartIndex = startIndex;
			Length = length;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => [SourceTemplateDataKey];

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				if (values.TryGetValue(SourceTemplateDataKey, out var value) && (value != null))
				{
					var stringValue = $"{value}";

					if (Length.HasValue)
					{
						if (stringValue.Length >= (StartIndex + Length))
						{
							return new ISI.Extensions.Documents.DocumentDataValue(stringValue.Substring(StartIndex, Length.Value));
						}

						return null;
					}

					if (stringValue.Length >= StartIndex)
					{
						return new ISI.Extensions.Documents.DocumentDataValue(stringValue.Substring(StartIndex));
					}
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new SubStringDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
				StartIndex = StartIndex,
				Length = Length,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("f0805e42-905c-49dd-9699-f23ae011899b")]
	[DataContract]
	public class SubStringDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new SubStringDocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey, StartIndex, Length);

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }

		[DataMember(Name = "startIndex", EmitDefaultValue = false)]
		public int StartIndex { get; set; }

		[DataMember(Name = "length", EmitDefaultValue = false)]
		public int? Length { get; set; }
	}
}