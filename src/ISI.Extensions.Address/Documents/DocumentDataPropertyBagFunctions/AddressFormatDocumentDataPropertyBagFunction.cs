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
	public class AddressFormatDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for FirstName")]
		public string FirstNameSourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for LastName")]
		public string LastNameSourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for CompanyName")]
		public string CompanyNameSourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for AddressLine1")]
		public string AddressLine1SourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for AddressLine2")]
		public string AddressLine2SourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for City")]
		public string CitySourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for State")]
		public string StateSourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Abbreviate State")]
		public bool AbbreviateState { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey("Source Template Data Key for PostalCode")]
		public string PostalCodeSourceTemplateDataKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Force To Upper Case")]
		public bool ForceToUpperCase { get; set; }

		public AddressFormatDocumentDataPropertyBagFunction()
		{

		}

		public AddressFormatDocumentDataPropertyBagFunction(string templateMergeKey, string firstNameSourceTemplateDataKey, string lastNameSourceTemplateDataKey, string companyNameSourceTemplateDataKey, string addressLine1SourceTemplateDataKey, string addressLine2SourceTemplateDataKey, string citySourceTemplateDataKey, string stateSourceTemplateDataKey, string postalCodeSourceTemplateDataKey, bool abbreviateState = false, bool forceToUpperCase = false)
		{
			TemplateMergeKey = templateMergeKey;
			FirstNameSourceTemplateDataKey = firstNameSourceTemplateDataKey;
			LastNameSourceTemplateDataKey = lastNameSourceTemplateDataKey;
			CompanyNameSourceTemplateDataKey = companyNameSourceTemplateDataKey;
			AddressLine1SourceTemplateDataKey = addressLine1SourceTemplateDataKey;
			AddressLine2SourceTemplateDataKey = addressLine2SourceTemplateDataKey;
			CitySourceTemplateDataKey = citySourceTemplateDataKey;
			StateSourceTemplateDataKey = stateSourceTemplateDataKey;
			AbbreviateState = abbreviateState;
			PostalCodeSourceTemplateDataKey = postalCodeSourceTemplateDataKey;
			ForceToUpperCase = forceToUpperCase;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys()
		{
			var sourceTemplateDataKeys = new List<string>();
			if (!string.IsNullOrWhiteSpace(FirstNameSourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(FirstNameSourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(LastNameSourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(LastNameSourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(CompanyNameSourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(CompanyNameSourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(AddressLine1SourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(AddressLine1SourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(AddressLine2SourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(AddressLine2SourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(CitySourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(CitySourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(StateSourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(StateSourceTemplateDataKey);
			}

			if (!string.IsNullOrWhiteSpace(PostalCodeSourceTemplateDataKey))
			{
				sourceTemplateDataKeys.Add(PostalCodeSourceTemplateDataKey);
			}

			return sourceTemplateDataKeys.ToArray();
		}

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
				var firstName = GetValue(values, FirstNameSourceTemplateDataKey);
				var lastName = GetValue(values, LastNameSourceTemplateDataKey);
				var companyName = GetValue(values, CompanyNameSourceTemplateDataKey);
				var addressLine1 = GetValue(values, AddressLine1SourceTemplateDataKey);
				var addressLine2 = GetValue(values, AddressLine2SourceTemplateDataKey);
				var city = GetValue(values, CitySourceTemplateDataKey);
				var state = GetValue(values, StateSourceTemplateDataKey);
				var postalCode = GetValue(values, PostalCodeSourceTemplateDataKey);

				if (AbbreviateState)
				{
					state = ISI.Extensions.Enum<ISI.Extensions.Address.State?>.Parse(state)?.GetAbbreviation() ?? state;
				}

				var formattedAddress = new StringBuilder();

				void addLine(string line)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						if (formattedAddress.Length > 0)
						{
							formattedAddress.AppendLine();
						}

						formattedAddress.Append(line);
					}
				}

				addLine($"{firstName} {lastName}".Trim());
				addLine(companyName);
				addLine(addressLine1);
				addLine(addressLine2);
				addLine(ISI.Extensions.Address.CityStateZipFormat(city, state, postalCode));

				if (formattedAddress.Length > 0)
				{
					if (ForceToUpperCase)
					{
						return new ISI.Extensions.Documents.DocumentDataValue(formattedAddress.ToString().ToUpper());
					}

					return new ISI.Extensions.Documents.DocumentDataValue(formattedAddress.ToString());
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new AddressFormatDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				FirstNameSourceTemplateDataKey = FirstNameSourceTemplateDataKey,
				LastNameSourceTemplateDataKey = LastNameSourceTemplateDataKey,
				CompanyNameSourceTemplateDataKey = CompanyNameSourceTemplateDataKey,
				AddressLine1SourceTemplateDataKey = AddressLine1SourceTemplateDataKey,
				AddressLine2SourceTemplateDataKey = AddressLine2SourceTemplateDataKey,
				CitySourceTemplateDataKey = CitySourceTemplateDataKey,
				StateSourceTemplateDataKey = StateSourceTemplateDataKey,
				AbbreviateState = AbbreviateState,
				PostalCodeSourceTemplateDataKey = PostalCodeSourceTemplateDataKey,
				ForceToUpperCase = ForceToUpperCase,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("4352b8d0-0ad8-4d22-8461-160f72bdb9d2")]
	[DataContract]
	public class AddressFormatDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new AddressFormatDocumentDataPropertyBagFunction(TemplateMergeKey, FirstNameSourceTemplateDataKey, LastNameSourceTemplateDataKey, CompanyNameSourceTemplateDataKey, AddressLine1SourceTemplateDataKey, AddressLine2SourceTemplateDataKey, CitySourceTemplateDataKey, StateSourceTemplateDataKey, PostalCodeSourceTemplateDataKey, AbbreviateState, ForceToUpperCase);

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "firstNameSourceTemplateDataKey", EmitDefaultValue = false)]
		public string FirstNameSourceTemplateDataKey { get; set; }

		[DataMember(Name = "lastNameSourceTemplateDataKey", EmitDefaultValue = false)]
		public string LastNameSourceTemplateDataKey { get; set; }

		[DataMember(Name = "companyNameSourceTemplateDataKey", EmitDefaultValue = false)]
		public string CompanyNameSourceTemplateDataKey { get; set; }

		[DataMember(Name = "addressLine1SourceTemplateDataKey", EmitDefaultValue = false)]
		public string AddressLine1SourceTemplateDataKey { get; set; }

		[DataMember(Name = "addressLine2SourceTemplateDataKey", EmitDefaultValue = false)]
		public string AddressLine2SourceTemplateDataKey { get; set; }

		[DataMember(Name = "citySourceTemplateDataKey", EmitDefaultValue = false)]
		public string CitySourceTemplateDataKey { get; set; }

		[DataMember(Name = "stateSourceTemplateDataKey", EmitDefaultValue = false)]
		public string StateSourceTemplateDataKey { get; set; }

		[DataMember(Name = "abbreviateState", EmitDefaultValue = false)]
		public bool AbbreviateState { get; set; }

		[DataMember(Name = "postalCodeSourceTemplateDataKey", EmitDefaultValue = false)]
		public string PostalCodeSourceTemplateDataKey { get; set; }

		[DataMember(Name = "forceToUpperCase", EmitDefaultValue = false)]
		public bool ForceToUpperCase { get; set; }
	}
}
