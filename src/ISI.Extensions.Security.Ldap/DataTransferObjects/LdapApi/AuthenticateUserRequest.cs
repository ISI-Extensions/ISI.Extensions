using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Security.Ldap.DataTransferObjects.LdapApi
{
	public class AuthenticateUserRequest : ILdapRequest
	{
		public string LdapHost { get; set; }
		public int? LdapPort { get; set; }
		public bool LdapStartTls { get; set; }
		public bool LdapSecureSocketLayer { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
	}
}