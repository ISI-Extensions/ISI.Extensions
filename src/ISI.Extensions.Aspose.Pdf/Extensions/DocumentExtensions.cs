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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class DocumentExtensions
	{
		public static void ConvertColorSpaceToGreyScale(this global::Aspose.Pdf.Document document)
		{
			var strategy = new global::Aspose.Pdf.RgbToDeviceGrayConversionStrategy();

			for (var idxPage = 1; idxPage <= document.Pages.Count; idxPage++)
			{
				var page = document.Pages[idxPage];

				strategy.Convert(page);
			}

			foreach (var documentPage in document.Pages)
			{
				var imagePlacementAbsorber = new global::Aspose.Pdf.ImagePlacementAbsorber();
				documentPage.Accept(imagePlacementAbsorber);

				// Get the count of images over specific page
				Console.WriteLine("Total Images = {0} over page number {1}", imagePlacementAbsorber.ImagePlacements.Count, documentPage.Number);

				var image_counter = 1;
				foreach (var imagePlacement in imagePlacementAbsorber.ImagePlacements)
				{
					var colorType = imagePlacement.Image.GetColorType();
					switch (colorType)
					{
						case global::Aspose.Pdf.ColorType.Grayscale:
							Console.WriteLine("Image {0} is GrayScale...", image_counter);
							break;
						case global::Aspose.Pdf.ColorType.Rgb:
							Console.WriteLine("Image {0} is RGB...", image_counter);
							break;
					}
					image_counter += 1;
				}
			}
		}
	}
}
