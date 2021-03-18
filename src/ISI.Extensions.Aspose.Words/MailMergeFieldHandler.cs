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
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class MailMergeFieldHandler : global::Aspose.Words.MailMerging.IFieldMergingCallback
		{
			public void FieldMerging(global::Aspose.Words.MailMerging.FieldMergingArgs args)
			{
				if (args.FieldValue is ISI.Extensions.Documents.IDocumentDataHtmlValue htmlValue)
				{
					var documentBuilder = new global::Aspose.Words.DocumentBuilder(args.Document);

					documentBuilder.MoveToMergeField(args.DocumentFieldName);
					documentBuilder.InsertHtml(htmlValue.Html);
				}
			}

			public void ImageFieldMerging(global::Aspose.Words.MailMerging.ImageFieldMergingArgs args)
			{
				if (args.FieldValue is ISI.Extensions.Documents.IDocumentDataImageValue imageValue)
				{
					var documentBuilder = new global::Aspose.Words.DocumentBuilder(args.Document);

					documentBuilder.MoveToMergeField(args.DocumentFieldName);

					if (imageValue.ImageWidthInPixels.HasValue && imageValue.ImageHeightInPixels.HasValue)
					{
						var leftInPoints = global::Aspose.Words.ConvertUtil.PixelToPoint(imageValue.LeftInPixels);
						var topInPoints = global::Aspose.Words.ConvertUtil.PixelToPoint(imageValue.TopInPixels);
						var widthInPoints = global::Aspose.Words.ConvertUtil.PixelToPoint(imageValue.ImageWidthInPixels.Value);
						var heightInPoints = global::Aspose.Words.ConvertUtil.PixelToPoint(imageValue.ImageHeightInPixels.Value);

						if (imageValue.UsePositioning)
						{
							documentBuilder.InsertImage(new System.IO.MemoryStream(imageValue.Image), imageValue.RelativeHorizontalPosition.ToRelativeHorizontalPosition(), leftInPoints, imageValue.RelativeVerticalPosition.ToRelativeVerticalPosition(), topInPoints, widthInPoints, heightInPoints, imageValue.WrapType.ToWrapType());
						}
						else
						{
							documentBuilder.InsertImage(new System.IO.MemoryStream(imageValue.Image), widthInPoints, heightInPoints);
						}
					}
					else
					{
						documentBuilder.InsertImage(imageValue.Image);
					}
				}

				if (args.FieldValue is byte[] bytesValue)
				{
					args.ImageStream = new System.IO.MemoryStream(bytesValue);
				}
			}
		}
	}
}