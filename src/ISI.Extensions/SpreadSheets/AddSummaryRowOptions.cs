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
	public class AddSummaryRowOptions
	{
		public string RowName { get; set; }

		public string HeaderCaption { get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle HeaderStyle { private get; set; } = null;
		public string HeaderStyleName { private get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle GetHeaderStyle(ISI.Extensions.SpreadSheets.IWorkbook workbook) => HeaderStyle ??= (string.IsNullOrWhiteSpace(HeaderStyleName) ? null : workbook.GetNamedStyle(HeaderStyleName));

		public ISI.Extensions.SpreadSheets.ICellStyle Style { private get; set; } = null;
		public string StyleName { private get; set; }
		public ISI.Extensions.SpreadSheets.ICellStyle GetStyle(ISI.Extensions.SpreadSheets.IWorkbook workbook) => Style ??= (string.IsNullOrWhiteSpace(StyleName) ? null : workbook.GetNamedStyle(StyleName));

		public string Formula { get; set; }

		public AddSummaryRowOptions()
		{

		}

		public static (AddSummaryRowOptions AddSummaryRowOptions, int RowOffset) GetAddSummaryRowOptions(System.Reflection.PropertyInfo rowPropertyInfo)
		{
			var rowAttribute = ((ISI.Extensions.SpreadSheets.RowAttribute[])(rowPropertyInfo.GetCustomAttributes(typeof(ISI.Extensions.SpreadSheets.RowAttribute), true))).FirstOrDefault();
			if (rowAttribute != null)
			{
				var rowOptions = new AddSummaryRowOptions()
				{
					RowName = rowAttribute.RowName,
					HeaderCaption = rowAttribute.HeaderCaption,
					HeaderStyleName = rowAttribute.HeaderStyleName,
					StyleName = rowAttribute.StyleName,
					Formula = rowAttribute.Formula,
				};

				if (string.IsNullOrWhiteSpace(rowOptions.RowName))
				{
					rowOptions.RowName = rowPropertyInfo.Name;
				}

				return (rowOptions, rowAttribute.RowOffset);
			}

			var dataMemberAttribute = ((System.Runtime.Serialization.DataMemberAttribute[])(rowPropertyInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.DataMemberAttribute), true))).FirstOrDefault();
			if (dataMemberAttribute != null)
			{
				var rowName = (string.IsNullOrWhiteSpace(dataMemberAttribute.Name) ? rowPropertyInfo.Name : dataMemberAttribute.Name);
				var rowOptions = new AddSummaryRowOptions()
				{
					RowName = rowName,
					HeaderCaption = rowName,
				};

				return (rowOptions, dataMemberAttribute.Order);
			}

			{
				var rowOptions = new AddSummaryRowOptions()
				{
					RowName = rowPropertyInfo.Name,
					HeaderCaption = rowPropertyInfo.Name,
				};

				return (rowOptions, -1);
			}
		}
	}
}
