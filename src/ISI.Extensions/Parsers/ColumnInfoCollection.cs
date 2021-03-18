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

namespace ISI.Extensions.Parsers
{
	public class ColumnInfoCollection<TRecord> : List<IColumnInfo<TRecord>>
		where TRecord : class, new()
	{
		private static ISI.Extensions.DataContract.DataMemberPropertyInfo[] properties = null;

		static ColumnInfoCollection()
		{
			var type = typeof(TRecord);

			if (type.IsDefined(typeof(System.Runtime.Serialization.DataContractAttribute), false))
			{
				properties = ISI.Extensions.DataContract.GetDataMemberPropertyInfos(type).OrderBy(property => property.Order).ToArray();
			}
			else
			{
				properties = type.GetProperties().Where(propertyInfo => propertyInfo.CanRead).Select(property => new ISI.Extensions.DataContract.DataMemberPropertyInfo(new System.Runtime.Serialization.DataMemberAttribute() { Name = property.Name }, property, false)).OrderBy(property => property.Order).ToArray();
			}
		}

		public static ColumnInfoCollection<TRecord> GetDefault()
		{
			var result = new ColumnInfoCollection<TRecord>();

			foreach (var property in properties)
			{
				result.Add(Activator.CreateInstance(typeof(ISI.Extensions.Parsers.ColumnInfo<,>).MakeGenericType(typeof(TRecord), property.PropertyInfo.PropertyType), new object[] {new[] {property.DataMemberAttribute.Name}, property.PropertyInfo, null, null}, null) as IColumnInfo<TRecord>);
			}

			return result;
		}

		public ColumnInfoCollection()
		{
			
		}

		public ColumnInfoCollection(IEnumerable<IColumnInfo<TRecord>> columns)
			: base(columns)
		{
			
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(null, property, null, null));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<object, TProperty> transformValue)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(null, property, transformValue, null));
		}

		public void Add<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<TRecord, string> formattedValue)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(null, property, null, formattedValue));
		}


		public void Add<TProperty>(string columnName, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(new[] { columnName }, property, null, null));
		}

		public void Add<TProperty>(string columnName, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<object, TProperty> transformValue)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(new[] { columnName }, property, transformValue, null));
		}


		public void Add<TProperty>(IEnumerable<string> columnNames, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(columnNames, property, null, null));
		}

		public void Add<TProperty>(IEnumerable<string> columnNames, System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property, Func<object, TProperty> transformValue)
		{
			base.Add(new ColumnInfo<TRecord, TProperty>(columnNames, property, transformValue, null));
		}
	}
}