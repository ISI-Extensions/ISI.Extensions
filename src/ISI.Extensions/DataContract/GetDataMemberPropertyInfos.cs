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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class DataContract
	{
		public static DataMemberPropertyInfo[] GetDataMemberPropertyInfos<TRecord>()
		{
			return GetDataMemberPropertyInfos(typeof (TRecord));
		}

		public static DataMemberPropertyInfo[] GetDataMemberPropertyInfos(Type type)
		{
			if (!_dataContractPropertyInfos.ContainsKey(type))
			{
				if (type.IsDefined(typeof (System.Runtime.Serialization.DataContractAttribute), false))
				{
					var properties = new List<DataMemberPropertyInfo>();

					foreach (var propertyInfo in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty))
					{
						var ignoreDataMemberAttribute = propertyInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.IgnoreDataMemberAttribute), false).FirstOrDefault() as System.Runtime.Serialization.IgnoreDataMemberAttribute;
						if (ignoreDataMemberAttribute == null)
						{
							var dataMemberAttribute = propertyInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.DataMemberAttribute), false).FirstOrDefault() as System.Runtime.Serialization.DataMemberAttribute;
							var isKey = propertyInfo.GetCustomAttributes(typeof(System.Runtime.Serialization.DataContractMemberKeyAttribute), false).Any();

							if (dataMemberAttribute != null)
							{
								var property = new DataMemberPropertyInfo(dataMemberAttribute, propertyInfo, isKey)
								{
									Order = properties.Count
								};
								properties.Add(property);
							}
						}
					}

					var index = 0;
					foreach (var property in properties.Where(p => p.DataMemberAttribute.Order > 0).OrderBy(p => p.DataMemberAttribute.Order))
					{
						property.Order = index++;
					}
					foreach (var property in properties.Where(p => p.DataMemberAttribute.Order <= 0).OrderBy(p => p.Order))
					{
						property.Order = index++;
					}

					_dataContractPropertyInfos.Add(type, properties.OrderBy(p => p.Order).ToArray());
				}
				else
				{
					throw new(string.Format("Type: \"{0}\" is not a DataContract", type.AssemblyQualifiedNameWithoutVersion()));
				}
			}

			return _dataContractPropertyInfos[type];
		}
	}
}
