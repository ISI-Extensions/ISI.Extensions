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

namespace ISI.Extensions.Extensions
{
	public static class UriBuilderExtensions
	{
		public static void SetPathAndQueryString(this UriBuilder uriBuilder, string pathAndQueryString, bool appendPath = false)
		{
			var path = string.Empty;
			var queryString = string.Empty;

			pathAndQueryString = pathAndQueryString.Trim();

			var delimiterIndex = pathAndQueryString.IndexOf("?");
			if (delimiterIndex >= 0)
			{
				if (delimiterIndex > 0)
				{
					path = pathAndQueryString.Substring(0, delimiterIndex);
				}
				if (delimiterIndex < pathAndQueryString.Length)
				{
					queryString = pathAndQueryString.Substring(delimiterIndex + 1);
				}
			}
			else if (pathAndQueryString.IndexOf("=") >= 0)
			{
				queryString = pathAndQueryString;
			}
			else
			{
				path = pathAndQueryString;
			}

			if (appendPath)
			{
				uriBuilder.AddDirectoryToPath(path);
			}
			else
			{
				uriBuilder.Path = path;
			}
			uriBuilder.Query = queryString;
		}

		public static UriBuilder AddDirectoryToPath(this UriBuilder uriBuilder, string directory)
		{
			if (!string.IsNullOrWhiteSpace(directory))
			{
				var path = (uriBuilder.Path ?? string.Empty).Trim();

				if (string.IsNullOrWhiteSpace(path))
				{
					path = directory;
				}
				else
				{
					path = $"{path}{(path.EndsWith("/") ? string.Empty : "/")}{directory.TrimStart("/")}";
				}

				while (path.StartsWith("/"))
				{
					path = path.Substring(1);
				}

				uriBuilder.Path = path;
			}

			return uriBuilder;
		}

		public static bool ContainsQueryStringParameter(this UriBuilder uriBuilder, string name)
		{
			return uriBuilder.Uri.ContainsQueryStringParameter(name);
		}

		public static bool ContainsQueryStringParameter(this Uri uri, string name)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				var values = System.Web.HttpUtility.ParseQueryString(uri.Query);

				return values.AllKeys.Contains(name, StringComparer.InvariantCultureIgnoreCase);
			}

			return false;
		}

		public static UriBuilder ConditionalAddQueryStringParameter(this UriBuilder uriBuilder, bool doAddItem, string name, string value, bool removeEmptyValues = true)
		{
			if (doAddItem)
			{
				return uriBuilder.AddQueryStringParameter(name, value, removeEmptyValues);
			}

			return uriBuilder;
		}

		public static UriBuilder AddQueryStringParameter(this UriBuilder uriBuilder, string name, long value)
		{
			return uriBuilder.AddQueryStringParameter(name, $"{value}");
		}

		public static UriBuilder AddQueryStringParameter(this UriBuilder uriBuilder, string name, int value)
		{
			return uriBuilder.AddQueryStringParameter(name, $"{value}");
		}

		public static UriBuilder AddQueryStringParameter(this UriBuilder uriBuilder, string name, bool value)
		{
			return uriBuilder.AddQueryStringParameter(name, value.TrueFalse(textCase: BooleanExtensions.TextCase.Lower));
		}

		public static UriBuilder AddQueryStringParameter(this UriBuilder uriBuilder, string name, string value, bool removeEmptyValues = true)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				var values = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

				values[name] = value;

				uriBuilder.Query = values.ToQueryString(removeEmptyValues);
			}

			return uriBuilder;
		}

		public static UriBuilder AddQueryStringParameters<TRequest>(this UriBuilder uriBuilder, TRequest request, bool removeEmptyValues = true)
		{
			if (request is IDictionary<string, string> keyValuePairs)
			{
				var values = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

				foreach (var keyValuePair in keyValuePairs)
				{
					values[keyValuePair.Key] = keyValuePair.Value;
				}

				uriBuilder.Query = values.ToQueryString(removeEmptyValues);

				return uriBuilder;
			}

			if (request is System.Collections.Specialized.NameValueCollection nameValuePairs)
			{
				var values = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

				foreach (string key in nameValuePairs.Keys)
				{
					values[key] = nameValuePairs[key];
				}

				uriBuilder.Query = values.ToQueryString(removeEmptyValues);

				return uriBuilder;
			}

			{
				var values = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

				var queryStringParameters = values.AllKeys.Select(key => $"{System.Web.HttpUtility.UrlEncode(key)}={System.Web.HttpUtility.UrlEncode(values[key])}").ToList();

				var keyValues = ISI.Extensions.DataContract.GetValuesDictionary(typeof(TRequest), request);

				foreach (var keyValue in keyValues)
				{
					var valueType = keyValue.Value.GetType();

					if (valueType.IsArray)
					{
						valueType = valueType.GetElementType();

						foreach (var value in ((IEnumerable<object>) keyValue.Value))
						{
							var parameterValue = (ISI.Extensions.Enum.IsEnum(valueType) ? ISI.Extensions.Enum.GetAbbreviation(valueType, value) : $"{value}");
							if (!removeEmptyValues || !string.IsNullOrWhiteSpace(parameterValue))
							{
								queryStringParameters.Add($"{System.Web.HttpUtility.UrlEncode(keyValue.Key)}={System.Web.HttpUtility.UrlEncode(parameterValue)}");
							}
						}
					}
					else
					{
						var parameterValue = (ISI.Extensions.Enum.IsEnum(valueType) ? ISI.Extensions.Enum.GetAbbreviation(valueType, keyValue.Value) : $"{keyValue.Value}");
						if (!removeEmptyValues || !string.IsNullOrWhiteSpace(parameterValue))
						{
							queryStringParameters.Add($"{System.Web.HttpUtility.UrlEncode(keyValue.Key)}={System.Web.HttpUtility.UrlEncode(parameterValue)}");
						}
					}
				}

				uriBuilder.Query = string.Join("&", queryStringParameters);

				return uriBuilder;
			}
		}

		public static string ToQueryString(this IDictionary<string, string> nameValuePairs, bool removeEmptyValues = true)
		{
			var queryStringParameters = new List<string>();

			foreach (var nameValuePair in nameValuePairs)
			{
				if (!removeEmptyValues || !string.IsNullOrWhiteSpace(nameValuePair.Value))
				{
					queryStringParameters.Add($"{System.Web.HttpUtility.UrlEncode(nameValuePair.Key)}={System.Web.HttpUtility.UrlEncode(nameValuePair.Value)}");
				}
			}

			return string.Join("&", queryStringParameters);
		}

		public static string ToQueryString(this System.Collections.Specialized.NameValueCollection nameValuePairs, bool removeEmptyValues = true)
		{
			var queryStringParameters = new List<string>();

			foreach (string key in nameValuePairs.Keys)
			{
				var value = nameValuePairs[key];

				if (!removeEmptyValues || !string.IsNullOrWhiteSpace(value))
				{
					queryStringParameters.Add($"{System.Web.HttpUtility.UrlEncode(key)}={System.Web.HttpUtility.UrlEncode(value)}");
				}
			}

			return string.Join("&", queryStringParameters);
		}
	}
}
