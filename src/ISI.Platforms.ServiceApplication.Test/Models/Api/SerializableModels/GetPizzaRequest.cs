using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Platforms.ServiceApplication.Test.Models.Api.SerializableModels
{
	[DataContract]
	public class GetPizzaRequest
	{
		[DataMember(Name = "pizzaUuid", EmitDefaultValue = false)]
		public Guid PizzaUuid { get; set; }

	}
}