using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Headscale.SerializableModels.HeadscaleApi
{
	[DataContract]
	public class RenameUserResponse
	{
		[DataMember(Name = "user", EmitDefaultValue = false)]
		public User User { get; set; }
	}
}