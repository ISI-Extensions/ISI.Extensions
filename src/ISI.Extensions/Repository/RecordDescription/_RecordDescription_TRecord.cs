#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Repository
{
	public class RecordDescription<TRecord> : IRecordDescription<TRecord>
	{
		public string Schema { get; }
		public string TableName { get; }
		public bool HasLocalClusteringIndex { get; }

		public IDictionary<string, IRecordPropertyDescription<TRecord>> PropertyDescriptionLookup { get; }
		public IRecordPropertyDescription<TRecord>[] PropertyDescriptions { get; }
		public IRecordPropertyDescription<TRecord>[] PrimaryKeyPropertyDescriptions { get; }
		public IRecordPropertyDescription<TRecord>[] RepositoryAssignedValuePropertyDescriptions { get; }

		private RecordIndex<TRecord>[] _indexes = null;
		public RecordIndex<TRecord>[] Indexes => _indexes ??= (Activator.CreateInstance<TRecord>() as IRecordIndexDescriptions<TRecord>)?.GetRecordIndexes()?.ToArray() ?? [];

		public RecordDescription(
			string schema,
			string tableName,
			bool hasLocalClusteringIndex,
			IRecordPropertyDescription<TRecord>[] properties,
			RecordIndex<TRecord>[] indexes = null)
		{
			Schema = schema;
			TableName = tableName;
			HasLocalClusteringIndex = hasLocalClusteringIndex;

			PropertyDescriptionLookup = properties.ToDictionary(property => property.PropertyInfo.Name, property => property);
			PropertyDescriptions = properties.OrderBy(p => p.Order).ToArray();
			PrimaryKeyPropertyDescriptions = properties.Where(p => p.PrimaryKeyAttribute != null).OrderBy(p => p.PrimaryKeyAttribute.Order).ToArray();
			RepositoryAssignedValuePropertyDescriptions = properties.Where(p => p.RepositoryAssignedValueAttribute != null).OrderBy(p => p.Order).ToArray();

			_indexes = indexes;
		}

		public string GetColumnName<TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> property)
		{
			var propertyInfo = ISI.Extensions.Reflection.GetPropertyInfo(property);

			return PropertyDescriptionLookup[propertyInfo.Name].ColumnName;
		}
	}
}
