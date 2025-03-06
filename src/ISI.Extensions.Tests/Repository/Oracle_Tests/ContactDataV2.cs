#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Runtime.Serialization;
using NUnit.Framework;

namespace ISI.Extensions.Tests.Repository
{
	[TestFixture]
	public partial class Oracle_Tests
	{
		[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
		[ISI.Extensions.Serialization.SerializerContractUuid("5085c134-cf53-4f54-9091-9ccaef5480b7")]
		[DataContract]
		public class ContactDataV2 : IContactData
		{
			[DataMember(Name = "contactId", EmitDefaultValue = false)]
			public int ContactId { get; set; }

			[DataMember(Name = "firstName", EmitDefaultValue = false)]
			public string FirstName { get; set; }

			[DataMember(Name = "lastName", EmitDefaultValue = false)]
			public string LastName { get; set; }

			[DataMember(Name = "title", EmitDefaultValue = false)]
			public string Title { get; set; }

			[DataMember(Name = "addressLine1", EmitDefaultValue = false)]
			public string AddressLine1 { get; set; }

			[DataMember(Name = "addressLine2", EmitDefaultValue = false)]
			public string AddressLine2 { get; set; }

			[DataMember(Name = "city", EmitDefaultValue = false)]
			public string City { get; set; }

			[DataMember(Name = "state", EmitDefaultValue = false)]
			public string State { get; set; }

			[DataMember(Name = "zip", EmitDefaultValue = false)]
			public string Zip { get; set; }

			[DataMember(Name = "phoneNumber", EmitDefaultValue = false)]
			public string PhoneNumber { get; set; }
		}
	}
}