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
		public partial class WorksheetPageSetup : ISI.Extensions.SpreadSheets.IWorksheetPageSetup
		{
			internal readonly ISI.Extensions.Aspose.Cells.Worksheet _worksheet = null;
			internal readonly global::Aspose.Cells.PageSetup _pageSetup = null;

			internal WorksheetPageSetup(ISI.Extensions.Aspose.Cells.Worksheet worksheet, global::Aspose.Cells.PageSetup pageSetup)
			{
				_worksheet = worksheet;
				_pageSetup = pageSetup;
			}

			public double TopMarginInch
			{
				get => _pageSetup.TopMarginInch;
				set => _pageSetup.TopMarginInch = value;
			}

			public double RightMarginInch
			{
				get => _pageSetup.RightMarginInch;
				set => _pageSetup.RightMarginInch = value;
			}

			public double BottomMarginInch
			{
				get => _pageSetup.BottomMarginInch;
				set => _pageSetup.BottomMarginInch = value;
			}

			public double LeftMarginInch
			{
				get => _pageSetup.LeftMarginInch;
				set => _pageSetup.LeftMarginInch = value;
			}

			public string PrintArea
			{
				get => _pageSetup.PrintArea;
				set => _pageSetup.PrintArea = value;
			}


			public ISI.Extensions.Documents.PageOrientation PageOrientation
			{
				get => _pageSetup.Orientation.ToPageOrientation();
				set => _pageSetup.Orientation = value.ToPageOrientation();
			}

			public void SetHeader(ISI.Extensions.SpreadSheets.HeaderSection section, string header)
			{
				_pageSetup.SetHeader(section.ToSection(), header);
			}

			public void SetFooter(ISI.Extensions.SpreadSheets.FooterSection section, string footer)
			{
				_pageSetup.SetFooter(section.ToSection(), footer);
			}
		}
	}
}
