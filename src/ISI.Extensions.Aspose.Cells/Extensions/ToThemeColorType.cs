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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.ThemeColorType ToThemeColorType(this ISI.Extensions.SpreadSheets.ThemeColorType themeColorType)
		{
			switch (themeColorType)
			{
				case ISI.Extensions.SpreadSheets.ThemeColorType.Background1: return global::Aspose.Cells.ThemeColorType.Background1;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Text1: return global::Aspose.Cells.ThemeColorType.Text1;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Background2: return global::Aspose.Cells.ThemeColorType.Background2;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Text2: return global::Aspose.Cells.ThemeColorType.Text2;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent1: return global::Aspose.Cells.ThemeColorType.Accent1;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent2: return global::Aspose.Cells.ThemeColorType.Accent2;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent3: return global::Aspose.Cells.ThemeColorType.Accent3;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent4: return global::Aspose.Cells.ThemeColorType.Accent4;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent5: return global::Aspose.Cells.ThemeColorType.Accent5;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Accent6: return global::Aspose.Cells.ThemeColorType.Accent6;
				case ISI.Extensions.SpreadSheets.ThemeColorType.Hyperlink: return global::Aspose.Cells.ThemeColorType.Hyperlink;
				case ISI.Extensions.SpreadSheets.ThemeColorType.FollowedHyperlink: return global::Aspose.Cells.ThemeColorType.FollowedHyperlink;
				case ISI.Extensions.SpreadSheets.ThemeColorType.StyleColor: return global::Aspose.Cells.ThemeColorType.StyleColor;
				default:
					throw new ArgumentOutOfRangeException(nameof(themeColorType), themeColorType, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.ThemeColorType ToThemeColorType(this global::Aspose.Cells.ThemeColorType themeColorType)
		{
			switch (themeColorType)
			{
				case global::Aspose.Cells.ThemeColorType.Background1: return ISI.Extensions.SpreadSheets.ThemeColorType.Background1;
				case global::Aspose.Cells.ThemeColorType.Text1: return ISI.Extensions.SpreadSheets.ThemeColorType.Text1;
				case global::Aspose.Cells.ThemeColorType.Background2: return ISI.Extensions.SpreadSheets.ThemeColorType.Background2;
				case global::Aspose.Cells.ThemeColorType.Text2: return ISI.Extensions.SpreadSheets.ThemeColorType.Text2;
				case global::Aspose.Cells.ThemeColorType.Accent1: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent1;
				case global::Aspose.Cells.ThemeColorType.Accent2: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent2;
				case global::Aspose.Cells.ThemeColorType.Accent3: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent3;
				case global::Aspose.Cells.ThemeColorType.Accent4: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent4;
				case global::Aspose.Cells.ThemeColorType.Accent5: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent5;
				case global::Aspose.Cells.ThemeColorType.Accent6: return ISI.Extensions.SpreadSheets.ThemeColorType.Accent6;
				case global::Aspose.Cells.ThemeColorType.Hyperlink: return ISI.Extensions.SpreadSheets.ThemeColorType.Hyperlink;
				case global::Aspose.Cells.ThemeColorType.FollowedHyperlink: return ISI.Extensions.SpreadSheets.ThemeColorType.FollowedHyperlink;
				case global::Aspose.Cells.ThemeColorType.StyleColor: return ISI.Extensions.SpreadSheets.ThemeColorType.StyleColor;
				default:
					throw new ArgumentOutOfRangeException(nameof(themeColorType), themeColorType, null);
			}
		}
	}
}
