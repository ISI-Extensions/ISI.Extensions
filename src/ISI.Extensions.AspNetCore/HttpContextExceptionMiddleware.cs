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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.AspNetCore
{
	public class HttpContextExceptionMiddleware
	{
		private static Microsoft.Extensions.Logging.ILogger _logger = null;
		public static Microsoft.Extensions.Logging.ILogger Logger => _logger ??= ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();

		protected readonly Microsoft.AspNetCore.Http.RequestDelegate Next;

		public HttpContextExceptionMiddleware(Microsoft.AspNetCore.Http.RequestDelegate next)
		{
			Next = next;
		}

		public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
		{
			try
			{
				await Next.Invoke(context);
			}
			catch (Exception exception)
			{
				var httpContextHelper = new HttpContextHelper(context, null, null);

				var httpContextLogState = new HttpContextLogState(Logger.OperationKey(), Logger.ActivityKey(), httpContextHelper.Identity, httpContextHelper.ServerVariables, httpContextHelper.QueryString, httpContextHelper.FormValues, httpContextHelper.Cookies, httpContextHelper.VisitorUuid, httpContextHelper.VisitUuid);

				Logger?.Log(LogLevel.Error, new EventId(1), httpContextLogState, exception, Formatter);
			}
		}

		private string Formatter(HttpContextLogState httpContextLogState, System.Exception exception) => exception.ErrorMessageFormatted();
	}
}