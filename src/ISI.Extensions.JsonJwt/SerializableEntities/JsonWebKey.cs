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
using System.Text;
using System.Runtime.Serialization;

namespace ISI.Extensions.JsonJwt.SerializableEntities
{
	[DataContract]
	public class JsonWebKey
	{
		[DataMember(Name = "alg", EmitDefaultValue = false)]
		public string Alg { get; set; }

		[DataMember(Name = "crv", EmitDefaultValue = false)]
		public string Crv { get; set; }

		[DataMember(Name = "d", EmitDefaultValue = false)]
		public string D { get; set; }

		[DataMember(Name = "dp", EmitDefaultValue = false)]
		public string DP { get; set; }

		[DataMember(Name = "dq", EmitDefaultValue = false)]
		public string DQ { get; set; }

		[DataMember(Name = "e", EmitDefaultValue = false)]
		public string E { get; set; }

		[DataMember(Name = "k", EmitDefaultValue = false)]
		public string K { get; set; }

		[DataMember(Name = "key_ops", EmitDefaultValue = false)]
		public string[] KeyOps { get; set; }

		[DataMember(Name = "kid", EmitDefaultValue = false)]
		public string Kid { get; set; }

		[DataMember(Name = "kty", EmitDefaultValue = false)]
		public string Kty { get; set; }

		[DataMember(Name = "n", EmitDefaultValue = false)]
		public string N { get; set; }

		[DataMember(Name = "oth", EmitDefaultValue = false)]
		public string[] Oth { get; set; }

		[DataMember(Name = "p", EmitDefaultValue = false)]
		public string P { get; set; }

		[DataMember(Name = "q", EmitDefaultValue = false)]
		public string Q { get; set; }

		[DataMember(Name = "qi", EmitDefaultValue = false)]
		public string QI { get; set; }

		[DataMember(Name = "use", EmitDefaultValue = false)]
		public string Use { get; set; }

		[DataMember(Name = "x", EmitDefaultValue = false)]
		public string X { get; set; }

		[DataMember(Name = "x5c", EmitDefaultValue = false)]
		public string[] X5c { get; set; }

		[DataMember(Name = "x5t", EmitDefaultValue = false)]
		public string X5t { get; set; }

		[DataMember(Name = "x5t#S256", EmitDefaultValue = false)]
		public string X5tS256 { get; set; }

		[DataMember(Name = "x5u", EmitDefaultValue = false)]
		public string X5u { get; set; }

		[DataMember(Name = "y", EmitDefaultValue = false)]
		public string Y { get; set; }
	}
}
