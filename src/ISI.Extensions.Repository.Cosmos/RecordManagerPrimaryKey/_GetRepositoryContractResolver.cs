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
using Microsoft.Extensions.Configuration;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository.Cosmos
{
	public abstract partial class RecordManagerPrimaryKey<TRecord, TRecordPrimaryKey>
	{
		private static global::Newtonsoft.Json.Serialization.DefaultContractResolver _repositoryContractResolver = null;
		private static readonly object _repositoryContractResolverLock = new();

		protected global::Newtonsoft.Json.Serialization.DefaultContractResolver GetRepositoryContractResolver()
		{
			if (_repositoryContractResolver == null)
			{
				lock (_repositoryContractResolverLock)
				{
					_repositoryContractResolver ??= new RepositoryContractResolver();
				}
			}

			return _repositoryContractResolver;
		}


		public class RepositoryContractResolver : global::Newtonsoft.Json.Serialization.DefaultContractResolver
		{
			private static readonly IDictionary<string, string> _PropertyNames = new Dictionary<string, string>();

			static RepositoryContractResolver()
			{
				var recordDescription = ISI.Extensions.Repository.RecordDescription.GetRecordDescription<TRecord>();

				if (recordDescription.PrimaryKeyPropertyDescriptions.NullCheckedCount() != 1)
				{
					throw new("Record can only have one primary key");
				}

				foreach (var propertyDescription in recordDescription.PropertyDescriptions)
				{
					_PropertyNames.Add(propertyDescription.PropertyName, propertyDescription.ColumnName.ToCamelCase());
				}

				_PropertyNames[recordDescription.PrimaryKeyPropertyDescriptions.First().PropertyName] = "id";
			}

			protected override IList<global::Newtonsoft.Json.Serialization.JsonProperty> CreateProperties(Type type, global::Newtonsoft.Json.MemberSerialization memberSerialization)
			{
				var jsonProperties =  base.CreateProperties(type, memberSerialization);

				if (type == typeof(TRecord))
				{
					foreach (var jsonProperty in jsonProperties)
					{
						if (_PropertyNames.TryGetValue(jsonProperty.UnderlyingName, out var key))
						{
							jsonProperty.PropertyName = key;
						}
					}
				}

				return jsonProperties;
			}
		}
	}
}