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
		public class Font : ISI.Extensions.SpreadSheets.IFont
		{
			internal readonly global::Aspose.Cells.Font _font = null;

			public Font(global::Aspose.Cells.Font font)
			{
				_font = font;
			}

			public int Charset
			{
				get => _font.Charset;
				set => _font.Charset = value;
			}

			public bool IsItalic
			{
				get => _font.IsItalic;
				set => _font.IsItalic = value;
			}

			public bool IsBold
			{
				get => _font.IsBold;
				set => _font.IsBold = value;
			}

			public ISI.Extensions.StringFormat.TextCase TextCase
			{
				get => _font.CapsType.ToTextCase();
				set => _font.CapsType = value.ToTextCase();
			}

			public ISI.Extensions.SpreadSheets.TextStrike TextStrike
			{
				get => _font.StrikeType.ToTextStrike();
				set => _font.StrikeType = value.ToTextStrike();
			}

			public bool IsStrikeout
			{
				get => _font.IsStrikeout;
				set => _font.IsStrikeout = value;
			}

			public double ScriptOffset
			{
				get => _font.ScriptOffset;
				set => _font.ScriptOffset = value;
			}

			public bool IsSuperscript
			{
				get => _font.IsSuperscript;
				set => _font.IsSuperscript = value;
			}

			public bool IsSubscript
			{
				get => _font.IsSubscript;
				set => _font.IsSubscript = value;
			}

			public ISI.Extensions.SpreadSheets.FontUnderline FontUnderline
			{
				get => _font.Underline.ToFontUnderline();
				set => _font.Underline = value.ToFontUnderline();
			}

			public string Name
			{
				get => _font.Name;
				set => _font.Name = value;
			}

			public double DoubleSize
			{
				get => _font.DoubleSize;
				set => _font.DoubleSize = value;
			}

			public int Size
			{
				get => _font.Size;
				set => _font.Size = value;
			}

			public ISI.Extensions.SpreadSheets.IThemeColor ThemeColor
			{
				get => new ISI.Extensions.Aspose.Cells.ThemeColor(_font.ThemeColor);
				set => _font.ThemeColor = value.GetAsposeThemeColor();
			}

			public System.Drawing.Color Color
			{
				get => _font.Color;
				set => _font.Color = value;
			}

			public int ArgbColor
			{
				get => _font.ArgbColor;
				set => _font.ArgbColor = value;
			}

			public bool IsNormalizeHeights
			{
				get => _font.IsNormalizeHeights;
				set => _font.IsNormalizeHeights = value;
			}
		}
	}
}