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

namespace ISI.Extensions.Extensions
{
	public static class ReflectionExtensions
	{
		public static void SetValueFromString(this System.Reflection.PropertyInfo propertyInfo, object @object, string value, string delimiter = ",")
		{
			SetValueFromString(propertyInfo, @object, value, [delimiter]);
		}

		public static void SetValueFromString(this System.Reflection.PropertyInfo propertyInfo, object @object, string value, string[] delimiters)
		{
			SetValueFromString(propertyInfo, @object, value, v => v.Split(delimiters, StringSplitOptions.RemoveEmptyEntries));
		}

		public static void SetValueFromString(this System.Reflection.PropertyInfo propertyInfo, object @object, string value, Func<string, string[]> parser)
		{
			if (propertyInfo.PropertyType == typeof(string))
			{
				propertyInfo.SetValue(@object, value, null);
			}
			else if (propertyInfo.PropertyType == typeof(Guid))
			{
				propertyInfo.SetValue(@object, value.ToGuid(), null);
			}
			else if (propertyInfo.PropertyType == typeof(Guid?))
			{
				propertyInfo.SetValue(@object, value.ToGuidNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(int))
			{
				propertyInfo.SetValue(@object, value.ToInt(), null);
			}
			else if (propertyInfo.PropertyType == typeof(int?))
			{
				propertyInfo.SetValue(@object, value.ToIntNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(long))
			{
				propertyInfo.SetValue(@object, value.ToLong(), null);
			}
			else if (propertyInfo.PropertyType == typeof(long?))
			{
				propertyInfo.SetValue(@object, value.ToLongNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(double))
			{
				propertyInfo.SetValue(@object, value.ToDouble(), null);
			}
			else if (propertyInfo.PropertyType == typeof(double?))
			{
				propertyInfo.SetValue(@object, value.ToDoubleNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(decimal))
			{
				propertyInfo.SetValue(@object, value.ToDecimal(), null);
			}
			else if (propertyInfo.PropertyType == typeof(decimal?))
			{
				propertyInfo.SetValue(@object, value.ToDecimalNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(float))
			{
				propertyInfo.SetValue(@object, value.ToFloat(), null);
			}
			else if (propertyInfo.PropertyType == typeof(float?))
			{
				propertyInfo.SetValue(@object, value.ToFloatNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(bool))
			{
				propertyInfo.SetValue(@object, value.ToBooleanNullable() ?? false, null);
			}
			else if (propertyInfo.PropertyType == typeof(bool?))
			{
				propertyInfo.SetValue(@object, value.ToBooleanNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(DateTime))
			{
				propertyInfo.SetValue(@object, value.ToDateTime(), null);
			}
			else if (propertyInfo.PropertyType == typeof(DateTime?))
			{
				propertyInfo.SetValue(@object, value.ToDateTimeNullable(), null);
			}
			else if (propertyInfo.PropertyType == typeof(TimeSpan))
			{
				propertyInfo.SetValue(@object, value.ToTimeSpan(), null);
			}
			else if (propertyInfo.PropertyType == typeof(TimeSpan?))
			{
				propertyInfo.SetValue(@object, value.ToTimeSpanNullable(), null);
			}
			else if (ISI.Extensions.Enum.IsEnum(propertyInfo.PropertyType))
			{
				propertyInfo.SetValue(@object, ISI.Extensions.Enum.Parse(propertyInfo.PropertyType, value), null);
			}
			else if (propertyInfo.PropertyType.IsArray)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					var values = parser(value);

					if (propertyInfo.PropertyType == typeof(string[]))
					{
						propertyInfo.SetValue(@object, values, null);
					}
					else if (propertyInfo.PropertyType == typeof(Guid[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToGuid()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(int[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToInt()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(long[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToLong()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(double[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToDouble()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(decimal[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToDecimal()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(bool[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToBoolean()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(DateTime[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToDateTime()).ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(TimeSpan[]))
					{
						propertyInfo.SetValue(@object, values.Select(v => v.ToTimeSpan()).ToArray(), null);
					}
					else if (ISI.Extensions.Enum.IsEnum(propertyInfo.PropertyType.GetElementType()))
					{
						var elementType = propertyInfo.PropertyType.GetElementType();
						propertyInfo.SetValue(@object, values.Select(v => ISI.Extensions.Enum.Parse(elementType, v)).ToArray(), null);
					}
				}
			}
		}
	}
}
