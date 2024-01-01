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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.SpreadSheets
{
	public class WorkbookRecordReader<TRecord> : IEnumerable<WorksheetRecordReader<TRecord>>, IDisposable
		where TRecord : class, new()
	{
		protected ISI.Extensions.SpreadSheets.IWorkbook Workbook { get; }
		protected bool DoWorkbookDisposable { get; }
		protected IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> Columns { get; }
		protected IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> OnReads { get; }

		private class WorkbookRecordReaderEnumerator : IEnumerator<WorksheetRecordReader<TRecord>>
		{
			protected ISI.Extensions.SpreadSheets.IWorkbook Workbook { get; }
			protected System.Collections.IEnumerator RowEnumerator { get; set; }

			protected IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> Columns { get; }
			protected IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> OnReads { get; }

			public WorkbookRecordReaderEnumerator(string fileName, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
				: this(columns, onReads)
			{
				var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

				Workbook = spreadSheetHelper.Open(fileName);
			}

			public WorkbookRecordReaderEnumerator(System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
				: this(columns, onReads)
			{
				var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

				Workbook = spreadSheetHelper.Open(stream);
			}

			public WorkbookRecordReaderEnumerator(ISI.Extensions.SpreadSheets.IWorkbook workbook, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
				: this(columns, onReads)
			{
				Workbook = workbook;
			}

			private WorkbookRecordReaderEnumerator(IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
			{
				Columns = columns;
				OnReads = onReads;

				Reset();
			}

			public void Reset()
			{
				Current = null;
				(RowEnumerator as IDisposable)?.Dispose();
				RowEnumerator = null;
			}

			public bool MoveNext()
			{
				RowEnumerator ??= Workbook.Worksheets.GetEnumerator();

				if (!RowEnumerator.MoveNext())
				{
					Current = null;
					return false;
				}

				Current = new WorksheetRecordReader<TRecord>(RowEnumerator.Current as IWorksheet, Columns, OnReads);

				return true;
			}

			public WorksheetRecordReader<TRecord> Current { get; private set; }

			object System.Collections.IEnumerator.Current => Current;

			public void Dispose()
			{

			}
		}

		public WorkbookRecordReader(string fileName, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(fileName);
			DoWorkbookDisposable = true;
		}

		public WorkbookRecordReader(System.IO.Stream stream, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			var spreadSheetHelper = ISI.Extensions.ServiceLocator.Current.GetService<ISpreadSheetHelper>();

			Workbook = spreadSheetHelper.Open(stream);
			DoWorkbookDisposable = true;
		}

		public WorkbookRecordReader(ISI.Extensions.SpreadSheets.IWorkbook workbook, IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns = null, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads = null)
			: this(columns, onReads)
		{
			Workbook = workbook;
		}

		private WorkbookRecordReader(IEnumerable<ISI.Extensions.Columns.IColumn<TRecord>> columns, IEnumerable<ISI.Extensions.Parsers.OnRead<TRecord>> onReads)
		{
			Columns = columns ?? ISI.Extensions.Columns.ColumnCollection<TRecord>.GetDefault();
			OnReads = onReads;
		}

		public IEnumerator<WorksheetRecordReader<TRecord>> GetEnumerator()
		{
			return new WorkbookRecordReaderEnumerator(Workbook, Columns, OnReads);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			if (DoWorkbookDisposable)
			{
				Workbook?.Dispose();
			}
		}
	}
}