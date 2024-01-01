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
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Telephony.MessageBus.CommunicationChannels.DataTransferObjects
{
	[DataContract]
	public class GetAvailableNumbersRequest
	{
		[DataMember(Name = "isoCountry", EmitDefaultValue = false)]
		public string IsoCountry { get; set; }

		[DataMember(Name = "areaCode", EmitDefaultValue = false)]
		public string AreaCode { get; set; }

		[DataMember(Name = "contains", EmitDefaultValue = false)]
		public string Contains { get; set; }

		[DataMember(Name = "inRegion", EmitDefaultValue = false)]
		public string InRegion { get; set; }

		[DataMember(Name = "inPostalCode", EmitDefaultValue = false)]
		public string InPostalCode { get; set; }

		private LatitudeLongitude _nearLatitudeLongitude = null;
		[DataMember(Name = "nearLatitudeLongitude", EmitDefaultValue = false)]
		public LatitudeLongitude NearLatitudeLongitude
		{
			get => _nearLatitudeLongitude ??= new();
			set => _nearLatitudeLongitude = value;
		}

		[DataMember(Name = "distance", EmitDefaultValue = false)]
		public int? Distance { get; set; }

		[DataMember(Name = "nearNumber", EmitDefaultValue = false)]
		public string NearNumber { get; set; }

		[DataMember(Name = "inLata", EmitDefaultValue = false)]
		public string InLata { get; set; }

		[DataMember(Name = "inRateCenter", EmitDefaultValue = false)]
		public string InRateCenter { get; set; }

		[DataMember(Name = "smsEnabled", EmitDefaultValue = false)]
		public bool? SmsEnabled { get; set; }

		[DataMember(Name = "mmsEnabled", EmitDefaultValue = false)]
		public bool? MmsEnabled { get; set; }

		[DataMember(Name = "voiceEnabled", EmitDefaultValue = false)]
		public bool? VoiceEnabled { get; set; }
	}
}
