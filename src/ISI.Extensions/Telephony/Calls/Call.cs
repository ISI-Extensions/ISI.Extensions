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

namespace ISI.Extensions.Telephony.Calls
{
	public class Call
	{
		public string SessionKey { get; set; }
		public string OwnerCallKey { get; set; }
		public string AccountKey { get; set; }
		public string PhoneNumberKey { get; set; }
		public string GroupKey { get; set; }
		public Direction Direction { get; set; }
		public string ForwardedFrom { get; set; }
		public string To { get; set; }
		public string ToFormatted { get; set; }
		public string From { get; set; }
		public string FromFormatted { get; set; }
		public string CallerName { get; set; }
		public CallStatus CallStatus { get; set; }
		public DateTime? StartDateTimeUtc { get; set; }
		public DateTime? StopDateTimeUtc { get; set; }
		public int? Duration { get; set; }
		public double? Price { get; set; }
		public string PriceUnit { get; set; }
		public AnsweredBy? AnsweredBy { get; set; }
		public string Annotation { get; set; }
		public CallResourceUrls ResourceUrls { get; set; }
		public DateTime? CreateDateTimeUtc { get; set; }
		public DateTime? ModifyDateTimeUtc { get; set; }
	}
}
