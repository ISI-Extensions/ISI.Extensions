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

namespace ISI.Extensions.SpreadSheets
{
	/// <summary>
	/// Represents flags which indicates applied formatting properties.
	/// </summary>
	public class SetStyleFlag
	{
		/// <summary>All properties will be applied.</summary>
		public bool All { get; set; }
		/// <summary>All borders settings will be applied.</summary>
		public bool Borders { get; set; }
		/// <summary>Left border settings will be applied.</summary>
		public bool LeftBorder { get; set; }
		/// <summary>Right border settings will be applied.</summary>
		public bool RightBorder { get; set; }
		/// <summary>Top border settings will be applied.</summary>
		public bool TopBorder { get; set; }
		/// <summary>Bottom border settings will be applied.</summary>
		public bool BottomBorder { get; set; }
		/// <summary>Diagonal down border settings will be applied.</summary>
		public bool DiagonalDownBorder { get; set; }
		/// <summary>Diagonal up border settings will be applied.</summary>
		public bool DiagonalUpBorder { get; set; }
		/// <summary>Font settings will be applied.</summary>
		public bool Font { get; set; }
		/// <summary>Font size setting will be applied.</summary>
		public bool FontSize { get; set; }
		/// <summary>Font name setting will be applied.</summary>
		public bool FontName { get; set; }
		/// <summary>Font color setting will be applied.</summary>
		public bool FontColor { get; set; }
		/// <summary>Font bold setting will be applied.</summary>
		public bool FontBold { get; set; }
		/// <summary>Font italic setting will be applied.</summary>
		public bool FontItalic { get; set; }
		/// <summary>Font underline setting will be applied.</summary>
		public bool FontUnderline { get; set; }
		/// <summary>Font strikeout setting will be applied.</summary>
		public bool FontStrike { get; set; }
		/// <summary>Font script setting will be applied.</summary>
		public bool FontScript { get; set; }
		/// <summary>Number format setting will be applied.</summary>
		public bool NumberFormat { get; set; }
		/// <summary>Horizontal alignment setting will be applied.</summary>
		public bool HorizontalAlignment { get; set; }
		/// <summary>Vertical alignment setting will be applied.</summary>
		public bool VerticalAlignment { get; set; }
		/// <summary>Indent level setting will be applied.</summary>
		public bool Indent { get; set; }
		/// <summary>Rotation setting will be applied.</summary>
		public bool Rotation { get; set; }
		/// <summary>Wrap text setting will be applied.</summary>
		public bool WrapText { get; set; }
		/// <summary>Shrink to fit setting will be applied.</summary>
		public bool ShrinkToFit { get; set; }
		/// <summary>Text direction setting will be applied.</summary>
		public bool TextDirection { get; set; }
		/// <summary>Cell shading setting will be applied.</summary>
		public bool CellShading { get; set; }
		/// <summary>Locked setting will be applied.</summary>
		public bool Locked { get; set; }
		/// <summary>Hide formula setting will be applied.</summary>
		public bool HideFormula { get; set; }
	}
}
