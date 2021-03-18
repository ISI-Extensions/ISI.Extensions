#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentImageData : ISI.Extensions.Documents.IDocumentImageData
		{
			internal global::Aspose.Words.Drawing.ImageData _imageData = null;

			public DocumentImageData(global::Aspose.Words.Drawing.ImageData imageData)
			{
				_imageData = imageData;
			}

			public void SetImage(System.IO.Stream stream)
			{
				_imageData.SetImage(stream);
			}

			public void SetImage(string fileName)
			{
				_imageData.SetImage(fileName);
			}

			public System.IO.Stream ToStream()
			{
				return _imageData.ToStream();
			}

			public byte[] ToByteArray()
			{
				return _imageData.ToByteArray();
			}

			public void Save(System.IO.Stream stream)
			{
				_imageData.Save(stream);
			}

			public void Save(string fileName)
			{
				_imageData.Save(fileName);
			}

			public byte[] ImageBytes
			{
				get => _imageData.ImageBytes;
				set => _imageData.ImageBytes = value;
			}

			public bool HasImage => _imageData.HasImage;
			public ISI.Extensions.Documents.IDocumentImageSize ImageSize => new DocumentImageSize(_imageData.ImageSize);
			public ISI.Extensions.Documents.ImageType ImageType => _imageData.ImageType.ToImageType();
			public bool IsLink => _imageData.IsLink;
			public bool IsLinkOnly => _imageData.IsLinkOnly;
			public string SourceFullName { get => _imageData.SourceFullName; set => _imageData.SourceFullName = value; }
			public string Title { get => _imageData.Title; set => _imageData.Title = value; }
			public double CropTop { get => _imageData.CropTop; set => _imageData.CropTop = value; }
			public double CropBottom { get => _imageData.CropBottom; set => _imageData.CropBottom = value; }
			public double CropLeft { get => _imageData.CropLeft; set => _imageData.CropLeft = value; }
			public double CropRight { get => _imageData.CropRight; set => _imageData.CropRight = value; }
			public ISI.Extensions.Documents.IDocumentBorderCollection Borders => new DocumentBorderCollection(_imageData.Borders);
			public System.Drawing.Color ChromaKey { get => _imageData.ChromaKey; set => _imageData.ChromaKey = value; }
			public double Brightness { get => _imageData.Brightness; set => _imageData.Brightness = value; }
			public double Contrast { get => _imageData.Contrast; set => _imageData.Contrast = value; }
			public bool BiLevel { get => _imageData.BiLevel; set => _imageData.BiLevel = value; }
			public bool GrayScale { get => _imageData.GrayScale; set => _imageData.GrayScale = value; }
		}
	}
}