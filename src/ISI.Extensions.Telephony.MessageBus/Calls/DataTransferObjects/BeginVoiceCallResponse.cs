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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Telephony.MessageBus.Calls.DataTransferObjects
{
	[DataContract]
	public class BeginVoiceCallResponse
	{
		[DataMember(Name = "accountKey", EmitDefaultValue = false)]
		public string AccountKey { get; set; }

		[DataMember(Name = "sessionKey", EmitDefaultValue = false)]
		public string SessionKey { get; set; }

		[DataMember(Name = "from", EmitDefaultValue = false)]
		public string From { get; set; }

		[DataMember(Name = "to", EmitDefaultValue = false)]
		public string To { get; set; }

		[DataMember(Name = "callStatus", EmitDefaultValue = false)]
		public CallStatus? CallStatus { get; set; }

		[DataMember(Name = "direction", EmitDefaultValue = false)]
		public Direction? Direction { get; set; }

		[DataMember(Name = "price", EmitDefaultValue = false)]
		public Decimal? Price { get; set; }

		[DataMember(Name = "createdDateTimeUtc", EmitDefaultValue = false)]
		public DateTime? CreatedDateTimeUtc { get; set; }

		[DataMember(Name = "updatedDateTimeUtc", EmitDefaultValue = false)]
		public DateTime? UpdatedDateTimeUtc { get; set; }

		[DataMember(Name = "success", EmitDefaultValue = false)]
		public bool Success { get; set; }
	}
}
