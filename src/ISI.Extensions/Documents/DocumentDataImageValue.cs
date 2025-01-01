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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Documents
{
	public class DocumentDataImageValue : ISI.Extensions.Documents.IDocumentDataImageValue
	{
		public byte[] Image { get; }

		private bool _usePositioning = false;
		public bool UsePositioning
		{
			get => _usePositioning;
			set
			{
				if (!ImageWidthInPixels.HasValue || !ImageHeightInPixels.HasValue)
				{
					CalculateImageSize();
				}
				_usePositioning = true;
			}
		}

		public ISI.Extensions.Documents.RelativeHorizontalPosition RelativeHorizontalPosition { get; set; } = RelativeHorizontalPosition.Default;
		public int LeftInPixels { get; set; } = 0;
		public ISI.Extensions.Documents.RelativeVerticalPosition RelativeVerticalPosition { get; set; } = RelativeVerticalPosition.Line;
		public int TopInPixels { get; set; } = 0;

		public ISI.Extensions.Documents.WrapType WrapType { get; set; } = WrapType.None;

		public int? ImageWidthInPixels { get; private set; }
		public int? ImageHeightInPixels { get; private set; }

		bool IDocumentDataValue.HasValue => (Image != null);
		object IDocumentDataValue.Value => Image;

		public DocumentDataImageValue(byte[] image, bool calculateImageSize = false)
		{
			Image = image;

			if (calculateImageSize)
			{
				CalculateImageSize();
			}
		}

		public DocumentDataImageValue(byte[] image, int? imageWidthPixels, int? imageHeightPixels)
		{
			Image = image;
			ImageWidthInPixels = imageWidthPixels;
			ImageHeightInPixels = imageHeightPixels;
		}

		private void CalculateImageSize()
		{
			throw new NotImplementedException();
			//using (var imageWrapper = System.Drawing.Image.FromStream(new System.IO.MemoryStream(Image)))
			//{
			//	ImageWidthInPixels = imageWrapper.Width;
			//	ImageHeightInPixels = imageWrapper.Height;
			//}
		}
	}
}
