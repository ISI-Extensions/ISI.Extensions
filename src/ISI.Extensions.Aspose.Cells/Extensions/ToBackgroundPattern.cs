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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.BackgroundType? ToBackgroundPattern(this ISI.Extensions.SpreadSheets.BackgroundPattern? pattern)
		{
			return (pattern.HasValue ? ToBackgroundPattern(pattern.GetValueOrDefault()) : (global::Aspose.Cells.BackgroundType?)null);
		}

		public static global::Aspose.Cells.BackgroundType ToBackgroundPattern(this ISI.Extensions.SpreadSheets.BackgroundPattern pattern)
		{
			switch (pattern)
			{
				case ISI.Extensions.SpreadSheets.BackgroundPattern.DiagonalCrosshatch: return global::Aspose.Cells.BackgroundType.DiagonalCrosshatch;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.DiagonalStripe: return global::Aspose.Cells.BackgroundType.DiagonalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Gray6: return global::Aspose.Cells.BackgroundType.Gray6;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Gray12: return global::Aspose.Cells.BackgroundType.Gray12;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Gray25: return global::Aspose.Cells.BackgroundType.Gray25;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Gray50: return global::Aspose.Cells.BackgroundType.Gray50;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Gray75: return global::Aspose.Cells.BackgroundType.Gray75;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.HorizontalStripe: return global::Aspose.Cells.BackgroundType.HorizontalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ReverseDiagonalStripe: return global::Aspose.Cells.BackgroundType.ReverseDiagonalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.Solid: return global::Aspose.Cells.BackgroundType.Solid;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThickDiagonalCrosshatch: return global::Aspose.Cells.BackgroundType.ThinDiagonalCrosshatch;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinDiagonalCrosshatch: return global::Aspose.Cells.BackgroundType.ThinDiagonalCrosshatch;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinDiagonalStripe: return global::Aspose.Cells.BackgroundType.ThinDiagonalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinHorizontalCrosshatch: return global::Aspose.Cells.BackgroundType.ThinHorizontalCrosshatch;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinHorizontalStripe: return global::Aspose.Cells.BackgroundType.ThinHorizontalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinReverseDiagonalStripe: return global::Aspose.Cells.BackgroundType.ThinReverseDiagonalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.ThinVerticalStripe: return global::Aspose.Cells.BackgroundType.ThinVerticalStripe;
				case ISI.Extensions.SpreadSheets.BackgroundPattern.VerticalStripe: return global::Aspose.Cells.BackgroundType.VerticalStripe;
			}

			return global::Aspose.Cells.BackgroundType.None;
		}

		public static ISI.Extensions.SpreadSheets.BackgroundPattern? ToBackgroundPattern(this global::Aspose.Cells.BackgroundType? pattern)
		{
			return (pattern.HasValue ? ToBackgroundPattern(pattern.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.BackgroundPattern?)null);
		}

		public static ISI.Extensions.SpreadSheets.BackgroundPattern ToBackgroundPattern(this global::Aspose.Cells.BackgroundType pattern)
		{
			switch (pattern)
			{
				case global::Aspose.Cells.BackgroundType.DiagonalCrosshatch: return ISI.Extensions.SpreadSheets.BackgroundPattern.DiagonalCrosshatch;
				case global::Aspose.Cells.BackgroundType.DiagonalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.DiagonalStripe;
				case global::Aspose.Cells.BackgroundType.Gray6: return ISI.Extensions.SpreadSheets.BackgroundPattern.Gray6;
				case global::Aspose.Cells.BackgroundType.Gray12: return ISI.Extensions.SpreadSheets.BackgroundPattern.Gray12;
				case global::Aspose.Cells.BackgroundType.Gray25: return ISI.Extensions.SpreadSheets.BackgroundPattern.Gray25;
				case global::Aspose.Cells.BackgroundType.Gray50: return ISI.Extensions.SpreadSheets.BackgroundPattern.Gray50;
				case global::Aspose.Cells.BackgroundType.Gray75: return ISI.Extensions.SpreadSheets.BackgroundPattern.Gray75;
				case global::Aspose.Cells.BackgroundType.HorizontalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.HorizontalStripe;
				case global::Aspose.Cells.BackgroundType.ReverseDiagonalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.ReverseDiagonalStripe;
				case global::Aspose.Cells.BackgroundType.Solid: return ISI.Extensions.SpreadSheets.BackgroundPattern.Solid;
				case global::Aspose.Cells.BackgroundType.ThickDiagonalCrosshatch: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinDiagonalCrosshatch;
				case global::Aspose.Cells.BackgroundType.ThinDiagonalCrosshatch: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinDiagonalCrosshatch;
				case global::Aspose.Cells.BackgroundType.ThinDiagonalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinDiagonalStripe;
				case global::Aspose.Cells.BackgroundType.ThinHorizontalCrosshatch: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinHorizontalCrosshatch;
				case global::Aspose.Cells.BackgroundType.ThinHorizontalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinHorizontalStripe;
				case global::Aspose.Cells.BackgroundType.ThinReverseDiagonalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinReverseDiagonalStripe;
				case global::Aspose.Cells.BackgroundType.ThinVerticalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.ThinVerticalStripe;
				case global::Aspose.Cells.BackgroundType.VerticalStripe: return ISI.Extensions.SpreadSheets.BackgroundPattern.VerticalStripe;
			}

			return ISI.Extensions.SpreadSheets.BackgroundPattern.None;
		}
	}
}
