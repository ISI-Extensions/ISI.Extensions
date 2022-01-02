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
using System.Runtime.Serialization;

namespace ISI.Extensions.Tracing.SerializableEntities.AsyncTraceListener.v1
{
	[DataContract(Name = "traceEvent", Namespace = "")]
	public class TraceEvent : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Tracing.TraceEvent>
	{
		ISI.Extensions.Tracing.TraceEvent ISI.Extensions.Converters.IExportTo<ISI.Extensions.Tracing.TraceEvent>.Export()
		{
			if (string.IsNullOrWhiteSpace(Environment))
			{
				Environment = "NotSpecified";
			}

			return Conversions.TraceEventConversions.Convert(this);
		}

		[DataMember(Name = "traceEventKey", EmitDefaultValue = false)]
		public string TraceEventKey { get; set; }

		[DataMember(Name = "traceEventDateTime", EmitDefaultValue = false)]
		public DateTime TraceEventDateTime { get; set; }

		[DataMember(Name = "traceEventType", EmitDefaultValue = false)]
		public string TraceEventType { get; set; }

		[DataMember(Name = "source", EmitDefaultValue = false)]
		public string Source { get; set; }

		[DataMember(Name = "machineName", EmitDefaultValue = false)]
		public string MachineName { get; set; }

		[DataMember(Name = "environment", EmitDefaultValue = false)]
		public string Environment { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }

		[DataMember(Name = "stackTrace", EmitDefaultValue = false)]
		public string StackTrace { get; set; }

		[DataMember(Name = "exception", EmitDefaultValue = false)]
		public TraceEventException Exception { get; set; }

		[DataMember(Name = "data", EmitDefaultValue = false)]
		public string Data { get; set; }

		[DataMember(Name = "traceEventKeyValues", EmitDefaultValue = false)]
		public TraceEventKeyValueCollection TraceEventKeyValues { get; set; }

		[DataMember(Name = "httpContextServerVariables", EmitDefaultValue = false)]
		public TraceEventKeyValueCollection HttpContextServerVariables { get; set; }

		[DataMember(Name = "serviceModelExecutionSummary", EmitDefaultValue = false)]
		public TraceEventServiceModelExecutionSummary ServiceModelExecutionSummary { get; set; }

		[DataMember(Name = "operationKey", EmitDefaultValue = false)]
		public string OperationKey { get; set; }

		[DataMember(Name = "activityKey", EmitDefaultValue = false)]
		public string ActivityKey { get; set; }
	}
}
