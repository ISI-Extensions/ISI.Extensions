#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public class WorksheetPictureCollection : ISI.Extensions.SpreadSheets.IWorksheetPictureCollection
		{
			internal readonly ISI.Extensions.SpreadSheets.IWorksheet _worksheet = null;
			internal readonly global::Aspose.Cells.Drawing.PictureCollection _pictures = null;

			internal WorksheetPictureCollection(ISI.Extensions.SpreadSheets.IWorksheet worksheet, global::Aspose.Cells.Drawing.PictureCollection pictures)
			{
				_worksheet = worksheet;
				_pictures = pictures;
			}

			public int Add(int upperLeftRow, int upperLeftColumn, int lowerRightRow, int lowerRightColumn, System.IO.Stream stream) => _pictures.Add(upperLeftRow, upperLeftColumn, lowerRightRow, lowerRightColumn, stream);
			public int Add(int upperLeftRow, int upperLeftColumn, int lowerRightRow, int lowerRightColumn, string fileName) => _pictures.Add(upperLeftRow, upperLeftColumn, lowerRightRow, lowerRightColumn, fileName);
			public int Add(int upperLeftRow, int upperLeftColumn, System.IO.Stream stream) => _pictures.Add(upperLeftRow, upperLeftColumn, stream);
			public int Add(int upperLeftRow, int upperLeftColumn, string fileName) => _pictures.Add(upperLeftRow, upperLeftColumn, fileName);
			public int Add(int upperLeftRow, int upperLeftColumn, System.IO.Stream stream, int widthScale, int heightScale) => _pictures.Add(upperLeftRow, upperLeftColumn, stream, widthScale, heightScale);
			public int Add(int upperLeftRow, int upperLeftColumn, string fileName, int widthScale, int heightScale) => _pictures.Add(upperLeftRow, upperLeftColumn, fileName, widthScale, heightScale);
			public ISI.Extensions.SpreadSheets.IWorksheetPicture this[int index] => new WorksheetPicture(this, _pictures[index]);
			public void Clear() => _pictures.Clear();
			public void RemoveAt(int index) => _pictures.RemoveAt(index);
		}
	}
}