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
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static partial class JavascriptHelper
	{
		#region DynamicPagesHelper
		//private static ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper _dynamicPagesHelper = null;
		//public static ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper DynamicPagesHelper => _dynamicPagesHelper ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.AspNetCore.Mvc.Areas.DynamicPages.DynamicPagesHelper>();
		#endregion

		private static Configuration _configuration = null;
		private static Configuration Configuration=> _configuration ??= ISI.Extensions.ServiceLocator.Current.GetService<Configuration>(() => new ISI.Extensions.DependencyInjection.RegistrationDeclarationByMapToType()
		{
			MapToType = typeof(Configuration),
			ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
		}); 

		public static readonly string JavascriptCollectionKey = $"JavascriptCollection-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)}";

		#region EncryptCookie
		private static bool? _encryptCookie = null;
		private static bool EncryptCookie
		{
			get
			{
				if (!_encryptCookie.HasValue)
				{
					switch (Configuration.DynamicPages.EncryptCookie)
					{
						case ISI.Extensions.Configuration.EnabledStatus.Default:
							_encryptCookie = false;
							break;

						case ISI.Extensions.Configuration.EnabledStatus.Always:
							_encryptCookie = true;
							break;

						case ISI.Extensions.Configuration.EnabledStatus.Never:
							_encryptCookie = false;
							break;

						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				return _encryptCookie.GetValueOrDefault();
			}
		}
		#endregion

		#region IsCombineJavaScriptEnabled
		private static bool? _isCombineJavaScriptEnabled = null;
		private static bool IsCombineJavaScriptEnabled
		{
			get
			{
				if (!_isCombineJavaScriptEnabled.HasValue)
				{
					switch (Configuration.DynamicPages.JavaScriptCombine)
					{
						case ISI.Extensions.Configuration.EnabledStatus.Default:
							_isCombineJavaScriptEnabled = false;
							break;

						case ISI.Extensions.Configuration.EnabledStatus.Always:
							_isCombineJavaScriptEnabled = true;
							break;

						case ISI.Extensions.Configuration.EnabledStatus.Never:
							_isCombineJavaScriptEnabled = false;
							break;

						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				return _isCombineJavaScriptEnabled.GetValueOrDefault();
			}
		}
		#endregion

		#region GetJavaScriptUrls
		public class JavaScriptDictionary
		{
			private readonly Dictionary<bool, Dictionary<bool, HtmlHelpers.ContentUrlCollection>> _urls;

			public JavaScriptDictionary()
			{
				_urls = new();

				Clear();
			}

			public HtmlHelpers.ContentUrlCollection this[bool defer, bool async] => _urls[defer][async];

			public void Clear()
			{
				_urls.Clear();

				_urls.Add(false, new());
				_urls[false].Add(false, new());
				_urls[false].Add(true, new());

				_urls.Add(true, new());
				_urls[true].Add(false, new());
				_urls[true].Add(true, new());
			}
		}
		public static JavaScriptDictionary GetJavaScriptUrls(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper)
		{
			var viewData = htmlHelper.ViewContext.ViewData;

			if (!(viewData[JavascriptCollectionKey] is JavaScriptDictionary javaScriptUrls))
			{
				javaScriptUrls = new();

				viewData[JavascriptCollectionKey] = javaScriptUrls;
			}

			return javaScriptUrls;
		}
		#endregion
	}
}
