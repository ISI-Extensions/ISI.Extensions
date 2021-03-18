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
		public static global::Aspose.Cells.LookInType? ToLookIn(this ISI.Extensions.SpreadSheets.LookIn? pattern)
		{
			return (pattern.HasValue ? ToLookIn(pattern.GetValueOrDefault()) : (global::Aspose.Cells.LookInType?)null);
		}

		public static global::Aspose.Cells.LookInType ToLookIn(this ISI.Extensions.SpreadSheets.LookIn pattern)
		{
			switch (pattern)
			{
				case ISI.Extensions.SpreadSheets.LookIn.ValuesExcludeFormulaCell: return global::Aspose.Cells.LookInType.ValuesExcludeFormulaCell;
				case ISI.Extensions.SpreadSheets.LookIn.Formulas: return global::Aspose.Cells.LookInType.Formulas;
				case ISI.Extensions.SpreadSheets.LookIn.Comments: return global::Aspose.Cells.LookInType.Comments;
				case ISI.Extensions.SpreadSheets.LookIn.OnlyFormulas: return global::Aspose.Cells.LookInType.OnlyFormulas;
				case ISI.Extensions.SpreadSheets.LookIn.OriginalValues: return global::Aspose.Cells.LookInType.OriginalValues;
			}

			return global::Aspose.Cells.LookInType.Values;
		}

		public static ISI.Extensions.SpreadSheets.LookIn? ToLookIn(this global::Aspose.Cells.LookInType? pattern)
		{
			return (pattern.HasValue ? ToLookIn(pattern.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.LookIn?)null);
		}

		public static ISI.Extensions.SpreadSheets.LookIn ToLookIn(this global::Aspose.Cells.LookInType pattern)
		{
			switch (pattern)
			{
				case global::Aspose.Cells.LookInType.ValuesExcludeFormulaCell: return ISI.Extensions.SpreadSheets.LookIn.ValuesExcludeFormulaCell;
				case global::Aspose.Cells.LookInType.Formulas: return ISI.Extensions.SpreadSheets.LookIn.Formulas;
				case global::Aspose.Cells.LookInType.Comments: return ISI.Extensions.SpreadSheets.LookIn.Comments;
				case global::Aspose.Cells.LookInType.OnlyFormulas: return ISI.Extensions.SpreadSheets.LookIn.OnlyFormulas;
				case global::Aspose.Cells.LookInType.OriginalValues: return ISI.Extensions.SpreadSheets.LookIn.OriginalValues;
			}

			return ISI.Extensions.SpreadSheets.LookIn.Values;
		}
	}
}
