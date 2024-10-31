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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class Enum_Tests
	{
		public enum MyEnum
		{
			[ISI.Extensions.EnumGuid("8cc7bc04-c312-4aa7-842f-ef1fe374cdee")] Yes,
			[ISI.Extensions.EnumGuid("acaa9be7-b2d1-4ff5-b654-0102cfa74891")] No,
		}

		[Test]
		public void Enum_Test()
		{
			var a1 = ISI.Extensions.Enum<MyEnum>.ToArray();


			var t1 = MyEnum.No;
			var t2 = t1.GetUuid();


			var t4 = (System.Enum)t1;

			var t5 = t4.GetUuid();

			var xxx = ISI.Extensions.Enum<MyEnum>.ParseUuid((Guid?)null);
			var xxxy = ISI.Extensions.Enum<MyEnum?>.ParseUuid((Guid?)null);

			ISI.Extensions.Enum.TryParseUuid(typeof(MyEnum?), (Guid?)null, out var parsedValueNull);
			ISI.Extensions.Enum.TryParseUuid(typeof(MyEnum?), "acaa9be7-b2d1-4ff5-b654-0102cfa74891", out var parsedValueS);
			ISI.Extensions.Enum.TryParseUuid(typeof(MyEnum?), Guid.Parse("acaa9be7-b2d1-4ff5-b654-0102cfa74891"), out var parsedValue);

			var x1 = ISI.Extensions.Enum.ParseUuid(typeof(MyEnum?), (Guid?)null);
			var x2 = ISI.Extensions.Enum.ParseUuid(typeof(MyEnum?), "acaa9be7-b2d1-4ff5-b654-0102cfa74891");
			var x3 = ISI.Extensions.Enum.ParseUuid(typeof(MyEnum?), Guid.Parse("acaa9be7-b2d1-4ff5-b654-0102cfa74891"));
		}
	}
}
