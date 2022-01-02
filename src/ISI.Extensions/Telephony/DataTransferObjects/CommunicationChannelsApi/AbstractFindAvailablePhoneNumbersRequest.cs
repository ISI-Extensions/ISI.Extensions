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

namespace ISI.Extensions.Telephony.DataTransferObjects.CommunicationChannelsApi
{
	public abstract class AbstractFindAvailablePhoneNumbersRequest
	{
		public string AccountKey { get; set; }

		public string IsoCountry { get; set; } = "US";
		public ISI.Extensions.Telephony.CommunicationChannels.PhoneNumberType PhoneNumberType { get; set; } = ISI.Extensions.Telephony.CommunicationChannels.PhoneNumberType.Local;

		public bool? SmsEnabled { get; set; }
		public bool? MmsEnabled { get; set; }
		public bool? VoiceEnabled { get; set; }
		public bool? FaxEnabled { get; set; }
		public bool? ExcludeAllAddressRequired { get; set; }
		public bool? ExcludeLocalAddressRequired { get; set; }
		public bool? ExcludeForeignAddressRequired { get; set; }

		public bool? Beta { get; set; }

		/// <summary>
		/// A pattern to match phone numbers on. Valid characters are '*' and [0-9a-zA-Z]. The '*' character will match any single digit.
		/// 
		/// </summary>
		public string Contains { get; set; }
	}
}
