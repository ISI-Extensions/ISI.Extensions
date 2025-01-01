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
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class WorksheetPicture : ISI.Extensions.SpreadSheets.IWorksheetPicture
		{
			internal readonly ISI.Extensions.SpreadSheets.IWorksheetPictureCollection _worksheetPictures = null;
			internal readonly global::Aspose.Cells.Drawing.Picture _picture = null;

			internal WorksheetPicture(ISI.Extensions.SpreadSheets.IWorksheetPictureCollection worksheetPictures, global::Aspose.Cells.Drawing.Picture picture)
			{
				_worksheetPictures = worksheetPictures;
				_picture = picture;
			}

			public void Move(int upperLeftRow, int upperLeftColumn) => _picture.Move(upperLeftRow, upperLeftColumn);
			
			public int OriginalHeight => _picture.OriginalHeight;
			public int OriginalWidth => _picture.OriginalWidth;

			public System.Drawing.Color BorderLineColor
			{
				get => _picture.BorderLineColor;
				set => _picture.BorderLineColor = value;
			}
			public double BorderWeight
			{
				get => _picture.BorderWeight;
				set => _picture.BorderWeight = value;
			}

			public byte[] Data
			{
				get => _picture.Data;
				set => _picture.Data = value;
			}

			public string SourceFullName
			{
				get => _picture.SourceFullName;
				set => _picture.SourceFullName = value;
			}

			public string Formula
			{
				get => _picture.Formula;
				set => _picture.Formula = value;
			}

			public bool IsAutoSize
			{
				get => _picture.IsAutoSize;
				set => _picture.IsAutoSize = value;
			}

			public bool IsLink 
			{
				get => _picture.IsLink;
				set => _picture.IsLink = value;
			}

			public bool IsDynamicDataExchange
			{
				get => _picture.IsDynamicDataExchange;
				set => _picture.IsDynamicDataExchange = value;
			}

			public bool DisplayAsIcon
			{
				get => _picture.DisplayAsIcon;
				set => _picture.DisplayAsIcon = value;
			}

			public ISI.Extensions.Documents.ImageType ImageType => _picture.ImageType.ToImageType();

			public double OriginalHeightCM => _picture.OriginalHeightCM;
			public double OriginalWidthCM => _picture.OriginalWidthCM;
			public double OriginalHeightInch => _picture.OriginalHeightInch;
			public double OriginalWidthInch => _picture.OriginalWidthInch;
		}
	}
}