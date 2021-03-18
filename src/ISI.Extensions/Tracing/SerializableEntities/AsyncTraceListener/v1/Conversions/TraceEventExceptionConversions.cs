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

namespace ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.Conversions
{
	public static class TraceEventExceptionConversions
	{
		public static TraceEventException Convert(ISI.Extensions.Tracing.TraceEventException source)
		{
			return source.NullCheckedConvert(value => new ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventException
			{
				ExceptionType = value.ExceptionType,
				Message = value.Message,
				StackTrace = value.StackTrace,
				ExtraTrackingInformation = value.ExtraTrackingInformation.ToNullCheckedCollection<ISI.Extensions.Tracing.TraceEventExceptionExtraTrackingInformation, ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformation, ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformationCollection>(TraceEventExceptionExtraTrackingInformationConversions.Convert),
				InnerException = value.InnerException.NullCheckedConvert(Convert)
			});
		}

		public static ISI.Extensions.Tracing.TraceEventException Convert(ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventException source)
		{
			return source.NullCheckedConvert(value => new ISI.Extensions.Tracing.TraceEventException
			{
				ExceptionType = value.ExceptionType,
				Message = value.Message,
				StackTrace = value.StackTrace,
				ExtraTrackingInformation = value.ExtraTrackingInformation.ToNullCheckedCollection<ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformation, ISI.Extensions.Tracing.TraceEventExceptionExtraTrackingInformation, ISI.Extensions.Tracing.TraceEventExceptionExtraTrackingInformationCollection>(TraceEventExceptionExtraTrackingInformationConversions.Convert),
				InnerException = value.InnerException.NullCheckedConvert(Convert)
			});
		}

		public static ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventException Convert(Exception source)
		{
			return source.NullCheckedConvert(value => new ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventException
			{
				ExceptionType = source.GetType().FullName,
				Message = source.Message,
				Exception = source.ErrorMessageFormatted(includeInnerExceptions: false),
				StackTrace = source.StackTrace,
				ExtraTrackingInformation = (source as ISI.Extensions.Tracing.IExtraTrackingInformationException)?.GetExtraTrackingInformation().ToNullCheckedCollection<KeyValuePair<string, string>, ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformation, ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformationCollection>(item => new ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1.TraceEventExceptionExtraTrackingInformation()
				{
					Key = item.Key,
					Value = item.Value
				}),
				InnerException = Convert(source.InnerException)
			});
		}
	}
}
