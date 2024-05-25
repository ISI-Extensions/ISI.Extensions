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
using LOCALENTITIES = ISI.Extensions.VisualStudioCode;

namespace ISI.Extensions.VisualStudioCode.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("0b5f557c-259c-47bc-b99e-acb6c8d12ecd")]
	public class VisualStudioCodeSettingsFormLocationAndSizeV1 : IVisualStudioCodeSettingsFormLocationAndSize
	{
		public static IVisualStudioCodeSettingsFormLocationAndSize ToSerializable(LOCALENTITIES.VisualStudioCodeSettingsFormLocationAndSize source)
		{
			return new VisualStudioCodeSettingsFormLocationAndSizeV1()
			{
				FormName = source.FormName,
				Left = source.Left,
				Top = source.Top,
				Width = source.Width,
				Height = source.Height,
			};
		}

		public LOCALENTITIES.VisualStudioCodeSettingsFormLocationAndSize Export()
		{
			return new LOCALENTITIES.VisualStudioCodeSettingsFormLocationAndSize()
			{
				FormName = FormName,
				Left = Left,
				Top = Top,
				Width = Width,
				Height = Height,
			};
		}

		[DataMember(Name = "formName", EmitDefaultValue = false)]
		public string FormName { get; set; }

		[DataMember(Name = "left", EmitDefaultValue = false)]
		public int Left { get; set; }

		[DataMember(Name = "top", EmitDefaultValue = false)]
		public int Top { get; set; }

		[DataMember(Name = "width", EmitDefaultValue = false)]
		public int Width { get; set; }

		[DataMember(Name = "height", EmitDefaultValue = false)]
		public int Height { get; set; }
	}
}
