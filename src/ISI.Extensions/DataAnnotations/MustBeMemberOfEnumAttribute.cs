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

namespace ISI.Extensions.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class MustBeMemberOfEnumAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
	{
		public bool AllowEmptyStrings { get; set; }
		public Type EnumType { get; }

		private bool _matchOnAnyValue = true;
		public bool MatchOnAnyValue
		{
			get => _matchOnAnyValue;
			set
			{
				_matchOnAnyValue = value;
				if (_matchOnAnyValue)
				{
					_matchOnIndex = false;
					_matchOnKey = false;
					_matchOnAbbreviation = false;
					_matchOnDescription = false;
				}
			}
		}

		private bool _matchOnIndex = false;
		public bool MatchOnIndex
		{
			get => _matchOnIndex;
			set
			{
				_matchOnIndex = value;
				if (_matchOnIndex)
				{
					_matchOnAnyValue = false;
				}
			}
		}

		private bool _matchOnKey = false;
		public bool MatchOnKey
		{
			get => _matchOnKey;
			set
			{
				_matchOnKey = value;
				if (_matchOnKey)
				{
					_matchOnAnyValue = false;
				}
			}
		}

		private bool _matchOnAbbreviation = false;
		public bool MatchOnAbbreviation
		{
			get => _matchOnAbbreviation;
			set
			{
				_matchOnAbbreviation = value;
				if (_matchOnAbbreviation)
				{
					_matchOnAnyValue = false;
				}
			}
		}
		
		private bool _matchOnDescription = false;
		public bool MatchOnDescription
		{
			get => _matchOnDescription;
			set
			{
				_matchOnDescription = value;
				if (_matchOnDescription)
				{
					_matchOnAnyValue = false;
				}
			}
		}

		public MustBeMemberOfEnumAttribute(Type enumType)
			: base(() => "Value is not valid")
		{
			EnumType = enumType;
		}
		
		public override bool IsValid(object model)
		{
			var value = string.Format("{0}", model);

			if (AllowEmptyStrings && string.IsNullOrWhiteSpace(value))
			{
				return true;
			}

			if (_matchOnAnyValue)
			{
				if (ISI.Extensions.Enum.TryParse(EnumType, value, out var parsedValue))
				{
					return true;
				}
			}

			if (_matchOnIndex)
			{
				if (ISI.Extensions.Enum.TryParseIndex(EnumType, value, out var parsedValue))
				{
					return true;
				}
			}

			if (_matchOnKey)
			{
				if (ISI.Extensions.Enum.TryParseKey(EnumType, value, out var parsedValue))
				{
					return true;
				}
			}

			if (_matchOnAbbreviation)
			{
				if (ISI.Extensions.Enum.TryParseAbbreviation(EnumType, value, out var parsedValue))
				{
					return true;
				}
			}

			if (_matchOnDescription)
			{
				if (ISI.Extensions.Enum.TryParseDescription(EnumType, value, out var parsedValue))
				{
					return true;
				}
			}

			return false;
		}
	}
}