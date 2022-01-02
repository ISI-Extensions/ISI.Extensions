#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
		public class HyperlinkCollection : ISI.Extensions.SpreadSheets.IHyperlinkCollection
		{
			internal readonly ISI.Extensions.Aspose.Cells.Worksheet _worksheet = null;
			internal readonly global::Aspose.Cells.HyperlinkCollection _hyperlinks = null;

			internal HyperlinkCollection(ISI.Extensions.Aspose.Cells.Worksheet worksheet, global::Aspose.Cells.HyperlinkCollection hyperlinks)
			{
				_worksheet = worksheet;
				_hyperlinks = hyperlinks;
			}

			public ISI.Extensions.SpreadSheets.IHyperlink this[int index] => new ISI.Extensions.Aspose.Cells.Hyperlink(this, _hyperlinks[index]);

			public int Add(string cellName, int totalRows, int totalColumns, string address)
			{
				return _hyperlinks.Add(cellName, totalRows, totalColumns, address);
			}

			public int Add(string startCellName, string endCellName, string address, string textToDisplay, string screenTip)
			{
				return _hyperlinks.Add(startCellName, endCellName, address, textToDisplay, screenTip);
			}

			public int Add(int firstRow, int firstColumn, int totalRows, int totalColumns, string address)
			{
				return _hyperlinks.Add(firstRow, firstColumn, totalRows, totalColumns, address);
			}

			public void Clear()
			{
				_hyperlinks.Clear();
			}

			public void RemoveAt(int index)
			{
				_hyperlinks.RemoveAt(index);
			}
		}
	}
}