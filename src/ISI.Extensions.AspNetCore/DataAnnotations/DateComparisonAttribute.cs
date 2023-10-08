#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public class DateComparisonAttribute : AbstractValidationAttribute
	{
		internal const string ValidationRuleName = "datecomparison";
		internal const string ValidationParameterIdToCheckName = "fieldidtocheck";
		internal const string ValidationParameterComparisonTypeName = "comparisontype";
		internal const string ValidationParameterAllowEmptyStrings = "allowemptystrings";

		protected override string ErrorMessage => "Please check date";

		public bool AllowEmptyStrings { get; set; }

		internal string FieldToCheck { get; }
		internal ISI.Extensions.ComparisonType ComparisonType { get; }

		public DateComparisonAttribute(string fieldToCheck, ISI.Extensions.ComparisonType comparisonType)
		{
			if (string.IsNullOrWhiteSpace(fieldToCheck))
			{
				throw new Exception("Need fieldToCheck");
			}

			FieldToCheck = fieldToCheck;
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
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterIdToCheckName}", FieldToCheck);
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterComparisonTypeName}", ComparisonType.GetAbbreviation());
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterAllowEmptyStrings}", AllowEmptyStrings.TrueFalse(false, BooleanExtensions.TextCase.Lower));
		}

		public override IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context)
		{
			var model = string.Format("{0}", context.Model);

			if (!AllowEmptyStrings || !string.IsNullOrWhiteSpace(model))
			{
				var nullableValue = model.ToDateTimeNullable();

				if (!nullableValue.HasValue)
				{
					return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
				}

				var value = nullableValue.Value;

				var fieldToCheck = context.ModelMetadata.ContainerType.GetProperty(FieldToCheck);
				if (fieldToCheck != null)
				{
					var valueToCheck = string.Format("{0}", fieldToCheck.GetValue(context.Container, null)).ToDateTime();

					switch (ComparisonType)
					{
						case ISI.Extensions.ComparisonType.Equal:
							if (value != valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.NotEqual:
							if (value == valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.LessThan:
							if (value >= valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.LessThanOrEqualTo:
							if (value > valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.GreaterThan:
							if (value <= valueToCheck)
							{
								return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
							}

							break;
						case ISI.Extensions.ComparisonType.GreaterThanOrEqualTo:
							if (value < valueToCheck)
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