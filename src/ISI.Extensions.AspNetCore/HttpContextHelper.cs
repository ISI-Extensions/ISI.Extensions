#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.AspNetCore
{
	public class HttpContextHelper
	{
		private static Configuration _configuration = null;
		internal static Configuration Configuration => _configuration ??= ISI.Extensions.ServiceLocator.Current.GetService<Configuration>();

		private static Microsoft.Extensions.Logging.ILogger _logger = null;
		internal static Microsoft.Extensions.Logging.ILogger Logger => _logger ??= ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();

		internal System.Security.Principal.IIdentity Identity = null;
		internal System.Collections.Specialized.NameValueCollection ServerVariables = null;
		internal System.Collections.Specialized.NameValueCollection QueryString = null;
		internal System.Collections.Specialized.NameValueCollection FormValues = null;
		internal System.Collections.Specialized.NameValueCollection Cookies = null;
		internal Guid? VisitorUuid = null;
		internal Guid? VisitUuid = null;

		public HttpContextHelper(Microsoft.AspNetCore.Http.HttpContext context, Guid? visitorUuid, Guid? visitUuid)
		{
			if (context != null)
			{
				try
				{
					if (Configuration.HttpContextLogging.RecordIdentity)
					{
						Identity = context?.User?.Identity;
					}
				}
				catch (Exception exception)
				{
					Logger?.LogError(exception, "Error getting Identity");
				}

				try
				{
					if (Configuration.HttpContextLogging.RecordServerVariables)
					{
						//ServerVariables = CloneNameValueCollection(context?.Request.ServerVariables);
					}
				}
				catch (Exception exception)
				{
					Logger?.LogError(exception, "Error getting ServerVariables");
				}

				try
				{
					if (Configuration.HttpContextLogging.RecordQueryString)
					{
						QueryString = CloneNameValueCollection(context.Request.QueryString);
					}
				}
				catch (Exception exception)
				{
					Logger?.LogError(exception, "Error getting QueryString");
				}

				try
				{
					if (Configuration.HttpContextLogging.RecordFormValues &&
							!string.Equals(context?.Request.Method, ISI.Extensions.HttpVerb.Get.GetAbbreviation(), StringComparison.InvariantCultureIgnoreCase) &&
							((context.Request.ContentType ?? string.Empty).IndexOf("multipart/form-data", StringComparison.InvariantCultureIgnoreCase) < 0) &&
							((context.Request.ContentType ?? string.Empty).IndexOf("json", StringComparison.InvariantCultureIgnoreCase) < 0))
					{
						FormValues = CloneNameValueCollection(context.Request.Form);
					}
				}
				catch (Exception exception)
				{
					Logger?.LogError(exception, "Error getting FormValues");
				}

				try
				{
					if (Configuration.HttpContextLogging.RecordCookies)
					{
						Cookies = CloneNameValueCollection(context.Request.Cookies);
					}
				}
				catch (Exception exception)
				{
					Logger?.LogError(exception, "Error getting Cookies");
				}
			}

			VisitorUuid = visitorUuid;
			VisitUuid = visitUuid;
		}

		#region CloneNameValueCollection
		private static System.Collections.Specialized.NameValueCollection CloneNameValueCollection(System.Collections.Specialized.NameValueCollection values)
		{
			System.Collections.Specialized.NameValueCollection result = null;

			if (values != null)
			{
				result = new System.Collections.Specialized.NameValueCollection(values);
			}

			return result;
		}

		private static System.Collections.Specialized.NameValueCollection CloneNameValueCollection(Microsoft.AspNetCore.Http.IFormCollection formValues)
		{
			System.Collections.Specialized.NameValueCollection result = null;

			if (formValues.NullCheckedAny())
			{
				result = new System.Collections.Specialized.NameValueCollection();

				foreach (var formValue in formValues)
				{
					result.Add(formValue.Key, formValue.Value);
				}
			}

			return result;
		}

		private static System.Collections.Specialized.NameValueCollection CloneNameValueCollection(Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
		{
			System.Collections.Specialized.NameValueCollection result = null;

			if (cookies.NullCheckedAny())
			{
				result = new System.Collections.Specialized.NameValueCollection();

				foreach (var cookie in cookies)
				{
					result.Add(cookie.Key, cookie.Value);
				}
			}

			return result;
		}

		private static System.Collections.Specialized.NameValueCollection CloneNameValueCollection(Microsoft.AspNetCore.Http.QueryString queryString)
		{
			System.Collections.Specialized.NameValueCollection result = null;

			if (queryString.HasValue)
			{
				result = new System.Collections.Specialized.NameValueCollection();

				var values = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString.Value);

				foreach (var value in values)
				{
					foreach (var stringValue in value.Value)
					{
						result.Add(value.Key, stringValue);
					}
				}

			}

			return result;
		}
		#endregion

		#region Serialize
		public string Serialize()
		{
			string result = null;

			if (Configuration.HttpContextLogging.RecordWebRequestDetail)
			{
				using (var memoryStream = new System.IO.MemoryStream())
				{
					var xmlWriterSettings = new System.Xml.XmlWriterSettings()
					{
						Indent = true,
						NewLineOnAttributes = false,
						CheckCharacters = false,
					};

					using (var xmlWriter = System.Xml.XmlWriter.Create(memoryStream, xmlWriterSettings))
					{
						xmlWriter.WriteStartElement("Request");

						#region VisitorUuid
						if (VisitorUuid.HasValue)
						{
							xmlWriter.WriteStartElement("VisitorUuid");
							xmlWriter.WriteValue(VisitorUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
							xmlWriter.WriteEndElement();
						}
						#endregion

						#region VisitUuid
						if (VisitUuid.HasValue)
						{
							xmlWriter.WriteStartElement("VisitUuid");
							xmlWriter.WriteValue(VisitUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
							xmlWriter.WriteEndElement();
						}
						#endregion

						#region Identity
						if (Configuration.HttpContextLogging.RecordIdentity && (Identity != null))
						{
							xmlWriter.WriteStartElement("Identity");

							xmlWriter.WriteStartElement("Name");
							xmlWriter.WriteValue(Identity.Name);
							xmlWriter.WriteEndElement();

							xmlWriter.WriteStartElement("IsAuthenticated");
							xmlWriter.WriteValue((Identity.IsAuthenticated ? "Yes" : "No"));
							xmlWriter.WriteEndElement();

							xmlWriter.WriteStartElement("AuthenticationType");
							xmlWriter.WriteValue(Identity.AuthenticationType);
							xmlWriter.WriteEndElement();

							xmlWriter.WriteEndElement();
						}
						#endregion

						if (Configuration.HttpContextLogging.RecordServerVariables && (ServerVariables != null))
						{
							SerializeCollection(xmlWriter, "ServerVariables", ServerVariables);
						}
						if (Configuration.HttpContextLogging.RecordQueryString && (QueryString != null))
						{
							SerializeCollection(xmlWriter, "QueryString", QueryString);
						}
						if (Configuration.HttpContextLogging.RecordFormValues && (FormValues != null))
						{
							SerializeCollection(xmlWriter, "FormValues", FormValues);
						}
						if (Configuration.HttpContextLogging.RecordCookies && (Cookies != null))
						{
							SerializeCollection(xmlWriter, "Cookies", Cookies);
						}

						xmlWriter.WriteEndElement();

						xmlWriter.Flush();
					}

					memoryStream.Rewind();

					using (var streamReader = new System.IO.StreamReader(memoryStream))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}

			return result;
		}

		private void SerializeCollection(System.Xml.XmlWriter xmlWriter, string collectionName, System.Collections.Specialized.NameValueCollection collection)
		{
			if (collection.NullCheckedAny())
			{
				xmlWriter.WriteStartElement(collectionName);

				foreach (string key in collection.Keys)
				{
					xmlWriter.WriteStartElement("item");
					xmlWriter.WriteAttributeString("name", key);
					SerializeValues(xmlWriter, collection.GetValues(key));
					xmlWriter.WriteEndElement();
				}

				xmlWriter.WriteEndElement();
			}
		}

		private void SerializeValues(System.Xml.XmlWriter xmlWriter, string[] values)
		{
			foreach (var value in values)
			{
				xmlWriter.WriteStartElement("value");
				xmlWriter.WriteValue(value);
				xmlWriter.WriteEndElement();
			}
		}
		#endregion
	}
}
