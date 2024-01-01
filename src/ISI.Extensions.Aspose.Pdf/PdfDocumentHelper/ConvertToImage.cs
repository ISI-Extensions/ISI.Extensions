#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Aspose
{
	public partial class Pdf
	{
		public partial class PdfDocumentHelper
		{
			public System.IO.Stream[] ConvertToImage(System.IO.Stream pdfStream, ISI.Extensions.Documents.ImageType imageType, Func<System.IO.Stream> getOutputStream = null)
			{
				var outputStreams = new List<System.IO.Stream>();

				var resolution = new global::Aspose.Pdf.Devices.Resolution(300);

				if (imageType == ISI.Extensions.Documents.ImageType.Tiff)
				{
					var imageDevice = new global::Aspose.Pdf.Devices.TiffDevice(resolution);

					var pdfDocument = new global::Aspose.Pdf.Document(pdfStream);

					var outputStream = getOutputStream?.Invoke() ?? new ISI.Extensions.Stream.TempFileStream();

					imageDevice.Process(pdfDocument, outputStream);

					outputStreams.Add(outputStream);
				}
				else
				{
					global::Aspose.Pdf.Devices.ImageDevice imageDevice = null;

					switch (imageType)
					{
						case ISI.Extensions.Documents.ImageType.Emf:
							imageDevice = new global::Aspose.Pdf.Devices.EmfDevice(resolution);
							break;
						case ISI.Extensions.Documents.ImageType.Jpeg:
							imageDevice = new global::Aspose.Pdf.Devices.JpegDevice(resolution);
							break;
						case ISI.Extensions.Documents.ImageType.Png:
							imageDevice = new global::Aspose.Pdf.Devices.PngDevice(resolution);
							break;
						case ISI.Extensions.Documents.ImageType.Bmp:
							imageDevice = new global::Aspose.Pdf.Devices.BmpDevice(resolution);
							break;
						case ISI.Extensions.Documents.ImageType.Gif:
							imageDevice = new global::Aspose.Pdf.Devices.GifDevice(resolution);
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null);
					}

					var pdfDocument = new global::Aspose.Pdf.Document(pdfStream);

					foreach (var page in pdfDocument.Pages)
					{
						var outputStream = getOutputStream?.Invoke() ?? new ISI.Extensions.Stream.TempFileStream();

						imageDevice.Process(page, outputStream);

						outputStreams.Add(outputStream);
					}
				}

				return outputStreams.ToArray();
			}
		}
	}
}
