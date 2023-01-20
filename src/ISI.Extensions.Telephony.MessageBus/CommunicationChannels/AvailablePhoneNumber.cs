#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Telephony.MessageBus.CommunicationChannels
{
	[DataContract]
	public class AvailablePhoneNumber
	{
		[DataMember(Name = "friendlyName", EmitDefaultValue = false)]
		public string FriendlyName { get; set; }

		[DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
		public string PhoneNumber { get; set; }

		[DataMember(Name = "lata", EmitDefaultValue = false)]
		public string Lata { get; set; }

		[DataMember(Name = "rateCenter", EmitDefaultValue = false)]
		public string RateCenter { get; set; }

		private LatitudeLongitude _latitudeLongitude = null;
		[DataMember(Name = "latitudeLongitude", EmitDefaultValue = false)]
		public LatitudeLongitude LatitudeLongitude
		{
			get => _latitudeLongitude ??= new(); 
			set => _latitudeLongitude = value;
		}

		[DataMember(Name = "region", EmitDefaultValue = false)]
		public string Region { get; set; }

		[DataMember(Name = "postalCode", EmitDefaultValue = false)]
		public string PostalCode { get; set; }

		[DataMember(Name = "isoCountry", EmitDefaultValue = false)]
		public string IsoCountry { get; set; }

		[DataMember(Name = "smsEnabled", EmitDefaultValue = false)]
		public bool SmsEnabled { get; set; }

		[DataMember(Name = "mmsEnabled", EmitDefaultValue = false)]
		public bool MmsEnabled { get; set; }

		[DataMember(Name = "voiceEnabled", EmitDefaultValue = false)]
		public bool VoiceEnabled { get; set; }
	}
}
