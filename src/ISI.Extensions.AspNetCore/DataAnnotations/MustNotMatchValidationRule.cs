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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	[DataAnnotationsValidationRule]
	public class MustNotMatchValidationRule : IDataAnnotationsValidationRule
	{
		public string GetJavaScriptUnobtrusiveValidationRule()
		{
			var result = new System.Text.StringBuilder();

			result.AppendFormat("		jQuery.validator.addMethod(\"{1}\",{0}", Environment.NewLine, MustNotMatchAttribute.ValidationRuleName);
			result.AppendLine("			function (value, element) {");
			result.AppendLine("				var isValid = true;");
			result.AppendLine();
			result.AppendLine("				var idParts = element.id.split(\"_\");");
			result.AppendLine("				idParts.pop();");
			result.AppendLine("				var idPrefix = idParts.join(\"_\") + \"_\";");
			result.AppendLine();
			result.AppendFormat("				var allowEmptyStrings = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, MustNotMatchAttribute.ValidationRuleName, MustNotMatchAttribute.ValidationParameterAllowEmptyStrings);
			result.AppendFormat("				var fieldIdsToCheck = jQuery(element).attr(\"data-val-{1}-{2}\").split(\",\");{0}", Environment.NewLine, MustNotMatchAttribute.ValidationRuleName, MustNotMatchAttribute.ValidationParameterIdsToCheckName);
			result.AppendLine();
			result.AppendLine("				if((allowEmptyStrings != \"true\") && (value == \"\")) {");
			result.AppendLine("					isValid = false;");
			result.AppendLine("				} else {");
			result.AppendLine("					var fieldsHaveValue = false;");
			result.AppendLine();
			result.AppendLine("					jQuery.each(fieldIdsToCheck, function (index, fieldId) {");
			result.AppendLine("						if(fieldId.indexOf(\"_\") < 0) {");
			result.AppendLine("							fieldId = idPrefix + fieldId;");
			result.AppendLine("						}");
			result.AppendLine("						var fieldValue = jQuery(\"#\" + fieldId).val();");
			result.AppendLine();
			result.AppendLine("						if (value == fieldValue) {");
			result.AppendLine("							isValid = false;");
			result.AppendLine("						}");
			result.AppendLine("					});");
			result.AppendLine("				}");
			result.AppendLine();
			result.AppendLine("				return isValid;");
			result.AppendLine("			},");
			result.AppendLine("			\"All Fields Must Not Match\");");
			result.AppendFormat("		jQuery.validator.unobtrusive.adapters.addBool(\"{1}\");{0}", Environment.NewLine, MustNotMatchAttribute.ValidationRuleName);

			return result.ToString();
		}
	}
}