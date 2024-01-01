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

namespace ISI.Extensions.Extensions
{
	public static class XElementExtensions
	{
		public static IEnumerable<System.Xml.Linq.XAttribute> GetAttributesByLocalName(this IEnumerable<System.Xml.Linq.XAttribute> xAttributes, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xAttributes.NullCheckedWhere(attribute => string.Equals(attribute.Name.LocalName, localName, stringComparison));
		}
		public static IEnumerable<System.Xml.Linq.XAttribute> GetAttributesByLocalName(this System.Xml.Linq.XElement xElement, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElement?.Attributes()?.GetAttributesByLocalName(localName, stringComparison);
		}
		public static System.Xml.Linq.XAttribute GetAttributeByLocalName(this IEnumerable<System.Xml.Linq.XAttribute> xAttributes, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xAttributes.NullCheckedFirstOrDefault(attribute => string.Equals(attribute.Name.LocalName, localName, stringComparison));
		}
		public static System.Xml.Linq.XAttribute GetAttributeByLocalName(this System.Xml.Linq.XElement xElement, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElement?.Attributes()?.GetAttributeByLocalName(localName, stringComparison);
		}

		public static IEnumerable<System.Xml.Linq.XElement> GetElementsByLocalName(this IEnumerable<System.Xml.Linq.XElement> xElements, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElements.NullCheckedWhere(attribute => string.Equals(attribute.Name.LocalName, localName, stringComparison));
		}
		public static IEnumerable<System.Xml.Linq.XElement> GetElementsByLocalName(this System.Xml.Linq.XElement xElement, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElement?.Elements()?.GetElementsByLocalName(localName, stringComparison);
		}
		public static System.Xml.Linq.XElement GetElementByLocalName(this IEnumerable<System.Xml.Linq.XElement> xElements, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElements.NullCheckedFirstOrDefault(attribute => string.Equals(attribute.Name.LocalName, localName, stringComparison));
		}
		public static System.Xml.Linq.XElement GetElementByLocalName(this System.Xml.Linq.XElement xElement, string localName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return xElement?.Elements()?.GetElementByLocalName(localName, stringComparison);
		}
	}
}
