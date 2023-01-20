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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.CellBorderType? ToCellBorder(this ISI.Extensions.SpreadSheets.CellBorder? border)
		{
			return (border.HasValue ? ToCellBorder(border.GetValueOrDefault()) : (global::Aspose.Cells.CellBorderType?) null);
		}

		public static global::Aspose.Cells.CellBorderType ToCellBorder(this ISI.Extensions.SpreadSheets.CellBorder border)
		{
			switch (border)
			{
				case ISI.Extensions.SpreadSheets.CellBorder.DashDot: return global::Aspose.Cells.CellBorderType.DashDot;
				case ISI.Extensions.SpreadSheets.CellBorder.DashDotDot: return global::Aspose.Cells.CellBorderType.DashDotDot;
				case ISI.Extensions.SpreadSheets.CellBorder.Dashed: return global::Aspose.Cells.CellBorderType.Dashed;
				case ISI.Extensions.SpreadSheets.CellBorder.Dotted: return global::Aspose.Cells.CellBorderType.Dotted;
				case ISI.Extensions.SpreadSheets.CellBorder.Double: return global::Aspose.Cells.CellBorderType.Double;
				case ISI.Extensions.SpreadSheets.CellBorder.Hair: return global::Aspose.Cells.CellBorderType.Hair;
				case ISI.Extensions.SpreadSheets.CellBorder.MediumDashDot: return global::Aspose.Cells.CellBorderType.MediumDashDot;
				case ISI.Extensions.SpreadSheets.CellBorder.MediumDashDotDot: return global::Aspose.Cells.CellBorderType.MediumDashDotDot;
				case ISI.Extensions.SpreadSheets.CellBorder.MediumDashed: return global::Aspose.Cells.CellBorderType.MediumDashed;
				case ISI.Extensions.SpreadSheets.CellBorder.Medium: return global::Aspose.Cells.CellBorderType.Medium;
				case ISI.Extensions.SpreadSheets.CellBorder.SlantedDashDot: return global::Aspose.Cells.CellBorderType.SlantedDashDot;
				case ISI.Extensions.SpreadSheets.CellBorder.Thick: return global::Aspose.Cells.CellBorderType.Thick;
				case ISI.Extensions.SpreadSheets.CellBorder.Thin: return global::Aspose.Cells.CellBorderType.Thin;
			}

			return global::Aspose.Cells.CellBorderType.None;
		}

		public static ISI.Extensions.SpreadSheets.CellBorder? ToCellBorder(this global::Aspose.Cells.CellBorderType? border)
		{
			return (border.HasValue ? ToCellBorder(border.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.CellBorder?)null);
		}

		public static ISI.Extensions.SpreadSheets.CellBorder ToCellBorder(this global::Aspose.Cells.CellBorderType border)
		{
			switch (border)
			{
				case global::Aspose.Cells.CellBorderType.DashDot: return ISI.Extensions.SpreadSheets.CellBorder.DashDot;
				case global::Aspose.Cells.CellBorderType.DashDotDot: return ISI.Extensions.SpreadSheets.CellBorder.DashDotDot;
				case global::Aspose.Cells.CellBorderType.Dashed: return ISI.Extensions.SpreadSheets.CellBorder.Dashed;
				case global::Aspose.Cells.CellBorderType.Dotted: return ISI.Extensions.SpreadSheets.CellBorder.Dotted;
				case global::Aspose.Cells.CellBorderType.Double: return ISI.Extensions.SpreadSheets.CellBorder.Double;
				case global::Aspose.Cells.CellBorderType.Hair: return ISI.Extensions.SpreadSheets.CellBorder.Hair;
				case global::Aspose.Cells.CellBorderType.MediumDashDot: return ISI.Extensions.SpreadSheets.CellBorder.MediumDashDot;
				case global::Aspose.Cells.CellBorderType.MediumDashDotDot: return ISI.Extensions.SpreadSheets.CellBorder.MediumDashDotDot;
				case global::Aspose.Cells.CellBorderType.MediumDashed: return ISI.Extensions.SpreadSheets.CellBorder.MediumDashed;
				case global::Aspose.Cells.CellBorderType.Medium: return ISI.Extensions.SpreadSheets.CellBorder.Medium;
				case global::Aspose.Cells.CellBorderType.SlantedDashDot: return ISI.Extensions.SpreadSheets.CellBorder.SlantedDashDot;
				case global::Aspose.Cells.CellBorderType.Thick: return ISI.Extensions.SpreadSheets.CellBorder.Thick;
				case global::Aspose.Cells.CellBorderType.Thin: return ISI.Extensions.SpreadSheets.CellBorder.Thin;
			}

			return ISI.Extensions.SpreadSheets.CellBorder.None;
		}
	}
}
