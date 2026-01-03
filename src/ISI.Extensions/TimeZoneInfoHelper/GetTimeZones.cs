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

namespace ISI.Extensions.Extensions
{
	public static partial class TimeZoneInfoHelper
	{
		#region TimeZones
		private static Dictionary<string, string> _TimeZones = null;
		public static Dictionary<string, string> TimeZones
		{
			get
			{
				if (_TimeZones == null)
				{
					_TimeZones = new();

					var timeZones = System.TimeZoneInfo.GetSystemTimeZones();

					foreach (var timeZone in timeZones)
					{
						_TimeZones.Add(timeZone.Id, timeZone.DisplayName);
					}
				}

				return _TimeZones;
			}
		}
		#endregion

		public static string GetTimeZone(string value)
		{
			string result = null;

			if (!string.IsNullOrEmpty(value))
			{
				if (value.Equals("NST", StringComparison.InvariantCultureIgnoreCase))
					value = "Newfoundland Standard Time";
				else if (value.Equals("NDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Newfoundland Standard Time";
				else if (value.Equals("AST", StringComparison.InvariantCultureIgnoreCase))
					value = "Atlantic Standard Time";
				else if (value.Equals("ADT", StringComparison.InvariantCultureIgnoreCase))
					value = "Atlantic Standard Time";
				else if (value.Equals("EST", StringComparison.InvariantCultureIgnoreCase))
					value = "Eastern Standard Time";
				else if (value.Equals("EDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Eastern Standard Time";
				else if (value.Equals("CST", StringComparison.InvariantCultureIgnoreCase))
					value = "Central Standard Time";
				else if (value.Equals("CDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Central Standard Time";
				else if (value.Equals("MST", StringComparison.InvariantCultureIgnoreCase))
					value = "Mountain Standard Time";
				else if (value.Equals("MDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Mountain Standard Time";
				else if (value.Equals("PST", StringComparison.InvariantCultureIgnoreCase))
					value = "Pacific Standard Time";
				else if (value.Equals("PDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Pacific Standard Time";
				else if (value.Equals("AKST", StringComparison.InvariantCultureIgnoreCase))
					value = "Alaska Standard Time";
				else if (value.Equals("AKDT", StringComparison.InvariantCultureIgnoreCase))
					value = "Alaska Standard Time";
				else if (value.Equals("HAST", StringComparison.InvariantCultureIgnoreCase))
					value = "Hawaii-Aleutian Standard Time";
				else if (value.Equals("HADT", StringComparison.InvariantCultureIgnoreCase))
					value = "Hawaii-Aleutian Standard Time";

				if (TimeZones.ContainsKey(value))
				{
					result = value;
				}
			}

			return result;
		}

		public static System.TimeZoneInfo GetTimeZoneInfo(string value)
		{
			return GetTimeZone(value).NullCheckedConvert(System.TimeZoneInfo.FindSystemTimeZoneById);
		}
	}
}
