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
	public interface ISetRowOffset
	{
		int RowOffset { set; }
	}

	public interface IAddSummaryRow<TSummary>
	{
		int RowOffset { get; }
		Type RowType { get; }
		Func<TSummary, bool> IsNull { get; }
		object GetValue(TSummary record);
		AddSummaryRowOptions RowOptions { get; }
	}

	public class AddSummaryRow<TSummary> : IAddSummaryRow<TSummary>, ISetRowOffset
	{
		public int RowOffset { get; private set; }
		public Type RowType { get; }
		public Func<TSummary, bool> IsNull { get; }
		public Func<TSummary, object> GetValue { get; }

		public AddSummaryRowOptions RowOptions { get; }

		int ISetRowOffset.RowOffset { set => RowOffset = value; }

		public AddSummaryRow(int rowOffset, Type propertyType, Func<TSummary, object> getValue, AddSummaryRowOptions rowOptions)
		{
			RowOffset = rowOffset;
			RowType = propertyType;
			var isNullable = (RowType.IsGenericType && (RowType.GetGenericTypeDefinition() == typeof(Nullable<>)));

			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};

			GetValue = getValue;

			RowOptions = rowOptions ?? new AddSummaryRowOptions();
		}

		object IAddSummaryRow<TSummary>.GetValue(TSummary record)
		{
			return GetValue(record);
		}
	}

	public class AddSummaryRow<TSummary, TProperty> : IAddSummaryRow<TSummary>, ISetRowOffset
	{
		public int RowOffset { get; private set; }
		public Type RowType => typeof(TProperty);
		public Func<TSummary, bool> IsNull { get; }
		public Func<TSummary, TProperty> GetValue { get; }
		public Func<TProperty, object> Transform { get; }
		public string Formula { get; set; }

		public AddSummaryRowOptions RowOptions { get; }

		int ISetRowOffset.RowOffset { set => RowOffset = value; }

		public AddSummaryRow(int rowOffset, System.Linq.Expressions.Expression<Func<TSummary, TProperty>> property, Func<TProperty, object> transform, AddSummaryRowOptions rowOptions)
		{
			RowOffset = rowOffset;
			var rowType = typeof(TProperty);
			var isNullable = (rowType.IsGenericType && (rowType.GetGenericTypeDefinition() == typeof(Nullable<>)));

			IsNull = record =>
			{
				if (isNullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};

			GetValue = property.Compile();
			Transform = transform ?? (_ => _);

			var memberExpression = (System.Linq.Expressions.MemberExpression)property.Body;

			RowOptions = rowOptions ?? (memberExpression.Member is System.Reflection.PropertyInfo propertyInfo ? AddSummaryRowOptions.GetAddSummaryRowOptions(propertyInfo).AddSummaryRowOptions : new AddSummaryRowOptions());

			if (string.IsNullOrWhiteSpace(RowOptions.RowName))
			{
				RowOptions.RowName = (memberExpression.Member is System.Reflection.PropertyInfo ? memberExpression.Member.Name : RowOptions.RowName);
			}
		}

		object IAddSummaryRow<TSummary>.GetValue(TSummary record)
		{
			return Transform(GetValue(record));
		}
	}
}
