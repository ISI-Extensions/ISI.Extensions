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
	public class SetSystemConfigGeneralRequest
	{
		[DataMember(Name = "ui_certificate", EmitDefaultValue = false)]
		public int? CertificateId { get; set; }

		[DataMember(Name = "ui_httpsport", EmitDefaultValue = false)]
		public int? UiHttpsPort { get; set; }

		[DataMember(Name = "ui_httpsredirect", EmitDefaultValue = false)]
		public bool? UiHttpsRedirect { get; set; }

		[DataMember(Name = "ui_httpsprotocols", EmitDefaultValue = false)]
		public string[] UiHttpsProtocols { get; set; }
	
		[DataMember(Name = "ui_port", EmitDefaultValue = false)]
		public int? UiPort { get; set; }

		[DataMember(Name = "ui_address", EmitDefaultValue = false)]
		public string[] UiAddress { get; set; }

		[DataMember(Name = "ui_v6address", EmitDefaultValue = false)]
		public string[] UiV6Address { get; set; }

		[DataMember(Name = "ui_allowlist", EmitDefaultValue = false)]
		public string[] UiAllowList { get; set; }

		[DataMember(Name = "ui_consolemsg", EmitDefaultValue = false)]
		public bool? UiConsoleMessage { get; set; }

		[DataMember(Name = "ui_x_frame_options", EmitDefaultValue = false)]
		public string UiXFrameOptions { get; set; }

		[DataMember(Name = "kbdmap", EmitDefaultValue = false)]
		public string Kbdmap { get; set; }

		[DataMember(Name = "timezone", EmitDefaultValue = false)]
		public string Timezone { get; set; }

		[DataMember(Name = "usage_collection", EmitDefaultValue = false)]
		public bool? UsageCollection { get; set; }

		[DataMember(Name = "ds_auth", EmitDefaultValue = false)]
		public bool? DsAuth { get; set; }

		[DataMember(Name = "ui_restart_delay", EmitDefaultValue = false)]
		public int? UiRestartDelayInSeconds { get; set; }

		[DataMember(Name = "rollback_timeout", EmitDefaultValue = false)]
		public int? RollBackTimeoutInSeconds { get; set; }
	}
}