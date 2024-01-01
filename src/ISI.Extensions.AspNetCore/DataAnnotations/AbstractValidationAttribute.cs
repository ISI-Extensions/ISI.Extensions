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
using Microsoft.AspNetCore.Html;

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	public abstract class AbstractValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IModelValidator, Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IClientModelValidator
	{
		protected virtual string ErrorMessage => "A data validation error has occurred for {0}";

		protected AbstractValidationAttribute()
		{

		}

		protected AbstractValidationAttribute(Func<string> errorMessageAccessor)
			: base(errorMessageAccessor)
		{

		}

		protected AbstractValidationAttribute(string errorMessage)
			: base(errorMessage)
		{

		}

		public abstract IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationResult> Validate(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ModelValidationContext context);

		public abstract void AddValidation(Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ClientModelValidationContext context);

		protected virtual void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
		{
			if (!attributes.ContainsKey(key))
			{
				attributes.Add(key, value);
			}
		}

		protected virtual string GetErrorMessage(Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata modelMetadata)
		{
			if (modelMetadata == null)
			{
				throw new ArgumentNullException(nameof(modelMetadata));
			}

			return string.Format(ErrorMessage, modelMetadata.DisplayName ?? modelMetadata.Name ?? modelMetadata.PropertyName ?? "<unknown>");
		}
	}
}
