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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.TextureIndex ToTextureIndex(this global::Aspose.Words.TextureIndex textureIndex)
		{
			switch (textureIndex)
			{
				case global::Aspose.Words.TextureIndex.TextureNone: return ISI.Extensions.Documents.TextureIndex.TextureNone;
				case global::Aspose.Words.TextureIndex.TextureSolid: return ISI.Extensions.Documents.TextureIndex.TextureSolid;
				case global::Aspose.Words.TextureIndex.Texture5Percent: return ISI.Extensions.Documents.TextureIndex.Texture5Percent;
				case global::Aspose.Words.TextureIndex.Texture10Percent: return ISI.Extensions.Documents.TextureIndex.Texture10Percent;
				case global::Aspose.Words.TextureIndex.Texture20Percent: return ISI.Extensions.Documents.TextureIndex.Texture20Percent;
				case global::Aspose.Words.TextureIndex.Texture25Percent: return ISI.Extensions.Documents.TextureIndex.Texture25Percent;
				case global::Aspose.Words.TextureIndex.Texture30Percent: return ISI.Extensions.Documents.TextureIndex.Texture30Percent;
				case global::Aspose.Words.TextureIndex.Texture40Percent: return ISI.Extensions.Documents.TextureIndex.Texture40Percent;
				case global::Aspose.Words.TextureIndex.Texture50Percent: return ISI.Extensions.Documents.TextureIndex.Texture50Percent;
				case global::Aspose.Words.TextureIndex.Texture60Percent: return ISI.Extensions.Documents.TextureIndex.Texture60Percent;
				case global::Aspose.Words.TextureIndex.Texture70Percent: return ISI.Extensions.Documents.TextureIndex.Texture70Percent;
				case global::Aspose.Words.TextureIndex.Texture75Percent: return ISI.Extensions.Documents.TextureIndex.Texture75Percent;
				case global::Aspose.Words.TextureIndex.Texture80Percent: return ISI.Extensions.Documents.TextureIndex.Texture80Percent;
				case global::Aspose.Words.TextureIndex.Texture90Percent: return ISI.Extensions.Documents.TextureIndex.Texture90Percent;
				case global::Aspose.Words.TextureIndex.TextureDarkHorizontal: return ISI.Extensions.Documents.TextureIndex.TextureDarkHorizontal;
				case global::Aspose.Words.TextureIndex.TextureDarkVertical: return ISI.Extensions.Documents.TextureIndex.TextureDarkVertical;
				case global::Aspose.Words.TextureIndex.TextureDarkDiagonalDown: return ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalDown;
				case global::Aspose.Words.TextureIndex.TextureDarkDiagonalUp: return ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalUp;
				case global::Aspose.Words.TextureIndex.TextureDarkCross: return ISI.Extensions.Documents.TextureIndex.TextureDarkCross;
				case global::Aspose.Words.TextureIndex.TextureDarkDiagonalCross: return ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalCross;
				case global::Aspose.Words.TextureIndex.TextureHorizontal: return ISI.Extensions.Documents.TextureIndex.TextureHorizontal;
				case global::Aspose.Words.TextureIndex.TextureVertical: return ISI.Extensions.Documents.TextureIndex.TextureVertical;
				case global::Aspose.Words.TextureIndex.TextureDiagonalDown: return ISI.Extensions.Documents.TextureIndex.TextureDiagonalDown;
				case global::Aspose.Words.TextureIndex.TextureDiagonalUp: return ISI.Extensions.Documents.TextureIndex.TextureDiagonalUp;
				case global::Aspose.Words.TextureIndex.TextureCross: return ISI.Extensions.Documents.TextureIndex.TextureCross;
				case global::Aspose.Words.TextureIndex.TextureDiagonalCross: return ISI.Extensions.Documents.TextureIndex.TextureDiagonalCross;
				case global::Aspose.Words.TextureIndex.Texture2Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture2Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture7Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture7Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture12Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture12Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture15Percent: return ISI.Extensions.Documents.TextureIndex.Texture15Percent;
				case global::Aspose.Words.TextureIndex.Texture17Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture17Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture22Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture22Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture27Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture27Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture32Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture32Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture35Percent: return ISI.Extensions.Documents.TextureIndex.Texture35Percent;
				case global::Aspose.Words.TextureIndex.Texture37Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture37Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture42Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture42Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture45Percent: return ISI.Extensions.Documents.TextureIndex.Texture45Percent;
				case global::Aspose.Words.TextureIndex.Texture47Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture47Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture52Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture52Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture55Percent: return ISI.Extensions.Documents.TextureIndex.Texture55Percent;
				case global::Aspose.Words.TextureIndex.Texture57Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture57Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture62Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture62Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture65Percent: return ISI.Extensions.Documents.TextureIndex.Texture65Percent;
				case global::Aspose.Words.TextureIndex.Texture67Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture67Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture72Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture72Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture77Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture77Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture82Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture82Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture85Percent: return ISI.Extensions.Documents.TextureIndex.Texture85Percent;
				case global::Aspose.Words.TextureIndex.Texture87Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture87Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture92Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture92Pt5Percent;
				case global::Aspose.Words.TextureIndex.Texture95Percent: return ISI.Extensions.Documents.TextureIndex.Texture95Percent;
				case global::Aspose.Words.TextureIndex.Texture97Pt5Percent: return ISI.Extensions.Documents.TextureIndex.Texture97Pt5Percent;
				case global::Aspose.Words.TextureIndex.TextureNil: return ISI.Extensions.Documents.TextureIndex.TextureNil;
				default:
					throw new ArgumentOutOfRangeException(nameof(textureIndex), textureIndex, null);
			}
		}

		public static global::Aspose.Words.TextureIndex ToTextureIndex(this ISI.Extensions.Documents.TextureIndex textureIndex)
		{
			switch (textureIndex)
			{
				case ISI.Extensions.Documents.TextureIndex.TextureNone: return global::Aspose.Words.TextureIndex.TextureNone;
				case ISI.Extensions.Documents.TextureIndex.TextureSolid: return global::Aspose.Words.TextureIndex.TextureSolid;
				case ISI.Extensions.Documents.TextureIndex.Texture5Percent: return global::Aspose.Words.TextureIndex.Texture5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture10Percent: return global::Aspose.Words.TextureIndex.Texture10Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture20Percent: return global::Aspose.Words.TextureIndex.Texture20Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture25Percent: return global::Aspose.Words.TextureIndex.Texture25Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture30Percent: return global::Aspose.Words.TextureIndex.Texture30Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture40Percent: return global::Aspose.Words.TextureIndex.Texture40Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture50Percent: return global::Aspose.Words.TextureIndex.Texture50Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture60Percent: return global::Aspose.Words.TextureIndex.Texture60Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture70Percent: return global::Aspose.Words.TextureIndex.Texture70Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture75Percent: return global::Aspose.Words.TextureIndex.Texture75Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture80Percent: return global::Aspose.Words.TextureIndex.Texture80Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture90Percent: return global::Aspose.Words.TextureIndex.Texture90Percent;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkHorizontal: return global::Aspose.Words.TextureIndex.TextureDarkHorizontal;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkVertical: return global::Aspose.Words.TextureIndex.TextureDarkVertical;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalDown: return global::Aspose.Words.TextureIndex.TextureDarkDiagonalDown;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalUp: return global::Aspose.Words.TextureIndex.TextureDarkDiagonalUp;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkCross: return global::Aspose.Words.TextureIndex.TextureDarkCross;
				case ISI.Extensions.Documents.TextureIndex.TextureDarkDiagonalCross: return global::Aspose.Words.TextureIndex.TextureDarkDiagonalCross;
				case ISI.Extensions.Documents.TextureIndex.TextureHorizontal: return global::Aspose.Words.TextureIndex.TextureHorizontal;
				case ISI.Extensions.Documents.TextureIndex.TextureVertical: return global::Aspose.Words.TextureIndex.TextureVertical;
				case ISI.Extensions.Documents.TextureIndex.TextureDiagonalDown: return global::Aspose.Words.TextureIndex.TextureDiagonalDown;
				case ISI.Extensions.Documents.TextureIndex.TextureDiagonalUp: return global::Aspose.Words.TextureIndex.TextureDiagonalUp;
				case ISI.Extensions.Documents.TextureIndex.TextureCross: return global::Aspose.Words.TextureIndex.TextureCross;
				case ISI.Extensions.Documents.TextureIndex.TextureDiagonalCross: return global::Aspose.Words.TextureIndex.TextureDiagonalCross;
				case ISI.Extensions.Documents.TextureIndex.Texture2Pt5Percent: return global::Aspose.Words.TextureIndex.Texture2Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture7Pt5Percent: return global::Aspose.Words.TextureIndex.Texture7Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture12Pt5Percent: return global::Aspose.Words.TextureIndex.Texture12Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture15Percent: return global::Aspose.Words.TextureIndex.Texture15Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture17Pt5Percent: return global::Aspose.Words.TextureIndex.Texture17Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture22Pt5Percent: return global::Aspose.Words.TextureIndex.Texture22Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture27Pt5Percent: return global::Aspose.Words.TextureIndex.Texture27Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture32Pt5Percent: return global::Aspose.Words.TextureIndex.Texture32Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture35Percent: return global::Aspose.Words.TextureIndex.Texture35Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture37Pt5Percent: return global::Aspose.Words.TextureIndex.Texture37Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture42Pt5Percent: return global::Aspose.Words.TextureIndex.Texture42Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture45Percent: return global::Aspose.Words.TextureIndex.Texture45Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture47Pt5Percent: return global::Aspose.Words.TextureIndex.Texture47Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture52Pt5Percent: return global::Aspose.Words.TextureIndex.Texture52Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture55Percent: return global::Aspose.Words.TextureIndex.Texture55Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture57Pt5Percent: return global::Aspose.Words.TextureIndex.Texture57Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture62Pt5Percent: return global::Aspose.Words.TextureIndex.Texture62Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture65Percent: return global::Aspose.Words.TextureIndex.Texture65Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture67Pt5Percent: return global::Aspose.Words.TextureIndex.Texture67Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture72Pt5Percent: return global::Aspose.Words.TextureIndex.Texture72Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture77Pt5Percent: return global::Aspose.Words.TextureIndex.Texture77Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture82Pt5Percent: return global::Aspose.Words.TextureIndex.Texture82Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture85Percent: return global::Aspose.Words.TextureIndex.Texture85Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture87Pt5Percent: return global::Aspose.Words.TextureIndex.Texture87Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture92Pt5Percent: return global::Aspose.Words.TextureIndex.Texture92Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture95Percent: return global::Aspose.Words.TextureIndex.Texture95Percent;
				case ISI.Extensions.Documents.TextureIndex.Texture97Pt5Percent: return global::Aspose.Words.TextureIndex.Texture97Pt5Percent;
				case ISI.Extensions.Documents.TextureIndex.TextureNil: return global::Aspose.Words.TextureIndex.TextureNil;
				default:
					throw new ArgumentOutOfRangeException(nameof(textureIndex), textureIndex, null);
			}
		}
	}
}