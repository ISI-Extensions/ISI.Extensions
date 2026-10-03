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
using System.Runtime.Serialization;

namespace ISI.Extensions.TrueNAS.SerializableModels
{
	[DataContract]
	public class SystemInfoResponse
	{
		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "buildtime", EmitDefaultValue = false)]
		public object BuildTime { get; set; }

		[DataMember(Name = "hostname", EmitDefaultValue = false)]
		public string Hostname { get; set; }

		[DataMember(Name = "physmem", EmitDefaultValue = false)]
		public long? PhysicalMemory { get; set; }

		[DataMember(Name = "model", EmitDefaultValue = false)]
		public string Model { get; set; }

		[DataMember(Name = "cores", EmitDefaultValue = false)]
		public int? Cores { get; set; }

		[DataMember(Name = "physical_cores", EmitDefaultValue = false)]
		public int? PhysicalCores { get; set; }

		[DataMember(Name = "loadavg", EmitDefaultValue = false)]
		public object[] LoadAverage { get; set; }

		[DataMember(Name = "uptime", EmitDefaultValue = false)]
		public string Uptime { get; set; }

		[DataMember(Name = "uptime_seconds", EmitDefaultValue = false)]
		public long? UptimeSeconds { get; set; }

		[DataMember(Name = "system_serial", EmitDefaultValue = false)]
		public string SystemSerial { get; set; }

		[DataMember(Name = "system_product", EmitDefaultValue = false)]
		public string SystemProduct { get; set; }

		[DataMember(Name = "system_product_version", EmitDefaultValue = false)]
		public string SystemProductVersion { get; set; }

		[DataMember(Name = "license", EmitDefaultValue = false)]
		public object License { get; set; }

		[DataMember(Name = "boottime", EmitDefaultValue = false)]
		public object BootTime { get; set; }

		[DataMember(Name = "datetime", EmitDefaultValue = false)]
		public object Datetime { get; set; }

		[DataMember(Name = "timezone", EmitDefaultValue = false)]
		public string Timezone { get; set; }

		[DataMember(Name = "system_manufacturer", EmitDefaultValue = false)]
		public string SystemManufacturer { get; set; }

		[DataMember(Name = "ecc_memory", EmitDefaultValue = false)]
		public bool? EccMemory { get; set; }
	}
}