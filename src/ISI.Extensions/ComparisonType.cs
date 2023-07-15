#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public enum ComparisonType
	{
		[ISI.Extensions.EnumGuid("be88ae1b-386c-4254-a274-9645b3ec6491","Equal", "eq")] Equal = 0,
		[ISI.Extensions.EnumGuid("b415c0f6-da36-4bc6-9770-dbf86e3ace7a","Not Equal", "neq")] NotEqual = 1,
		[ISI.Extensions.EnumGuid("2bddac92-e495-411f-bc39-eade62f35977","Less Than", "lt")] LessThan = 2,
		[ISI.Extensions.EnumGuid("7d871e5a-7e20-41ad-939d-87214e7ea00a","Less Than Or Equal To", "lteq")] LessThanOrEqualTo = 3,
		[ISI.Extensions.EnumGuid("2b84b998-81ae-4014-8e0a-e3d8dd3b1737","Greater Than", "gt")] GreaterThan = 4,
		[ISI.Extensions.EnumGuid("16f00ed4-3e47-45b6-a7a1-c4b685cd6edf","Greater Than Or Equal To", "gteq")] GreaterThanOrEqualTo = 5
	}
}
