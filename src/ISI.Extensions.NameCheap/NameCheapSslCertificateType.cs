using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.NameCheap
{
	public enum NameCheapSslCertificateType
	{
		[ISI.Extensions.EnumGuid("5ce85571-4f15-44eb-b9aa-113beb91fb6f", "PositiveSSL")] PositiveSSL,
		[ISI.Extensions.EnumGuid("672709e2-10ec-4b25-be64-9d7741186e11", "EssentialSSL")] EssentialSSL,
		[ISI.Extensions.EnumGuid("9c8eb5fd-57fa-43ad-aa31-920e8b67fb0f", "InstantSSL")] InstantSSL,
		[ISI.Extensions.EnumGuid("2d1e5be4-cb64-49d7-aa1c-ef7217ab888f", "InstantSSL Pro")] InstantSSLPro,
		[ISI.Extensions.EnumGuid("325c4c40-5fbd-4057-8114-158cca295c6c", "PremiumSSL")] PremiumSSL,
		[ISI.Extensions.EnumGuid("74986da1-9d1c-424e-ad50-ee945a61da0f", "EV SSL")] EVSSL,
		[ISI.Extensions.EnumGuid("30a3f914-dd3c-468d-9ed8-26ea677683fe", "PositiveSSL Wildcard")] PositiveSSLWildcard,
		[ISI.Extensions.EnumGuid("100ce072-e474-4815-a32d-94cb9dbccd8f", "EssentialSSL Wildcard")] EssentialSSLWildcard,
		[ISI.Extensions.EnumGuid("51aa2789-1362-4265-afba-45deacddb7f3", "PremiumSSL Wildcard")] PremiumSSLWildcard,
		[ISI.Extensions.EnumGuid("7d4cbac4-0501-48ce-a58e-1c6f57fbdadc", "PositiveSSL Multi Domain")] PositiveSSLMultiDomain,
		[ISI.Extensions.EnumGuid("bd453535-48d2-4129-b173-2472357443ba", "Multi Domain SSL")] MultiDomainSSL,
		[ISI.Extensions.EnumGuid("5fcce8ba-f640-469c-9220-44d68a1e415a", "Unified Communications")] UnifiedCommunications,
		[ISI.Extensions.EnumGuid("c58c6782-6946-4d2d-9660-bc36d1428919", "EV Multi Domain SSL")] EVMultiDomainSSL,
	}
}
