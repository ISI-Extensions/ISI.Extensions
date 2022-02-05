using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Security
{
	public class User
	{
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
		public string DistinguishedName { get; set; }
		public string[] Roles { get; set; }
	}
}
