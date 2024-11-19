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

namespace ISI.Extensions.WebClient
{
	public partial class Rest
	{
		public interface IFormData
		{

		}

		public class FormData : IFormData
		{
			public string Key { get; set; }

			public string Value
			{
				get => ((Values == null) || (Values.Length != 1) ? null : Values[0]);
				set => Values = (value == null ? null : [value]);
			}

			public string[] Values { get; set; }

			public bool UrlEncode { get; set; } = true;
		}

		public class FileFormData : IFormData
		{
			public System.IO.Stream Stream { get; set; }
			public string FileName { get; set; }
			public string FileFieldName { get; set; } = "uploadFile";
		}

		public class FormDataCollection : List<IFormData>
		{
			public System.Text.Encoding Encoding { get; set; } = System.Text.Encoding.ASCII;

			public FormDataCollection()
			{

			}
			public FormDataCollection(IEnumerable<FormData> items)
			{
				AddRange(items);
			}
			public FormDataCollection(IEnumerable<KeyValuePair<string, string>> items)
			{
				AddRange(items);
			}
			public FormDataCollection(IEnumerable<KeyValuePair<string, IEnumerable<string>>> items)
			{
				AddRange(items);
			}

			public new void AddRange(IEnumerable<FormData> items)
			{
				foreach (var item in items)
				{
					Add(item.Key, item.Values);
				}
			}

			public void AddRange(IEnumerable<KeyValuePair<string, string>> items)
			{
				foreach (var item in items)
				{
					Add(item.Key, item.Value);
				}
			}

			public void AddRange(IEnumerable<KeyValuePair<string, IEnumerable<string>>> items)
			{
				foreach (var item in items)
				{
					Add(item.Key, item.Value);
				}
			}

			public new void Add(IFormData formData)
			{
				if (formData is FileFormData)
				{
					Encoding = Encoding.UTF8;
				}

				base.Add(formData);
			}

			public FileFormData Add(System.IO.Stream stream, string fileName, string fileFieldName = "uploadFile")
			{
				var fileFormData = new FileFormData()
				{
					Stream = stream,
					FileName = fileName,
					FileFieldName = fileFieldName,
				};

				base.Add(fileFormData);

				return fileFormData;
			}

			public FormData Add(string key, string value, bool urlEncode = true)
			{
				var formData = new FormData()
				{
					Key = key,
					Value = value,
					UrlEncode = urlEncode,
				};

				this.Add(formData);

				return formData;
			}

			public FormData Add(string key, IEnumerable<string> values, bool urlEncode = true)
			{
				var formData = new FormData()
				{
					Key = key,
					Values = values.ToArray(),
					UrlEncode = urlEncode,
				};

				this.Add(formData);

				return formData;
			}

			public FormData Add(string key, int value)
			{
				var formData = new FormData()
				{
					Key = key,
					Value = string.Format("{0}", value),
				};

				this.Add(formData);

				return formData;
			}

			public FormData Add(string key, double value)
			{
				var formData = new FormData()
				{
					Key = key,
					Value = string.Format("{0}", value),
				};

				this.Add(formData);

				return formData;
			}

			public override string ToString()
			{
				var keyValues = new List<string>();

				foreach (var formData in this.Where(formData => formData is FormData))
				{
					if (formData is FormData keyValueFormData)
					{
						if (keyValueFormData.Values.Length == 0)
						{
							keyValues.Add(string.Format("{0}=", (keyValueFormData.UrlEncode ? System.Web.HttpUtility.UrlEncode(keyValueFormData.Key) : keyValueFormData.Key)));
						}
						else if (keyValueFormData.Values.Length == 1)
						{
							keyValues.Add(string.Format("{0}={1}", (keyValueFormData.UrlEncode ? System.Web.HttpUtility.UrlEncode(keyValueFormData.Key) : keyValueFormData.Key), (keyValueFormData.UrlEncode ? System.Web.HttpUtility.UrlEncode(keyValueFormData.Value) : keyValueFormData.Value)));
						}
						else if (keyValueFormData.Values.Length > 1)
						{
							foreach (var value in keyValueFormData.Values)
							{
								keyValues.Add(string.Format("{0}[]={1}", (keyValueFormData.UrlEncode ? System.Web.HttpUtility.UrlEncode(keyValueFormData.Key) : keyValueFormData.Key), (keyValueFormData.UrlEncode ? System.Web.HttpUtility.UrlEncode(value) : keyValueFormData.Value)));
							}
						}
					}
				}

				return string.Join("&", keyValues);
			}

			public void WriteEncodedRequest(System.Net.HttpWebRequest webRequest, WebRequestDetails webRequestDetails)
			{
				if (this.All(formData => formData is FormData))
				{
					var body = ToString();

					webRequest.GetRequestStream().TextWrite(Encoding, body);

					webRequestDetails?.SetFormData(this);
				}
				else
				{
					var boundary = string.Format("---------------------------{0}", Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.Base36));

					//if (webRequest.Headers["Content-Type"] != null)
					//{
					//	webRequest.Headers.Remove("Content-Type");
					//}

					webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

					using (var tempStream = new ISI.Extensions.Stream.TempFileStream())
					{
						var isFirst = true;
						foreach (var formData in this)
						{
							if (formData is FormData keyValueFormData)
							{
								if (keyValueFormData.Values.Length == 0)
								{
									tempStream.TextWrite(System.Text.Encoding.UTF8, "\r\n--{0}\r\n", boundary);
									tempStream.TextWrite(System.Text.Encoding.UTF8, "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", keyValueFormData.Key, string.Empty);
								}
								else
								{
									foreach (var value in keyValueFormData.Values)
									{
										tempStream.TextWrite(System.Text.Encoding.UTF8, "\r\n--{0}\r\n", boundary);
										tempStream.TextWrite(System.Text.Encoding.UTF8, "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", keyValueFormData.Key, value);
									}
								}
								isFirst = false;
							}
							else if (formData is FileFormData fileFormData)
							{
								if (isFirst)
								{
									tempStream.TextWrite(System.Text.Encoding.UTF8, "\r\n--{0}\r\n", boundary);
								}
								tempStream.TextWrite(System.Text.Encoding.UTF8, "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", fileFormData.FileFieldName, fileFormData.FileName, ISI.Extensions.MimeType.GetMimeType(fileFormData.FileName));

								fileFormData.Stream.Rewind();

								fileFormData.Stream.CopyTo(tempStream);

								tempStream.TextWrite(System.Text.Encoding.UTF8, "\r\n--{0}--\r\n", boundary);
							}
						}


						tempStream.Flush();
						tempStream.Rewind();

						webRequest.ContentLength = tempStream.Length;

						var requestStream = webRequest.GetRequestStream();

						var chunkSize = 1427; // any larger will cause an SSL request to fail
						tempStream.CopyTo(requestStream, chunkSize: chunkSize);

						requestStream.Flush();

						tempStream.Rewind();
						webRequestDetails?.SetBodyRaw(tempStream.TextReadToEnd());
					}
				}
			}
		}


		public static FormDataCollection DataContractSerialize(object value, string prefix = "")
		{
			var result = new FormDataCollection();

			var type = value.GetType();

			var propertyInfos = ISI.Extensions.DataContract.GetDataMemberPropertyInfos(type);

			foreach (var propertyInfo in propertyInfos.OrderBy(p => p.DataMemberAttribute.Order).ThenBy(p => p.DataMemberAttribute.Name))
			{
				var key = (string.IsNullOrEmpty(prefix) ? propertyInfo.PropertyName : string.Format("{0}[{1}]", prefix, propertyInfo.PropertyName));

				if ((propertyInfo.PropertyInfo.PropertyType == typeof(string)) || (propertyInfo.PropertyInfo.PropertyType == typeof(string)))
				{
					var propertyValue = string.Format("{0}", propertyInfo.PropertyInfo.GetValue(value));
					var addValue = true;

					if (!propertyInfo.DataMemberAttribute.EmitDefaultValue)
					{

						addValue = !string.IsNullOrEmpty(propertyValue);
					}

					if (addValue)
					{
						result.Add(key, string.Format("{0}", propertyValue));
					}
				}
				else if (propertyInfo.PropertyInfo.PropertyType == typeof(bool))
				{
					var propertyValue = (bool)propertyInfo.PropertyInfo.GetValue(value);
					var addValue = true;

					if (!propertyInfo.DataMemberAttribute.EmitDefaultValue)
					{
						addValue = propertyValue;
					}

					if (addValue)
					{
						result.Add(key, propertyValue.TrueFalse(textCase: BooleanExtensions.TextCase.Lower));
					}
				}
				else if (propertyInfo.PropertyInfo.PropertyType == typeof(bool?))
				{
					var propertyValue = (bool?)propertyInfo.PropertyInfo.GetValue(value);
					var addValue = true;

					if (!propertyInfo.DataMemberAttribute.EmitDefaultValue)
					{
						addValue = propertyValue.GetValueOrDefault();
					}

					if (addValue)
					{
						result.Add(key, propertyValue.TrueFalse(textCase: BooleanExtensions.TextCase.Lower));
					}
				}
				else if ((propertyInfo.PropertyInfo.PropertyType == typeof(DateTime)) || (propertyInfo.PropertyInfo.PropertyType == typeof(DateTime?)))
				{
					var propertyValue = propertyInfo.PropertyInfo.GetValue(value);
					var addValue = true;

					if (!propertyInfo.DataMemberAttribute.EmitDefaultValue)
					{
						if (propertyValue == null)
						{
							addValue = false;
						}
						else if (propertyInfo.DefaultValue != null)
						{
							addValue = !propertyInfo.DefaultValue.Equals(propertyValue);
						}
					}

					if (addValue)
					{
						result.Add(key, string.Format("{0}", propertyValue));
					}
				}
				else if (!propertyInfo.PropertyInfo.PropertyType.IsInterface && !propertyInfo.PropertyInfo.PropertyType.IsClass)
				{
					var propertyValue = propertyInfo.PropertyInfo.GetValue(value);
					var addValue = true;

					if (!propertyInfo.DataMemberAttribute.EmitDefaultValue)
					{
						if (propertyValue == null)
						{
							addValue = false;
						}
						else if (propertyInfo.DefaultValue != null)
						{
							addValue = !propertyInfo.DefaultValue.Equals(propertyValue);
						}
					}

					if (addValue)
					{
						result.Add(key, string.Format("{0}", propertyValue));
					}
				}
				else
				{
					result.AddRange(DataContractSerialize(propertyInfo.PropertyInfo.GetValue(value), key));
				}
			}

			return result;
		}
	}
}