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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.GetPropertiesResponse GetProperties(DTOs.GetPropertiesRequest request)
		{
			var response = new DTOs.GetPropertiesResponse();

			if (SvnIsInstalled)
			{
				var properties = new List<(string Path, IEnumerable<KeyValuePair<string, string>> Properties)>();

				var arguments = new List<string>();

				arguments.Add("proplist");
				arguments.Add("-v");

				switch (request.Depth)
				{
					case Depth.Empty:
						arguments.Add("--depth empty");
						break;
					case Depth.Files:
						arguments.Add("--depth files");
						break;
					case Depth.Children:
						arguments.Add("--depth immediates");
						break;
					case Depth.Infinity:
						arguments.Add("--depth infinity");
						break;
				}

				arguments.Add(string.Format("\"{0}\"", request.Source.TrimEnd(System.IO.Path.DirectorySeparatorChar)));

				AddCredentials(arguments, request);

				var content = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = new NullLogger(),
					ProcessExeFullName = "svn",
					Arguments = arguments.ToArray(),
				}).Output;

				var contentItems = new Queue<string>(content.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));

				var pathPropertyKey = string.Empty;
				var pathProperties = (IDictionary<string, string>)null;

				while (contentItems.Any())
				{
					var contentItem = contentItems.Dequeue();

					if ((contentItem.Length > 0) && (contentItem[0] != ' '))
					{
						pathProperties = new Dictionary<string, string>();
						properties.Add((Path: contentItem.Split(new string[] { "'" }, StringSplitOptions.RemoveEmptyEntries)[1], pathProperties));
					}
					else if ((contentItem.Length > 2) && (contentItem.Substring(0, 2) == "  ") && (contentItem[2] != ' '))
					{
						if (pathProperties != null)
						{
							pathPropertyKey = contentItem.Trim();
							pathProperties.Add(pathPropertyKey, string.Empty);
						}
					}
					else if ((contentItem.Length > 4) && (contentItem.Substring(0, 4) == "    ") && (contentItem[4] != ' '))
					{
						if (!string.IsNullOrWhiteSpace(pathPropertyKey) && (pathProperties != null))
						{
							pathProperties[pathPropertyKey] += string.Format("{0}\n", contentItem.Trim());
						}
					}
				}

				response.Properties = properties;
			}

			return response;
		}
	}
}