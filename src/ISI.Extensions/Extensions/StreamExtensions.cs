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
	public static class StreamExtensions
	{
		public static void Rewind(this System.IO.Stream stream, bool flushFirst = true)
		{
			if (stream != null)
			{
				if (flushFirst)
				{
					stream.Flush();
				}

				if (stream.CanSeek)
				{
					stream.Position = 0;
				}
			}
		}

		public static byte[] ReadBytes(this System.IO.Stream sourceStream)
		{
			byte[] result = null;

			try
			{
				sourceStream.Flush();

				var position = sourceStream.Position;

				sourceStream.Rewind();

				var length = (int)sourceStream.Length;
				result = new byte[length];

				sourceStream.Read(result, 0, length);

				if (sourceStream.CanSeek)
				{
					sourceStream.Position = position;
				}
			}
			catch (Exception e)
			{
#if DEBUG
				Console.WriteLine(e.Message);
				System.Diagnostics.Debugger.Break();
#endif
			}

			return result;
		}

		public static string TextReadToEnd(this System.IO.Stream stream)
		{
			try
			{
				using (var streamReader = new System.IO.StreamReader(stream, detectEncodingFromByteOrderMarks: true))
				{
					return streamReader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
#if DEBUG
				Console.WriteLine(e.Message);
				System.Diagnostics.Debugger.Break();
#endif
			}

			return string.Empty;
		}

		public static System.IO.Stream CopyToNewStream<TStream>(this System.IO.Stream sourceStream, long startPosition = 0, int chunkSize = 2048, int maxSize = -1, bool enableWaitForBuffer = true)
			where TStream : System.IO.Stream, new()
		{
			var targetStream = new TStream();

			if (!CopyTo(sourceStream, targetStream, startPosition, chunkSize, maxSize, enableWaitForBuffer))
			{
				targetStream?.Dispose();
				targetStream = null;
			}

			targetStream?.Rewind();

			return targetStream;
		}

		public static bool CopyTo(this System.IO.Stream sourceStream, System.IO.Stream targetStream, long startPosition = 0, int chunkSize = 2048, int maxSize = -1, bool enableWaitForBuffer = true)
		{
			var buffer = new byte[chunkSize];

			try
			{
				var transferred = 0;

				if (sourceStream.CanSeek && (startPosition >= 0))
				{
					sourceStream.Flush();
					sourceStream.Position = startPosition;
				}

				var readBlocks = sourceStream.Read(buffer, 0, chunkSize);

				while ((readBlocks > 0) && ((maxSize < 0) || (transferred < maxSize)))
				{
					targetStream.Write(buffer, 0, readBlocks);

					if (enableWaitForBuffer && (readBlocks > 0) && (readBlocks < chunkSize))
					{
						System.Threading.Thread.Sleep(100);
					}

					if (readBlocks > 0)
					{
						if (maxSize > 0)
						{
							transferred += readBlocks;
						}
						readBlocks = sourceStream.Read(buffer, 0, chunkSize);
					}
				}

				targetStream.Flush();

				return true;
			}
			catch (Exception exception)
			{
#if DEBUG
				Console.WriteLine(exception.Message);
				System.Diagnostics.Debugger.Break();
#endif
				return false;
			}
		}

		public static bool TextWrite(this System.IO.Stream targetStream, string format, params object[] args)
		{
			return TextWrite(targetStream, Encoding.Default, string.Format(format, args));
		}

		public static bool TextWrite(this System.IO.Stream targetStream, System.Text.Encoding encoding, string format, params object[] args)
		{
			return TextWrite(targetStream, encoding, string.Format(format, args));
		}

		public static bool TextWrite(this System.IO.Stream targetStream, string value)
		{
			return TextWrite(targetStream, Encoding.Default, value);
		}

		public static bool TextWrite(this System.IO.Stream targetStream, System.Text.Encoding encoding, string value)
		{
			var result = false;

			var buffer = encoding.GetBytes(value);

			try
			{
				targetStream.Write(buffer, 0, buffer.Length);
				targetStream.Flush();

				result = true;
			}
			catch (Exception e)
			{
#if DEBUG
				Console.WriteLine(e.Message);
				System.Diagnostics.Debugger.Break();
#endif
				result = false;
			}

			return result;
		}

		public static string GetChecksum(this System.IO.Stream stream)
		{
			if (stream != null)
			{
				stream.Rewind(true);

				var hashAlgorithm = new System.Security.Cryptography.SHA256Managed();

				var checksum = hashAlgorithm.ComputeHash(stream);

				stream.Rewind();

				return BitConverter.ToString(checksum).Replace("-", string.Empty);
			}

			return null;
		}
	}
}
