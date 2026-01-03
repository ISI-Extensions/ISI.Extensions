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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class RequiredIfNotCheckedAttribute : AbstractValidationAttribute
	{
		internal const string ValidationRuleName = "requiredifnotchecked";
		internal const string ValidationParameterIdToCheckName = "fieldidtocheck";

		protected override string ErrorMessage => "Field Is Required";

		internal string FieldToCheck { get; }

		public RequiredIfNotCheckedAttribute(string fieldToCheck)
		{
			FieldToCheck = fieldToCheck;
		}

		public override void AddValidation(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ClientModelValidationContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}", GetErrorMessage(context.ModelMetadata));
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}-{ValidationParameterIdToCheckName}", FieldToCheck);
		}

		public override IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context)
		{
			var value = $"{context.Model}";

			var fieldToCheck = context.ModelMetadata.ContainerType.GetProperty(FieldToCheck);
			if (fieldToCheck != null)
			{
				var isChecked = $"{fieldToCheck.GetValue(context.Container, null)}".ToBoolean();

				if (!isChecked && string.IsNullOrWhiteSpace($"{value}"))
				{
					return [new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage)];
				}
			}

			return [];
		}
	}
}