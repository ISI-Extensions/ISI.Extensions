using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using LOCALENTITIES = ISI.Extensions.Scm;

namespace ISI.Extensions.Git.SerializableModels
{
	[DataContract]
	public class WorkingCopyCommitInformation
	{
		[DataMember(Name = "commit", EmitDefaultValue = false)]
		public string CommitKey { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public string Author { get; set; }

		[DataMember(Name = "email", EmitDefaultValue = false)]
		public string AuthorEmail { get; set; }

		[DataMember(Name = "date", EmitDefaultValue = false)]
		public string CommitDateTimeUtc { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }
	}
}