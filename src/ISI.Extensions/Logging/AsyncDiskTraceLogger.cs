#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Logging
{
	public class AsyncDiskTraceLogger : global::Microsoft.Extensions.Logging.ILogger
	{
		protected string CategoryName { get; }

		public AsyncDiskTraceLogger(

			string categoryName)
		{
			CategoryName = categoryName;
		}

		public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel, global::Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			//var traceEvent = new ISI.Extensions.Tracing.TraceEvent()
			//{
			//	TraceEventKey = eventId.ToString(),
			//	TraceEventDateTime = DateTime.UtcNow,
			//	TraceEventType = state?.GetType().Name,
			//	Source = source.Source,
			//	MachineName = System.Environment.MachineName,
			//	Environment = source.Environment,
			//	Message = source.Message,
			//	StackTrace = System.Environment.StackTrace,
			//	Exception = source.Exception,
			//	Data = source.Data,
			//	TraceEventKeyValues = source.TraceEventKeyValues.ToNullCheckedArray(Convert),
			//	HttpContextServerVariables = source.HttpContextServerVariables.ToNullCheckedArray(Convert),
			//	ServiceModelExecutionSummary = source.ServiceModelExecutionSummary,
			//};

			//if (state is ILogState logState)
			//{
			//	traceEvent.OperationKey = logState.OperationKey;
			//	traceEvent.ActivityKey = logState.ActivityKey;
			//}

			//if (state is IHttpContextLogState httpContextLogState)
			//{
			//	traceEvent.Identity = httpContextLogState.Identity;
			//	traceEvent.HttpContextServerVariables = httpContextLogState.ServerVariables.ToNullCheckedArray(Convert);
			//	traceEvent.QueryString = httpContextLogState.QueryString;
			//	traceEvent.OperationKey = httpContextLogState.OperationKey;
			//	traceEvent.OperationKey = httpContextLogState.OperationKey;
			//	traceEvent.OperationKey = httpContextLogState.OperationKey;
			//	traceEvent.OperationKey = httpContextLogState.OperationKey;
			//	traceEvent.OperationKey = httpContextLogState.OperationKey;
			//}



			//switch (logLevel.ToTraceLevel())
			//{
			//	case ISI.Extensions.Tracing.TraceLevel.Debug:
			//		Trace.Debug(string.Format("{0}-{1}: {2}", CategoryName, eventId.ToString(), formatter(state, exception)));
			//		break;
			//	case ISI.Extensions.Tracing.TraceLevel.Information:
			//		Trace.Information(string.Format("{0}-{1}: {2}", CategoryName, eventId.ToString(), formatter(state, exception)));
			//		break;
			//	case ISI.Extensions.Tracing.TraceLevel.Warning:
			//		Trace.Warning(string.Format("{0}-{1}: {2}", CategoryName, eventId.ToString(), formatter(state, exception)));
			//		break;
			//	case ISI.Extensions.Tracing.TraceLevel.Error:
			//		var traceCode = ISI.Extensions.Tracing.TraceCodes.Default;
			//		Trace.TraceService.TraceData(System.Diagnostics.TraceEventType.Error, traceCode.ToEventId(), new ISI.Extensions.Tracing.TraceEvent(traceCode, Trace.OperationKey, Trace.ActivityUuid)
			//		{
			//			Message = string.Format("{0}-{1}: {2}", CategoryName, eventId.ToString(), formatter(state, exception)),
			//			Exception = exception
			//		});
			//		break;
			//}
		}

		public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) => true;

		public IDisposable BeginScope<TState>(TState state) => new AsyncDiskTraceLogScope();
	}

	public class AsyncDiskTraceLogger<TCategoryName> : global::Microsoft.Extensions.Logging.ILogger<TCategoryName>
	{
		protected global::Microsoft.Extensions.Logging.ILoggerFactory LoggerFactory { get; }

		private global::Microsoft.Extensions.Logging.ILogger _logger { get; }

		public AsyncDiskTraceLogger(
			global::Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
		{
			LoggerFactory = loggerFactory;
			_logger = LoggerFactory.CreateLogger(typeof(TCategoryName).FullName);
		}

		public void Log<TState>(global::Microsoft.Extensions.Logging.LogLevel logLevel, global::Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => _logger.Log<TState>(logLevel, eventId, state, exception, formatter);

		public bool IsEnabled(global::Microsoft.Extensions.Logging.LogLevel logLevel) => _logger.IsEnabled(logLevel);

		public IDisposable BeginScope<TState>(TState state) => _logger.BeginScope<TState>(state);
	}
}
