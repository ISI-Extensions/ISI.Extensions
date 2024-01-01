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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class DateTimeComparisonAttribute : AbstractValidationAttribute
	{
		internal const string ValidationRuleName = "datetimecomparison";
		internal const string ValidationParameterTimeFieldIdToIncludeName = "timefieldidtoinclude";
		internal const string ValidationParameterDateFieldIdToCheckName = "datefieldidtocheck";
		internal const string ValidationParameterTimeFieldIdToCheckName = "timefieldidtocheck";
		internal const string ValidationParameterComparisonTypeName = "comparisontype";
		internal const string ValidationParameterAllowEmptyStrings = "allowemptystrings";

		protected override string ErrorMessage => "Please check date";

		public bool AllowEmptyStrings { get; set; }
		public string TimeFieldToInclude { get; set; }
		public string TimeFieldToCheck { get; set; }

		internal string DateFieldToCheck { get; }
		internal ISI.Extensions.ComparisonType ComparisonType { get; }

		public DateTimeComparisonAttribute(string dateFieldToCheck, ISI.Extensions.ComparisonType comparisonType)
		{
			if (string.IsNullOrWhiteSpace(dateFieldToCheck))
			{
				throw new Exception("Need dateFieldToCheck");
			}

			DateFieldToCheck = dateFieldToCheck;
			ComparisonType = comparisonType;
		}

		public override void AddValidation(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ClientModelValidationContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}", GetErrorMessage(context.ModelMetadata));
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterTimeFieldIdToIncludeName}", TimeFieldToInclude);
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterDateFieldIdToCheckName}", DateFieldToCheck);
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterTimeFieldIdToCheckName}", TimeFieldToCheck);
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterComparisonTypeName}", ComparisonType.GetAbbreviation());
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterAllowEmptyStrings}", AllowEmptyStrings.TrueFalse(false, BooleanExtensions.TextCase.Lower));
		}

		public override IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context)
		{
			var model = string.Format("{0}", context.Model);

			if (!AllowEmptyStrings || !string.IsNullOrWhiteSpace(model))
			{
				var dateTimeValueToParse = model;
				var timeFieldToInclude = context.ModelMetadata.ContainerType.GetProperty(TimeFieldToInclude);
				if (timeFieldToInclude != null)
				{
					var timeFieldToIncludeValue = string.Format("{0}", timeFieldToInclude.GetValue(context.Container, null));

					dateTimeValueToParse = string.Format("{0} {1}", dateTimeValueToParse, timeFieldToIncludeValue);
				}

				var dateTimeNullableValue = dateTimeValueToParse.ToDateTimeNullable();

				if (!dateTimeNullableValue.HasValue)
				{
					return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
				}

				var dateTimeValue = dateTimeNullableValue.Value;

				var dateFieldToCheck = context.ModelMetadata.ContainerType.GetProperty(DateFieldToCheck);
				if (dateFieldToCheck != null)
				{
					dateTimeValueToParse = string.Format("{0}", dateFieldToCheck.GetValue(context.Container, null));
					var timeFieldToCheck = context.ModelMetadata.ContainerType.GetProperty(TimeFieldToCheck);
					if (timeFieldToInclude != null)
					{
						var timeFieldToIncludeValue = string.Format("{0}", timeFieldToInclude.GetValue(context.Container, null));

						dateTimeValueToParse = string.Format("{0} {1}", dateTimeValueToParse, timeFieldToIncludeValue);
					}

					var valueToCheck = dateTimeValueToParse.ToDateTime();

					switch (ComparisonType)
					{
						case ISI.Extensions.ComparisonType.Equal:
							if (dateTimeValue != valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.NotEqual:
							if (dateTimeValue == valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.LessThan:
							if (dateTimeValue >= valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.LessThanOrEqualTo:
							if (dateTimeValue > valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.GreaterThan:
							if (dateTimeValue <= valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.GreaterThanOrEqualTo:
							if (dateTimeValue < valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}

			return Enumerable.Empty<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult>();
		}
	}
}
