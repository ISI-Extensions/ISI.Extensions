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
using ISI.Extensions.Aspose.Barcodes.Extensions;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Barcodes.DataTransferObjects.BarcodeGenerator;

namespace ISI.Extensions.Aspose.Barcodes
{
	public partial class BarcodeGenerator
	{
		public DTOs.GenerateBarcodeResponse GenerateBarcode(DTOs.IGenerateBarcodeRequest request)
		{
			var response = new DTOs.GenerateBarcodeResponse();

			switch (request)
			{
				case DTOs.GenerateBarcodeUsingSymbologyRequest generateBarcodeUsingSymbologyRequest:
					if (generateBarcodeUsingSymbologyRequest.Symbology == ISI.Extensions.Barcodes.Symbology.OneCode)
					{
						using (var generator = new global::Aspose.BarCode.Generation.BarcodeGenerator(global::Aspose.BarCode.Generation.EncodeTypes.OneCode, generateBarcodeUsingSymbologyRequest.Value))
						{
							generator.Parameters.Barcode.XDimension.Inches = .02f;
							generator.Parameters.Barcode.BarHeight.Pixels = 15f;
							generator.Parameters.Barcode.Padding.Top.Pixels = 5;
							generator.Parameters.Barcode.Padding.Right.Pixels = 5;
							generator.Parameters.Barcode.Padding.Bottom.Pixels = 5;
							generator.Parameters.Barcode.Padding.Left.Pixels = 5;

							generator.Parameters.Barcode.CodeTextParameters.Location = global::Aspose.BarCode.Generation.CodeLocation.None;

							generator.Save(generateBarcodeUsingSymbologyRequest.ImageStream, generateBarcodeUsingSymbologyRequest.ImageFormat.ToBarcodeImageFormat());
						}
					}
					else
					{
						using (var generator = new global::Aspose.BarCode.Generation.BarcodeGenerator(generateBarcodeUsingSymbologyRequest.Symbology.ToSymbology(), generateBarcodeUsingSymbologyRequest.Value))
						{
							generator.Parameters.Barcode.Padding.Left.SetUnit(generateBarcodeUsingSymbologyRequest.LeftMargin);
							generator.Parameters.Barcode.Padding.Right.SetUnit(generateBarcodeUsingSymbologyRequest.RightMargin);
							generator.Parameters.Barcode.Padding.Top.SetUnit(generateBarcodeUsingSymbologyRequest.TopMargin);
							generator.Parameters.Barcode.Padding.Bottom.SetUnit(generateBarcodeUsingSymbologyRequest.BottomMargin);

							if (generateBarcodeUsingSymbologyRequest.Font != null)
							{
								generator.Parameters.CaptionAbove.Font.SetFont(generateBarcodeUsingSymbologyRequest.Font);
								generator.Parameters.CaptionBelow.Font.SetFont(generateBarcodeUsingSymbologyRequest.Font);
							}

							generator.Parameters.AutoSizeMode = global::Aspose.BarCode.Generation.AutoSizeMode.None;

							if (generateBarcodeUsingSymbologyRequest.xDimension.HasValue())
							{
								generator.Parameters.Barcode.XDimension.SetUnit(generateBarcodeUsingSymbologyRequest.xDimension);
							}

							if (generateBarcodeUsingSymbologyRequest.BarWidthReduction.HasValue())
							{
								generator.Parameters.Barcode.BarWidthReduction.SetUnit(generateBarcodeUsingSymbologyRequest.BarWidthReduction);
							}

							if (generateBarcodeUsingSymbologyRequest.BarcodeHeight.HasValue())
							{
								generator.Parameters.AutoSizeMode = global::Aspose.BarCode.Generation.AutoSizeMode.Nearest;
								generator.Parameters.ImageHeight.SetUnit(generateBarcodeUsingSymbologyRequest.BarcodeHeight);
							}

							if (generateBarcodeUsingSymbologyRequest.BarcodeWidth.HasValue())
							{
								generator.Parameters.AutoSizeMode = global::Aspose.BarCode.Generation.AutoSizeMode.Nearest;
								generator.Parameters.ImageWidth.SetUnit(generateBarcodeUsingSymbologyRequest.BarcodeWidth);
							}

							generator.Parameters.Barcode.CodeTextParameters.Location = generateBarcodeUsingSymbologyRequest.CaptionLocation.ToCodeLocation();

							if (generateBarcodeUsingSymbologyRequest.BorderDashStyle == ISI.Extensions.Barcodes.BorderDashStyle.None)
							{
								generator.Parameters.Border.Visible = false;
							}
							else
							{
								generator.Parameters.Border.Visible = true;
								generator.Parameters.Border.DashStyle = generateBarcodeUsingSymbologyRequest.BorderDashStyle.ToBorderDashStyle();
							}

							generator.Save(generateBarcodeUsingSymbologyRequest.ImageStream, generateBarcodeUsingSymbologyRequest.ImageFormat.ToBarcodeImageFormat());
						}
					}
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			return response;
		}
	}
}