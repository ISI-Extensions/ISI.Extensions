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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class DateRangeAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
	{
		#region DateTimeStamper
		private static ISI.Extensions.DateTimeStamper.IDateTimeStamper _dateTimeStamper = null;
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper => _dateTimeStamper ??= (ISI.Extensions.ServiceLocator.Current?.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>() ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper());
		#endregion

		public bool AllowEmptyStrings { get; set; }
		public bool DateOnly { get; set; }

		#region MinValue
		private TimeSpan? _minTimeSpan = null;
		private DateTime? _minDateTime = null;
		public string MinValue
		{
			get => (_minTimeSpan.HasValue ? _minTimeSpan.Formatted(TimeSpanExtensions.TimeSpanFormat.Precise) : (_minDateTime.HasValue ? _minDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise) : string.Empty));
			set
			{
				_minTimeSpan = value.ToTimeSpanNullable();
				_minDateTime = value.ToDateTimeNullable();
			}
		}
		public DateTime? MinDateTime
		{
			get
			{
				DateTime? result = null;

				if (_minTimeSpan.HasValue)
				{
					result = DateTimeStamper.CurrentDateTime() + _minTimeSpan.GetValueOrDefault();
				}

				if (_minDateTime.HasValue)
				{
					result = DateTime.SpecifyKind(_minDateTime.GetValueOrDefault(), DateTimeKind.Utc);
				}

				if (DateOnly && result.HasValue)
				{
					var value = result.GetValueOrDefault();

					result = DateTime.SpecifyKind(new DateTime(value.Year, value.Month, value.Day), DateTimeKind.Utc);
				}

				return result;
			}
		}
		#endregion

		#region MaxValue
		private TimeSpan? _maxTimeSpan = null;
		private DateTime? _maxDateTime = null;
		public string MaxValue
		{
			get => (_maxTimeSpan.HasValue ? _maxTimeSpan.Formatted(TimeSpanExtensions.TimeSpanFormat.Precise) : (_maxDateTime.HasValue ? _maxDateTime.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise) : string.Empty));
			set
			{
				_maxTimeSpan = value.ToTimeSpanNullable();
				_maxDateTime = value.ToDateTimeNullable();
			}
		}
		public DateTime? MaxDateTime
		{
			get
			{
				DateTime? result = null;

				if (_maxTimeSpan.HasValue)
				{
					result = DateTimeStamper.CurrentDateTime() + _maxTimeSpan.GetValueOrDefault();
				}

				if (_maxDateTime.HasValue)
				{
					result = DateTime.SpecifyKind(_maxDateTime.GetValueOrDefault(), DateTimeKind.Utc);
				}

				if (DateOnly && result.HasValue)
				{
					var value = result.GetValueOrDefault();

					result = DateTime.SpecifyKind(new DateTime(value.Year, value.Month, value.Day), DateTimeKind.Utc).AddDays(1).AddSeconds(-1);
				}

				return result;
			}
		}
		#endregion

		public DateRangeAttribute()
			: base(() => "Field is not within range")
		{
		}

		public override bool IsValid(object model)
		{
			var isValid = true;

			var value = string.Format("{0}", model).ToDateTimeNullable();

			if (!AllowEmptyStrings && !value.HasValue)
			{
				isValid = false;
			}

			if (isValid)
			{
				var minDateTime = MinDateTime;
				if (value.HasValue && MinDateTime.HasValue)
				{
					if (value < minDateTime)
					{
						isValid = false;
					}
				}
			}

			if (isValid)
			{
				var maxDateTime = MaxDateTime;
				if (value.HasValue && MaxDateTime.HasValue)
				{
					if (value > maxDateTime)
					{
						isValid = false;
					}
				}
			}

			return isValid;
		}
	}
}