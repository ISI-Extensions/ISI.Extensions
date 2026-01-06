using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.SerializableModels
{
	[DataContract]
	public class AutoCompleteResponse
	{
		[DataMember(Name = "value", EmitDefaultValue = false)]
		public string Value { get; set; }

		[DataMember(Name = "label", EmitDefaultValue = false)]
		public string Label { get; set; }

		[DataMember(Name = "data", EmitDefaultValue = false)]
		public AutoCompleteResponseData Data { get; set; }

		[DataMember(Name = "nativeName", EmitDefaultValue = false)]
		public string NativeName { get; set; }
	}

	[DataContract]
	public class AutoCompleteResponseData
	{
		[DataMember(Name = "badgeStyle", EmitDefaultValue = false)]
		public string BadgeStyle { get; set; }

		[DataMember(Name = "badgeClass", EmitDefaultValue = false)]
		public string BadgeClass { get; set; }
	}
}