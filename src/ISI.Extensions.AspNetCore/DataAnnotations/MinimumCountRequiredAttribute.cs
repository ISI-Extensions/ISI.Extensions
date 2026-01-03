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

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class MinimumCountRequiredAttribute : AbstractValidationAttribute
	{
		internal const string ValidationRuleName = "minimumcountrequired";
		internal const string ValidationMinimumCountRequiredName = "minimumcountrequired";
		internal const string ValidationFieldIdsToCheckName = "fieldidstocheck";

		protected override string ErrorMessage => "Minimum number of required fields not meet";

		internal int MinimumCountRequired { get; }
		internal string[] FieldsToCheck { get; }

		public MinimumCountRequiredAttribute(int minimumCountRequired, params string[] fieldsToCheck)
		{
			MinimumCountRequired = minimumCountRequired;

			if (fieldsToCheck.Length == 0)
			{
				throw new Exception("Need fieldsToCheck");
			}

			FieldsToCheck = fieldsToCheck;
		}

		public override void AddValidation(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ClientModelValidationContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}", GetErrorMessage(context.ModelMetadata));
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}-{ValidationMinimumCountRequiredName}", $"{MinimumCountRequired}");
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}-{ValidationFieldIdsToCheckName}", string.Join(",", FieldsToCheck));
		}

		public override IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context)
		{
			var fieldCount = 0;

			foreach (var fieldName in FieldsToCheck)
			{
				var fieldToCheck = context.ModelMetadata.ContainerType.GetProperty(fieldName);
				if (fieldToCheck != null)
				{
					var valueToCheck = fieldToCheck.GetValue(context.Container, null);

					if (!string.IsNullOrWhiteSpace($"{valueToCheck}"))
					{
						fieldCount++;
					}
				}
			}

			if (fieldCount < MinimumCountRequired)
			{
				return [new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage)];
			}

			return [];
		}
	}
}