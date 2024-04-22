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
using System.Runtime.Serialization;

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class ListRepositoriesResponse
	{
		[DataMember(Name = "page", EmitDefaultValue = false)]
		public int Page { get; set; }

		[DataMember(Name = "pageTotal", EmitDefaultValue = false)]
		public int PageTotal { get; set; }

		[DataMember(Name = "_links", EmitDefaultValue = false)]
		public ListRepositoriesResponseLinks Links { get; set; }

		[DataMember(Name = "_embedded", EmitDefaultValue = false)]
		public ListRepositoriesResponseEmbedded Embedded { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseLinks
	{
		[DataMember(Name = "self", EmitDefaultValue = false)]
		public Link Self { get; set; }

		[DataMember(Name = "first", EmitDefaultValue = false)]
		public Link First { get; set; }

		[DataMember(Name = "next", EmitDefaultValue = false)]
		public Link Next { get; set; }

		[DataMember(Name = "last", EmitDefaultValue = false)]
		public Link Last { get; set; }

		[DataMember(Name = "create", EmitDefaultValue = false)]
		public Link Create { get; set; }
	}

	[DataContract]
	public class ListRepositoriesResponseEmbedded
	{
		[DataMember(Name = "repositories", EmitDefaultValue = false)]
		public Repository[] Repositories { get; set; }
	}
}