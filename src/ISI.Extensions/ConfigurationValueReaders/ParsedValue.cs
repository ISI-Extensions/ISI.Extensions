﻿#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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

namespace ISI.Extensions.ConfigurationValueReaders
{
	public class ParsedValue
	{
		public string Value { get; }
		public string Prefix { get; }
		public string Key { get; }
		public string KeyIndex { get; }
		public string DefaultValue { get; }

		public ParsedValue(string value)
		{
			Value = value;

			if (!string.IsNullOrWhiteSpace(Value))
			{
				var pieces = value.Split([':'], 2, StringSplitOptions.RemoveEmptyEntries);

				if (pieces.Length > 1)
				{
					Prefix = pieces[0].Trim();

					pieces = pieces[1].Split([';'], 2, StringSplitOptions.RemoveEmptyEntries);

					Key = pieces[0].Trim();
					DefaultValue = (pieces.Length > 1 ? pieces[1].Trim() : string.Empty);

					{
						var keyIndexOffset = Key.IndexOf("[");
						if ((keyIndexOffset > 0) && Key.EndsWith("]"))
						{
							KeyIndex = Key.Substring(keyIndexOffset).Trim('[', ']', ' ');
							Key = Key.Substring(0, keyIndexOffset).Trim('[', ']', ' ');
						}
					}
				}
			}
		}
	}
}
