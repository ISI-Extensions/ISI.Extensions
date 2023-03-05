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

namespace ISI.Extensions.ConfigurationHelper
{
	public class ClassicConnectionStringsSectionConfigurationFileParser
	{
		private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private ClassicConnectionStringsSectionConfigurationFileParser()
		{
		}

		public static IDictionary<string, string> Parse(System.IO.Stream stream) => new ClassicConnectionStringsSectionConfigurationFileParser().ParseStream(stream);

		private IDictionary<string, string> ParseStream(System.IO.Stream stream)
		{
			_data.Clear();

			using (System.IO.TextReader textReader = new System.IO.StreamReader(stream, Encoding.UTF8))
			{
				var configurationXml = textReader.ReadToEnd();

				var index = configurationXml.IndexOf("<connectionStrings.", StringComparison.InvariantCulture);
				if (index >= 0)
				{
					var elementName = configurationXml.Substring(index + 1);
					index = elementName.IndexOf(">", StringComparison.InvariantCultureIgnoreCase);
					if (index > 0)
					{
						elementName = elementName.Substring(0, index);

						configurationXml = configurationXml.Replace(elementName, "connectionStrings");
					}
				}

				using (var stringReader = new System.IO.StringReader(configurationXml))
				{
					using (var xmlTextReader = new System.Xml.XmlTextReader(stringReader))
					{
						var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ConnectionStrings));

						if (xmlSerializer.Deserialize(xmlTextReader) is ConnectionStrings connectionStrings)
						{
							foreach (var connection in connectionStrings.Connections)
							{
								_data.Add(string.Format("ConnectionStrings:{0}", connection.Name), connection.ConnectionString);
							}
						}
					}
				}
			}
			
			return _data;
		}
		
		/// <remarks/>
		[System.Serializable]
		[System.ComponentModel.DesignerCategory("code")]
		[System.Xml.Serialization.XmlType(AnonymousType = true)]
		[System.Xml.Serialization.XmlRoot(Namespace = "", ElementName = "connectionStrings", IsNullable = false)]
		public class ConnectionStrings
		{
			[System.Xml.Serialization.XmlElement("add")]
			public Connection[] Connections { get; set; }
		}

		/// <remarks/>
		[System.Serializable]
		[System.ComponentModel.DesignerCategory("code")]
		[System.Xml.Serialization.XmlType(AnonymousType = true)]
		public class Connection
		{
			[System.Xml.Serialization.XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			[System.Xml.Serialization.XmlAttribute(AttributeName = "connectionString")]
			public string ConnectionString { get; set; }

			[System.Xml.Serialization.XmlAttribute(AttributeName = "providerName")]
			public string ProviderName { get; set; }
		}
	}
}

