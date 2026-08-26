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
	public enum NameCheapSslCertificateStatus
	{
		[ISI.Extensions.EnumGuid("5416b14c-bfe3-411a-ad6c-c818f083c50a", "Purchased", "purchased")] Purchased,
		[ISI.Extensions.EnumGuid("212bbec2-3ca8-4522-ab57-2a7807d3e155", "Replaced", "replaced")] Replaced,
		[ISI.Extensions.EnumGuid("a1a3a4f5-3243-4828-8495-6ead6f0346da", "Active", "active")] Active,
		[ISI.Extensions.EnumGuid("be47e8ba-1db5-4b51-8313-9e4672a884a2", "Purchase Error", "purchaseerror")] PurchaseError,
		[ISI.Extensions.EnumGuid("726d5b50-d639-4f03-ab5c-0aa4e616a2a9", "New Purchase", "newpurchase")] NewPurchase,
		[ISI.Extensions.EnumGuid("8637d1df-5132-4af6-91f7-b3b9d8884703", "New Renewal", "newrenewal")] NewRenewal,
		[ISI.Extensions.EnumGuid("6e17b441-39f5-4e8b-a22a-e45cbfacf1b7", "Cancelled", "cancelled")] Cancelled,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
		//[ISI.Extensions.EnumGuid("xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxx")] xxxxxxxxxxxxxxxxxxxxxxxxxx,
	}
}
