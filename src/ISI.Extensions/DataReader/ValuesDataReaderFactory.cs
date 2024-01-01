#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions.DataReader
{
	public class ValuesDataReaderFactory
	{
		public static ISI.Extensions.DataReader.IDataReader GetValuesDataReaderByFileName(string fileName, System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
		{
			var fileExtension = System.IO.Path.GetExtension(fileName);

			if (ISI.Extensions.TextParserFactory.FileExtensionToTextDelimiter.TryGetValue(fileExtension, out var textDelimiter))
			{
				var textParser = ISI.Extensions.TextParserFactory.GetTextParser(textDelimiter);

				return new TextParserDataReader(stream, textParser, columns, transformRecord);
			}

			if (string.Equals(fileExtension, "xlsx", StringComparison.InvariantCultureIgnoreCase) || string.Equals(fileExtension, "xls", StringComparison.InvariantCultureIgnoreCase))
			{
				return new ISI.Extensions.SpreadSheets.WorkbookValuesDataReader(stream, columns, transformRecord);
			}

			return null;
		}

		public static ISI.Extensions.DataReader.IDataReader GetValuesDataReaderByTextDelimiter(TextParserFactory.TextDelimiter textDelimiter, System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn> columns = null, ISI.Extensions.DataReader.TransformRecord transformRecord = null)
		{
			var textParser = ISI.Extensions.TextParserFactory.GetTextParser(textDelimiter);

			return new TextParserDataReader(stream, textParser, columns, transformRecord);
		}
	}
}
