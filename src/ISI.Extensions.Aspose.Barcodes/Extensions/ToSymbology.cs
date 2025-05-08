#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions.Aspose.Barcodes.Extensions
{
	public static partial class BarcodeExtensions
	{
		public static global::Aspose.BarCode.Generation.BaseEncodeType ToSymbology(this ISI.Extensions.Barcodes.Symbology symbology)
		{
			switch (symbology)
			{
				case ISI.Extensions.Barcodes.Symbology.Codabar:
					return global::Aspose.BarCode.Generation.EncodeTypes.Codabar;
				case ISI.Extensions.Barcodes.Symbology.Code11:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code11;
				case ISI.Extensions.Barcodes.Symbology.Code128:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code128;
				case ISI.Extensions.Barcodes.Symbology.Code39Extended:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code39FullASCII;
				case ISI.Extensions.Barcodes.Symbology.Code93Extended:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code93;
				case ISI.Extensions.Barcodes.Symbology.Interleaved2of5:
					return global::Aspose.BarCode.Generation.EncodeTypes.Interleaved2of5;
				case ISI.Extensions.Barcodes.Symbology.Code39Standard:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code39;
				case ISI.Extensions.Barcodes.Symbology.Code93Standard:
					return global::Aspose.BarCode.Generation.EncodeTypes.Code93;
				case ISI.Extensions.Barcodes.Symbology.MSI:
					return global::Aspose.BarCode.Generation.EncodeTypes.MSI;
				case ISI.Extensions.Barcodes.Symbology.Standard2of5:
					return global::Aspose.BarCode.Generation.EncodeTypes.Standard2of5;
				case ISI.Extensions.Barcodes.Symbology.DataMatrix:
					return global::Aspose.BarCode.Generation.EncodeTypes.DataMatrix;
				case ISI.Extensions.Barcodes.Symbology.GS1DataMatrix:
					return global::Aspose.BarCode.Generation.EncodeTypes.GS1DataMatrix;
				case ISI.Extensions.Barcodes.Symbology.GS1Code128:
					return global::Aspose.BarCode.Generation.EncodeTypes.GS1Code128;
				case ISI.Extensions.Barcodes.Symbology.EAN13:
					return global::Aspose.BarCode.Generation.EncodeTypes.EAN13;
				case ISI.Extensions.Barcodes.Symbology.EAN8:
					return global::Aspose.BarCode.Generation.EncodeTypes.EAN8;
				case ISI.Extensions.Barcodes.Symbology.ITF14:
					return global::Aspose.BarCode.Generation.EncodeTypes.ITF14;
				case ISI.Extensions.Barcodes.Symbology.Pdf417:
					return global::Aspose.BarCode.Generation.EncodeTypes.Pdf417;
				case ISI.Extensions.Barcodes.Symbology.Planet:
					return global::Aspose.BarCode.Generation.EncodeTypes.Planet;
				case ISI.Extensions.Barcodes.Symbology.Postnet:
					return global::Aspose.BarCode.Generation.EncodeTypes.Postnet;
				case ISI.Extensions.Barcodes.Symbology.QR:
					return global::Aspose.BarCode.Generation.EncodeTypes.QR;
				case ISI.Extensions.Barcodes.Symbology.UPCA:
					return global::Aspose.BarCode.Generation.EncodeTypes.UPCA;
				case ISI.Extensions.Barcodes.Symbology.UPCE:
					return global::Aspose.BarCode.Generation.EncodeTypes.UPCE;
				case ISI.Extensions.Barcodes.Symbology.Aztec:
					return global::Aspose.BarCode.Generation.EncodeTypes.Aztec;
				case ISI.Extensions.Barcodes.Symbology.EAN14:
					return global::Aspose.BarCode.Generation.EncodeTypes.EAN14;
				case ISI.Extensions.Barcodes.Symbology.SSCC18:
					return global::Aspose.BarCode.Generation.EncodeTypes.SSCC18;
				case ISI.Extensions.Barcodes.Symbology.MacroPdf417:
					return global::Aspose.BarCode.Generation.EncodeTypes.MacroPdf417;
				case ISI.Extensions.Barcodes.Symbology.OneCode:
					return global::Aspose.BarCode.Generation.EncodeTypes.OneCode;
				case ISI.Extensions.Barcodes.Symbology.AustraliaPost:
					return global::Aspose.BarCode.Generation.EncodeTypes.AustraliaPost;
				case ISI.Extensions.Barcodes.Symbology.RM4SCC:
					return global::Aspose.BarCode.Generation.EncodeTypes.RM4SCC;
				case ISI.Extensions.Barcodes.Symbology.Matrix2of5:
					return global::Aspose.BarCode.Generation.EncodeTypes.Matrix2of5;
				case ISI.Extensions.Barcodes.Symbology.DeutschePostIdentcode:
					return global::Aspose.BarCode.Generation.EncodeTypes.DeutschePostIdentcode;
				case ISI.Extensions.Barcodes.Symbology.PZN:
					return global::Aspose.BarCode.Generation.EncodeTypes.PZN;
				case ISI.Extensions.Barcodes.Symbology.ItalianPost25:
					return global::Aspose.BarCode.Generation.EncodeTypes.ItalianPost25;
				case ISI.Extensions.Barcodes.Symbology.IATA2of5:
					return global::Aspose.BarCode.Generation.EncodeTypes.IATA2of5;
				case ISI.Extensions.Barcodes.Symbology.VIN:
					return global::Aspose.BarCode.Generation.EncodeTypes.VIN;
				case ISI.Extensions.Barcodes.Symbology.DeutschePostLeitcode:
					return global::Aspose.BarCode.Generation.EncodeTypes.DeutschePostLeitcode;
				case ISI.Extensions.Barcodes.Symbology.OPC:
					return global::Aspose.BarCode.Generation.EncodeTypes.OPC;
				case ISI.Extensions.Barcodes.Symbology.ITF6:
					return global::Aspose.BarCode.Generation.EncodeTypes.ITF6;
				case ISI.Extensions.Barcodes.Symbology.AustralianPosteParcel:
					return global::Aspose.BarCode.Generation.EncodeTypes.AustralianPosteParcel;
				default:
					throw new ArgumentOutOfRangeException(nameof(symbology));
			}
		}
	}
}