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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatorType = Aspose.Cells.OperatorType;

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.OperatorType ToOperatorType(this ISI.Extensions.SpreadSheets.OperatorType operatorType)
		{
			switch (operatorType)
			{
				case ISI.Extensions.SpreadSheets.OperatorType.None:
					return global::Aspose.Cells.OperatorType.None;

				case ISI.Extensions.SpreadSheets.OperatorType.Between:
					return global::Aspose.Cells.OperatorType.Between;

				case ISI.Extensions.SpreadSheets.OperatorType.NotBetween:
					return global::Aspose.Cells.OperatorType.NotBetween;

				case ISI.Extensions.SpreadSheets.OperatorType.Equal:
					return global::Aspose.Cells.OperatorType.Equal;

				case ISI.Extensions.SpreadSheets.OperatorType.NotEqual:
					return global::Aspose.Cells.OperatorType.NotEqual;

				case ISI.Extensions.SpreadSheets.OperatorType.GreaterThan:
					return global::Aspose.Cells.OperatorType.GreaterThan;

				case ISI.Extensions.SpreadSheets.OperatorType.GreaterOrEqual:
					return global::Aspose.Cells.OperatorType.GreaterOrEqual;

				case ISI.Extensions.SpreadSheets.OperatorType.LessThan:
					return global::Aspose.Cells.OperatorType.LessThan;

				case ISI.Extensions.SpreadSheets.OperatorType.LessOrEqual:
					return global::Aspose.Cells.OperatorType.LessOrEqual;

				default:
					throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
			}
		}

		public static ISI.Extensions.SpreadSheets.OperatorType ToOperatorType(this global::Aspose.Cells.OperatorType operatorType)
		{
			switch (operatorType)
			{
				case OperatorType.Between:
					return ISI.Extensions.SpreadSheets.OperatorType.Between;

				case OperatorType.Equal:
					return ISI.Extensions.SpreadSheets.OperatorType.Equal;

				case OperatorType.GreaterThan:
					return ISI.Extensions.SpreadSheets.OperatorType.GreaterOrEqual;

				case OperatorType.GreaterOrEqual:
					return ISI.Extensions.SpreadSheets.OperatorType.GreaterOrEqual;

				case OperatorType.LessThan:
					return ISI.Extensions.SpreadSheets.OperatorType.LessThan;

				case OperatorType.LessOrEqual:
					return ISI.Extensions.SpreadSheets.OperatorType.LessOrEqual;

				case OperatorType.None:
					return ISI.Extensions.SpreadSheets.OperatorType.None;

				case OperatorType.NotBetween:
					return ISI.Extensions.SpreadSheets.OperatorType.NotBetween;

				case OperatorType.NotEqual:
					return ISI.Extensions.SpreadSheets.OperatorType.NotEqual;

				default:
					throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
			}
		}
	}
}
