#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
		public static global::Aspose.Cells.Drawing.GradientStyleType ToGradientStyle(this ISI.Extensions.SpreadSheets.GradientStyle gradientStyle)
		{
			switch (gradientStyle)
			{
				case ISI.Extensions.SpreadSheets.GradientStyle.DiagonalDown: return global::Aspose.Cells.Drawing.GradientStyleType.DiagonalDown; 
				case ISI.Extensions.SpreadSheets.GradientStyle.DiagonalUp: return global::Aspose.Cells.Drawing.GradientStyleType.DiagonalUp; 
				case ISI.Extensions.SpreadSheets.GradientStyle.FromCenter: return global::Aspose.Cells.Drawing.GradientStyleType.FromCenter; 
				case ISI.Extensions.SpreadSheets.GradientStyle.FromCorner: return global::Aspose.Cells.Drawing.GradientStyleType.FromCorner; 
				case ISI.Extensions.SpreadSheets.GradientStyle.Horizontal: return global::Aspose.Cells.Drawing.GradientStyleType.Horizontal; 
				case ISI.Extensions.SpreadSheets.GradientStyle.Vertical: return global::Aspose.Cells.Drawing.GradientStyleType.Vertical; 
			}

			return global::Aspose.Cells.Drawing.GradientStyleType.Unknown;
		}

		public static ISI.Extensions.SpreadSheets.GradientStyle ToGradientStyle(this global::Aspose.Cells.Drawing.GradientStyleType gradientStyle)
		{
			switch (gradientStyle)
			{
				case global::Aspose.Cells.Drawing.GradientStyleType.DiagonalDown: return ISI.Extensions.SpreadSheets.GradientStyle.DiagonalDown; 
				case global::Aspose.Cells.Drawing.GradientStyleType.DiagonalUp: return ISI.Extensions.SpreadSheets.GradientStyle.DiagonalUp; 
				case global::Aspose.Cells.Drawing.GradientStyleType.FromCenter: return ISI.Extensions.SpreadSheets.GradientStyle.FromCenter; 
				case global::Aspose.Cells.Drawing.GradientStyleType.FromCorner: return ISI.Extensions.SpreadSheets.GradientStyle.FromCorner; 
				case global::Aspose.Cells.Drawing.GradientStyleType.Horizontal: return ISI.Extensions.SpreadSheets.GradientStyle.Horizontal; 
				case global::Aspose.Cells.Drawing.GradientStyleType.Vertical: return ISI.Extensions.SpreadSheets.GradientStyle.Vertical; 
			}

			return ISI.Extensions.SpreadSheets.GradientStyle.Unknown;
		}
	}
}
