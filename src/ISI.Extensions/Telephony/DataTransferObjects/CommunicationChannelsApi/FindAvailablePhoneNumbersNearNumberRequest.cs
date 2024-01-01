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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Telephony.DataTransferObjects.CommunicationChannelsApi
{
	public class FindAvailablePhoneNumbersNearNumberRequest : AbstractFindAvailablePhoneNumbersRequest
	{
		/// <summary>
		/// Given a phone number, find a geographically close number within Distance miles. Distance defaults to 25 miles.
		/// </summary>
		public string NearNumber { get; set; }

		/// <summary>
		/// Specifies the search radius for a Near- query in miles. If not specified this defaults to 25 miles.
		/// </summary>
		public int Distance { get; set; } = 25;

		/// <summary>
		/// Limit results to the same postal code as NearNumber.
		/// </summary>
		public bool? SamePostalCodeAsNearNumber { get; set; }

		/// <summary>
		/// Limit results to the same Locality as NearNumber.
		/// </summary>
		public bool? SameLocalityAsNearNumber { get; set; }

		/// <summary>
		/// Limit results to the same region as NearNumber.
		/// </summary>
		public bool? SameRegionAsNearNumber { get; set; }

		/// <summary>
		/// Limit results to a specific rate center, or given a phone number search within the same rate center as that number. Requires InLata to be set as well.
		/// </summary>
		public bool? SameRateCenterAsNearNumber { get; set; }

		/// <summary>
		/// Limit results to the same Local access and transport area (LATA) as NearNumber.
		/// </summary>
		public bool? SameLataAsNearNumber { get; set; }
	}
}
