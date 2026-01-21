using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class GetAccessTokenRequest
	{
		[DataMember(Name = "username")]
		public string Username { get; set; }

		[DataMember(Name = "password")]
		public string Password { get; set; }

		[DataMember(Name = "cookie")]
		public bool Cookie { get; set; }

		[DataMember(Name = "grant_type")]
		public string GrantType { get; set; } = "password";
	}
}