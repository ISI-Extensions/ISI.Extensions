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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public partial class WorksheetCells
		{
			public ISI.Extensions.SpreadSheets.AddRecordsResponse AddRecords<TRecord>(IEnumerable<TRecord> records, int startRow, int startColumn, ISI.Extensions.SpreadSheets.AddRecordsColumnCollection<TRecord> columnDefinitions = null)
			{
				var response = new ISI.Extensions.SpreadSheets.AddRecordsResponse();

				columnDefinitions ??= ISI.Extensions.SpreadSheets.AddRecordsColumnCollection<TRecord>.BuildDefault(_worksheet._worksheets._workbook);

				if (columnDefinitions.Any(columnDefinition => columnDefinition.ColumnOffset < 0))
				{
					var columnOffset = columnDefinitions.Max(columnDefinition => columnDefinition.ColumnOffset) + 1;

					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => columnDefinition.ColumnOffset < 0).OfType<ISI.Extensions.SpreadSheets.ISetColumnOffset>())
					{
						columnDefinition.ColumnOffset = columnOffset++;
					}
				}

				var workbook = _worksheet._worksheets._workbook;

				var columnKeyOffsets = columnDefinitions.GetColumnKeyOffsets();

				var row = startRow;

				foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => columnDefinition.ColumnOptions.Width.HasValue))
				{
					_worksheetCells.Columns[startColumn + columnDefinition.ColumnOffset].Width = columnDefinition.ColumnOptions.Width.GetValueOrDefault();
				}

				if (columnDefinitions.Any(columnDefinition => !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.HeaderCaption)))
				{
					response.HeaderRow = row;

					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => columnDefinition.ColumnOptions.GetHeaderStyle(workbook) != null))
					{
						_worksheetCells[row, startColumn + columnDefinition.ColumnOffset].SetStyle(columnDefinition.ColumnOptions.GetHeaderStyle(workbook).GetAsposeStyle());
					}
					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.HeaderCaption)))
					{
						_worksheetCells[row, startColumn + columnDefinition.ColumnOffset].Value = columnDefinition.ColumnOptions.HeaderCaption;
					}

					row++;
				}

				response.StartRow = row;
				response.StartColumn = startColumn + columnDefinitions.Min(column => column.ColumnOffset);
				response.StopColumn = startColumn + columnDefinitions.Max(column => column.ColumnOffset);

				var replacements = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
				foreach (var columnDefinition in columnDefinitions)
				{
					var key = string.Format("[Column-{0}]", columnDefinition.ColumnOptions.ColumnName);
					if (!replacements.ContainsKey(key))
					{
						replacements.Add(key, ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(startColumn + columnDefinition.ColumnOffset));
					}
				}
				replacements.Add("[StartRow]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(response.StartRow));
				replacements.Add("[StartColumn]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(response.StartColumn));
				replacements.Add("[StopColumn]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(response.StopColumn));

				if (records != null)
				{
					foreach (var record in records)
					{
						foreach (var columnDefinition in columnDefinitions)
						{
							this[row, startColumn + columnDefinition.ColumnOffset].UpdateCell(cell =>
							{
								var asposeCell = cell.GetAsposeCell();

								if (columnDefinition.ColumnOptions.GetStyle(workbook) != null)
								{
									asposeCell.SetStyle(columnDefinition.ColumnOptions.GetStyle(workbook).GetAsposeStyle());
								}

								if (!(record is ISI.Extensions.SpreadSheets.IAddRecordsSkipRow))
								{
									if (columnDefinition.ColumnOptions.IsImage)
									{
										var rawValue = columnDefinition.GetValue(record);
										if (rawValue != null)
										{
											switch (rawValue)
											{
												case byte[] valueBytes:
													_worksheet.Pictures.Add(asposeCell.Row, asposeCell.Column, new System.IO.MemoryStream(valueBytes));
													break;
												case System.IO.Stream valueStream:
													_worksheet.Pictures.Add(asposeCell.Row, asposeCell.Column, valueStream);
													break;
											}
										}
									}
									else if (string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.Formula))
									{
										if (!columnDefinition.IsNull(record))
										{
											asposeCell.Value = columnDefinition.GetValue(record);
										}
									}
									else 
									{
										var formula = columnDefinition.ColumnOptions.Formula.Replace(new Dictionary<string, string>()
										{
										{"[Row]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(row)},
										{"[Column]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(startColumn + columnDefinition.ColumnOffset)},
										}).Replace(replacements);

										asposeCell.Formula = columnDefinitions.BuildFormula(columnKeyOffsets, startColumn, row, startColumn + columnDefinition.ColumnOffset, formula);
									}
								}
							});
						}

						row++;
					}
				}

				response.StopRow = row - 1;

				if (columnDefinitions.Any(columnDefinition => !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.FooterCaption) || !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.FooterFormula)))
				{
					response.FooterRow = row;

					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => columnDefinition.ColumnOptions.GetFooterStyle(workbook) != null))
					{
						_worksheetCells[row, startColumn + columnDefinition.ColumnOffset].SetStyle(columnDefinition.ColumnOptions.GetFooterStyle(workbook).GetAsposeStyle());
					}
					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.FooterCaption)))
					{
						_worksheetCells[row, startColumn + columnDefinition.ColumnOffset].Value = columnDefinition.ColumnOptions.FooterCaption;
					}
					foreach (var columnDefinition in columnDefinitions.Where(columnDefinition => !string.IsNullOrWhiteSpace(columnDefinition.ColumnOptions.FooterFormula)))
					{
						var formula = columnDefinition.ColumnOptions.FooterFormula;

						formula = formula.Replace(new Dictionary<string, string>()
						{
							{"[Row]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(row)},
							{"[Column]", ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(startColumn + columnDefinition.ColumnOffset)},
							{"[StopRow]", ISI.Extensions.SpreadSheets.Coordinates.RowKey(response.StopRow)},
						}).Replace(replacements);

						_worksheetCells[row, startColumn + columnDefinition.ColumnOffset].Formula = columnDefinitions.BuildFormula(columnKeyOffsets, startColumn, row, startColumn + columnDefinition.ColumnOffset, formula);
					}
				}

				return response;
			}
		}
	}
}