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

namespace ISI.Extensions.Journal
{
	public class JournalEntryAssociationCollection : HashSet<IJournalEntryAssociation>, IJournalEntryAssociationCollection
	{
		public class EqualityComparer : IEqualityComparer<IJournalEntryAssociation>
		{
			private static EqualityComparer _instance = null;
			public static EqualityComparer Instance => _instance ??= new EqualityComparer();

			public bool Equals(IJournalEntryAssociation x, IJournalEntryAssociation y)
			{
				return ((x.AssociationTypeUuid == y.AssociationTypeUuid) && string.Equals(x.AssociationKey, y.AssociationKey, StringComparison.InvariantCultureIgnoreCase));
			}

			public int GetHashCode(IJournalEntryAssociation journalEntryAssociation)
			{
				return string.Format("{0}|{1}", journalEntryAssociation.AssociationTypeUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens), journalEntryAssociation.AssociationKey.ToLower()).GetHashCode();
			}
		}

		public JournalEntryAssociationCollection()
			: base(EqualityComparer.Instance)
		{

		}

		public JournalEntryAssociationCollection(IEnumerable<IJournalEntryAssociation> associations)
			: base(associations, EqualityComparer.Instance)
		{

		}

		void IJournalEntryAssociationCollection.Add(IJournalEntryAssociation association)
		{
			Add(association);
		}

		public void Add(Guid associationTypeUuid, string associationKey)
		{
			Add(new JournalEntryAssociation()
			{
				AssociationTypeUuid = associationTypeUuid,
				AssociationKey = associationKey
			});
		}

		public void Add(Guid associationTypeUuid, Guid associationUuid)
		{
			Add(new JournalEntryAssociation()
			{
				AssociationTypeUuid = associationTypeUuid,
				AssociationKey = associationUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)
			});
		}

		public void Add(Guid associationTypeUuid, int associationId)
		{
			Add(new JournalEntryAssociation()
			{
				AssociationTypeUuid = associationTypeUuid,
				AssociationKey = string.Format("{0}", associationId)
			});
		}

		public void Remove(Guid associationTypeUuid)
		{
			RemoveWhere(association => association.AssociationTypeUuid == associationTypeUuid);
		}

		public string this[Guid associationTypeUuid]
		{
			get => this.FirstOrDefault(association => association.AssociationTypeUuid == associationTypeUuid)?.AssociationKey;
			set
			{
				Remove(associationTypeUuid);
				Add(associationTypeUuid, value);
			}
		}
	}
}