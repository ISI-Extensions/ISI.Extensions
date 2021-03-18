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
	public interface IWorksheetCellsColumn
	{
		/// <summary>Applies formats for a whole column.</summary>
		/// <param name="style">The style object which will be applied.</param>
		/// <param name="flag">Flags which indicates applied formatting properties.</param>
		void ApplyStyle(ISI.Extensions.SpreadSheets.ICellStyle style, ISI.Extensions.SpreadSheets.SetStyleFlag flag);
		
		/// <summary>Gets the index of this column.</summary>
		int Index { get; }
		
		/// <summary>Gets and sets the column width in unit of characters.</summary>
		double Width { get; set; }

		/// <summary>Indicates whether the column is hidden.</summary>
		bool IsHidden { get; set; }

		/// <summary>Gets the style of this column.</summary>
		/// <remarks>
		/// You have to call Column.ApplyStyle() method to save your changing with the row style,
		/// otherwise it will not effect.
		/// </remarks>
		ISI.Extensions.SpreadSheets.ICellStyle Style { get; }
	}
}
