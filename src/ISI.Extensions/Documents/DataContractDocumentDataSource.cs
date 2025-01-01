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
using System.Runtime.Serialization;

namespace ISI.Extensions.Documents
{
	public class DataContractDocumentDataSource<TModel> : ISI.Extensions.Documents.IDocumentDataSource
	{
		private static IDictionary<string, System.Reflection.PropertyInfo> _properties = null;

		private static bool _recordHasPropertyBag = false;

		static DataContractDocumentDataSource()
		{
			var type = typeof(TModel);

			_properties = new Dictionary<string, System.Reflection.PropertyInfo>(StringComparer.InvariantCultureIgnoreCase);
			_recordHasPropertyBag = type.GetInterfaces().Contains(typeof(ISI.Extensions.DataReader.IRecordHasPropertyBag));

			if (type.IsDefined(typeof(DataContractAttribute), false))
			{
				var properties = ISI.Extensions.DataContract.GetDataMemberPropertyInfos(type);

				foreach (var property in properties)
				{
					_properties.Add(property.PropertyName, property.PropertyInfo);
				}
			}
			else
			{
				var properties = type.GetProperties();

				foreach (var property in properties)
				{
					if (property.CanRead)
					{
						_properties.Add(property.Name, property);
					}
				}
			}
		}

		private IEnumerator<TModel> _enumerator = null;

		public string TableName { get; }

		public DataContractDocumentDataSource(string tableName, IEnumerable<TModel> items)
		{
			TableName = tableName;
			_enumerator = items.GetEnumerator();
		}

		public bool MoveNext()
		{
			return _enumerator.MoveNext();

		}

		public ISI.Extensions.Documents.IDocumentDataValue GetValue(string fieldName)
		{
			if (_recordHasPropertyBag)
			{
				if (_enumerator.Current is ISI.Extensions.DataReader.IRecordHasPropertyBag record)
				{
					if (record.PropertyBag.TryGetValue(fieldName, out var fieldValue))
					{
						return new ISI.Extensions.Documents.DocumentDataValue(fieldValue);
					}
				}
			}

			if (_properties.TryGetValue(fieldName, out var propertyInfo))
			{
				return new ISI.Extensions.Documents.DocumentDataValue(propertyInfo.GetValue(_enumerator.Current));
			}

			return new ISI.Extensions.Documents.DocumentDataValue();
		}

		public virtual ISI.Extensions.Documents.IDocumentDataSource GetChildDataSource(string tableName)
		{
			throw new NotImplementedException();
		}
	}
}
