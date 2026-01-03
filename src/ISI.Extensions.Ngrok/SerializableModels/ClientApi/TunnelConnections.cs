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
using System.Runtime.Serialization;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Ngrok.SerializableModels.ClientApi
{
	[DataContract]
	public class TunnelConnections : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Ngrok.TunnelConnections>
	{
		public ISI.Extensions.Ngrok.TunnelConnections Export()
		{
			return new()
			{
				Count = Count,
				Gauge = Gauge,
				Rate1 = Rate1,
				Rate5 = Rate5,
				Rate15 = Rate15,
				P50 = P50,
				P90 = P90,
				P95 = P95,
				P99 = P99,
			};
		}

		public static TunnelConnections Convert(ISI.Extensions.Ngrok.TunnelConnections value)
		{
			return value.NullCheckedConvert(source => new TunnelConnections()
			{
				Count = source.Count,
				Gauge = source.Gauge,
				Rate1 = source.Rate1,
				Rate5 = source.Rate5,
				Rate15 = source.Rate15,
				P50 = source.P50,
				P90 = source.P90,
				P95 = source.P95,
				P99 = source.P99,
			});
		}

		[DataMember(Name = "count", EmitDefaultValue = false)]
		public int Count { get; set; }

		[DataMember(Name = "gauge", EmitDefaultValue = false)]
		public double Gauge { get; set; }

		[DataMember(Name = "rate1", EmitDefaultValue = false)]
		public double Rate1 { get; set; }

		[DataMember(Name = "rate5", EmitDefaultValue = false)]
		public double Rate5 { get; set; }

		[DataMember(Name = "rate15", EmitDefaultValue = false)]
		public double Rate15 { get; set; }

		[DataMember(Name = "p50", EmitDefaultValue = false)]
		public double P50 { get; set; }

		[DataMember(Name = "p90", EmitDefaultValue = false)]
		public double P90 { get; set; }

		[DataMember(Name = "p95", EmitDefaultValue = false)]
		public double P95 { get; set; }

		[DataMember(Name = "p99", EmitDefaultValue = false)]
		public double P99 { get; set; }
	}
}