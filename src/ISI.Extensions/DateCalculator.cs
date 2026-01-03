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

namespace ISI.Extensions
{
	public class DateCalculator
	{
		public enum Interval
		{
			[ISI.Extensions.EnumGuid("ba776087-84a7-4854-9edd-c1d493473df7")] None = 0,
			[ISI.Extensions.EnumGuid("0cd46939-57ff-4fd3-9956-5f4461d80be8")] Daily = 10001,

			[ISI.Extensions.EnumGuid("cd8ca7d3-14ce-411d-ba02-38d0e66d1827", "Monday Thru Friday")] MonThruFri = 10002,
			[ISI.Extensions.EnumGuid("896b65a1-50a2-4f5c-a444-4250c2d0fa92")] Sunday = 10011,
			[ISI.Extensions.EnumGuid("2392c97e-15ae-45a4-8149-5d760b2e2c9f")] Monday = 10012,
			[ISI.Extensions.EnumGuid("cb5a3b4c-ace0-4d54-bcc2-aeb826b3f364")] Tuesday = 10013,
			[ISI.Extensions.EnumGuid("44e95959-cb2a-4002-92c9-eaffb7a9b7e1")] Wednesday = 10014,
			[ISI.Extensions.EnumGuid("e4f2adbf-9e7e-4f5f-ba37-a3b50330a654")] Thursday = 10015,
			[ISI.Extensions.EnumGuid("6a407350-3199-4027-ac39-9c3a5f16e3db")] Friday = 10016,
			[ISI.Extensions.EnumGuid("6a02b84c-4cfa-4325-b9ae-997b625b5999")] Saturday = 10017,

			[ISI.Extensions.EnumGuid("b7d126ef-d438-4f19-8e42-aaf56411884d", "First of Month")] Month1st = 10101,
			[ISI.Extensions.EnumGuid("9a63c571-acb0-4f67-934d-72ed22d34f07", "Fifth of Month")] Month5th = 10105,
			[ISI.Extensions.EnumGuid("5b0ac0c1-0fbd-4554-9f63-0afbfd8bd6b1", "Tenth of Month")] Month10th = 10110,
			[ISI.Extensions.EnumGuid("ec6fd173-a270-458f-b327-a5bd64dd3cc7", "Fifteenth of Month")] Month15th = 10115,
			[ISI.Extensions.EnumGuid("f292e610-d6bf-4946-abc3-ce89b6e0c763", "First of Quarter")] Quarter1st = 10201,
			[ISI.Extensions.EnumGuid("717f85ca-4079-48ee-81c5-5d0fa1c3c0d3", "Fifth of Quarter")] Quarter5th = 10205,
			[ISI.Extensions.EnumGuid("4abcffc4-26d1-4f61-a2cd-7379a370a5d6", "Tenth of Quarter")] Quarter10th = 10210,
			[ISI.Extensions.EnumGuid("1d4e1b86-1941-4b00-acc3-9ee11a434aac", "Fifteenth of Quarter")] Quarter15th = 10215,
			[ISI.Extensions.EnumGuid("ecafa848-9db5-4173-ad9c-6a8b1f79ba68", "Even Week Sunday")] EvenSunday = 20011,
			[ISI.Extensions.EnumGuid("fb945155-7ce3-430b-ba54-0c56c73abac9", "Even Week Monday")] EvenMonday = 20012,
			[ISI.Extensions.EnumGuid("f246fb06-2e3a-4412-b36a-8810dce927dc", "Even Week Tuesday")] EvenTuesday = 20013,
			[ISI.Extensions.EnumGuid("417f09b9-6861-484c-9fc8-5641a8ec6e38", "Even Week Wednesday")] EvenWednesday = 20014,
			[ISI.Extensions.EnumGuid("0f87688b-a8e4-4787-97e4-73f224c18929", "Even Week Thursday")] EvenThursday = 20015,
			[ISI.Extensions.EnumGuid("0da9792f-182a-4646-ae9b-5bb9cc58b600", "Even Week Friday")] EvenFriday = 20016,
			[ISI.Extensions.EnumGuid("6be67523-5aef-4103-a15b-1eb9f65184f5", "Even Week Saturday")] EvenSaturday = 20017,
			[ISI.Extensions.EnumGuid("44436fcd-ad52-4f04-a28f-80471c8708ac", "Odd Week Sunday")] OddSunday = 20021,
			[ISI.Extensions.EnumGuid("60a594cf-4061-4b72-b035-117d135c7ddf", "Odd Week Monday")] OddMonday = 20022,
			[ISI.Extensions.EnumGuid("b834c54a-3f95-45ad-9c85-b8e7b8e17850", "Odd Week Tuesday")] OddTuesday = 20023,
			[ISI.Extensions.EnumGuid("9cdd29af-2bb2-4d38-8872-94beb827a748", "Odd Week Wednesday")] OddWednesday = 20024,
			[ISI.Extensions.EnumGuid("18f3fffc-5472-4363-ad36-7dcc85387a67", "Odd Week Thursday")] OddThursday = 20025,
			[ISI.Extensions.EnumGuid("b4edc3b1-0f55-43ec-b8a8-0069bd673ac5", "Odd Week Friday")] OddFriday = 20026,
			[ISI.Extensions.EnumGuid("5003e936-6ba7-4abb-b3fd-4241f4a1542a", "Odd Week Saturday")] OddSaturday = 20027,
		}

		public static DateTime? NextIntervalDate(DateTime value, Interval interval)
		{
			var calendar = new System.Globalization.CultureInfo("en-US").Calendar;

			switch (interval)
			{
				case Interval.None:
					break;

				case Interval.Daily:
					return value.AddDays(1);

				case Interval.MonThruFri:
					value = value.AddDays(1);
					if (value.DayOfWeek == DayOfWeek.Saturday) value = value.AddDays(1);
					if (value.DayOfWeek == DayOfWeek.Sunday) value = value.AddDays(1);
					return value;

				case Interval.Sunday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Sunday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Monday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Monday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Tuesday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Tuesday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Wednesday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Wednesday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Thursday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Thursday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Friday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Friday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Saturday:
					value = value.AddDays(1);
					while (value.DayOfWeek != DayOfWeek.Saturday)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenSunday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Sunday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenMonday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Monday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenTuesday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Tuesday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenWednesday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Wednesday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenThursday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Thursday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenFriday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Friday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.EvenSaturday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 0) && (value.DayOfWeek != DayOfWeek.Saturday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddSunday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Sunday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddMonday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Monday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddTuesday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Tuesday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddWednesday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Wednesday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddThursday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Thursday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddFriday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Friday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.OddSaturday:
					value = value.AddDays(1);
					while ((calendar.GetWeekOfYear(value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday) % 2 != 1) && (value.DayOfWeek != DayOfWeek.Saturday))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Month1st:
					value = value.AddDays(1);
					while (value.Day != 1)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Month5th:
					value = value.AddDays(1);
					while (value.Day != 5)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Month10th:
					value = value.AddDays(1);
					while (value.Day != 10)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Month15th:
					value = value.AddDays(1);
					while (value.Day != 15)
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Quarter1st:
					value = value.AddDays(1);
					while ((value.Month % 4 != 0) && (value.Day != 1))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Quarter5th:
					value = value.AddDays(1);
					while ((value.Month % 4 != 0) && (value.Day != 5))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Quarter10th:
					value = value.AddDays(1);
					while ((value.Month % 4 != 0) && (value.Day != 10))
					{
						value = value.AddDays(1);
					}
					return value;

				case Interval.Quarter15th:
					value = value.AddDays(1);
					while ((value.Month % 4 != 0) && (value.Day != 15))
					{
						value = value.AddDays(1);
					}
					return value;

				default:
					return value.AddDays((int)interval);
			}

			return null;
		}
	}
}