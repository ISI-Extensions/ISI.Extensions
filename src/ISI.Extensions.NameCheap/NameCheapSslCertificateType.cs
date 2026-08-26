#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.NameCheap
{
	public enum NameCheapSslCertificateType
	{
		[ISI.Extensions.EnumGuid("5ce85571-4f15-44eb-b9aa-113beb91fb6f", "PositiveSSL", "positivessl", active: false)] PositiveSSL,
		[ISI.Extensions.EnumGuid("672709e2-10ec-4b25-be64-9d7741186e11", "EssentialSSL", "essentialssl", active: false)] EssentialSSL,
		[ISI.Extensions.EnumGuid("9c8eb5fd-57fa-43ad-aa31-920e8b67fb0f", "InstantSSL", "instantssl", active: false)] InstantSSL,
		[ISI.Extensions.EnumGuid("2d1e5be4-cb64-49d7-aa1c-ef7217ab888f", "InstantSSL Pro", "instantssl pro", active: false)] InstantSSLPro,
		[ISI.Extensions.EnumGuid("325c4c40-5fbd-4057-8114-158cca295c6c", "PremiumSSL", "premiumssl", active: false)] PremiumSSL,
		[ISI.Extensions.EnumGuid("74986da1-9d1c-424e-ad50-ee945a61da0f", "EV SSL", "ev sll", active: false)] EV_SSL,
		[ISI.Extensions.EnumGuid("30a3f914-dd3c-468d-9ed8-26ea677683fe", "PositiveSSL Wildcard", "positivessl wildcard", active: false)] PositiveSSLWildcard,
		[ISI.Extensions.EnumGuid("100ce072-e474-4815-a32d-94cb9dbccd8f", "EssentialSSL Wildcard", "essentialssl wildcard", active: false)] EssentialSSLWildcard,
		[ISI.Extensions.EnumGuid("51aa2789-1362-4265-afba-45deacddb7f3", "PremiumSSL Wildcard", "premiumssl wildcard", active: false)] PremiumSSLWildcard,
		[ISI.Extensions.EnumGuid("7d4cbac4-0501-48ce-a58e-1c6f57fbdadc", "PositiveSSL Multi Domain", "positivessl multi domain", active: false)] PositiveSSLMultiDomain,
		[ISI.Extensions.EnumGuid("bd453535-48d2-4129-b173-2472357443ba", "Multi Domain SSL", "multi domain ssl", active: false)] MultiDomainSSL,
		[ISI.Extensions.EnumGuid("5fcce8ba-f640-469c-9220-44d68a1e415a", "Unified Communications", "unified communications", active: false)] UnifiedCommunications,
		[ISI.Extensions.EnumGuid("c58c6782-6946-4d2d-9660-bc36d1428919", "EV Multi Domain SSL", "ev multi domain ssl", active: false)] EV_MultiDomainSSL,

		[ISI.Extensions.EnumGuid("5baed553-16cd-46ce-aad0-44cc67dd01bc", "Standard SSL (SSL.com)", "standard ssl sslcom")] StandardSSL_SSLcom,
		[ISI.Extensions.EnumGuid("52ed89c4-c91e-4c36-a2f3-8fd33b6e9fa7", "Standard Wildcard SSL (SSL.com)", "standard wildcard ssl sslcom")] StandardWildcardSSL_SSLcom,
		[ISI.Extensions.EnumGuid("28037ffe-3abd-40b6-b5c5-16294cd8e17e", "SAN Certificate SSL (SSL.com)", "san certificate ssl sslcom")] SANCertificateSSL_SSLcom,
		[ISI.Extensions.EnumGuid("8dbabdf0-6199-452f-8e70-6ae97cf86ca2", "High Assurance SSL (SSL.com)", "high assurance ssl sslcom")] HighAssuranceSSL_SSLcom,
		[ISI.Extensions.EnumGuid("62681671-20be-4393-9aea-27f2cc7ecf9b", "OV Wildcard SSL (SSL.com)", "ov wildcard ssl sslcom")] OV_WildcardSSL_SSLcom,
		[ISI.Extensions.EnumGuid("4ec02b59-18f7-4f73-ae04-c237e7d30f34", "OV Multi-domain SSL (SSL.com)", "ov multi-domain ssl sslcom")] OV_MultiDomainSSL_SSLcom,
		[ISI.Extensions.EnumGuid("693e9386-a4cf-422c-b089-d211f09203eb", "EV SSL (SSL.com)", "ev ssl sslcom")] EV_SSL_SSLcom,
		[ISI.Extensions.EnumGuid("155c9556-3195-4a57-93ea-90ddb09acd17", "EV Multi Domain SSL (SSL.com)", "ev multi domain ssl sslcom")] EV_MultiDomainSSL_SSLcom,
	}
}
