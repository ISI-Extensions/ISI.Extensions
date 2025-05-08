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

namespace ISI.Extensions.Barcodes
{
	public enum Symbology
	{
		[ISI.Extensions.EnumGuid("c0176a1c-df0f-49e3-bbaf-f3ec381a1739")] Codabar,
		[ISI.Extensions.EnumGuid("f160ea0d-10b2-4030-8852-4e0bc151f835")] Code11,
		[ISI.Extensions.EnumGuid("25fb2b6b-c724-45fb-bbf4-ef54062877c8")] Code128,
		[ISI.Extensions.EnumGuid("fe6d9251-9b01-4679-b315-f53fbc88e227")] Code39Extended,
		[ISI.Extensions.EnumGuid("d36ebc83-7caf-4a50-b243-275a4d34439a")] Code39Code39FullASCII,
		[ISI.Extensions.EnumGuid("b9f16ee0-4504-4f00-8bd4-a58ecdfa54c7")] Code93Extended,
		[ISI.Extensions.EnumGuid("c6a6c404-0d1a-474c-bd80-273b3d80c83f")] Interleaved2of5,
		[ISI.Extensions.EnumGuid("4e8cdb61-050e-41f2-871a-8951b1b82f1d")] Code39,
		[ISI.Extensions.EnumGuid("7af25a42-5168-44cb-a021-facf82063d0a")] Code39Standard,
		[ISI.Extensions.EnumGuid("75f08015-0ab5-4e18-b307-a3b5089c79cf")] Code93Standard,
		[ISI.Extensions.EnumGuid("5a36c033-3b2e-4f91-ab99-3c7b1acf5a63")] Code93,
		[ISI.Extensions.EnumGuid("3a017fd4-1c84-4634-a817-0136557c48a3")] MSI,
		[ISI.Extensions.EnumGuid("2dfb6f28-5299-4e73-af7e-205d4acc8d86")] Standard2of5,
		[ISI.Extensions.EnumGuid("67f0c660-9a20-47b4-ace9-971cb0694893")] DataMatrix,
		[ISI.Extensions.EnumGuid("1a7d3102-dcc9-45a2-8096-5615cd7da8e0")] GS1DataMatrix,
		[ISI.Extensions.EnumGuid("09ad08fd-3fdb-402b-85d2-4085e75fd2a0")] GS1Code128,
		[ISI.Extensions.EnumGuid("6e559346-7857-4ab9-9b73-94ab5d6b67cb")] EAN13,
		[ISI.Extensions.EnumGuid("b4297aa3-22f8-4b18-abf8-5634d5f0a544")] EAN8,
		[ISI.Extensions.EnumGuid("3f5da8d0-56c0-4b49-baea-c30b27fb47e3")] ITF14,
		[ISI.Extensions.EnumGuid("0fecc3d6-3de4-49f8-8c07-372fe2def537")] Pdf417,
		[ISI.Extensions.EnumGuid("ee5a74e3-a4f8-489f-b032-88795b050110")] Planet,
		[ISI.Extensions.EnumGuid("6fc76a1e-f16c-46ba-aefe-d233887cc4d4")] Postnet,
		[ISI.Extensions.EnumGuid("0be57563-088a-4c94-8b78-fbb37bc50108")] QR,
		[ISI.Extensions.EnumGuid("52f4ed4c-84e8-4035-881c-88fa49f9329b")] UPCA,
		[ISI.Extensions.EnumGuid("15cae463-7a00-437d-a10e-76d3c7326f76")] UPCE,
		[ISI.Extensions.EnumGuid("7ab2f5a0-9832-44cc-9b6a-9cac991bc541")] Aztec,
		[ISI.Extensions.EnumGuid("0c17e161-d9cf-4d3e-a436-4c77044f1456")] EAN14,
		[ISI.Extensions.EnumGuid("3ec42ee9-ee8d-4166-a4d7-661b9e03d169")] SSCC18,
		[ISI.Extensions.EnumGuid("de02e068-bae0-4880-862c-fe0672bf0355")] MacroPdf417,
		[ISI.Extensions.EnumGuid("12bc596e-2c82-428d-80ba-f1139114cf3b")] OneCode,
		[ISI.Extensions.EnumGuid("2de1c0e2-1120-4a39-a295-d98671c0e481")] AustraliaPost,
		[ISI.Extensions.EnumGuid("e38a9d98-6d6d-4055-8a83-132234c53adb")] RM4SCC,
		[ISI.Extensions.EnumGuid("87e16da1-c184-4ab3-85b0-18427a4ee588")] Matrix2of5,
		[ISI.Extensions.EnumGuid("386df339-11d0-44be-b0ee-5c76aedad88c")] DeutschePostIdentcode,
		[ISI.Extensions.EnumGuid("b9c56790-5e8b-4813-9f51-8ef9264af8e6")] PZN,
		[ISI.Extensions.EnumGuid("feaabdd6-f3c8-4960-b82c-a6053887124f")] ItalianPost25,
		[ISI.Extensions.EnumGuid("14b4032f-284b-46f7-bb4b-dff2ca2d4be3")] IATA2of5,
		[ISI.Extensions.EnumGuid("ecfef085-3b61-4ce4-b395-72652028ee85")] VIN,
		[ISI.Extensions.EnumGuid("f3263ec7-d61f-4cfd-8972-bf11f935b856")] DeutschePostLeitcode,
		[ISI.Extensions.EnumGuid("ca6d0d19-301e-4e9e-8fed-cad2a4158aa8")] OPC,
		[ISI.Extensions.EnumGuid("252218dc-487d-42ad-aa26-a104be67e0f2")] ITF6,
		[ISI.Extensions.EnumGuid("23690389-721f-4312-98f7-a5c6123d0184")] AustralianPosteParcel
	}
}
