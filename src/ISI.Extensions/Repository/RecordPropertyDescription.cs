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
	public class RecordPropertyDescription<TRecord> : IRecordPropertyDescription<TRecord>
	{
		public System.Reflection.PropertyInfo PropertyInfo { get; }
		public System.Runtime.Serialization.DataMemberAttribute DataMemberAttribute { get; }
		public RecordPropertyAttribute RecordPropertyAttribute { get; }
		public bool Ignore { get; private set; }
		public PrimaryKeyAttribute PrimaryKeyAttribute { get; }
		public RepositoryAssignedValueAttribute RepositoryAssignedValueAttribute { get; }

		public string ColumnName { get; protected set; }
		public int Order { get; internal set; } = -1;
		public virtual string PropertyName => PropertyInfo.Name;
		public virtual string PropertyType { get; protected set; }
		public int PropertySize { get; protected set; }
		public int? Precision { get; protected set; }
		public int? Scale { get; protected set; }
		public bool Nullable { get; protected set; }
		public RepositoryAssignedValueAttribute Default => RepositoryAssignedValueAttribute;
		public Type ValueType { get; protected set; }

		public bool IsPartOfPrimaryKey => (PrimaryKeyAttribute != null);

		public Func<TRecord, object> GetValue { get; protected set; }
		public Action<TRecord, object> SetValue { get; protected set; }
		public Func<TRecord, bool> IsNull { get; protected set; }

		public ISI.Extensions.Extensions.Converters.PropertyValueGetterSetterAttribute PropertyValueGetterSetterAttribute { get; protected set; }

		public bool CanBeSerialized { get; protected set; }

		public RecordPropertyDescription(System.Reflection.PropertyInfo propertyInfo, System.Attribute[] propertyAttributes, bool canBeSerialized)
		{
			PropertyInfo = propertyInfo;
			ColumnName = PropertyInfo.Name;

			ValueType = propertyInfo.PropertyType;
			GetValue = record => propertyInfo.GetValue(record);
			SetValue = (record, value) => propertyInfo.SetValue(record, value);

			PropertyValueGetterSetterAttribute = propertyAttributes.OfType<ISI.Extensions.Extensions.Converters.PropertyValueGetterSetterAttribute>().FirstOrDefault();
			if (PropertyValueGetterSetterAttribute != null)
			{
				ValueType = PropertyValueGetterSetterAttribute.PropertyValueType;
				GetValue = record => PropertyValueGetterSetterAttribute.GetPropertyValue(propertyInfo.GetValue(record));
				SetValue = (record, value) => PropertyValueGetterSetterAttribute.SetPropertyValue(record, value);
			}

			Nullable = ((ValueType.IsGenericType && (ValueType.GetGenericTypeDefinition() == typeof(Nullable<>))) || (ValueType == typeof(string)));

			IsNull = record =>
			{
				if (Nullable)
				{
					return (GetValue(record) == null);
				}

				return false;
			};

			DataMemberAttribute = propertyAttributes.OfType<System.Runtime.Serialization.DataMemberAttribute>().FirstOrDefault();
			if (DataMemberAttribute != null)
			{
				if (!string.IsNullOrEmpty(DataMemberAttribute.Name))
				{
					ColumnName = DataMemberAttribute.Name;
				}
				if (DataMemberAttribute.Order >= 0)
				{
					Order = DataMemberAttribute.Order;
				}
			}

			RecordPropertyAttribute = propertyAttributes.OfType<RecordPropertyAttribute>().FirstOrDefault();
			if (RecordPropertyAttribute != null)
			{
				if (!string.IsNullOrWhiteSpace(RecordPropertyAttribute.ColumnName))
				{
					ColumnName = RecordPropertyAttribute.ColumnName;
				}

				PropertyType = RecordPropertyAttribute.PropertyType;
				PropertySize = RecordPropertyAttribute.PropertySize;
				Precision = (RecordPropertyAttribute.Precision > 0 ? RecordPropertyAttribute.Precision : (int?)null);
				Scale = (RecordPropertyAttribute.Scale >= 0 ? RecordPropertyAttribute.Scale : (int?)null);
				if (RecordPropertyAttribute.NullableHasValue)
				{
					Nullable = RecordPropertyAttribute.Nullable;
				}
				if (RecordPropertyAttribute.Order >= 0)
				{
					Order = RecordPropertyAttribute.Order;
				}
			}

			PrimaryKeyAttribute = propertyAttributes.OfType<PrimaryKeyAttribute>().FirstOrDefault();
			if (PrimaryKeyAttribute != null)
			{
				Nullable = false;
			}

			RepositoryAssignedValueAttribute = propertyAttributes.OfType<RepositoryAssignedValueAttribute>().FirstOrDefault();

			if (propertyAttributes.OfType<IgnoreRecordPropertyAttribute>().Any())
			{
				Ignore = true;
			}



			CanBeSerialized = canBeSerialized;
		}

		public static RecordPropertyDescription<TRecord> GetArchiveDateTimeRecordPropertyDescription(string archiveTableArchiveDateTimeColumnName)
		{
			var propertyInfo = typeof(ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty).First(property => string.Equals(property.Name, nameof(ISI.Extensions.Repository.IRecordManagerRecordWithArchiveDateTime.ArchiveDateTimeUtc), StringComparison.CurrentCulture));

			var propertyAttributes = new System.Attribute[]
			{
				new ISI.Extensions.Repository.RecordPropertyAttribute()
				{
					ColumnName = archiveTableArchiveDateTimeColumnName,
				},
			};

			return new(propertyInfo, propertyAttributes, false);
		}
	}
}