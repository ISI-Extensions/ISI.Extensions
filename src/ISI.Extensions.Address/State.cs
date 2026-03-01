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

namespace ISI.Extensions
{
	public partial class Address
	{
		[System.Runtime.Serialization.DataContract(Namespace = "")]
		public enum State //IDs are their respective FIPS ID -- http://www.itl.nist.gov/fipspubs/fip5-2.htm
		{
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Not Known", "NotKnown", false)] NotKnown = 0,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Alabama", "AL")] Alabama = 1,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Alaska", "AK")] Alaska = 2,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Arizona", "AZ")] Arizona = 4,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Arkansas", "AR")] Arkansas = 5,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("California", "CA")] California = 6,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Colorado", "CO")] Colorado = 8,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Connecticut", "CT")] Connecticut = 9,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Delaware", "DE")] Delaware = 10,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("District of Columbia", "DC")] DistrictOfColumbia = 11,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Florida", "FL")] Florida = 12,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Georgia", "GA")] Georgia = 13,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Hawaii", "HI")] Hawaii = 15,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Idaho", "ID")] Idaho = 16,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Illinois", "IL")] Illinois = 17,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Indiana", "IN")] Indiana = 18,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Iowa", "IA")] Iowa = 19,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Kansas", "KS")] Kansas = 20,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Kentucky", "KY")] Kentucky = 21,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Louisiana", "LA")] Louisiana = 22,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Maine", "ME")] Maine = 23,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Maryland", "MD")] Maryland = 24,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Massachusetts", "MA")] Massachusetts = 25,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Michigan", "MI")] Michigan = 26,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Minnesota", "MN")] Minnesota = 27,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Mississippi", "MS")] Mississippi = 28,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Missouri", "MO")] Missouri = 29,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Montana", "MT")] Montana = 30,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Nebraska", "NE")] Nebraska = 31,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Nevada", "NV")] Nevada = 32,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("New Hampshire", "NH")] NewHampshire = 33,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("New Jersey", "NJ")] NewJersey = 34,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("New Mexico", "NM")] NewMexico = 35,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("New York", "NY")] NewYork = 36,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("North Carolina", "NC")] NorthCarolina = 37,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("North Dakota", "ND")] NorthDakota = 38,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Ohio", "OH")] Ohio = 39,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Oklahoma", "OK")] Oklahoma = 40,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Oregon", "OR")] Oregon = 41,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Pennsylvania", "PA")] Pennsylvania = 42,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Rhode Island", "RI")] RhodeIsland = 44,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("South Carolina", "SC")] SouthCarolina = 45,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("South Dakota", "SD")] SouthDakota = 46,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Tennessee", "TN")] Tennessee = 47,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Texas", "TX")] Texas = 48,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Utah", "UT")] Utah = 49,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Vermont", "VT")] Vermont = 50,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Virginia", "VA")] Virginia = 51,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Washington", "WA")] Washington = 53,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("West Virginia", "WV")] WestVirginia = 54,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Wisconsin", "WI")] Wisconsin = 55,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Wyoming", "WY")] Wyoming = 56,



			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("American Samoa", "AS")] AmericanSamoa = 3,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Federated States of Micronesia", "FM")] FederatedStatesOfMicronesia = 64,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Guam", "GU")] Guam = 66,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Marshall Islands", "MH")] MarshallIslands = 68,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Northern Mariana Islands", "MP")] NorthernMarianaIslands = 69,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Palau", "PW")] Palau = 70,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Puerto Rico", "PR")] PuertoRico = 72,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Armed Forces Americas", "AA")] ArmedForcesAmericas = 34034,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Armed Forces Europe", "AE")] ArmedForcesEurope = 09717,
			[System.Runtime.Serialization.EnumMember, ISI.Extensions.Enum("Armed Forces Pacific", "AP")] ArmedForcesPacific = 96204,
		}
	}
}