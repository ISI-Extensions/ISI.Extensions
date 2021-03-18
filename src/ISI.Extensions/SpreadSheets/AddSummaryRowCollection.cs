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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.SpreadSheets
{
	public class AddSummaryRowCollection<TSummary> : List<IAddSummaryRow<TSummary>>
	{
		private static System.Text.RegularExpressions.Regex _formulaRegex = new System.Text.RegularExpressions.Regex("(?:{)(?<definition>(?:row)|(?:row))(?:(?<sign>[-\\+])(?<offset>\\d+))?(?:})*");

		private int GetNextRowOffset()
		{
			if (this.Any())
			{
				return this.Max(row => row.RowOffset) + 1;
			}

			return 0;
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, Func<TProperty, object> transform, AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(GetNextRowOffset(), property, transform, rowOptions));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, Func<TProperty, object> transform)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(GetNextRowOffset(), property, transform, null));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(GetNextRowOffset(), property, null, rowOptions));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(GetNextRowOffset(), property, null, null));
		}

		public void Add<TProperty>(AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary>(GetNextRowOffset(), null, null, rowOptions));
		}





		public void Add<TProperty>(int rowOffset, System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, Func<TProperty, object> transform, AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(rowOffset, property, transform, rowOptions));
		}

		public void Add<TProperty>(int rowOffset, System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, Func<TProperty, object> transform)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(rowOffset, property, transform, null));
		}

		public void Add<TProperty>(int rowOffset, System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(rowOffset, property, null, rowOptions));
		}

		public void Add<TProperty>(int rowOffset, System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property)
		{
			base.Add(new AddSummaryRow<TSummary, TProperty>(rowOffset, property, null, null));
		}

		public void Add<TProperty>(int rowOffset, AddSummaryRowOptions rowOptions)
		{
			base.Add(new AddSummaryRow<TSummary>(rowOffset, null, null, rowOptions));
		}



		public virtual IDictionary<string, int> GetRowKeyOffsets()
		{
			var rowKeyOffsets = new Dictionary<string, int>();

			foreach (var row in this)
			{
				rowKeyOffsets.Add(string.Format("{{{0}}}", row.RowOffset), row.RowOffset);

				if (!string.IsNullOrWhiteSpace(row.RowOptions.RowName))
				{
					var rowName = string.Format("{{{0}}}", row.RowOptions.RowName);

					if (!rowKeyOffsets.ContainsKey(rowName))
					{
						rowKeyOffsets.Add(rowName, row.RowOffset);
					}
				}
			}

			return rowKeyOffsets;
		}

		public virtual string BuildFormula(IDictionary<string, int> rowKeyOffsets, int startColumn, int currentRow, int currentColumn, string formula)
		{
			foreach (var rowKeyOffset in rowKeyOffsets)
			{
				var columnKey = string.Format("{0}", startColumn + rowKeyOffset.Value);

				formula = formula.Replace(rowKeyOffset.Key, columnKey);
			}

			var formulaMatch = _formulaRegex.Match(formula);

			while (formulaMatch.Success)
			{
				var key = formulaMatch.Value;

				var definintion = formulaMatch.Groups["definition"].Value;
				var sign = formulaMatch.Groups["sign"].Value;
				var offset = formulaMatch.Groups["offset"].Value.ToInt();

				if (string.Equals(definintion, "column", StringComparison.InvariantCultureIgnoreCase))
				{
					var columnKey = ISI.Extensions.SpreadSheets.Coordinates.ColumnKey(currentColumn + offset * (string.Equals(sign, "-", StringComparison.CurrentCultureIgnoreCase) ? -1 : 1));

					formula = formula.Replace(key, columnKey);
				}
				else if (string.Equals(definintion, "row", StringComparison.InvariantCultureIgnoreCase))
				{
					var rowKey = string.Format("{0}", currentRow + 1 + offset * (string.Equals(sign, "-", StringComparison.CurrentCultureIgnoreCase) ? -1 : 1));

					formula = formula.Replace(key, rowKey);
				}

				formulaMatch = formulaMatch.NextMatch();
			}

			return formula;
		}

		public static AddSummaryRowCollection<TSummary> BuildDefault(ISI.Extensions.SpreadSheets.IWorkbook workbook)
		{
			var rows = new AddSummaryRowCollection<TSummary>();

			foreach (var propertyInfo in typeof(TSummary).GetProperties())
			{
				if (!propertyInfo.GetCustomAttributes(typeof(ISI.Extensions.SpreadSheets.IgnoreRowAttribute), true).Any())
				{
					var row = AddSummaryRowOptions.GetAddSummaryRowOptions(propertyInfo);

					rows.Add(new AddSummaryRow<TSummary>(row.RowOffset, propertyInfo.PropertyType, record => propertyInfo.GetValue(record), row.AddSummaryRowOptions));
				}
			}

			return rows;
		}
	}
}
