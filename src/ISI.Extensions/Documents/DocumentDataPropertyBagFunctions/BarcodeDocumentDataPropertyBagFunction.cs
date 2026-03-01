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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Documents
{
	[ISI.Extensions.Documents.DocumentDataPropertyBagFunction]
	public class BarcodeDocumentDataPropertyBagFunction : IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys
	{
		private static System.ComponentModel.TypeConverter _fontTypeConverter = null;
		protected System.ComponentModel.TypeConverter FontTypeConverter => _fontTypeConverter ??= System.ComponentModel.TypeDescriptor.GetConverter(typeof(System.Drawing.Font));

		private static ISI.Extensions.Barcodes.IBarcodeGenerator _barCodeGenerator = null;
		protected ISI.Extensions.Barcodes.IBarcodeGenerator BarCodeGenerator => _barCodeGenerator ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Barcodes.IBarcodeGenerator>();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionTemplateMergeKey]
		public string TemplateMergeKey { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionSourceTemplateDataKey]
		public string SourceTemplateDataKey { get; set; }

		public ISI.Extensions.Images.ImageFormat ImageFormat => ISI.Extensions.Images.ImageFormat.Bmp;

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Font")]
		public System.Drawing.Font Font { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("Symbology")]
		public ISI.Extensions.Barcodes.Symbology Symbology { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("xDimension")]
		public ISI.Extensions.UnitOfMeasure.Distance xDimension { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BarWidthReduction")]
		public ISI.Extensions.UnitOfMeasure.Distance BarWidthReduction { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BarcodeHeight")]
		public ISI.Extensions.UnitOfMeasure.Distance BarcodeHeight { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BarcodeWidth")]
		public ISI.Extensions.UnitOfMeasure.Distance BarcodeWidth { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("LeftMargin")]
		public ISI.Extensions.UnitOfMeasure.Distance LeftMargin { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("RightMargin")]
		public ISI.Extensions.UnitOfMeasure.Distance RightMargin { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("TopMargin")]
		public ISI.Extensions.UnitOfMeasure.Distance TopMargin { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BottomMargin")]
		public ISI.Extensions.UnitOfMeasure.Distance BottomMargin { get; set; } = new();

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("BorderDashStyle")]
		public ISI.Extensions.Barcodes.BorderDashStyle BorderDashStyle { get; set; }

		[ISI.Extensions.Documents.DocumentDataPropertyBagFunctionStaticValue("CaptionLocation")]
		public ISI.Extensions.Barcodes.CaptionLocation CaptionLocation { get; set; }

		public BarcodeDocumentDataPropertyBagFunction()
		{

		}

		public BarcodeDocumentDataPropertyBagFunction(string templateMergeKey, string sourceTemplateDataKey,
			string font,
			ISI.Extensions.Barcodes.Symbology symbology, ISI.Extensions.UnitOfMeasure.Distance xDimension,
			ISI.Extensions.UnitOfMeasure.Distance barWidthReduction, ISI.Extensions.UnitOfMeasure.Distance barcodeHeight, ISI.Extensions.UnitOfMeasure.Distance barcodeWidth,
			ISI.Extensions.UnitOfMeasure.Distance leftMargin, ISI.Extensions.UnitOfMeasure.Distance rightMargin, ISI.Extensions.UnitOfMeasure.Distance topMargin, ISI.Extensions.UnitOfMeasure.Distance bottomMargin,
			ISI.Extensions.Barcodes.BorderDashStyle borderDashStyle, ISI.Extensions.Barcodes.CaptionLocation captionLocation)
		{
			TemplateMergeKey = templateMergeKey;
			SourceTemplateDataKey = sourceTemplateDataKey;
			Font = (System.Drawing.Font)FontTypeConverter.ConvertFromString(font);
			Symbology = symbology;
			this.xDimension = xDimension;
			BarWidthReduction = barWidthReduction;
			BarcodeHeight = barcodeHeight;
			BarcodeWidth = barcodeWidth;
			LeftMargin = leftMargin;
			RightMargin = rightMargin;
			TopMargin = topMargin;
			BottomMargin = bottomMargin;
			BorderDashStyle = borderDashStyle;
			CaptionLocation = captionLocation;
		}

		string[] IDocumentDataPropertyBagFunctionWithSourceTemplateDataKeys.GetSourceTemplateDataKeys() => [SourceTemplateDataKey];

		protected string GetValue(IDictionary<string, object> values)
		{
			if (values.TryGetValue(SourceTemplateDataKey, out var value) && (value != null))
			{
				return $"{value}";
			}

			return string.Empty;
		}

		public ISI.Extensions.Documents.DocumentDataPropertyBagFunctionDelegate GetDocumentDataPropertyBagFunction()
		{
			return values =>
			{
				var value = GetValue(values);

				if (!string.IsNullOrWhiteSpace(value))
				{
					using (var imageStream = new System.IO.MemoryStream())
					{
						BarCodeGenerator.GenerateBarcode(new ISI.Extensions.Barcodes.DataTransferObjects.BarcodeGenerator.GenerateBarcodeUsingSymbologyRequest()
						{
							Symbology = Symbology,
							Value = value,
							Font = Font,
							xDimension = xDimension,
							BarWidthReduction = BarWidthReduction,
							BarcodeHeight = BarcodeHeight,
							BarcodeWidth = BarcodeWidth,
							LeftMargin = LeftMargin,
							RightMargin = RightMargin,
							TopMargin = TopMargin,
							BottomMargin = BottomMargin,
							BorderDashStyle = BorderDashStyle,
							CaptionLocation = CaptionLocation,
							ImageStream = imageStream,
							ImageFormat = ImageFormat,
						});

						return new ISI.Extensions.Documents.DocumentDataImageValue(imageStream.ReadBytes(), true);
					}
				}

				return null;
			};
		}

		public ISI.Extensions.Documents.IDocumentDataPropertyBagFunctionDefinition GetPropertyBagFunctionDefinition()
		{
			return new DocumentDataPropertyBagFunctionDefinition(new BarcodeDocumentDataPropertyBagFunctionSerializableDefinitionV1()
			{
				TemplateMergeKey = TemplateMergeKey,
				SourceTemplateDataKey = SourceTemplateDataKey,
				Font = FontTypeConverter.ConvertToString(Font),
				Symbology = Symbology,
				xDimension = xDimension?.GetStringValue(),
				BarWidthReduction = BarWidthReduction?.GetStringValue(),
				BarcodeHeight = BarcodeHeight?.GetStringValue(),
				BarcodeWidth = BarcodeWidth?.GetStringValue(),
				LeftMargin = LeftMargin?.GetStringValue(),
				RightMargin = RightMargin?.GetStringValue(),
				TopMargin = TopMargin?.GetStringValue(),
				BottomMargin = BottomMargin?.GetStringValue(),
				BorderDashStyle = BorderDashStyle,
				CaptionLocation = CaptionLocation,
			});
		}
	}

	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("8eeca686-2288-4454-8ddc-6a41a0a387b1")]
	[DataContract]
	public class BarcodeDocumentDataPropertyBagFunctionSerializableDefinitionV1 : IDocumentDataPropertyBagFunctionSerializableDefinition
	{
		public IDocumentDataPropertyBagFunctionDefinition Export() => new DocumentDataPropertyBagFunctionDefinition(this);

		public virtual IDocumentDataPropertyBagFunction CreateDocumentDataPropertyBagFunction()
		{
			return new BarcodeDocumentDataPropertyBagFunction(TemplateMergeKey, SourceTemplateDataKey, Font, Symbology, xDimension, BarWidthReduction, BarcodeHeight, BarcodeWidth, LeftMargin, RightMargin, TopMargin, BottomMargin, BorderDashStyle, CaptionLocation);
		}

		[DataMember(Name = "templateMergeKey", EmitDefaultValue = false)]
		public string TemplateMergeKey { get; set; }

		[DataMember(Name = "sourceTemplateDataKey", EmitDefaultValue = false)]
		public string SourceTemplateDataKey { get; set; }

		[DataMember(Name = "font", EmitDefaultValue = false)]
		public string Font { get; set; }

		[DataMember(Name = "symbologyUuid", EmitDefaultValue = false)]
		public string __Symbology
		{
			get => Symbology.GetUuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
			set => Symbology = ISI.Extensions.Enum<ISI.Extensions.Barcodes.Symbology>.ParseUuid(value);
		}

		[IgnoreDataMember] public ISI.Extensions.Barcodes.Symbology Symbology { get; set; }

		[DataMember(Name = "xDimension", EmitDefaultValue = false)]
		public string xDimension { get; set; }

		[DataMember(Name = "BarWidthReduction", EmitDefaultValue = false)]
		public string BarWidthReduction { get; set; }

		[DataMember(Name = "barcodeHeight", EmitDefaultValue = false)]
		public string BarcodeHeight { get; set; }

		[DataMember(Name = "barcodeWidth", EmitDefaultValue = false)]
		public string BarcodeWidth { get; set; }

		[DataMember(Name = "leftMargin", EmitDefaultValue = false)]
		public string LeftMargin { get; set; }

		[DataMember(Name = "rightMargin", EmitDefaultValue = false)]
		public string RightMargin { get; set; }

		[DataMember(Name = "topMargin", EmitDefaultValue = false)]
		public string TopMargin { get; set; }

		[DataMember(Name = "bottomMargin", EmitDefaultValue = false)]
		public string BottomMargin { get; set; }

		[DataMember(Name = "borderDashStyleUuid", EmitDefaultValue = false)]
		public string __BorderDashStyle
		{
			get => BorderDashStyle.GetUuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
			set => BorderDashStyle = ISI.Extensions.Enum<ISI.Extensions.Barcodes.BorderDashStyle>.ParseUuid(value);
		}

		[IgnoreDataMember] public ISI.Extensions.Barcodes.BorderDashStyle BorderDashStyle { get; set; }

		[DataMember(Name = "captionLocationUuid", EmitDefaultValue = false)]
		public string __CaptionLocation
		{
			get => CaptionLocation.GetUuid().Formatted(GuidExtensions.GuidFormat.WithHyphens);
			set => CaptionLocation = ISI.Extensions.Enum<ISI.Extensions.Barcodes.CaptionLocation>.ParseUuid(value);
		}

		[IgnoreDataMember] public ISI.Extensions.Barcodes.CaptionLocation CaptionLocation { get; set; }
	}
}