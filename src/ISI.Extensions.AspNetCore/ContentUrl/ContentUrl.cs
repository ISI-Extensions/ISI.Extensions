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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ISI.Extensions.AspNetCore
{
	public class ContentUrl : IContentUrl
	{
		public string VirtualPath { get; }
		public bool IsExternalContent { get; }

		protected string Url { get; private set; }
		protected string CdnUrl { get; private set; }

		private static string _rootUrl = null;
		protected string RootUrl => _rootUrl ??= "~/";

		public ContentUrl(string url, bool isOriginalUrl = false)
		{
			Url = url;

			//url = url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

			//if (!isOriginalUrl)
			//{
			//	url = ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetOriginalUrl(url);
			//	url = url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
			//}

			//if (url.StartsWith(RootUrl, StringComparison.InvariantCultureIgnoreCase))
			//{
			//	Url = null;
			//	VirtualPath = System.Web.HttpUtility..VirtualPathUtility.ToAppRelative(url);
			//}
			//else if (System.Web.VirtualPathUtility.IsAppRelative(url))
			//{
			//	Url = null;
			//	VirtualPath = url;
			//}
			//else
			//{
			//	IsExternalContent = true;
			//}
		}

		public string GetUrl(bool considerContentDistributionNetwork = true)
		{
			//	if (IsExternalContent)
			//	{
			//		return Url;
			//	}

			//	var url = Url ??= System.Web.VirtualPathUtility.ToAbsolute(VirtualPath);

			//	if (considerContentDistributionNetwork)
			//	{
			//		url = CdnUrl ??= ISI.Extensions.AspNetCore.ContentDistributionNetwork.GetUrl(url);
			//	}

			//	var cacheBusterKey = GetCacheBusterKey();

			//	if (!string.IsNullOrEmpty(cacheBusterKey))
			//	{
			//		url = string.Format("{0}{1}{2}", url, (url.IndexOf("?") > 0 ? "&" : "?"), cacheBusterKey);
			//	}

			//	return url;
			return Url;
		}

		public string GetCacheBusterKey()
		{
			//if (!string.IsNullOrEmpty(VirtualPath))
			//{
			//	if (System.Web.Hosting.HostingEnvironment.VirtualPathProvider.FileExists(VirtualPath))
			//	{
			//		var virtualFile = System.Web.Hosting.HostingEnvironment.VirtualPathProvider.GetFile(VirtualPath) as ISI.Extensions.AspNetCore.WebVirtualPathProvider.WebVirtualFile;

			//		return virtualFile?.GetVersionKey();
			//	}
			//}

			return null;
		}

		public override string ToString()
		{
			return GetUrl(true);
		}

		public string ToHtmlString()
		{
			return ToString();
		}

		public void WriteTo(TextWriter writer, HtmlEncoder encoder)
		{
			writer.Write(Url);
		}
	}
}
