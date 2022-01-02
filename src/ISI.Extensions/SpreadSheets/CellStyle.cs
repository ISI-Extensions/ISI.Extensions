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

namespace ISI.Extensions.SpreadSheets
{
	public class CellStyle
	{
		public string StyleName { get; internal set; }

		public NumberFormat? NumberFormat { get; set; }
		public string NumberFormatCustom { get; set; }

		public System.Drawing.Color? ForegroundColor { get; set; }
		public System.Drawing.Color? BackgroundColor { get; set; }

		public BackgroundPattern? Pattern { get; set; }

		public HorizontalAlignment? HorizontalAlignment { get; set; }
		public VerticalAlignment? VerticalAlignment { get; set; }

		public string FontName { get; set; }
		public System.Drawing.Color? FontColor { get; set; }
		public int? FontSize { get; set; }
		public double? FontSizeDouble { get; set; }
		public bool? FontIsBold { get; set; }
		public bool? FontIsItalic { get; set; }
		public bool? FontIsStrikeout { get; set; }
		public bool? FontIsSubscript { get; set; }
		public bool? FontIsSuperscript { get; set; }

		public CellBorder? TopBorder { get; set; }
		public CellBorder? RightBorder { get; set; }
		public CellBorder? BottomBorder { get; set; }
		public CellBorder? LeftBorder { get; set; }

		public CellStyle(string styleName)
		{
			StyleName = styleName;
		}

		public CellStyle Clone(string newStyleName)
		{
			var result = new CellStyle(newStyleName)
																			{
																				NumberFormat = NumberFormat,
																				NumberFormatCustom = NumberFormatCustom,

																				ForegroundColor = ForegroundColor,
																				BackgroundColor = BackgroundColor,

																				Pattern = Pattern,

																				HorizontalAlignment = HorizontalAlignment,
																				VerticalAlignment = VerticalAlignment,

																				FontName = FontName,
																				FontColor = FontColor,
																				FontSize = FontSize,
																				FontSizeDouble = FontSizeDouble,
																				FontIsBold = FontIsBold,
																				FontIsItalic = FontIsItalic,
																				FontIsStrikeout = FontIsStrikeout,
																				FontIsSubscript = FontIsSubscript,
																				FontIsSuperscript = FontIsSuperscript,

																				TopBorder = TopBorder,
																				RightBorder = RightBorder,
																				BottomBorder = BottomBorder,
																				LeftBorder = LeftBorder
																			};

			return result;
		}
	}
}
