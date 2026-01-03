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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository
{
	public partial class RecordDescription
	{
		private static IRecordDescription BuildRecordDescription<TRecord>()
		{
			var recordType = typeof(TRecord);

			var schema = string.Empty;
			var tableName = recordType.Name;
			var hasLocalClusteringIndex = false;

			var attributes = recordType.GetCustomAttributes().ToNullCheckedArray(NullCheckCollectionResult.Empty);

			var recordAttribute = attributes.OfType<RecordAttribute>().FirstOrDefault();
			if (recordAttribute != null)
			{
				schema = recordAttribute.Schema;
				tableName = recordAttribute.TableName;
				hasLocalClusteringIndex = recordAttribute.HasLocalClusteringIndex;
			}

			var propertyInfos = recordType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty);

			var typesWithSerializerContractUuid = GetTypesWithSerializerContractUuid();

			var properties = propertyInfos.Select(propertyInfo => new RecordPropertyDescription<TRecord>(propertyInfo, propertyInfo.GetCustomAttributes().ToNullCheckedArray(NullCheckCollectionResult.Empty), typesWithSerializerContractUuid.Contains(propertyInfo.PropertyType))).Where(property => !property.Ignore).ToArray();

			var primaryKeyProperties = properties.Where(p => p.PrimaryKeyAttribute != null).ToArray();

			if (properties.Any())
			{
				var maxOrder = properties.Max(property => property.Order);
				maxOrder = (maxOrder < 0 ? 0 : maxOrder + 1);
				foreach (var property in properties)
				{
					if (property.Order < 0)
					{
						property.Order = maxOrder++;
					}
				}
			}

			if (primaryKeyProperties.Any())
			{
				var maxOrder = primaryKeyProperties.Max(property => property.PrimaryKeyAttribute.Order);
				maxOrder = (maxOrder < 0 ? 0 : maxOrder + 1);
				foreach (var property in properties.Where(p => p.PrimaryKeyAttribute != null))
				{
					if (property.PrimaryKeyAttribute.Order < 0)
					{
						property.PrimaryKeyAttribute.Order = maxOrder++;
					}
				}
			}

			return new RecordDescription<TRecord>(schema, tableName, hasLocalClusteringIndex, properties.Cast<IRecordPropertyDescription<TRecord>>().ToArray());
		}
	}
}