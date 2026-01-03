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
using System.Text;

namespace ISI.Extensions.Extensions
{
	public static class MonthExtensions
	{
		private static IDictionary<string, Month> MonthLookup = new Dictionary<string, Month>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ "January", Month.January },
			{ "Jan", Month.January },
			{ "February", Month.February },
			{ "Feb", Month.February },
			{ "March", Month.March },
			{ "Mar", Month.March },
			{ "April", Month.April },
			{ "Apr", Month.April },
			{ "May", Month.May },
			{ "June", Month.June },
			{ "Jun", Month.June },
			{ "July", Month.July },
			{ "Jul", Month.July },
			{ "August", Month.August },
			{ "Aug", Month.August },
			{ "September", Month.September },
			{ "Sept", Month.September },
			{ "Sep", Month.September },
			{ "October", Month.October },
			{ "Oct", Month.October },
			{ "November", Month.November },
			{ "Nov", Month.November },
			{ "December", Month.December },
			{ "Dec", Month.December },
		};

		public static Month? ToMonthNullable(this string value)
		{
			if (MonthLookup.TryGetValue(value, out var month))
			{
				return month;
			}

			return null;
		}
	}
}
