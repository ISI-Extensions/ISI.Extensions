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
	[DataAnnotationsValidationRule]
	public class DateComparisonValidationRule : IDataAnnotationsValidationRule
	{
		public string GetJavaScriptUnobtrusiveValidationRule()
		{
			var result = new System.Text.StringBuilder();

			result.AppendFormat("		jQuery.validator.addMethod(\"{1}\",{0}", Environment.NewLine, DateComparisonAttribute.ValidationRuleName);
			result.AppendLine("			function (value, element) {");
			result.AppendLine("				var hasValidValue = true;");
			result.AppendLine();
			result.AppendLine("				value = value.replace(\"-\", \"/\");");
			result.AppendLine("				var dateTimeValue = Date.parse(value);");
			result.AppendLine();
			result.AppendFormat("				var allowEmptyStrings = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, DateComparisonAttribute.ValidationRuleName, DateComparisonAttribute.ValidationParameterAllowEmptyStrings);
			result.AppendLine();
			result.AppendLine("				if(!allowEmptyStrings || (value > '')) {");
			result.AppendFormat("					var comparisonType = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, DateComparisonAttribute.ValidationRuleName, DateComparisonAttribute.ValidationParameterComparisonTypeName);
			result.AppendLine();
			result.AppendLine("					var idParts = element.id.split(\"_\");");
			result.AppendLine("					idParts.pop();");
			result.AppendLine("					var idPrefix = idParts.join(\"_\") + \"_\";");
			result.AppendLine();
			result.AppendFormat("					var fieldIdToCheck = jQuery(element).attr(\"data-val-{1}-{2}\");{0}", Environment.NewLine, DateComparisonAttribute.ValidationRuleName, DateComparisonAttribute.ValidationParameterIdToCheckName);
			result.AppendLine("					if(fieldIdToCheck.indexOf(\"_\") < 0) {");
			result.AppendLine("						fieldIdToCheck = idPrefix + fieldIdToCheck;");
			result.AppendLine("					}");
			result.AppendLine();
			result.AppendLine("					var fieldValue = jQuery(\"#\" + fieldIdToCheck).val();");
			result.AppendLine("					fieldValue = fieldValue.replace(\"-\", \"/\");");
			result.AppendLine("					var dateTimeFieldValue = Date.parse(fieldValue);");
			result.AppendLine();
			result.AppendLine("					switch(comparisonType) {");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.Equal));
			result.AppendLine("							hasValidValue = (dateTimeValue == dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.NotEqual));
			result.AppendLine("							hasValidValue = (dateTimeValue != dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.LessThan));
			result.AppendLine("							hasValidValue = (dateTimeValue < dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.LessThanOrEqualTo));
			result.AppendLine("							hasValidValue = (dateTimeValue <= dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.GreaterThan));
			result.AppendLine("							hasValidValue = (dateTimeValue > dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendFormat("						case \"{1}\":{0}", Environment.NewLine, ISI.Extensions.Enum<ISI.Extensions.ComparisonType>.GetAbbreviation(ISI.Extensions.ComparisonType.GreaterThanOrEqualTo));
			result.AppendLine("							hasValidValue = (dateTimeValue >= dateTimeFieldValue);");
			result.AppendLine("							break;");
			result.AppendLine("					}");
			result.AppendLine("				}");
			result.AppendLine();
			result.AppendLine("				return hasValidValue;");
			result.AppendLine("			},");
			result.AppendLine("			\"Please check date\");");
			result.AppendFormat("		jQuery.validator.unobtrusive.adapters.addBool(\"{1}\");{0}", Environment.NewLine, DateComparisonAttribute.ValidationRuleName);

			return result.ToString();
		}
	}
}