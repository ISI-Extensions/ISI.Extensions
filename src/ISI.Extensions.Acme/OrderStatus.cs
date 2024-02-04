#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Acme
{
	public enum OrderStatus
	{
		[ISI.Extensions.EnumGuid("31664f17-6a6d-4a10-90ee-866dc414e2a8", "Pending", "pending")] Pending,
		[ISI.Extensions.EnumGuid("87f620f5-2df8-4c17-9a74-2f248b7fdd90", "Ready", "ready")] Ready,
		[ISI.Extensions.EnumGuid("3e85a92f-0fb2-445e-94f2-019a9bf87808", "Processing", "processing")] Processing,
		[ISI.Extensions.EnumGuid("e1ecff96-95f9-4e06-bae3-45b4ea1d43da", "Valid", "valid")] Valid,
		[ISI.Extensions.EnumGuid("3585ba4a-de32-4a0e-ae7d-88afdd2e7c05", "Invalid", "invalid")] Invalid,
	}
}