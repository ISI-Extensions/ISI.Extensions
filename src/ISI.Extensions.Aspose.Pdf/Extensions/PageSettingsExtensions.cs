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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class PageSettingsExtensions
	{
		public static global::Aspose.Pdf.Printing.PageSettings ToPageSettings(this System.Drawing.Printing.PageSettings pageSettings, System.Drawing.Printing.PrinterSettings printerSettings)
		{
			if (pageSettings == null)
			{
				return null;
			}

			return new global::Aspose.Pdf.Printing.PageSettings(printerSettings.ToPrinterSettings())
			{
				Color = pageSettings.Color,
				Landscape = pageSettings.Landscape,
				Margins = pageSettings.Margins.NullCheckedConvert(margins => new global::Aspose.Pdf.Devices.Margins(margins.Left, margins.Right, margins.Top, margins.Bottom)),
				PaperSize = pageSettings.PaperSize.NullCheckedConvert(paperSize => new global::Aspose.Pdf.Printing.PaperSize(paperSize.PaperName, paperSize.Width, paperSize.Height)),
				PaperSource = pageSettings.PaperSource.NullCheckedConvert(paperSource => new global::Aspose.Pdf.Printing.PaperSource()
				{
					SourceName = paperSource.SourceName,
				}),
				PrinterResolution = pageSettings.PrinterResolution.NullCheckedConvert(printerResolution => new global::Aspose.Pdf.Printing.PrinterResolution()
				{
					Kind = printerResolution.Kind.ToPrinterResolutionKind(),
					X = printerResolution.X,
					Y = printerResolution.Y,
				}),
			};
		}
	}
}
