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

namespace ISI.Extensions.SpreadSheets
{
	public class Coordinates
	{
		public static string RowKey(int row)
		{
			return $"{row + 1}";
		}

		/// <summary>
		/// column = 0 => A
		/// </summary>
		public static string ColumnKey(int column)
		{
			column++;
			var columnKey = string.Empty;

			while (column > 0)
			{
				var modulo = (column - 1) % 26;
				columnKey = Convert.ToChar(65 + modulo) + columnKey;
				column = (int)((column - modulo) / 26);
			}

			return columnKey;
		}

		public static int ColumnNumber(string columnKey)
		{
			var columnNumber = 0;

			var columnKeyQueue = new Queue<char>(columnKey);

			while (columnKeyQueue.Any())
			{
				columnNumber *= 26;
				columnNumber += ((int) columnKeyQueue.Dequeue()) - 64;
			}

			return columnNumber - 1;
		}

		/// <summary>
		/// row = 0, column = 0 => A1
		/// </summary>
		public static string Coordinate(int row, int column, bool absolute = false)
		{
			var referenceType = (absolute ? "$" : string.Empty);

			return $"{referenceType}{ColumnKey(column)}{referenceType}{RowKey(row)}";
		}

		/// <summary>
		/// sheetName = "sheet1", row = 0, column = 0 => sheet1!A1
		/// </summary>
		public static string Coordinate(string sheetName, int row, int column, bool absolute = false)
		{
			return $"'{sheetName}'!{Coordinate(row, column, absolute)}";
		}

		/// <summary>
		/// sheetName = "sheet1", row = 0, column = 0 => sheet1!A1
		/// </summary>
		public static string Coordinate(IWorksheet worksheet, int row, int column, bool absolute = false)
		{
			return Coordinate(worksheet.Name, row, column, absolute);
		}

		/// <summary>
		/// startRow = 0, startColumn = 0, stopRow = 0, stopColumn = 2 => A1:A2
		/// </summary>
		public static string CoordinateRange(int startRow, int startColumn, int stopRow, int stopColumn, bool absolute = false)
		{
			return $"{Coordinate(startRow, startColumn, absolute)}:{Coordinate(stopRow, stopColumn, absolute)}";
		}

		/// <summary>
		/// sheetName = "sheet1", startRow = 0, startColumn = 0, stopRow = 0, stopColumn = 2 => sheet1!A1:A2
		/// </summary>
		public static string CoordinateRange(string sheetName, int startRow, int startColumn, int stopRow, int stopColumn, bool absolute = false)
		{
			return $"'{sheetName}'!{CoordinateRange(startRow, startColumn, stopRow, stopColumn, absolute)}";
		}

		/// <summary>
		/// sheetName = "sheet1", startRow = 0, startColumn = 0, stopRow = 0, stopColumn = 2 => sheet1!A1:A2
		/// </summary>
		public static string CoordinateRange(IWorksheet worksheet, int startRow, int startColumn, int stopRow, int stopColumn, bool absolute = false)
		{
			return CoordinateRange(worksheet.Name, startRow, startColumn, stopRow, stopColumn, absolute);
		}
	}
}
