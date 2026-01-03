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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore
{
	public class HttpContextLogState : ISI.Extensions.Logging.IHttpContextLogState
	{
		public string OperationKey { get; }
		public string ActivityKey { get; }

		public System.Security.Principal.IIdentity Identity { get; }
		public System.Collections.Specialized.NameValueCollection ServerVariables { get; }
		public System.Collections.Specialized.NameValueCollection QueryString { get; }
		public System.Collections.Specialized.NameValueCollection FormValues { get; }
		public string JsonBody { get; }
		public System.Collections.Specialized.NameValueCollection Cookies { get; }

		public Guid? VisitorUuid { get; }
		public Guid? VisitUuid { get; }

		public HttpContextLogState(
			string operationKey,
			string activityKey,
			System.Security.Principal.IIdentity identity,
			System.Collections.Specialized.NameValueCollection serverVariables,
			System.Collections.Specialized.NameValueCollection queryString, 
			System.Collections.Specialized.NameValueCollection formValues,
			string jsonBody,
			System.Collections.Specialized.NameValueCollection cookies,
			Guid? visitorUuid, 
			Guid? visitUuid)
		{
			OperationKey = operationKey;
			ActivityKey = activityKey;

			Identity = identity;
			ServerVariables = serverVariables;
			QueryString = queryString;
			FormValues = formValues;
			JsonBody = jsonBody;
			Cookies = cookies;

			VisitorUuid = visitorUuid;
			VisitUuid = visitUuid;
		}

		private void Format(StringBuilder stringBuilder, string nameValuesDescription, System.Collections.Specialized.NameValueCollection nameValues)
		{
			if (nameValues.NullCheckedAny())
			{
				stringBuilder.AppendFormat("{0}:\n", nameValuesDescription);
				foreach (var key in nameValues.AllKeys)
				{
					stringBuilder.AppendFormat("  {0}: \"{1}\"\n", key, nameValues[key]);
				}
			}
		}

		public string Formatted()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendFormat("OperationKey: \"{0}\"\n", OperationKey);
			stringBuilder.AppendFormat("ActivityKey: \"{0}\"\n", ActivityKey);
			stringBuilder.AppendFormat("Identity.Name: \"{0}\"\n", Identity?.Name);
			stringBuilder.AppendFormat("Identity.IsAuthenticated: \"{0}\"\n", (Identity?.IsAuthenticated ?? false).TrueFalse());
			Format(stringBuilder, "ServerVariables", ServerVariables);
			Format(stringBuilder, "QueryString", QueryString);
			if (!string.IsNullOrWhiteSpace(JsonBody))
			{
				stringBuilder.AppendFormat("JsonBody:\n {0}\n", JsonBody);
			}
			Format(stringBuilder, "FormValues", FormValues);
			Format(stringBuilder, "Cookies", Cookies);

			stringBuilder.AppendFormat("VisitorUuid: \"{0}\"\n", VisitorUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
			stringBuilder.AppendFormat("VisitUuid: \"{0}\"\n", VisitUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));

			return stringBuilder.ToString();
		}
	}
}
