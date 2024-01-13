using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Acme.SerializableModels.Orders
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("9e7c0673-221e-4246-b889-40319eaa785b")]
	public class AcmeOrderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook : IAcmeOrderCertificateDomainPostRenewalAction
	{
		[DataMember(Name = "setCertificatesUrl", EmitDefaultValue = false)]
		public string SetCertificatesUrl { get; set; }
	}
}