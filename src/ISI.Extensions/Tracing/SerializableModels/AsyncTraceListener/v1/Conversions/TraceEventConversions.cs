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

namespace ISI.Extensions.Tracing.SerializableModels.AsyncTraceListener.v1.Conversions
{
	public static class TraceEventConversions
	{
		public static TraceEvent Convert(ISI.Extensions.Tracing.TraceEvent source)
		{
			return source.NullCheckedConvert(value => new TraceEvent
			{
				TraceEventKey = value.TraceEventKey,
				TraceEventDateTime = value.TraceEventDateTime,
				TraceEventType = value.TraceEventType,
				Source = value.Source,
				MachineName = value.MachineName,
				Environment = value.Environment,
				Message = value.Message,
				StackTrace = value.StackTrace,
				Exception = value.Exception.NullCheckedConvert(TraceEventExceptionConversions.Convert),
				Data = value.Data,
				TraceEventKeyValues = value.TraceEventKeyValues.ToNullCheckedCollection<ISI.Extensions.Tracing.TraceEventKeyValue, TraceEventKeyValue, TraceEventKeyValueCollection>(TraceEventKeyValueConversions.Convert),
				HttpContextServerVariables = value.HttpContextServerVariables.ToNullCheckedCollection<ISI.Extensions.Tracing.TraceEventKeyValue, TraceEventKeyValue, TraceEventKeyValueCollection>(TraceEventKeyValueConversions.Convert),
				ServiceModelExecutionSummary = value.ServiceModelExecutionSummary.NullCheckedConvert(TraceEventServiceModelExecutionSummaryConversions.Convert),
				OperationKey = value.OperationKey,
				ActivityKey = value.ActivityKey
			});
		}

		public static ISI.Extensions.Tracing.TraceEvent Convert(TraceEvent source)
		{
			return source.NullCheckedConvert(value => new ISI.Extensions.Tracing.TraceEvent
			{
				TraceEventKey = value.TraceEventKey,
				TraceEventDateTime = value.TraceEventDateTime,
				TraceEventType = value.TraceEventType,
				Source = value.Source,
				MachineName = value.MachineName,
				Environment = value.Environment,
				Message = value.Message,
				StackTrace = value.StackTrace,
				Exception = value.Exception.NullCheckedConvert(TraceEventExceptionConversions.Convert),
				Data = value.Data,
				TraceEventKeyValues = value.TraceEventKeyValues.ToNullCheckedArray(TraceEventKeyValueConversions.Convert),
				HttpContextServerVariables = value.HttpContextServerVariables.ToNullCheckedArray(TraceEventKeyValueConversions.Convert),
				ServiceModelExecutionSummary = value.ServiceModelExecutionSummary.NullCheckedConvert(TraceEventServiceModelExecutionSummaryConversions.Convert),
				OperationKey = value.OperationKey,
				ActivityKey = value.ActivityKey
			});
		}
	}
}