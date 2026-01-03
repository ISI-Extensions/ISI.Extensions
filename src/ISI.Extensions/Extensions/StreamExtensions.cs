#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using System.Reflection;
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

		public static void SkipToEnd(this System.IO.Stream stream)
		{
			if (stream != null)
			{
				var buffer = new byte[60 * 1024];

				while (stream.Read(buffer, 0, buffer.Length) > 0)
				{
					// Keep reading until we're at the end of the stream.
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
			catch (Exception exception)
			{
#if DEBUG
				Console.WriteLine(exception.Message);
				System.Diagnostics.Debugger.Break();
#endif
			}

			return string.Empty;
		}

		public static string ReadAsStringToEnd(this System.IO.Stream stream, System.Text.Encoding encoding = null)
		{
			encoding ??= Encoding.UTF8;

			try
			{
				var length = (int)(stream.Length - stream.Position);

				var data = new byte[length];

				stream.Read(data, 0, length);

				return encoding.GetString(data);
			}
			catch (Exception exception)
			{
#if DEBUG
				Console.WriteLine(exception.Message);
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

		public static TStruct ReadStruct<TStruct>(this System.IO.Stream stream)
			where TStruct : struct
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			var size = System.Runtime.InteropServices.Marshal.SizeOf<TStruct>();

			var data = new byte[size];
			var totalRead = 0;

			while (totalRead < size)
			{
				var read = stream.Read(data, totalRead, size - totalRead);

				if (read == 0)
				{
					break;
				}

				totalRead += read;
			}

			if (totalRead < size)
			{
				throw new InvalidOperationException("Not enough data");
			}

			// Convert from network byte order (big endian) to little endian.
			RespectEndianness<TStruct>(data);

			var pinnedData = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);

			try
			{
				var ptr = pinnedData.AddrOfPinnedObject();
				return System.Runtime.InteropServices.Marshal.PtrToStructure<TStruct>(ptr);
			}
			finally
			{
				pinnedData.Free();
			}
		}

		public static int WriteStruct<TStruct>(this System.IO.Stream stream, TStruct data)
			where TStruct : struct
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			var bytes = new byte[System.Runtime.InteropServices.Marshal.SizeOf<TStruct>()];

			var handle = System.Runtime.InteropServices.GCHandle.Alloc(bytes, System.Runtime.InteropServices.GCHandleType.Pinned);

			try
			{
				System.Runtime.InteropServices.Marshal.StructureToPtr(data, handle.AddrOfPinnedObject(), true);
			}
			finally
			{
				handle.Free();
			}

			RespectEndianness<TStruct>(bytes);

			stream.Write(bytes, 0, bytes.Length);

			return bytes.Length;
		}


		private static void RespectEndianness<T>(byte[] data)
		{
			foreach (var field in typeof(T).GetTypeInfo().DeclaredFields)
			{
				var fieldType = field.FieldType;

				if (fieldType.GetTypeInfo().IsEnum)
				{
					fieldType = System.Enum.GetUnderlyingType(fieldType);
				}

				var fieldLength = 0;
				if ((fieldType == typeof(short)) || (fieldType == typeof(ushort)))
				{
					fieldLength = 2;
				}
				else if (fieldType == typeof(int) || fieldType == typeof(uint))
				{
					fieldLength = 4;
				}

				if (fieldLength > 0)
				{
					var offset = System.Runtime.InteropServices.Marshal.OffsetOf<T>(field.Name).ToInt32();

					Array.Reverse(data, offset, fieldLength);
				}
			}
		}
	}
}
