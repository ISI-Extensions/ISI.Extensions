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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static class TimeZoneHelpers
	{
		private static ISI.Extensions.DateTimeStamper.IDateTimeStamper _dateTimeStamper = null;
		private static ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper => _dateTimeStamper ??= (ISI.Extensions.ServiceLocator.Current?.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>() ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper());

		public static readonly string TimeZoneCookieName = "TimeZone";

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
		{
			if (ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo == null)
			{
				if (cookies.TryGetValue(TimeZoneCookieName, out var cookieValue))
				{
					try
					{
						ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo = ISI.Extensions.Extensions.TimeZoneInfoHelper.FindSystemTimeZoneByOlsonTimeZone(cookieValue);
					}
					catch
					{
					}
				}
			}

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		private static void SetTimeZoneInfoFromObject(object @object)
		{
			if ((ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo == null) && (@object is IHasTimeZoneName hasTimeZoneName))
			{
				var timeZone = TimeZoneInfoHelper.GetTimeZone(hasTimeZoneName.TimeZoneName);

				if (!string.IsNullOrEmpty(timeZone))
				{
					ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById(timeZone);
				}
			}

			if ((ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo == null) && (@object is IHasTimeZoneInfo))
			{
				ISI.Extensions.Extensions.TimeZoneInfoHelper.TimeZoneInfo = ((IHasTimeZoneInfo)@object).TimeZoneInfo;
			}
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(this System.Security.Principal.IPrincipal user)
		{
			SetTimeZoneInfoFromObject(user.Identity);

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper)
		{
			SetTimeZoneInfoFromObject(htmlHelper.ViewData.Model);

			htmlHelper.ViewContext.HttpContext.User.GetTimeZoneInfo();

			htmlHelper.ViewContext.HttpContext.Request.Cookies.GetTimeZoneInfo();

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Mvc.Filters.ResultExecutingContext filterContext)
		{
			filterContext.HttpContext.User.GetTimeZoneInfo();

			filterContext.HttpContext.Request.Cookies.GetTimeZoneInfo();

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext filterContext)
		{
			//filterContext.Controller.GetTimeZoneInfo();

			filterContext.HttpContext.User.GetTimeZoneInfo();

			filterContext.HttpContext.Request.Cookies.GetTimeZoneInfo();

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		//public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Mvc.ControllerBase controller)
		//{
		//	SetTimeZoneInfoFromObject(controller.ViewData.Model);

		//	controller.ControllerContext.HttpContext.User.GetTimeZoneInfo();

		//	controller.ControllerContext.HttpContext.Request.Cookies.GetTimeZoneInfo();

		//	return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		//}

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Mvc.Controller controller)
		{
			controller.HttpContext.User.GetTimeZoneInfo();

			controller.HttpContext.Request.Cookies.GetTimeZoneInfo();

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(this Microsoft.AspNetCore.Http.HttpRequest request)
		{
			request.HttpContext.User.GetTimeZoneInfo();

			request.Cookies.GetTimeZoneInfo();

			return ISI.Extensions.Extensions.TimeZoneInfoHelper.GetTimeZoneInfo();
		}

		public static readonly string TimeOffsetCookieName = "TimeOffset";

		[ThreadStatic]
		private static TimeSpan? _timeOffset;

		public static TimeSpan GetTimeOffset(this Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
		{
			if (!_timeOffset.HasValue)
			{
				if (cookies.TryGetValue(TimeOffsetCookieName, out var cookieValue))
				{
					var offset = cookieValue.ToIntNullable();

					if (offset != null)
					{
						_timeOffset = new TimeSpan(offset.Value, 0, 0);
					}
				}
			}

			_timeOffset ??= System.TimeZone.CurrentTimeZone.GetUtcOffset(DateTimeStamper.CurrentDateTime());

			return _timeOffset.GetValueOrDefault();
		}

		public static TimeSpan GetTimeOffset(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper htmlHelper)
		{
			return htmlHelper.ViewContext.HttpContext.Request.Cookies.GetTimeOffset();
		}

		public static TimeSpan GetTimeOffset(this Microsoft.AspNetCore.Mvc.Controller controller)
		{
			return controller.HttpContext.Request.Cookies.GetTimeOffset();
		}

		public static TimeSpan GetTimeOffset(this Microsoft.AspNetCore.Http.HttpRequest request)
		{
			return request.Cookies.GetTimeOffset();
		}
	}
}
