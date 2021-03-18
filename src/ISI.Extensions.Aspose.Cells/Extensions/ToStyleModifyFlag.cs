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
		public static global::Aspose.Cells.StyleModifyFlag ToStyleModifyFlag(this ISI.Extensions.SpreadSheets.StyleModifyFlag styleModifyFlag)
		{
			switch (styleModifyFlag)
			{
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Borders: return global::Aspose.Cells.StyleModifyFlag.Borders;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.LeftBorder: return global::Aspose.Cells.StyleModifyFlag.LeftBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.RightBorder: return global::Aspose.Cells.StyleModifyFlag.RightBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.TopBorder: return global::Aspose.Cells.StyleModifyFlag.TopBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.BottomBorder: return global::Aspose.Cells.StyleModifyFlag.BottomBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.HorizontalBorder: return global::Aspose.Cells.StyleModifyFlag.HorizontalBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.VerticalBorder: return global::Aspose.Cells.StyleModifyFlag.VerticalBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Diagonal: return global::Aspose.Cells.StyleModifyFlag.Diagonal;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.DiagonalDownBorder: return global::Aspose.Cells.StyleModifyFlag.DiagonalDownBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.DiagonalUpBorder: return global::Aspose.Cells.StyleModifyFlag.DiagonalUpBorder;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Font: return global::Aspose.Cells.StyleModifyFlag.Font;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSize: return global::Aspose.Cells.StyleModifyFlag.FontSize;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontName: return global::Aspose.Cells.StyleModifyFlag.FontName;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontFamily: return global::Aspose.Cells.StyleModifyFlag.FontFamily;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontCharset: return global::Aspose.Cells.StyleModifyFlag.FontCharset;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontColor: return global::Aspose.Cells.StyleModifyFlag.FontColor;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontWeight: return global::Aspose.Cells.StyleModifyFlag.FontWeight;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontItalic: return global::Aspose.Cells.StyleModifyFlag.FontItalic;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontUnderline: return global::Aspose.Cells.StyleModifyFlag.FontUnderline;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontStrike: return global::Aspose.Cells.StyleModifyFlag.FontStrike;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontScript: return global::Aspose.Cells.StyleModifyFlag.FontScript;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.NumberFormat: return global::Aspose.Cells.StyleModifyFlag.NumberFormat;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.HorizontalAlignment: return global::Aspose.Cells.StyleModifyFlag.HorizontalAlignment;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.VerticalAlignment: return global::Aspose.Cells.StyleModifyFlag.VerticalAlignment;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Indent: return global::Aspose.Cells.StyleModifyFlag.Indent;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Rotation: return global::Aspose.Cells.StyleModifyFlag.Rotation;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.WrapText: return global::Aspose.Cells.StyleModifyFlag.WrapText;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.ShrinkToFit: return global::Aspose.Cells.StyleModifyFlag.ShrinkToFit;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.TextDirection: return global::Aspose.Cells.StyleModifyFlag.TextDirection;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.CellShading: return global::Aspose.Cells.StyleModifyFlag.CellShading;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Pattern: return global::Aspose.Cells.StyleModifyFlag.Pattern;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.ForegroundColor: return global::Aspose.Cells.StyleModifyFlag.ForegroundColor;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.BackgroundColor: return global::Aspose.Cells.StyleModifyFlag.BackgroundColor;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.Locked: return global::Aspose.Cells.StyleModifyFlag.Locked;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.HideFormula: return global::Aspose.Cells.StyleModifyFlag.HideFormula;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.AlignmentSettings: return global::Aspose.Cells.StyleModifyFlag.AlignmentSettings;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontScheme: return global::Aspose.Cells.StyleModifyFlag.FontScheme;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontDirty: return global::Aspose.Cells.StyleModifyFlag.FontDirty;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSpellingError: return global::Aspose.Cells.StyleModifyFlag.FontSpellingError;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontUFillTx: return global::Aspose.Cells.StyleModifyFlag.FontUFillTx;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSpacing: return global::Aspose.Cells.StyleModifyFlag.FontSpacing;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontKerning: return global::Aspose.Cells.StyleModifyFlag.FontKerning;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontEqualize: return global::Aspose.Cells.StyleModifyFlag.FontEqualize;
				case ISI.Extensions.SpreadSheets.StyleModifyFlag.FontCap: return global::Aspose.Cells.StyleModifyFlag.FontCap;
			}

			return global::Aspose.Cells.StyleModifyFlag.All;
		}

		public static ISI.Extensions.SpreadSheets.StyleModifyFlag ToStyleModifyFlag(this global::Aspose.Cells.StyleModifyFlag styleModifyFlag)
		{
			switch (styleModifyFlag)
			{
				case global::Aspose.Cells.StyleModifyFlag.Borders: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Borders;
				case global::Aspose.Cells.StyleModifyFlag.LeftBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.LeftBorder;
				case global::Aspose.Cells.StyleModifyFlag.RightBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.RightBorder;
				case global::Aspose.Cells.StyleModifyFlag.TopBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.TopBorder;
				case global::Aspose.Cells.StyleModifyFlag.BottomBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.BottomBorder;
				case global::Aspose.Cells.StyleModifyFlag.HorizontalBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.HorizontalBorder;
				case global::Aspose.Cells.StyleModifyFlag.VerticalBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.VerticalBorder;
				case global::Aspose.Cells.StyleModifyFlag.Diagonal: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Diagonal;
				case global::Aspose.Cells.StyleModifyFlag.DiagonalDownBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.DiagonalDownBorder;
				case global::Aspose.Cells.StyleModifyFlag.DiagonalUpBorder: return ISI.Extensions.SpreadSheets.StyleModifyFlag.DiagonalUpBorder;
				case global::Aspose.Cells.StyleModifyFlag.Font: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Font;
				case global::Aspose.Cells.StyleModifyFlag.FontSize: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSize;
				case global::Aspose.Cells.StyleModifyFlag.FontName: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontName;
				case global::Aspose.Cells.StyleModifyFlag.FontFamily: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontFamily;
				case global::Aspose.Cells.StyleModifyFlag.FontCharset: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontCharset;
				case global::Aspose.Cells.StyleModifyFlag.FontColor: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontColor;
				case global::Aspose.Cells.StyleModifyFlag.FontWeight: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontWeight;
				case global::Aspose.Cells.StyleModifyFlag.FontItalic: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontItalic;
				case global::Aspose.Cells.StyleModifyFlag.FontUnderline: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontUnderline;
				case global::Aspose.Cells.StyleModifyFlag.FontStrike: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontStrike;
				case global::Aspose.Cells.StyleModifyFlag.FontScript: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontScript;
				case global::Aspose.Cells.StyleModifyFlag.NumberFormat: return ISI.Extensions.SpreadSheets.StyleModifyFlag.NumberFormat;
				case global::Aspose.Cells.StyleModifyFlag.HorizontalAlignment: return ISI.Extensions.SpreadSheets.StyleModifyFlag.HorizontalAlignment;
				case global::Aspose.Cells.StyleModifyFlag.VerticalAlignment: return ISI.Extensions.SpreadSheets.StyleModifyFlag.VerticalAlignment;
				case global::Aspose.Cells.StyleModifyFlag.Indent: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Indent;
				case global::Aspose.Cells.StyleModifyFlag.Rotation: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Rotation;
				case global::Aspose.Cells.StyleModifyFlag.WrapText: return ISI.Extensions.SpreadSheets.StyleModifyFlag.WrapText;
				case global::Aspose.Cells.StyleModifyFlag.ShrinkToFit: return ISI.Extensions.SpreadSheets.StyleModifyFlag.ShrinkToFit;
				case global::Aspose.Cells.StyleModifyFlag.TextDirection: return ISI.Extensions.SpreadSheets.StyleModifyFlag.TextDirection;
				case global::Aspose.Cells.StyleModifyFlag.CellShading: return ISI.Extensions.SpreadSheets.StyleModifyFlag.CellShading;
				case global::Aspose.Cells.StyleModifyFlag.Pattern: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Pattern;
				case global::Aspose.Cells.StyleModifyFlag.ForegroundColor: return ISI.Extensions.SpreadSheets.StyleModifyFlag.ForegroundColor;
				case global::Aspose.Cells.StyleModifyFlag.BackgroundColor: return ISI.Extensions.SpreadSheets.StyleModifyFlag.BackgroundColor;
				case global::Aspose.Cells.StyleModifyFlag.Locked: return ISI.Extensions.SpreadSheets.StyleModifyFlag.Locked;
				case global::Aspose.Cells.StyleModifyFlag.HideFormula: return ISI.Extensions.SpreadSheets.StyleModifyFlag.HideFormula;
				case global::Aspose.Cells.StyleModifyFlag.AlignmentSettings: return ISI.Extensions.SpreadSheets.StyleModifyFlag.AlignmentSettings;
				case global::Aspose.Cells.StyleModifyFlag.FontScheme: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontScheme;
				case global::Aspose.Cells.StyleModifyFlag.FontDirty: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontDirty;
				case global::Aspose.Cells.StyleModifyFlag.FontSpellingError: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSpellingError;
				case global::Aspose.Cells.StyleModifyFlag.FontUFillTx: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontUFillTx;
				case global::Aspose.Cells.StyleModifyFlag.FontSpacing: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontSpacing;
				case global::Aspose.Cells.StyleModifyFlag.FontKerning: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontKerning;
				case global::Aspose.Cells.StyleModifyFlag.FontEqualize: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontEqualize;
				case global::Aspose.Cells.StyleModifyFlag.FontCap: return ISI.Extensions.SpreadSheets.StyleModifyFlag.FontCap;
			}

			return ISI.Extensions.SpreadSheets.StyleModifyFlag.All;
		}
	}
}
