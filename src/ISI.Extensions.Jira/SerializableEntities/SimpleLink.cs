#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using System.Runtime.Serialization;

namespace ISI.Extensions.Jira.SerializableEntities
{
	[DataContract]
	public partial class SimpleLink : ISI.Extensions.Converters.IExportTo<ISI.Extensions.Jira.SimpleLink>
	{
		public ISI.Extensions.Jira.SimpleLink Export()
		{
			return new ISI.Extensions.Jira.SimpleLink()
			{
				SimpleLinkId = SimpleLinkId,
				StyleClass = StyleClass,
				IconClass = IconClass,
				Label = Label,
				Title = Title,
				Href = Href,
				Weight = Weight,
			};
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string SimpleLinkId { get; set; }

		[DataMember(Name = "styleClass", EmitDefaultValue = false)]
		public string StyleClass { get; set; }

		[DataMember(Name = "iconClass", EmitDefaultValue = false)]
		public string IconClass { get; set; }

		[DataMember(Name = "label", EmitDefaultValue = false)]
		public string Label { get; set; }

		[DataMember(Name = "title", EmitDefaultValue = false)]
		public string Title { get; set; }

		[DataMember(Name = "href", EmitDefaultValue = false)]
		public string Href { get; set; }

		[DataMember(Name = "weight", EmitDefaultValue = false)]
		public int Weight { get; set; }
	}
}