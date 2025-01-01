#region Copyright & License
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class Enum<TEnum>
	{
		public class EnumInformation
		{
			public int Index { get; internal set; }
			public TEnum Value { get; internal set; }
			public string Key { get; internal set; }
			public string DefaultDescription { get; internal set; }
			public string WithSpaces { get; internal set; }
			public string Abbreviation { get; internal set; }
			public string SerializationValue { get; internal set; }
			public bool Active { get; internal set; }
			public int Order { get; internal set; }
			public string[] Aliases { get; internal set; }
			public Guid? Uuid { get; internal set; }

			internal EnumInformation(int index, TEnum value, string defaultDescription, string withSpaces, string abbreviation, bool active, int order)
			{
				Index = index;
				Value = value;
				Key = value.ToString();
				DefaultDescription = defaultDescription;
				WithSpaces = withSpaces;
				Abbreviation = abbreviation;
				Active = active;
				Order = order;
			}

			internal EnumInformation(int index, TEnum value)
				: this(index, value, value.ToString(), value.ToString().RemoveTitleCase(), null, true, System.Convert.ToInt32(value))
			{
			}
		}
	}
}
