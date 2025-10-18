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
using System.Threading.Tasks;

namespace ISI.Extensions
{
	public class TextParserFactory
	{
		public enum TextDelimiter
		{
			CommaSeparatedValues,
			PipeDelimitedValues,
			TabDelimitedValues
		}

		internal static char GetTextDelimiter(TextDelimiter textDelimiter)
		{
			switch (textDelimiter)
			{
				case TextDelimiter.CommaSeparatedValues:
					return ',';
				case TextDelimiter.PipeDelimitedValues:
					return '|';
				case TextDelimiter.TabDelimitedValues:
					return '\t';
				default:
					throw new ArgumentOutOfRangeException(nameof(textDelimiter), textDelimiter, null);
			}
		}

		internal static readonly IDictionary<string, TextDelimiter> FileExtensionToTextDelimiter = new Dictionary<string, TextDelimiter>(StringComparer.InvariantCultureIgnoreCase)
		{
			{"csv", TextDelimiter.CommaSeparatedValues},
			{"psv", TextDelimiter.PipeDelimitedValues},
			{"tsv", TextDelimiter.TabDelimitedValues},
			{"tab", TextDelimiter.TabDelimitedValues},
		};

		internal static TextDelimiter GetTextDelimiterByFileName(string fileName)
		{
			var fileExtension = fileName.Split(['?'], StringSplitOptions.RemoveEmptyEntries).First().Split(new [] { '.' }).Last();

			if (FileExtensionToTextDelimiter.ContainsKey(fileExtension))
			{
				return FileExtensionToTextDelimiter[fileExtension];
			}

			throw new($"Delimited parser for file Extension: \"{fileExtension}\" not found");
		}

		public static ISI.Extensions.Parsers.ITextParser GetTextParser(TextDelimiter textDelimiter)
		{
			return new ISI.Extensions.Parsers.DelimitedTextParser(GetTextDelimiter(textDelimiter));
		}

		public static ISI.Extensions.Parsers.ITextParser GetTextParser(char textDelimiter)
		{
			return new ISI.Extensions.Parsers.DelimitedTextParser(textDelimiter);
		}

		public static ISI.Extensions.Parsers.ITextParser GetTextParserByFileName(string fileName)
		{
			return GetTextParser(GetTextDelimiterByFileName(fileName));
		}
	}
}
