using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Security.DataTransferObjects.SecurityApi;

namespace ISI.Extensions.Security.ActiveDirectory
{
	public partial class SecurityApi
	{
		internal class UserPropertyKey
		{
			public const string NameKey = "name";
			public const string EmailAddressKey = "mail";
			public const string FirstNameKey = "givenname";
			public const string LastNameKey = "sn";
			public const string UserNameKey = "sAMAccountName";
			public const string DistinguishedNameKey = "distinguishedName";
			public const string RolesKey = "memberof";
		}
	}
}