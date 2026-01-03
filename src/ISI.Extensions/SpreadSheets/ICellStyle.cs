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
using System.Threading.Tasks;

namespace ISI.Extensions.SpreadSheets
{
	public interface ICellStyle
	{
		ICellStyle Clone(string newName = null);
		void Update();
		bool IsModified(StyleModifyFlag modifyFlag);
		bool SetBorder(Border borderEdge, CellBorder borderStyle, System.Drawing.Color borderColor);
		void SetCustom(string custom, bool builtinPreference);
		void SetTwoColorGradient(System.Drawing.Color color1, System.Drawing.Color color2, GradientStyle gradientStyleType, int variant);
		void GetTwoColorGradient(out System.Drawing.Color color1, out System.Drawing.Color color2, out GradientStyle gradientStyleType, out int variant);
		IThemeColor BackgroundThemeColor { get; set; }
		IThemeColor ForegroundThemeColor { get; set; }
		string Name { get; set; }
		BackgroundPattern Pattern { get; set; }
		ISI.Extensions.SpreadSheets.IBorderCollection Borders { get; }
		System.Drawing.Color BackgroundColor { get; set; }
		int BackgroundArgbColor { get; set; }
		System.Drawing.Color ForegroundColor { get; set; }
		int ForegroundArgbColor { get; set; }
		ISI.Extensions.SpreadSheets.ICellStyle ParentStyle { get; }
		int IndentLevel { get; set; }
		ISI.Extensions.SpreadSheets.IFont Font { get; }
		int RotationAngle { get; set; }
		ISI.Extensions.SpreadSheets.VerticalAlignment VerticalAlignment { get; set; }
		ISI.Extensions.SpreadSheets.HorizontalAlignment HorizontalAlignment { get; set; }
		bool IsTextWrapped { get; set; }
		int Number { get; set; }
		ISI.Extensions.SpreadSheets.NumberFormat NumberFormat { get; set; }
		bool IsLocked { get; set; }
		string Custom { get; set; }
		string CultureCustom { get; set; }
		bool IsFormulaHidden { get; set; }
		bool ShrinkToFit { get; set; }
		ISI.Extensions.SpreadSheets.TextDirection TextDirection { get; set; }
		bool IsJustifyDistributed { get; set; }
		bool QuotePrefix { get; }
		bool IsGradient { get; set; }
		bool IsPercent { get; }
		bool IsDateTime { get; }
	}
}
