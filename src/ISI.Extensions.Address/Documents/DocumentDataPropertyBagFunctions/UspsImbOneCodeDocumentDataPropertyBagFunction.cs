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
	public class UspsImbOneCodeDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		public interface INextUspsSerialNumberHelper
		{
			int GetNextUspsSerialNumber(int mailerId, int barCodeId, int specialServices, string postalCode);
			void SetUspsSerialNumberEncodedValue(int mailerId, int barCodeId, int specialServices, string postalCode, int uspsSerialNumber, string encodedValue);
		}

		private static ISI.Extensions.Barcodes.IBarcodeGenerator _barCodeGenerator = null;
		protected ISI.Extensions.Barcodes.IBarcodeGenerator BarCodeGenerator => _barCodeGenerator ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Barcodes.IBarcodeGenerator>();

		private INextUspsSerialNumberHelper _nextUspsSerialNumberHelper = null;
		protected INextUspsSerialNumberHelper NextUspsSerialNumberHelper => _nextUspsSerialNumberHelper ??= ISI.Extensions.ServiceLocator.Current.GetService(Type.GetType(NextUspsSerialNumberType)) as INextUspsSerialNumberHelper;

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		public ISI.Extensions.Images.ImageFormat ImageFormat => ISI.Extensions.Images.ImageFormat.Bmp;

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Mailer ID")]
		public int MailerId { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BarCodeId")]
		public int BarCodeId { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Special Services")]
		public int SpecialServices { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("NextUspsSerialNumberType")]
		public string NextUspsSerialNumberType { get; set; }

		public UspsImbOneCodeDocumentDataPropertyBagFunction()
		{

		}

		public UspsImbOneCodeDocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey, int mailerId, int barcodeId, int specialServices, string nextUspsSerialNumberType)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
			MailerId = mailerId;
			BarCodeId = barcodeId;
			SpecialServices = specialServices;
			NextUspsSerialNumberType = nextUspsSerialNumberType;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => [SourceTemplateDataKey];

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				if (values.TryGetValue(SourceTemplateDataKey, out var value) && (value != null))
				{
					var postalCode = ISI.Extensions.Address.ZipFormat($"{value}", ISI.Extensions.Address.ZipStyle.ZipPlus4WithDashTruncateEmptyZip4);

					if (!string.IsNullOrWhiteSpace(postalCode))
					{
						var sequenceNumber = NextUspsSerialNumberHelper.GetNextUspsSerialNumber(MailerId, BarCodeId, SpecialServices, postalCode);

						var uspsImbOneCode = new ISI.Extensions.Address.UspsImbOneCode()
						{
							MailerId = MailerId,
							BarCodeId = BarCodeId,
							SpecialServices = SpecialServices,
							SequenceNumber = sequenceNumber,
							ZipCode = postalCode,
						};

						NextUspsSerialNumberHelper.SetUspsSerialNumberEncodedValue(MailerId, BarCodeId, SpecialServices, postalCode, sequenceNumber, uspsImbOneCode.GetEncoded());

						using (var imageStream = new System.IO.MemoryStream())
						{
							BarCodeGenerator.GenerateBarcode(new ISI.Extensions.Barcodes.DataTransferObjects.BarcodeGenerator.GenerateBarcodeUsingSymbologyRequest()
							{
								Symbology = ISI.Extensions.Barcodes.Symbology.OneCode,
								Value = uspsImbOneCode.Value,
								ImageStream = imageStream,
							});

							return new ISI.Extensions.Documents.DocumentDataImageValue(imageStream.ReadBytes(), true);
						}
					}
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new UspsImbOneCodeDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
				MailerId = MailerId,
				BarCodeId = BarCodeId,
				SpecialServices = SpecialServices,
				NextUspsSerialNumberType = NextUspsSerialNumberType,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("36acb5b8-5a3a-4e9a-ab11-0902411e362d")]
	[DataContract]
	public class UspsImbOneCodeDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);
		public IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction() => new UspsImbOneCodeDocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey, MailerId, BarCodeId, SpecialServices, NextUspsSerialNumberType);

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }

		[DataMember(Name = "mailerId", EmitDefaultValue = false)]
		public int MailerId { get; set; }

		[DataMember(Name = "barCodeId", EmitDefaultValue = false)]
		public int BarCodeId { get; set; }

		[DataMember(Name = "specialServices", EmitDefaultValue = false)]
		public int SpecialServices { get; set; }

		[DataMember(Name = "nextUspsSerialNumberType", EmitDefaultValue = false)]
		public string NextUspsSerialNumberType { get; set; }
	}
}