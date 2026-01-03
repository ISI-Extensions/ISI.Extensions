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
using ISI.Extensions.Repository;

namespace ISI.Extensions.Repository
{
	public class EnumInformationRecordCollection : List<EnumInformationRecord>
	{
		private static readonly System.Text.RegularExpressions.Regex PascalizeRegex = new(@"(?:[^a-zA-Z0-9]*)(?<first>[a-zA-Z0-9])(?<reminder>[a-zA-Z0-9]*)(?:[^a-zA-Z0-9]*)");

		public string AliasesDelimiter { get; }

		public EnumInformationRecordCollection(string aliasesDelimiter = "|")
		{
			AliasesDelimiter = aliasesDelimiter;
		}

		public EnumInformationRecordCollection(IEnumerable<EnumInformationRecord> enumInformationRecords, string aliasesDelimiter = "|")
			: base(enumInformationRecords)
		{
			AliasesDelimiter = aliasesDelimiter;
		}

		public void Add(int enumId, Guid? enumUuid, string key, string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				key = PascalizeRegex.Replace(description, m => m.Groups["first"].ToString().ToUpper() + m.Groups["reminder"].ToString().ToLower());
			}

			Add(new EnumInformationRecord()
			{
				EnumId = enumId,
				EnumUuid = enumUuid,
				Key = key,
				Description = description,
				Abbreviation = abbreviation,
				Active = active,
				Order = order,
				Aliases = (aliases.NullCheckedAny() ? string.Join(AliasesDelimiter, aliases) : null),
			});
		}

		public void Add(int enumId, string key, string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			Add(enumId, (Guid?)null, key, description, abbreviation, active, order, aliases);
		}

		public void Add(string key, string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			var enumId = (this.Any() ? this.Max(@enum => @enum.EnumId) + 1 : 0);

			Add(enumId, key, description, abbreviation, active, order, aliases);
		}

		public void Add(string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			Add(null, description, abbreviation, active, order, aliases);
		}

		public void Add(Guid enumUuid, string key, string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			var enumId = (this.Any() ? this.Max(@enum => @enum.EnumId) + 1 : 0);

			Add(enumId, enumUuid, key, description, abbreviation, active, order, aliases);
		}

		public void Add(Guid enumUuid, string description, string abbreviation = null, bool active = true, int? order = null, IEnumerable<string> aliases = null)
		{
			Add(enumUuid, null, description, abbreviation, active, order, aliases);
		}
	}
}
