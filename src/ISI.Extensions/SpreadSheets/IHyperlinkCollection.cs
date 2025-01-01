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

namespace ISI.Extensions.SpreadSheets
{
	public interface IHyperlinkCollection
	{
		/// <summary>
		/// Adds a hyperlink to a specified cell or a range of cells.
		/// </summary>
		/// <param name="firstRow">First row of the hyperlink range.</param>
		/// <param name="firstColumn">First column of the hyperlink range.</param>
		/// <param name="totalRows">Number of rows in this hyperlink range.</param>
		/// <param name="totalColumns">Number of columns of this hyperlink range.</param>
		/// <param name="address">Address of the hyperlink.</param>
		/// <returns>
		/// <see cref="T:Aspose.Cells.Hyperlink" /> object index.</returns>
		/// <example>
		///   <code>
		///       [C#]
		/// 
		///       Worksheet worksheet = excel.Worksheets[0];
		///       worksheet.Hyperlinks.Add("A4", 1, 1, "http://www.xxxxxxxxxxxxxxxxxxxxxxx.com");
		///       worksheet.Hyperlinks.Add("A5", 1, 1, "c:\\book1.xls");
		/// 
		///       [Visual Basic]
		/// 
		///       Dim worksheet as Worksheet = excel.Worksheets(0)
		///       worksheet.Hyperlinks.Add("A4", 1, 1, "http://www.xxxxxxxxxxxxxxxxxxxxxxxxxxx.com")
		///       worksheet.Hyperlinks.Add("A5", 1, 1, "c:\\book1.xls")
		/// 
		///       </code>
		/// </example>
		int Add(int firstRow, int firstColumn, int totalRows, int totalColumns, string address);
		/// <summary>
		/// Adds a hyperlink to a specified cell or a range of cells.
		/// </summary>
		/// <param name="cellName">Cell name.</param>
		/// <param name="totalRows">Number of rows in this hyperlink range.</param>
		/// <param name="totalColumns">Number of columns of this hyperlink range.</param>
		/// <param name="address">Address of the hyperlink.</param>
		/// <returns>
		/// <see cref="T:Aspose.Cells.Hyperlink" /> object index.</returns>
		int Add(string cellName, int totalRows, int totalColumns, string address);
		/// <summary>
		/// Adds a hyperlink to a specified cell or a range of cells.
		/// </summary>
		/// <param name="startCellName">The top-left cell of the range.</param>
		/// <param name="endCellName">The bottom-right cell of the range.</param>
		/// <param name="address">Address of the hyperlink.</param>
		/// <param name="textToDisplay">The text to be displayed for the specified hyperlink.</param>
		/// <param name="screenTip">The screenTip text for the specified hyperlink.</param>
		/// <returns>
		/// <see cref="T:Aspose.Cells.Hyperlink" /> object index.</returns>
		int Add(string startCellName, string endCellName, string address, string textToDisplay, string screenTip);
		/// <summary>Remove the hyperlink  at the specified index.</summary>
		/// <param name="index">The zero based index of the element.</param>
		void RemoveAt(int index);
		/// <summary>Clears all hyperlinks.</summary>
		void Clear();
		/// <summary>
		/// Gets the <see cref="T:Aspose.Cells.Hyperlink" /> element at the specified index.
		/// </summary>
		/// <param name="index">The zero based index of the element.</param>
		/// <returns>The element at the specified index.</returns>
		IHyperlink this[int index] { get; }
	}
}
