using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Acme.SerializableModels.AcmeOrders
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("9e7c0673-221e-4246-b889-40319eaa785b")]
	public class OrderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook : IOrderCertificateDomainPostRenewalAction
	{
		[DataMember(Name = "headerAuthenticationKey", EmitDefaultValue = false)]
		public string HeaderAuthenticationKey { get; set; }

		[DataMember(Name = "headerAuthenticationValue", EmitDefaultValue = false)]
		public string HeaderAuthenticationValue { get; set; }

		[DataMember(Name = "setCertificatesUrl", EmitDefaultValue = false)]
		public string SetCertificatesUrl { get; set; }
	}
}