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

namespace ISI.Extensions.Documents
{
	public delegate IDocumentDataSource DocumentDataCreator(string tableName, object model, IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType);

	public abstract class DocumentData
	{
		protected IDictionary<Type, DocumentDataCreator> DocumentDataCreatorsByType { get; }

		protected DocumentData(IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType)
		{
			DocumentDataCreatorsByType = documentDataCreatorsByType ?? new Dictionary<Type, DocumentDataCreator>();
		}

		#region DocumentDataCreatorsByTableName
		protected abstract IDictionary<string, Func<object, IDocumentDataSource>> DocumentDataCreatorsByTableName { get; }

		protected virtual IEnumerable<KeyValuePair<string, Func<object, IDocumentDataSource>>> GetDocumentDataCreatorsByTableName()
		{
			return GetDocumentDataCreatorsByTableName(this.GetType());
		}

		protected virtual IEnumerable<KeyValuePair<string, Func<object, IDocumentDataSource>>> GetDocumentDataCreatorsByTableName(Type modelType)
		{
			var documentDataCreatorsByTableName = new Dictionary<string, Func<object, IDocumentDataSource>>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var propertyInfo in modelType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty))
			{
				var documentDataSourceAttribute = propertyInfo.GetCustomAttributes(typeof(DocumentDataSourceAttribute), true).OfType<DocumentDataSourceAttribute>().FirstOrDefault();
				if (documentDataSourceAttribute != null)
				{
					var tableName = (string.IsNullOrWhiteSpace(documentDataSourceAttribute.TableName) ? propertyInfo.Name : documentDataSourceAttribute.TableName);

					if (!documentDataCreatorsByTableName.ContainsKey(tableName))
					{
						var documentDataSourceModelType = propertyInfo.PropertyType.GetElementType();

						if (DocumentDataCreatorsByType.TryGetValue(documentDataSourceModelType, out var documentDataCreator))
						{
							documentDataCreatorsByTableName.Add(tableName, model => documentDataCreator(tableName, model, DocumentDataCreatorsByType));
						}
						else
						{
							documentDataCreatorsByTableName.Add(tableName, model => Activator.CreateInstance(typeof(DocumentDataSource<>).MakeGenericType(documentDataSourceModelType), tableName, propertyInfo.GetValue(model), DocumentDataCreatorsByType) as IDocumentDataSource);
						}
					}
				}

				var documentDataSourceValuesAttribute = propertyInfo.GetCustomAttributes(typeof(DocumentDataSourceValuesAttribute), true).OfType<DocumentDataSourceValuesAttribute>().FirstOrDefault();
				if (documentDataSourceValuesAttribute != null)
				{
					var tableName = (string.IsNullOrWhiteSpace(documentDataSourceValuesAttribute.TableName) ? propertyInfo.Name : documentDataSourceValuesAttribute.TableName);
					var fieldName = (string.IsNullOrWhiteSpace(documentDataSourceValuesAttribute.FieldName) ? propertyInfo.Name : documentDataSourceValuesAttribute.FieldName);

					if (!documentDataCreatorsByTableName.ContainsKey(tableName))
					{
						var documentDataSourceModelType = propertyInfo.PropertyType.GetElementType();

						if (DocumentDataCreatorsByType.TryGetValue(documentDataSourceModelType, out var documentDataCreator))
						{
							documentDataCreatorsByTableName.Add(tableName, model => documentDataCreator(tableName, model, DocumentDataCreatorsByType));
						}
						else
						{
							documentDataCreatorsByTableName.Add(tableName, model => Activator.CreateInstance(typeof(DocumentDataSourceValues<>).MakeGenericType(documentDataSourceModelType), tableName, fieldName, propertyInfo.GetValue(model), DocumentDataCreatorsByType) as IDocumentDataSource);
						}
					}
				}
			}

			return documentDataCreatorsByTableName;
		}
		#endregion

		#region GetChildDataSource
		public virtual IDocumentDataSource GetChildDataSource(object model, string tableName)
		{
			if (DocumentDataCreatorsByTableName.TryGetValue(tableName, out var documentDataSourceCreator))
			{
				return documentDataSourceCreator(model);
			}

			throw new NotImplementedException();
		}

		public abstract IDocumentDataSource GetChildDataSource(string tableName);
		#endregion

		#region ValueGettersByFieldName
		protected abstract IDictionary<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>> ValueGettersByFieldName { get; }

		protected virtual IEnumerable<KeyValuePair<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>>> GetValueGettersByFieldName()
		{
			return GetValueGettersByFieldName(this.GetType());
		}

		protected virtual IEnumerable<KeyValuePair<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>>> GetValueGettersByFieldName(Type modelType)
		{
			var valueGettersByFieldName = new Dictionary<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var propertyInfo in modelType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty))
			{
				var documentDataValueAttribute = propertyInfo.GetCustomAttributes(typeof(DocumentDataValueAttribute), true).OfType<DocumentDataValueAttribute>().FirstOrDefault();
				if (documentDataValueAttribute != null)
				{
					var fieldName = (string.IsNullOrWhiteSpace(documentDataValueAttribute.FieldName) ? propertyInfo.Name : documentDataValueAttribute.FieldName);

					if (!valueGettersByFieldName.ContainsKey(fieldName))
					{
						if (propertyInfo.PropertyType == typeof(string))
						{
							valueGettersByFieldName.Add(fieldName, model => new ISI.Extensions.Documents.DocumentDataValue((propertyInfo.GetValue(model) as string) ?? string.Empty));
						}
						else if (propertyInfo.PropertyType == typeof(byte[]))
						{
							valueGettersByFieldName.Add(fieldName, model => new ISI.Extensions.Documents.DocumentDataImageValue((propertyInfo.GetValue(model) as byte[])));
						}
						else if (propertyInfo.PropertyType == typeof(System.IO.Stream))
						{
							valueGettersByFieldName.Add(fieldName, model =>
							{
								var stream = propertyInfo.GetValue(model) as System.IO.Stream;
								var documentDataImageValue = new ISI.Extensions.Documents.DocumentDataImageValue((propertyInfo.GetValue(model) as byte[]));
								stream?.Dispose();
								return documentDataImageValue;
							});
						}
						else if (propertyInfo.PropertyType.IsGenericType && (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
						{
							valueGettersByFieldName.Add(fieldName, model =>
							{
								var value = propertyInfo.GetValue(model);

								if (value == null)
								{
									return new ISI.Extensions.Documents.DocumentDataValue(string.Empty);
								}

								return new ISI.Extensions.Documents.DocumentDataValue(value);
							});
						}
						else
						{
							valueGettersByFieldName.Add(fieldName, model => new ISI.Extensions.Documents.DocumentDataValue(propertyInfo.GetValue(model)));
						}
					}
				}
			}

			return valueGettersByFieldName;
		}
		#endregion

		#region GetValue
		protected virtual ISI.Extensions.Documents.IDocumentDataValue GetValue(object model, string fieldName)
		{
			if (ValueGettersByFieldName.TryGetValue(fieldName, out var valueGetter))
			{
				return valueGetter(model);
			}

			return new ISI.Extensions.Documents.DocumentDataValue();
		}

		public abstract ISI.Extensions.Documents.IDocumentDataValue GetValue(string fieldName);
		#endregion
	}
}
