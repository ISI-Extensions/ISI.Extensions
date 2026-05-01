using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using LOCALENTITIES = ISI.Extensions.Nginx;

namespace ISI.Extensions.Nginx.SerializableModels
{
	[DataContract]
	[ISI.Extensions.Serialization.PreferredSerializerJsonDataContract]
	[ISI.Extensions.Serialization.SerializerContractUuid("7ac2382d-f6e6-4512-b415-38642546d9e8")]
	public class FormLocationAndSizeV1 : IFormLocationAndSize
	{
		public static IFormLocationAndSize ToSerializable(LOCALENTITIES.FormLocationAndSize source)
		{
			return new FormLocationAndSizeV1()
			{
				FormName = source.FormName,
				Left = source.Left,
				Top = source.Top,
				Width = source.Width,
				Height = source.Height,
			};
		}

		public LOCALENTITIES.FormLocationAndSize Export()
		{
			return new LOCALENTITIES.FormLocationAndSize()
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