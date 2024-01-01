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
	public class MustMatchAttribute : AbstractValidationAttribute
	{
		internal const string ValidationRuleName = "mustmatch";
		internal const string ValidationParameterAllowEmptyStrings = "allowemptystrings";
		internal const string ValidationParameterIdsToCheckName = "fieldidstocheck";

		protected override string ErrorMessage => "All Fields Must Match";

		public bool AllowEmptyStrings { get; set; }

		internal string[] PropertiesToCheck { get; }

		public MustMatchAttribute(params string[] propertiesToCheck)
		{
			if (propertiesToCheck.Length == 0)
			{
				throw new Exception("Need propertiesToCheck");
			}

			PropertiesToCheck = propertiesToCheck;
		}

		public override void AddValidation(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ClientModelValidationContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, $"data-val-{ValidationRuleName}", GetErrorMessage(context.ModelMetadata));
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterAllowEmptyStrings}", AllowEmptyStrings.TrueFalse(false, BooleanExtensions.TextCase.Lower));
			MergeAttribute(context.Attributes, $"data-val-{ValidationParameterIdsToCheckName}", string.Join(",", PropertiesToCheck));
		}

		public override IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context)
		{
			var value = string.Format("{0}", context.Model);

			foreach (var propertyName in PropertiesToCheck)
			{
				var propertyToCheck = context.ModelMetadata.ContainerType.GetProperty(propertyName);
				if (propertyToCheck != null)
				{
					var valueToCheck = propertyToCheck.GetValue(context.Container, null);

					if (!string.Equals(value, valueToCheck))
					{
						return new[] { new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult(context.ModelMetadata.PropertyName, ErrorMessage) };
					}
				}
			}

			return Enumerable.Empty<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult>();
		}
	}
}