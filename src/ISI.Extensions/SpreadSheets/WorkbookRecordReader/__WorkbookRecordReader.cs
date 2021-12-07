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
	public partial class WorkbookRecordReader<TRecord> : IDisposable
		where TRecord : class, new()
	{
		protected ISI.Extensions.SpreadSheets.IWorkbook Workbook { get; }
		protected IEnumerable<ISI.Extensions.Parsers.IColumnInfo<TRecord>> Columns { get; }
		protected IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> OnReads { get; }

		public WorkbookRecordReader(ISpreadSheetHelper spreadSheetHelper, string fileName, IEnumerable<ISI.Extensions.Parsers.IColumnInfo<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
		{
			Workbook = spreadSheetHelper.Open(fileName);
			Columns = columns ?? ISI.Extensions.Parsers.ColumnInfoCollection<TRecord>.GetDefault();
			OnReads = onReads;
		}

		public WorkbookRecordReader(ISpreadSheetHelper spreadSheetHelper, System.IO.Stream stream, IEnumerable<ISI.Extensions.Parsers.IColumnInfo<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
		{
			Workbook = spreadSheetHelper.Open(stream);
			Columns = columns ?? ISI.Extensions.Parsers.ColumnInfoCollection<TRecord>.GetDefault();
			OnReads = onReads;
		}

		public WorkbookRecordReader(ISI.Extensions.SpreadSheets.IWorkbook workbook, IEnumerable<ISI.Extensions.Parsers.IColumnInfo<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
		{
			Workbook = workbook;
			Columns = columns ?? ISI.Extensions.Parsers.ColumnInfoCollection<TRecord>.GetDefault();
			OnReads = onReads;
		}

		public WorksheetRecordReaders Worksheets => new(Workbook, Columns, OnReads);

		public void Dispose()
		{
			Workbook?.Dispose();
		}
	}
}