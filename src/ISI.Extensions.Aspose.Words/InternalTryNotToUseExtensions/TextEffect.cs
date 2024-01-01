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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.TextEffect ToTextEffect(this global::Aspose.Words.TextEffect textEffect)
		{
			switch (textEffect)
			{
				case global::Aspose.Words.TextEffect.None: return ISI.Extensions.Documents.TextEffect.None;
				case global::Aspose.Words.TextEffect.LasVegasLights: return ISI.Extensions.Documents.TextEffect.LasVegasLights;
				case global::Aspose.Words.TextEffect.BlinkingBackground: return ISI.Extensions.Documents.TextEffect.BlinkingBackground;
				case global::Aspose.Words.TextEffect.SparkleText: return ISI.Extensions.Documents.TextEffect.SparkleText;
				case global::Aspose.Words.TextEffect.MarchingBlackAnts: return ISI.Extensions.Documents.TextEffect.MarchingBlackAnts;
				case global::Aspose.Words.TextEffect.MarchingRedAnts: return ISI.Extensions.Documents.TextEffect.MarchingRedAnts;
				case global::Aspose.Words.TextEffect.Shimmer: return ISI.Extensions.Documents.TextEffect.Shimmer;
				default:
					throw new ArgumentOutOfRangeException(nameof(textEffect), textEffect, null);
			}
		}

		public static global::Aspose.Words.TextEffect ToTextEffect(this ISI.Extensions.Documents.TextEffect textEffect)
		{
			switch (textEffect)
			{
				case ISI.Extensions.Documents.TextEffect.None: return global::Aspose.Words.TextEffect.None;
				case ISI.Extensions.Documents.TextEffect.LasVegasLights: return global::Aspose.Words.TextEffect.LasVegasLights;
				case ISI.Extensions.Documents.TextEffect.BlinkingBackground: return global::Aspose.Words.TextEffect.BlinkingBackground;
				case ISI.Extensions.Documents.TextEffect.SparkleText: return global::Aspose.Words.TextEffect.SparkleText;
				case ISI.Extensions.Documents.TextEffect.MarchingBlackAnts: return global::Aspose.Words.TextEffect.MarchingBlackAnts;
				case ISI.Extensions.Documents.TextEffect.MarchingRedAnts: return global::Aspose.Words.TextEffect.MarchingRedAnts;
				case ISI.Extensions.Documents.TextEffect.Shimmer: return global::Aspose.Words.TextEffect.Shimmer;
				default:
					throw new ArgumentOutOfRangeException(nameof(textEffect), textEffect, null);
			}
		}
	}
}