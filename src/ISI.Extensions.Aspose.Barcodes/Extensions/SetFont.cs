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
using System.Text;

namespace ISI.Extensions.Aspose.Barcodes.Extensions
{
	public static partial class BarcodeExtensions
	{
		public static void SetFont(this global::Aspose.BarCode.Generation.FontUnit fontUnit, System.Drawing.Font font)
		{
			fontUnit.FamilyName = font.FontFamily.Name;
			fontUnit.Style = font.Style.ToFontStyle();

			switch (font.Unit)
			{
				case System.Drawing.GraphicsUnit.Display:
					break;
				case System.Drawing.GraphicsUnit.Pixel:
					fontUnit.Size.Pixels = font.Size;
					break;
				case System.Drawing.GraphicsUnit.Point:
					fontUnit.Size.Point = font.Size;
					break;
				case System.Drawing.GraphicsUnit.Inch:
					fontUnit.Size.Inches = font.Size;
					break;
				case System.Drawing.GraphicsUnit.Document:
					fontUnit.Size.Document = font.Size;
					break;
				case System.Drawing.GraphicsUnit.Millimeter:
					fontUnit.Size.Millimeters = font.Size;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
