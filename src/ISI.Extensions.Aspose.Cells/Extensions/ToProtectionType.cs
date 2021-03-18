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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.ProtectionType ToProtectionType(this ISI.Extensions.SpreadSheets.ProtectionType protectionType)
		{
			switch (protectionType)
			{
				case ISI.Extensions.SpreadSheets.ProtectionType.All:
					return global::Aspose.Cells.ProtectionType.All;

				case ISI.Extensions.SpreadSheets.ProtectionType.Contents:
					return global::Aspose.Cells.ProtectionType.Contents;

				case ISI.Extensions.SpreadSheets.ProtectionType.Objects:
					return global::Aspose.Cells.ProtectionType.Objects;

				case ISI.Extensions.SpreadSheets.ProtectionType.Scenarios:
					return global::Aspose.Cells.ProtectionType.Scenarios;

				case ISI.Extensions.SpreadSheets.ProtectionType.Structure:
					return global::Aspose.Cells.ProtectionType.Structure;

				case ISI.Extensions.SpreadSheets.ProtectionType.Windows:
					return global::Aspose.Cells.ProtectionType.Windows;

				case ISI.Extensions.SpreadSheets.ProtectionType.None:
					return global::Aspose.Cells.ProtectionType.None;

				default:
					throw new ArgumentOutOfRangeException(nameof(protectionType), protectionType, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.ProtectionType ToProtectionType(this global::Aspose.Cells.ProtectionType protectionType)
		{
			switch (protectionType)
			{
				case global::Aspose.Cells.ProtectionType.All:
					return ISI.Extensions.SpreadSheets.ProtectionType.All;

				case global::Aspose.Cells.ProtectionType.Contents:
					return ISI.Extensions.SpreadSheets.ProtectionType.Contents;

				case global::Aspose.Cells.ProtectionType.Objects:
					return ISI.Extensions.SpreadSheets.ProtectionType.Objects;

				case global::Aspose.Cells.ProtectionType.Scenarios:
					return ISI.Extensions.SpreadSheets.ProtectionType.Scenarios;

				case global::Aspose.Cells.ProtectionType.Structure:
					return ISI.Extensions.SpreadSheets.ProtectionType.Structure;

				case global::Aspose.Cells.ProtectionType.Windows:
					return ISI.Extensions.SpreadSheets.ProtectionType.Windows;

				case global::Aspose.Cells.ProtectionType.None:
					return ISI.Extensions.SpreadSheets.ProtectionType.None;

				default:
					throw new ArgumentOutOfRangeException(nameof(protectionType), protectionType, null);
			}
		}
	}
}
