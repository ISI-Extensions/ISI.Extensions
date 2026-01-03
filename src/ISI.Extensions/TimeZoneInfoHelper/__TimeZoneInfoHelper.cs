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

namespace ISI.Extensions
{
	public interface IHasTimeZoneName
	{
		string TimeZoneName { get; }
	}

	public interface IHasTimeZoneInfo
	{
		System.TimeZoneInfo TimeZoneInfo { get; }
	}
}



namespace ISI.Extensions.Extensions
{
	public static partial class TimeZoneInfoHelper
	{
		[ThreadStatic]
		public static System.TimeZoneInfo TimeZoneInfo;

		public static System.TimeZoneInfo GetTimeZoneInfo()
		{
			return TimeZoneInfo ??= System.TimeZoneInfo.Local;
		}

		public static IDictionary<string, string> _olsonTimeZones = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
		{
			{"Africa/Bangui", "W. Central Africa Standard Time"},
			{"Africa/Cairo", "Egypt Standard Time"},
			{"Africa/Casablanca", "Morocco Standard Time"},
			{"Africa/Harare", "South Africa Standard Time"},
			{"Africa/Johannesburg", "South Africa Standard Time"},
			{"Africa/Lagos", "W. Central Africa Standard Time"},
			{"Africa/Monrovia", "Greenwich Standard Time"},
			{"Africa/Nairobi", "E. Africa Standard Time"},
			{"Africa/Windhoek", "Namibia Standard Time"},
			{"America/Anchorage", "Alaskan Standard Time"},
			{"America/Argentina/San_Juan", "Argentina Standard Time"},
			{"America/Asuncion", "Paraguay Standard Time"},
			{"America/Bahia", "Bahia Standard Time"},
			{"America/Bogota", "SA Pacific Standard Time"},
			{"America/Buenos_Aires", "Argentina Standard Time"},
			{"America/Caracas", "Venezuela Standard Time"},
			{"America/Cayenne", "SA Eastern Standard Time"},
			{"America/Chicago", "Central Standard Time"},
			{"America/Chihuahua", "Mountain Standard Time (Mexico)"},
			{"America/Cuiaba", "Central Brazilian Standard Time"},
			{"America/Denver", "Mountain Standard Time"},
			{"America/Fortaleza", "SA Eastern Standard Time"},
			{"America/Godthab", "Greenland Standard Time"},
			{"America/Guatemala", "Central America Standard Time"},
			{"America/Halifax", "Atlantic Standard Time"},
			{"America/Indianapolis", "US Eastern Standard Time"},
			{"America/La_Paz", "SA Western Standard Time"},
			{"America/Los_Angeles", "Pacific Standard Time"},
			{"America/Mexico_City", "Mexico Standard Time"},
			{"America/Montevideo", "Montevideo Standard Time"},
			{"America/New_York", "Eastern Standard Time"},
			{"America/Noronha", "UTC-02"},
			{"America/Phoenix", "US Mountain Standard Time"},
			{"America/Regina", "Canada Central Standard Time"},
			{"America/Santa_Isabel", "Pacific Standard Time (Mexico)"},
			{"America/Santiago", "Pacific SA Standard Time"},
			{"America/Sao_Paulo", "E. South America Standard Time"},
			{"America/St_Johns", "Newfoundland Standard Time"},
			{"America/Tijuana", "Pacific Standard Time"},
			{"Antarctica/McMurdo", "New Zealand Standard Time"},
			{"Atlantic/South_Georgia", "UTC-02"},
			{"Asia/Almaty", "Central Asia Standard Time"},
			{"Asia/Amman", "Jordan Standard Time"},
			{"Asia/Baghdad", "Arabic Standard Time"},
			{"Asia/Baku", "Azerbaijan Standard Time"},
			{"Asia/Bangkok", "SE Asia Standard Time"},
			{"Asia/Beirut", "Middle East Standard Time"},
			{"Asia/Calcutta", "India Standard Time"},
			{"Asia/Colombo", "Sri Lanka Standard Time"},
			{"Asia/Damascus", "Syria Standard Time"},
			{"Asia/Dhaka", "Bangladesh Standard Time"},
			{"Asia/Dubai", "Arabian Standard Time"},
			{"Asia/Irkutsk", "North Asia East Standard Time"},
			{"Asia/Jerusalem", "Israel Standard Time"},
			{"Asia/Kabul", "Afghanistan Standard Time"},
			{"Asia/Kamchatka", "Kamchatka Standard Time"},
			{"Asia/Karachi", "Pakistan Standard Time"},
			{"Asia/Katmandu", "Nepal Standard Time"},
			{"Asia/Kolkata", "India Standard Time"},
			{"Asia/Krasnoyarsk", "North Asia Standard Time"},
			{"Asia/Kuala_Lumpur", "Singapore Standard Time"},
			{"Asia/Kuwait", "Arab Standard Time"},
			{"Asia/Magadan", "Magadan Standard Time"},
			{"Asia/Muscat", "Arabian Standard Time"},
			{"Asia/Novosibirsk", "N. Central Asia Standard Time"},
			{"Asia/Oral", "West Asia Standard Time"},
			{"Asia/Rangoon", "Myanmar Standard Time"},
			{"Asia/Riyadh", "Arab Standard Time"},
			{"Asia/Seoul", "Korea Standard Time"},
			{"Asia/Shanghai", "China Standard Time"},
			{"Asia/Singapore", "Singapore Standard Time"},
			{"Asia/Taipei", "Taipei Standard Time"},
			{"Asia/Tashkent", "West Asia Standard Time"},
			{"Asia/Tbilisi", "Georgian Standard Time"},
			{"Asia/Tehran", "Iran Standard Time"},
			{"Asia/Tokyo", "Tokyo Standard Time"},
			{"Asia/Ulaanbaatar", "Ulaanbaatar Standard Time"},
			{"Asia/Vladivostok", "Vladivostok Standard Time"},
			{"Asia/Yakutsk", "Yakutsk Standard Time"},
			{"Asia/Yekaterinburg", "Ekaterinburg Standard Time"},
			{"Asia/Yerevan", "Armenian Standard Time"},
			{"Atlantic/Azores", "Azores Standard Time"},
			{"Atlantic/Cape_Verde", "Cape Verde Standard Time"},
			{"Atlantic/Reykjavik", "Greenwich Standard Time"},
			{"Australia/Adelaide", "Cen. Australia Standard Time"},
			{"Australia/Brisbane", "E. Australia Standard Time"},
			{"Australia/Darwin", "AUS Central Standard Time"},
			{"Australia/Hobart", "Tasmania Standard Time"},
			{"Australia/Perth", "W. Australia Standard Time"},
			{"Australia/Sydney", "AUS Eastern Standard Time"},
			{"Etc/GMT", "UTC"},
			{"Etc/GMT+11", "UTC-11"},
			{"Etc/GMT+12", "Dateline Standard Time"},
			{"Etc/GMT+2", "UTC-02"},
			{"Etc/GMT-12", "UTC+12"},
			{"Europe/Amsterdam", "W. Europe Standard Time"},
			{"Europe/Athens", "GTB Standard Time"},
			{"Europe/Belgrade", "Central Europe Standard Time"},
			{"Europe/Berlin", "W. Europe Standard Time"},
			{"Europe/Brussels", "Romance Standard Time"},
			{"Europe/Budapest", "Central Europe Standard Time"},
			{"Europe/Dublin", "GMT Standard Time"},
			{"Europe/Helsinki", "FLE Standard Time"},
			{"Europe/Istanbul", "GTB Standard Time"},
			{"Europe/Kiev", "FLE Standard Time"},
			{"Europe/London", "GMT Standard Time"},
			{"Europe/Minsk", "E. Europe Standard Time"},
			{"Europe/Moscow", "Russian Standard Time"},
			{"Europe/Paris", "Romance Standard Time"},
			{"Europe/Sarajevo", "Central European Standard Time"},
			{"Europe/Warsaw", "Central European Standard Time"},
			{"Indian/Mauritius", "Mauritius Standard Time"},
			{"Pacific/Apia", "Samoa Standard Time"},
			{"Pacific/Auckland", "New Zealand Standard Time"},
			{"Pacific/Fiji", "Fiji Standard Time"},
			{"Pacific/Guadalcanal", "Central Pacific Standard Time"},
			{"Pacific/Guam", "West Pacific Standard Time"},
			{"Pacific/Honolulu", "Hawaiian Standard Time"},
			{"Pacific/Pago_Pago", "UTC-11"},
			{"Pacific/Port_Moresby", "West Pacific Standard Time"},
			{"Pacific/Tongatapu", "Tonga Standard Time"}
		};

		private static IDictionary<string, string> _reverseOlsonTimeZones = null;

		static TimeZoneInfoHelper()
		{
			_reverseOlsonTimeZones = _olsonTimeZones.Select(olsonTimeZone => olsonTimeZone.Value).Distinct().ToDictionary(timeZone => timeZone, timeZone => _olsonTimeZones.First(olsonTimeZone => olsonTimeZone.Value == timeZone).Key, StringComparer.InvariantCultureIgnoreCase);

			//foreach (var olsonTimeZone in _reverseOlsonTimeZones.Where(olsonTimeZone => olsonTimeZone.Key.Contains("_")))
			//{
			//	_reverseOlsonTimeZones.Add(olsonTimeZone.Key.Replace("_", string.Empty), olsonTimeZone.Value);
			//}

			//foreach (var olsonTimeZone in _olsonTimeZones.Where(olsonTimeZone => olsonTimeZone.Key.Contains("_")))
			//{
			//	_olsonTimeZones.Add(olsonTimeZone.Key.Replace("_", string.Empty), olsonTimeZone.Value);
			//}
		}
	}
}
