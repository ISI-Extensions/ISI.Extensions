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
	public class Coordinates
	{
		public static string RowKey(int row)
		{
			return string.Format("{0}", row + 1);
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
			return string.Format("{2}{0}{2}{1}", ColumnKey(column), RowKey(row), (absolute ? "$" : string.Empty));
		}

		/// <summary>
		/// row = 0, column = 0 => A1
		/// </summary>
		public static string CoordinateRange(int startRow, int startColumn, int stopRow, int stopColumn, bool absolute = false)
		{
			return string.Format("{0}:{1}", Coordinate(startRow, startColumn, absolute), Coordinate(stopRow, stopColumn, absolute));
		}
	}
}
