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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.LineStyle ToLineStyle(this global::Aspose.Words.LineStyle lineStyle)
		{
			switch (lineStyle)
			{
				case global::Aspose.Words.LineStyle.None: return ISI.Extensions.Documents.LineStyle.None;
				case global::Aspose.Words.LineStyle.Single: return ISI.Extensions.Documents.LineStyle.Single;
				case global::Aspose.Words.LineStyle.Thick: return ISI.Extensions.Documents.LineStyle.Thick;
				case global::Aspose.Words.LineStyle.Double: return ISI.Extensions.Documents.LineStyle.Double;
				case global::Aspose.Words.LineStyle.Hairline: return ISI.Extensions.Documents.LineStyle.Hairline;
				case global::Aspose.Words.LineStyle.Dot: return ISI.Extensions.Documents.LineStyle.Dot;
				case global::Aspose.Words.LineStyle.DashLargeGap: return ISI.Extensions.Documents.LineStyle.DashLargeGap;
				case global::Aspose.Words.LineStyle.DotDash: return ISI.Extensions.Documents.LineStyle.DotDash;
				case global::Aspose.Words.LineStyle.DotDotDash: return ISI.Extensions.Documents.LineStyle.DotDotDash;
				case global::Aspose.Words.LineStyle.Triple: return ISI.Extensions.Documents.LineStyle.Triple;
				case global::Aspose.Words.LineStyle.ThinThickSmallGap: return ISI.Extensions.Documents.LineStyle.ThinThickSmallGap;
				case global::Aspose.Words.LineStyle.ThickThinSmallGap: return ISI.Extensions.Documents.LineStyle.ThickThinSmallGap;
				case global::Aspose.Words.LineStyle.ThinThickThinSmallGap: return ISI.Extensions.Documents.LineStyle.ThinThickThinSmallGap;
				case global::Aspose.Words.LineStyle.ThinThickMediumGap: return ISI.Extensions.Documents.LineStyle.ThinThickMediumGap;
				case global::Aspose.Words.LineStyle.ThickThinMediumGap: return ISI.Extensions.Documents.LineStyle.ThickThinMediumGap;
				case global::Aspose.Words.LineStyle.ThinThickThinMediumGap: return ISI.Extensions.Documents.LineStyle.ThinThickThinMediumGap;
				case global::Aspose.Words.LineStyle.ThinThickLargeGap: return ISI.Extensions.Documents.LineStyle.ThinThickLargeGap;
				case global::Aspose.Words.LineStyle.ThickThinLargeGap: return ISI.Extensions.Documents.LineStyle.ThickThinLargeGap;
				case global::Aspose.Words.LineStyle.ThinThickThinLargeGap: return ISI.Extensions.Documents.LineStyle.ThinThickThinLargeGap;
				case global::Aspose.Words.LineStyle.Wave: return ISI.Extensions.Documents.LineStyle.Wave;
				case global::Aspose.Words.LineStyle.DoubleWave: return ISI.Extensions.Documents.LineStyle.DoubleWave;
				case global::Aspose.Words.LineStyle.DashSmallGap: return ISI.Extensions.Documents.LineStyle.DashSmallGap;
				case global::Aspose.Words.LineStyle.DashDotStroker: return ISI.Extensions.Documents.LineStyle.DashDotStroker;
				case global::Aspose.Words.LineStyle.Emboss3D: return ISI.Extensions.Documents.LineStyle.Emboss3D;
				case global::Aspose.Words.LineStyle.Engrave3D: return ISI.Extensions.Documents.LineStyle.Engrave3D;
				case global::Aspose.Words.LineStyle.Outset: return ISI.Extensions.Documents.LineStyle.Outset;
				case global::Aspose.Words.LineStyle.Inset: return ISI.Extensions.Documents.LineStyle.Inset;
				default:
					throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null);
			}
		}

		public static global::Aspose.Words.LineStyle ToLineStyle(this ISI.Extensions.Documents.LineStyle lineStyle)
		{
			switch (lineStyle)
			{
				case ISI.Extensions.Documents.LineStyle.None: return global::Aspose.Words.LineStyle.None;
				case ISI.Extensions.Documents.LineStyle.Single: return global::Aspose.Words.LineStyle.Single;
				case ISI.Extensions.Documents.LineStyle.Thick: return global::Aspose.Words.LineStyle.Thick;
				case ISI.Extensions.Documents.LineStyle.Double: return global::Aspose.Words.LineStyle.Double;
				case ISI.Extensions.Documents.LineStyle.Hairline: return global::Aspose.Words.LineStyle.Hairline;
				case ISI.Extensions.Documents.LineStyle.Dot: return global::Aspose.Words.LineStyle.Dot;
				case ISI.Extensions.Documents.LineStyle.DashLargeGap: return global::Aspose.Words.LineStyle.DashLargeGap;
				case ISI.Extensions.Documents.LineStyle.DotDash: return global::Aspose.Words.LineStyle.DotDash;
				case ISI.Extensions.Documents.LineStyle.DotDotDash: return global::Aspose.Words.LineStyle.DotDotDash;
				case ISI.Extensions.Documents.LineStyle.Triple: return global::Aspose.Words.LineStyle.Triple;
				case ISI.Extensions.Documents.LineStyle.ThinThickSmallGap: return global::Aspose.Words.LineStyle.ThinThickSmallGap;
				case ISI.Extensions.Documents.LineStyle.ThickThinSmallGap: return global::Aspose.Words.LineStyle.ThickThinSmallGap;
				case ISI.Extensions.Documents.LineStyle.ThinThickThinSmallGap: return global::Aspose.Words.LineStyle.ThinThickThinSmallGap;
				case ISI.Extensions.Documents.LineStyle.ThinThickMediumGap: return global::Aspose.Words.LineStyle.ThinThickMediumGap;
				case ISI.Extensions.Documents.LineStyle.ThickThinMediumGap: return global::Aspose.Words.LineStyle.ThickThinMediumGap;
				case ISI.Extensions.Documents.LineStyle.ThinThickThinMediumGap: return global::Aspose.Words.LineStyle.ThinThickThinMediumGap;
				case ISI.Extensions.Documents.LineStyle.ThinThickLargeGap: return global::Aspose.Words.LineStyle.ThinThickLargeGap;
				case ISI.Extensions.Documents.LineStyle.ThickThinLargeGap: return global::Aspose.Words.LineStyle.ThickThinLargeGap;
				case ISI.Extensions.Documents.LineStyle.ThinThickThinLargeGap: return global::Aspose.Words.LineStyle.ThinThickThinLargeGap;
				case ISI.Extensions.Documents.LineStyle.Wave: return global::Aspose.Words.LineStyle.Wave;
				case ISI.Extensions.Documents.LineStyle.DoubleWave: return global::Aspose.Words.LineStyle.DoubleWave;
				case ISI.Extensions.Documents.LineStyle.DashSmallGap: return global::Aspose.Words.LineStyle.DashSmallGap;
				case ISI.Extensions.Documents.LineStyle.DashDotStroker: return global::Aspose.Words.LineStyle.DashDotStroker;
				case ISI.Extensions.Documents.LineStyle.Emboss3D: return global::Aspose.Words.LineStyle.Emboss3D;
				case ISI.Extensions.Documents.LineStyle.Engrave3D: return global::Aspose.Words.LineStyle.Engrave3D;
				case ISI.Extensions.Documents.LineStyle.Outset: return global::Aspose.Words.LineStyle.Outset;
				case ISI.Extensions.Documents.LineStyle.Inset: return global::Aspose.Words.LineStyle.Inset;
				default:
					throw new ArgumentOutOfRangeException(nameof(lineStyle), lineStyle, null);
			}
		}
	}
}