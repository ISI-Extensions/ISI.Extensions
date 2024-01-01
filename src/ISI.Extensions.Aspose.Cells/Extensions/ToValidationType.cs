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
using ValidationType = Aspose.Cells.ValidationType;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.ValidationType ToValidationType(this ISI.Extensions.SpreadSheets.ValidationType validationType)
		{
			switch (validationType)
			{
				case ISI.Extensions.SpreadSheets.ValidationType.AnyValue:
					return global::Aspose.Cells.ValidationType.AnyValue;

				case ISI.Extensions.SpreadSheets.ValidationType.WholeNumber:
					return global::Aspose.Cells.ValidationType.WholeNumber;

				case ISI.Extensions.SpreadSheets.ValidationType.Decimal:
					return global::Aspose.Cells.ValidationType.Decimal;

				case ISI.Extensions.SpreadSheets.ValidationType.List:
					return global::Aspose.Cells.ValidationType.List;

				case ISI.Extensions.SpreadSheets.ValidationType.Date:
					return global::Aspose.Cells.ValidationType.Date;

				case ISI.Extensions.SpreadSheets.ValidationType.Time:
					return global::Aspose.Cells.ValidationType.Time;

				case ISI.Extensions.SpreadSheets.ValidationType.TextLength:
					return global::Aspose.Cells.ValidationType.TextLength;

				case ISI.Extensions.SpreadSheets.ValidationType.Custom:
					return global::Aspose.Cells.ValidationType.Custom;

				default:
					throw new ArgumentOutOfRangeException(nameof(validationType), validationType, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.ValidationType ToValidationType(this global::Aspose.Cells.ValidationType validationType)
		{
			switch (validationType)
			{
				case ValidationType.AnyValue:
					return ISI.Extensions.SpreadSheets.ValidationType.AnyValue;

				case ValidationType.WholeNumber:
					return ISI.Extensions.SpreadSheets.ValidationType.WholeNumber;

				case ValidationType.Decimal:
					return ISI.Extensions.SpreadSheets.ValidationType.Decimal;

				case ValidationType.List:
					return ISI.Extensions.SpreadSheets.ValidationType.List;

				case ValidationType.Date:
					return ISI.Extensions.SpreadSheets.ValidationType.Date;

				case ValidationType.Time:
					return ISI.Extensions.SpreadSheets.ValidationType.Time;

				case ValidationType.TextLength:
					return ISI.Extensions.SpreadSheets.ValidationType.TextLength;

				case ValidationType.Custom:
					return ISI.Extensions.SpreadSheets.ValidationType.Custom;

				default:
					throw new ArgumentOutOfRangeException(nameof(validationType), validationType, null);
			}
		}
	}
}
