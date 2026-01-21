using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class SetRepositoryRoleErrorResponse
	{
		[DataMember(Name = "transactionId", EmitDefaultValue = false)]
		public string TransactionId { get; set; }

		[DataMember(Name = "errorCode", EmitDefaultValue = false)]
		public string ErrorCode { get; set; }

		[DataMember(Name = "context", EmitDefaultValue = false)]
		public SetRepositoryRoleErrorResponseContext[] Context { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }

		[DataMember(Name = "additionalMessages", EmitDefaultValue = false)]
		public SetRepositoryRoleErrorResponseAdditionalMessage[] AdditionalMessages { get; set; }

		[DataMember(Name = "violations", EmitDefaultValue = false)]
		public SetRepositoryRoleErrorResponseViolation[] Violations { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }
	}

	[DataContract]
	public class SetRepositoryRoleErrorResponseContext
	{
		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }
	}

	[DataContract]
	public class SetRepositoryRoleErrorResponseAdditionalMessage
	{
		[DataMember(Name = "key", EmitDefaultValue = false)]
		public string Key { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }
	}

	[DataContract]
	public class SetRepositoryRoleErrorResponseViolation
	{
		[DataMember(Name = "path", EmitDefaultValue = false)]
		public string Path { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }
	}
}