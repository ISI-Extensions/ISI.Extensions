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

namespace ISI.Extensions.SpreadSheets
{
	public interface IFont
	{
		/// <summary>Represent the character set.</summary>
		int Charset { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the font is italic.
		/// </summary>
		bool IsItalic { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the font is bold.
		/// </summary>
		bool IsBold { get; set; }
		/// <summary>Gets and sets the text caps type.</summary>
		ISI.Extensions.StringFormat.TextCase TextCase { get; set; }
		/// <summary>Gets the strike type of the text.</summary>
		ISI.Extensions.SpreadSheets.TextStrike TextStrike { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the font is single strikeout.
		/// </summary>
		bool IsStrikeout { get; set; }
		/// <summary>Gets and sets the script offset,in unit of percentage</summary>
		double ScriptOffset { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the font is super script.
		/// </summary>
		bool IsSuperscript { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the font is subscript.
		/// </summary>
		bool IsSubscript { get; set; }
		/// <summary>Gets or sets the font underline type.</summary>
		ISI.Extensions.SpreadSheets.FontUnderline FontUnderline { get; set; }
		/// <summary>
		/// Gets  or sets the name of the <see cref="T:Aspose.Cells.Font" />.
		/// </summary>
		/// <example>
		///   <code>
		///       [C#]
		/// 
		///       Style style;
		///       ..........
		///       Font font = style.Font;
		///       font.Name = "Times New Roman";
		/// 
		///       [Visual Basic]
		/// 
		///       Dim style As Style
		///       ..........
		///       Dim font As Font =  style.Font
		///       font.Name = "Times New Roman"
		///       </code>
		/// </example>
		string Name { get; set; }
		/// <summary>Gets and sets the double size of the font.</summary>
		double DoubleSize { get; set; }
		/// <summary>Gets or sets the size of the font.</summary>
		int Size { get; set; }
		/// <summary>Gets and sets the theme color.</summary>
		/// <remarks>
		/// If the font color is not a theme color, NULL will be returned.
		/// </remarks>
		ISI.Extensions.SpreadSheets.IThemeColor ThemeColor { get; set; }
		/// <summary>
		/// Gets or sets the <see cref="T:System.Drawing.Color" /> of the font.
		/// </summary>
		System.Drawing.Color Color { get; set; }
		/// <summary>Gets and sets the color with a 32-bit ARGB value.</summary>
		int ArgbColor { get; set; }
		/// <summary>
		/// Indicates whether the normalization of height that is to be applied to the text run.
		/// </summary>
		bool IsNormalizeHeights { get; set; }
	}
}
