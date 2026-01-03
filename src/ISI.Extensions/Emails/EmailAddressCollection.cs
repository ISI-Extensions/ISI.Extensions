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
using ISI.Extensions.Emails.Extensions;

namespace ISI.Extensions.Emails
{
	public class EmailAddressCollection : ICollection<IEmailAddress>
	{
		private readonly Dictionary<string, IEmailAddress> _emailAddresses = new(StringComparer.InvariantCultureIgnoreCase);

		public EmailAddressCollection() { }

		public EmailAddressCollection(IEnumerable<IEmailAddress> emailAddresses)
		{
			AddRange(emailAddresses);
		}

		public EmailAddressCollection(IEnumerable<string> emailAddresses)
		{
			AddRange(emailAddresses.Select(EmailAddress.Create));
		}

		public void Add(IEmailAddress emailAddress)
		{
			if (emailAddress != null)
			{
				var key = emailAddress.Formatted();

				if (!_emailAddresses.ContainsKey(key))
				{
					_emailAddresses.Add(key, emailAddress);
				}
			}
		}

		public void AddRange(IEnumerable<IEmailAddress> emailAddresses)
		{
			if (emailAddresses != null)
			{
				foreach (var emailAddress in emailAddresses)
				{
					Add(emailAddress);
				}
			}
		}

		public void Clear() => _emailAddresses.Clear();

		public bool Contains(IEmailAddress emailAddress) => _emailAddresses.ContainsKey(emailAddress.Formatted());

		public void CopyTo(IEmailAddress[] array, int arrayIndex) => _emailAddresses.Values.CopyTo(array, arrayIndex);

		public bool Remove(IEmailAddress emailAddress) => _emailAddresses.Remove(emailAddress.Formatted());

		public IEnumerator<IEmailAddress> GetEnumerator() => _emailAddresses.Values.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _emailAddresses.Count;
		public bool IsReadOnly => false;

		public static implicit operator EmailAddressCollection(IEmailAddress[] values) => new(values);
		public static implicit operator EmailAddressCollection(string[] values) => new(values);
		public static implicit operator EmailAddressCollection(EmailAddress value) => new([value]);
		public static implicit operator EmailAddressCollection(string value) => new([EmailAddress.Create(value)]);
	}
}
