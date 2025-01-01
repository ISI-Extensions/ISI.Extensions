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

namespace ISI.Extensions.Extensions
{
	public static class SpreadSheetExtensions
	{
		private static readonly IDictionary<ISI.Extensions.SpreadSheets.FileFormat, string> _toFileNameExtension = null;
		private static readonly IDictionary<string, ISI.Extensions.SpreadSheets.FileFormat> _toFileFormat = null;

		static SpreadSheetExtensions()
		{
			_toFileNameExtension = new Dictionary<ISI.Extensions.SpreadSheets.FileFormat, string>()
			{
				{ISI.Extensions.SpreadSheets.FileFormat.Default, "xlsx"},
				{ISI.Extensions.SpreadSheets.FileFormat.CommaSeparatedValues, "csv"},
				{ISI.Extensions.SpreadSheets.FileFormat.TabDelimited, "tsv"},
				{ISI.Extensions.SpreadSheets.FileFormat.Pdf, "pdf"},
				{ISI.Extensions.SpreadSheets.FileFormat.Html, "html"},
				{ISI.Extensions.SpreadSheets.FileFormat.Xls, "xls"},
				{ISI.Extensions.SpreadSheets.FileFormat.Xlsx, "xlsx"}
			};

			_toFileFormat = _toFileNameExtension.Where(_ => _.Key != ISI.Extensions.SpreadSheets.FileFormat.Default).ToDictionary(_ => _.Value, _ => _.Key, StringComparer.InvariantCultureIgnoreCase);
		}

		public static ISI.Extensions.SpreadSheets.FileFormat ToFileFormat(string fileNameExtension, ISI.Extensions.SpreadSheets.FileFormat defaultFileFormat = ISI.Extensions.SpreadSheets.FileFormat.Default)
		{
			fileNameExtension = System.IO.Path.GetExtension(fileNameExtension);

			if (_toFileFormat.TryGetValue(fileNameExtension, out var fileFormat))
			{
				return fileFormat;
			}

			return defaultFileFormat;
		}

		public static ISI.Extensions.Documents.FileFormat ToFileFormat(this ISI.Extensions.SpreadSheets.FileFormat fileFormat)
		{
			switch (fileFormat)
			{
				case ISI.Extensions.SpreadSheets.FileFormat.Default:
					return ISI.Extensions.Documents.FileFormat.Default;
				case ISI.Extensions.SpreadSheets.FileFormat.CommaSeparatedValues:
					return ISI.Extensions.Documents.FileFormat.CommaSeparatedValues;
				case ISI.Extensions.SpreadSheets.FileFormat.TabDelimited:
					return ISI.Extensions.Documents.FileFormat.TabDelimited;
				case ISI.Extensions.SpreadSheets.FileFormat.Pdf:
					return ISI.Extensions.Documents.FileFormat.Pdf;
				case ISI.Extensions.SpreadSheets.FileFormat.Html:
					return ISI.Extensions.Documents.FileFormat.Html;
				case ISI.Extensions.SpreadSheets.FileFormat.Xls:
					return ISI.Extensions.Documents.FileFormat.Xls;
				case ISI.Extensions.SpreadSheets.FileFormat.Xlsx:
					return ISI.Extensions.Documents.FileFormat.Xlsx;
				default:
					throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
			}
		}

		public static string ToFileName(this ISI.Extensions.SpreadSheets.FileFormat fileFormat, string fileName)
		{
			return string.Format("{0}.{1}", fileName, fileFormat.ToFileNameExtension());
		}

		public static string ToFileNameExtension(this ISI.Extensions.SpreadSheets.FileFormat fileFormat)
		{
			return _toFileNameExtension[fileFormat];
		}
	}
}
