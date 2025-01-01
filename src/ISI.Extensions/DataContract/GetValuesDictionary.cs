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

namespace ISI.Extensions
{
	public partial class DataContract
	{
		public static IDictionary<string, string> GetStringValuesDictionary(Type type, object record)
		{
			var stringValues = new Dictionary<string, string>();

			var properties = GetDataMemberPropertyInfos(type);

			var keyValues = GetValuesDictionary(properties, record);

			foreach (var keyValue in keyValues)
			{
				var valueType = keyValue.Value.GetType();

				if (ISI.Extensions.Enum.IsEnum(valueType))
				{
					stringValues.Add(keyValue.Key, ISI.Extensions.Enum.GetAbbreviation(valueType, keyValue.Value));
				}
				else
				{
					stringValues.Add(keyValue.Key, string.Format("{0}", keyValue.Value));
				}
			}

			return stringValues;
		}

		public static IDictionary<string, object> GetValuesDictionary<TRecord>(TRecord record)
		{
			return GetValuesDictionary(typeof(TRecord), record);
		}

		public static IDictionary<string, object> GetValuesDictionary(Type type, object record)
		{
			var properties = GetDataMemberPropertyInfos(type);

			return GetValuesDictionary(properties, record);
		}

		private static IDictionary<string, object> GetValuesDictionary(DataMemberPropertyInfo[] properties, object record)
		{
			return properties
				.Where(property => property.PropertyInfo.CanRead)
				.OrderBy(property => property.Order)
				.ToDictionary(property => property.DataMemberAttribute.Name, property => property.PropertyInfo.GetValue(record, null));
		}
	}
}
