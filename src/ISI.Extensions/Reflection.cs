#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions
{
	public class Reflection
	{
		public static System.Reflection.PropertyInfo GetPropertyInfo<TRecord, TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> getProperty)
		{
			if (!(getProperty.Body is System.Linq.Expressions.MemberExpression member))
			{
				throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", getProperty.ToString()));
			}

			if (!(member.Member is System.Reflection.PropertyInfo propertyInfo))
			{
				throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", getProperty.ToString()));
			}

			return propertyInfo;
		}

		public static string GetPropertyName<TRecord, TProperty>(System.Linq.Expressions.Expression<Func<TRecord, TProperty>> expression, bool returnRandomNameIfNotFound = false)
		{
			//if (expression.Body is System.Linq.Expressions.UnaryExpression unaryExpression)
			//{
			//	var member = ((System.Linq.Expressions.MemberExpression)unaryExpression.Operand).Member;
			//	var columnName = member.Name;
			//	if (member.GetCustomAttributes().FirstOrDefault(attribute => attribute is System.Runtime.Serialization.DataMemberAttribute) is System.Runtime.Serialization.DataMemberAttribute dataMemberAttribute)
			//	{
			//		if (!string.IsNullOrWhiteSpace(dataMemberAttribute?.Name))
			//		{
			//			columnName = dataMemberAttribute.Name;
			//		}
			//	}

			//	return columnName;
			//}

			if (expression.Body is System.Linq.Expressions.MemberExpression memberExpression)
			{
				if (memberExpression.Member is System.Reflection.PropertyInfo propertyInfo)
				{
					return propertyInfo.Name;
				}
			}

			if (returnRandomNameIfNotFound)
			{
				return string.Format("{0:D}", Guid.NewGuid());
			}

			throw new ArgumentException(string.Format("Expression '{0}' cannot become a name.", expression.Body.ToString()));
		}
	}
}
