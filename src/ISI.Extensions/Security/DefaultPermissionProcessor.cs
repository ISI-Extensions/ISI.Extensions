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

namespace ISI.Extensions.Security
{
	public class DefaultPermissionProcessor : IPermissionProcessor
	{
		public virtual IEnumerable<string> GetUserRoles(string userKey)
		{
			return string.IsNullOrWhiteSpace(userKey) ? null : Array.Empty<string>();
		}

		public virtual IEnumerable<string> GetUserRoles(ISI.Extensions.UserKey userKey)
		{
			return string.IsNullOrWhiteSpace(userKey?.Value) ? null : Array.Empty<string>();
		}

		public virtual bool IsAuthorized(string userKey, IEnumerable<string> allowedRoles)
		{
			return IsAuthorized(GetUserRoles(userKey), allowedRoles);
		}

		public virtual bool IsAuthorized(ISI.Extensions.UserKey userKey, IEnumerable<string> allowedRoles)
		{
			return IsAuthorized(GetUserRoles(userKey), allowedRoles);
		}

		public virtual bool IsAuthorized(IEnumerable<string> userRoles, IEnumerable<string> allowedRoles)
		{
			allowedRoles = new HashSet<string>(allowedRoles ?? Array.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);

			if (((HashSet<string>)allowedRoles).Contains(ISI.Extensions.Security.Roles.AnonymousUsers))
			{
				return true;
			}

			if (((HashSet<string>)allowedRoles).Contains(ISI.Extensions.Security.Roles.AuthenticatedUsers) && (userRoles != null))
			{
				return true;
			}

			foreach (var userRole in userRoles.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				if (((HashSet<string>)allowedRoles).Contains(userRole))
				{
					return true;
				}
			}

			return false;
		}

		public virtual string[] GetDefaultPermissions()
		{
			return new[] { ISI.Extensions.Security.Roles.AnonymousUsers };
		}

		public virtual string[] GetDefaultAuthenticatedPermissions()
		{
			return new[] { ISI.Extensions.Security.Roles.AuthenticatedUsers };
		}
	}
}
