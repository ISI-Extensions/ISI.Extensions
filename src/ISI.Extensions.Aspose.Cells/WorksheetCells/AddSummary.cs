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
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetCells
		{
			public ISI.Extensions.SpreadSheets.AddSummaryResponse AddSummary<TSummary>(TSummary summary, int startRow, int startColumn, int valueColumnOffset = 1, ISI.Extensions.SpreadSheets.AddSummaryRowCollection<TSummary> rowDefinitions = null)
			{
				var response = new ISI.Extensions.SpreadSheets.AddSummaryResponse();

				rowDefinitions ??= ISI.Extensions.SpreadSheets.AddSummaryRowCollection<TSummary>.BuildDefault(_worksheet._worksheets._workbook);

				if (rowDefinitions.Any(rowDefinition => rowDefinition.RowOffset < 0))
				{
					var rowOffset = rowDefinitions.Max(rowDefinition => rowDefinition.RowOffset) + 1;

					foreach (var rowDefinition in rowDefinitions.Where(rowDefinition => rowDefinition.RowOffset < 0).OfType<ISI.Extensions.SpreadSheets.ISetRowOffset>())
					{
						rowDefinition.RowOffset = rowOffset++;
					}
				}

				var workbook = _worksheet._worksheets._workbook;

				var rowKeyOffsets = rowDefinitions.GetRowKeyOffsets();

				if (rowDefinitions.Any(rowDefinition => !string.IsNullOrWhiteSpace(rowDefinition.RowOptions.HeaderCaption)))
				{
					foreach (var rowDefinition in rowDefinitions.Where(rowDefinition => rowDefinition.RowOptions.GetHeaderStyle(workbook) != null))
					{
						_worksheetCells[startRow + rowDefinition.RowOffset, startColumn].SetStyle(rowDefinition.RowOptions.GetHeaderStyle(workbook).GetAsposeStyle());
					}
					foreach (var rowDefinition in rowDefinitions.Where(rowDefinition => !string.IsNullOrWhiteSpace(rowDefinition.RowOptions.HeaderCaption)))
					{
						_worksheetCells[startRow + rowDefinition.RowOffset, startColumn].Value = rowDefinition.RowOptions.HeaderCaption;
					}
				}

				response.StartRow = startRow + rowDefinitions.Min(rowDefinition => rowDefinition.RowOffset);
				response.StopRow = startRow + rowDefinitions.Max(rowDefinition => rowDefinition.RowOffset);
				response.StartColumn = startColumn;
				response.StopColumn = startColumn + valueColumnOffset;

				var replacements = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
				foreach (var rowDefinition in rowDefinitions)
				{
					var key = string.Format("[Row-{0}]", rowDefinition.RowOptions.RowName);
					if (!replacements.ContainsKey(key))
					{
						replacements.Add(key, ISI.Extensions.SpreadSheets.Coordinates.RowKey(startRow + rowDefinition.RowOffset));
					}
				}
				replacements.Add("[StartRow]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(response.StartRow));
				replacements.Add("[StopRow]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(response.StopRow));
				replacements.Add("[StartColumn]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(response.StartColumn));
				replacements.Add("[StopColumn]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(response.StopColumn));

				foreach (var rowDefinition in rowDefinitions)
				{
					this[startRow + rowDefinition.RowOffset, startColumn + valueColumnOffset].UpdateCell(cell =>
					{
						var asposeCell = cell.GetAsposeCell();

						if (rowDefinition.RowOptions.GetStyle(workbook) != null)
						{
							asposeCell.SetStyle(rowDefinition.RowOptions.GetStyle(workbook).GetAsposeStyle());
						}

						var formula = rowDefinition.RowOptions.Formula;
						if (string.IsNullOrWhiteSpace(formula))
						{
							if (!rowDefinition.IsNull(summary))
							{
								asposeCell.Value = rowDefinition.GetValue(summary);
							}
						}
						else
						{
							formula = formula.Replace(new Dictionary<string, string>()
							{
								{"[Row]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(startRow + rowDefinition.RowOffset)},
								{"[Column]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(startColumn + valueColumnOffset)},
							}).Replace(replacements);

							asposeCell.Formula = rowDefinitions.BuildFormula(rowKeyOffsets, startColumn, startRow + rowDefinition.RowOffset, startColumn + valueColumnOffset, formula);
						}
					});
				}

				return response;
			}
		}
	}
}