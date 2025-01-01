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
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class CellStyle : ISI.Extensions.SpreadSheets.ICellStyle
		{
			internal readonly ISI.Extensions.Aspose.Cells.Workbook _workbook = null;
			internal readonly global::Aspose.Cells.Style _cellStyle = null;

			internal CellStyle(ISI.Extensions.Aspose.Cells.Workbook workbook, global::Aspose.Cells.Style cellStyle)
			{
				_workbook = workbook;
				_cellStyle = cellStyle;
			}

			public ISI.Extensions.SpreadSheets.ICellStyle Clone(string newName = null)
			{
				var cellStyle = _workbook.CreateCellStyle();

				cellStyle.GetAsposeStyle().Copy(_cellStyle);

				if (!string.IsNullOrWhiteSpace(newName))
				{
					cellStyle.Name = newName;
				}

				return cellStyle;
			}

			public void Update()
			{
				_cellStyle.Update();
			}

			public bool IsModified(ISI.Extensions.SpreadSheets.StyleModifyFlag modifyFlag)
			{
				return _cellStyle.IsModified(modifyFlag.ToStyleModifyFlag());
			}

			public bool SetBorder(ISI.Extensions.SpreadSheets.Border borderEdge, ISI.Extensions.SpreadSheets.CellBorder borderStyle, System.Drawing.Color borderColor)
			{
				return _cellStyle.SetBorder(borderEdge.ToBorder(), borderStyle.ToCellBorder(), borderColor);
			}

			public void SetCustom(string custom, bool builtinPreference)
			{
				_cellStyle.SetCustom(custom, builtinPreference);
			}

			public void SetTwoColorGradient(System.Drawing.Color color1, System.Drawing.Color color2, ISI.Extensions.SpreadSheets.GradientStyle gradientStyle, int variant)
			{
				_cellStyle.SetTwoColorGradient(color1, color2, gradientStyle.ToGradientStyle(), variant);
			}

			public void GetTwoColorGradient(out System.Drawing.Color color1, out System.Drawing.Color color2, out ISI.Extensions.SpreadSheets.GradientStyle gradientStyle, out int variant)
			{
				_cellStyle.GetTwoColorGradient(out color1, out color2, out var _gradientStyleType, out variant);

				gradientStyle = _gradientStyleType.ToGradientStyle();
			}

			public ISI.Extensions.SpreadSheets.IThemeColor BackgroundThemeColor
			{
				get => new ThemeColor(_cellStyle.BackgroundThemeColor);
				set => _cellStyle.BackgroundThemeColor = value.GetAsposeThemeColor();
			}

			public ISI.Extensions.SpreadSheets.IThemeColor ForegroundThemeColor
			{
				get => new ThemeColor(_cellStyle.ForegroundThemeColor);
				set => _cellStyle.ForegroundThemeColor = value.GetAsposeThemeColor();
			}

			public string Name
			{
				get => _cellStyle.Name;
				set => _cellStyle.Name = value;
			}

			public ISI.Extensions.SpreadSheets.BackgroundPattern Pattern
			{
				get => _cellStyle.Pattern.ToBackgroundPattern();
				set => _cellStyle.Pattern = value.ToBackgroundPattern();
			}

			public ISI.Extensions.SpreadSheets.IBorderCollection Borders => new ISI.Extensions.Aspose.Cells.BorderCollection(_cellStyle.Borders);

			public System.Drawing.Color BackgroundColor
			{
				get => _cellStyle.BackgroundColor;
				set => _cellStyle.BackgroundColor = value;
			}

			public int BackgroundArgbColor
			{
				get => _cellStyle.BackgroundArgbColor;
				set => _cellStyle.BackgroundArgbColor = value;
			}

			public System.Drawing.Color ForegroundColor
			{
				get => _cellStyle.ForegroundColor;
				set => _cellStyle.ForegroundColor = value;
			}

			public int ForegroundArgbColor
			{
				get => _cellStyle.ForegroundArgbColor;
				set => _cellStyle.ForegroundArgbColor = value;
			}

			public ISI.Extensions.SpreadSheets.ICellStyle ParentStyle => new ISI.Extensions.Aspose.Cells.CellStyle(_workbook, _cellStyle.ParentStyle);

			public int IndentLevel
			{
				get => _cellStyle.IndentLevel;
				set => _cellStyle.IndentLevel = value;
			}

			public ISI.Extensions.SpreadSheets.IFont Font => new ISI.Extensions.Aspose.Cells.Font(_cellStyle.Font);

			public int RotationAngle
			{
				get => _cellStyle.RotationAngle;
				set => _cellStyle.RotationAngle = value;
			}

			public ISI.Extensions.SpreadSheets.VerticalAlignment VerticalAlignment
			{
				get => _cellStyle.VerticalAlignment.ToVerticalAlignment();
				set => _cellStyle.VerticalAlignment = value.ToVerticalAlignment();
			}

			public ISI.Extensions.SpreadSheets.HorizontalAlignment HorizontalAlignment
			{
				get => _cellStyle.HorizontalAlignment.ToHorizontalAlignment();
				set => _cellStyle.HorizontalAlignment = value.ToHorizontalAlignment();
			}

			public bool IsTextWrapped
			{
				get => _cellStyle.IsTextWrapped;
				set => _cellStyle.IsTextWrapped = value;
			}

			public int Number
			{
				get => _cellStyle.Number;
				set => _cellStyle.Number = value;
			}

			public ISI.Extensions.SpreadSheets.NumberFormat NumberFormat
			{
				get => CellsExtensions.ToNumberFormat(_cellStyle.Number);
				set => _cellStyle.Number = value.ToNumberFormat();
			}

			public bool IsLocked
			{
				get => _cellStyle.IsLocked;
				set => _cellStyle.IsLocked = value;
			}

			public string Custom
			{
				get => _cellStyle.Custom;
				set => _cellStyle.Custom = value;
			}

			public string CultureCustom
			{
				get => _cellStyle.CultureCustom;
				set => _cellStyle.CultureCustom = value;
			}

			public bool IsFormulaHidden
			{
				get => _cellStyle.IsFormulaHidden;
				set => _cellStyle.IsFormulaHidden = value;
			}

			public bool ShrinkToFit
			{
				get => _cellStyle.ShrinkToFit;
				set => _cellStyle.ShrinkToFit = value;
			}

			public ISI.Extensions.SpreadSheets.TextDirection TextDirection
			{
				get => _cellStyle.TextDirection.ToTextDirection();
				set => _cellStyle.TextDirection = value.ToTextDirection();
			}

			public bool IsJustifyDistributed
			{
				get => _cellStyle.IsJustifyDistributed;
				set => _cellStyle.IsJustifyDistributed = value;
			}

			public bool QuotePrefix => _cellStyle.QuotePrefix;

			public bool IsGradient
			{
				get => _cellStyle.IsGradient;
				set => _cellStyle.IsGradient = value;
			}

			public bool IsPercent => _cellStyle.IsPercent;

			public bool IsDateTime => _cellStyle.IsDateTime;
		}
	}
}
