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

namespace ISI.Extensions.SpreadSheets
{
	public class AddRecordsColumnCollection<TRecord> : List<IAddRecordsColumn<TRecord>>
	{
		private static System.Text.RegularExpressions.Regex _formulaRegex = new("(?:{)(?<definition>(?:row)|(?:column))(?:(?<sign>[-\\+])(?<offset>\\d+))?(?:})*");

		private int GetNextColumnOffset()
		{
			if (this.Any())
			{
				return this.Max(column => column.ColumnOffset) + 1;
			}

			return 0;
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TProperty, object> transform, AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(GetNextColumnOffset(), property, transform, columnOptions));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TProperty, object> transform)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(GetNextColumnOffset(), property, transform, null));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(GetNextColumnOffset(), property, null, columnOptions));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(GetNextColumnOffset(), property, null, null));
		}

		public void Add<TProperty>(AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord>(GetNextColumnOffset(), null, null, columnOptions));
		}





		public void Add<TProperty>(int columnOffset, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TProperty, object> transform, AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(columnOffset, property, transform, columnOptions));
		}

		public void Add<TProperty>(int columnOffset, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TProperty, object> transform)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(columnOffset, property, transform, null));
		}

		public void Add<TProperty>(int columnOffset, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(columnOffset, property, null, columnOptions));
		}

		public void Add<TProperty>(int columnOffset, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			base.Add(new AddRecordsColumn<TRecord, TProperty>(columnOffset, property, null, null));
		}

		public void Add<TProperty>(int columnOffset, AddRecordsColumnOptions columnOptions)
		{
			base.Add(new AddRecordsColumn<TRecord>(columnOffset, null, null, columnOptions));
		}



		public virtual IDictionary<string, int> GetColumnKeyOffsets()
		{
			var columnKeyOffsets = new Dictionary<string, int>();

			foreach (var column in this)
			{
				columnKeyOffsets.Add($"{{{column.ColumnOffset}}}", column.ColumnOffset);

				if (!string.IsNullOrWhiteSpace(column.ColumnOptions.ColumnName))
				{
					var columnName = $"{{{column.ColumnOptions.ColumnName}}}";

					if (!columnKeyOffsets.ContainsKey(columnName))
					{
						columnKeyOffsets.Add(columnName, column.ColumnOffset);
					}
				}
			}

			return columnKeyOffsets;
		}

		public virtual string BuildFormula(IDictionary<string, int> columnKeyOffsets, int startColumn, int currentRow, int currentColumn, string formula)
		{
			foreach (var columnKeyOffset in columnKeyOffsets)
			{
				var columnKey = ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(startColumn + columnKeyOffset.Value);

				formula = formula.Replace(columnKeyOffset.Key, columnKey);
			}

			var formulaMatch = _formulaRegex.Match(formula);

			while (formulaMatch.Success)
			{
				var key = formulaMatch.Value;

				var definition = formulaMatch.Groups["definition"].Value;
				var sign = formulaMatch.Groups["sign"].Value;
				var offset = formulaMatch.Groups["offset"].Value.ToInt();

				if (string.Equals(definition, "column", StringComparison.InvariantCultureIgnoreCase))
				{
					var columnKey = ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(currentColumn + offset * (string.Equals(sign, "-", StringComparison.CurrentCultureIgnoreCase) ? -1 : 1));

					formula = formula.Replace(key, columnKey);
				}
				else if (string.Equals(definition, "row", StringComparison.InvariantCultureIgnoreCase))
				{
					var rowKey = $"{currentRow + 1 + offset * (string.Equals(sign, "-", StringComparison.CurrentCultureIgnoreCase) ? -1 : 1)}";

					formula = formula.Replace(key, rowKey);
				}

				formulaMatch = formulaMatch.NextMatch();
			}

			return formula;
		}

		public static AddRecordsColumnCollection<TRecord> BuildDefault(ISI.Extensions.SpreadSheets.IWorkbook workbook)
		{
			var columns = new AddRecordsColumnCollection<TRecord>();

			foreach (var propertyInfo in typeof(TRecord).GetProperties())
			{
				if (!propertyInfo.GetCustomAttributes(typeof(ISI.Extensions.SpreadSheets.IgnoreColumnAttribute), true).Any())
				{
					var column = AddRecordsColumnOptions.GetAddRecordsColumnOptions(propertyInfo);

					columns.Add(new AddRecordsColumn<TRecord>(column.ColumnOffset, propertyInfo.PropertyType, record => propertyInfo.GetValue(record), column.AddRecordsColumnOptions));
				}
			}

			return columns;
		}
	}
}
