#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public partial class DataContract
	{
		public class DataMemberPropertyInfo
		{
			public System.Runtime.Serialization.DataMemberAttribute DataMemberAttribute { get; }
			public System.Reflection.PropertyInfo PropertyInfo { get; }
			public bool IsKey { get; }

			public DataMemberPropertyInfo(System.Runtime.Serialization.DataMemberAttribute dataMemberAttribute, System.Reflection.PropertyInfo propertyInfo, bool isKey)
			{
				DataMemberAttribute = dataMemberAttribute;
				PropertyInfo = propertyInfo;
				IsKey = isKey;
			}

			public T GetDefaultGeneric<T>()
			{
				return default;
			}

			private bool _defaultValueIsGenerated = false;
			private object _defaultValue;
			public object DefaultValue
			{
				get
				{
					if (!_defaultValueIsGenerated)
					{
						_defaultValue = this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(PropertyInfo.GetType()).Invoke(this, null);
						_defaultValueIsGenerated = true;
					}

					return _defaultValue;
				}
			}

			public int Order { get; set; }

			private string _propertyName = null;
			public string PropertyName => (_propertyName ?? (string.IsNullOrEmpty(DataMemberAttribute.Name) ? PropertyInfo.Name : DataMemberAttribute.Name));
		}
	}
}
