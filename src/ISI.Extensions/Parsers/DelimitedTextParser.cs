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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Parsers
{
	public class DelimitedTextParser : ITextParser
	{
		public char Delimiter { get; }
		public char TextQualifier { get; }

		public DelimitedTextParser(char delimiter)
		{
			Delimiter = delimiter;
			TextQualifier = '\"';
		}

		public ITextParserResponse Read(string source)
		{
			ITextParserResponse result = null;

			if (!string.IsNullOrEmpty(source))
			{
				var values = new List<string>();

				var sourceIndex = 0;
				var sourceLength = source.Length;
				var fieldLength = 0;

				void AddField()
				{
					if (fieldLength > 0)
					{
						var value = source.Substring(sourceIndex, fieldLength);

						if (value[0] == TextQualifier)
						{
							var textLength = value.Length - 1;

							while ((textLength > 0) && (value[textLength] != TextQualifier))
							{
								textLength--;
							}

							value = (textLength > 0 ? value.Substring(1, textLength - 1) : value.Substring(1));
						}
						else
						{
							value = value.Trim();
						}

						values.Add(value);

						sourceIndex += fieldLength;
						fieldLength = 0;
					}
					else
					{
						values.Add(string.Empty);
					}

					sourceIndex++;
				}

				var isInTextField = false;
				while ((sourceIndex + fieldLength) < sourceLength)
				{
					var cursor = source[sourceIndex + fieldLength];

					if (cursor == TextQualifier)
					{
						if (isInTextField)
						{
							isInTextField = false;
						}
						else
						{
							while (source[sourceIndex] == ' ')
							{
								sourceIndex++;
								fieldLength--;
							}
							isInTextField = true;
						}
						fieldLength++;
					}
					else if(!isInTextField && (cursor == Delimiter))
					{
						AddField();
					}
					else
					{
						fieldLength++;
					}
				}

				if (sourceIndex < sourceLength)
				{
					AddField();
				}

				//var values = source.Split(new[] { Delimiter }, StringSplitOptions.None);

				result = new TextParserResponse(true, source, values);
			}

			return result ?? new TextParserResponse(false, null, null);
		}

		public ITextParserResponse Read(System.IO.StreamReader stream)
		{
			ITextParserResponse result = null;

			var continueProcessing = true;

			while (continueProcessing)
			{
				continueProcessing = false;
				if (!stream.EndOfStream)
				{
					var line = stream.ReadLine();

					if (string.IsNullOrEmpty(line))
					{
						continueProcessing = true;
					}
					else
					{
						result = Read(line);
					}
				}
			}

			return result ?? new TextParserResponse(false, null, null);
		}
	}
}