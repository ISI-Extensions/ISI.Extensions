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

namespace ISI.Extensions.Documents
{
	public class DocumentDataSourceRoot<TModel> : DocumentData, IDocumentDataSourceRoot
	{
		protected override IDictionary<string, Func<object, IDocumentDataSource>> DocumentDataCreatorsByTableName => GetDocumentDataCreatorsByTableName().ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value, StringComparer.InvariantCultureIgnoreCase);

		private static IDictionary<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>> _valueGettersByFieldName = null;
		protected override IDictionary<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>> ValueGettersByFieldName => (_valueGettersByFieldName ??= GetValueGettersByFieldName().ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value, StringComparer.InvariantCultureIgnoreCase));

		protected string ModelsTableName { get; }

		protected IEnumerable<TModel> Models { get; }

		public DocumentDataSourceRoot(string modelsTableName, IEnumerable<TModel> models, IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType = null)
			: base(documentDataCreatorsByType)
		{
			Models = models;

			if (string.IsNullOrWhiteSpace(modelsTableName))
			{
				var documentDataAttribute = typeof(TModel).GetCustomAttributes(typeof(DocumentDataAttribute), true).OfType<DocumentDataAttribute>().FirstOrDefault();

				modelsTableName = documentDataAttribute?.TableName;
			}

			if (string.IsNullOrWhiteSpace(modelsTableName))
			{
				modelsTableName = typeof(TModel).Name;
			}

			ModelsTableName = modelsTableName;
		}
		public DocumentDataSourceRoot(IEnumerable<TModel> models, IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType = null)
			: this(string.Empty, models, documentDataCreatorsByType)
		{
		}
		public DocumentDataSourceRoot(string modelsTableName, TModel model, IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType = null)
			: this(modelsTableName, [model], documentDataCreatorsByType)
		{
		}
		public DocumentDataSourceRoot(TModel model, IDictionary<Type, DocumentDataCreator> documentDataCreatorsByType = null)
			: this(string.Empty, model, documentDataCreatorsByType)
		{
		}

		protected override IEnumerable<KeyValuePair<string, Func<object, IDocumentDataSource>>> GetDocumentDataCreatorsByTableName()
		{
			return
			[
				new KeyValuePair<string, Func<object, IDocumentDataSource>>(ModelsTableName, model => new DocumentDataSource<TModel>(ModelsTableName, Models, DocumentDataCreatorsByType))
			];
		}

		protected override IEnumerable<KeyValuePair<string, Func<object, ISI.Extensions.Documents.IDocumentDataValue>>> GetValueGettersByFieldName()
		{
			return GetValueGettersByFieldName(typeof(TModel));
		}

		public override ISI.Extensions.Documents.IDocumentDataValue GetValue(string fieldName)
		{
			return base.GetValue(Models, fieldName);
		}

		public override IDocumentDataSource GetChildDataSource(string tableName)
		{
			return base.GetChildDataSource(Models, tableName);
		}
	}
}
