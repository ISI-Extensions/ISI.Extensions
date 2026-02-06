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

namespace ISI.Extensions
{
	public class PropertyBagHelper
	{
		public delegate string[] GetPropertyBagParameterDelegate<TPropertyBag>(TPropertyBag propertyBag)
			where TPropertyBag : class;

		public delegate void SetPropertyBagParameterDelegate<TPropertyBag>(TPropertyBag propertyBag, string[] values)
			where TPropertyBag : class;

		public delegate TPropertyBagParameter CreatePropertyBagParameterDelegate<TPropertyBagParameterAttribute, TPropertyBagParameter, TPropertyBag>(TPropertyBagParameterAttribute propertyBagParameterAttribute, PropertyInfo propertyInfo, bool isMultiValue, bool isNullable, Type baseType, GetPropertyBagParameterDelegate<TPropertyBag> getValues, SetPropertyBagParameterDelegate<TPropertyBag> setValues)
			where TPropertyBagParameterAttribute : System.Attribute
			where TPropertyBagParameter : class
			where TPropertyBag : class;

		public static TPropertyBagParameter[] GetPropertyBagParameters<TPropertyBagParameterAttribute, TPropertyBagParameter, TPropertyBag>(Type propertyBagSourceType, CreatePropertyBagParameterDelegate<TPropertyBagParameterAttribute, TPropertyBagParameter, TPropertyBag> createPropertyBagParameter)
			where TPropertyBagParameterAttribute : System.Attribute
			where TPropertyBagParameter : class
			where TPropertyBag : class
		{
			var propertyInfos = propertyBagSourceType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty);

			return propertyInfos.ToNullCheckedArray(propertyInfo =>
			{
				var propertyBagParameterAttribute = propertyInfo.GetCustomAttributes().OfType<TPropertyBagParameterAttribute>().FirstOrDefault();
				if (propertyBagParameterAttribute != null)
				{
					var isMultiValue = false;
					var isNullable = false;
					var baseType = typeof(object);
					GetPropertyBagParameterDelegate<TPropertyBag> getValues = null;
					SetPropertyBagParameterDelegate<TPropertyBag> setValues = null;

					if (propertyInfo.PropertyType == typeof(bool))
					{
						baseType = typeof(bool);
						getValues = propertyBag => [((bool)propertyInfo.GetValue(propertyBag)).TrueFalse()];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToBoolean());
					}
					else if (propertyInfo.PropertyType == typeof(bool?))
					{
						isNullable = true;
						baseType = typeof(bool);
						getValues = propertyBag => [((bool?)propertyInfo.GetValue(propertyBag)).TrueFalse()];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToBooleanNullable());
					}
					else if (propertyInfo.PropertyType == typeof(string[]))
					{
						isMultiValue = true;
						baseType = typeof(string);
						getValues = propertyBag => propertyInfo.GetValue(propertyBag) as string[];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values);
					}
					else if (propertyInfo.PropertyType == typeof(string))
					{
						isNullable = true;
						baseType = typeof(string);
						getValues = propertyBag => [propertyInfo.GetValue(propertyBag) as string ?? string.Empty];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.NullCheckedFirstOrDefault() ?? string.Empty);
					}
					else if (propertyInfo.PropertyType == typeof(Guid))
					{
						baseType = typeof(Guid);
						getValues = propertyBag => [((Guid)propertyInfo.GetValue(propertyBag)).Formatted(GuidExtensions.GuidFormat.WithHyphens)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToGuid());
					}
					else if (propertyInfo.PropertyType == typeof(Guid?))
					{
						isNullable = true;
						baseType = typeof(Guid);
						getValues = propertyBag => [((Guid?)propertyInfo.GetValue(propertyBag)).Formatted(GuidExtensions.GuidFormat.WithHyphens)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToGuidNullable());
					}
					else if (propertyInfo.PropertyType == typeof(Guid[]))
					{
						isMultiValue = true;
						baseType = typeof(Guid);
						getValues = propertyBag => ((Guid[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => value.Formatted(GuidExtensions.GuidFormat.WithHyphens));
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToGuid()));
					}
					else if (propertyInfo.PropertyType == typeof(int))
					{
						baseType = typeof(int);
						getValues = propertyBag => [$"{((int)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToInt());
					}
					else if (propertyInfo.PropertyType == typeof(int?))
					{
						isNullable = true;
						baseType = typeof(int);
						getValues = propertyBag => [$"{((int?)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToIntNullable());
					}
					else if (propertyInfo.PropertyType == typeof(int[]))
					{
						isMultiValue = true;
						baseType = typeof(int);
						getValues = propertyBag => ((int[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => $"{value}");
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToInt()));
					}
					else if (propertyInfo.PropertyType == typeof(long))
					{
						baseType = typeof(long);
						getValues = propertyBag => [$"{((long)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToLong());
					}
					else if (propertyInfo.PropertyType == typeof(long?))
					{
						baseType = typeof(long);
						isNullable = true;
						getValues = propertyBag => [$"{((long?)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToLongNullable());
					}
					else if (propertyInfo.PropertyType == typeof(long[]))
					{
						baseType = typeof(long);
						isMultiValue = true;
						getValues = propertyBag => ((long[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => $"{value}");
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToLong()));
					}
					else if (propertyInfo.PropertyType == typeof(double))
					{
						baseType = typeof(double);
						getValues = propertyBag => [$"{((double)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDouble());
					}
					else if (propertyInfo.PropertyType == typeof(double?))
					{
						baseType = typeof(double);
						isNullable = true;
						getValues = propertyBag => [$"{((double?)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDoubleNullable());
					}
					else if (propertyInfo.PropertyType == typeof(double[]))
					{
						baseType = typeof(double);
						getValues = propertyBag => ((double[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => $"{value}");
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToDouble()));
					}
					else if (propertyInfo.PropertyType == typeof(decimal))
					{
						baseType = typeof(decimal);
						getValues = propertyBag => [$"{((decimal)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDecimal());
					}
					else if (propertyInfo.PropertyType == typeof(decimal?))
					{
						baseType = typeof(decimal);
						isNullable = true;
						getValues = propertyBag => [$"{((decimal?)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDecimalNullable());
					}
					else if (propertyInfo.PropertyType == typeof(decimal[]))
					{
						baseType = typeof(decimal);
						isMultiValue = true;
						getValues = propertyBag => ((decimal[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => $"{value}");
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToDecimal()));
					}
					else if (propertyInfo.PropertyType == typeof(float))
					{
						baseType = typeof(float);
						getValues = propertyBag => [$"{((float)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToFloat());
					}
					else if (propertyInfo.PropertyType == typeof(float?))
					{
						baseType = typeof(float);
						isNullable = true;
						getValues = propertyBag => [$"{((float?)propertyInfo.GetValue(propertyBag))}"];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToFloatNullable());
					}
					else if (propertyInfo.PropertyType == typeof(float[]))
					{
						baseType = typeof(float);
						isMultiValue = true;
						getValues = propertyBag => ((float[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => $"{value}");
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToFloat()));
					}
					else if (propertyInfo.PropertyType == typeof(DateTime))
					{
						baseType = typeof(DateTime);
						getValues = propertyBag => [((DateTime)propertyInfo.GetValue(propertyBag)).Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDateTimeUtc());
					}
					else if (propertyInfo.PropertyType == typeof(DateTime?))
					{
						baseType = typeof(DateTime);
						isNullable = true;
						getValues = propertyBag => [((DateTime?)propertyInfo.GetValue(propertyBag)).Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToDateTimeUtcNullable());
					}
					else if (propertyInfo.PropertyType == typeof(DateTime[]))
					{
						baseType = typeof(DateTime);
						isMultiValue = true;
						getValues = propertyBag => ((DateTime[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => value.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeUniversalPrecise));
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToDateTimeUtc()));
					}
					else if (propertyInfo.PropertyType == typeof(TimeSpan))
					{
						baseType = typeof(TimeSpan);
						getValues = propertyBag => [((TimeSpan)propertyInfo.GetValue(propertyBag)).Formatted(TimeSpanExtensions.TimeSpanFormat.Precise)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToTimeSpan());
					}
					else if (propertyInfo.PropertyType == typeof(TimeSpan?))
					{
						baseType = typeof(TimeSpan);
						isNullable = true;
						getValues = propertyBag => [((TimeSpan?)propertyInfo.GetValue(propertyBag)).Formatted(TimeSpanExtensions.TimeSpanFormat.Precise)];
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, (values.NullCheckedFirstOrDefault() ?? string.Empty).ToTimeSpanNullable());
					}
					else if (propertyInfo.PropertyType == typeof(TimeSpan[]))
					{
						baseType = typeof(TimeSpan);
						isMultiValue = true;
						getValues = propertyBag => ((TimeSpan[])propertyInfo.GetValue(propertyBag)).ToNullCheckedArray(value => value.Formatted(TimeSpanExtensions.TimeSpanFormat.Precise));
						setValues = (propertyBag, values) => propertyInfo.SetValue(propertyBag, values.ToNullCheckedArray(value => value.ToTimeSpan()));
					}
					else
					{
						throw new Exception($"Cannot have a parameter of type \"{propertyInfo.PropertyType.Name}\"");
					}

					return createPropertyBagParameter(propertyBagParameterAttribute, propertyInfo, isMultiValue, isNullable, baseType, getValues, setValues);
				}

				return default;
			}).Where(parameter => parameter != null).ToArray();
		}
	}
}
