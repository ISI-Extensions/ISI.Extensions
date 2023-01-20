#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public class AddRecordsColumnOptions
	{
		public string ColumnName { get; set; }

		public string HeaderCaption { get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle HeaderStyle { private get; set; } = null;
		public string HeaderStyleName { private get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle GetHeaderStyle(ISI.Extensions.SpreadSheets.IWorkbook workbook) => HeaderStyle ??= (string.IsNullOrWhiteSpace(HeaderStyleName) ? null : workbook.GetNamedStyle(HeaderStyleName));

		public ISI.Extensions.SpreadSheets.ICellStyle Style { private get; set; } = null;
		public string StyleName { private get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle GetStyle(ISI.Extensions.SpreadSheets.IWorkbook workbook) => Style ??= (string.IsNullOrWhiteSpace(StyleName) ? null : workbook.GetNamedStyle(StyleName));

		public double? Width { get; set; }

		public string Formula { get; set; }

		public string FooterCaption { get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle FooterStyle { private get; set; } = null;
		public string FooterStyleName { private get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle GetFooterStyle(ISI.Extensions.SpreadSheets.IWorkbook workbook) => FooterStyle ??= (string.IsNullOrWhiteSpace(FooterStyleName) ? null : workbook.GetNamedStyle(FooterStyleName));
		public string FooterFormula { get; set; }

		public bool IsImage { get; set; }

		public ISI.Extensions.SpreadSheets.ColumnAttribute ColumnAttribute { get; private set; }
		
		public AddRecordsColumnOptions()
		{

		}

		public static (AddRecordsColumnOptions AddRecordsColumnOptions, int ColumnOffset) GetAddRecordsColumnOptions(System.Reflection.PropertyInfo columnPropertyInfo)
		{
			var columnAttribute = ((ISI.Extensions.SpreadSheets.ColumnAttribute[])(columnPropertyInfo.GetCustomAttributes(typeof(ISI.Extensions.SpreadSheets.ColumnAttribute), true))).FirstOrDefault();
			if (columnAttribute != null)
			{
				var columnOptions = new AddRecordsColumnOptions()
				{
					ColumnName = columnAttribute.ColumnName,
					HeaderCaption = columnAttribute.HeaderCaption,
					HeaderStyleName = columnAttribute.HeaderStyleName,
					StyleName = columnAttribute.StyleName,
					Formula = columnAttribute.Formula,
					FooterCaption = columnAttribute.FooterCaption,
					FooterStyleName = columnAttribute.FooterStyleName,
					FooterFormula = columnAttribute.FooterFormula,
					ColumnAttribute = columnAttribute,
					IsImage = (columnAttribute is ColumnImageAttribute),
				};

				if (columnAttribute.HasWidthValue)
				{
					columnOptions.Width = columnAttribute.Width;
				}

				if (string.IsNullOrWhiteSpace(columnOptions.ColumnName))
				{
					columnOptions.ColumnName = columnPropertyInfo.Name;
				}

				return (columnOptions, columnAttribute.ColumnOffset);
			}

			var dataMemberAttribute = ((System.Runtime.Serialization.DataMemberAttribute[])(columnPropertyInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.DataMemberAttribute), true))).FirstOrDefault();
			if (dataMemberAttribute != null)
			{
				var columnName = (string.IsNullOrWhiteSpace(dataMemberAttribute.Name) ? columnPropertyInfo.Name : dataMemberAttribute.Name);
				var columnOptions = new AddRecordsColumnOptions()
				{
					ColumnName = columnName,
					HeaderCaption = columnName,
				};

				return (columnOptions, dataMemberAttribute.Order);
			}

			{
				var columnOptions = new AddRecordsColumnOptions()
				{
					ColumnName = columnPropertyInfo.Name,
					HeaderCaption = columnPropertyInfo.Name,
				};

				return (columnOptions, -1);
			}
		}
	}
}
