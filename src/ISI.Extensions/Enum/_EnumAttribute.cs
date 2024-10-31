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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class EnumGuidAttribute : EnumAttribute
	{
		internal EnumGuidAttribute()
			: base()
		{

		}

		public EnumGuidAttribute(string uuid)
			: base()
		{
			Uuid = uuid;
			Active = true;
		}
		public EnumGuidAttribute(string uuid, string description)
			: base(description)
		{
			Uuid = uuid;
		}
		public EnumGuidAttribute(string uuid, string description, bool active)
			: base(description, active)
		{
			Uuid = uuid;
		}
		public EnumGuidAttribute(string uuid, string description, bool active, int order)
			: base(description, active, order)
		{
			Uuid = uuid;
		}

		public EnumGuidAttribute(string uuid, string description, string abbreviation)
			: base(description, abbreviation)
		{
			Uuid = uuid;
		}
		public EnumGuidAttribute(string uuid, string description, string abbreviation, bool active)
			: base(description, abbreviation, active)
		{
			Uuid = uuid;
		}
		public EnumGuidAttribute(string uuid, string description, string abbreviation, bool active, int order) 
			: base(description, abbreviation, active, order)
		{
			Uuid = uuid;
		}
	}

	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class EnumAttribute : Attribute
	{
		public string Description { get; internal set; }
		public string Abbreviation { get; internal set; }
		public bool Active { get; internal set; }
		public int? Order { get; internal set; }
		public string[] Aliases { get; set; }
		public string Uuid { get; set; }

		internal EnumAttribute()
		{
			Active = true;
		}

		public EnumAttribute(string description)
		{
			Description = description;
			Active = true;
		}
		public EnumAttribute(string description, bool active)
		{
			Description = description;
			Active = active;
		}
		public EnumAttribute(string description, bool active, int order)
		{
			Description = description;
			Active = active;
			Order = order;
		}

		public EnumAttribute(string description, string abbreviation)
		{
			Description = description;
			Abbreviation = abbreviation;
			Active = true;
		}
		public EnumAttribute(string description, string abbreviation, bool active)
		{
			Description = description;
			Abbreviation = abbreviation;
			Active = active;
		}
		public EnumAttribute(string description, string abbreviation, bool active, int order)
		{
			Description = description;
			Abbreviation = abbreviation;
			Active = active;
			Order = order;
		}
	}
}