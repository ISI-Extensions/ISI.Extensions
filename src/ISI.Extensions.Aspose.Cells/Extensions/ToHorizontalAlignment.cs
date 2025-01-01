#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
		public static global::Aspose.Cells.TextAlignmentType? ToHorizontalAlignment(this ISI.Extensions.SpreadSheets.HorizontalAlignment? horizontalAlignment)
		{
			return (horizontalAlignment.HasValue ? ToHorizontalAlignment(horizontalAlignment.GetValueOrDefault()) : (global::Aspose.Cells.TextAlignmentType?) null);
		}

		public static global::Aspose.Cells.TextAlignmentType ToHorizontalAlignment(this ISI.Extensions.SpreadSheets.HorizontalAlignment horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.General: return global::Aspose.Cells.TextAlignmentType.General;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Center: return global::Aspose.Cells.TextAlignmentType.Center;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Distributed: return global::Aspose.Cells.TextAlignmentType.Distributed;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Fill: return global::Aspose.Cells.TextAlignmentType.Fill;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Justify: return global::Aspose.Cells.TextAlignmentType.Justify;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Left: return global::Aspose.Cells.TextAlignmentType.Left;
				case ISI.Extensions.SpreadSheets.HorizontalAlignment.Right: return global::Aspose.Cells.TextAlignmentType.Right;
			}

			throw new ArgumentOutOfRangeException(nameof(horizontalAlignment), horizontalAlignment, null);
		}

		public static ISI.Extensions.SpreadSheets.HorizontalAlignment? ToHorizontalAlignment(this global::Aspose.Cells.TextAlignmentType? horizontalAlignment)
		{
			return (horizontalAlignment.HasValue ? ToHorizontalAlignment(horizontalAlignment.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.HorizontalAlignment?)null);
		}

		public static ISI.Extensions.SpreadSheets.HorizontalAlignment ToHorizontalAlignment(this global::Aspose.Cells.TextAlignmentType horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
				case global::Aspose.Cells.TextAlignmentType.General: return ISI.Extensions.SpreadSheets.HorizontalAlignment.General;
				case global::Aspose.Cells.TextAlignmentType.Center: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Center;
				case global::Aspose.Cells.TextAlignmentType.Distributed: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Distributed;
				case global::Aspose.Cells.TextAlignmentType.Fill: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Fill;
				case global::Aspose.Cells.TextAlignmentType.Justify: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Justify;
				case global::Aspose.Cells.TextAlignmentType.Left: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Left;
				case global::Aspose.Cells.TextAlignmentType.Right: return ISI.Extensions.SpreadSheets.HorizontalAlignment.Right;
			}

			throw new ArgumentOutOfRangeException(nameof(horizontalAlignment), horizontalAlignment, null);
		}
	}
}
