using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.GoDaddy.SerializableEntities
{
	[DataContract]
	public class Error
	{
		public ISI.Extensions.GoDaddy.Error Export()
		{
			return new ISI.Extensions.GoDaddy.Error()
			{
				Code = Code,
				Fields = Fields.ToNullCheckedArray(field => field.Export()),
				Message = Message,
			};
		}

		[DataMember(Name = "code", EmitDefaultValue = false)]
		public string Code { get; set; }

		[DataMember(Name = "fields", EmitDefaultValue = false)]
		public ErrorField[] Fields { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }
	}

	[DataContract]
	public class ErrorField
	{
		public ISI.Extensions.GoDaddy.ErrorField Export()
		{
			return new ISI.Extensions.GoDaddy.ErrorField()
			{
				Code = Code,
				Message = Message,
				Path = Path,
				PathRelated = PathRelated,
			};
		}

		[DataMember(Name = "code", EmitDefaultValue = false)]
		public string Code { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		public string Message { get; set; }

		[DataMember(Name = "path", EmitDefaultValue = false)]
		public string Path { get; set; }

		[DataMember(Name = "pathRelated", EmitDefaultValue = false)]
		public string PathRelated { get; set; }
	}
}
