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

namespace ISI.Extensions.AspNetCore.DataAnnotations
{
	[DataAnnotationsValidationRule]
	public class RequiredIfValueValidationRule : IDataAnnotationsValidationRule
	{
		public string GetJavaScriptUnobtrusiveValidationRule()
		{
			var result = new System.Text.StringBuilder();

			result.AppendFormat("		jQuery.validator.addMethod(\"{1}\",{0}", Environment.NewLine, RequiredIfValueAttribute.ValidationRuleName);
			result.AppendLine("			function (value, element) {");
			result.AppendLine("				var result = true;");
			result.AppendLine();
			result.AppendLine("				var idParts = element.id.split(\"_\");");
			result.AppendLine("				idParts.pop();");
			result.AppendLine("				var idPrefix = idParts.join(\"_\") + \"_\";");
			result.AppendLine();
			result.AppendFormat("				var fieldIdToCheck = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, RequiredIfValueAttribute.ValidationRuleName, RequiredIfValueAttribute.ValidationFieldIdToCheckName);
			result.AppendLine("				if(fieldIdToCheck.indexOf(\"_\") < 0) {");
			result.AppendLine("					fieldIdToCheck = idPrefix + fieldIdToCheck;");
			result.AppendLine("				}");
			result.AppendFormat("				var fieldValue = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, RequiredIfValueAttribute.ValidationRuleName, RequiredIfValueAttribute.ValidationFieldValueName);
			result.AppendLine();
			result.AppendLine("				if(jQuery(\"#\" + fieldIdToCheck).val() == fieldValue) {");
			result.AppendLine("		      result = (value.length > 0);");
			result.AppendLine("				}");
			result.AppendLine();
			result.AppendLine("				return result;");
			result.AppendLine("			},");
			result.AppendLine("			\"Field Is Required\");");
			result.AppendFormat("		jQuery.validator.unobtrusive.adapters.addBool(\"{1}\");{0}", Environment.NewLine, RequiredIfValueAttribute.ValidationRuleName);

			return result.ToString();
		}
	}
}