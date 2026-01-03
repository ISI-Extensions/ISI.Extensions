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
using System.Text;

namespace ISI.Extensions
{
	public static class MimeTypes
	{
		public const string PlainText = "text/plain";
		public const string Html = "text/html";
		public const string Xml = "application/xml";
		public const string TextXml = "text/plain,application/xhtml+xml,application/xml";
		public const string Json = "application/json";
	}

	public class MimeType
	{
		private static IDictionary<string, string> _knownMimeTypes = null;
		public static IDictionary<string, string> KnownMimeTypes => _knownMimeTypes ??= GetMimeTypes();

		private static IDictionary<string, string> GetMimeTypes()
		{
			var knownMimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

			knownMimeTypes.Add("aac", "audio/aac");
			knownMimeTypes.Add("abw", "application/x-abiword");
			knownMimeTypes.Add("arc", "application/x-freearc");
			knownMimeTypes.Add("avi", "video/x-msvideo");
			knownMimeTypes.Add("azw", "application/vnd.amazon.ebook");
			knownMimeTypes.Add("bin", "application/octet-stream");
			knownMimeTypes.Add("bmp", "image/bmp");
			knownMimeTypes.Add("bz", "application/x-bzip");
			knownMimeTypes.Add("bz2", "application/x-bzip2");
			knownMimeTypes.Add("csh", "application/x-csh");
			knownMimeTypes.Add("css", "text/css");
			knownMimeTypes.Add("csv", "text/csv");
			knownMimeTypes.Add("doc", "application/msword");
			knownMimeTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
			knownMimeTypes.Add("eot", "application/vnd.ms-fontobject");
			knownMimeTypes.Add("epub", "application/epub+zip");
			knownMimeTypes.Add("gz", "application/gzip");
			knownMimeTypes.Add("gif", "image/gif");
			knownMimeTypes.Add("htm", ISI.Extensions.MimeTypes.Html);
			knownMimeTypes.Add("html", ISI.Extensions.MimeTypes.Html);
			knownMimeTypes.Add("ico", "image/vnd.microsoft.icon");
			knownMimeTypes.Add("ics", "text/calendar");
			knownMimeTypes.Add("jar", "application/java-archive");
			knownMimeTypes.Add("jpeg", "image/jpeg");
			knownMimeTypes.Add("jpg", "image/jpeg");
			knownMimeTypes.Add("js", "text/x-javascript");
			knownMimeTypes.Add("json", ISI.Extensions.MimeTypes.Json);
			knownMimeTypes.Add("jsonld", "application/ld+json");
			knownMimeTypes.Add("mid", "audio/midi");
			knownMimeTypes.Add("midi", "audio/midi");
			knownMimeTypes.Add("mjs", "text/javascript");
			knownMimeTypes.Add("mp3", "audio/mpeg");
			knownMimeTypes.Add("mpeg", "video/mpeg");
			knownMimeTypes.Add("mpkg", "application/vnd.apple.installer+xml");
			knownMimeTypes.Add("odp", "application/vnd.oasis.opendocument.presentation");
			knownMimeTypes.Add("ods", "application/vnd.oasis.opendocument.spreadsheet");
			knownMimeTypes.Add("odt", "application/vnd.oasis.opendocument.text");
			knownMimeTypes.Add("oga", "audio/ogg");
			knownMimeTypes.Add("ogv", "video/ogg");
			knownMimeTypes.Add("ogx", "application/ogg");
			knownMimeTypes.Add("opus", "audio/opus");
			knownMimeTypes.Add("otf", "font/otf");
			knownMimeTypes.Add("png", "image/png");
			knownMimeTypes.Add("pdf", "application/pdf");
			knownMimeTypes.Add("php", "application/x-httpd-php");
			knownMimeTypes.Add("ppt", "application/vnd.ms-powerpoint");
			knownMimeTypes.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
			knownMimeTypes.Add("rar", "application/vnd.rar");
			knownMimeTypes.Add("rtf", "application/rtf");
			knownMimeTypes.Add("sh", "application/x-sh");
			knownMimeTypes.Add("svg", "image/svg+xml");
			knownMimeTypes.Add("swf", "application/x-shockwave-flash");
			knownMimeTypes.Add("tar", "application/x-tar");
			knownMimeTypes.Add("tif", "image/tiff");
			knownMimeTypes.Add("tiff", "image/tiff");
			knownMimeTypes.Add("ts", "video/mp2t");
			knownMimeTypes.Add("ttf", "font/ttf");
			knownMimeTypes.Add("txt", ISI.Extensions.MimeTypes.PlainText);
			knownMimeTypes.Add("vsd", "application/vnd.visio");
			knownMimeTypes.Add("wav", "audio/wav");
			knownMimeTypes.Add("weba", "audio/webm");
			knownMimeTypes.Add("webm", "video/webm");
			knownMimeTypes.Add("webp", "image/webp");
			knownMimeTypes.Add("woff", "font/woff");
			knownMimeTypes.Add("woff2", "font/woff2");
			knownMimeTypes.Add("xhtml", "application/xhtml+xml");
			knownMimeTypes.Add("xls", "application/vnd.ms-excel");
			knownMimeTypes.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
			knownMimeTypes.Add("xml", "text/xml");
			knownMimeTypes.Add("xul", "application/vnd.mozilla.xul+xml");
			knownMimeTypes.Add("zip", "application/zip");
			knownMimeTypes.Add("7z", "application/x-7z-compressed");

			knownMimeTypes.Add("scss", "text/css");
			knownMimeTypes.Add("scssx", "text/css");
			knownMimeTypes.Add("csscss", "text/css");
			knownMimeTypes.Add("vbscss", "text/css");
			knownMimeTypes.Add("less", "text/css");
			knownMimeTypes.Add("lessx", "text/css");
			knownMimeTypes.Add("csless", "text/css");
			knownMimeTypes.Add("vbless", "text/css");
			knownMimeTypes.Add("cssx", "text/css");
			knownMimeTypes.Add("cscss", "text/css");
			knownMimeTypes.Add("vbcss", "text/x-javascript");
			knownMimeTypes.Add("jsx", "text/x-javascript");
			knownMimeTypes.Add("csjs", "text/x-javascript");
			knownMimeTypes.Add("vbjs", "text/x-javascript");

			return knownMimeTypes;
		}

		public static string GetMimeType(string fileName)
		{
			{
				if (KnownMimeTypes.TryGetValue(fileName, out var mimeType))
				{
					return mimeType;
				}
			}

			{
				var fileExtension = System.IO.Path.GetExtension(fileName).Trim('.');

				if (KnownMimeTypes.TryGetValue(fileExtension, out var mimeType))
				{
					return mimeType;
				}
			}

			return null;
		}
	}
}