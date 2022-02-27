#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
		public class DelimitedTextParserContext : ITextParserContext
		{
			public int BufferSize { get; private set; }
			public char[] Buffer { get; }
			public int BufferOffset { get; set; }

			public bool Primed { get; private set; }
			public bool EndOfStream => (Primed && (Buffer[0] == '\0'));

			public DelimitedTextParserContext()
			{
				BufferSize = 2048;
				Buffer = new char[BufferSize];
			}

			public void ReadBlock(System.IO.StreamReader stream)
			{
				if (stream.EndOfStream)
				{
					Buffer[0] = '\0';
					BufferSize = 1;
				}
				else
				{
					BufferSize = stream.ReadBlock(Buffer, 0, BufferSize);
				}

				BufferOffset = 0;

				Primed = true;
			}

			public void IncrementBufferOffset(System.IO.StreamReader stream)
			{
				BufferOffset++;
				if (BufferOffset >= BufferSize)
				{
					ReadBlock(stream);
				}
			}

			bool ITextParserContext.EndOfSource => EndOfStream;
		}

		public ITextParserResponse Read(ITextParserContext context, string source)
		{
			using (var memoryStream = new System.IO.MemoryStream())
			{
				memoryStream.TextWrite(source);

				memoryStream.Rewind();

				using (var streamReader = new System.IO.StreamReader(memoryStream))
				{
					return Read(context, streamReader);
				}
			}
		}

		public char Delimiter { get; }

		public DelimitedTextParser(char delimiter)
		{
			Delimiter = delimiter;
		}

		public ITextParserContext CreateTextParserContext() => new DelimitedTextParserContext();

		public ITextParserResponse Read(ITextParserContext context, System.IO.StreamReader stream)
		{
			if (!(context is DelimitedTextParserContext delimitedTextParserContext))
			{
				throw new Exception("context must not be null and of type DelimitedTextParserContext");
			}

			var source = string.Empty;
			var values = new List<string>();

			if (!delimitedTextParserContext.EndOfStream)
			{
				if (!delimitedTextParserContext.Primed)
				{
					delimitedTextParserContext.ReadBlock(stream);
				}

				var endOfLine = false;
				var isInQuotes = false;

				var sourceValue = string.Empty;
				const int sourceBufferSize = 2048;
				var sourceBuffer = new char[sourceBufferSize];
				var sourceBufferOffset = 0;

#if DEBUG
				var debugSource = string.Empty;
#endif

				void initializeSource()
				{
					sourceValue = string.Empty;
					sourceBufferOffset = 0;
				}

				void addToSourceValue(char sourcePiece)
				{
#if DEBUG
					debugSource = string.Format("{0}{1}", debugSource, sourcePiece);
#endif

					sourceBuffer[sourceBufferOffset] = sourcePiece;

					sourceBufferOffset++;

					if (sourceBufferOffset >= sourceBufferSize)
					{
						sourceValue = string.Format("{0}{1}", sourceValue, new string(sourceBuffer, 0, sourceBufferOffset));
						sourceBufferOffset = 0;
					}
				}

				string getSourceValue()
				{
					if (sourceBufferOffset > 0)
					{
						return string.Format("{0}{1}", sourceValue, new string(sourceBuffer, 0, sourceBufferOffset));
					}

					return sourceValue;
				}

				initializeSource();

				var fieldValue = string.Empty;
				const int fieldBufferSize = 2048;
				var fieldBuffer = new char[fieldBufferSize];
				var fieldBufferOffset = 0;

				void initializeField()
				{
					fieldValue = string.Empty;
					fieldBufferOffset = 0;
				}

				void addToFieldValue(char fieldPiece)
				{
					fieldBuffer[fieldBufferOffset] = fieldPiece;

					fieldBufferOffset++;

					if (fieldBufferOffset >= fieldBufferSize)
					{
						fieldValue = string.Format("{0}{1}", fieldValue, new string(fieldBuffer, 0, fieldBufferOffset));
						fieldBufferOffset = 0;
					}
				}

				string getFieldValue()
				{
					if (fieldBufferOffset > 0)
					{
						return string.Format("{0}{1}", fieldValue, new string(fieldBuffer, 0, fieldBufferOffset));
					}

					return fieldValue;
				}

				initializeField();

				var startOfField = true;
				var endOfField = false;

				while (!endOfLine)
				{
					if (startOfField && (isInQuotes = delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset] == '"'))
					{
						delimitedTextParserContext.IncrementBufferOffset(stream);
						addToSourceValue('"');
					}

					startOfField = false;

					addToSourceValue(delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset]);

					switch (delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset])
					{
						case '"':
							delimitedTextParserContext.IncrementBufferOffset(stream);
							if (delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset] == '"')
							{
								delimitedTextParserContext.IncrementBufferOffset(stream);
								addToFieldValue('"');
								addToSourceValue('"');
							}
							else
							{
								isInQuotes = false;
							}
							break;

						case '\r':
						case '\n':
							if (isInQuotes)
							{
								addToFieldValue(delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset]);
								delimitedTextParserContext.IncrementBufferOffset(stream);
							}
							else
							{
								endOfLine = true;
								while ((delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset] == '\r') || (delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset] == '\n'))
								{
									addToSourceValue(delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset]);
									delimitedTextParserContext.IncrementBufferOffset(stream);
								}
							}
							break;

						default:
							if (!isInQuotes && (delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset] == Delimiter))
							{
								endOfField = true;
							}
							else
							{
								addToFieldValue(delimitedTextParserContext.Buffer[delimitedTextParserContext.BufferOffset]);
							}
							delimitedTextParserContext.IncrementBufferOffset(stream);
							break;
					}

					if (delimitedTextParserContext.EndOfStream)
					{
						endOfLine = true;
					}

					if (endOfLine)
					{
						endOfField = true;
						source = getSourceValue();
						initializeSource();
					}

					if (endOfField)
					{
						values.Add(getFieldValue());
						initializeField();

						startOfField = true;
						endOfField = false;
						isInQuotes = false;
					}
				}
			}

			return new TextParserResponse(values.Any(), source, values);
		}

		public string GetUnparsed(IEnumerable<string> recordValues)
		{
			return string.Join(string.Format("{0}", Delimiter), recordValues.Select(recordValue =>
			{
				var value = string.Format("{0}", recordValue);

				value = value.Replace("\"", "\"\"");

				if ((value.IndexOf(Delimiter) >= 0) || (value.IndexOf('\r') >= 0) || (value.IndexOf('\n') >= 0))
				{
					value = string.Format("\"{0}\"", value);
				}

				return value;
			}));
		}
	}
}